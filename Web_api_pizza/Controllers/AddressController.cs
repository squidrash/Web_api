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

        /// <summary>
        /// Получение адреса по Id
        /// </summary>
        /// <param name="id"> Id адреса</param>
        /// <response code="200">Конкретный адрес</response>
        /// <response cpdle="400">Неверно указан Id адреса </response>
        /// <response code="404">Адрес не найден</response>
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

        /// <summary>
        /// Поиск адреса по городу, улице, дому...
        /// </summary>
        /// <param name="address">адрес в виде объекта класса AddressDTO</param>
        /// <see cref="AddressDTO"/>
        /// <response code="200">Конкретный адрес</response>
        /// <response code="400">Неверно указаны данные адреса</response>
        /// <response code="404">Адрес не найден</response>
        [HttpPost("find")]
        public IActionResult FindAddress(AddressDTO address)
        {
            if(address.City == null)
            {
                ModelState.AddModelError("City", "Не указан город");

            }
            if (address.Street == null)
            {
                ModelState.AddModelError("Street", "Не указана улица");
            }
            if (address.NumberOfBuild == null)
            {
                ModelState.AddModelError("Build", "Не указан дом");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _addressService.FindAddress(address);

            if(result == null)
            {
                return NotFound("Адрес не найден");
            }
            
            return Ok(result);
        }

        /// <summary>
        /// Добавление адреса в БД
        /// </summary>
        /// <param name="address">адрес в виде объекта класса AddressDTO</param>
        /// <see cref="AddressDTO"/>
        /// <response code="200">Сообщение о результате операции(адрес добавлен)</response>
        /// <response code="400">Адрес уже есть в базе</response>
        [HttpPost("Create")]
        public IActionResult CreateAddress(AddressDTO address)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _addressService.CreateDeliveryAddress(address);
            if(result.IsSuccess == false)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Измение адреса 
        /// </summary>
        /// <param name="address">адрес в виде объекта класса AddressDTO</param>
        /// <see cref="AddressDTO"/>
        /// <response code="200">Адрес изменен</response>
        /// <response code="400">Неверно указаны даннные адреса</response>
        /// <response code="404">Адрес не найден</response>
        [HttpPut("Edit")]
        public IActionResult EditAddress(AddressDTO address)
        {
            if(address.Id <=0)
            {
                return BadRequest($"Недопустимое значение Id - \"{address.Id}\"");
            }
            var result = _addressService.EditDeliveryAddress(address);
            if(result == null)
            {
                return NotFound($"Адрес не найден Id - \"{address.Id}\"");
            }
            if(result.IsSuccess == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Удалить адрес из БД
        /// </summary>
        /// <param name="id">Id адреса</param>
        /// <response code="200">Адрес удален</response>
        /// <response code="400">Неверно указан Id адреса</response>
        /// <response code="404">Адрес не найден</response>
        [HttpDelete("Delete/{id}")]
        public IActionResult RemoveAddress(int id)
        {
            if (id <= 0)
            {
                return BadRequest($"Недопустимое значение Id - \"{id}\"");
            }
            var result =  _addressService.RemoveDeliveryAddress(id);
            if (result == null)
            {
                return NotFound($"Адрес найден Id - \"{id}\"");
            }
            return Ok(result);
        }
    }
}
