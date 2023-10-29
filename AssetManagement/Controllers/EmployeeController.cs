using AssetManagement.Models;
using AssetManagement.Models.Dto;
using AssetManagement.Repository.IRepository;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace AssetManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _empRepo;

        public EmployeeController(IEmployeeRepository empRepo)
        {
            _empRepo = empRepo;
        }
        // GET: EmployeeController
        public async Task<ActionResult> Index()
        {
            var list = await _empRepo.GetAll();
            return View(list);
        }

        // GET: EmployeeController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var entitity = await _empRepo.Get(x => x.EmpId == id);
            return View(entitity);
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Employee entity)
        {
            try
            {
                await _empRepo.CreateAsync(entity);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var entitity = await _empRepo.Get(x => x.EmpId == id);
            return View(entitity);
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Employee entity)
        {
            try
            {
                await _empRepo.Update(entity);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var entitity = await _empRepo.Get(x => x.EmpId == id);
            return View(entitity);
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                var entitity = await _empRepo.Get(x => x.EmpId == id);
                if (entitity != null)
                {
                    await _empRepo.RemoveAsync(entitity);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<FileResult> ExportToExcel()
        {
            var employees = await _empRepo.GetAll();// Retrieve employees from your database

            // Create Excel package
            return ExportToExcel(employees, "Employees", "Employees.xlsx");

        }

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
