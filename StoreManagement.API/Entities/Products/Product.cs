namespace StoreManagement.API.Entities
{
    //todo data annotations
    public class Product
    {
        public string Sku { get; set; }

        public string Name { get; set; }

        public float UnitWeight { get; set; }
         
        public float Price { get; set; }

        public string Currency { get; set; }

        //todo what about this, is this related to Quantity?
        public bool Available { get; set; }

        public float Quantity { get; set; }

        public string Type { get; set; }
    }
}
