using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Web_api_pizza.Storage;
using Web_api_pizza.Storage.DTO;

namespace Web_api_pizza.Services
{
    public interface IDishCategoryService
    {
        public List<DishCategoryDTO> GetCatigories();
        public OperationResult AddNewCategory(DishCategoryDTO category);
        public OperationResult EditCategory(DishCategoryDTO category);
        public OperationResult DeleteCategory(int categoryId);
    }
    public class DishCategoryService :IDishCategoryService
    {
        private readonly IMapper _mapper;
        private readonly PizzaDbContext _context;
        public DishCategoryService(IMapper mapper, PizzaDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        /// <summary>
        /// Метод получения списка категорий
        /// </summary>
        /// <returns></returns>
        public List<DishCategoryDTO> GetCatigories ()
        {
            var categoriesEntity = _context.Categories
                .OrderBy(x => x.Id)
                .ToList();

            var categoriesDTO = _mapper.Map<List<DishCategoryDTO>>(categoriesEntity);

            return categoriesDTO;
        }

        /// <summary>
        /// Метод добавляет новую категорию в БД
        /// </summary>
        public OperationResult AddNewCategory(DishCategoryDTO category)
        {
            var categoryEntity = _context.Categories
                .Where(x => x.Name == category.Name)
                .FirstOrDefault();

            var result = new OperationResult(false);

            if (categoryEntity != null)
            {
                result.Message = "Такая категория уже существует";
                return result;
            }

            categoryEntity = _mapper.Map(category, categoryEntity);
            _context.Add(categoryEntity);
            _context.SaveChanges();

            result.IsSuccess = true;
            result.Message = "Категория добавлена";
            return result;
        }

        /// <summary>
        /// Метод позволяет изменить категорию
        /// </summary>
        public OperationResult EditCategory(DishCategoryDTO category)
        {
            var categoryEntity = _context.Categories
                .FirstOrDefault(x => x.Id == category.Id);

            if(categoryEntity == null)
            {
                return null;
            }

            categoryEntity = _mapper.Map(category, categoryEntity);
            _context.SaveChanges();

            return new OperationResult(true, "Успешно");
        }

        /// <summary>
        /// Метод удаляет категорию из БД
        /// </summary>
        /// <param name="categoryId"> Id категории </param>
        /// <returns></returns>
        public OperationResult DeleteCategory(int categoryId)
        {
            var categoryEntity = _context.Categories
                .FirstOrDefault(x => x.Id == categoryId);

            if (categoryEntity == null)
                return null;

            _context.Categories.Remove(categoryEntity);
            _context.SaveChanges();
            return new OperationResult(true, "Категория удалена");
        }
    }
}
