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

        public void RegistrationCustomer(CustomerDTO customer);

        //Регистируем сразу несколько пользователей
        //public void RegistrationManyCustomers(List<CustomerDTO> customers)

        public void DeleteCustomer(int id);

        //удалить сразу несколько
        //public void DeleteManyCustomers(int[] customersId);

        public void EditCustomer(CustomerDTO customer);
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
            return customerDTO;
        }
        public void RegistrationCustomer(CustomerDTO customer)
        {
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
            }
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
        public void DeleteCustomer(int id)
        {
            var removableCustomer = _context.Customers.FirstOrDefault(c => c.Id == id);
            _context.Customers.Remove(removableCustomer);
            _context.SaveChanges();
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
        public void EditCustomer(CustomerDTO customer)
        {
            var infoCustomer = _context.Customers
                .FirstOrDefault(c => c.Id == customer.Id);

            infoCustomer.Name = customer.Name;
            infoCustomer.LastName = customer.LastName;
            infoCustomer.Phone = customer.Phone;
            infoCustomer.Discount = customer.Discount;

            _context.SaveChanges();
        }

    }
}
