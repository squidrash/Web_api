using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Web_api_pizza.Filters;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Enums;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.Services
{
    public interface IOrderService
    {
        public OrderDTO GetOneOrder(int id);
        public string ChangeOrderStatus(int orderId, string orderStatus);
        public string CreateOrder(List<DishDTO> dishes, int customerId = 0, int addressId = 0);
        public string RemoveOrder(int id);
        public List<OrderDTO> GetAllOrders(OrderFilter filter, int customerId = 0);
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
            {"New", StatusEnum.New },
            {"Confirmed", StatusEnum.Confirmed },
            {"Preparing", StatusEnum.Preparing },
            {"OnTheWay", StatusEnum.OnTheWay},
            {"Delivered", StatusEnum.Delivered},
            {"Cancelled", StatusEnum.Cancelled}
        };

        // поменял когда делал сайт
        //private readonly Dictionary<string, StatusEnum> OrderStatusesDic = new Dictionary<string, StatusEnum>
        //{
        //    {"Новый", StatusEnum.New },
        //    {"Подтвержден", StatusEnum.Confirmed },
        //    {"Готовится", StatusEnum.Preparing },
        //    {"В пути", StatusEnum.OnTheWay},
        //    {"Доставлен", StatusEnum.Delivered},
        //    {"Отменен", StatusEnum.Cancelled}
        //};
        private readonly Dictionary<StatusEnum,IEnumerable<StatusEnum>> _statusChangeRule = new Dictionary<StatusEnum, IEnumerable<StatusEnum>>
        {
            { StatusEnum.New, new[]{ StatusEnum.Confirmed, StatusEnum.Cancelled } },
            { StatusEnum.Confirmed, new[]{ StatusEnum.Preparing } },
            { StatusEnum.Preparing, new[]{ StatusEnum.OnTheWay } },
            { StatusEnum.OnTheWay, new[]{ StatusEnum.Delivered } },
            { StatusEnum.Delivered, new StatusEnum[] { } },
            { StatusEnum.Cancelled, new StatusEnum[] { } },
        };
        // поменял когда делал сайт
        //private readonly List<string> StatusList = new List<string>()
        //{
        //    "Новый","Подтвержден","Готовится","В пути","Доставлен","Отменен"
        //};
        private readonly List<string> StatusList = new List<string>()
        {
            "New","Confirmed","Preparing","OnTheWay","Delivered","Cancelled"
        };

        //public List<OrderDTO> GetAllOrders(OrderFilter filter, int customerId = 0)
        //{
        //    var orders = _context.Orders
        //        .Include(o => o.Products)
        //        .ThenInclude(p => p.Dish)
        //        .Include(o => o.AddressOrder)
        //        .ThenInclude(a => a.Address)
        //        .AsQueryable();

        //    if (customerId != 0)
        //        orders = orders.Where(x => x.CustomerEntityId == customerId);

        //    orders = filter.Filters(orders);


        //    List<OrderEntity> ordersEntity;
        //    ordersEntity = orders.OrderByDescending(o => o.CreatTime).ToList();

        //    Console.WriteLine("Это объект из базы");
        //    foreach (var o in ordersEntity)
        //    {
        //        Console.WriteLine($"продукты заказа {o.Id}");
        //        foreach (var p in o.Products)
        //        {
        //            Console.WriteLine(p.Dish.ProductName);
        //            Console.WriteLine(p.Dish.Price);
        //        }
        //        Console.WriteLine("адрес");

        //        if (o.AddressOrder != null)
        //        {
        //            Console.WriteLine(o.AddressOrder.Address.City);
        //        }

        //    }

        //    var ordersDTO = _mapper.Map<List<OrderEntity>, List<OrderDTO>>(ordersEntity);

        //    //var ordersDTO = new List<OrderDTO>();
        //    //foreach (var o in ordersEntity)
        //    //{
        //    //    var order = GetOneOrder(o.Id);
        //    //    ordersDTO.Add(order);
        //    //}
        //    return ordersDTO;
        //}

        public List<OrderDTO> GetAllOrders(OrderFilter filter, int customerId = 0)
        {
            var orders = _context.Orders
                .Include(o => o.Customer)
                    .ThenInclude(c => c.Customer)
                .Include(o => o.Products)
                    .ThenInclude(p => p.Dish)
                .Include(o => o.AddressOrder)
                    .ThenInclude(a => a.Address)
                .AsQueryable();

            if (customerId != 0)
            {
                orders = orders.Where(x => x.Customer.CustomerEntityId == customerId);
                
            }

            orders = filter.Filters(orders);
            

            List<OrderEntity> ordersEntity;
            ordersEntity = orders.OrderByDescending(o => o.CreatTime).ToList();

            //Console.WriteLine("Это объект из базы");
            //foreach (var o in ordersEntity)
            //{
            //    if (o.Customer != null)
            //    {
            //        Console.WriteLine(o.Customer.Name);
            //    }

            //    Console.WriteLine($"продукты заказа {o.Id}");
            //    foreach (var p in o.Products)
            //    {
            //        Console.WriteLine(p.Dish.ProductName);
            //        Console.WriteLine(p.Dish.Price);
            //    }
            //    Console.WriteLine("адрес");

            //    if (o.AddressOrder != null)
            //    {
            //        Console.WriteLine(o.AddressOrder.Address.City);
            //    }

            //}
            var ordersDTO = _mapper.Map<List<OrderEntity>, List<OrderDTO>>(ordersEntity);
            
            return ordersDTO;
        }


        //нужен ли этот  метод вообще?
        //public OrderDTO GetOneOrder(int id)
        //{
        //    var orderEntity = _context.Orders.
        //        Where(o => o.Id == id)
        //        .Include(o => o.Products)
        //        .Include(o => o.AddressOrder)
        //        .FirstOrDefault();
        //    var getListDishes = GetListDishes(orderEntity);
        //    var order = getListDishes;

        //    if (orderEntity.AddressOrder != null)
        //    {
        //        var getOrderAddress = GetOrderAddress(orderEntity.AddressOrder.AddressEntityId);
        //        order.Address = getOrderAddress;
        //    }
        //    if (orderEntity.CustomerEntityId != null)
        //    {
        //        var getCustomerOrder = GetCustomerOrder((int)orderEntity.CustomerEntityId);
        //        order.Client = getCustomerOrder;
        //    }
        //    return order;
        //}

        public OrderDTO GetOneOrder(int id)
        {
            var orderEntity = _context.Orders.
                Where(o => o.Id == id)
                .Include(o => o.Customer)
                .Include(o => o.Products)
                .ThenInclude(p => p.Dish)
                .Include(o => o.AddressOrder)
                .ThenInclude(a => a.Address)
                .FirstOrDefault();
            var orderDTO = _mapper.Map<OrderEntity, OrderDTO>(orderEntity);
            return orderDTO;
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

        //новый
        public string CreateOrder(List<DishDTO> dishes, int customerId = 0, int addressId = 0)
        {
            string message = null;
            var order = CreateOrderDishes(dishes);
            if(order == null)
            {
                message = "NullMenu";
                return message;
                
            }

            if(customerId != 0)
            {
                var customerOrder = CreateOrderCustomer(order, customerId);
                if (customerOrder == null)
                {
                    message = "NullCustomer";
                    return message;
                }
            }

            if (addressId != 0)
            {
                var addressOrder = CreateAddressOrder(order.Id, addressId);
                if (addressOrder == null)
                {
                    RemoveOrder(order.Id);
                    message = "NullAddress";
                    return message;
                }
            }
            message = "Заказ создан";
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

        //private OrderDTO GetListDishes(OrderEntity order)
        //{
        //    var orderDTO = _mapper.Map<OrderEntity, OrderDTO>(order);
        //    var dishesList = new List<DishDTO>();
        //    foreach (var d in order.Products)
        //    {
        //        var dishDTO = _menuService.GetOneDish(d.DishEntityId);
        //        dishDTO.Quantity = d.Quantity;
        //        dishesList.Add(dishDTO);
        //    }
        //    orderDTO.Dishes = dishesList;
        //    return orderDTO;
        //}
        //private AddressDTO GetOrderAddress(int addressId)
        //{
        //    var address = _addressService.GetDeliveryAddress(addressId);

        //    return address;
        //}
        
        //private PersonDTO GetCustomerOrder(int clientId)
        //{
        //    var customerEntity = _context.Customers.FirstOrDefault(c => c.Id == clientId);
        //    var personDTO = _mapper.Map<CustomerEntity, PersonDTO>(customerEntity);
        //    return personDTO;
        //}

        

        private OrderEntity CreateOrderDishes(List<DishDTO> dishes)
        {

            //validate
            

            var dishEntity = _context.Dishes
                .Where(x => dishes.Select(y => y.Id.Value)
                                  .Contains(x.Id))
                .ToList();

            if (dishEntity.Count != dishes.Count)
                return null;

            var order = new OrderEntity() {
                CreatTime = DateTime.Now,
                Status = StatusEnum.New
            };
            _context.Orders.Add(order);
            _context.SaveChanges();

            decimal total = 0;
            var orderDish = dishEntity.Select(x =>
            {
                var quantity = dishes.Where(d => d.Id == x.Id)
                                     .Select(y => y.Quantity > 0 ? y.Quantity : 1)
                                     .First();

                total += quantity * x.Price;

                return new OrderDishEntity
                {
                    OrderEntityId = order.Id,
                    DishEntityId = x.Id,
                    Quantity = quantity
                };
            });

            _context.OrderDishEntities.AddRange(orderDish);
            order.TotalSum = total;
            _context.SaveChanges();


            /*  decimal totalSum = 0;
              foreach(var d in dishes)
              {
                  var findDish = _menuService.GetOneDish(d.Id.Value);
                  if (findDish == null)
                  {
                      RemoveOrder(orderFromDb.Id);
                      return null;
                  }
                  if (d.Quantity <= 0)
                  {
                      d.Quantity = 1;
                  }
                  totalSum += findDish.Price * d.Quantity;

                  //можно ли это действие сделать один раз?
                  var orderDish = new OrderDishEntity { OrderEntityId = orderFromDb.Id, DishEntityId = (int)d.Id, Quantity = d.Quantity };
                  _context.OrderDishEntities.Add(orderDish);
                  _context.SaveChanges();
              }
            */

            return order;
        }
        private string CreateOrderCustomer(OrderEntity order, int customerId)
        {
            string message;
            var findCustomer = _context.Customers.FirstOrDefault(c => c.Id == customerId);
            if (findCustomer == null)
            {
                message = null;
                return message;
                
            }
            //order.Customer.CustomerEntityId = customerId;
            var customerOrder = new CustomerOrderEntity { CustomerEntityId = customerId, OrderEntityId = order.Id };
            _context.CustomerOrderEntities.Add(customerOrder);
            _context.SaveChanges();
            message = "связь создана";
            return message;
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
    }
}
