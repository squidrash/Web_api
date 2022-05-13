using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web_api_pizza.Services;
using Web_api_pizza.Storage.DTO;

namespace Web_api_pizza.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishImageController :ControllerBase
    {
        private readonly IDishImageService _imageService;
        public DishImageController(IDishImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet("getDishImage")]
        public IActionResult GetDishImage(string name)
        {
            if (name == null)
                return null;

            var image = _imageService.GetDishImage(name);
            return File(image, "image/jpeg");
            
        }
        [HttpGet("getOfferImage")]
        public IActionResult GetOfferImage(string name)
        {
            if (name == null)
                return null;

            var image = _imageService.GetOfferImage(name);
            return File(image, "image/jpeg");

        }

        [HttpPost("addImages")]
        public IActionResult addImages()
        {
            var file = Request.Form.Files[0];
            Console.WriteLine(file.Name);
            Console.WriteLine(file.FileName);
            using var fileStream = new BinaryReader(file.OpenReadStream());
            var dishPic = fileStream.ReadBytes((int)file.Length);

             _imageService.AddNewImage(dishPic);
            return Ok();
        }
    }
}
