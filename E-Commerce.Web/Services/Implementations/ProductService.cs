using E_Commerce.Application.Common;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Web.Helpers;
using E_Commerce.Web.Services.Interfaces;
using E_Commerce.Web.ViewModels;
using System.Linq.Expressions;

namespace E_Commerce.Web.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileUploadService _fileUploadService;
        public ProductService(IUnitOfWork unitOfWork,IFileUploadService fileUploadService)
        {
            _unitOfWork = unitOfWork;
            _fileUploadService = fileUploadService;
        }

        public async Task<Product>? GetByIdAsync(int id)
        {
            return await _unitOfWork.Products.GetByIdAsync(id);
        }
        

        public async Task<IEnumerable<Product>> GetAllAsync(Expression<Func<Product, bool>>? criteria = null, params Expression<Func<Product, object>>[] includes)
        {
            return await _unitOfWork.Products.GetAllAsync(criteria, includes);
        }

        public async Task<Product>? Details(int id)
        {
            return await _unitOfWork.Products.GetProductWithCategoryProductsAsync(id);
        }

        public async Task<bool> AddAsync(CreateProductVM model)
        {
            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Stock = model.Stock,
                BestSeller = model.BestSeller,
                CategoryId = model.CategoryId,
                ImageUrl = await _fileUploadService.UploadFileAsync(model.Image, FileConstants.ProductFolderPath)
            };

            await _unitOfWork.Products.AddAsync(product);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> EditAsync(EditProductVM model)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(model.Id)!;
            if (product == null) return false;

            string currentImage = product.ImageUrl;
            string newImage = string.Empty;
            var hasNewImage = model.Image is not null;

            try
            {
                if (hasNewImage)
                {
                    newImage = await _fileUploadService.UploadFileAsync(model.Image!, FileConstants.ProductFolderPath);
                    product.ImageUrl = newImage;
                }
                else
                {
                    product.ImageUrl = currentImage;
                }
                product.Name = model.Name;
                product.Description = model.Description;
                product.Price = model.Price;
                product.Stock = model.Stock;
                product.BestSeller = model.BestSeller;
                product.CategoryId = model.CategoryId;

                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                {
                    if (hasNewImage)
                        _fileUploadService.DeleteFile(currentImage);
                    return true;
                }
                else
                {
                    if (hasNewImage)
                        _fileUploadService.DeleteFile(newImage);
                    return false;
                }
            }
            catch(Exception ex)
            {
                if (hasNewImage)
                    _fileUploadService.DeleteFile(newImage);
                throw new ApplicationException($"Error updating Product: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await GetByIdAsync(id);
            if (product == null)
                return false;

            _unitOfWork.Products.Delete(product);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result > 0)
            {
                _fileUploadService.DeleteFile(product.ImageUrl);
                return true;
            }
            return false;
        }

        public async Task<List<Product>> SearchById(int id)
        {
            var product = await _unitOfWork.Products.FindAsync(p => p.Id == id,p => p.Category);
            if (product == null)
                return new List<Product>();
            return new List<Product> { product };
        }
    }
}
