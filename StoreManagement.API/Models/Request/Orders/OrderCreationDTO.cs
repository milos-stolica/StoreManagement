using System.ComponentModel.DataAnnotations;

namespace StoreManagement.API.Models
{
    public class OrderCreationDTO
    {
        [Required]
        [MaxLength(50)]
        public string ProductSku { get; set; }

        [Required]
        [Range(0.0, float.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public float Quantity { get; set; }
    }
}
