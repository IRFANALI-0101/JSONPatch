using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JSONPatch.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
//https://www.c-sharpcorner.com/article/json-patch-in-asp-net-core-web-api/
namespace JSONPatch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
            [HttpGet]
            public IActionResult JsonPatchWithModelState()
            {
                var customer = CreateCustomer();
                return new ObjectResult(customer);
            }

            [HttpPatch]
            public IActionResult JsonPatchWithModelState(
                [FromBody] JsonPatchDocument<Customer> patchDoc)
            {
                if (patchDoc != null)
                {
                    var customer = CreateCustomer();

                    patchDoc.ApplyTo(customer, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState);

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    return new ObjectResult(customer);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }

            private Customer CreateCustomer()
            {
                return new Customer
                {
                    CustomerName = "John",
                    Orders = new List<Order>()
                {
                    new Order
                    {
                        OrderName = "Order0",
                        OrderType="Urgent"
                    },
                    new Order
                    {
                        OrderName = "Order1",
                        OrderType="Normal"
                    }
                }
                };
            }
        }
}
