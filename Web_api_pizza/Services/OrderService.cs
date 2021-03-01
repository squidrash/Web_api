using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CreateDb.Storage;
using CreateDb.Storage.DTO;
using CreateDb.Storage.Models;
using Microsoft.EntityFrameworkCore;
using Web_api_pizza.Storage.Enums;

namespace Web_api_pizza.Services
{
    public interface IOrderService
    {
        public List<OrderDTO> GetAllOrders(int customerId = 0);
        public OrderDTO GetOneOrder(int id);
        public string ChangeOrderStatus(int orderId, string orderStatus);
        public string CreateOrder(List<DishDTO> dishes, int customerId = 0, int addressId = 0);
        public string RemoveOrder(int id);
    }
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly PizzaDbContext _context;
        public OrderService(IMapper mapper, PizzaDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        readonly Dictionary<string, StatusEnum> OrderStatusesDic = new Dictionary<string, StatusEnum>
        {
            {"Новый", StatusEnum.New },
            {"Готовится", StatusEnum.Preparing },
            {"В пути", StatusEnum.OnTheWay},
            {"Доставлен", StatusEnum.Delivered},
            {"Отменен", StatusEnum.Cancelled}
        };

        public List<OrderDTO> GetAllOrders(int customerId = 0)
        {
            List<OrderEntity> ordersEntity;
            if (customerId != 0)
            {
                ordersEntity = _context.Orders
                    .Include(o => o.Customer)
                    .Where(o => o.CustomerEntityId == customerId)
                    .Include(o => o.Products)
                    .ThenInclude(p => p.Dish)
                    .ToList();
            }
            else
            {
                ordersEntity = _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.Products)
                    .ThenInclude(p => p.Dish)
                    .ToList();
            }
            var orders = _mapper.Map<List<OrderEntity>, List<OrderDTO>>(ordersEntity);
            return orders;
        }
        public OrderDTO GetOneOrder(int id)
        {
            var orderEntity = _context.Orders.
                Where(o => o.Id == id)
                .Include(o => o.Customer)
                .Include(o => o.Products)
                .ThenInclude(p => p.Dish)
                .FirstOrDefault();
            var orderDTO = _mapper.Map<OrderEntity, OrderDTO>(orderEntity);
            return orderDTO;
        }

        public string ChangeOrderStatus(int orderId, string orderStatus)
        {
            try
            {
                var changeStatus = _context.Orders.FirstOrDefault(o => o.Id == orderId);
                changeStatus.Status = OrderStatusesDic[orderStatus];
                _context.SaveChanges();
                return "Статус изменен";
            }
            catch (Exception e)
            {
                return $"Неудалось изменить статус {e.Message}";
            }
        }

        public string CreateOrder(List<DishDTO> dishes, int customerId = 0, int addressId = 0)
        {
            try
            {
                var order = CustomerOrder(customerId);
                foreach (var d in dishes)
                {
                    OrderDishes(order.Id, d.Id, d.Quantity);
                }
                if (addressId != 0)
                {
                    AddressOrder(order.Id, addressId);
                }
                return "Заказ создан";
            }
            catch
            {
                return "Неверные данные заказа";
            }
        }
        private OrderEntity CustomerOrder(int customerId)
        {
            var order = new OrderEntity();

            order.CreatTime = DateTime.Now;
            order.Status = StatusEnum.New;
            if (customerId != 0)
            {
                order.CustomerEntityId = customerId;
            }
            _context.Orders.Add(order);
            _context.SaveChanges();

            var orderFromDb = _context.Orders
                .Where(o => o.CreatTime == order.CreatTime)
                .Where(o => o.Status == order.Status)
                .Where(o => o.CustomerEntityId == order.CustomerEntityId)
                .FirstOrDefault();
            return orderFromDb;
        }
        private void OrderDishes(int orderId, int dishId, int quantity = 1)
        {
            var orderDish = new OrderDishEntity { OrderEntityId = orderId, DishEntityId = dishId, Quantity = quantity };
            _context.OrderDishEntities.Add(orderDish);
            _context.SaveChanges();
        }
        private void AddressOrder(int orderId, int addressId)
        {
            var addressOrder = new AddressOrderEntity { OrderEntityId = orderId, AddressEntityId = addressId };
            _context.AddressOrderEntities.Add(addressOrder);
            _context.SaveChanges();
        }

        public string RemoveOrder(int id)
        {
            string message;
            try
            {
                var order = _context.Orders.FirstOrDefault(o => o.Id == id);
                if(order == null)
                {
                    message = "Заказ не найден";
                }
                else
                {
                    _context.Orders.Remove(order);
                    _context.SaveChanges();
                    message = "Заказ удален";
                }
            }
            catch
            {
                message = "Ошибка при удалении заказа";
            }
            return message;
        }
    }
}
