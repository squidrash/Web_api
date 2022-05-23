using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Web_api_pizza.Filters;
using Web_api_pizza.Services;
using Web_api_pizza.Storage.DTO;

namespace Web_api_pizza.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("getAll")]
        public IActionResult GetAllCustomers([FromQuery] CustomerFilter filter)
        {
            var customers =_customerService.GetAllCustomers(filter);
            return Ok(customers);
        }

        [HttpGet("getOne/{id}")]
        public IActionResult GetoneCustomers(int id)
        {
            if (id <= 0)
            {
                return BadRequest($"Недопустимое значение Id - \"{id}\"");
            }
            var customer = _customerService.GetOneCustomer(id);
            if(customer == null)
            {
                return NotFound($"Пользователь не нейден. Id - \"{id}\"");
            }
            return Ok(customer);
        }
        // попозже разобраться как получать все в ДТО
        [HttpGet("getOneWithInfo/{id}")]
        public IActionResult GetOneWithInfo(int id)
        {
            if (id <= 0)
            {
                return BadRequest($"Недопустимое значение Id - \"{id}\"");
            }
            var customer = _customerService.GetCustomerWithAllInfo(id);
            if (customer == null)
            {
                return NotFound($"Пользователь не нейден. Id - \"{id}\"");
            }
            
            return Ok(customer);
        }

        [HttpPost("registration")]
        public IActionResult RegistrationOneCustomer(CustomerDTO customer)
        {
            var mess = _customerService.RegistrationCustomer(customer);
            return Ok(mess);
        }

        [HttpPut("edit")]
        public IActionResult EditCustomer(CustomerDTO customer)
        {
            if(customer.Id <= 0)
            {
                ModelState.AddModelError("Id", $"Недопустимое значение Id - \"{customer.Id}\"");
            }
            if (customer.Discount < 0 || customer.Discount > 20)
            {
                ModelState.AddModelError("Id", $"Недопустимое значение Discount - \"{customer.Discount}\"");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var  message =_customerService.EditCustomer(customer);
            if(message == null)
            {
                return NotFound($"Пользователь не найден Id - \"{customer.Id}\"");
            }
            return Ok(message);
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            if(id <= 0)
            {
                ModelState.AddModelError("Id", $"Недопустимое значение Id - \"{id}\"");
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var message = _customerService.DeleteCustomer(id);
            if(message == null)
            {
                return NotFound("Пользователь не найден");
            }
            return Ok(message);
        }

        [HttpPost("createConnection")]
        public IActionResult CreateCustomerAddress(int customerId, AddressDTO address)
        {
            if (customerId <= 0)
            {
                return BadRequest($"Недопустимое значение Id пользователя - \"{customerId}\"");
            }
            var message = _customerService.CreateCustomerAddress(customerId, address);
            if(message == null)
            {
                return BadRequest("Адрес не найден");
            }
            return Ok(message);
        }
        [HttpDelete("removeConnection")]
        public IActionResult RemoveCustomerAddress(int customerId, int addressId)
        {
            if (customerId <= 0)
            {
                ModelState.AddModelError("customerId", $"Недопустимое значение Id пользователя - \"{customerId}\"");
            }
            if (addressId <= 0)
            {
                ModelState.AddModelError("addressId", $"Недопустимое значение Id адреса - \"{addressId}\"");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var message = _customerService.RemoveCustomerAddress(customerId, addressId);
            if(message == null)
            {
                return BadRequest($" Связь между клиентом с ID-{customerId} и адресом с ID-{addressId} не найдена");
            }
            return Ok(message);

        }
        [HttpPut("editConnection")]
        public IActionResult EditCustomerAddress(int customerId, int oldAddressId, AddressDTO newAddress)
        {
            if (customerId <= 0)
            {
                ModelState.AddModelError("customerId", $"Недопустимое значение Id пользователя - \"{customerId}\"");
            }
            if (oldAddressId <= 0)
            {
                ModelState.AddModelError("addressId", $"Недопустимое значение Id адреса - \"{oldAddressId}\"");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var message = _customerService.EditCustomerAddress(customerId, oldAddressId, newAddress);
            if(message == "nullNewAddress")
            {
                return BadRequest("Адрес не найден");
            }
            if(message == "nullCustomerAddress")
            {
                return BadRequest($" Связь между клиентом с ID-{customerId} и адресом с ID-{oldAddressId} не найдена");
            }
            return Ok(message);
        }
    }
}
