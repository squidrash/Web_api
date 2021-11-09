using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web_api_pizza.Services;
using Web_api_pizza.Storage.DTO;

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

        [HttpGet("{id}")]
        public IActionResult GetAddress(int id)
        {
            if (id <= 0)
            {
                return BadRequest($"Недопустимое значение Id - \"{id}\"");
            }
            var address = _addressService.GetDeliveryAddress(id);
            if (address == null)
            {
                return NotFound($"Адрес не найден - \"{id}\"");
            }
            return Ok(address);
        }

        
        [HttpPost("Create")]
        public IActionResult CreateAddress(AddressDTO address)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var message = _addressService.CreateDeliveryAddress(address);
            return Ok(message);
        }

        [HttpPut("Edit")]
        public IActionResult EditAddress(AddressDTO address)
        {
            if(address.Id <=0)
            {
                return BadRequest($"Недопустимое значение Id - \"{address.Id}\"");
            }
            var message = _addressService.EditDeliveryAddress(address);
            if(message == null)
            {
                return NotFound($"Адрес не найден Id - \"{address.Id}\"");
            }
            return Ok(message);
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult RemoveAddress(int id)
        {
            if (id <= 0)
            {
                return BadRequest($"Недопустимое значение Id - \"{id}\"");
            }
            var message =  _addressService.RemoveDeliveryAddress(id);
            if (message == null)
            {
                return NotFound($"Адрес найден Id - \"{id}\"");
            }
            return Ok(message);
        }
    }
}
