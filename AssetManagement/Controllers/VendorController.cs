using AssetManagement.Models;
using AssetManagement.Models.Dto;
using AssetManagement.Repository.IRepository;
using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

//using Kendo.Mvc.UI;

namespace AssetManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VendorController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Vendor> _vendorRepository;

        public VendorController(IMapper mapper, IRepository<Vendor> vendorRepository)
        {
            _mapper = mapper;
            _vendorRepository = vendorRepository;
        }

        // GET: VendorController
        public async Task<ActionResult> Index()
        {
            var list = await _vendorRepository.GetAll();
            //VendorViewDto vendorViewDto = new VendorViewDto();
            var vendorViewDto = _mapper.Map<List<VendorViewDto>>(list);

            //return Json(vendorViewDto);

            return View(vendorViewDto);
        }

        // GET: VendorController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var item = await _vendorRepository.Get(x => x.VendorId == id);
            VendorViewDto vendorViewDto = new VendorViewDto();
            vendorViewDto = _mapper.Map<VendorViewDto>(item);
            return View(vendorViewDto);
        }

        // GET: VendorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VendorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(VendorViewDto vendorViewDto)
        {
            try
            {
                Vendor vendor = new();
                vendor = _mapper.Map<Vendor>(vendorViewDto);
                await _vendorRepository.CreateAsync(vendor);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VendorController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var item = await _vendorRepository.Get(x => x.VendorId == id);
            VendorViewDto vendorViewDto = new VendorViewDto();
            vendorViewDto = _mapper.Map<VendorViewDto>(item);
            return View(vendorViewDto);
        }

        // POST: VendorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, VendorViewDto vendorViewDto)
        {
            try
            {
                Vendor vendor = new();
                vendor = _mapper.Map<Vendor>(vendorViewDto);
                await _vendorRepository.UpdateEntityAsync(vendor);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VendorController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var item = await _vendorRepository.Get(x => x.VendorId == id);
            VendorViewDto vendorViewDto = new VendorViewDto();
            vendorViewDto = _mapper.Map<VendorViewDto>(item);
            return View(vendorViewDto);
        }

        // POST: VendorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, VendorViewDto vendorViewDto)
        {
            try
            {
                var item = await _vendorRepository.Get(x => x.VendorId == id);

                if(item != null)
                {
                    await _vendorRepository.RemoveAsync(item);
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
