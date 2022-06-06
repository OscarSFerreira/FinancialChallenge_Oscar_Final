using BuyRequest.Application.DTO;
using BuyRequest.Application.Interfaces;
using BuyRequest.UnitTest.AutoFaker;
using BuyRequestAPI.Controllers;
using FinancialChallenge_Oscar.Infrastructure.Paging;
using Moq;
using Moq.AutoMock;
using System.Threading.Tasks;
using Xunit;

namespace BuyRequest.UnitTest.BuyRequestTest
{
    public class BuyRequestControllerTest
    {
        private readonly AutoMocker Mocker;

        public BuyRequestControllerTest()
        {
            Mocker = new AutoMocker();
        }

        [Fact(DisplayName = "PostBuyRequest Test")]
        public async Task PostBuyRequest()
        {
            var buyReq = BuyRequestFaker.GenerateDTO();

            var buyReqService = Mocker.GetMock<IBuyRequestService>();
            buyReqService.Setup(X => X.Post(buyReq));

            var buyReqController = Mocker.CreateInstance<BuyRequestController>();

            await buyReqController.Post(buyReq);

            buyReqService.Verify(x => x.Post(It.IsAny<BuyRequestDTO>()), Times.Once());
        }

        [Fact(DisplayName = "GetAllBuyRequest Test")]
        public async Task GetAllBuyRequest()
        {
            var buyReqService = Mocker.GetMock<IBuyRequestService>();
            buyReqService.Setup(x => x.GetAll(null));

            var buyReqController = Mocker.CreateInstance<BuyRequestController>();

            PageParameter pageParameters = new PageParameter();

            await buyReqController.GetAll(pageParameters);

            buyReqService.Verify(x => x.GetAll(pageParameters), Times.Once());
        }

        [Fact(DisplayName = "GetByIdBuyRequest Test")]
        public async Task GetByIdBuyRequest()
        {
            var buyReq = new Domain.Entities.BuyRequest();

            var buyReqService = Mocker.GetMock<IBuyRequestService>();
            buyReqService.Setup(x => x.GetById(buyReq.Id));

            var buyReqController = Mocker.CreateInstance<BuyRequestController>();

            await buyReqController.GetById(buyReq.Id);

            buyReqService.Verify(x => x.GetById(buyReq.Id), Times.Once());
        }

        [Fact(DisplayName = "GetByClientIdBuyRequest Test")]
        public async Task GetByClientIdBuyRequest()
        {
            var buyReq = new Domain.Entities.BuyRequest();

            var buyReqService = Mocker.GetMock<IBuyRequestService>();
            buyReqService.Setup(x => x.GetByClientIdAsync(buyReq.ClientId));

            var buyReqController = Mocker.CreateInstance<BuyRequestController>();

            await buyReqController.GetByClientIdAsync(buyReq.ClientId);

            buyReqService.Verify(x => x.GetByClientIdAsync(buyReq.ClientId), Times.Once());
        }

        [Fact(DisplayName = "UpdateBuyRequest Test")]
        public async Task UpdateBankRequest()
        {
            var buyReq = BuyRequestFaker.GenerateDTO();

            var buyReqService = Mocker.GetMock<IBuyRequestService>();
            buyReqService.Setup(x => x.GetById(buyReq.Id));
            buyReqService.Setup(X => X.UpdateAsync(buyReq));

            var buyReqController = Mocker.CreateInstance<BuyRequestController>();

            await buyReqController.UpdateAsync(buyReq);

            buyReqService.Verify(x => x.UpdateAsync(It.IsAny<BuyRequestDTO>()), Times.Once());
        }

        [Fact(DisplayName = "DeleteBuyRequest Test")]
        public async Task DeleteBuyRequest()
        {
            var buyReq = BuyRequestFaker.Generate();

            var buyReqService = Mocker.GetMock<IBuyRequestService>();
            buyReqService.Setup(x => x.DeleteById(buyReq.Id));

            var buyReqController = Mocker.CreateInstance<BuyRequestController>();

            await buyReqController.DeleteById(buyReq.Id);

            buyReqService.Verify(x => x.DeleteById(buyReq.Id), Times.Once());
        }
    }
}