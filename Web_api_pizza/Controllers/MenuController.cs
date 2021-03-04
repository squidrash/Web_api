using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web_api_pizza.Services;
using Web_api_pizza.Storage.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        //работает
        //протестирован
        [HttpGet("fullMenu")]
        public IActionResult GetMenu()
        {
            var menu = _menuService.GetFullMenu();
            return Ok(menu);
        }
        //работает
        //протестирован
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

        //Работает
        //протестирован
        [HttpPost("add")]
        public IActionResult AddToMenu(DishDTO dishes)
        {
            //if (dishes.Price <= 0)
            //{
            //    ModelState.AddModelError("Price", $"Недопустимое значение цены - {dishes.Price}");
            //}
            if (dishes.Price <= 0)
            {
                return BadRequest($"Недопустимое значение цены - \"{dishes.Price}\"");
            }


            //if (dishes.ProductName == null)
            //{
            //    ModelState.AddModelError("ProductName", "Укажите блюдо");

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            var dishMess = _menuService.AddToMenu(dishes);
            return Ok(dishMess);
        }

        
        //работает
        //протестирован
        [HttpPut("edit")]
        public IActionResult Edit( DishDTO dish)
        {
            if(dish.Id <= 0)
            {
                ModelState.AddModelError("Id", $"Недопустимое значение Id - \"{dish.Id}\"");
            }
            if(dish.Price <= 0)
            {
                ModelState.AddModelError("Price", $"Недопустимое значение цены - \"{dish.Price}\"");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
                
            var mess = _menuService.EditMenu(dish);
            if (mess == null)
            {
                return NotFound($"Блюдо не найдено Id - \"{dish.Id}\"");
            }
            return Ok(mess);
        }

        // DELETE api/menu/
        //Работает
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest($"Недопустимое значение Id - \"{id}\"");
            }
            var mess = _menuService.RemoveFromMenu(id);
            if(mess == null)
            {
                return NotFound($"Блюдо не найдено Id - \"{id}\"");
            }
            return Ok(mess);
            
        }
    }
}
