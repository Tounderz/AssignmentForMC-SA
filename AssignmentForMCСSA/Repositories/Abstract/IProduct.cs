using AssignmentForMCСSA.Data.Models.Dto;
using AssignmentForMCСSA.Data.Models.View;
using AssignmentForMCСSA.Data.Models;

namespace AssignmentForMCСSA.Repositories.Abstract
{
    public interface IProduct
    {
        Task<List<ProductModel>> GetProducts();
        Task<ProductModel> GetProduct(int id);
        Task<ProductDto> GetProductDto(ProductModel product);
        Task<StatusModel> CreateProduct(ProductDto dto);
        Task<StatusModel> EditProduct(ProductDto dto);
        Task<StatusModel> DeleteProduct(int id);
        Task<List<ProductModel>> SortingProductById(string critertion);
        Task<List<ProductModel>> FilteringProductsByCategory(string critertion);
        Task<ProductView> PaginetionProducts(List<ProductModel> products, int page, string actionName);
    }
}
