using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Web_api_pizza.Filters;
using Web_api_pizza.Storage;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.Services
{
    public interface IMenuService
    {
        public List<DishDTO> GetFullMenu(DishFilter filter);
        public DishDTO GetOneDish(int id);
        public OperationResult RemoveFromMenu(int id);
        public OperationResult EditMenu(DishDTO dish);
        public OperationResult AddToMenu(DishDTO dishDTO);
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

        public List<DishDTO> GetFullMenu(DishFilter filter)
        {
            var menu = _context.Dishes
                .OrderBy(x => x)
                .AsQueryable();

            menu = filter.Filters(menu);
            var menuEntity = menu.ToList();

            var menuDTO = _mapper.Map<List<DishDTO>>(menuEntity);

            return menuDTO;
        }
        public DishDTO GetOneDish(int id)
        {
            var dishEntity = _context.Dishes.FirstOrDefault(d => d.Id == id);
            var dishDTO = _mapper.Map<DishDTO>(dishEntity);
            return dishDTO;
        }
        public OperationResult RemoveFromMenu(int id)
        {
            var removeDish = _context.Dishes.FirstOrDefault(m => m.Id == id);
            if(removeDish == null || removeDish.IsActive == false)
            {
                return null;
            }
            removeDish.IsActive = false;
            _context.SaveChanges();
            var result = new OperationResult(true, "Блюдо удалено");
            return result;

        }
        public OperationResult EditMenu(DishDTO dish)
        {
            var result = RemoveFromMenu(dish.Id.Value);

            if (result == null)
            {
                return null;
            }

            var editableDish = _mapper.Map<DishEntity>(dish);
            editableDish.Id = 0;

            _context.Dishes.Add(editableDish);
            _context.SaveChanges();
            result.IsSuccess = true;
            result.Message = "Блюдо изменено";
            return result;
        }
        public OperationResult AddToMenu(DishDTO dish)
        {
            dish.Id = 0;
            var dishEntity = _context.Dishes
                .Where(d => d.ProductName == dish.ProductName && d.IsActive == true)
                .FirstOrDefault();

            var result = new OperationResult(false);
            if (dishEntity != null)
            {
                result.Message = "Блюдо уже есть в меню";
                return result;
            }

            dishEntity = _mapper.Map(dish, dishEntity);
            _context.Dishes.Add(dishEntity);
            _context.SaveChanges();

            result.IsSuccess = true;
            result.Message = "Блюдо добавлено";
            return result;
            
            
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
