using System;
using System.Collections.Generic;

namespace StoreManagement.API.Models
{
    public class OrderReceiptDTO
    {
        public IEnumerable<OrderLineDTO> OrderLines { get; set; }

        public float TotalPrice { get; set; }

        public string Currency { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
