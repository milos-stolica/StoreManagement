using System.ComponentModel.DataAnnotations;

namespace StoreManagement.API.Entities
{
    public class Product
    {
        public string Sku { get; set; }

        public string Name { get; set; }

        public float UnitWeight { get; set; }
         
        public float Price { get; set; }

        public string Currency { get; set; }

        public bool Available { get; set; } //todo what about this, is it related to Quantity?

        public float Quantity { get; set; }

        public string Type { get; set; }
    }
}
