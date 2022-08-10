using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Web_api_pizza.Filters;
using Web_api_pizza.Storage;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.Services
{
    public interface ICustomerService
    {
        /// <summary>
        /// Получение списка пользователей
        /// </summary>
        /// <param name="filter">Опциональный параметр с фильтрами по имени, фамилии, телефону</param>
        /// <see cref="CustomerFilter"/>
        /// <returns>Список пользователей</returns>
        public List<CustomerDTO> GetAllCustomers(CustomerFilter filter);

        /// <summary>
        /// Получение личных данных пользователя
        /// </summary>
        /// <param name="id"> Id пользователя</param>
        /// <returns>Личные данные конкретного пользователя</returns>
        public PersonDTO GetOneCustomer(int id);

        /// <summary>
        /// Получение данных пользователя, включая данные заказов и адреса доставки
        /// </summary>
        /// <param name="id">Id пользователя</param>
        /// <returns>Данные конкретного пользователя</returns>
        public CustomerDTO GetCustomerWithAllInfo(int id);

        /// <summary>
        /// Добавление пользователя в БД
        /// </summary>
        /// <param name="customer">Пользователь в виде объекта CustomerDTO</param>
        /// <see cref="CustomerDTO"/>
        /// <returns>Результат операции в виде объекта OperationResult(bool результат операции , сообщение)</returns>
        public OperationResult RegistrationCustomer(CustomerDTO customer);

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="id">Id пользователя</param>
        /// <returns>Результат операции в виде объекта OperationResult(bool результат операции , сообщение)</returns>
        public OperationResult DeleteCustomer(int id);

        /// <summary>
        /// Изменение данных пользователя
        /// </summary>
        /// <param name="customer">Пользователь в виде объекта CustomerDTO</param>
        /// <see cref="CustomerDTO"/>
        /// <returns>Результат операции в виде объекта OperationResult(bool результат операции , сообщение)</returns>
        public OperationResult EditCustomer(CustomerDTO customer);

        /// <summary>
        /// Создание связи пользователь-адрес в БД
        /// </summary>
        /// <param name="customerId">Id пользователя</param>
        /// <param name="address">Адрес в виде объекта AddressDTO</param>
        /// <see cref="AddressDTO"/>
        /// <returns>Результат операции в виде объекта OperationResult(bool результат операции , сообщение)</returns>
        public OperationResult CreateCustomerAddress(int customerId, AddressDTO address);

        /// <summary>
        /// Удаление связи пользователь-адрес в БД
        /// </summary>
        /// <param name="customerId">Id пользователя</param>
        /// <param name="addressId">Id адреса</param>
        /// <returns>Результат операции в виде объекта OperationResult(bool результат операции , сообщение)</returns>
        public OperationResult RemoveCustomerAddress(int customerId, int addressId);

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

        public OperationResult RegistrationCustomer(CustomerDTO customer)
        {
            customer.Id = 0;
            if (customer.Discount == null)
            {
                customer.Discount = 0;
            }
            
            var checkCustomer = _context.Customers
                .FirstOrDefault(c => c.Name == customer.Name
                    && c.LastName == customer.LastName
                    && c.Phone == customer.Phone);

            var result = new OperationResult(false);
            if(checkCustomer != null)
            {
                result.Message = "Пользователь уже существует";
                return result;
            }

            var customerEntity = _mapper.Map<CustomerDTO, CustomerEntity>(customer);
            _context.Customers.Add(customerEntity);

            _context.SaveChanges();
            result.IsSuccess = true;
            result.Message = "Пользователь зарегистрирован";
            
            return result;
        }

        public OperationResult DeleteCustomer(int id)
        {
            var removableCustomer = _context.Customers
                .FirstOrDefault(c => c.Id == id);

            if (removableCustomer == null)
            {
                return null;
            }

            _context.Customers.Remove(removableCustomer);
            _context.SaveChanges();
            var result = new OperationResult(true, "Пользователь удален");
            return result;
        }


        public OperationResult EditCustomer(CustomerDTO customer)
        {
            
            var infoCustomer = _context.Customers
            .FirstOrDefault(c => c.Id == customer.Id);
            if (infoCustomer == null)
            {
                return null;
            }

            var result = new OperationResult(false);

            if (infoCustomer.Name == customer.Name &&
            infoCustomer.LastName == customer.LastName &&
            infoCustomer.Phone == customer.Phone &&
            infoCustomer.Discount == customer.Discount)
            {
                result.Message = "Нет данных для изменения";
                return result;
            }

            infoCustomer.Name = customer.Name;
            infoCustomer.LastName = customer.LastName;
            infoCustomer.Phone = customer.Phone;
            infoCustomer.Discount = customer.Discount;

            _context.SaveChanges();

            result.IsSuccess = true;
            result.Message = "Данные пользователя изменены";
            return result;
        }

        public OperationResult CreateCustomerAddress(int customerId, AddressDTO address)
        {
            var result = new OperationResult(false);

            var customerEntity = _context.Customers
                .FirstOrDefault(c => c.Id == customerId);
            if(customerEntity == null)
            {
                result.Message = "Пользователь не найден";
                return result;
            }
            var findAddress = FindAddress(address);
            if(findAddress == null)
            {
                result.Message = "Адрес не найден";
                return result;
            }

            var checkCustomerAddress = CheckCustomerAddress(customerId, findAddress.Id);

            if(checkCustomerAddress != null)
            {
                result.Message = "Связь между пользователем и адресом уже существует";
                return result;
            }

            var customerAddress = new CustomerAddressEntity { AddressEntityId = findAddress.Id, CustomerEntityId = customerId };
            _context.CustomerAddressEntities.Add(customerAddress);
            _context.SaveChanges();

            result.IsSuccess = true;
            result.Message = "Связь между пользователем и адресом создана";
            
            return result;
        }

        public OperationResult RemoveCustomerAddress(int customerId, int addressId)
        {
            var checkCustomerAddress = CheckCustomerAddress(customerId, addressId);

            if(checkCustomerAddress == null)
            {
                return null;
            }

            _context.CustomerAddressEntities.Remove(checkCustomerAddress);
            _context.SaveChanges();

            var result = new OperationResult(true, "Связь удалена");
            return result;
        }

        private AddressEntity FindAddress(AddressDTO address)
        {
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
