using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Web_api_pizza.Filters;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.Services
{
    public interface ICustomerService
    {
        public List<CustomerDTO> GetAllCustomers(CustomerFilter filter);
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

        public string CreateCustomerAddress(int customerId, AddressDTO address);
        public string EditCustomerAddress(int customerId, int oldAddressId, AddressDTO newAddress);
        public string RemoveCustomerAddress(int customerId, int addressId);

    }
    public class CustomerService : ICustomerService
    {
        private readonly IMapper _mapper;
        private readonly PizzaDbContext _context;
        private readonly IAddressService _addressService;
        public CustomerService(PizzaDbContext context, IMapper mapper,
            IAddressService addressService)
        {
            _context = context;
            _mapper = mapper;
            _addressService = addressService;
        }
        public List<CustomerDTO> GetAllCustomers(CustomerFilter filter)
        {
            var customers = _context.Customers
                .OrderBy(c => c.Name)
                .ThenBy(c => c.LastName)
                .AsQueryable();

            customers = filter.Filters(customers);
            var customersEntity = customers.ToList();

            var customersDTO = _mapper.Map<List<CustomerEntity>, List<CustomerDTO>>(customersEntity);
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
                    .ThenInclude(ce => ce.Order)
                        
                        .ThenInclude(o => o.Products)
                            .ThenInclude(p => p.Dish)
                            

                .Include(c => c.Orders)
                    .ThenInclude(ce => ce.Order)
                        .ThenInclude(ae => ae.AddressOrder)
                            .ThenInclude(ad => ad.Address)

                .Include(c => c.Addresses)
                    .ThenInclude(a => a.Address)
                .FirstOrDefault();
            var customerDTO = _mapper.Map<CustomerEntity, CustomerDTO>(customerEntity);
            return customerDTO;
        }

        public string RegistrationCustomer(CustomerDTO customer)
        {
            customer.Id = 0;
            if (customer.Discount == null)
            {
                customer.Discount = 0;
            }
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
                if (checkCustomer == null)
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

            if (removableCustomer == null)
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
            if (infoCustomer.Name == customer.Name &&
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

        public string CreateCustomerAddress(int customerId, AddressDTO address)
        {
            string message;
            var findAddress = FindAddress(address);
            if(findAddress == null)
            {
                message = null;
                return message;
            }

            var checkCustomerAddress = CheckCustomerAddress(customerId, findAddress.Id);
            
            if (checkCustomerAddress == null)
            {
                var customerAddress = new CustomerAddressEntity { AddressEntityId = findAddress.Id, CustomerEntityId = customerId };
                _context.CustomerAddressEntities.Add(customerAddress);
                _context.SaveChanges();
                //ненужное сообщение для клиента
                //только для проверки
                //удалить после тестирования
                message = "Связь между пользователем и адресом создана";
            }
            //ненужное сообщение для клиента
            //только для проверки
            //удалить после тестирования
            else
            {
                message = "Связь между пользователем и адресом уже существует";
            }
            return message;
            
        }

        public string RemoveCustomerAddress(int customerId, int addressId)
        {
            string message;
            var checkCustomerAddress = CheckCustomerAddress(customerId, addressId);
            if(checkCustomerAddress == null)
            {
                message = null;
                return message;
            }
            _context.CustomerAddressEntities.Remove(checkCustomerAddress);
            _context.SaveChanges();
            message = "Связь удалена";
            return message;
        }

        public string EditCustomerAddress(int customerId, int oldAddressId, AddressDTO newAddress)
        {
            string message;
            var findAddress = FindAddress(newAddress);
            if (findAddress == null)
            {
                message = "nullNewAddress";
                return message;
            }

            var checkCustomerAddress = CheckCustomerAddress(customerId, oldAddressId);

            if (checkCustomerAddress == null)
            {
                message = "nullCustomerAddress";
                return message;
            }
            checkCustomerAddress.AddressEntityId = findAddress.Id;
            _context.SaveChanges();
            message = "Адрес изменен";
            return message;
        }

        private AddressEntity FindAddress(AddressDTO address)
        {
            //нужно ли вызывать этот метод из сервиса
            //или лучше прописать локально?
            var findAddress = _addressService.FindAddress(address);
            return findAddress;
        }
        private CustomerAddressEntity CheckCustomerAddress(int customerId, int addressId)
        {
            var checkCustomerAddress = _context.CustomerAddressEntities
                .Where(c => c.CustomerEntityId == customerId && c.AddressEntityId == addressId)
                .FirstOrDefault();
            return checkCustomerAddress;
        }

    }
}
