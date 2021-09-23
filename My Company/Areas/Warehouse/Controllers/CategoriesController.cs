using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.DIctionaries;
using My_Company.EnumTypes;
using My_Company.Helpers;
using My_Company.Interfaces;
using My_Company.Models;
using Newtonsoft.Json;
using Attribute = My_Company.Models.Attribute;

namespace My_Company.Areas.Warehouse.Controllers
{
    [Area("Warehouse")]
    [Authorize(Roles = Constants.Roles.MainAdministrator)]
    public class CategoriesController : Controller
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public CategoriesController(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        // GET: Warehouse/Categories
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var category = await _repositoryWrapper.CategoriesRepository.GetCategoryWithAttributes(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            var categoryViewModel = _mapper.Map<CategoryDetailsViewModel>(category);
            categoryViewModel.CategoryTree = await _repositoryWrapper.CategoriesRepository.GetCategoryTree(category);

            return View(categoryViewModel);
        }

        // GET: Warehouse/Categories/Create
        public async Task<IActionResult> Create()
        {
            ViewData["ParentCategoryId"] = new SelectList(await _repositoryWrapper.CategoriesRepository.GetCategoriesTree(), "Id", "Tree");
            ViewData["Attributes"] = new SelectList(CategoryAttributeTypesDictionary.AttributeDictionary, "Key", "Value");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CreateCategoryViewModel createCategoryDto)
        {
            if (createCategoryDto == null)
                return BadRequest();

            var categoryDb = _mapper.Map<Category>(createCategoryDto);

            _repositoryWrapper.CategoriesRepository.Create(categoryDb);

            await _repositoryWrapper.Save();

            TempData["attributes"] = JsonConvert.SerializeObject(categoryDb.Attributes.Where(attr => attr.Type == AttributeType.Dictionary).ToList(),
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

            return RedirectToAction(nameof(CreateAttributesValues));
        }

        [HttpGet]
        public IActionResult GetInheritedAttributes(int? id)
        {
            if (id == null)
                return BadRequest();

            return ViewComponent("InheritedAttributesTable", new { parentCategoryId = id.Value });
        }

        /*validate category name*/
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> CheckName(CreateCategoryViewModel createCategoryDto)
        {
            var exists = await _repositoryWrapper.CategoriesRepository.CheckName(createCategoryDto.CategoryName);

            if (!exists)
                return Json(true);
            else
                return Json("Nazwa zajęta");
        }

        [HttpGet]
        public IActionResult CreateAttributesValues()
        {
            if (TempData["attributes"] == null)
                return BadRequest();

            var attributes = JsonConvert.DeserializeObject<List<Attribute>>(TempData["attributes"] as string);

            if (attributes.Count() == 0)
                return RedirectToAction(nameof(Index));

            var attributeValuesDtos = _mapper.Map<List<AttributeValuesViewModel>>(attributes);

            return View(attributeValuesDtos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAttributesValues(List<AttributeValuesViewModel> attributes)
        {
            if (attributes == null)
                return BadRequest();

            foreach (var attr in attributes)
            {
                await _repositoryWrapper.CategoryAttributesRepository.AddAttributeValue(attr.AttributeId, attr.Values);
            }

            await _repositoryWrapper.Save();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult GetList(CategoryListFilters filters)
        {
            if (filters == null)
                return BadRequest();

            return ViewComponent("CategoriesList", filters);
        }

        //// GET: Warehouse/Categories/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var category = await _context.Categories.FindAsync(id);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "Id", "Id", category.ParentCategoryId);
        //    return View(category);
        //}

        //// POST: Warehouse/Categories/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryName,Descripttion,ParentCategoryId")] Category category)
        //{
        //    if (id != category.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(category);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CategoryExists(category.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "Id", "Id", category.ParentCategoryId);
        //    return View(category);
        //}

        [HttpGet]
        public async Task<IActionResult> EditValues(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var attribute = await _repositoryWrapper.CategoryAttributesRepository.GetAttributeWithCategoryAndValuesById(id.Value);
            if (attribute == null)
            {
                return NotFound();
            }

            ViewData["CategoryName"] = attribute.Category.CategoryName;
            ViewData["AttributeName"] = attribute.Name;
            ViewData["AttributeId"] = attribute.Id;

            List<string> values = attribute.AttributeDictionaryValues.Select(a => a.Value).ToList();

            return View(values);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditValues(int? id, List<string> values)
        {
            Attribute attribute = null;
            if(ModelState.IsValid)
            {
                if (id == null || values == null)
                {
                    return BadRequest();
                }

                attribute = await _repositoryWrapper.CategoryAttributesRepository.GetAttributeWithCategoryAndValuesTrackedById(id.Value);

                attribute.AttributeDictionaryValues = _mapper.Map<List<AttributeDictionaryValues>>(values);

                _repositoryWrapper.CategoryAttributesRepository.Update(attribute);

                await _repositoryWrapper.Save();

                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryName"] = attribute.Category.CategoryName;
            ViewData["AttributeName"] = attribute.Name;
            ViewData["AttributeId"] = attribute.Id;
            return View(values);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] int? id)
        {
            if (id == null)
                return BadRequest();

            try
            {
                var category = await _repositoryWrapper.CategoriesRepository.GetById(id.Value);

                if (category == null)
                    return NotFound();

                _repositoryWrapper.CategoriesRepository.Delete(category);

                await _repositoryWrapper.Save();
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
