const { Beatmap, Calculator } = require("./node_modules/rosu-pp");
const axios = require("./node_modules/axios");
const crypto = require("node:crypto");
const path = require("node:path");
const fs = require("node:fs");

let currentStatus = null;
let currentMode = 0;
let nowPlaying = false;
let hasEnded = false;
let startTime = null;
let endTime = null;

(async function main(username) {
    const userData = "./src/user/" + username + ".json";
    while (true) {
        try {
            let response = await axios.get("http://127.0.0.1:24050/json");
            
            if (response.data.menu.state == 2) currentMode = response.data.gameplay.gameMode;
            if (currentStatus == null) currentStatus = response.data.menu.state;

            if (currentStatus == 2 && response.data.menu.state == 7) {
                hasEnded = false;
                endTime = new Date().getTime();
                currentStatus = response.data.menu.state;
                const BannedModstext = fs.readFileSync("BannedMods.txt", "utf-8");
                const BannedMods = BannedModstext.split(",").filter(mod => {
                    return mod != "" && mod.length == 2;
                });
                const mod = response.data.menu.mods.str.match(/.{2}/g);
                let bannedmodflag = mod.some(resultMod => BannedMods.includes(resultMod));
                if (bannedmodflag) {
                    startTime = null;
                    endTime = null;
                    currentStatus = response.data.menu.state;
                    response = null;
                    continue;
                }

                const mappath = path.join(response.data.settings.folders.songs, response.data.menu.bm.path.folder, response.data.menu.bm.path.file);
                const params = {
                    mode: response.data.menu.gameMode,
                    mods: response.data.menu.mods.num,
                    n300: response.data.gameplay.hits["300"],
                    n100: response.data.gameplay.hits["100"],
                    n50: response.data.gameplay.hits["50"],
                    nMisses: response.data.gameplay.hits["0"],
                    nKatu: response.data.gameplay.hits.katu,
                    nGeki: response.data.gameplay.hits.geki,
                    combo: response.data.gameplay.combo.max
                };
                const pp = calculator(mappath, params);
                const confirmHash = crypto.createHash("md5").update(fs.readFileSync(mappath), "binary").digest("hex");
                const data = {
                    "title" : response.data.menu.bm.metadata.title + " by " + response.data.menu.bm.metadata.artist,
                    "mapper": response.data.menu.bm.metadata.mapper,
                    "version": response.data.menu.bm.metadata.difficulty,
                    "pp" : Math.round(pp * 100) / 100,
                    "score" : response.data.gameplay.score,
                    "mods" : response.data.menu.mods.str,
                    "acc" : response.data.gameplay.accuracy,
                    "combo" : response.data.gameplay.combo.max,
                    "300" : response.data.gameplay.hits["300"],
                    "100" : response.data.gameplay.hits["100"],
                    "50" : response.data.gameplay.hits["50"],
                    "miss" : response.data.gameplay.hits["0"],
                    "katu" : response.data.gameplay.hits.katu,
                    "geki" : response.data.gameplay.hits.geki,
                    "rank" : response.data.gameplay.hits.grade.current,
                    "date" : new Date().toLocaleString(),
                    "hash" : confirmHash
                };

                if (!fs.existsSync(userData)) {
                    fs.writeFileSync(userData, JSON.stringify({
                        "username": username,
                        "lastGamemode" : 0,
                        "globalPP": {
                            "osu": 0,
                            "taiko": 0,
                            "catch": 0,
                            "mania": 0
                        },
                        "bonusPP": {
                            "osu": 0,
                            "taiko": 0,
                            "catch": 0,
                            "mania": 0
                        },
                        "globalACC": {
                            "osu": 0,
                            "taiko": 0,
                            "catch": 0,
                            "mania": 0
                        },
                        "playtime" : {
                            "osu": "0h 0m",
                            "taiko": "0h 0m",
                            "catch": "0h 0m",
                            "mania": "0h 0m"
                        },
                        "playtimeCalculate" : {
                            "osu": 0,
                            "taiko": 0,
                            "catch": 0,
                            "mania": 0
                        },
                        "playcount" : {
                            "osu": 0,
                            "taiko": 0,
                            "catch": 0,
                            "mania": 0
                        },
                        "pp": {
                            "osu": [],
                            "taiko": [],
                            "catch": [],
                            "mania": []
                        }
                    }, null, 4));
                }

                let json = JSON.parse(fs.readFileSync(userData, "utf-8"));
                const mode = modeConverter(currentMode);

                if (json.pp[mode].length > 0) {
                    let flag = false;
                    let continueflag = false;
                    for (let i = 0; i < json.pp[mode].length; i++) {
                        if (json.pp[mode][i].hash == confirmHash) {
                            if ((json.pp[mode][i].score < response.data.gameplay.score && json.pp[mode][i].mods == response.data.menu.mods.str) || (pp > json.pp[mode][i].pp)) {
                                json.pp[mode][i] = data;
                                flag = true;
                                break;
                            } else {
                                if (startTime != null) {
                                    const time = endTime - startTime;
                                    const playtime = formatTime(json.playtimeCalculate[mode] + time);
                                    json.playtime[mode] = playtime;
                                    json.playtimeCalculate[mode] += time;
                                }
                                json.playcount[mode] += 1;
                                json.lastGamemode = currentMode;
                                fs.writeFileSync(userData, JSON.stringify(json, null, 4));
                                startTime = null;
                                endTime = null;
                                continueflag = true;
                                break;
                            }
                        }
                    }
                    if (continueflag) {
                        currentStatus = response.data.menu.state;
                        response = null;
                        continue;
                    }
                    if (!flag) json.pp[mode].push(data);
                } else {
                    json.pp[mode].push(data);
                }

                json.pp[mode].sort((a, b) => {
                    return b.pp - a.pp;
                });

                const bonusPP = calculateBonusPP(json, mode);
                const globalPP = calculateGlobalPP(json, mode) + bonusPP;
                const globalACC = calculateGlobalACC(json, mode);

                if (startTime != null) {
                    const time = endTime - startTime;
                    const playtime = formatTime(json.playtimeCalculate[mode] + time);
                    json.playtime[mode] = playtime;
                    json.playtimeCalculate[mode] += time;
                }

                json.playcount[mode] += 1;
                json.bonusPP[mode] = bonusPP;
                json.globalPP[mode] = globalPP;
                json.globalACC[mode] = globalACC;
                json.lastGamemode = currentMode;
                fs.writeFileSync(userData, JSON.stringify(json, null, 4));
                json = null;
                startTime = null;
                endTime = null;
            }

            if ((currentStatus == 5 || currentStatus == 7) && response.data.menu.state == 2) {
                nowPlaying = true;
            } else if (currentStatus == 2 && response.data.menu.state == 5) {
                hasEnded = true;
            }

            if (nowPlaying) {
                nowPlaying = false;
                startTime = new Date().getTime();
            }
            
            if (hasEnded) {
                hasEnded = false;
                if (!fs.existsSync(userData) || startTime == null) {
                    startTime = null;
                    endTime = null;
                } else {
                    endTime = new Date().getTime();

                    if (endTime - startTime < 10000) {
                        startTime = null;
                        endTime = null;
                        currentStatus = response.data.menu.state;
                        response = null;
                        continue;
                    }

                    let json = JSON.parse(fs.readFileSync(userData, "utf-8"));
                    const mode = modeConverter(currentMode);

                    if (startTime != null) {
                        const time = endTime - startTime;
                        const playtime = formatTime(json.playtimeCalculate[mode] + time);
                        json.playtime[mode] = playtime;
                        json.playtimeCalculate[mode] += time;
                    }

                    json.playcount[mode] += 1;
                    json.lastGamemode = currentMode;
                    fs.writeFileSync(userData, JSON.stringify(json, null, 4));
                    startTime = null;
                    endTime = null;
                    json = null;
                }
            }
            
            currentStatus = response.data.menu.state;
            response = null;
        } catch (e) {
            console.log(e);
        }
    }
})(process.argv[2]);

