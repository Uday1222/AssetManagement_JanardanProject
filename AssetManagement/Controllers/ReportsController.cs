using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using Microsoft.AspNetCore.Mvc;
using AssetManagement.Repository.IRepository;
using AssetManagement.Models;

namespace AssetManagement.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IRepository<Asset> _assetRepo;

        public ReportsController(IRepository<Asset> assetRepo)
        {
            _assetRepo = assetRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<FileResult> AssetsAvailable()
        {
            var assets = await _assetRepo.GetAll(); // Retrieve employees from your database

            var availableAssets = assets.Where(x => x.Status == "Available").ToList();

            // Create Excel package
            return ExportToExcel(availableAssets, "availableAssets", "availableAssets.xlsx");
        }

        //public async Task<FileResult> ExportToExcel()
        //{
        //    var employees = await _empRepo.GetAll();// Retrieve employees from your database

        //    // Create Excel package
        //    return ExportToExcel(employees, "Employees", "Employees.xlsx");

        //}

        public FileResult ExportToExcel<T>(List<T> data, string sheetName, string fileName)
        {
            // Create a memory stream to store the Excel file
            MemoryStream memoryStream = new MemoryStream();

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
            {
                // Add a WorkbookPart to the document
                WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                // Add a WorksheetPart to the WorkbookPart
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                // Create a Sheets object and append the new worksheet
                Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
                Sheet sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = sheetName };
                sheets.Append(sheet);

                // Get the sheet data
                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                // Use reflection to get properties of the generic type
                var properties = typeof(T).GetProperties();

                // Add headers to Excel sheet
                Row headerRow = new Row();
                foreach (var property in properties)
                {
                    headerRow.AppendChild(CreateCell(property.Name));
                }
                sheetData.AppendChild(headerRow);

                // Add data to Excel sheet
                foreach (var item in data)
                {
                    Row dataRow = new Row();
                    foreach (var property in properties)
                    {
                        var value = property.GetValue(item, null);
                        dataRow.AppendChild(CreateCell(value?.ToString() ?? ""));
                    }
                    sheetData.AppendChild(dataRow);
                }
            }

            // Save the memory stream to a byte array
            byte[] byteArray = memoryStream.ToArray();
            memoryStream.Close();

            // Return the Excel file as a FileResult
            return File(byteArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        private Cell CreateCell(string text)
        {
            return new Cell()
            {
                CellValue = new CellValue(text),
                DataType = new EnumValue<CellValues>(CellValues.String)
            };
        }
    }
}
