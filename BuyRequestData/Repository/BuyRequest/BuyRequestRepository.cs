using BuyRequest.Data.Context;
using BuyRequest.Data.Repository.ProductRequest;
using FinancialChallenge_Oscar.Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BuyRequest.Data.Repository.BuyRequest
{
    public class BuyRequestRepository : GenericRepository<Domain.Entities.BuyRequest>, IBuyRequestRepository
    {
        public BuyRequestRepository(BuyRequestContext context, IProductRequestRepository productRequestRepository) : base(context)
        {
            SetInclude(x => x.Include(i => i.BuyRequestProducts));
        }

        //public override async Task UpdateAsync(Domain.Entities.BuyRequest entity)
        //{
        //    var result = await base.GetByIdAsync(entity.Id);

        //    var buyInputProductsIds = entity.Products.Select(x => x.Id).ToList();
        //    var productDele = result.Products.Where(x => !buyInputProductsIds.Contains(x.Id)).ToList();

        //    foreach (var prod in productDele)
        //    {
        //        await _productRequestRepository.DeleteAsync(prod);
        //    }

        //    await base.UpdateAsync(entity);
        //}
    }
}