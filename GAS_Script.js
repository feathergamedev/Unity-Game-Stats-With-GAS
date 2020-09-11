// Copy this script to Google Apps Script 

function doPost(e) 
{
  // TODO : Replace with your own Spreadsheet ID.
  var app = SpreadsheetApp.openById("13cJe2iY1ZHyHjrYoayrAQ85KvbhapfnAS_RvOmu5ll4");
  
  var sheet = app.getSheets()[0];
  var parameter = e.parameter;

  var visitCountRow = 3;
  var visitCountCol = 4;
  
  var rankStartRow = 7;
  var rankStartCol = 2;
  
  var totalRankCountRow = 3;
  var totalRankCountCol = 2;
  
  var outputMsg = "";
  
  if (parameter.method == "GainNewVisitor")
  {
    var range = sheet.getRange(visitCountRow,visitCountCol);
    var curVisitCount = range.getValue();
    
    range.setValue(curVisitCount+1);   
    
    outputMsg = curVisitCount+1;
  }
  else if (parameter.method == "AddNewScore")
  {
    var score = parseInt(parameter.score, 10);
    
    var myRank = 1;
    var curRow = rankStartRow;            
    var range = sheet.getRange(curRow, rankStartCol);
    var scoreOnBoard = range.getValue();

    var curRankCount = sheet.getRange(totalRankCountRow, totalRankCountCol).getValue();            
    
    while (score < scoreOnBoard)
    {      
      myRank += sheet.getRange(curRow, rankStartCol+1).getValue();
      
      curRow++;      
      range = sheet.getRange(curRow, rankStartCol);
      
      if (myRank > curRankCount)
        break;
      
      scoreOnBoard = range.getValue();
    }    
    
    if (score == scoreOnBoard)
    {
      range = sheet.getRange(curRow, rankStartCol+1);
      range.setValue(range.getValue()+1);
    }
    else
    {
      sheet.insertRowBefore(curRow);
      sheet.getRange(curRow,rankStartCol).setValue(score); 
      sheet.getRange(curRow,rankStartCol+1).setValue(1);            
    }
          
    outputMsg = myRank;    
  }          
  else if (parameter.method == "GetScoreByRank")
  {
    var targetRank = parseInt(parameter.targetRank, 10);
    var curRankCount = sheet.getRange(totalRankCountRow, totalRankCountCol).getValue();                
    
    if (targetRank > curRankCount)
      return ContentService.createTextOutput("?");
    
    var curRank = 0;
    var curRow = rankStartRow;            

    while (true)
    {      
      curRank += sheet.getRange(curRow, rankStartCol+1).getValue();        
      
      if (curRank >= targetRank)
      {
        outputMsg = sheet.getRange(curRow, rankStartCol).getValue(); 
        break;        
      }      
      else
        curRow++;
    }
  }  
  else if (parameter.method == "GetScoreOfTopN")
  {
    var number = parseInt(parameter.number, 10);      
    var curRankCount = sheet.getRange(totalRankCountRow, totalRankCountCol).getValue();            
    var result = "";

    var counter = 0;
    var i = 0;    
    
    while (counter < number)
    {     
      
      if (counter >= curRankCount)
      {   
        result += "?";

        counter++;
          
        if (counter != number)
          result += ",";                  
      }
      else
      {
        var playerScore = sheet.getRange(rankStartRow+i, rankStartCol).getValue();
        var playerCount = sheet.getRange(rankStartRow+i, rankStartCol+1).getValue();
        
        for (var j=0; j<playerCount; j++)
        {
          result += playerScore.toString();
          
          counter++;
          
          if (counter == number)
            break;        
          else
            result += ",";          
        }            
      }           
      
      i++;            
    }
    
    outputMsg = result;
  }
  
  return ContentService.createTextOutput(outputMsg);
}
