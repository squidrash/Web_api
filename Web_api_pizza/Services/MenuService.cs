using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Web_api_pizza.Filters;
using Web_api_pizza.Storage;
using Web_api_pizza.Storage.DTO;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.Services
{
    public interface IMenuService
    {
        /// <summary>
        /// Получения списка блюд
        /// </summary>
        /// <param name="filter">Опциональный параметр. Фильтр по названию блюд, категории</param>
        /// <see cref="DishFilter"/>
        /// <returns>Список блюд</returns>
        public List<MenuDTO> GetMenu(DishFilter filter);

        /// <summary>
        /// Получение данных одного блюда
        /// </summary>
        /// <param name="id">Id блюда</param>
        /// <returns>Данные конкретного блюда</returns>
        public DishDTO GetOneDish(int id);

        /// <summary>
        /// Удаление блюда из меню. Блюдо помечается в БД как неактивное
        /// </summary>
        /// <param name="id">Id блюда</param>
        /// <returns>Результат операции в виде объекта OperationResult(результат bool, сообщение)</returns>
        public OperationResult RemoveFromMenu(int id);

        /// <summary>
        /// Изменение данных блюда.
        /// Создается новый измененнный экземпляр блюда.
        /// Старое блюдо остается в базе и помечается как неактивное.
        /// </summary>
        /// <param name="dish">Блюдо с измененными данными</param>
        /// <returns>Результат операции в виде объекта OperationResult(результат bool, сообщение)</returns>
        public OperationResult EditMenu(DishDTO dish);

        /// <summary>
        /// Добавление блюда в меню
        /// </summary>
        /// <param name="dish">Данные блюда</param>
        /// <returns>Результат операции в виде объекта OperationResult(результат bool, сообщение)</returns>
        public OperationResult AddToMenu(DishDTO dish);
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

        public List<MenuDTO> GetMenu(DishFilter filter)
        {
            var menu = _context.Dishes
                .Include(x => x.Category)
                .OrderBy(x => x.CategoryId)
                .AsQueryable();

            menu = filter.Filters(menu);
            var menuEntity = menu.ToList();

            var fullMenu = menuEntity
                .GroupBy(x => x.CategoryId)
                .Select(y => new MenuDTO
                {
                    CategoryId = y.Key,
                    CategoryName = y.Select(x =>
                    {
                       var name = x.Category.Name;
                        return name;
                    }).ToList()[0],
                    Dishes = y.Select(x =>
                    {
                        var mainDish = _mapper.Map<MainDishDTO>(x);
                        return mainDish;
                    }).ToList()

                }).ToList();

            return fullMenu;
        }

        public DishDTO GetOneDish(int id)
        {
            var dishEntity = _context.Dishes
                .Include(x => x.Category)
                .FirstOrDefault(d => d.Id == id);

            var dishDTO = _mapper.Map<DishDTO>(dishEntity);
            return dishDTO;
        }
        public OperationResult RemoveFromMenu(int id)
        {
            var removeDish = _context.Dishes.FirstOrDefault(m => m.Id == id);

            if (removeDish == null || removeDish.IsActive == false)
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
            var dishEntity = _context.Dishes.FirstOrDefault(x => x.Id == dish.Id);

            if(dishEntity == null)
            {
                return null;
            }

            if (dishEntity.ProductName == dish.ProductName && dishEntity.Price == dish.Price)
            {
                dishEntity.CategoryId = dish.Category.Id;
                dishEntity.Image = dish.Image;
                dishEntity.Description = dish.Description;
            }
            else
            {
                var result = RemoveFromMenu(dish.Id.Value);

                if (result == null)
                {
                    return null;
                }

                var editableDish = _mapper.Map<DishEntity>(dish);
                editableDish.CategoryId = dish.Category.Id;
                editableDish.Id = 0;

                _context.Dishes.Add(editableDish);
            }
            
            _context.SaveChanges();
            return new OperationResult(true, "Блюдо изменено");
        }
        
        public OperationResult AddToMenu(DishDTO dish)
        {
            var dishEntity = _context.Dishes
                .FirstOrDefault(d => d.ProductName == dish.ProductName && d.IsActive == true);

            var result = new OperationResult(false);

            if (dishEntity != null)
            {
                result.Message = "Блюдо уже есть в меню";
                return result;
            }

            var newDish = new DishEntity();
            newDish = _mapper.Map(dish, newDish);
            newDish.IsActive = true;
            if(dish.Category == null)
            {
                newDish.CategoryId = 0;
            }
            else
            {
                var category = _context.Categories
                    .FirstOrDefault(x => x.Id == dish.Category.Id);
                if (category == null)
                {
                    result.Message = "Такой категории нет в БД";
                    return result;
                }
            }
            _context.Dishes.Add(newDish);
            _context.SaveChanges();

            result.IsSuccess = true;
            result.Message = "Блюдо добавлено";
            return result;
            
            
        }
    }
}
