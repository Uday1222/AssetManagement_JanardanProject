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
        private readonly IRepository<AssetDetails> _assetDetailsRepo;

        public ReportsController(IRepository<Asset> assetRepo, IRepository<AssetDetails> assetDetailsRepo)
        {
            _assetRepo = assetRepo;
            _assetDetailsRepo = assetDetailsRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<FileResult> AssetsAvailable()
        {
            var assets = await _assetRepo.GetAll(); // Retrieve employees from your database

            var availableAssets = assets.Where(x => x.Status == "Available" || x.Status == "Returned").ToList();

            return ExportToExcel(availableAssets, "availableAssets", "availableAssets.xlsx");
        }

        public async Task<FileResult> PurchasedReport()
        {
            var entity = await _assetRepo.GetAll();
            return ExportToExcel(entity, "PurchasedReport", "PurchasedReport.xlsx");
        }

        public async Task<FileResult> GivenToEmployeeReport()
        {
            var assets = await _assetRepo.GetAll();
            var statusOfAssets = await _assetDetailsRepo.GetAll();

            var result = (from asset in assets
                         join details in statusOfAssets
                         on asset.SerialNo equals details.SerialNo
                         select new
                         {
                             GivenDate = details.GivenDate,
                             ItemType = asset.ItemType,
                             Model = asset.Model,
                             SerialNo = asset.SerialNo,
                             EmpId = details.EmpId,
                             EmpName = details.EmpName,
                             Stats = details.Status
                         }).ToList();

            return ExportToExcel(result, "GivenToEmployeeReport", "GivenToEmployeeReport.xlsx");
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
