using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CleanArchMvc.WebUI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(IProductService productService, ICategoryService categoryService, IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _categoryService = categoryService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProducts();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Recurso para carregar o input SELECT do html na página Razor.
            ViewBag.CategoryId = new SelectList(await _categoryService.GetCategories(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDTO product, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                await LoadAndSaveImage(product, file);
                await _productService.Add(product);
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _productService.GetById(id);

            if (product == null) return NotFound();

            var categories = await _categoryService.GetCategories();

            ViewBag.CategoryId = new SelectList(categories, "Id", "Name", product.CategoryId);

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductDTO product, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                await LoadAndSaveImage(product, file);
                await _productService.Update(product);
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _productService.GetById(id);

            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost(), ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _productService.Remove(id);
            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _productService.GetById(id);

            if (product == null) return NotFound();

            if(!string.IsNullOrEmpty(product.Image))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products", product.Image);
                ViewBag.ImageExist = System.IO.File.Exists(imagePath);
            }
            else
                ViewBag.ImageExist = false;

            return View(product);
        }

        public async Task LoadAndSaveImage(ProductDTO product, IFormFile file)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            if (file == null)
                return;

            product.Image = $"{Guid.NewGuid()}__{Path.GetFileName(file.FileName)}";
            string pathToSave = Path.Combine(uploadsFolder, product.Image);

            using (FileStream fs = new FileStream(pathToSave, FileMode.Create))
                await file.CopyToAsync(fs);
        }
    }
}
