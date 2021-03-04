using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.Services
{
    public interface IMenuService
    {
        public List<DishDTO> GetFullMenu();
        public DishDTO GetOneDish(int id);
        public string RemoveFromMenu(int id);
        public string EditMenu(DishDTO dish);
        public string AddToMenu(DishDTO dishDTO);
        //добавить сразу несколько
        //проверить нужен ли он
        //public void AddToMenu(List<DishDTO> dishesDTOs);
    }
    public class MenuService : IMenuService
    {
        private readonly IMapper _mapper;
        private readonly PizzaDbContext _context;
        public MenuService(PizzaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //работает
        public List<DishDTO> GetFullMenu()
        {
            var menu = _context.Dishes.OrderBy(x => x).ToList();
            var fullMenu = _mapper.Map<List<DishEntity>, List<DishDTO>>(menu);

            return fullMenu;
        }
        public DishDTO GetOneDish(int id)
        {
            var dishEntity = _context.Dishes.FirstOrDefault(d => d.Id == id);
            var dishDto = _mapper.Map<DishEntity, DishDTO>(dishEntity);
            return dishDto;
        }
        public string RemoveFromMenu(int id)
        {
            var removeDish = _context.Dishes.FirstOrDefault(m => m.Id == id);
            string message;
            if(removeDish == null)
            {
                message = null;
                return message;
            }
            _context.Dishes.Remove(removeDish);
            _context.SaveChanges();
            message = "Блюдо удалено";
            return message;

        }
        public string EditMenu(DishDTO dish)
        {
            var editDish = _context.Dishes.FirstOrDefault(d => d.Id == dish.Id);

            string message;
            if (editDish == null)
            {
                return message = null;
            }
            if (editDish.ProductName == dish.ProductName
            && editDish.Price == dish.Price)
            {
                message = "что изменять то?";
                return message;
            }
            editDish.ProductName = dish.ProductName;
            editDish.Price = dish.Price;

            _context.SaveChanges();
            message = "Изменено";
            return message;
        }
        public string AddToMenu(DishDTO dish)
        {
            dish.Id = 0;
            var checkDish = _context.Dishes
                .Where(d => d.ProductName == dish.ProductName)
                .FirstOrDefault();
            string message;
            if (checkDish == null)
            {
                var dishesEntity = _mapper.Map<DishDTO, DishEntity>(dish);
                _context.Dishes.Add(dishesEntity);
                _context.SaveChanges();
                message = "Блюдо добавлено";
                return message;
            }
            else
            {
                message = "Блюдо уже есть в меню";
                return message;
            }
        }
        public void AddToMenu(List<DishDTO> dishesDTOs)
        {
            var dishesEntities = _mapper.Map<List<DishDTO>, List<DishEntity>>(dishesDTOs);
            foreach (var dish in dishesEntities)
            {
                var checkDish = _context.Dishes
                .Where(d => d.ProductName == dish.ProductName)
                .FirstOrDefault();
                if(checkDish == null)
                {
                    _context.Dishes.Add(dish);
                }
            }
            _context.SaveChanges();
        }
    }
}
