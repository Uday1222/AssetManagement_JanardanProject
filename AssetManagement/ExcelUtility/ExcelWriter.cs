using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;

namespace AssetManagement.ExcelUtility
{
    public class ExcelWriter : ResourceCleaner
    {
        private SpreadsheetDocument document;
        private WorkbookPart workbookpart;
        private Sheets sheets;

        private string ColumnLetter(int intCol)
        {
            var intFirstLetter = ((intCol) / 676) + 64;
            var intSecondLetter = ((intCol % 676) / 26) + 64;
            var intThirdLetter = (intCol % 26) + 65;

            var firstLetter = (intFirstLetter > 64)
                ? (char)intFirstLetter : ' ';
            var secondLetter = (intSecondLetter > 64)
                ? (char)intSecondLetter : ' ';
            var thirdLetter = (char)intThirdLetter;

            return string.Concat(firstLetter, secondLetter,
                thirdLetter).Trim();
        }

        private Cell CreateTextCell(string header, UInt32 index, string text, CellValues dataType = CellValues.InlineString, uint styleIndex = 0)
        {

            var cell = new Cell
            {
                DataType = CellValues.InlineString,
                CellReference = header + index,
                StyleIndex = styleIndex,

            };

            var istring = new InlineString();
            var t = new Text { Text = text };
            istring.AppendChild(t);

            cell.AppendChild(istring);
            return cell;
        }

        public void GenerateExcel(OpenXMLData data, string reportPath)
        {
            //var stream = new MemoryStream();
            string path = reportPath;
            document = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook);

            workbookpart = document.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();
            var worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
            var sheetData = new SheetData();

            worksheetPart.Worksheet = new Worksheet(sheetData);

            WorkbookStylesPart stylePart = workbookpart.AddNewPart<WorkbookStylesPart>();
            stylePart.Stylesheet = GenerateStylesheet();
            stylePart.Stylesheet.Save();

            sheets = document.WorkbookPart.Workbook.
                AppendChild<Sheets>(new Sheets());

            var sheet = new Sheet()
            {
                Id = document.WorkbookPart
                .GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = data.SheetName ?? "ReportData"
            };
            sheets.AppendChild(sheet);

            // Add header
            UInt32 rowIdex = 0;
            var row = new Row { RowIndex = ++rowIdex, Height = 25, CustomHeight = true };
            sheetData.AppendChild(row);
            var cellIdex = 0;

            foreach (var header in data.Headers)
            {
                row.AppendChild(CreateTextCell(ColumnLetter(cellIdex++),
                    rowIdex, header ?? string.Empty, styleIndex: 2));
            }
            if (data.Headers.Count > 0)
            {
                // Add the column configuration if available
                if (data.ColumnConfigurations != null)
                {
                    var columns = (Columns)data.ColumnConfigurations.Clone();
                    worksheetPart.Worksheet
                        .InsertAfter(columns, worksheetPart
                        .Worksheet.SheetFormatProperties);
                }
            }

            // Add sheet data
            foreach (var rowData in data.DataRows)
            {
                cellIdex = 0;
                row = new Row { RowIndex = ++rowIdex };
                sheetData.AppendChild(row);
                foreach (var callData in rowData)
                {
                    var cell = CreateTextCell(ColumnLetter(cellIdex++),
                        rowIdex, callData ?? string.Empty, styleIndex: 1);
                    row.AppendChild(cell);
                }
            }

            workbookpart.Workbook.Save();

            Columns lstColumns = worksheetPart.Worksheet.GetFirstChild<Columns>();
            Boolean needToInsertColumns = false;
            if (lstColumns == null)
            {
                lstColumns = new Columns();
                needToInsertColumns = true;
            }
            lstColumns.Append(new Column() { Min = 1, Max = 15, Width = 15, CustomWidth = true });

            if (needToInsertColumns)
                worksheetPart.Worksheet.InsertAt(lstColumns, 0);

            // Get the sheetData cells
            sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
            //return stream.ToArray();
        }



