using System;
using System.Linq;
using AutoMapper;
using Web_api_pizza.Storage;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.Services
{ 
    public interface IAddressService
    {
        /// <summary>
        /// Получение адреса по Id
        /// </summary>
        /// <param name="id">Id адреса</param>
        /// <returns>адрес в виде объекта класса AddressDTO</returns>
        /// <see cref="AddressDTO"/>
        public AddressDTO GetDeliveryAddress(int id);

        /// <summary>
        /// Добавление нового адреса в БД
        /// </summary>
        /// <param name="address">адрес в виде объекта класса AddressDTO</param>
        /// <returns>Результат операции в виде объекта OperationResult(bool результат операции , сообщение)</returns>
        /// <see cref="AddressDTO"/>
        public OperationResult CreateDeliveryAddress(AddressDTO address);

        /// <summary>
        /// Изменение данных адреса
        /// </summary>
        /// <param name="address">адрес в виде объекта класса AddressDTO</param>
        /// <returns>Результат операции в виде объекта OperationResult(bool результат операции , сообщение)</returns>
        /// <see cref="AddressDTO"/>
        public OperationResult EditDeliveryAddress(AddressDTO address);

        /// <summary>
        /// Удаление адреса из БД
        /// </summary>
        /// <param name="id">Id адреса</param>
        /// <returns>Результат операции в виде объекта OperationResult(bool результат операции , сообщение)</returns>
        public OperationResult RemoveDeliveryAddress(int id);

        /// <summary>
        /// Метод поиска адреса по городу, улице, дому...
        /// </summary>
        /// <param name="address">объект класса AddressDTO</param>
        /// <returns>адрес в виде объекта класса AddressDTO</returns>
        /// <see cref="AddressDTO"/>
        public AddressEntity FindAddress(AddressDTO address);
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
        
        public OperationResult CreateDeliveryAddress(AddressDTO address)
        {
            address.Id = 0;
            var result = new OperationResult(false);
            var checkAddress = FindAddress(address);
            if (checkAddress == null)
            {
                var addressEntity = _mapper.Map<AddressDTO, AddressEntity>(address);
                _context.Addresses.Add(addressEntity);
                _context.SaveChanges();
                result.IsSuccess = true;

                result.Message = "Адрес добавлен";
            }
            else
            { 
                result.Message = "Адрес уже есть в базе";
            }
            return result;
        }

                public AddressEntity FindAddress(AddressDTO address)
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

        public OperationResult EditDeliveryAddress(AddressDTO address)
        {
            var editableAddress = _context.Addresses.FirstOrDefault(a => a.Id == address.Id);
            if(editableAddress == null)
            {
                return null;
            }

            var result = new OperationResult(false);
            if(editableAddress.City == address.City &&
            editableAddress.Street == address.Street &&
            editableAddress.NumberOfBuild == address.NumberOfBuild &&
            editableAddress.NumberOfEntrance == address.NumberOfEntrance &&
            editableAddress.Apartment == address.Apartment)
            {
                result.Message = "Нет данных для изменения";
                return result;
            }
            editableAddress.City = address.City;
            editableAddress.Street = address.Street;
            editableAddress.NumberOfBuild = address.NumberOfBuild;
            editableAddress.NumberOfEntrance = address.NumberOfEntrance;
            editableAddress.Apartment = address.Apartment;

            _context.SaveChanges();

            result.IsSuccess = true;
            result.Message = "Адрес изменен";
            return result;
        }

        public OperationResult RemoveDeliveryAddress(int id)
        {
            var removableAddress = _context.Addresses.FirstOrDefault(a => a.Id == id);
            if(removableAddress == null)
            {
                return null;
            }
            _context.Addresses.Remove(removableAddress);
            _context.SaveChanges();
            var result = new OperationResult(true, "Адрес удален");
            return result;
        }
    }
}
