using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OfficeOpenXml;
using System.IO;


public class EnemyDataScript : MonoBehaviour {
    
    [System.Serializable]
    public struct chessData{
        public string chessName;
        public int id;
        public int star;
        public int price;
        public string _race_class;
        public int max_HP;

        public float baseAttack_dmg;
        public float atk_speed;
        public float armor;
        public float baoji;
        public float lifeSteal;
        public float HP_Regen;
        public float atk_Range;
    }

    public chessData[] allChessData;



    public void GetString(){
        string filePath = @"Assets\Resources\_Data\ChessData.xlsx";

        FileInfo fileInfo = new FileInfo(filePath);

        using (ExcelPackage excelPackage = new ExcelPackage(fileInfo)){

            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["EnemyData"];

            //string s = worksheet.Cells[1,1].Value.ToString();

            int chessNumber = worksheet.Dimension.End.Row-1;
            allChessData = new chessData[chessNumber];
            for(int i=1; i<worksheet.Dimension.End.Row; i++){              
                allChessData[i-1].chessName = worksheet.Cells[i+1,1].Value.ToString();
                allChessData[i-1].id = int.Parse(worksheet.Cells[i+1,2].Value.ToString());
                allChessData[i-1].star = int.Parse(worksheet.Cells[i+1,3].Value.ToString());
                allChessData[i-1].price = int.Parse(worksheet.Cells[i+1,4].Value.ToString());
                allChessData[i-1]._race_class = worksheet.Cells[i+1,5].Value.ToString();
                allChessData[i-1].max_HP = int.Parse(worksheet.Cells[i+1,6].Value.ToString());

                allChessData[i-1].baseAttack_dmg = float.Parse(worksheet.Cells[i+1,7].Value.ToString());
                allChessData[i-1].atk_speed = float.Parse(worksheet.Cells[i+1,8].Value.ToString());
                allChessData[i-1].armor = float.Parse(worksheet.Cells[i+1,9].Value.ToString());
                allChessData[i-1].baoji = float.Parse(worksheet.Cells[i+1,10].Value.ToString());
                allChessData[i-1].lifeSteal = float.Parse(worksheet.Cells[i+1,11].Value.ToString());
                allChessData[i-1].HP_Regen = float.Parse(worksheet.Cells[i+1,12].Value.ToString());
                allChessData[i-1].atk_Range = float.Parse(worksheet.Cells[i+1,13].Value.ToString());
            }
        }

    }
    


}


