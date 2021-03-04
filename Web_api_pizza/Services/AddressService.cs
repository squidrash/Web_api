using System;
using System.Linq;
using AutoMapper;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.Services
{ 
    public interface IAddressService
    {
        public AddressDTO GetDeliveryAddress(int id);
        public string CreateDeliveryAddress(AddressDTO address, int customerId = 0);
        public string EditDeliveryAddress(AddressDTO address);
        public string RemoveDeliveryAddress(int id);
    }
    public class AddressService : IAddressService
    {
        private readonly IMapper _mapper;
        private readonly PizzaDbContext _context;
        public AddressService(PizzaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public AddressDTO GetDeliveryAddress(int id)
        {
                var addressEntity = _context.Addresses.FirstOrDefault(a => a.Id == id);
                var addressDTO = _mapper.Map<AddressEntity, AddressDTO>(addressEntity);
                return addressDTO;
        }
        public string CreateDeliveryAddress(AddressDTO address, int customerId = 0)
        {
            address.Id = 0;
            string message;
            var checkAddress = FindAddress(address);
            if (checkAddress == null)
            {
                var addressEntity = _mapper.Map<AddressDTO, AddressEntity>(address);
                _context.Addresses.Add(addressEntity);
                _context.SaveChanges();
                message = "Адрес добавлен";
            }
            //ненужное сообщение для клиента
            //только для проверки
            //удалить после тестирования
            else
            { 
                message = "Адрес уже есть в базе";
            }
            if(customerId != 0)
            {
                var findAddress = FindAddress(address);
                var checkCustomerAddress = _context.CustomerAddressEntities
                    .Where(c => c.CustomerEntityId == customerId && c.AddressEntityId == findAddress.Id)
                    .FirstOrDefault();
                if(checkCustomerAddress == null)
                {
                    var customerAddress = new CustomerAddressEntity { AddressEntityId = findAddress.Id, CustomerEntityId = customerId };
                    _context.CustomerAddressEntities.Add(customerAddress);
                    _context.SaveChanges();
                    //ненужное сообщение для клиента
                    //только для проверки
                    //удалить после тестирования
                    message += "\nСвязь между пользователем и адресом создана";
                }
                //ненужное сообщение для клиента
                //только для проверки
                //удалить после тестирования
                else
                {
                    message += "\nСвязь между пользователем и адресом уже существует";
                }
            }
            return message;
        }
        private AddressEntity FindAddress(AddressDTO address)
        {
            var findAddress = _context.Addresses
                .Where(a => a.City == address.City)
                .Where(a => a.Street == address.Street)
                .Where(a => a.NumberOfBuild == address.NumberOfBuild)
                .Where(a => a.NumberOfEntrance == address.NumberOfEntrance)
                .Where(a => a.Apartment == address.Apartment)
                .FirstOrDefault();
            return findAddress;
        }
        public string EditDeliveryAddress(AddressDTO address)
        {
            string message;
            var editableAddress = _context.Addresses.FirstOrDefault(a => a.Id == address.Id);
            if(editableAddress == null)
            {
                message = null;
                return message;
            }
            if(editableAddress.City == address.City &&
            editableAddress.Street == address.Street &&
            editableAddress.NumberOfBuild == address.NumberOfBuild &&
            editableAddress.NumberOfEntrance == address.NumberOfEntrance &&
            editableAddress.Apartment == address.Apartment)
            {
                message = "что изменять то?";
                return message;
            }
            editableAddress.City = address.City;
            editableAddress.Street = address.Street;
            editableAddress.NumberOfBuild = address.NumberOfBuild;
            editableAddress.NumberOfEntrance = address.NumberOfEntrance;
            editableAddress.Apartment = address.Apartment;

            _context.SaveChanges();
            message = "Адрес изменен";
            return message;
        }
        public string RemoveDeliveryAddress(int id)
        {
            var removableAddress = _context.Addresses.FirstOrDefault(a => a.Id == id);
            string message;
            if(removableAddress == null)
            {
                message = null;
                return message;
            }
            _context.Addresses.Remove(removableAddress);
            _context.SaveChanges();
            message = "Адрес удален";
            return message;
        }
    }
}
