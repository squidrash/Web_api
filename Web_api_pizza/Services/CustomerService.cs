using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.Services
{
    public interface ICustomerService
    {
        public List<CustomerDTO> GetAllCustomers();
        public PersonDTO GetOneCustomer(int id);

        //получаем клиента со всеми данныйми(заказы, адреса)
        public CustomerDTO GetCustomerWithAllInfo(int id);

        public string RegistrationCustomer(CustomerDTO customer);

        //Регистируем сразу несколько пользователей
        //public void RegistrationManyCustomers(List<CustomerDTO> customers)

        public string DeleteCustomer(int id);

        //удалить сразу несколько
        //public void DeleteManyCustomers(int[] customersId);

        public string EditCustomer(CustomerDTO customer);
    }
    public class CustomerService : ICustomerService
    {
        private readonly IMapper _mapper;
        private readonly PizzaDbContext _context;
        private readonly IOrderService _orderService;
        private readonly IAddressService _addressService;
        public CustomerService(PizzaDbContext context, IMapper mapper,
            IOrderService orderService, IAddressService addressService)
        {
            _context = context;
            _mapper = mapper;
            _orderService = orderService;
            _addressService = addressService;
        }
        public List<CustomerDTO> GetAllCustomers()
        {
            var customersEntity = _context.Customers
                .OrderBy(c => c.Name)
                .ThenBy(c => c.LastName)
                .ToList();
            var customersDTO = _mapper.Map<List<CustomerEntity>, List<CustomerDTO>> (customersEntity);
            return customersDTO;
        }
        public PersonDTO GetOneCustomer(int id)
        {
            var customerEntity = _context.Customers.FirstOrDefault(c => c.Id == id);
            var personDTO = _mapper.Map<CustomerEntity, PersonDTO>(customerEntity);
            return personDTO;
        }
        public CustomerDTO GetCustomerWithAllInfo(int id)
        {
                var customerEntity = _context.Customers
                    .Where(c => c.Id == id)
                    .Include(c => c.Orders)
                    .Include(u => u.Addresses)
                    .FirstOrDefault();
                var customerDTO = _mapper.Map<CustomerEntity, CustomerDTO>(customerEntity);
            
            var listOrders = new List<OrderDTO>();
            foreach (var o in customerEntity.Orders)
            {
                var order = _orderService.GetOneOrder(o.Id);
                order.Client = null;
                listOrders.Add(order);
            }
            var listAddresses = new List<AddressDTO>();
            foreach (var a in customerEntity.Addresses)
            {
                var address = _addressService.GetDeliveryAddress(a.AddressEntityId);
                listAddresses.Add(address);
            }
            customerDTO.Orders = listOrders;
            customerDTO.Address = listAddresses;
            return customerDTO;
        }
        public string RegistrationCustomer(CustomerDTO customer)
        {
            customer.Id = 0;
            string message;
            var checkCustomer = _context.Customers
                .Where(c => c.Name == customer.Name)
                .Where(c => c.LastName == customer.LastName)
                .Where(c => c.Phone == customer.Phone)
                .FirstOrDefault();
            if (checkCustomer == null)
            {
                var customerEntity = _mapper.Map<CustomerDTO, CustomerEntity>(customer);
                _context.Customers.Add(customerEntity);

                _context.SaveChanges();
                message = "Пользователь зарегистрирован";
            }
            else
            {
                message = "Пользователь уже существует";
            }
            return message;
        }
        public void RegistrationManyCustomers(List<CustomerDTO> customers)
        {
            var customersEntities = _mapper.Map<List<CustomerDTO>, List<CustomerEntity>>(customers);
            foreach (var customer in customersEntities)
            {
                var checkCustomer = _context.Customers
                .Where(c => c.Name == customer.Name)
                .Where(c => c.LastName == customer.LastName)
                .Where(c => c.Phone == customer.Phone)
                .FirstOrDefault();
                if(checkCustomer == null)
                {
                    _context.Customers.Add(customer);
                }
            }
            _context.SaveChanges();
        }
        public string DeleteCustomer(int id)
        {
            string message;
            var removableCustomer = _context.Customers.FirstOrDefault(c => c.Id == id);
                
            if(removableCustomer == null)
            {
                message = null;
                return message;
            }
            
            _context.Customers.Remove(removableCustomer);
            _context.SaveChanges();
            message = "Пользователь удален";
            return message;
        }
        public void DeleteManyCustomers(int[] customersId)
        {
            foreach (var cId in customersId)
            {
                var removableCustomer = _context.Customers.FirstOrDefault(c => c.Id == cId);
                _context.Customers.Remove(removableCustomer);
            }
            _context.SaveChanges();
        }
        
        public string EditCustomer(CustomerDTO customer)
        {
            string message;
            var infoCustomer = _context.Customers
            .FirstOrDefault(c => c.Id == customer.Id);
            if (infoCustomer == null)
            {
                message = null;
                return message;
            }
            if(infoCustomer.Name == customer.Name &&
            infoCustomer.LastName == customer.LastName &&
            infoCustomer.Phone == customer.Phone &&
            infoCustomer.Discount == customer.Discount)
            {
                message = "что изменять то?";
                return message;
            }
            
            infoCustomer.Name = customer.Name;
            infoCustomer.LastName = customer.LastName;
            infoCustomer.Phone = customer.Phone;
            infoCustomer.Discount = customer.Discount;
            _context.SaveChanges();
            message = "Пользователь изменен";
            return message;
        }

    }
}
