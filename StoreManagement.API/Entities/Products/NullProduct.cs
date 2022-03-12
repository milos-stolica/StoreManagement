namespace StoreManagement.API.Entities
{
    public class NullProduct : Product
    {
        public NullProduct()
        {
           Sku = string.Empty;
           Name = string.Empty;
           UnitWeight = 0;
           Price = 0;
           Currency = string.Empty;
           Available = false;
           Quantity = 0;
           Type = string.Empty;
        }
    }
}
