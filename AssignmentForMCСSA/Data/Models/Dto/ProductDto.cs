using AssignmentForMCСSA.Repositories.Implementation;
using System.ComponentModel.DataAnnotations;

namespace AssignmentForMCСSA.Data.Models.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "ProductTitleIsTooShort", MinimumLength = 2)]
        [Required(ErrorMessage = "ProductTitleIsTooShort")]
        public string Title { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "ThePriceOfAProductCannotBeLessThan0")]
        [Required(ErrorMessage = "ThePriceOfAProductCannotBeLessThan0")]
        public int Price { get; set; }

        [StringLength(50, ErrorMessage = "ProductCategoryIsTooShort", MinimumLength = 2)]
        [Required(ErrorMessage = "ProductCategoryIsTooShort")]
        public string Category { get; set; }

        [StringLength(500, ErrorMessage = "ProductDescriptionIsTooShort", MinimumLength = 2)]
        [Required(ErrorMessage = "ProductDescriptionIsTooShort")]
        public string Description { get; set; }

        public IFormFile? Image { get; set; }
    }
}
