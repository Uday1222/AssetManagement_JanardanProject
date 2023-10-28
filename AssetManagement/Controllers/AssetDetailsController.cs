using AssetManagement.Models;
using AssetManagement.Models.Dto;
using AssetManagement.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AssetManagement.Controllers
{
    public class AssetDetailsController : Controller
    {
        private readonly IRepository<AssetDetails> _repository;
        private readonly IRepository<Asset> _assetRepo;

        public AssetDetailsController(IRepository<AssetDetails> repository, IRepository<Asset> assetRepo)
        {
            _repository = repository;
            _assetRepo = assetRepo;
        }
        // GET: AssetDetailsController
        public async Task<ActionResult> Index()
        {
            var list = await _repository.GetAll();
            return View(list);
        }

        // GET: AssetDetailsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AssetDetailsController/Create
        public ActionResult Create()
        {
            AssetDetailsViewDto assetDetailsView = new AssetDetailsViewDto();
            var statusOptions = new List<SelectListItem>
        {
            //new SelectListItem { Text = "Available", Value = "Available" },
            new SelectListItem { Text = "Received", Value = "Received" },
            new SelectListItem { Text = "Return", Value = "Return" }
        };
            assetDetailsView.Status = statusOptions;
            return View(assetDetailsView);
        }

        // POST: AssetDetailsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AssetDetailsViewDto entity)
        {
            try
            {
                var item = await _assetRepo.Get(x => x.SerialNo == entity.SerialNo);

                if (item != null)
                {
                    AssetDetails assetDetails = new AssetDetails();
                    assetDetails.SerialNo = entity.SerialNo;
                    assetDetails.Status = entity.SelectedStatus;
                    assetDetails.AdditionalDetails = entity.AdditionalDetails;
                    assetDetails.EmpId = entity.EmpId;
                    assetDetails.EmpName = entity.EmpName;
                    assetDetails.GivenDate = entity.GivenDate;
                    assetDetails.Comments = entity.Comments;

                    await _repository.CreateAsync(assetDetails);

                    item.Status = entity.SelectedStatus;
                    await _assetRepo.UpdateEntityAsync(item);
                }
                else
                {
                    ModelState.AddModelError("SerialNo", "Asset is not available");
                    return View(new AssetDetailsViewDto());
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AssetDetailsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AssetDetailsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AssetDetailsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AssetDetailsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
