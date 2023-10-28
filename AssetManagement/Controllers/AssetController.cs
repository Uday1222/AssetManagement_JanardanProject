using AssetManagement.Models;
using AssetManagement.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Controllers
{
    public class AssetController : Controller
    {
        private readonly IRepository<Asset> _assetRepo;

        public AssetController(IRepository<Asset> assetRepo)
        {
            _assetRepo = assetRepo;
        }
        // GET: AssetController
        public async Task<ActionResult> Index()
        {
            var entity = await _assetRepo.GetAll();
            return View(entity);
        }

        // GET: AssetController/Details/5
        public async Task<ActionResult> Details(int id)
        {

            var entitity = await _assetRepo.Get(x => x.AssetId == id);
            return View(entitity);
        }

        // GET: AssetController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AssetController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Asset entity)
        {
            try
            {
                entity.Status = "Available";
                await _assetRepo.CreateAsync(entity);
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
            var entitity = await _assetRepo.Get(x => x.AssetId == id);
            return View(entitity);
        }

        // POST: AssetController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Asset entity)
        {
            try
            {
                await _assetRepo.UpdateEntityAsync(entity);
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
            var entitity = await _assetRepo.Get(x => x.AssetId == id);
            return View(entitity);
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
