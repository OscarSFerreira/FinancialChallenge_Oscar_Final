using AutoMapper;
using FinancialChallenge_Oscar.Infrastructure.Paging;
using Moq;
using Moq.AutoMock;
using Product.Application.DTO;
using Product.Application.Mapping;
using Product.Application.Services;
using Product.Data.Repository;
using Product.UnitTest.AutoFaker;
using System.Threading.Tasks;
using Xunit;

namespace Product.UnitTest.ProductTest
{
    public class ProductServiceTest
    {
        private readonly AutoMocker Mocker;
        private readonly IMapper _mapper;

        public ProductServiceTest()
        {
            Mocker = new AutoMocker();
            if (_mapper == null)
            {
                var mapConfig = new MapperConfiguration(x =>
                {
                    x.AddProfile(new ProductMappingProfile());
                });
                _mapper = mapConfig.CreateMapper();
            }
        }

        [Fact(DisplayName = "ServicePostProduct Test")]
        public async Task PostProduct()
        {
            var prod = ProductFaker.GenerateProdDTO();
            var mapper = _mapper.Map<Domain.Entities.Product>(prod);

            var prodRepository = Mocker.GetMock<IProductRepository>();
            var mockMapper = Mocker.GetMock<IMapper>();
            mockMapper.Setup(x => x.Map<Domain.Entities.Product>(It.IsAny<ProductDTO>())).Returns(mapper);
            var prodService = Mocker.CreateInstance<ProductService>();

            await prodService.Post(prod);

            prodRepository.Verify(x => x.AddAsync(It.IsAny<Domain.Entities.Product>()), Times.Once());
        }

        [Fact(DisplayName = "ServiceGetAllProduct Test")]
        public async Task GetAllProduct()
        {
            PageParameter pageParameters = new PageParameter();
            var prod = ProductFaker.GenerateProdList();

            var prodRepository = Mocker.GetMock<IProductRepository>();
            prodRepository.Setup(x => x.GetAllWithPaging(pageParameters)).ReturnsAsync(prod);

            var prodService = Mocker.CreateInstance<ProductService>();

            await prodService.GetAll(pageParameters);

            prodRepository.Verify(x => x.GetAllWithPaging(pageParameters), Times.Once());
        }

        [Fact(DisplayName = "ServiceGetByIdProduct Test")]
        public async Task GetByIdProduct()
        {
            var prod = new Domain.Entities.Product();

            var prodRepository = Mocker.GetMock<IProductRepository>();
            prodRepository.Setup(x => x.GetByIdAsync(prod.Id)).ReturnsAsync(prod);

            var prodService = Mocker.CreateInstance<ProductService>();

            await prodService.GetById(prod.Id);

            prodRepository.Verify(x => x.GetByIdAsync(prod.Id), Times.Once());
        }

        [Fact(DisplayName = "ServiceGetProductByProductCategory Test")]
        public async Task GetProductByProductCategory()
        {
            var prod = ProductFaker.GenerateProd();
            var prodList = ProductFaker.GenerateProdList();

            var prodRepository = Mocker.GetMock<IProductRepository>();
            prodRepository.Setup(x => x.GetProductCategoryAsync(x => x.ProductCategory == prod.ProductCategory)).ReturnsAsync(prodList);

            var prodService = Mocker.CreateInstance<ProductService>();

            await prodService.GetByCategory(prod.ProductCategory);

            prodRepository.Verify(x => x.GetProductCategoryAsync(x => x.ProductCategory == prod.ProductCategory), Times.Once());
        }

        [Fact(DisplayName = "ServiceUpdateProduct Test")]
        public async Task UpdateProduct()
        {
            var prod = ProductFaker.GenerateProd();
            var mapper = _mapper.Map<ProductDTO>(prod);

            var prodRepository = Mocker.GetMock<IProductRepository>();
            var mockMapper = Mocker.GetMock<IMapper>();

            prodRepository.Setup(x => x.GetByIdAsync(prod.Id)).ReturnsAsync(prod);
            prodRepository.Setup(X => X.UpdateAsync(prod));
            mockMapper.Setup(x => x.Map<Domain.Entities.Product>(It.IsAny<ProductDTO>())).Returns(prod);

            var prodService = Mocker.CreateInstance<ProductService>();

            await prodService.ChangeProduct(prod.Id, mapper);

            prodRepository.Verify(x => x.UpdateAsync(prod), Times.Once());
        }

        [Fact(DisplayName = "DeleteProduct Test")]
        public async Task DeleteProduct()
        {
            var prod = ProductFaker.GenerateProd();
            var mapper = _mapper.Map<ProductDTO>(prod);

            var prodRepository = Mocker.GetMock<IProductRepository>();
            var mockMapper = Mocker.GetMock<IMapper>();

            prodRepository.Setup(x => x.GetByIdAsync(prod.Id)).ReturnsAsync(prod);
            prodRepository.Setup(x => x.DeleteAsync(prod));
            mockMapper.Setup(x => x.Map<ProductDTO>(It.IsAny<Domain.Entities.Product>())).Returns(mapper);

            var prodService = Mocker.CreateInstance<ProductService>();

            await prodService.DeleteById(prod.Id);

            prodRepository.Verify(x => x.DeleteAsync(prod), Times.Once());
        }
    }
}