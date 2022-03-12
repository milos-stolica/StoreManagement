using StoreManagement.API.Repositories;

namespace StoreManagement.API.Services
{
    public class OrderedProductDetails
    {
        public string Sku { get; set; }

        public float RemainingQuantity { get; set; }

        public float OrderedQuantity { get; set; }

        public float Price { get; set; }

        public string Currency { get; set; }

        public int UnitWeight { get; set; }

        public EntityStatus Status { get; set; }
    }
}
