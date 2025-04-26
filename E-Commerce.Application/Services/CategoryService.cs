using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitofwork)
        {
            _unitOfWork = unitofwork;
        }
        public async Task<IEnumerable<Category>> GetAllAsync(Expression<Func<Category, bool>>? criteria = null, params Expression<Func<Category, object>>[] includes )
        {
            return await _unitOfWork.Categories.GetAllAsync(criteria,includes);
        }
    }
}
