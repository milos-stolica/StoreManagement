using System.Collections.Generic;

namespace StoreManagement.API.Models
{
    public class OrderUnprocessableDTO
    {
        public IEnumerable<string> Messages { get; set; }
    }
}
