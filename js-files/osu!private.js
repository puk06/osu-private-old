const { Beatmap, Calculator } = require("./node_modules/rosu-pp");
const axios = require("./node_modules/axios");
const path = require("path");
const fs = require("fs");
const crypto = require("crypto");
let currentStatus = null;
let currentMode = null;

async function main(username) {
    let res;
    try {
        res = await axios.get("http://127.0.0.1:24050/json");
        currentMode = res.data.menu.gameMode;
    } catch (e) {
        console.log(e)
    }
        setInterval(async () => {
            try {
                let response = await axios.get("http://127.0.0.1:24050/json");
                if (currentStatus == 2 && response.data.menu.state == 7) {
                    currentStatus = response.data.menu.state;
                    const mod = response.data.menu.mods.str.match(/.{2}/g);
                    if (mod.includes("AT")
                        || mod.includes("AP")
                        || mod.includes("RX")
                        || mod.includes("V2")
                        || mod.includes("CN")
                        || mod.includes("SO")
                        || mod.includes("TG")
                    ) {
                        response = null;
                        return;
                    }

                    const mappath = path.join(response.data.settings.folders.songs, response.data.menu.bm.path.folder, response.data.menu.bm.path.file)
                    const params = {
                        mode: response.data.menu.gameMode,
                        mods: response.data.menu.mods.num,
                        n300: response.data.gameplay.hits["300"],
                        n100: response.data.gameplay.hits["100"],
                        n50: response.data.gameplay.hits["50"],
                        nMisses: response.data.gameplay.hits["0"],
                        nKatu: response.data.gameplay.hits["katu"],
                        nGeki: response.data.gameplay.hits["geki"],
                        combo: response.data.gameplay.combo["max"],
                    }
                    const pp = calculator(mappath, params);

                    const confirmHash = crypto.createHash("sha256").update(response.data.menu.bm.metadata.title + response.data.menu.bm.metadata.artist + response.data.menu.bm.metadata.mapper + response.data.menu.bm.metadata.difficulty).digest("hex");
                    const data = {
                        "title" : response.data.menu.bm.metadata.title + " by " + response.data.menu.bm.metadata.artist,
                        "mapper": response.data.menu.bm.metadata.mapper,
                        "version": response.data.menu.bm.metadata.difficulty,
                        "pp" : Math.round(pp * 100) / 100,
                        "score" : response.data.gameplay.score,
                        "mods" : response.data.menu.mods.str,
                        "acc" : response.data.gameplay.accuracy,
                        "combo" : response.data.gameplay.combo["max"],
                        "300" : response.data.gameplay.hits["300"],
                        "100" : response.data.gameplay.hits["100"],
                        "50" : response.data.gameplay.hits["50"],
                        "miss" : response.data.gameplay.hits["0"],
                        "katu" : response.data.gameplay.hits["katu"],
                        "geki" : response.data.gameplay.hits["geki"],
                        "rank" : response.data.gameplay.hits.grade.current,
                        "date" : new Date().toLocaleString(),
                        "hash" : crypto.createHash("sha256").update(response.data.menu.bm.metadata.title + response.data.menu.bm.metadata.artist + response.data.menu.bm.metadata.mapper + response.data.menu.bm.metadata.difficulty).digest("hex"),
                    }

                    if (!fs.existsSync("./src/user/" + username + ".json")) {
                        fs.writeFileSync("./src/user/" + username + ".json", JSON.stringify({
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
                            "pp": {
                                "osu": [],
                                "taiko": [],
                                "catch": [],
                                "mania": []
                            }
                        }, null, 4));
                    }

                    const json = JSON.parse(fs.readFileSync("./src/user/" + username + ".json", "utf-8"));
                    const mode = modeConverter(currentMode);

                    if (json.pp[mode].length > 0) {
                        let flag = false;
                        for (let i = 0; i < json.pp[mode].length; i++) {
                            if (json.pp[mode][i].hash == confirmHash) {
                                if ((json.pp[mode][i].score < response.data.gameplay.score && json.pp[mode][i].mods == response.data.menu.mods.str) || (pp > json.pp[mode][i].pp)) {
                                    json.pp[mode][i] = data;
                                    flag = true;
                                } else {
                                    response = null;
                                    return;
                                }
                            }
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
                    json.bonusPP[mode] = bonusPP;
                    json.globalPP[mode] = globalPP;
                    json.globalACC[mode] = globalACC;
                    json.lastGamemode = currentMode;
                    fs.writeFileSync("./src/user/" + username + ".json", JSON.stringify(json, null, 4));
                }
                currentStatus = response.data.menu.state;
                currentMode = response.data.menu.gameMode;
                response = null;
            } catch (e) {
                console.log(e)
            }
        }, 100);
}

main(process.argv[2]);

function calculator (mappath, params) {
    const beatmap = new Beatmap({ path: mappath });
    const pp = new Calculator(params).performance(beatmap).pp;
    return pp;
}

function calculateGlobalPP (json, mode) {
    let globalPP = 0;
    for (let i = 0; i < Math.min(json.pp[mode].length, 100); i++) {
        globalPP += json.pp[mode][i].pp * Math.pow(0.95, i);
    }
    return globalPP;
}

function calculateGlobalACC (json, mode) {
    let globalACC = [];
    for (let i = 0; i < Math.min(json.pp[mode].length, 100); i++) {
        globalACC.push(json.pp[mode][i].acc);
    }
    return globalACC.reduce((a, b) => a + b) / globalACC.length;
}

function calculateBonusPP (json, mode) {
    let bonusPP = 0;
    for (let i = 0; i < json.pp[mode].length; i++) {
        bonusPP += 416.6667 * (1 - Math.pow(0.9994, i + 1));
    }
    return bonusPP;
}

function modeConverter (mode) {
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
