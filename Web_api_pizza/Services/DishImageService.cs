using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Web_api_pizza.Storage.DTO;

namespace Web_api_pizza.Services
{
    public interface IDishImageService
    {
        public byte[] GetDishImage(string name);
        public byte[] GetOfferImage(string name);
        //public void AddNewImage(DishImageDTO image);
        public void AddNewImage(byte[] image);
    }
    public class DishImageService: IDishImageService
    {
        private readonly IMapper _mapper;
        private readonly PizzaDbContext _context;
        private readonly IOptions<ImagesServiceOptions> _imageOptions;
        public DishImageService(IMapper mapper, PizzaDbContext context,
            IOptions<ImagesServiceOptions> imageOptions)
        {
            _mapper = mapper;
            _context = context;
            _imageOptions = imageOptions;
        }

        public byte[] GetDishImage(string name)
        {
            var dirName = _imageOptions.Value.DishImagePath;
            var path = $"{dirName}{name}";
            byte[] file = File.ReadAllBytes(path);
            return file;
        }
        public byte[] GetOfferImage(string name)
        {
            var dirName = _imageOptions.Value.OfferImagePath;
            var path = $"{dirName}{name}";
            byte[] file = File.ReadAllBytes(path);
            return file;
        }


        public void AddNewImage(byte[] image)
        {
            var dirName = _imageOptions.Value.DishImagePath;
            var path = $"{dirName}kek2.jpeg";
            using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
            {
                // записываем в файл строку
                writer.Write(image);
            }
        }

    }
}