function calculator(mappath, params) {
    const beatmap = new Beatmap({ path: mappath });
    const pp = new Calculator(params).performance(beatmap).pp;
    return pp;
}

function calculateGlobalPP(json, mode) {
    let globalPP = 0;
    for (let i = 0; i < Math.min(json.pp[mode].length, 100); i++) {
        globalPP += json.pp[mode][i].pp * Math.pow(0.95, i);
    }
    return globalPP;
}

function calculateGlobalACC(json, mode) {
    let globalAcc = 0;
    for (let i = 0; i < Math.min(json.pp[mode].length, 100); i++) {
        globalAcc += (json.pp[mode][i].acc * Math.pow(0.95, i));
    }
    globalAcc *= 100 / (20 * (1 - Math.pow(0.95, json.pp[mode].length)));
    return Math.round(globalAcc) / 100;
}

function calculateBonusPP(json, mode) {
    return 416.6667 * (1 - Math.pow(0.9994, json.pp[mode].length));
}

function formatTime(time) {
    const hours = Math.floor(time / 3600000);
    const minutes = Math.floor((time - (hours * 3600000)) / 60000);
    return hours + "h " + minutes + "m";
}

function modeConverter(mode) {
    switch (mode) {
        case 0:
            return "osu";
        case 1:
            return "taiko";
        case 2:
            return "catch";
        case 3:
            return "mania";
    }
}
