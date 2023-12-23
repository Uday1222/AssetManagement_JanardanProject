using AssetManagement.Models;
using AssetManagement.Models.Dto;
using AssetManagement.Repository.IRepository;
using AutoMapper;
using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AssetController : Controller
    {
        private readonly IRepository<Asset> _assetRepo;
        private readonly IMapper _mapper;

        public AssetController(IRepository<Asset> assetRepo, IMapper mapper)
        {
            _assetRepo = assetRepo;
            _mapper = mapper;
        }
        // GET: AssetController

        
        public async Task<ActionResult> Index()
        {
            var entity = await _assetRepo.GetAll();
            List<AssetViewDto> assetViewDto = new List<AssetViewDto>();
            assetViewDto = _mapper.Map<List<AssetViewDto>>(entity);
            return View(assetViewDto);
        }

        // GET: AssetController/Details/5
        public async Task<ActionResult> Details(int id)
        {

            var entity = await _assetRepo.Get(x => x.AssetId == id);
            AssetViewDto assetViewDto = new AssetViewDto();
            assetViewDto = _mapper.Map<AssetViewDto>(entity);
            return View(assetViewDto);
        }

        // GET: AssetController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AssetController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AssetViewDto entity)
        {
            try
            {
                //entity.Status = "Available";
                Asset asset = new Asset();
                asset = _mapper.Map<Asset>(entity);
                await _assetRepo.CreateAsync(asset);
                TempData["success"] = "Asset Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AssetController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var entity = await _assetRepo.Get(x => x.AssetId == id);
            AssetViewDto assetViewDto = new AssetViewDto();
            assetViewDto = _mapper.Map<AssetViewDto>(entity);
            return View(assetViewDto);
        }

        // POST: AssetController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, AssetViewDto entity)
        {
            try
            {
                Asset asset = new Asset();
                asset = _mapper.Map<Asset>(entity);
                await _assetRepo.UpdateEntityAsync(asset);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AssetController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var entity = await _assetRepo.Get(x => x.AssetId == id);
            AssetViewDto assetViewDto = new AssetViewDto();
            assetViewDto = _mapper.Map<AssetViewDto>(assetViewDto);
            return View(assetViewDto);
        }

        // POST: AssetController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                var entitity = await _assetRepo.Get(x => x.AssetId == id);
                await _assetRepo.RemoveAsync(entitity);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