        public void AddSheetToWorkbook(OpenXMLData data, OpenXMLData Implementdata = null, OpenXMLData Deferdata = null, OpenXMLData SignOffApprovalconditionData = null, OpenXMLData AgeingAssignImpleConditionData = null, string reportPath = null)
        {
            WorksheetPart worksheetPart = null;
            SheetData sheetData = null;
            if (document == null)
            {
                document = SpreadsheetDocument.Create(reportPath, SpreadsheetDocumentType.Workbook);
                workbookpart = document.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();
                worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);
                WorkbookStylesPart stylePart = workbookpart.AddNewPart<WorkbookStylesPart>();
                stylePart.Stylesheet = GenerateStylesheet();
                stylePart.Stylesheet.Save();
                sheets = document.WorkbookPart.Workbook.
               AppendChild<Sheets>(new Sheets());
            }
            int Total = 0;
            foreach (var item in data.DataRows)
            {
                Total = Total + Convert.ToInt32(item[1]);
            }
            foreach (var item in Implementdata.DataRows)
            {
                Total = Total + Convert.ToInt32(item[1]);
            }
            foreach (var item in Deferdata.DataRows)
            {
                Total = Total + Convert.ToInt32(item[1]);
            }
            foreach (var item in SignOffApprovalconditionData.DataRows)
            {
                Total = Total + Convert.ToInt32(item[1]);
            }
            foreach (var item in AgeingAssignImpleConditionData.DataRows)
            {
                Total = Total + Convert.ToInt32(item[1]);
            }
            worksheetPart = document.WorkbookPart.AddNewPart<WorksheetPart>();

            sheetData = new SheetData();
            worksheetPart.Worksheet = new Worksheet(sheetData);

            uint sheetId = 1;
            if (sheets.Elements<Sheet>().Count() > 0)
            {
                sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
            }

            var sheet = new Sheet()
            {
                Id = document.WorkbookPart
                .GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = data.SheetName ?? "ReportData Sheet " + sheetId
            };
            sheets.AppendChild(sheet);

            // Add header
            UInt32 rowIdex = 1;
            var row = new Row { RowIndex = ++rowIdex, Height = 30, CustomHeight = true };
            var cellIdex = 0;
            //Add Header Text

            sheetData.AppendChild(row);
            String[] HeadingName = new String[] { "Total Open Ideas:",Convert.ToString(Total), "Red Flagged Rules",
                @"Idea Validation,Accept Implementer, Provide More Info","Implementation",@"Deferred,Implementation Sign off pending,Approval in progress","Assign Implementer Role"," > 12 months from Target Implementation Date" };

            String[] HeadingText = new String[] { "(For quick reference)","> 45 days", " > 270 days","> 60 days","> 30 days",
                @"Red Flag" };

            int IndexCount = 0;
            foreach (var item in HeadingName)
            {
                if (IndexCount <= 1)
                {
                    row.AppendChild(CreateTextCell(ColumnLetter(cellIdex++),
                           rowIdex, item, styleIndex: 2));
                }
                else
                {
                    row.AppendChild(CreateTextCell(ColumnLetter(cellIdex++),
                                               rowIdex, item, styleIndex: 6));
                }
                IndexCount++;
            }

            //Add Heading Text
            IndexCount = 0;
            cellIdex = 2;
            row = new Row { RowIndex = ++rowIdex, Height = 15, CustomHeight = true };
            sheetData.AppendChild(row);
            foreach (var item in HeadingText)
            {
                if (IndexCount <= 0)
                {
                    row.AppendChild(CreateTextCell(ColumnLetter(cellIdex++),
                       rowIdex, item, styleIndex: 6));
                }
                else
                {
                    row.AppendChild(CreateTextCell(ColumnLetter(cellIdex++),
                      rowIdex, item, styleIndex: 1));
                }
                IndexCount++;
            }
            row = new Row { RowIndex = ++rowIdex };
            //End Header

            row = new Row { RowIndex = ++rowIdex, Height = 25, CustomHeight = true };
            sheetData.AppendChild(row);
            cellIdex = 0;

