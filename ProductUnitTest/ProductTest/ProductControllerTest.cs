using FinancialChallenge_Oscar.Infrastructure.Paging;
using Moq;
using Moq.AutoMock;
using Product.Application.DTO;
using Product.Application.Interfaces;
using Product.UnitTest.AutoFaker;
using ProductAPI.Controllers;
using System.Threading.Tasks;
using Xunit;

namespace Product.UnitTest.ProductTest
{
    public class ProductControllerTest
    {
        private readonly AutoMocker Mocker;

        public ProductControllerTest()
        {
            Mocker = new AutoMocker();
        }

        [Fact(DisplayName = "PostProduct Test")]
        public async Task PostProduct()
        {
            var prod = ProductFaker.GenerateProdDTO();

            var prodService = Mocker.GetMock<IProductService>();
            prodService.Setup(x => x.Post(prod));

            var prodController = Mocker.CreateInstance<ProductController>();

            await prodController.Post(prod);

            prodService.Verify(x => x.Post(It.IsAny<ProductDTO>()), Times.Once());
        }

        [Fact(DisplayName = "GetAllProduct Test")]
        public async Task GetAllProduct()
        {
            var prodService = Mocker.GetMock<IProductService>();
            prodService.Setup(x => x.GetAll(null));

            var prodController = Mocker.CreateInstance<ProductController>();

            PageParameter pageParameters = new PageParameter();

            await prodController.GetAll(pageParameters);

            prodService.Verify(x => x.GetAll(pageParameters), Times.Once());
        }

        [Fact(DisplayName = "GetByIdProduct Test")]
        public async Task GetByIdProduct()
        {
            var prod = ProductFaker.GenerateProd();

            var prodService = Mocker.GetMock<IProductService>();
            prodService.Setup(x => x.GetById(prod.Id));

            var prodController = Mocker.CreateInstance<ProductController>();

            await prodController.GetById(prod.Id);

            prodService.Verify(x => x.GetById(prod.Id), Times.Once());
        }

        [Fact(DisplayName = "GetProductByProductCategory Test")]
        public async Task GetCategory()
        {
            var prod = ProductFaker.GenerateProd();

            var prodService = Mocker.GetMock<IProductService>();
            prodService.Setup(x => x.GetByCategory(prod.ProductCategory));

            var prodController = Mocker.CreateInstance<ProductController>();

            await prodController.GetByProductCategory(prod.ProductCategory);

            prodService.Verify(x => x.GetByCategory(prod.ProductCategory), Times.Once());
        }

        [Fact(DisplayName = "UpdateProduct Test")]
        public async Task UpdateProduct()
        {
            var prod = ProductFaker.GenerateProd();
            var prodDTO = ProductFaker.GenerateProdDTO();

            var prodService = Mocker.GetMock<IProductService>();
            prodService.Setup(x => x.GetById(prod.Id));
            prodService.Setup(x => x.ChangeProduct(prod.Id, prodDTO));

            var prodController = Mocker.CreateInstance<ProductController>();

            await prodController.ChangeProduct(prod.Id, prodDTO);

            prodService.Verify(x => x.ChangeProduct(prod.Id, It.IsAny<ProductDTO>()), Times.Once());
        }

        [Fact(DisplayName = "DeleteProduct Test")]
        public async Task DeleteProduct()
        {
            var prod = ProductFaker.GenerateProd();

            var prodService = Mocker.GetMock<IProductService>();
            prodService.Setup(x => x.DeleteById(prod.Id));

            var prodController = Mocker.CreateInstance<ProductController>();

            await prodController.DeleteById(prod.Id);

            prodService.Verify(x => x.DeleteById(prod.Id), Times.Once());
        }
    }
}