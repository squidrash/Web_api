using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Web_api_pizza.Filters;
using Web_api_pizza.Storage;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Enums;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.Services
{
    public interface IOrderService
    {
        public OrderDTO GetOneOrder(int id);
        public string ChangeOrderStatus(int orderId, string orderStatus);
        public string CreateOrder(List<DishDTO> dishes, string promocode, int customerId = 0, int addressId = 0);
        public string RemoveOrder(int id);
        public List<OrderDTO> GetAllOrders(OrderFilter filter, int customerId = 0);
    }
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly PizzaDbContext _context;
        private readonly ISpecialOfferService _offer;
        public OrderService(IMapper mapper, PizzaDbContext context, ISpecialOfferService offer)
        {
            _mapper = mapper;
            _context = context;
            _offer = offer;
        }

        private readonly Dictionary<string, OrderStatusEnum> OrderStatusesDic = new Dictionary<string, OrderStatusEnum>
        {
            {"New", OrderStatusEnum.New },
            {"Confirmed", OrderStatusEnum.Confirmed },
            {"Preparing", OrderStatusEnum.Preparing },
            {"OnTheWay", OrderStatusEnum.OnTheWay},
            {"Delivered", OrderStatusEnum.Delivered},
            {"Cancelled", OrderStatusEnum.Cancelled}
        };

        //поменял когда делал сайт
        //private readonly Dictionary<string, OrderStatusEnum> OrderStatusesDic = new Dictionary<string, OrderStatusEnum>
        //{
        //    {"Новый", OrderStatusEnum.New },
        //    {"Подтвержден", OrderStatusEnum.Confirmed },
        //    {"Готовится", OrderStatusEnum.Preparing },
        //    {"В пути", OrderStatusEnum.OnTheWay},
        //    {"Доставлен", OrderStatusEnum.Delivered},
        //    {"Отменен", OrderStatusEnum.Cancelled}
        //};
        private readonly Dictionary<OrderStatusEnum,IEnumerable<OrderStatusEnum>> _statusChangeRule = new Dictionary<OrderStatusEnum, IEnumerable<OrderStatusEnum>>
        {
            { OrderStatusEnum.New, new[]{ OrderStatusEnum.Confirmed, OrderStatusEnum.Cancelled } },
            { OrderStatusEnum.Confirmed, new[]{ OrderStatusEnum.Preparing } },
            { OrderStatusEnum.Preparing, new[]{ OrderStatusEnum.OnTheWay } },
            { OrderStatusEnum.OnTheWay, new[]{ OrderStatusEnum.Delivered } },
            { OrderStatusEnum.Delivered, new OrderStatusEnum[] { } },
            { OrderStatusEnum.Cancelled, new OrderStatusEnum[] { } },
        };
        //поменял когда делал сайт
        //private readonly List<string> StatusList = new List<string>()
        //{
        //    "Новый","Подтвержден","Готовится","В пути","Доставлен","Отменен"
        //};
        private readonly List<string> StatusList = new List<string>()
        {
            "New","Confirmed","Preparing","OnTheWay","Delivered","Cancelled"
        };

        public List<OrderDTO> GetAllOrders(OrderFilter filter, int customerId = 0)
        {
            var orders = _context.Orders
                .Include(o => o.Customer)
                    .ThenInclude(c => c.Customer)
                .Include(o => o.Products)
                    .ThenInclude(p => p.Dish)
                .Include(o => o.AddressOrder)
                    .ThenInclude(a => a.Address)
                .OrderByDescending(o => o)
                .AsQueryable();

            if (customerId != 0)
            {
                orders = orders.Where(x => x.Customer.CustomerEntityId == customerId);
            }

            orders = filter.Filters(orders);
            

            List<OrderEntity> ordersEntity;
            ordersEntity = orders.OrderByDescending(o => o.CreateTime).ToList();

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
            var ordersDTO = _mapper.Map<List<OrderDTO>>(ordersEntity);
            
            return ordersDTO;
        }

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
        }
        
        public string CreateOrder(List<DishDTO> dishes, string promocode, int customerId, int addressId)
        {
            var resultValidation = OrderValidate(dishes, customerId, addressId, promocode);
            if (resultValidation.IsSuccess == false)
            {
                return resultValidation.Message;
            }

            var order = CreateOrderDishes(dishes);
            if(promocode != null)
            {
                var checkOfferResult = (ResultOfferCheck)resultValidation;
                order.DiscountSum = checkOfferResult.DiscountSum;
                order.TotalSum -= checkOfferResult.DiscountSum;
            }

            //if (promocode != null)
            //{
            //    //var checkOfferResult = (ResultOfferCheck)_offer.CheckComplianceSpecialOffer(dishes, promocode);
            //    var checkOfferResult = _offer.CheckComplianceSpecialOffer(dishes, promocode);
            //    var checkOfferResult1 = (ResultOfferCheck)checkOfferResult;
            //    if (checkOfferResult.IsSuccess == false)
            //    {
            //        return checkOfferResult.Message;
            //    }
            //    order.DiscountSum = checkOfferResult1.DiscountSum;
            //    order.TotalSum -= checkOfferResult1.DiscountSum;
            //}

            if (customerId != 0)
            {
                var customerOrder = new CustomerOrderEntity
                {
                    CustomerEntityId = customerId,
                    OrderEntityId = order.Id
                };
                _context.CustomerOrderEntities.Add(customerOrder);
            }
            if (addressId != 0)
            {
                var addressOrder = new AddressOrderEntity { OrderEntityId = order.Id, AddressEntityId = addressId };
                _context.AddressOrderEntities.Add(addressOrder);
            }

            _context.SaveChanges();

            string message = "Заказ создан";
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

            private OperationResult OrderValidate(List<DishDTO> dishes, int customerId, int addressId, string promocode)
        {
            var result = new OperationResult(false);

            var dishEntity = _context.Dishes
                .Where(x => dishes.Select(y => y.Id.Value)
                                  .Contains(x.Id))
                .ToList();

            if (dishEntity.Count != dishes.Count)
            {
                result.Message = "Блюда не соответствуют БД";
                return result;
            }
            if (customerId != 0)
            {
                var customer = _context.Customers.FirstOrDefault(c => c.Id == customerId);
                if (customer == null)
                {
                    result.Message = "Пользователь не найден";
                    return result;
                }
            }

            if (addressId != 0)
            {
                var address = _context.Addresses.FirstOrDefault(a => a.Id == addressId);
                if (address == null)
                {
                    result.Message = "Адрес не найден";
                    return result;
                }
            }

            
            if (promocode != null)
            {
                var checkOfferResult = _offer.CheckComplianceSpecialOffer(dishes, promocode);
                
                return checkOfferResult;
            }

            result.IsSuccess = true;
            result.Message = "Успешно";
            return result;
        }

        private OrderEntity CreateOrderDishes(List<DishDTO> dishes)
        {
            var dishEntity = _context.Dishes
                .Where(x => dishes.Select(y => y.Id.Value)
                                  .Contains(x.Id))
                .ToList();

            var order = new OrderEntity() {
                CreateTime = DateTime.Now,
                Status = OrderStatusEnum.New
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

            return order;
        }
    }
}
