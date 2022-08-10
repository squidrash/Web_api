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

        /// <summary>
        /// Получения списка акций
        /// </summary>
        /// <param name="filter">Опциональный параметр. Фильтрация по типу акции</param>
        /// <response code="200">Список акций</response>
        [HttpGet("all")]
        public IActionResult GetAllSpecialOffers([FromQuery] SpecialOfferFilter filter)
        {
            var orders = _specialOfferService.GetAllSpecialOffers(filter);
            return Ok(orders);
        }

        /// <summary>
        /// Получение данных конкретной акции
        /// </summary>
        /// <param name="id"> Id акции</param>
        /// <response code="200">Акцию со всеми данными</response>
        /// <response code="400">Неверно указан Id акции</response>
        /// <response code="404"> Акция не найдена</response>
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

        /// <summary>
        /// Добавление новой акции
        /// </summary>
        /// <param name="specialOffer">Данные акции</param>
        /// <response code="200">Акция добавлена</response>
        /// <response code="400">Ошибки связаные с неверно указаными данными акции</response>
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
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Изменение данных акции
        /// </summary>
        /// <param name="specialOffer">Данные акции</param>
        /// <response code="200">Акция изменена</response>
        /// <response code="400">Ошибки связаные с неверно указаными данными акции</response>
        /// <response code="404">Акция не найдена</response>
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
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Удаление акции
        /// </summary>
        /// <param name="specialOfferId">Id акции</param>
        /// <response code="200">Акция удалена</response>
        /// <response code="400">неверно указан Id акции</response>
        /// <response code="404">акция не найдена</response>
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
            var result = _specialOfferService.DeleteSpecialOffer(id);
            if (result == null)
            {
                return NotFound("Акция не найдена");
            }
            return Ok(result);
        }

        /// <summary>
        /// Проверка соответствия списка блюд условиям акции
        /// </summary>
        /// <param name="dishes">Список проверяемых блюд</param>
        /// <param name="promoCode">промокод акции по которой будет проводиться проверка</param>
        /// <response code="200">Результат проверки</response>
        /// <response code="400">Ошибки связаные с неверно указаынм промокодом, отсутсвием блюд для проверки соответсвия и несоответсвии условиям акции</response>
        [HttpPost("checkOffer")]
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
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
