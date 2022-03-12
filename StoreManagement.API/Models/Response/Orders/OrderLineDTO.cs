namespace StoreManagement.API.Models
{
    public class OrderLineDTO
    {
        public string ProductSku { get; set; }

        public float Quantity { get; set; }

        public float Price { get; set; }

        public string Currency { get; set; }
    }
}
