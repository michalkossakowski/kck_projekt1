using kck_api.Database;
using kck_api.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace kck_api.Controller
    {
        public class CategoryController
        {
            private static CategoryController _instance;
            private readonly ApplicationDbContext _context;

            private CategoryController()
            {
                _context = ApplicationDbContext.GetInstance();
            }

            public static CategoryController GetInstance()
            {
                return _instance ??= new CategoryController();
            }

            public async Task AddCategoryAsync(CategoryModel category)
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
            }

            public async Task<List<CategoryModel>> GetAllCategoriesAsync()
            {
                return await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            }

            public async Task<CategoryModel> GetCategoryByIdAsync(int categoryId)
            {
                return await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            }
            public async Task<int> GetOrCreateCategoryIdAsync(string categoryName)
            {
            CategoryModel? category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);
            if (category != null)
            {
               return category.Id;
            }

            // Tworzenie nowej kategorii, jeśli nie istnieje
            CategoryModel newCategory = new CategoryModel(categoryName);
            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();
            return newCategory.Id;
            }
            public async Task EditCategoryAsync(int categoryId, string newName)
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
                if (category != null)
                {
                    category.Name = newName;
                    await _context.SaveChangesAsync();
                }
            }

            public async Task RemoveCategoryAsync(int categoryId)
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
                if (category != null)
                {
                    _context.Categories.Remove(category);
                    await _context.SaveChangesAsync();
                }
            }
            public async Task<string> GetCategoryNameByIdAsync(int categoryId)
            {
                var category = await _context.Categories.FindAsync(categoryId);
                return category?.Name ?? "Nieznana kategoria";
            }
    }
    }