using AssignmentForMCСSA.Data.Models.Dto;
using AssignmentForMCСSA.Data.Models.View;
using AssignmentForMCСSA.Data.Models;
using AssignmentForMCСSA.Utils;
using AssignmentForMCСSA.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using AssignmentForMCСSA.Data.Db;

namespace AssignmentForMCСSA.Repositories.Implementation
{
    public class ProductRepository : IProduct
    {
        private readonly AppDbContext _context;
        private readonly ISaveImg _saveImg;
        private readonly ILanguage _language;

        public ProductRepository(AppDbContext context, ISaveImg saveImg, ILanguage language)
        {
            _context = context;
            _saveImg = saveImg;
            _language = language;
        }

        public async Task<List<ProductModel>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            if (products == null || products.Count <= 0)
            {
                return null;
            }

            return products;
        }

        public async Task<ProductModel> GetProduct(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(i => i.Id == id);
            if (product == null)
            {
                return null;
            }

            return product;
        }

        public async Task<ProductDto> GetProductDto(ProductModel product)
        {
            var dto = new ProductDto()
            {
                Id = product.Id,
                Title = product.Title,
                Category = product.Category,
                Description = product.Description,
                Image = null
            };

            return dto;
        }

        public async Task<StatusModel> CreateProduct(ProductDto dto)
        {
            var status = new StatusModel();
            var check = await CheckTitle(dto.Title, 0);
            if (!check)
            {
                status.StatusCode = ConstStatus.CODE_UNSUCCESSFUL;
                status.StatusMessage = _language.GetKey("ProductByThatNameAlreadyExists").Value;
                return status;
            }

            var product = new ProductModel()
            {
                Title = !string.IsNullOrEmpty(dto.Title) ? dto.Title : string.Empty,
                Price = dto.Price > 0 ? dto.Price : 0,
                Category = !string.IsNullOrEmpty(dto.Category) ? dto.Category : string.Empty,
                Description = !string.IsNullOrEmpty(dto.Description) ? dto.Description : string.Empty,
                Image = dto.Image != null ? await _saveImg.SaveImg(dto.Image) : string.Empty
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            status.StatusCode = ConstStatus.CODE_SUCCESSFUL;
            status.StatusMessage = _language.GetKey("ProductAddedSuccessfully").Value;
            return status;
        }

        public async Task<StatusModel> EditProduct(ProductDto dto)
        {
            var status = new StatusModel();
            var check = await CheckTitle(dto.Title, dto.Id);
            if (!check)
            {
                status.StatusCode = ConstStatus.CODE_UNSUCCESSFUL;
                status.StatusMessage = _language.GetKey("ProductByThatNameAlreadyExists").Value;
                return status;
            }

            var product = await GetProduct(dto.Id);
            if (product == null)
            {
                status.StatusCode = ConstStatus.CODE_UNSUCCESSFUL;
                status.StatusMessage = _language.GetKey("NotProduct").Value;
                return status;
            }

            product.Title = !string.IsNullOrEmpty(dto.Title) ? dto.Title : product.Title;
            product.Price = dto.Price > 0 ? dto.Price : product.Price;
            product.Category = !string.IsNullOrEmpty(dto.Category) ? dto.Category : product.Category;
            product.Description = !string.IsNullOrEmpty(dto.Description) ? dto.Description : product.Description;
            product.Image = dto.Image != null ? await _saveImg.SaveImg(dto.Image) : product.Image;


            _context.Update(product);
            await _context.SaveChangesAsync();

            status.StatusCode = ConstStatus.CODE_SUCCESSFUL;
            status.StatusMessage = _language.GetKey("ProductSuccessfullyUpdated").Value;
            return status;
        }

        public async Task<StatusModel> DeleteProduct(int id)
        {
            var status = new StatusModel();
            var product = await GetProduct(id);
            if (product == null)
            {
                status.StatusCode = ConstStatus.CODE_UNSUCCESSFUL;
                status.StatusMessage = _language.GetKey("NotProduct").Value;
                return status;
            }

            status.StatusCode = ConstStatus.CODE_SUCCESSFUL;
            status.StatusMessage = _language.GetKey("ProductSuccessfullyDeleted").Value;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return status;
        }

        public async Task<List<ProductModel>> SortingProductById(string critertion)
        {
            var products = await GetProducts();
            if (products == null)
            {
                return null;
            }

            switch (critertion)
            {
                case "Down":
                    products = products.OrderByDescending(i => i.Id).ToList();
                    break;
                case "Up":
                    products = products.OrderBy(i => i.Id).ToList();
                    break;
                default:
                    break;
            }

            return products;
        }
        public async Task<List<ProductModel>> FilteringProductsByCategory(string critertion)
        {
            var products = await _context.Products.Where(i => i.Category.Contains(critertion)).ToListAsync();
            if (products.Count <= 0 || products == null)
            {
                return null;
            }

            return products;
        }

        public async Task<ProductView> PaginetionProducts(List<ProductModel> products, int page, string actionName)
        {
            decimal count = products.Count;
            var countPages = Convert.ToInt32(Math.Ceiling(count / 5));
            int start = 5 * (page - 1);
            products = products.Skip(start).Take(5).ToList();
            var pagination = new PaginationView()
            {
                CountPage = countPages,
                PageIndex = page <= 0 ? 1 : page,
                ControllerName = "Products",
                ActionName = actionName
            };

            var productsViewModel = new ProductView()
            {
                Products = products,
                Pagination = pagination
            };

            return productsViewModel;
        }

        private async Task<bool> CheckTitle(string title, int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(i => i.Title.ToLower() == title.ToLower());
            if (product == null || product.Id == id)
            {
                return true;
            }

            return false;
        }
    }
}
