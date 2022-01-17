using System;
using Microsoft.AspNetCore.Mvc;
using Web_api_pizza.Filters;
using Web_api_pizza.Services;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Enums;

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
        
        [HttpGet("fullMenu")]
        public IActionResult GetMenu([FromQuery] DishFilter filter)
        {
            var menu = _menuService.GetFullMenu(filter);
            return Ok(menu);
        }
        
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

        
        [HttpPost("add")]
        public IActionResult AddToMenu(DishDTO dish)
        {
            if (dish.Price <= 0)
            {
                ModelState.AddModelError("Price", $"Недопустимое значение цены - \"{dish.Price}\"");
            }
            //if (Enum.IsDefined<DishCategoryEnum>(dish.Category) == false)
            //{
            //    ModelState.AddModelError("Category", "Не указана категория блюда");
            //}


            if (dish.ProductName == null)
            {
                ModelState.AddModelError("ProductName", "Укажите блюдо");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _menuService.AddToMenu(dish);
            return Ok(result.Message);
        }

       
        [HttpPut("edit")]
        public IActionResult Edit(DishDTO dish)
        {
            if(dish.Id <= 0)
            {
                ModelState.AddModelError("Id", $"Недопустимое значение Id - \"{dish.Id}\"");
            }
            //if(dish.Price <= 0)
            //{
            //    ModelState.AddModelError("Price", $"Недопустимое значение цены - \"{dish.Price}\"");
            //}
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
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

      
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
            return Ok(result.Message);
            
        }
    }
}
