﻿using E_Commerce.Application.Common;
using E_Commerce.Domain.Entities;
using E_Commerce.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace E_Commerce.Web.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<Category>? GetByIdAsync(int id);
        Task<IEnumerable<SelectListItem>> GetSelectList();
        Task<IEnumerable<Category>> GetAllAsync(Expression<Func<Category, bool>>? criteria = null, params Expression<Func<Category, object>>[] includes);
        Task AddAsync(CreateCategoryVM category);
        Task<bool> EditAsync(EditCategoryVM category);
        Task<bool> DeleteAsync(int id);
        Task<PagedResult<Product>> GetProducts(int id, int pageNumber, int pageSize);
    }
}
