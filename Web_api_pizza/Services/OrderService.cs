using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Enums;
using Web_api_pizza.Storage.Models;

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
        //private readonly ICustomerService _customerService;
        public OrderService(IMapper mapper, PizzaDbContext context,
            IMenuService menuService, IAddressService addressService)
        {
            _mapper = mapper;
            _context = context;
            _menuService = menuService;
            _addressService = addressService;
            //_customerService = customerService;
        }

        private readonly Dictionary<string, StatusEnum> OrderStatusesDic = new Dictionary<string, StatusEnum>
        {
            {"Новый", StatusEnum.New },
            {"Подтвержден", StatusEnum.Confirmed },
            {"Готовится", StatusEnum.Preparing },
            {"В пути", StatusEnum.OnTheWay},
            {"Доставлен", StatusEnum.Delivered},
            {"Отменен", StatusEnum.Cancelled}
        };
        private readonly Dictionary<StatusEnum,IEnumerable<StatusEnum>> _statusChangeRule = new Dictionary<StatusEnum, IEnumerable<StatusEnum>>
        {
            { StatusEnum.New, new[]{ StatusEnum.Confirmed, StatusEnum.Cancelled } },
            { StatusEnum.Confirmed, new[]{ StatusEnum.Preparing } },
            { StatusEnum.Preparing, new[]{ StatusEnum.OnTheWay } },
            { StatusEnum.OnTheWay, new[]{ StatusEnum.Delivered } },
            { StatusEnum.Delivered, new StatusEnum[] { } },
            { StatusEnum.Cancelled, new StatusEnum[] { } },
        };
        private readonly List<string> StatusList = new List<string>()
        {
            "Новый","Подтвержден","Готовится","В пути","Доставлен","Отменен"
        };

        //public List<OrderDTO> GetAllOrders(int customerId = 0)
        //{
        //    List<OrderEntity> ordersEntity;

        //    if (customerId != 0)
        //    {
        //        ordersEntity = _context.Orders
        //            .Include(o => o.Customer)
        //            .Where(o => o.CustomerEntityId == customerId)
        //            .Include(o => o.Products)
        //            .ThenInclude(p => p.Dish)
        //            .OrderByDescending(o => o.CreatTime)
        //            .ToList();
        //    }
        //    else
        //    {
        //        ordersEntity = _context.Orders
        //            .Include(o => o.Products)
        //            .OrderByDescending(o=> o.CreatTime)
        //            .ToList();
        //    }
        //    //var ordersDTO = _mapper.Map<List<OrderEntity>, List<OrderDTO>>(ordersEntity);
        //    var ordersDTO = new List<OrderDTO>();
        //    foreach (var o in ordersEntity)
        //    {
        //        var order = GetListDishes(o);
        //        ordersDTO.Add(order);
        //    }
        //    return ordersDTO;
        //}
        public List<OrderDTO> GetAllOrders(int customerId = 0)
        {
            List<OrderEntity> ordersEntity;

            if (customerId != 0)
            {
                ordersEntity = _context.Orders
                    .Where(o => o.CustomerEntityId == customerId)
                    .OrderByDescending(o => o.CreatTime)
                    .ToList();
                foreach( var o in ordersEntity)
                {
                    o.CustomerEntityId = 0;
                }
            }
            else
            {
                ordersEntity = _context.Orders
                    .OrderByDescending(o => o.CreatTime)
                    .ToList();
            }
            //var ordersDTO = _mapper.Map<List<OrderEntity>, List<OrderDTO>>(ordersEntity);
            var ordersDTO = new List<OrderDTO>();
            foreach (var o in ordersEntity)
            {
                var order = GetOneOrder(o.Id);
                ordersDTO.Add(order);
            }
            return ordersDTO;
        }

        // нужен ли такой громоздкий метод???
        public OrderDTO GetOneOrder(int id)
        {
            var orderEntity = _context.Orders.
                Where(o => o.Id == id)
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
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            string message;
            if(order == null)
            {
                message = null;
                return message;
            }
            if(!StatusList.Contains(orderStatus))
            {
                message = "BadStatus";
                return message;
            }
            var newStatus = OrderStatusesDic[orderStatus];
            var alowedStatus = _statusChangeRule[order.Status];
            if (alowedStatus.Contains(newStatus))
            {
                order.Status = newStatus;
                _context.SaveChanges();
                message = "Статус изменен";
                return message;
            }
            else
            {
                message = "BadStatus";
                return message;
            }
            //var checkStatus = CheckStatus(changeStatus.Status, orderStatus);
            //if(checkStatus)
            //{
            //    changeStatus.Status = OrderStatusesDic[orderStatus];
            //    _context.SaveChanges();
            //    message = "Статус изменен";
            //    return message;
            //}
            //else
            //{
            //    message = "BadStatus";
            //    return message;
            //}
        }

        public string CreateOrder(List<DishDTO> dishes, int customerId = 0, int addressId = 0)
        {
            string message = null;
            var order = CreateCustomerOrder(customerId);

            if(order == null)
            {
                message = "NullCustomer";
                return message;
            }
            foreach (var d in dishes)
            {
                if(d.Id > 0)
                {
                        CreateOrderDishes(order.Id, (int)d.Id, d.Quantity);
                        message += $"\nБлюдо добавлено Id - {d.Id}";
                }
            }
            if(message == null)
            {
                RemoveOrder(order.Id);
                message = "NullMenu";
                return message;
            }
            if (addressId != 0)
            {
                var addressOrder = CreateAddressOrder(order.Id, addressId);
                if(addressOrder == null)
                {
                    RemoveOrder(order.Id);
                    message = "NullAddress";
                    return message;
                }
            }
            message += "\nЗаказ создан";
            return message;
        }

        public string RemoveOrder(int id)
        {
            string message;
            
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if(order == null)
            {
                message = null;
                return message;
            }
            else
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
                message = "Заказ удален";
                return message;
            }
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
        
        private PersonDTO GetCustomerOrder(int clientId)
        {
            var customerEntity = _context.Customers.FirstOrDefault(c => c.Id == clientId);
            var personDTO = _mapper.Map<CustomerEntity, PersonDTO>(customerEntity);
            return personDTO;
        }

        private OrderEntity CreateCustomerOrder(int customerId)
        {
            var order = new OrderEntity();

            order.CreatTime = DateTime.Now;
            order.Status = StatusEnum.New;
            if (customerId != 0)
            {
                var person = GetCustomerOrder(customerId);
                if(person != null)
                {
                    order.CustomerEntityId = customerId;
                }
                else
                {
                    order = null;
                    return order;
                }
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
            if(quantity <= 0)
            {
                quantity = 1;
            }
            var orderDish = new OrderDishEntity { OrderEntityId = orderId, DishEntityId = dishId, Quantity = quantity };
            _context.OrderDishEntities.Add(orderDish);
            _context.SaveChanges();
        }

        private string CreateAddressOrder(int orderId, int addressId)
        {
            string message;
            var findAddress = _addressService.GetDeliveryAddress(addressId);
            if(findAddress == null)
            {
                message = null;
                return message;
            }
            var addressOrder = new AddressOrderEntity { OrderEntityId = orderId, AddressEntityId = addressId };
            _context.AddressOrderEntities.Add(addressOrder);
            _context.SaveChanges();
            message = "связь создана";
            return message;
        }
        //private bool CheckStatus(StatusEnum oldStatus, string newStatus)
        //{
        //    bool isChangeStatus = false;
        //    var changeStatus = OrderStatusesDic[newStatus];
        //    switch(oldStatus)
        //    {
        //        case StatusEnum.New:
        //            if(changeStatus == StatusEnum.Confirmed || changeStatus == StatusEnum.Cancelled)
        //            {
        //                isChangeStatus = true;
        //            }
        //            break;
        //        case StatusEnum.Confirmed:
        //            if(changeStatus == StatusEnum.Preparing)
        //            {
        //                isChangeStatus = true;
        //            }
        //            break;
        //        case StatusEnum.Preparing:
        //            if(changeStatus == StatusEnum.OnTheWay)
        //            {
        //                isChangeStatus = true;
        //            }
        //            break;
        //        case StatusEnum.OnTheWay:
        //            if (changeStatus == StatusEnum.Delivered)
        //            {
        //                isChangeStatus = true;
        //            }
        //            break;
        //        default:
        //            isChangeStatus = false;
        //            break;
        //    }
        //    return isChangeStatus;
        //}
    }
}
