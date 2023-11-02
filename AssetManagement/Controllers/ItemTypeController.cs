using AssetManagement.Models;
using AssetManagement.Models.Dto;
using AssetManagement.Repository.IRepository;
using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Controllers
{
    public class ItemTypeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRepository<ItemTypes> _itemTypesRepo;

        public ItemTypeController(IMapper mapper, IRepository<ItemTypes> itemTypesRepo)
        {
            _mapper = mapper;
            _itemTypesRepo = itemTypesRepo;
        }
        // GET: ItemTypeController
        public async Task<ActionResult> Index()
        {
            var item = await _itemTypesRepo.GetAll();
            var itemTypeDto = _mapper.Map<List<ItemTypeViewDto>>(item);
            return View(itemTypeDto);
        }

        // GET: ItemTypeController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            ItemTypeViewDto itemTypeDto = new ItemTypeViewDto();
            var item = await _itemTypesRepo.Get(x => x.ItemTypeId == id);
            itemTypeDto = _mapper.Map<ItemTypeViewDto>(item);
            return View(itemTypeDto);
        }

        // GET: ItemTypeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ItemTypeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ItemTypeViewDto itemTypeViewDto)
        {
            try
            {
                var itemType = _mapper.Map<ItemTypes>(itemTypeViewDto);
                await _itemTypesRepo.CreateAsync(itemType);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(itemTypeViewDto);
            }
        }

        // GET: ItemTypeController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            ItemTypeViewDto itemTypeDto = new ItemTypeViewDto();
            var item = await _itemTypesRepo.Get(x => x.ItemTypeId == id);
            itemTypeDto = _mapper.Map<ItemTypeViewDto>(item);
            return View(itemTypeDto);
        }

        // POST: ItemTypeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ItemTypeViewDto itemTypeViewDto)
        {
            try
            {
                ItemTypes itemType = new ItemTypes();
                itemType = _mapper.Map<ItemTypes>(itemTypeViewDto);
                
                if(itemType!= null)
                {
                    await _itemTypesRepo.UpdateEntityAsync(itemType);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(itemTypeViewDto);
            }
        }

        // GET: ItemTypeController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            ItemTypeViewDto itemTypeDto = new ItemTypeViewDto();
            var item = await _itemTypesRepo.Get(x => x.ItemTypeId == id);
            itemTypeDto = _mapper.Map<ItemTypeViewDto>(item);
            return View(itemTypeDto);
        }

        // POST: ItemTypeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                ItemTypes itemType = new ItemTypes();
                var item = await _itemTypesRepo.Get(x => x.ItemTypeId == id);
                if(item != null)
                {
                    await _itemTypesRepo.RemoveAsync(item);
                }
                else
                {
                    return NotFound();
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
