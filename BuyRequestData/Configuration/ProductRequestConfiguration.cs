using BuyRequest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BuyRequest.Data.Configuration
{
    public class ProductRequestConfiguration : IEntityTypeConfiguration<Domain.Entities.ProductRequest>
    {
        public void Configure(EntityTypeBuilder<ProductRequest> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.ProductQuantity).IsRequired();
            builder.Property(x => x.ProductPrice).IsRequired();
            builder.Property(x => x.ProductId).IsRequired();
            builder.Property(x => x.ProductCategory).IsRequired();
            builder.Property(x => x.ProductDescription).IsRequired();
            builder.Property(x => x.BuyRequestId).IsRequired();
            builder.Property(x => x.Total).IsRequired();
        }
    }
}
