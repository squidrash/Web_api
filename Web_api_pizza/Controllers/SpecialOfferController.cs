using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Web_api_pizza.Filters;
using Web_api_pizza.Services;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Enums;

namespace Web_api_pizza.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialOfferController : ControllerBase
    {
        private readonly ISpecialOfferService _specialOfferService;
        public SpecialOfferController(ISpecialOfferService specialOfferService)
        {
            _specialOfferService = specialOfferService;
        }

        [HttpGet("all")]
        public IActionResult GetAllSpecialOffers([FromQuery] SpecialOfferFilter filter)
        {
            var orders = _specialOfferService.GetAllSpecialOffers(filter);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public IActionResult GetSpecialOffer(int id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError("Id", $"Недопустимое значение Id - \"{id}\"");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var offer = _specialOfferService.GetSpecialOffer(id);

            if (offer == null)
            {
                return NotFound($"Специальное предложение не найдено. Id - \"{id}\"");
            }
            return Ok(offer);
        }

        [HttpPost("newOffer")]
        public IActionResult AddNewSpecialOffer(SpecialOfferDTO specialOffer)
        {
            if(Enum.IsDefined<TypeOfferEnum>(specialOffer.TypeOffer) == false)
            {
                ModelState.AddModelError("TypeOffer", "Не указан тип акции");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _specialOfferService.AddNewSpecialOffer(specialOffer);
            if (result.IsSuccess == false)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpPut("edit")]
        public IActionResult EditCustomer(SpecialOfferDTO specialOffer)
        {
            if (specialOffer.Id <= 0)
            {
                ModelState.AddModelError("Id", $"Недопустимое значение Id - \"{specialOffer.Id}\"");
            }
            if (Enum.IsDefined<TypeOfferEnum>(specialOffer.TypeOffer) == false)
            {
                ModelState.AddModelError("TypeOffer", "Не указан тип акции");
            }
            if (specialOffer.Description == null)
            {
                ModelState.AddModelError("Description", "Поле Description не может быть пустым");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _specialOfferService.EditSpecialOffer(specialOffer);
            if (result == null)
            {
                return NotFound($"Специальное предложение не найдено. Id - \"{specialOffer.Id}\"");
            }
            if(result.IsSuccess == false)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteSpecialOffer(int id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError("Id", $"Недопустимое значение Id - \"{id}\"");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var message = _specialOfferService.DeleteSpecialOffer(id);
            if (message == null)
            {
                return NotFound("Акция не найдена");
            }
            return Ok(message);
        }
        [HttpGet("checkOffer")]
        public IActionResult CheckComplianceSpecialOffer(List<DishDTO> dishes, string promoCode)
        {
            if (promoCode == null)
            {
                ModelState.AddModelError("PromoCode", $"Не указан промокод");
            }
            if (dishes == null)
            {
                ModelState.AddModelError("Dishes", $"Не указаны блюда для проверки соответствия акции");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _specialOfferService.CheckComplianceSpecialOffer(dishes, promoCode);
            if(result.IsSuccess == false)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }
    }
}
