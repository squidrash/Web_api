using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web_api_pizza.Filters;
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

        /// <summary>
        /// Получение списка заказов
        /// </summary>
        /// <param name="filter">
        /// Опциональный параметр.
        /// Фильтрация по статусу, наличию клиента и адреса
        /// </param>
        /// <response code="200"> Список заказов</response>
        [HttpGet("all")]
        public IActionResult GetAllOrders([FromQuery] OrderFilter filter)
        {
            var orders = _orderService.GetAllOrders(filter);
            return Ok(orders);
        }

        /// <summary>
        /// Получение списка заказов одного пользователя
        /// </summary>
        /// <param name="id"> Id пользователя</param>
        /// <param name="filter">Опциональный параметр.
        /// Фильтрация по статусу, адреса</param>
        /// <response code="200"></response>
        /// <response code="400">Неверно указан Id пользователя</response>
        /// <response code="404">Пользователь не найден</response>
        [HttpGet("customer/{id}/")]
        public IActionResult CustomerOrders(int id, [FromQuery] OrderFilter filter)
        {
            if (id <= 0)
            {
                ModelState.AddModelError("Id", $"Недопустимое значение Id - \"{id}\"");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orders = _orderService.GetAllOrders(filter, id);

            if (orders == null)
            {
                return NotFound($"Пользователь не найден Id - \"{id}\"");
            }
            return Ok(orders);
        }

        /// <summary>
        /// Получение данных конкретного заказа
        /// </summary>
        /// <param name="id">Id заказа</param>
        /// <response code="200">Данные конкретного заказа</response>
        /// <response code="400">Неверно указан Id заказа</response>
        /// <response code="404">Заказ не найден</response>
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

        /// <summary>
        ////Изменение статуса заказа
        /// </summary>
        /// <param name="orderId"> Id заказа</param>
        /// <param name="orderStatus"> Новый статус заказа</param>
        /// <response code="200">Сообщение об успешном изменени статуса</response>
        /// <response code="400">Ошибки связаные с неверно указаным Id заказа, статуса, невозможностью изменить статус на новый</response>
        /// <response code="404">Заказ не найден</response>
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
            var result = _orderService.ChangeOrderStatus(orderId, orderStatus);
            if(result == null)
            {
                return NotFound($"Заказ не найден Id — {orderId}");
            }
            if(result.IsSuccess == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Создание нового заказа
        /// </summary>
        /// <param name="dishes"> Список блюд которые входят в казаз</param>
        /// <param name="promocode">Опциональный параметр. Промокод на скидку</param>
        /// <param name="customerId">Опциональный параметр. Id пользователя, сделавшего заказ</param>
        /// <param name="addressId">Опциональный параметр. Id адреса доставки</param>
        /// <response code="200">Сообщение об успешном создании заказа</response>
        /// <response code="400">Возможные ошибки:
        /// 1)неверно указаны Id пользователя и адреса,
        /// 2)блюда не указаны или их нет в БД,
        /// 3)если введен промокон:
        ///     3а) акция не найдена
        ///     3b) блюда не соответствуют условиям акции</response>
        [HttpPost("create")]
        public IActionResult CreateOrder(List<DishDTO> dishes, [FromQuery] string promoCode, [FromQuery] int customerId, [FromQuery] int addressId)
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
            var result = _orderService.CreateOrder(dishes, promoCode, customerId, addressId);
            if (result.IsSuccess == false)
            {
                return BadRequest(result);
            }
            
            return Ok(result);
        }

        /// <summary>
        /// Удаление заказа
        /// </summary>
        /// <param name="id">Id заказа</param>
        /// <response code="200">Сообщение об успешном удалении</response>
        /// <response code="400">Неверно указан Id заказа</response>
        /// <response code="404">Заказ не найден</response>
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
            var result = _orderService.RemoveOrder(id);
            if(result == null)
            {
                return NotFound($"Заказ не найден Id — \"{id}\"");
            }
            return Ok(result);
        }
    }
}
