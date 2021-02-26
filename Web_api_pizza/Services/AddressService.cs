using System;
using System.Linq;
using AutoMapper;
using CreateDb.Storage;
using CreateDb.Storage.DTO;
using CreateDb.Storage.Models;

namespace Web_api_pizza.Services
{ 
    public interface IAddressService
    {
        public AddressDTO GetDeliveryAddress(int id);
        public void CreateDeliveryAddress(AddressDTO address, int customerId = 0);
        public void EditDeliveryAddress(AddressDTO address);
        public void RemoveDeliveryAddress(int id);
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
            //try
            //{
                var addressEntity = _context.Addresses.FirstOrDefault(a => a.Id == id);
                var addressDTO = _mapper.Map<AddressEntity, AddressDTO>(addressEntity);
                return addressDTO;
            //}
            //catch
            //{
            //    return "Неверные данные пользователя";
            //}
        }
        public void CreateDeliveryAddress(AddressDTO address, int customerId = 0)
        {
            var checkAddress = FindAddress(address);
            if (checkAddress == null)
            {
                var addressEntity = _mapper.Map<AddressDTO, AddressEntity>(address);
                _context.Addresses.Add(addressEntity);
                _context.SaveChanges();
            }
            if(customerId != 0)
            {
                var findAddress = FindAddress(address);
                var checkCustomerAddress = _context.CustomerAddressEntities
                    .Where(c => c.CustomerEntityId == customerId && c.AddressEntityId == address.Id)
                    .FirstOrDefault();
                if(checkCustomerAddress == null)
                {
                    var customerAddress = new CustomerAddressEntity { AddressEntityId = address.Id, CustomerEntityId = customerId };
                    _context.CustomerAddressEntities.Add(customerAddress);
                    _context.SaveChanges();
                }
            }
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
        public void EditDeliveryAddress(AddressDTO address)
        {
            var editableAddress = _context.Addresses.FirstOrDefault(a => a.Id == address.Id);

            editableAddress.City = address.City;
            editableAddress.Street = address.Street;
            editableAddress.NumberOfBuild = address.NumberOfBuild;
            editableAddress.NumberOfEntrance = address.NumberOfEntrance;
            editableAddress.Apartment = address.Apartment;

            _context.SaveChanges();
        }
        public void RemoveDeliveryAddress(int id)
        {
            var removableAddress = _context.Addresses.FirstOrDefault(a => a.Id == id);
            _context.Addresses.Remove(removableAddress);
            _context.SaveChanges();
        }
    }
}
