using FinancialChallenge_Oscar.Infrastructure.Paging;
using Product.Application.DTO;
using Product.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Product.Application.Interfaces
{
    public interface IProductService
    {
        Task Post(ProductDTO input);

        Task<IEnumerable<Domain.Entities.Product>> GetAll(PageParameter parameters);

        Task<Domain.Entities.Product> GetById(Guid id);

        Task<IEnumerable<Domain.Entities.Product>> GetByCategory(ProductCategory category);

        Task<Domain.Entities.Product> ChangeProduct(Guid id, ProductDTO prodRequest);

        Task<Domain.Entities.Product> DeleteById(Guid id);
    }
}