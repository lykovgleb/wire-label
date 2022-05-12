using WireLabel.BL.Models;
using WireLabel.BL.Services.Interfaces;
using Excel = Microsoft.Office.Interop.Excel;

namespace WireLabel.BL.Services;

public class ExcelService : IExcelService
{
    private const string WireSheetName = "Wires";
    private const string BOMSheetName = "BOM";
    private const int WirePartNumberLength = 13;    
    public void MadeExcelFile(List<Label> labelsForOneWorkplace, string path)
    {
        var ex = new Excel.Application
        {
            DisplayAlerts = false,
            SheetsInNewWorkbook = 2
        };
        var workbook = ex.Workbooks.Add(Type.Missing);
        var wireSheet = ex.Worksheets[1];
        var bomSheet = ex.Worksheets[2];
        wireSheet.Name = WireSheetName;
        bomSheet.Name = BOMSheetName;
        var fileName = labelsForOneWorkplace[0].Module;
        var bomList = new List<Label>();
        for(int row = 1; labelsForOneWorkplace.Count != 0; row+=4)
        {
            for(int column = 1; column<4; column++)
            {               
                while (labelsForOneWorkplace[0].PartNumber.Length != WirePartNumberLength)
                {
                    bomList.Add(labelsForOneWorkplace[0]);
                    labelsForOneWorkplace.RemoveAt(0);
                    if (labelsForOneWorkplace.Count == 0) break;
                }
                if (labelsForOneWorkplace.Count == 0) break;
                wireSheet.Cells[row, column].Value = labelsForOneWorkplace[0].Module;
                string allVariants = " ";
                foreach (var variant in labelsForOneWorkplace[0].NumberOfVariants)
                {
                    allVariants += variant + " ";
                }
                wireSheet.Cells[row + 1, column].Value = allVariants.Trim();
                string allNames = " ";
                foreach (var Name in labelsForOneWorkplace[0].Names)
                {
                    allNames += Name + " ";
                }
                wireSheet.Cells[row + 2, column].Value = allNames.Trim();
                wireSheet.Cells[row + 3, column].Value = labelsForOneWorkplace[0].PartNumber;
                labelsForOneWorkplace.RemoveAt(0);
                if (labelsForOneWorkplace.Count == 0) break;
            }            
        }
        for (int row = 1; bomList.Count != 0; row += 4)
        {
            bomSheet.Cells[row + 1, 1].Value = bomList[0].Module;
            string allNames = " ";
            foreach (var Name in bomList[0].Names)
            {
                allNames += Name + " ";
            }
            bomSheet.Cells[row + 2, 1].Value = allNames.Trim();
            bomSheet.Cells[row + 3, 1].Value = bomList[0].PartNumber;
            bomList.RemoveAt(0);
        }
        ex.Application.ActiveWorkbook.SaveAs(path + @"\" + fileName);
        ex.Application.ActiveWorkbook.Close();     
    }    
}
