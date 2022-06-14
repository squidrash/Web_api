using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web_api_pizza.OrderObserver;

namespace Web_api_pizza.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewOrderObserverController :ControllerBase
    {
        private readonly INotification _notification;
        public NewOrderObserverController(INotification notification)
        {
            _notification = notification;
        }

        [HttpGet("checkNewOrder")]
        public async Task<IActionResult> CheckNewOrderAsync()
        {
             var result = await Task.Run(() => _notification.CheckNewOrders());
            return Ok(result);
        }
    }
}
