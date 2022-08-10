using Microsoft.AspNetCore.Mvc;
using Web_api_pizza.Filters;
using Web_api_pizza.Services;
using Web_api_pizza.Storage.DTO;

namespace Web_api_pizza.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;
        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        /// <summary>
        /// Получения списка блюд
        /// </summary>
        /// <param name="filter">Опциональный параметр. Фильтр по названию блюд, категории</param>
        /// <see cref="DishFilter"/>
        /// <response code="200"> Список блюд</response>
        [HttpGet("fullmenu")]
        public IActionResult GetMenu([FromQuery] DishFilter filter)
        {
            var menu = _menuService.GetMenu(filter);
            return Ok(menu);
        }

        /// <summary>
        /// Получение данных одного блюда
        /// </summary>
        /// <param name="id">Id блюда</param>
        /// <response code="200">Данные блюда</response>
        /// <response code="400">Неверно указан Id блюда</response>
        /// <response code="404">Блюдо не найдено</response>
        [HttpGet("onedish/{id}")]
        public IActionResult GetDish(int id)
        {
            if(id <= 0)
            {
                return BadRequest($"Недопустимое значение Id - \"{id}\"");
            }
            var dish = _menuService.GetOneDish(id);
            if (dish == null)
            {
                return NotFound($"Блюдо не найдено - \"{id}\"");
            }
                return Ok(dish);
        }

        /// <summary>
        /// Добавление блюда в меню
        /// </summary>
        /// <param name="dish">Данные блюда</param>
        /// <response code="200">Результат операции в виде объекта OperationResult(результат bool, сообщение)</response>
        /// <response code="400">Неверно указаны данные блюда
        /// Блюдо уже есть в меню
        /// Неверно указана категория блюда</response>
        [HttpPost("add")]
        public IActionResult AddToMenu(DishDTO dish)
        {
            if (dish.Price <= 0)
            {
                ModelState.AddModelError("Price", $"Недопустимое значение цены - \"{dish.Price}\"");
            }

            if (dish.ProductName == null)
            {
                ModelState.AddModelError("ProductName", "Укажите блюдо");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _menuService.AddToMenu(dish);
            if(result.IsSuccess == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Изменение данных блюда.
        /// </summary>
        /// <param name="dish">Блюдо с измененными данными</param>
        /// <response code="200">Результат операции в виде объекта OperationResult(результат bool, сообщение)</response>
        /// <response code="400">Неверно указаны данные блюда</response>
        /// <response code="404">Блюдо не найдено</response>
        [HttpPut("edit")]
        public IActionResult Edit(DishDTO dish)
        {
            if(dish.Id <= 0)
            {
                ModelState.AddModelError("Id", $"Недопустимое значение Id - \"{dish.Id}\"");
            }
            if (dish.Price <= 0)
            {
                ModelState.AddModelError("Price", $"Недопустимое значение цены - \"{dish.Price}\"");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
                
            var result = _menuService.EditMenu(dish);
            if (result == null)
            {
                return NotFound($"Блюдо не найдено Id - \"{dish.Id}\"");
            }
            if(result.IsSuccess == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Удаление блюда из меню.
        /// </summary>
        /// <param name="id">Id блюда</param>
        /// <response code="200">Результат операции в виде объекта OperationResult(результат bool, сообщение)</response>
        /// <response code="400">Неверно указан Id блюда</response>
        /// <response code="404">Блюдо не найдено</response>
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest($"Недопустимое значение Id - \"{id}\"");
            }
            var result = _menuService.RemoveFromMenu(id);
            if(result == null)
            {
                return NotFound($"Блюдо не найдено Id - \"{id}\"");
            }
            return Ok(result);
            
        }
    }
}