            foreach (var header in data.Headers)
            {
                row.AppendChild(CreateTextCell(ColumnLetter(cellIdex++),
                    rowIdex, header ?? string.Empty, styleIndex: 2));
            }
            if (data.Headers.Count > 0)
            {
                // Add the column configuration if available
                if (data.ColumnConfigurations != null)
                {
                    var columns = (Columns)data.ColumnConfigurations.Clone();
                    worksheetPart.Worksheet
                        .InsertAfter(columns, worksheetPart
                        .Worksheet.SheetFormatProperties);
                }
            }

            // Add sheet data
            foreach (var rowData in data.DataRows)
            {
                cellIdex = 0;
                IndexCount = 0;
                uint ColorIndex = 0;
                row = new Row { RowIndex = ++rowIdex, Height = 15, CustomHeight = true };
                sheetData.AppendChild(row);
                foreach (var callData in rowData)
                {
                    if (IndexCount <= 1)
                        ColorIndex = 1;
                    else if (IndexCount >= 2 && IndexCount <= 3)
                        ColorIndex = 5;
                    else if (IndexCount >= 4 && IndexCount <= 5)
                        ColorIndex = 4;
                    else if (IndexCount >= 6 && IndexCount <= 7)
                        ColorIndex = 3;
                    else if (IndexCount > 7)
                        ColorIndex = 1;
                    var cell = CreateTextCell(ColumnLetter(cellIdex++),
                    rowIdex, callData ?? string.Empty, styleIndex: ColorIndex);
                    row.AppendChild(cell);

                    IndexCount++;
                }

            }


            // Implement Condition
            row = new Row { RowIndex = ++rowIdex };
            row = new Row { RowIndex = ++rowIdex, Height = 25, CustomHeight = true };
            sheetData.AppendChild(row);
            cellIdex = 0;
            foreach (var header in Implementdata.Headers)
            {
                row.AppendChild(CreateTextCell(ColumnLetter(cellIdex++),
                    rowIdex, header ?? string.Empty, styleIndex: 2));
            }
            if (Implementdata.Headers.Count > 0)
            {
                // Add the column configuration if available
                if (Implementdata.ColumnConfigurations != null)
                {
                    var columns = (Columns)data.ColumnConfigurations.Clone();
                    worksheetPart.Worksheet
                        .InsertAfter(columns, worksheetPart
                        .Worksheet.SheetFormatProperties);
                }
            }

            // Add sheet data
            foreach (var rowData in Implementdata.DataRows)
            {
                cellIdex = 0;
                IndexCount = 0;
                uint ColorIndex = 0;
                row = new Row { RowIndex = ++rowIdex, Height = 15, CustomHeight = true };
                sheetData.AppendChild(row);
                foreach (var callData in rowData)
                {
                    if (IndexCount <= 1)
                        ColorIndex = 1;
                    else if (IndexCount >= 2 && IndexCount <= 4)
                        ColorIndex = 5;
                    else if (IndexCount >= 5 && IndexCount <= 7)
                        ColorIndex = 4;
                    else if (IndexCount >= 8 && IndexCount <= 10)
                        ColorIndex = 3;
                    else if (IndexCount > 10)
                        ColorIndex = 1;
                    var cell = CreateTextCell(ColumnLetter(cellIdex++),
                        rowIdex, callData ?? string.Empty, styleIndex: ColorIndex);
                    row.AppendChild(cell);
                    IndexCount++;
                }

            }



            // Defer Condition
            row = new Row { RowIndex = ++rowIdex };
            row = new Row { RowIndex = ++rowIdex, Height = 25, CustomHeight = true };
            sheetData.AppendChild(row);
            cellIdex = 0;
            foreach (var header in Deferdata.Headers)
            {
                row.AppendChild(CreateTextCell(ColumnLetter(cellIdex++),
                    rowIdex, header ?? string.Empty, styleIndex: 2));
            }
            if (Deferdata.Headers.Count > 0)
            {
                // Add the column configuration if available
                if (Deferdata.ColumnConfigurations != null)
                {
                    var columns = (Columns)data.ColumnConfigurations.Clone();
                    worksheetPart.Worksheet
                        .InsertAfter(columns, worksheetPart
                        .Worksheet.SheetFormatProperties);
                }
            }

