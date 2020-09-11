# Unity-Game-Stats-With-GAS

## 介紹
`Unity Game Stats With GAS` 是專為 Unity Engine 開發的工具，旨在結合 GAS(Google Apps Script)，用 Google 雲端試算表達成玩家的數據統計。

情境1：玩家進入遊戲時，顯示他是第 n 位遊玩者

情境2：Casual Game通關後，上傳遊玩分數並計算玩家獲得的名次

情境3：列出排行榜上的前 10 名玩家成績

這個工具必須在有網路的環境下才能運作，再加上資料安全性、穩定性等因素，所以僅建議用在 Game Jam、Demo 作品上。

## 核心要素
● Google 雲端硬碟的試算表

● 有連網的 Unity 專案執行環境(編輯器內, WebGL, Mobile..皆可)

## 目前具備的功能
● 統計遊玩人次

● 將成績登錄到排行榜，取得名次

● 查詢排行榜上第 n 名的成績

● 列出排行榜上的前 n 筆成績

## 優點
● 輕鬆搭建免費的數據後台

## 缺點
● 資料保護性低，取得 URL 的人都可以隨意發 Request 影響資料

● 如果短時間接收到太多 Request ，有可能會丟失部分執行命令

## 實裝步驟
1. 打開我的 Google Sheet 範本：https://docs.google.com/spreadsheets/d/13cJe2iY1ZHyHjrYoayrAQ85KvbhapfnAS_RvOmu5ll4/edit?usp=sharing

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

![](https://i.imgur.com/JrbiqWY.png)

7. 把 Current web app URL 複製起來 (等等會用到)

![](https://i.imgur.com/udcTwo6.png)

8. 把 PlayerStatsManager.cs 放入 Unity 的 Asset 資料夾裡

9. 把 Current web app URL 字串貼上到 PlayerStatsManager.cs 的 GAS_URL
![](https://i.imgur.com/4g2offe.png)

## DEMO場景
![](https://i.imgur.com/FK9lBrF.png)


所有核心功能的 Function 都可宣告 Action<string> 接收來自 GAS 的回傳字串，再視個人需求對資料進行後續運用。

Ex1.
```
DEMO_PlayerStatsManager.Instance.NewVisit((x) =>
{
    ShowVisitorCount(x);
});
```
假設玩家是第 20 個遊玩人次，Request發送完畢後，變數 `x` 就會回傳 `"20"`。

Ex2.
```
public void GetScoreOfTop10()
{
    DEMO_PlayerStatsManager.Instance.GetScoreOfTopN(10, (x) =>
    {
        RefreshLeaderBoard(x);
    });
}

private void RefreshLeaderBoard(string str)
{
    var data = str.Split(',');

    m_leaderBoardText.text = "";

    for (int i = 0; i < 10; i++)
    {
        m_leaderBoardText.text += string.Format("No.{0} : {1}\n", i + 1, data[i]);
    }

    Debug.Log("Leaderboard is refreshed.");
}
```

`GetScoreOfTopN(int number)`的回傳值是包含 N 筆成績的字串，每筆成績以逗號`,`隔開，如果排行榜上的成績筆數少於 N ，剩下的會以問號`?`顯示。範例：`"100,50,30,15,?,?"`
可以用 `String.Split(',')` 將每筆數字拆開並轉存成 `Array`。

## 注意事項
● 可以直接從雲端試算表修改內容，但要注意讀/寫資料的目標儲存格位置。相關設定在指令碼編輯器裡。

● 傳輸數據的型別都是設計為`string`，記得視需求作轉換。

● 如果排行榜上不存在所要求的特定名次成績，會以問號`?`形式回傳。

## 參考資源
● [Class Sheet | Apps Script | Google Developers](https://developers.google.com/apps-script/reference/spreadsheet/sheet)

● [【阿空】用Google試算表建立資料庫！？(連結Unity) →【GAS (Google Apps Script) - Spreadsheet】](https://youtu.be/SfRXsiuzbCI)

