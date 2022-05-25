using BankRequest.Application.DTO;
using BankRequest.Application.Interfaces;
using BankRequest.UnitTest.AutoFaker;
using BankRequestAPI.Controllers;
using FinancialChallenge_Oscar.Infrastructure.Paging;
using Moq;
using Moq.AutoMock;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BankRequest.UnitTest.BankRequestTest
{
    public class BankRequestControllerTest
    {
        private readonly AutoMocker Mocker;
        public BankRequestControllerTest()
        {
            Mocker = new AutoMocker();
        }

        [Fact(DisplayName = "PostBankRequest Test")]
        public async Task PostBankRequest()
        {
            var bankRequest = BankRequestFaker.GenerateBankReqDTO();

            var bankReqService = Mocker.GetMock<IBankRequestService>();
            bankReqService.Setup(X => X.PostBankRecord(bankRequest));

            var bankReqController = Mocker.CreateInstance<BankRequestController>();

            await bankReqController.Post(bankRequest);

            bankReqService.Verify(x => x.PostBankRecord(It.IsAny<BankRequestDTO>()), Times.Once());
        }

        [Fact(DisplayName = "GetAllBankRequest Test")]
        public async Task GetAllBankRequest()
        {
            var bankReqService = Mocker.GetMock<IBankRequestService>();
            bankReqService.Setup(x => x.GetAll(null));

            var bankReqController = Mocker.CreateInstance<BankRequestController>();

            PageParameter pageParameters = new PageParameter();

            await bankReqController.GetAll(pageParameters);

            bankReqService.Verify(x => x.GetAll(pageParameters), Times.Once());
        }

        [Fact(DisplayName = "GetByIdBankRequest Test")]
        public async Task GetByIdBankRequest()
        {
            var bankRequest = new Domain.Entities.BankRequest();

            var bankReqService = Mocker.GetMock<IBankRequestService>();
            bankReqService.Setup(x => x.GetById(bankRequest.Id));

            var bankReqController = Mocker.CreateInstance<BankRequestController>();

            await bankReqController.GetById(bankRequest.Id);

            bankReqService.Verify(x => x.GetById(bankRequest.Id), Times.Once());
        }

        [Fact(DisplayName = "GetByOriginIdBankRequest Test")]
        public async Task GetByOriginIdBankRequest()
        {
            var id = Guid.NewGuid();

            var bankRequest = new Domain.Entities.BankRequest();

            var bankReqService = Mocker.GetMock<IBankRequestService>();
            bankReqService.Setup(x => x.GetByOriginId(id));

            var bankReqController = Mocker.CreateInstance<BankRequestController>();

            await bankReqController.GetByOriginId(id);

            bankReqService.Verify(x => x.GetByOriginId(id), Times.Once());
        }

        [Fact(DisplayName = "UpdateBankRequest Test")]
        public async Task UpdateBankRequest()
        {
            var bankRequest = BankRequestFaker.GenerateBankReq();

            var bankReqService = Mocker.GetMock<IBankRequestService>();
            bankReqService.Setup(x => x.GetById(bankRequest.Id)).ReturnsAsync(bankRequest);
            bankReqService.Setup(X => X.ChangeBankRequest(bankRequest.Id, new BankRequestDTO()));

            var bankReqController = Mocker.CreateInstance<BankRequestController>();

            await bankReqController.ChangeBankRequest(bankRequest.Id, new BankRequestDTO());

            bankReqService.Verify(x => x.ChangeBankRequest(bankRequest.Id, It.IsAny<BankRequestDTO>()), Times.Once());
        }

    }

}
