using BusinessLogicLayer.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantWPF.Tests
{
    public class FoodServiceTests
    {
        [Test]
        public void GetAll_ShouldNotThrow()
        {
            var service = new FoodService();
            var result = service.GetAll();

            Assert.That(result, Is.Not.Null);
        }
    }
}
