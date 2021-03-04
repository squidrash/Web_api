using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetAllCustomers()
        {
            var customers =_customerService.GetAllCustomers();
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
        public string EditCustomer(CustomerDTO customer)
        {
             var  message =_customerService.EditCustomer(customer);
            return message;
        }

        [HttpDelete("delete/{id}")]
        public string DeleteCustomer(int id)
        {
            _customerService.DeleteCustomer(id);
            return "Пользователь удален";
        }
    }
}
