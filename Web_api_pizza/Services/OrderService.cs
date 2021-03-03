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
        private readonly IMenuService _menuService;
        private readonly IAddressService _addressService;
        private readonly ICustomerService _customerService;
        public OrderService(IMapper mapper, PizzaDbContext context, ICustomerService customerService,
            IMenuService menuService, IAddressService addressService)
        {
            _mapper = mapper;
            _context = context;
            _menuService = menuService;
            _addressService = addressService;
            _customerService = customerService;
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
                    .OrderByDescending(o => o.CreatTime)
                    .ToList();
            }
            else
            {
                ordersEntity = _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.Products)
                    .ThenInclude(p => p.Dish)
                    .OrderByDescending(o=> o.CreatTime)
                    .ToList();
            }
            //var ordersDTO = _mapper.Map<List<OrderEntity>, List<OrderDTO>>(ordersEntity);
            var ordersDTO = new List<OrderDTO>();
            foreach (var o in ordersEntity)
            {
                var order = GetListDishes(o);
                ordersDTO.Add(order);
            }
            return ordersDTO;
        }

        // нужен литакой громоздкий метод???
        public OrderDTO GetOneOrder(int id)
        {
            var orderEntity = _context.Orders.
                Where(o => o.Id == id)
                //.Include(o => o.Customer)
                .Include(o => o.Products)
                .Include(o => o.AddressOrder)
                .FirstOrDefault();
            var getListDishes = GetListDishes(orderEntity);
            var order = getListDishes;

            if(orderEntity.AddressOrder != null)
            {
                var getOrderAddress = GetOrderAddress(orderEntity.AddressOrder.AddressEntityId);
                order.Address = getOrderAddress;
            }
            if(orderEntity.CustomerEntityId != null)
            {
                var getCustomerOrder = GetCustomerOrder((int)orderEntity.CustomerEntityId);
                order.Client = getCustomerOrder;
            }
            return order;
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
                var order = CreateCustomerOrder(customerId);
                foreach (var d in dishes)
                {
                    CreateOrderDishes(order.Id, (int)d.Id, d.Quantity);

                }
                if (addressId != 0)
                {
                    CreateAddressOrder(order.Id, addressId);
                }
                return "Заказ создан";
            }
            catch
            {
                return "Неверные данные заказа";
            }
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

        private OrderDTO GetListDishes(OrderEntity order)
        {
            var orderDTO = _mapper.Map<OrderEntity, OrderDTO>(order);
            var dishesList = new List<DishDTO>();
            foreach (var d in order.Products)
            {
                var dishDTO = _menuService.GetOneDish(d.DishEntityId);
                dishDTO.Quantity = d.Quantity;
                dishesList.Add(dishDTO);
            }
            orderDTO.Dishes = dishesList;
            return orderDTO;
        }
        private AddressDTO GetOrderAddress(int addressId)
        {
            var address = _addressService.GetDeliveryAddress(addressId);

            return address;
        }
        // нужно решить проблему с повторением данных
        private CustomerDTO GetCustomerOrder(int clientId)
        {
            var customerDTO = _customerService.GetOneCustomer(clientId);
            customerDTO.Orders = null;
            return customerDTO;
        }

        private OrderEntity CreateCustomerOrder(int customerId)
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

        private void CreateOrderDishes(int orderId, int dishId, int quantity = 1)
        {
            var orderDish = new OrderDishEntity { OrderEntityId = orderId, DishEntityId = dishId, Quantity = quantity };
            _context.OrderDishEntities.Add(orderDish);
            _context.SaveChanges();
        }

        private void CreateAddressOrder(int orderId, int addressId)
        {
            var addressOrder = new AddressOrderEntity { OrderEntityId = orderId, AddressEntityId = addressId };
            _context.AddressOrderEntities.Add(addressOrder);
            _context.SaveChanges();
        }
    }
}
