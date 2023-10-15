<img src="https://github.com/puk06/osu-private/assets/86549420/e06c137b-d802-4995-a106-1991730e9879" width="1000">

# osu!Private
オフライン環境で動くオフラインユーザー向けのosu! PP計算機です！

osu! PP calculator for offline users that works in an offline environment 🚀

# ScreenShot
![](https://github.com/puk06/osu-private/assets/86549420/5d83d44b-5b16-4e31-913b-c40aae19d9dd)

# How to use(JP)
1. **ユーザー名を入力する**
    *  ユーザー名について
    > - ユーザー名を新たに入力すると、ユーザーが作成されます。
    > - ユーザー名をリストから選択するとそのユーザー情報が読み込まれます。
    > - **新しいユーザーを作成した場合、記録を1つ付けないとユーザー情報が保存されないので注意してください。**

<br>

2. **Play osu!をクリック！**

# How to use(EN)
1. **Enter a Username**
    * About Username.
    > - Entering a new user name creates a user.
    > - Selecting a user name from the list will load that user's information.
    > - **Please note that when a new user is created, user information will not be saved unless one record is added.**

<br>

2. **Click "Play osu!" !!**

# About this Software
- **このソフトについて**
  - このソフトは、オフライン環境で通常のプライベートサーバー、Banchoと同じようにGlobalPP、ACC、BonusPPを計算します。
  - osu!でのログインも必要ありません！
  * このソフトはこのような方におすすめです。
    > - オフラインユーザーだけどPPが見たいという方
    > - 練習でオフラインになるけどPPが気になるという方
    > - 特定のPPまでタイムアタックがしたいという方
    > - 既にosu!のアカウントを持ってるけど、また0から初めたいという方
    > - サブ垢をたくさん作りたいという方

<br>

- **記録**
  - 記録はプレイ画面→リザルト画面に移った瞬間に付きます。
  - そのため、リプレイを見るだけでも記録が付いてしまいます。
  - 記録を保存する際に、リザルト画面が出て、ソフトにPPが反映されてから選曲画面に戻るようにしてください！

<br>

- **PP計算**
  - このソフトのPP計算には[rosu-pp-js v0.9.4](https://github.com/MaxOhn/rosu-pp-js/releases/tag/v0.9.4)を使用しています。
  - rosu-pp-js v0.9.4は現在時点(2023/10/15)での最新のPP計算式です。

<br>

- **ユーザーデータの引き継ぎ**
  - ユーザーデータは、srcフォルダ内のuserというフォルダの中に(ユーザー名).jsonが入っているので、引き継ぎたいデータをコピーし、新しいソフトのユーザーフォルダに貼り付けるだけです。

<br>

# Important
- **リプレイ**
  > - このソフトは仕様上、他人のリザルトを見るだけで記録が付いてしまいます。他人の記録を保存したくない場合は、リプレイを見ないようにしてください！
  > - リプレイでもプレイ画面からリザルト画面に行った瞬間に記録されるので、リザルト画面が表示される前に選曲画面に戻れば保存されません！

<br>

- **ウイルス**
  > - 詳しくは分かりませんが、このソフトがウイルス判定を受ける事があるようです。(実際に私がこのソフトを作っているときにウイルス判定されて削除されました。)
  > - 1000%このソフトがウイルスでは無いことを保証します。絶対です。コードもとても短いので、心配であればチェックしてください！主なコードとして、osu!private.js、mainForm.cs、registrationForm.cs、Program.csの4つが挙げられます。
  > - もし怖すぎて心配という方は、自分でビルドするのが良いと思います。Visual Studioなどを使えば簡単にビルドが可能です！

# Dependencies
- **rosu-pp-js**
  > - Github : https://github.com/MaxOhn/rosu-pp-js
  > - User : [MaxOhn](https://github.com/MaxOhn)
  > - Lisence : [MIT License](https://github.com/MaxOhn/rosu-pp-js/blob/main/LICENSE)https://github.com/MaxOhn/rosu-pp-js/blob/main/LICENSE

<br>

- **Gosumemory**
  > - Github : https://github.com/l3lackShark/gosumemory
  > - User : [l3lackShark](https://github.com/l3lackShark)
  > - Lisence : [GNU General Public License v3.0](https://github.com/l3lackShark/gosumemory/blob/master/LICENSE)

