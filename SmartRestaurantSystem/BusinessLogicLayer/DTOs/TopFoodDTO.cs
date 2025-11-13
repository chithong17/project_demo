using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class TopFoodDTO
    {
        public string FoodName { get; set; }
        public int QuantitySold { get; set; }
        public string? ColorHex { get; set; }
    }
}
