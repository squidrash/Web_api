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

        /// <summary>
        /// Получение списка пользователей
        /// </summary>
        /// <param name="filter">Опциональный параметр с фильтрами по имени, фамилии, номеру телефона</param>
        /// <response code="200">Список пользователей</response>
        [HttpGet("getAll")]
        public IActionResult GetAllCustomers([FromQuery] CustomerFilter filter)
        {
            var customers =_customerService.GetAllCustomers(filter);
            return Ok(customers);
        }

        /// <summary>
        /// Получение личных данных пользователя
        /// </summary>
        /// <param name="id"> Id пользователя</param>
        /// <response code="200">Личные данные конкретного пользователя</response>
        /// <response code="400">Недопустимое значение Id пользователя</response>
        /// <response code="404">Пользователь не найден</response>
        [HttpGet("getOne/{id}")]
        public IActionResult GetOneCustomers(int id)
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

        /// <summary>
        /// Получение данных пользователя, включая данные заказов и адреса доставки
        /// </summary>
        /// <param name="id">Id пользователя</param>
        /// <response code="200">Данные пользователя</response>
        /// <response code="400">Недопустимое значение Id пользователя</response>
        /// <response code="404">Пользователь не найден</response>
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

        /// <summary>
        /// Добавление пользователя в БД
        /// </summary>
        /// <param name="customer">Пользователь в виде объекта CustomerDTO</param>
        /// <see cref="CustomerDTO"/>
        /// <response code="200">Пользователь зарегистрирован</response>
        /// <response code="400">Пользователь уже существует</response>
        [HttpPost("registration")]
        public IActionResult RegistrationOneCustomer(CustomerDTO customer)
        {
            var result = _customerService.RegistrationCustomer(customer);
            if(result.IsSuccess == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Изменение данных пользователя
        /// </summary>
        /// <param name="customer">Пользователь в виде объекта CustomerDTO</param>
        /// <see cref="CustomerDTO"/>
        /// <response code="200">Данные пользователя изменены</response>
        /// <response code="400">Ошибки связаные с недопустимыми значениями полей Id, Discount или отсутвием данных для изменения</response>
        /// <response code="404">Пользователь не найден</response>

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

            var  result =_customerService.EditCustomer(customer);
            if(result == null)
            {
                return NotFound($"Пользователь не найден Id - \"{customer.Id}\"");
            }
            if(result.IsSuccess == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="id">Id пользователя</param>
        /// <response code="200">Пользователь удален</response>
        /// <response code="400">Недопустимое значение Id пользователя</response>
        /// <response code="404">Пользователь не найден</response> 
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
            var result = _customerService.DeleteCustomer(id);
            if(result == null)
            {
                return NotFound("Пользователь не найден");
            }
            return Ok(result);
        }

        /// <summary>
        /// Создание связи пользователь-адрес в БД
        /// </summary>
        /// <param name="customerId">Id пользователя</param>
        /// <param name="address">Адрес в виде объекта AddressDTO</param>
        /// <see cref="AddressDTO"/>
        /// <response code="200">Связь пользователь-адрес создана</response>
        /// <response code="400">Ошибки связаные с недопустимыми значениями поля customerId,
        /// неверно указаным адресом и Id пользователя,
        /// а также уже существующей связи пользователь-адрес</response>
        [HttpPost("createConnection")]
        public IActionResult CreateCustomerAddress(int customerId, AddressDTO address)
        {
            if (customerId <= 0)
            {
                return BadRequest($"Недопустимое значение Id пользователя - \"{customerId}\"");
            }

            var result = _customerService.CreateCustomerAddress(customerId, address);
            if (result.IsSuccess == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Удаление связи пользователь-адрес в БД
        /// </summary>
        /// <param name="customerId">Id пользователя</param>
        /// <param name="addressId">Id адреса</param>
        /// <response code="200">Связь пользователь-адрес удалена</response>
        /// <response code="400"Ошибки связаные с недопустимыми значениями Id пользователя и адреса</response>
        /// <response code="404">Связь пользователь-адрес не найдена</response>
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
            var result = _customerService.RemoveCustomerAddress(customerId, addressId);
            if(result == null)
            {
                return NotFound($" Связь между пользователем с ID-{customerId} и адресом с ID-{addressId} не найдена");
            }
            return Ok(result);

        }
    }
}
