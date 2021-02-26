using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CreateDb.Storage;
using CreateDb.Storage.DTO;
using CreateDb.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace Web_api_pizza.Services
{
    public interface IOrderService
    {

    }
    public class OrderService :IOrderService
    {
        private readonly Mapper _mapper;
        private readonly PizzaDbContext _context;
        public OrderService(Mapper mapper, PizzaDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        readonly Dictionary<string, StatusDTO> OrderStatusesDic = new Dictionary<string, StatusDTO>
        {
            {"Новый", StatusDTO.New },
            {"Готовится", StatusDTO.Preparing },
            {"В пути", StatusDTO.OnTheWay},
            {"Доставлен", StatusDTO.Delivered},
            {"Отменен", StatusDTO.Cancelled}
        };

        public List<OrderDTO> GetAllOrders(CustomerEntity customer = null)
        {
            List<OrderEntity> ordersEntity;
            if (customer != null)
            {
                ordersEntity = customer.Orders.ToList();
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
            var orderEntity = _context.Orders.FirstOrDefault(o => o.Id == id);
            var orderDTO = _mapper.Map<OrderEntity, OrderDTO>(orderEntity);
            return orderDTO;
        }

        public string ChangeOrderStatus(int orderId, string orderStatus)
        {
            try
            {
                var changeStatus = _context.Orders.FirstOrDefault(o => o.Id == orderId);
                changeStatus.Status = (Status)OrderStatusesDic[orderStatus];
                _context.SaveChanges();
                return "Статус изменен";
            }
            catch(Exception e)
            {
                return $"Неудалось изменить статус {e.Message}";
            }
        }

        public void CreateOrder(List<DishDTO> dishes, int customerId = 0, int addressId = 0)
        {
            var order = CustomerOrder(customerId);
            foreach (var d in dishes)
            {
                OrderDishes(order.Id, d.Id, d.Quantity);
            }
            if(addressId != 0)
            {
                AddressOrder(order.Id, addressId);
            }
        }
        private OrderEntity CustomerOrder(int customerId)
        {
            var order = new OrderEntity();

            order.CreatTime = DateTime.Now;
            order.Status = Status.New;
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
    }
}
