using AssignmentForMCСSA.Data.Models.Dto;
using AssignmentForMCСSA.Repositories.Abstract;
using AssignmentForMCСSA.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentForMCСSA.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProduct _product;
        private ILanguage _language;

        public ProductsController(IProduct product, ILanguage language)
        {
            _product = product;
            _language = language;
        }


        [HttpGet]
        public async Task<IActionResult> GetProducts(int currentPage)
        {
            var products = await _product.GetProducts();
            if (products == null)
            {
                ViewBag.Title = _language.GetKey("ProductsAreNull").Value;
                return View("Error", "Index");
            }

            var productsView = await _product.PaginetionProducts(products, currentPage, "GetProducts");
            return View(productsView);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _product.GetProduct(id);
            if (product == null)
            {
                return View("Error", "Home");
            }

            return View(product);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(ProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var status = await _product.CreateProduct(dto);
            if (status.StatusCode != ConstStatus.CODE_SUCCESSFUL)
            {
                return View(dto);
            }

            return RedirectToAction(nameof(GetProducts));
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _product.GetProduct(id);
            if (product == null)
            {
                return View();
            }

            var dto = await _product.GetProductDto(product);

            return View(dto);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(ProductDto dto)
        {
            var stauts = await _product.EditProduct(dto);
            if (stauts.StatusCode != ConstStatus.CODE_SUCCESSFUL)
            {
                return View(dto);
            }

            return RedirectToAction(nameof(GetProducts));
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _product.DeleteProduct(id);
            if (status.StatusCode != ConstStatus.CODE_SUCCESSFUL)
            {
                return View();
            }

            return RedirectToAction("");
        }

        [HttpGet]
        public async Task<IActionResult> Sorting(int currentPage, string critertion)
        {
            var sortList = await _product.SortingProductById(critertion);
            if (sortList == null)
            {
                return View("Error", "Home");
            }

            var products = await _product.PaginetionProducts(sortList, currentPage, "Sorting");
            
            ViewBag.Title = critertion;

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Filtering(int currentPage, string critertion)
        {
            var filtringList = await _product.FilteringProductsByCategory(critertion);
            if (filtringList == null)
            {
                return View("Error", "Home");
            }

            var products = await _product.PaginetionProducts(filtringList, currentPage, "Filtering");

            ViewBag.Title = critertion;

            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> Filtering(string critertion)
        {
            var filtringList = await _product.FilteringProductsByCategory(critertion);
            if (filtringList == null)
            {
                return View("Error", "Home");
            }

            var products = await _product.PaginetionProducts(filtringList, 1, "Filtering");

            ViewBag.Title = critertion;

            return View(products);
        }
    }
}
