using System;
using System.Collections.Generic;
using CreateDb.Storage.DTO;
using Microsoft.AspNetCore.Mvc;
using Web_api_pizza.Services;

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
        public List<CustomerDTO> GetAllCustomers()
        {

            var customers =_customerService.GetAllCustomers();
            return customers;
        }

        [HttpGet("getOne")]
        public CustomerDTO GetoneCustomers(int id)
        {
            var customer = _customerService.GetOneCustomer(id);
            return customer;
        }

        [HttpGet("getOneWithInfo")]
        public CustomerDTO GetOneWithInfo(int id)
        {
            var customer = _customerService.GetCustomerWithAllInfo(id);
            return customer;
        }

        [HttpPost("registration")]
        public string RegistrationOneCustomer(CustomerDTO customer)
        {
            _customerService.RegistrationCustomer(customer);
            return "Успешно";
        }

        [HttpPut("edit")]
        public string EditCustomer(CustomerDTO customer)
        {
             var  message =_customerService.EditCustomer(customer);
            return message;
        }

        [HttpDelete("delete")]
        public string DeleteCustomer(int id)
        {
            _customerService.DeleteCustomer(id);
            return "Пользователь удален";
        }
    }
}
