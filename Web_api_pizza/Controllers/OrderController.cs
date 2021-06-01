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
        public IActionResult ChangeStatus(int orderId, string orderStatus)
        {
            if(orderId <= 0)
            {
                ModelState.AddModelError("Id", $"Недопустимое значение Id  заказа - \"{orderId}\"");
            }
            if(orderStatus == null)
            {
                ModelState.AddModelError("Status", "Введите статус заказа");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var status = _orderService.ChangeOrderStatus(orderId, orderStatus);
            if(status == null)
            {
                return NotFound($"Заказ не найден Id — {orderId}");
            }
            if(status == "BadStatus")
            {
                return BadRequest("Недопустимый статус");
            }
            return Ok(status);
        }

        //работают все 3 
        [HttpPost("create")]
        public IActionResult CreateOrder(List<DishDTO> dishes, int customerId = 0, int addressId = 0)
        {
            if(dishes == null)
            {
                ModelState.AddModelError("Dishes","Выберите блюдо(а)");
            }
            if(customerId < 0)
            {
                ModelState.AddModelError("customerId", $"Недопустимое значение Id клиента - \"{customerId}\"");
            }
            if (addressId < 0)
            {
                ModelState.AddModelError("addressId", $"Недопустимое значение Id адреса - \"{addressId}\"");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var order = _orderService.CreateOrder(dishes, customerId, addressId);
            if (order == "NullDish")
            {
                return BadRequest($"Ошибка при создании заказа: некоторых блюд нет в меню {order}");
            }
            if (order == "NullMenu")
            {
                return BadRequest($"Ошибка при создании заказа: Нет блюд {order}");
            }
            if (order == "NullCustomer")
            {
                return BadRequest($"Ошибка при создании заказа: Клиент не найден {order}");
            }
            if (order == "NullAddress")
            {
                return BadRequest($"Ошибка при создании заказа: Адрес не найден {order}");
            }
            return Ok(order);
        }
        //работает
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            if(id <= 0)
            {
                ModelState.AddModelError("Id", $"Недопустимое значение Id - \"{id}\"");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var order = _orderService.RemoveOrder(id);
            if(order == null)
            {
                return NotFound($"Заказ не найден Id — \"{id}\"");
            }
            return Ok(order);
        }
    }
}