            // Add sheet data
            foreach (var rowData in Deferdata.DataRows)
            {
                cellIdex = 0;
                IndexCount = 0;
                uint ColorIndex = 0;
                row = new Row { RowIndex = ++rowIdex, Height = 15, CustomHeight = true };
                sheetData.AppendChild(row);
                foreach (var callData in rowData)
                {

                    if (IndexCount <= 1 || IndexCount > 4)
                        ColorIndex = 1;
                    else if (IndexCount >= 2 && IndexCount <= 4)
                        ColorIndex = 5;
                    var cell = CreateTextCell(ColumnLetter(cellIdex++),
                        rowIdex, callData ?? string.Empty, styleIndex: ColorIndex);
                    row.AppendChild(cell);
                    IndexCount++;
                }

            }


            // Implement SignOff and Approval in progress Condition
            row = new Row { RowIndex = ++rowIdex };
            row = new Row { RowIndex = ++rowIdex, Height = 25, CustomHeight = true };
            sheetData.AppendChild(row);
            cellIdex = 0;
            foreach (var header in SignOffApprovalconditionData.Headers)
            {
                row.AppendChild(CreateTextCell(ColumnLetter(cellIdex++),
                    rowIdex, header ?? string.Empty, styleIndex: 2));
            }
            if (SignOffApprovalconditionData.Headers.Count > 0)
            {
                // Add the column configuration if available
                if (SignOffApprovalconditionData.ColumnConfigurations != null)
                {
                    var columns = (Columns)data.ColumnConfigurations.Clone();
                    worksheetPart.Worksheet
                        .InsertAfter(columns, worksheetPart
                        .Worksheet.SheetFormatProperties);
                }
            }

            // Add sheet data
            foreach (var rowData in SignOffApprovalconditionData.DataRows)
            {
                cellIdex = 0;
                IndexCount = 0;
                uint ColorIndex = 0;
                row = new Row { RowIndex = ++rowIdex, Height = 15, CustomHeight = true };
                sheetData.AppendChild(row);
                foreach (var callData in rowData)
                {
                    if (IndexCount <= 1)
                        ColorIndex = 1;
                    else if (IndexCount >= 2 && IndexCount <= 4)
                        ColorIndex = 5;
                    else if (IndexCount >= 5 && IndexCount <= 6)
                        ColorIndex = 4;
                    else if (IndexCount >= 7 && IndexCount <= 8)
                        ColorIndex = 3;
                    else if (IndexCount > 8)
                        ColorIndex = 1;
                    var cell = CreateTextCell(ColumnLetter(cellIdex++),
                        rowIdex, callData ?? string.Empty, styleIndex: ColorIndex);
                    row.AppendChild(cell);
                    IndexCount++;
                }

            }

            // Assign Implementer Role Condition
            row = new Row { RowIndex = ++rowIdex };
            row = new Row { RowIndex = ++rowIdex, Height = 25, CustomHeight = true };
            sheetData.AppendChild(row);
            cellIdex = 0;
            foreach (var header in AgeingAssignImpleConditionData.Headers)
            {
                row.AppendChild(CreateTextCell(ColumnLetter(cellIdex++),
                    rowIdex, header ?? string.Empty, styleIndex: 2));
            }
            if (AgeingAssignImpleConditionData.Headers.Count > 0)
            {
                // Add the column configuration if available
                if (AgeingAssignImpleConditionData.ColumnConfigurations != null)
                {
                    var columns = (Columns)data.ColumnConfigurations.Clone();
                    worksheetPart.Worksheet
                        .InsertAfter(columns, worksheetPart
                        .Worksheet.SheetFormatProperties);
                }
            }

