using WireLabel.BL.Models;
using WireLabel.BL.Services.Interfaces;
using Excel = Microsoft.Office.Interop.Excel;
using System.Drawing;

namespace WireLabel.BL.Services;

public class ExcelService : IExcelService
{
    private const string WireSheetName = "Wires";
    private const string BOMSheetName = "BOM";
    private const string WireType = "ZUSCHNITT";
    private const string WireTypeRus = "Провод";
    private const string SpliceType = "SP_";
    private const string SpliceTypeRus = "Клип";
    private const string AntennaType = "ANTENNE";
    private const string AntennaTypeRus = "Антена";
    private const string TwistedWiresType = "RINGKABELSCHUH";
    private const string TwistedWiresTypeRus = "Плетёнка";
    private const string TerminalType = "GEHAEUSE";
    private const string TerminalTypeAbbreviated = "GEH.";
    private const string TerminalTypeRus = "Разъём";
    private const string CapType = "BUCHSENGEHAEUSE";
    private const string CapTypeRus = "Крышка";
    private const string HubType = "TUELLE";
    private const string HubTypeRus = "Втулка";
    private const string HolderType = "HALTER";
    private const string HolderTypeRus = "Крепление";
    private const string ClipType = "CLIP";
    private const string ClipTypeRus = "Клипса";
    private const string TapeType = "BAND";
    private const string ThermalLabelType = "THERMO-ETIKETT";
    private const string UnknownPartRus = "Деталь";
    private const int WirePartNumberLength = 13;
    private const int FontSize = 16;
    private const int NameFontSize = 28;
    private const string hygroscopicPart = "KONDYC";
    public void MakeExcelFile(List<Label> labelsForOneWorkplace, string path)
    {
        var ex = new Excel.Application
        {
            DisplayAlerts = false,
            SheetsInNewWorkbook = 2
        };
        ex.Workbooks.Add(Type.Missing);
        var wireSheet = ex.Worksheets[1];
        var bomSheet = ex.Worksheets[2];
        wireSheet.Name = WireSheetName;
        bomSheet.Name = BOMSheetName;
        var fileName = labelsForOneWorkplace[0].Module;
        var wireList = new List<Label>();
        var bomList = new List<Label>();
        SortToWireAndBOMList(labelsForOneWorkplace, wireList, bomList);
        FillWireList(wireList, wireSheet);
        FillBOMList(bomList, bomSheet);
        ex.Application.ActiveWorkbook.SaveAs(path + @"\" + fileName);
        ex.Application.ActiveWorkbook.Close();
    }
    private void SortToWireAndBOMList(List<Label> labelsForOneWorkplace, List<Label> wireList, List<Label> bomList)
    {
        foreach(var label in labelsForOneWorkplace)
        {
            if (label.Type.ToUpper().Contains(WireType))
            {
                ChangeTypeAndAddToList(label, WireTypeRus, wireList);
                continue;
            }

            if (label.Type.ToUpper().Contains(SpliceType))
            {
                ChangeTypeAndAddToList(label, SpliceTypeRus, wireList);
                continue;
            }

            if (label.Type.ToUpper().Contains(AntennaType))
            {
                ChangeTypeAndAddToList(label, AntennaTypeRus, wireList);
                continue;
            }

            if (label.Type.ToUpper().Contains(TwistedWiresType))
            {
                ChangeTypeAndAddToList(label, TwistedWiresTypeRus, wireList);
                continue;
            }

            if (label.Type.ToUpper().Contains(TerminalType) || label.Type.ToUpper().Contains(TerminalTypeAbbreviated))
            {
                ChangeTypeAndAddToList(label, TerminalTypeRus, bomList);
                continue;
            }

            if (label.Type.ToUpper().Contains(CapType))
            {
                ChangeTypeAndAddToList(label, CapTypeRus, bomList);
                continue;
            }

            if (label.Type.ToUpper().Contains(HubType))
            {
                ChangeTypeAndAddToList(label, HubTypeRus, bomList);
                continue;
            }

            if (label.Type.ToUpper().Contains(ThermalLabelType) || label.Type.ToUpper().Contains(TapeType))
            {
                continue;
            }

            if (label.PartNumber.Length == WirePartNumberLength)
                ChangeTypeAndAddToList(label, WireTypeRus, wireList);
            else
                ChangeTypeAndAddToList(label, UnknownPartRus, bomList);
        }
    }

    private void ChangeTypeAndAddToList(Label label, string type, List<Label> labelsList)
    {
        label.Type = type;
        labelsList.Add(label);
    }

    private void FillWireList(List<Label> wireList, dynamic wireSheet)
    {
        for (int row = 1; wireList.Count != 0; row += 4)
        {
            for (int column = 1; column <= 3; column++)
            {
                wireSheet.Cells[row, column].Value = wireList[0].Type + " " + wireList[0].Module;
                wireSheet.Cells[row, column].Font.Size = FontSize;
                wireSheet.Cells[row + 1, column].Value = ListToString(wireList[0].NumberOfVariants);
                wireSheet.Cells[row + 1, column].Font.Size = FontSize;
                wireSheet.Cells[row + 2, column].Value = ListToString(wireList[0].Names);
                if (wireList[0].SpliceComposition != null)
                {
                    wireSheet.Cells[row + 2, column].Value += "\n(" + ListToString(wireList[0].SpliceComposition) + ")";
                    wireSheet.Cells[row + 2, column].Font.Size = FontSize;
                }
                else
                    wireSheet.Cells[row + 2, column].Font.Size = NameFontSize;
                wireSheet.Cells[row + 3, column].Value = wireList[0].PartNumber;
                wireSheet.Cells[row + 3, column].Font.Size = FontSize;
                wireList.RemoveAt(0);
                if (wireList.Count == 0)
                {
                    SetFormat(wireSheet, row + 3, 3, 25);
                    break;
                }
            }
        }
    }
    private void FillBOMList(List<Label> bomList, dynamic bomSheet)
    {
        for (int row = 1; bomList.Count != 0; row += 4)
        {
            bomSheet.Cells[row, 1].Value = bomList[0].Type;
            bomSheet.Cells[row, 1].Font.Size = FontSize;
            bomSheet.Cells[row + 1, 1].Value = bomList[0].Module;
            bomSheet.Cells[row + 1, 1].Font.Size = FontSize;
            bomSheet.Cells[row + 2, 1].Value = ListToString(bomList[0].Names);
            bomSheet.Cells[row + 2, 1].Font.Size = NameFontSize;
            bomSheet.Cells[row + 3, 1].Value = bomList[0].PartNumber;
            bomSheet.Cells[row + 3, 1].Font.Size = FontSize;
            bomSheet.Cells[row + 3, 2].Value = bomList[0].DrowingNumber;
            bomSheet.Cells[row + 3, 2].Font.Size = FontSize;
            bomSheet.Range(bomSheet.Cells[row, 2], bomSheet.Cells[row + 2, 2]).Merge(false);
            if (bomList[0].DrowingNumber.ToUpper().Contains(hygroscopicPart))
            {
                bomSheet.Range(bomSheet.Cells[row, 1], bomSheet.Cells[row + 3, 2]).Interior.Color = Color.FromArgb(1, 0, 176, 240);
            }
            bomList.RemoveAt(0);
            if (bomList.Count == 0)
            {
                SetFormat(bomSheet, row + 3, 2, 35);
            }
        }        
    }

    private string ListToString(List<string> words)
    {
        string allWords = words[0];
        if (words.Count == 1) return allWords;
        words.RemoveAt(0);
        foreach (var word in words)
        {
            allWords += ", " + word;
        }       
        return allWords;
    }

    private void SetFormat(dynamic sheet, int row, int column, int columnWidth)
    {
        Excel.Range range = sheet.Range(sheet.Cells[1, 1], sheet.Cells[row, column]);
        range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
        range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        range.EntireColumn.ColumnWidth = columnWidth;
        range.EntireRow.AutoFit();
        range.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;
        range.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
        range.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
        range.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
        range.Borders.get_Item(Excel.XlBordersIndex.xlInsideHorizontal).LineStyle = Excel.XlLineStyle.xlContinuous;
        range.Borders.get_Item(Excel.XlBordersIndex.xlInsideVertical).LineStyle = Excel.XlLineStyle.xlContinuous;
    }
}
