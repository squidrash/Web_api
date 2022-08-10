using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Web_api_pizza.Filters;
using Web_api_pizza.OrderObserver;
using Web_api_pizza.Storage;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Enums;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.Services
{
    public interface IOrderService
    {
        /// <summary>
        /// Получение списка заказов
        /// </summary>
        /// <param name="filter">Опциональный параметр.
        /// Фильтрация по статусу, наличию клиента и адреса</param>
        /// <param name="customerId">Опциональный параметр. Id пользователя.</param>
        /// <returns>Спиcок всех заказов или заказов конкреткого пользователя, если указан Id пользователя</returns>
        public List<OrderDTO> GetAllOrders(OrderFilter filter, int customerId = 0);

        /// <summary>
        /// Получение данных конкретного заказа
        /// </summary>
        /// <param name="id">Id заказа</param>
        /// <returns>Данные конкретного заказа</returns>
        public OrderDTO GetOneOrder(int id);

        /// <summary>
        /// Создание нового заказа
        /// </summary>
        /// <param name="dishes"> Список блюд которые входят в казаз</param>
        /// <param name="promocode">Опциональный параметр. Промокод на скидку</param>
        /// <param name="customerId">Опциональный параметр. Id пользователя, сделавшего заказ</param>
        /// <param name="addressId">Опциональный параметр. Id адреса доставки</param>
        /// <returns>Результат операции в виде объекта OperationResult(результат bool, сообщение)</returns>
        public OperationResult CreateOrder(List<DishDTO> dishes, string promocode, int customerId = 0, int addressId = 0);

        /// <summary>
        ////Изменение статуса заказа
        /// </summary>
        /// <param name="orderId"> Id заказа</param>
        /// <param name="orderStatus"> Новый статус заказа</param>
        /// <returns>Результат операции в виде объекта OperationResult(результат bool, сообщение)</returns>
        public OperationResult ChangeOrderStatus(int orderId, string orderStatus);

        /// <summary>
        /// Удаление заказа
        /// </summary>
        /// <param name="id">Id заказа</param>
        /// <returns>Результат операции в виде объекта OperationResult(результат bool, сообщение)</returns>
        public OperationResult RemoveOrder(int id);
    }
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly PizzaDbContext _context;
        private readonly ISpecialOfferService _offer;
        private readonly ISubject _subject;
        public OrderService(IMapper mapper, PizzaDbContext context, ISpecialOfferService offer, ISubject subject)
        {
            _mapper = mapper;
            _context = context;
            _offer = offer;
            _subject = subject;
        }

        private readonly Dictionary<string, OrderStatusEnum> OrderStatusesDic = new Dictionary<string, OrderStatusEnum>
        {
            {"New", OrderStatusEnum.New },
            {"Confirmed", OrderStatusEnum.Confirmed },
            {"Preparing", OrderStatusEnum.Preparing },
            {"OnTheWay", OrderStatusEnum.OnTheWay},
            {"Delivered", OrderStatusEnum.Delivered},
            {"Canceled", OrderStatusEnum.Canceled}
        };

        
        private readonly Dictionary<OrderStatusEnum,IEnumerable<OrderStatusEnum>> _statusChangeRule = new Dictionary<OrderStatusEnum, IEnumerable<OrderStatusEnum>>
        {
            { OrderStatusEnum.New, new[]{ OrderStatusEnum.Confirmed, OrderStatusEnum.Canceled } },
            { OrderStatusEnum.Confirmed, new[]{ OrderStatusEnum.Preparing } },
            { OrderStatusEnum.Preparing, new[]{ OrderStatusEnum.OnTheWay } },
            { OrderStatusEnum.OnTheWay, new[]{ OrderStatusEnum.Delivered } },
            { OrderStatusEnum.Delivered, new OrderStatusEnum[] { } },
            { OrderStatusEnum.Canceled, new OrderStatusEnum[] { } },
        };
        
        private readonly List<string> StatusList = new List<string>()
        {
            "New","Confirmed","Preparing","OnTheWay","Delivered","Canceled"
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
                .OrderByDescending(o => o.CreateTime)
                .AsQueryable();

            if (customerId != 0)
            {
                orders = orders.Where(x => x.Customer.CustomerEntityId == customerId);
            }

            orders = filter.Filters(orders);
            

            List<OrderEntity> ordersEntity = orders.ToList();

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

        public OperationResult ChangeOrderStatus(int orderId, string orderStatus)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);

            if(order == null)
            {
                return null;
            }

            var result = new OperationResult(false);

            if(!StatusList.Contains(orderStatus))
            {
                result.Message = "Неверно указан статус заказа";
                return result;
            }
            var newStatus = OrderStatusesDic[orderStatus];
            var alowedStatus = _statusChangeRule[order.Status];

            if (!alowedStatus.Contains(newStatus))
            {
                result.Message = "Нельзя изменить на выбраный статус";
                return result;
            }

            order.Status = newStatus;
            _context.SaveChanges();

            result.IsSuccess = true;
            result.Message = "Статус изменен";

            return result;
        }
        
        public OperationResult CreateOrder(List<DishDTO> dishes, string promocode, int customerId, int addressId)
        {
            var resultValidation = OrderValidate(dishes, customerId, addressId, promocode);
            if (resultValidation.IsSuccess == false)
            {
                return resultValidation;
            }

            var order = CreateOrderDishes(dishes);
            if(promocode != null)
            {
                var checkOfferResult = (ResultOfferCheck)resultValidation;
                order.DiscountSum = checkOfferResult.DiscountSum;
                order.TotalSum -= checkOfferResult.DiscountSum;
            }

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

            _subject.Notify();

            var result = new OperationResult(true, "Заказ создан");
            return result;
        }

        public OperationResult RemoveOrder(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);

            if(order == null)
            {
                return null;
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();

            var result = new OperationResult(true, "Заказ удален");
            return result;
            
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
