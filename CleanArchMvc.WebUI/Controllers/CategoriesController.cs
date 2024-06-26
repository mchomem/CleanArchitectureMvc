﻿using CleanArchMvc.Domain.Entities;

namespace CleanArchMvc.WebUI.Controllers;

[Authorize]
public class CategoriesController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
        => _categoryService = categoryService;

    [HttpGet]
    public async Task<IActionResult> Index(int pg = 1)
    {
        var categories = await _categoryService.GetCategories();

        const int pageSize = 10;
        if (pg < 1)
            pg = 1;

        int rescCount = categories.Count();

        var pager = new Pager(rescCount, pg, pageSize);
        int resSkip = (pg - 1) * pageSize;

        var data = categories.Skip(resSkip).Take(pager.PageSize).ToList();

        this.ViewBag.Pager = pager;

        return View(data);
    }

    [HttpGet] // Método para requisitar o formulário
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryDTO category)
    {
        if (ModelState.IsValid)
        {
            await _categoryService.Add(category);
            return RedirectToAction(nameof(Index));
        }

        return View(category);
    }

    [HttpGet] // Isso faz carregar (consulta) a página de edição contendo o registro para edição.
    public async Task<IActionResult> Edit(int? id)
    {
        return await GetCategory(id);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CategoryDTO category)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _categoryService.Update(category);
            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(category);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        return await GetCategory(id);
    }

    [HttpPost(), ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _categoryService.Remove(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        return await GetCategory(id);
    }

    public async Task<IActionResult> GetCategory(int? id)
    {
        if (id == null) return NotFound();

        var category = await _categoryService.GetById(id);

        if (category == null) return NotFound();

        return View(category);
    }
}