            // Add sheet data
            foreach (var rowData in AgeingAssignImpleConditionData.DataRows)
            {
                cellIdex = 0;
                IndexCount = 0;
                uint ColorIndex = 0;
                row = new Row { RowIndex = ++rowIdex, Height = 15, CustomHeight = true };
                sheetData.AppendChild(row);
                foreach (var callData in rowData)
                {
                    if (IndexCount <= 1 || IndexCount > 5)
                        ColorIndex = 1;
                    else if (IndexCount >= 2 && IndexCount <= 3)
                        ColorIndex = 5;
                    else if (IndexCount == 4)
                        ColorIndex = 4;
                    else if (IndexCount == 5)
                        ColorIndex = 3;

                    var cell = CreateTextCell(ColumnLetter(cellIdex++),
                        rowIdex, callData ?? string.Empty, styleIndex: ColorIndex);
                    row.AppendChild(cell);
                    IndexCount++;
                }

            }

            //Columns columns1 = workbookpart.Workbook.GetFirstChild<Columns>();
            ////other code here
            //Column column1 = new Column() { Min =1, Max = 1, Width = 500, CustomWidth = true };
            //columns1.Append(column1);
            //workbookpart.Workbook.Append(columns1);
            workbookpart.Workbook.Save();

            Columns lstColumns = worksheetPart.Worksheet.GetFirstChild<Columns>();
            Boolean needToInsertColumns = false;
            if (lstColumns == null)
            {
                lstColumns = new Columns();
                needToInsertColumns = true;
            }
            lstColumns.Append(new Column() { Min = 1, Max = 15, Width = 25, CustomWidth = true });

            if (needToInsertColumns)
                worksheetPart.Worksheet.InsertAt(lstColumns, 0);

