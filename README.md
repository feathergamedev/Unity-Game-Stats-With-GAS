# Unity Game Stats With GAS

[DEMO Video](https://youtu.be/Nl4VP78Qrmo)

[Playable WebGL Demo](https://feather-chung.itch.io/unity-game-stats-with-gas)

## 介紹
`Unity Game Stats With GAS` 是專為 Unity Engine 開發的小工具，結合 GAS(Google Apps Script)，用 Google 雲端試算表打造簡易的數據統計資料庫。

情境1：玩家進入遊戲時，顯示他是第 n 位遊玩者

情境2：通過關卡後，上傳玩家的分數，並計算他在所有玩家當中的名次

情境3：列出排行榜上前 10 名的玩家成績

這個工具必須在有網路的環境下才能運作，再加上資料安全性、穩定性等因素，所以不建議在正式產品中使用。

## 核心要素
● 放在 Google 雲端硬碟的試算表

● 有連網的 Unity 專案執行環境(編輯器內, WebGL, Mobile..皆可)

## 目前具備的功能
● 統計遊玩人次 `GainNewVisitor()`

● 統計通關人次 `GainNewFinisher()`

● 將成績登錄到排行榜，取得名次 `AddNewScore(int score)`

● 查詢排行榜上第 n 名的成績 `GetScoreByRank(int rank)`

● 列出排行榜上的前 n 筆成績 `GetScoreOfTopN(int number)`

## 優點
● 輕鬆搭建免付費的數據後台

## 缺點
● 資料保護性低，取得 URL 的人都可以隨意發 Request 影響資料

● 試算表同步效率差，如果太密集接收到寫檔 Request ，可能會遺漏部份行為。

## 實裝步驟
1. 用新分頁開啟我的 Google Sheet 範本：https://docs.google.com/spreadsheets/d/13cJe2iY1ZHyHjrYoayrAQ85KvbhapfnAS_RvOmu5ll4/edit?usp=sharing

2. 建立一份副本在你的雲端硬碟(放在你有編輯權限的位置)

3. 打開你建立的副本後，點擊上方選單：工具 -> 指令碼編輯器

4. 把GAS_Script.js內容貼到指令碼編輯器

5. 把 Line 3 標示處改成你的試算表 ID (網址的.../d/後面那一串)

![](https://i.imgur.com/RKSBYrc.png)

6. 點擊上方選單：發布 -> 部署為網頁應用程式

    ● 設定 Project version : 新增 

    (每當更動這裡的 code 都要重新部署，並且選「新增」版本)

    ● 設定 Execute the app as ： Me

    ● 設定 Who can access to the app : Anyone, even anonymous

![](https://i.imgur.com/4wXNF0T.png)


7. 把 Current web app URL 複製起來 (等等會用到)

![](https://i.imgur.com/g1dr1LI.png)

8. 把 `PlayerStatsManager.cs` 以及 `Demo資料夾` 放到 Unity 專案的 Asset 資料夾內

9. 把 Current web app URL 字串貼上到 `PlayerStatsManager.cs` 的 `GAS_URL`
![](https://i.imgur.com/4g2offe.png)

## DEMO場景
![](https://i.imgur.com/bW8cgNB.gif)


所有核心功能的 Function 都可使用 `Action<string>` 接收來自 GAS 的回傳字串。
以 DEMO 場景內來說，我一律使用帶有 `x` 變數的 Lambda 表示式，再依個人需求對資料進行後續運用(刷新UI、判斷勝敗...等)。

Ex1.
```
private void GainNewVisitor()
{
    DEMO_PlayerStatsManager.Instance.GainNewVisitor((x) =>
    {
        ShowVisitorCount(x);
    });
}
```
假設玩家是第 20 個遊玩人次，Request發送完畢後，變數 `x` 就會回傳 `"20"`。

Ex2.
```
public void GetScoreOfTop10()
{
    DEMO_PlayerStatsManager.Instance.GetScoreOfTopN(10, (x) =>
    {
        RefreshLeaderboard(x);
    });
}

private void RefreshLeaderboard(string str)
{
    var data = str.Split(',');

    m_leaderBoardText.text = "";

    for (int i = 0; i < 10; i++)
    {
        m_leaderBoardText.text += string.Format("No.{0} : {1}\n", i + 1, data[i]);
    }
}
```

`GetScoreOfTopN(int number)`的回傳值是包含 N 筆成績的字串，每筆成績以逗號`,`隔開。
如果排行榜上的成績筆數少於 N 個，剩下的會以問號`?`顯示。 
(ex.`"100,50,30,15,?,?"`)

可以用 `String.Split(',')` 將每筆數字拆開並轉存成 `Array`。

## 注意事項
● 可以直接從雲端試算表修改內容，但要維持讀/寫資料的目標儲存格位置。相關設定在指令碼編輯器裡。
```
  var visitorCountRow = 3;
  var visitorCountCol = 4;
  // 代表「紀錄遊玩人次」的儲存格在 D3
```
● 傳輸數據的型別都是設計為`string`，記得視需求作轉換。

● 如果排行榜上不存在所要求的特定名次，該筆成績    會以問號`?`形式回傳。

## 參考資源
● [Class Sheet | Apps Script | Google Developers](https://developers.google.com/apps-script/reference/spreadsheet/sheet)

● [【阿空】用Google試算表建立資料庫！？(連結Unity) →【GAS (Google Apps Script) - Spreadsheet】](https://youtu.be/SfRXsiuzbCI)

