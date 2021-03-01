using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreateDb.Storage.DTO;
using Microsoft.AspNetCore.Mvc;
using Web_api_pizza.Services;

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
        [HttpGet("all")]
        public List<OrderDTO> GetAllOrders()
        {
            var orders = _orderService.GetAllOrders();
            return orders;
        }

        [HttpGet("customer")]
        public List<OrderDTO> CustomerOrders(int id)
        {
            var orders = _orderService.GetAllOrders(id);
            return orders;
        }

        [HttpGet("specific")]
        public OrderDTO GetOneOrder(int id)
        {
            var order = _orderService.GetOneOrder(id);
            return order;
        }

        //[HttpPut("changeStatus")]
        //public string ChangeStatus(int orderId, string orderStatus)
        //{
        //    var status = _orderService.ChangeOrderStatus(orderId, orderStatus);
        //    return status;
        //}

        [HttpPost("create")]
        public string CreateOrder(List<DishDTO> dishes, int customerId = 0, int addressId = 0)
        {
            var order = _orderService.CreateOrder(dishes, customerId, addressId);
            return order;
        }


    }
}