            // Get the sheetData cells
            sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

        }

        public void BPOAddSheetToWorkbook(OpenXMLData data, OpenXMLData Implementdata = null, OpenXMLData SignOffApprovalconditionData = null, OpenXMLData AgeingAssignImpleConditionData = null, string reportPath = null)
        {
            WorksheetPart worksheetPart = null;
            SheetData sheetData = null;
            if (document == null)
            {
                document = SpreadsheetDocument.Create(reportPath, SpreadsheetDocumentType.Workbook);
                workbookpart = document.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();
                worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);
                WorkbookStylesPart stylePart = workbookpart.AddNewPart<WorkbookStylesPart>();
                stylePart.Stylesheet = GenerateStylesheet();
                stylePart.Stylesheet.Save();
                sheets = document.WorkbookPart.Workbook.
               AppendChild<Sheets>(new Sheets());
            }
            int Total = 0;
            foreach (var item in data.DataRows)
            {
                Total = Total + Convert.ToInt32(item[1]);
            }
            foreach (var item in Implementdata.DataRows)
            {
                Total = Total + Convert.ToInt32(item[1]);
            }

            foreach (var item in SignOffApprovalconditionData.DataRows)
            {
                Total = Total + Convert.ToInt32(item[1]);
            }
            foreach (var item in AgeingAssignImpleConditionData.DataRows)
            {
                Total = Total + Convert.ToInt32(item[1]);
            }
            worksheetPart = document.WorkbookPart.AddNewPart<WorksheetPart>();

            sheetData = new SheetData();
            worksheetPart.Worksheet = new Worksheet(sheetData);

            uint sheetId = 1;
            if (sheets.Elements<Sheet>().Count() > 0)
            {
                sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
            }

            var sheet = new Sheet()
            {
                Id = document.WorkbookPart
                .GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = data.SheetName ?? "ReportData Sheet " + sheetId
            };
            sheets.AppendChild(sheet);

            // Add header
            UInt32 rowIdex = 1;
            var row = new Row { RowIndex = ++rowIdex, Height = 30, CustomHeight = true };
            var cellIdex = 0;
            //Add Header Text

            sheetData.AppendChild(row);
            String[] HeadingName = new String[] { "Total Open Ideas:",Convert.ToString(Total), "Red Flagged Rules",
                @"Idea Validation, Assign Implementer,Implementation Sign off pending, Approval in progress",@"Implementation"," > 12 months from Target Implementation Date" };

            String[] HeadingText = new String[] { "(For quick reference)","> 90 days", ">12 months from Target Implementation Date",
                @"Red Flag" };

            int IndexCount = 0;
            foreach (var item in HeadingName)
            {
                if (IndexCount <= 1)
                {
                    row.AppendChild(CreateTextCell(ColumnLetter(cellIdex++),
                           rowIdex, item, styleIndex: 2));
                }
                else
                {
                    row.AppendChild(CreateTextCell(ColumnLetter(cellIdex++),
                                               rowIdex, item, styleIndex: 6));
                }
                IndexCount++;
            }

            //Add Heading Text
            IndexCount = 0;
            cellIdex = 2;
            row = new Row { RowIndex = ++rowIdex, Height = 15, CustomHeight = true };
            sheetData.AppendChild(row);
            foreach (var item in HeadingText)
            {
                if (IndexCount <= 0)
                {
                    row.AppendChild(CreateTextCell(ColumnLetter(cellIdex++),
                       rowIdex, item, styleIndex: 6));
                }
                else
                {
                    row.AppendChild(CreateTextCell(ColumnLetter(cellIdex++),
                      rowIdex, item, styleIndex: 1));
                }
                IndexCount++;
            }
            row = new Row { RowIndex = ++rowIdex };
            //End Header

            row = new Row { RowIndex = ++rowIdex, Height = 25, CustomHeight = true };
            sheetData.AppendChild(row);
            cellIdex = 0;

            foreach (var header in data.Headers)
            {
                row.AppendChild(CreateTextCell(ColumnLetter(cellIdex++),
                    rowIdex, header ?? string.Empty, styleIndex: 2));
            }
            if (data.Headers.Count > 0)
            {
                // Add the column configuration if available
                if (data.ColumnConfigurations != null)
                {
                    var columns = (Columns)data.ColumnConfigurations.Clone();
                    worksheetPart.Worksheet
                        .InsertAfter(columns, worksheetPart
                        .Worksheet.SheetFormatProperties);
                }
            }

            // Add sheet data
            foreach (var rowData in data.DataRows)
            {
                cellIdex = 0;
                IndexCount = 0;
                uint ColorIndex = 0;
                row = new Row { RowIndex = ++rowIdex, Height = 15, CustomHeight = true };
                sheetData.AppendChild(row);
                foreach (var callData in rowData)
                {
                    if (IndexCount <= 1)
                        ColorIndex = 1;
                    else if (IndexCount >= 2 && IndexCount <= 3)
                        ColorIndex = 5;
                    else if (IndexCount >= 4 && IndexCount <= 5)
                        ColorIndex = 4;
                    else if (IndexCount >= 6 && IndexCount <= 7)
                        ColorIndex = 3;
                    else if (IndexCount > 7)
                        ColorIndex = 1;
                    var cell = CreateTextCell(ColumnLetter(cellIdex++),
                    rowIdex, callData ?? string.Empty, styleIndex: ColorIndex);
                    row.AppendChild(cell);

                    IndexCount++;
                }

            }


            // Implement Condition
            row = new Row { RowIndex = ++rowIdex };
            row = new Row { RowIndex = ++rowIdex, Height = 25, CustomHeight = true };
            sheetData.AppendChild(row);
            cellIdex = 0;
            foreach (var header in Implementdata.Headers)
            {
                row.AppendChild(CreateTextCell(ColumnLetter(cellIdex++),
                    rowIdex, header ?? string.Empty, styleIndex: 2));
            }
            if (Implementdata.Headers.Count > 0)
            {
                // Add the column configuration if available
                if (Implementdata.ColumnConfigurations != null)
                {
                    var columns = (Columns)data.ColumnConfigurations.Clone();
                    worksheetPart.Worksheet
                        .InsertAfter(columns, worksheetPart
                        .Worksheet.SheetFormatProperties);
                }
            }

            // Add sheet data
            foreach (var rowData in Implementdata.DataRows)
            {
                cellIdex = 0;
                IndexCount = 0;
                uint ColorIndex = 0;
                row = new Row { RowIndex = ++rowIdex, Height = 15, CustomHeight = true };
                sheetData.AppendChild(row);
                foreach (var callData in rowData)
                {
                    if (IndexCount <= 1)
                        ColorIndex = 1;
                    else if (IndexCount == 2)
                        ColorIndex = 5;
                    else if (IndexCount == 3)
                        ColorIndex = 4;
                    else if (IndexCount == 4)
                        ColorIndex = 3;
                    else if (IndexCount > 4)
                        ColorIndex = 1;
                    var cell = CreateTextCell(ColumnLetter(cellIdex++),
                        rowIdex, callData ?? string.Empty, styleIndex: ColorIndex);
                    row.AppendChild(cell);
                    IndexCount++;
                }

            }


            // Implement SignOff and Approval in progress Condition
            row = new Row { RowIndex = ++rowIdex };
            row = new Row { RowIndex = ++rowIdex, Height = 25, CustomHeight = true };
            sheetData.AppendChild(row);
            cellIdex = 0;
            foreach (var header in SignOffApprovalconditionData.Headers)
            {
                row.AppendChild(CreateTextCell(ColumnLetter(cellIdex++),
                    rowIdex, header ?? string.Empty, styleIndex: 2));
            }
            if (SignOffApprovalconditionData.Headers.Count > 0)
            {
                // Add the column configuration if available
                if (SignOffApprovalconditionData.ColumnConfigurations != null)
                {
                    var columns = (Columns)data.ColumnConfigurations.Clone();
                    worksheetPart.Worksheet
                        .InsertAfter(columns, worksheetPart
                        .Worksheet.SheetFormatProperties);
                }
            }

            // Add sheet data
            foreach (var rowData in SignOffApprovalconditionData.DataRows)
            {
                cellIdex = 0;
                IndexCount = 0;
                uint ColorIndex = 0;
                row = new Row { RowIndex = ++rowIdex, Height = 15, CustomHeight = true };
                sheetData.AppendChild(row);
                foreach (var callData in rowData)
                {
                    if (IndexCount <= 1)
                        ColorIndex = 1;
                    else if (IndexCount >= 2 && IndexCount <= 5)
                        ColorIndex = 5;
                    else if (IndexCount >= 6 && IndexCount <= 7)
                        ColorIndex = 4;
                    else if (IndexCount >= 8 && IndexCount <= 9)
                        ColorIndex = 3;
                    else if (IndexCount > 9)
                        ColorIndex = 1;
                    var cell = CreateTextCell(ColumnLetter(cellIdex++),
                        rowIdex, callData ?? string.Empty, styleIndex: ColorIndex);
                    row.AppendChild(cell);
                    IndexCount++;
                }

            }

            // Assign Implementer Role Condition
            row = new Row { RowIndex = ++rowIdex };
            row = new Row { RowIndex = ++rowIdex, Height = 25, CustomHeight = true };
            sheetData.AppendChild(row);
            cellIdex = 0;
            foreach (var header in AgeingAssignImpleConditionData.Headers)
            {
                row.AppendChild(CreateTextCell(ColumnLetter(cellIdex++),
                    rowIdex, header ?? string.Empty, styleIndex: 2));
            }
            if (AgeingAssignImpleConditionData.Headers.Count > 0)
            {
                // Add the column configuration if available
                if (AgeingAssignImpleConditionData.ColumnConfigurations != null)
                {
                    var columns = (Columns)data.ColumnConfigurations.Clone();
                    worksheetPart.Worksheet
                        .InsertAfter(columns, worksheetPart
                        .Worksheet.SheetFormatProperties);
                }
            }

            // Add sheet data
            foreach (var rowData in AgeingAssignImpleConditionData.DataRows)
            {
                cellIdex = 0;
                IndexCount = 0;
                uint ColorIndex = 0;
                row = new Row { RowIndex = ++rowIdex, Height = 15, CustomHeight = true };
                sheetData.AppendChild(row);
                foreach (var callData in rowData)
                {
                    if (IndexCount <= 1 || IndexCount > 5)
                        ColorIndex = 1;
                    else if (IndexCount == 2)
                        ColorIndex = 5;
                    else if (IndexCount >= 3 && IndexCount <= 4)
                        ColorIndex = 4;
                    else if (IndexCount >= 5 && IndexCount <= 7)
                        ColorIndex = 3;

                    var cell = CreateTextCell(ColumnLetter(cellIdex++),
                        rowIdex, callData ?? string.Empty, styleIndex: ColorIndex);
                    row.AppendChild(cell);
                    IndexCount++;
                }

            }

            //Columns columns1 = workbookpart.Workbook.GetFirstChild<Columns>();
            ////other code here
            //Column column1 = new Column() { Min =1, Max = 1, Width = 500, CustomWidth = true };
            //columns1.Append(column1);
            //workbookpart.Workbook.Append(columns1);
            workbookpart.Workbook.Save();

            Columns lstColumns = worksheetPart.Worksheet.GetFirstChild<Columns>();
            Boolean needToInsertColumns = false;
            if (lstColumns == null)
            {
                lstColumns = new Columns();
                needToInsertColumns = true;
            }
            lstColumns.Append(new Column() { Min = 1, Max = 15, Width = 25, CustomWidth = true });

            if (needToInsertColumns)
                worksheetPart.Worksheet.InsertAt(lstColumns, 0);

            // Get the sheetData cells
            sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

        }

        public void CloseExcelWriter()
        {
            document.Close();
            document.Dispose();
        }

        private Stylesheet GenerateStylesheet()
        {
            Stylesheet styleSheet = null;

            Fonts fonts = new Fonts(
                new Font( // Index 0 - default
                    new FontSize() { Val = 10 }
                //  new Bold()


                ),
                new Font( // Index 1 - header
                    new FontSize() { Val = 12 },
                    new Bold(),
                    new Color() { Rgb = "FFFFFF" }

                ),
                 new Font( // Index 2 - default
                    new FontSize() { Val = 10 },
                     new Bold()


                ),
                  new Font( // Index 3 - default
                    new FontSize() { Val = 10 },
                     new Bold()


                ),
                   new Font( // Index 4 - default
                    new FontSize() { Val = 10 },
                    new Bold()


                ),
                    new Font( // Index 5 - default
                    new FontSize() { Val = 10 },
                    new Bold()


                ),
                     new Font( // Index 6 - default
                    new FontSize() { Val = 10 },
                     new Bold()



                )
                );


            Fills fills = new Fills(
                    new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 0 - default
                    new Fill(new PatternFill() { PatternType = PatternValues.Gray125 }), // Index 1 - default
                    new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "07788e" } })
                    { PatternType = PatternValues.Solid }), // Index 2 - header
                    new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "F02652" } })
                    { PatternType = PatternValues.Solid }), // Index 3 - Red
                        new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "E5F022" } })
                        { PatternType = PatternValues.Solid }), // Index 4 - Yellow
                          new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "05A608" } })
                          { PatternType = PatternValues.Solid }), // Index 5 - Green
                          new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "580DBA" } })
                          { PatternType = PatternValues.Solid })// Index 6 - Header Other Color

                );

            Borders borders = new Borders(
                    new Border(), // index 0 default
                    new Border( // index 1 black border
                        new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new DiagonalBorder())
                );

            CellFormats cellFormats = new CellFormats(
                    new CellFormat(), // default
                    new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Left, Vertical = VerticalAlignmentValues.Center }) { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true }, // body
                    new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Left, Vertical = VerticalAlignmentValues.Center }) { FontId = 1, FillId = 2, BorderId = 1, ApplyFill = true }, // header
                     new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }) { FontId = 3, FillId = 3, BorderId = 1, ApplyFill = true }, // Red
                       new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }) { FontId = 4, FillId = 4, BorderId = 1, ApplyFill = true }, // yellow
                         new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }) { FontId = 5, FillId = 5, BorderId = 1, ApplyFill = true }, // green
                           new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }) { FontId = 1, FillId = 6, BorderId = 1, ApplyFill = true }, // Other Header
                           new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Left, Vertical = VerticalAlignmentValues.Center, WrapText = true }) { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true } // body for reviewer details

                );

            styleSheet = new Stylesheet(fonts, fills, borders, cellFormats);

            return styleSheet;
        }
    }
}
