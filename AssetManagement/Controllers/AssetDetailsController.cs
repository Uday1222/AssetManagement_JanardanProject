using AssetManagement.Migrations;
using AssetManagement.Models;
using AssetManagement.Models.Dto;
using AssetManagement.Repository.IRepository;
using AutoMapper;
using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AssetManagement.Controllers
{
    public class AssetDetailsController : Controller
    {
        private readonly IRepository<AssetDetails> _repository;
        private readonly IRepository<Asset> _assetRepo;
        private readonly IMapper _mapper;

        public AssetDetailsController(IRepository<AssetDetails> repository, IRepository<Asset> assetRepo, IMapper mapper)
        {
            _repository = repository;
            _assetRepo = assetRepo;
            _mapper = mapper;
        }
        // GET: AssetDetailsController
        public async Task<ActionResult> Index()
        {
            var list = await _repository.GetAll();
            return View(list);
        }

        // GET: AssetDetailsController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var item = await _repository.Get(x => x.AssetDetailsId == id);

            return View(item);
        }

        // GET: AssetDetailsController/Create
        public ActionResult Create()
        {
            AssetDetailsViewDto assetDetailsView = new AssetDetailsViewDto();
            var statusOptions = new List<SelectListItem>
        {
            //new SelectListItem { Text = "Available", Value = "Available" },
            new SelectListItem { Text = "Received", Value = "Received" },
            new SelectListItem { Text = "Returned", Value = "Returned" }
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
                //var item = await _assetRepo.Get(x => x.SerialNo == entity.SerialNo);

                //if (item != null)
                //{
                    AssetDetails assetDetails = new AssetDetails();

                    assetDetails = _mapper.Map<AssetDetails>(entity);

                    assetDetails.Status = entity.SelectedStatus;

                    await _repository.CreateAsync(assetDetails);

                    //item.Status = entity.SelectedStatus;
                    //await _assetRepo.UpdateEntityAsync(item);
                //}
                //else
                //{
                //    ModelState.AddModelError("SerialNo", "Asset is not available");
                //    return View(new AssetDetailsViewDto());
                //}
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AssetDetailsController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            AssetDetailsViewDto assetDetailsView = new AssetDetailsViewDto();
            var statusOptions = new List<SelectListItem>
        {
            //new SelectListItem { Text = "Available", Value = "Available" },
            new SelectListItem { Text = "Received", Value = "Received" },
            new SelectListItem { Text = "Returned", Value = "Returned" }
        };
            assetDetailsView.Status = statusOptions;

            var item = await _repository.Get(x => x.AssetDetailsId == id);
            assetDetailsView.SerialNo = item.SerialNo;
            assetDetailsView.AdditionalDetails = item.AdditionalDetails;
            assetDetailsView.Comments = item.Comments;
            assetDetailsView.GivenDate = item.GivenDate;
            assetDetailsView.EmpId = item.EmpId;
            assetDetailsView.EmpName = item.EmpName;
            assetDetailsView.AssetDetailsId = item.AssetDetailsId;

            return View(assetDetailsView);
        }

        // POST: AssetDetailsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, AssetDetailsViewDto entity)
        {
            try
            {
                AssetDetails assetDetails = new AssetDetails();
                assetDetails.AssetDetailsId = id;
                assetDetails.SerialNo = entity.SerialNo;
                assetDetails.Status = entity.SelectedStatus;
                assetDetails.AdditionalDetails = entity.AdditionalDetails;
                assetDetails.EmpId = entity.EmpId;
                assetDetails.EmpName = entity.EmpName;
                assetDetails.GivenDate = entity.GivenDate;
                assetDetails.Comments = entity.Comments;

                await _repository.UpdateEntityAsync(assetDetails);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(entity);
            }
        }

        // GET: AssetDetailsController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var item = await _repository.Get(x => x.AssetDetailsId == id);

            return View(item);
        }

        // POST: AssetDetailsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                var item = await _repository.Get(x => x.AssetDetailsId == id);

                if(item != null)
                {
                   await _repository.RemoveAsync(item);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
