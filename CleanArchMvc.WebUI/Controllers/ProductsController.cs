namespace CleanArchMvc.WebUI.Controllers;

[Authorize]
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
    public async Task<IActionResult> Index(int pg = 1)
    {
        var products = await _productService.GetProducts();

        const int pageSize = 10;
        if (pg < 1)
            pg = 1;

        int rescCount = products.Count();

        var pager = new Pager(rescCount, pg, pageSize);

        int resSkip = (pg - 1) * pageSize;

        var data = products.Skip(resSkip).Take(pager.PageSize).ToList();

        this.ViewBag.Pager = pager;

        return View(data);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        // Recurso para carregar o input SELECT do html na página Razor.
        ViewBag.CategoryId = new SelectList(await _categoryService.GetCategories(), "Id", "Name");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductDTO product, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            if(file != null)
                await UploadAndSaveImage(product, file);

            await _productService.Add(product);
            return RedirectToAction(nameof(Index));
        }
        else
        {
            ViewBag.CategoryId = new SelectList(await _categoryService.GetCategories(), "Id", "Name");
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
    public async Task<IActionResult> Edit(ProductDTO product, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            var oldImageName = !string.IsNullOrEmpty(product.Image) ? product.Image : string.Empty;

            // Somente muda a imagem se o campo de arquivo for alimentado, caso contrário mantem o que já vem do banco.
            if (file != null)
            {
                await UploadAndSaveImage(product, file);

                // excluir do servidor a imagem antiga.
                if (!string.IsNullOrEmpty(oldImageName))
                    await DeleteImage(oldImageName);
            }

            await _productService.Update(product);
            return RedirectToAction(nameof(Index));
        }
        else
        {
            ViewBag.CategoryId = new SelectList(await _categoryService.GetCategories(), "Id", "Name");
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
        await _productService.Remove(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var product = await _productService.GetById(id);

        if (product == null) return NotFound();

        if (!string.IsNullOrEmpty(product.Image))
        {
            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products", product.Image);
            ViewBag.ImageExist = System.IO.File.Exists(imagePath);
        }
        else
            ViewBag.ImageExist = false;

        return View(product);
    }

    public async Task UploadAndSaveImage(ProductDTO product, IFormFile file)
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

    public async Task DeleteImage(string fileName)
    {
        await Task.Run(() =>
        {
            string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products", fileName);
            FileInfo fi = new(imagePath);

            if (fi.Exists)
                fi.Delete();
        });
    }
}
