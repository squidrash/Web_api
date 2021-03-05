using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web_api_pizza.Services;
using Web_api_pizza.Storage.DTO;

namespace Web_api_pizza.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        //работает
        [HttpGet("all")]
        public IActionResult GetAllOrders()
        {
            var orders = _orderService.GetAllOrders();
            return Ok(orders);
        }

        //работает
        [HttpGet("customer/{id}")]
        public IActionResult CustomerOrders(int id)
        {
            if(id <= 0)
            {
                ModelState.AddModelError("Id", $"Недопустимое значение Id - \"{id}\"");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var orders = _orderService.GetAllOrders(id);
            if (orders == null)
            {
                return NotFound($"Пользователь не найден Id - \"{id}\"");
            }
            return Ok(orders);
        }
        //работает
        [HttpGet("specific/{id}")]
        public IActionResult GetOneOrder(int id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError("Id", $"Недопустимое значение Id - \"{id}\"");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var order = _orderService.GetOneOrder(id);
            if (order == null)
            {
                return NotFound($"Заказ не найден Id - \"{id}\"");
            }
            return Ok(order);
        }

        //работает
        //проработать алгоритм изменения статуса
        [HttpPut("changeStatus")]
        public string ChangeStatus(int orderId, string orderStatus)
        {
            var status = _orderService.ChangeOrderStatus(orderId, orderStatus);
            return status;
        }

        //работают все 3 
        [HttpPost("create")]
        public string CreateOrder(List<DishDTO> dishes, int customerId = 0, int addressId = 0)
        {
            var order = _orderService.CreateOrder(dishes, customerId, addressId);
            return order;
        }
        //работает
        [HttpDelete("delete")]
        public string Delete(int id)
        {
            var order = _orderService.RemoveOrder(id);
            return order;
        }
    }
}
