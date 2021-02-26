using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreateDb.Storage.DTO;
using Microsoft.AspNetCore.Mvc;
using Web_api_pizza.Services;

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
        // GET: api/menu
        [HttpGet("fullMenu")]
        //[HttpGet]
        //[Route("fullMenu")]
        public ActionResult<List<DishDTO>> GetMenu()
        {
            var menu = _menuService.GetFullMenu();
            return menu;
        }
        [HttpGet("oneDish")]
        //[HttpGet]
        //[Route("oneDish")]
        public DishDTO GetDish(int id)
        {
            var dish = _menuService.GetOneDish(id);
            return dish;
        }

        // POST api/menu/
        //Работает
        [HttpPost]
        public string AddToMenu(DishDTO dishes)
        {
            _menuService.AddToMenu(dishes);
            return "Успешно";
        }

        // PUT api/menu/
        //работает
        [HttpPut]
        public void Put( DishDTO dish)
        {
            _menuService.EditMenu(dish);
        }

        // DELETE api/menu/
        //Работает
        [HttpDelete]
        public string Delete(int id)
        {
            _menuService.RemoveFromMenu(id);
            return "Успешно";
        }
    }
}
