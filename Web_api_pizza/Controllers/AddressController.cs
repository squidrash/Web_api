using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreateDb.Storage.DTO;
using Microsoft.AspNetCore.Mvc;
using Web_api_pizza.Services;

namespace Web_api_pizza.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("Get")]
        public AddressDTO GetAddress(int id)
        {
            var address = _addressService.GetDeliveryAddress(id);
            return address;
        }

        
        [HttpPost("Create")]
        public string CreateAddress(AddressDTO address, int customerId = 0)
        {
            _addressService.CreateDeliveryAddress(address, customerId);
            return "Успешно добавлен адрес";
        }

        [HttpPut("Edit")]
        public string EditAddress(AddressDTO address)
        {
            _addressService.EditDeliveryAddress(address);
            return "Адрес изменен";
        }

        [HttpDelete("Delete")]
        public string RemoveAddress(int id)
        {
            _addressService.RemoveDeliveryAddress(id);
            return "Адрес удален";
        }
    }
}
