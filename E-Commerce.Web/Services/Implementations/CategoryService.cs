using E_Commerce.Application.Common;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Web.Helpers;
using E_Commerce.Web.Services.Interfaces;
using E_Commerce.Web.ViewModels;
using System.Linq.Expressions;

namespace E_Commerce.Web.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileUploadService _fileUploadService;
        public CategoryService(IUnitOfWork unitofwork, IFileUploadService fileUploadService)
        {
            _unitOfWork = unitofwork;
            _fileUploadService = fileUploadService;
        }
        public async Task<IEnumerable<Category>> GetAllAsync(Expression<Func<Category, bool>>? criteria = null, params Expression<Func<Category, object>>[] includes)
        {
            return await _unitOfWork.Categories.GetAllAsync(criteria, includes);
        }
        public async Task AddAsync(CategoryVM model)
        {
            var category = new Category
            {
                Name = model.Name,
                IsTopCategory = model.IsTopCategory,
                IsSection = model.IsSection,
                ImageUrl = await _fileUploadService.UploadFileAsync(model.Image, FileConstants.CategoryFolderPath)
            };
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();
        }

        public Task<Category>? GetByIdAsync(int id)
        {
            return _unitOfWork.Categories.GetByIdAsync(id);
        }

        public async Task<bool> EditAsync(CategoryVM model)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(model.Id)!;

            if (category == null)
                return false;

            string currentImage = category.ImageUrl;
            var hasNewImage = model.Image is not null;
            string newImagePath = string.Empty;

            try
            {
                if (hasNewImage)
                {
                    newImagePath = await _fileUploadService.UploadFileAsync(model.Image!, FileConstants.CategoryFolderPath);
                    category.ImageUrl = newImagePath;
                }
                category.Name = model.Name;
                category.IsTopCategory = model.IsTopCategory;
                category.IsSection = model.IsSection;

                var result = await _unitOfWork.SaveChangesAsync();
                if(result > 0)
                {
                    if (currentImage != null)
                        _fileUploadService.DeleteFile(currentImage);
                    return true;
                }
                else
                {
                    if (hasNewImage)
                        _fileUploadService.DeleteFile(newImagePath);
                    return false;
                }
            }
            catch(Exception ex)
            {
                if (hasNewImage)
                    _fileUploadService.DeleteFile(newImagePath);
                throw new ApplicationException($"Error updating category: {ex.Message}", ex);
            }
        }
    }
}
