using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CreateDb.Storage;
using CreateDb.Storage.DTO;
using CreateDb.Storage.Models;

namespace Web_api_pizza.Services
{
    public interface IMenuService
    {
        public List<DishDTO> GetFullMenu();
        public DishDTO GetOneDish(int id);
        public void RemoveFromMenu(int id);
        public void EditMenu(DishDTO dish);
        public void AddToMenu(DishDTO dishDTO);
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
        public void RemoveFromMenu(int id)
        {
            var removeDish = _context.Dishes.FirstOrDefault(m => m.Id == id);

            _context.Dishes.Remove(removeDish);
            _context.SaveChanges();
        }
        public void EditMenu(DishDTO dish)
        {
            var editDish = _context.Dishes.FirstOrDefault(d => d.Id == dish.Id);

            editDish.ProductName = dish.ProductName;
            editDish.Price = dish.Price;

            _context.SaveChanges();
        }
        public void AddToMenu(DishDTO dish)
        {
            var checkDish = _context.Dishes
                .Where(d => d.ProductName == dish.ProductName)
                .FirstOrDefault();
            if (checkDish == null)
            {
                var dishesEntity = _mapper.Map<DishDTO, DishEntity>(dish);
                _context.Dishes.Add(dishesEntity);
                _context.SaveChanges();
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
