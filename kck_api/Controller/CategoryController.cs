using kck_api.Database;
using kck_api.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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

            public async Task<bool> AddCategoryAsync(CategoryModel category)
            {
                try
                {
                    await _context.Categories.AddAsync(category);
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            public async Task<List<CategoryModel>> GetAllCategoriesAsync()
            {
                try
                {
                    return await _context.Categories.OrderBy(c => c.Name).ToListAsync();
                }
                catch (Exception ex)
                {
                    return new List<CategoryModel>();
                }
            }

        public async Task<CategoryModel> GetCategoryByIdAsync(int categoryId)
        {
            try
            {
                return await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<int> GetOrCreateCategoryIdAsync(string categoryName)
        {
            try
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
            catch (Exception ex)
            {
                return -1;
            }
        }
        public async Task<bool> EditCategoryAsync(int categoryId, string newName)
        {
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
                if (category != null)
                {
                    category.Name = newName;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> IsCategoryExistsAsync(string categoryName)
        {
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);
                if (category != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> RemoveCategoryAsync(int categoryId)
        {
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
                if (category != null)
                {
                    _context.Categories.Remove(category);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
            public async Task<string> GetCategoryNameByIdAsync(int categoryId)
            {
                var category = await _context.Categories.FindAsync(categoryId);
                return category?.Name ?? "-";
            }
    }
    }