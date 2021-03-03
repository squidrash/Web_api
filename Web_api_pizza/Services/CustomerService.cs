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
    public interface ICustomerService
    {
        public List<CustomerDTO> GetAllCustomers();
        public CustomerDTO GetOneCustomer(int id);

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
        public CustomerService(PizzaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
        public CustomerDTO GetOneCustomer(int id)
        {
            var customerEntity = _context.Customers.FirstOrDefault(c => c.Id == id);
            var customerDTO = _mapper.Map<CustomerEntity, CustomerDTO>(customerEntity);
            return customerDTO;
        }
        public CustomerDTO GetCustomerWithAllInfo(int id)
        {
                var customerEntity = _context.Customers
                    .Where(c => c.Id == id)
                    .Include(c => c.Orders)
                    .ThenInclude(o => o.Products)
                    .ThenInclude(p => p.Dish)
                    .Include(u => u.Addresses)
                    .ThenInclude(a => a.Address)
                    .FirstOrDefault(c => c.Id == id);
                var customerDTO = _mapper.Map<CustomerEntity, CustomerDTO>(customerEntity);
            //только для проверки потом удалить
            Console.WriteLine($"{customerEntity.Name} {customerEntity.LastName}");
            foreach(var o in customerEntity.Orders)
            {
                Console.WriteLine(o.Id);
                Console.WriteLine(o.CreatTime);
                Console.WriteLine(o.Status);
                foreach(var p in o.Products)
                {
                    Console.WriteLine($"{p.Dish.ProductName} - {p.Dish.Price}");
                }
            }
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
            try
            {
                string message;
                var removableCustomer = _context.Customers.FirstOrDefault(c => c.Id == id);
                
                if(removableCustomer == null)
                {
                    message = "Пользователь не найден";
                }
                else
                {
                    _context.Customers.Remove(removableCustomer);
                    _context.SaveChanges();
                    message = "Пользователь удален";
                }
                return message;
            }
            catch
            {
                return "Ошибка при удалении пользователя";
            }
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
            }
            else
            {
                infoCustomer.Name = customer.Name;
                infoCustomer.LastName = customer.LastName;
                infoCustomer.Phone = customer.Phone;
                infoCustomer.Discount = customer.Discount;
                _context.SaveChanges();
                message = "Пользователь изменен";
            }
            return message;
        }

    }
}
