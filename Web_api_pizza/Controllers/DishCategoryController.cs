using System;
using Microsoft.AspNetCore.Mvc;
using Web_api_pizza.Services;
using Web_api_pizza.Storage.DTO;

namespace Web_api_pizza.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishCategoryController : ControllerBase
    {
        private readonly IDishCategoryService _categoryService;
        public DishCategoryController(IDishCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("categories")]
        public IActionResult GetCategories()
        {
            var categories = _categoryService.GetCatigories();
            return Ok(categories);
        }

        [HttpPost("add")]
        public IActionResult AddNewCategory(DishCategoryDTO category)
        {
            if(category.Name == null)
            {
                ModelState.AddModelError("Name", "Не уканано название категории");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _categoryService.AddNewCategory(category);
            if (result.IsSuccess == false)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpPut("edit")]
        public IActionResult EditCategory (DishCategoryDTO category)
        {
            if (category.Name == null)
            {
                ModelState.AddModelError("Name", "Не уканано название категории");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _categoryService.EditCategory(category);
            if (result == null)
                return NotFound("Категория не найдена");

            if (result.IsSuccess == false)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteCategory (int id)
        {
            if(id < 1)
            {
                return BadRequest($"Недопустимое значение Id - \"{id}\"");
            }

            var result = _categoryService.DeleteCategory(id);

            if (result == null)
                return NotFound($"Категория не найдена Id - \"{id}\"");

            return Ok(result.Message);
        }
    }
}
