using AutoMapper;
using BankRequest.Application.DTO;
using BankRequest.Application.Mapping;
using BankRequest.Application.Services;
using BankRequest.Data.Repository;
using BankRequest.UnitTest.AutoFaker;
using FinancialChallenge_Oscar.Infrastructure.Paging;
using Moq;
using Moq.AutoMock;
using System.Threading.Tasks;
using Xunit;

namespace BankRequest.UnitTest.BankRequestTest
{
    public class BankRequestServiceTest
    {
        private readonly AutoMocker Mocker;
        private readonly IMapper _mapper;

        public BankRequestServiceTest()
        {
            Mocker = new AutoMocker();
            if (_mapper == null)
            {
                var mapConfig = new MapperConfiguration(x =>
                {
                    x.AddProfile(new BankRequestMappingProfile());
                });
                _mapper = mapConfig.CreateMapper();
            }
        }

        //ADICIONAR TESTES QUE FALHEM

        [Fact(DisplayName = "ServicePostBankRequest Test")]
        public async Task PostBankRequest()
        {
            var bankRequest = BankRequestFaker.GenerateBankReqDTO();
            var mapper = _mapper.Map<Domain.Entities.BankRequest>(bankRequest);

            var bankReqRepository = Mocker.GetMock<IBankRequestRepository>();
            var mockMapper = Mocker.GetMock<IMapper>();
            mockMapper.Setup(x => x.Map<Domain.Entities.BankRequest>(It.IsAny<BankRequestDTO>())).Returns(mapper);
            var bankReqService = Mocker.CreateInstance<BankRequestService>();

            await bankReqService.PostBankRecord(bankRequest); //ver validators

            bankReqRepository.Verify(x => x.AddAsync(It.IsAny<Domain.Entities.BankRequest>()), Times.Once());
        }

        [Fact(DisplayName = "ServiceGetAllBankRequest Test")]
        public async Task GetAllBankRequest()
        {
            PageParameter pageParameters = new PageParameter();
            var bankRequest = BankRequestFaker.GenerateBankList();

            var bankReqService = Mocker.GetMock<IBankRequestRepository>();
            bankReqService.Setup(x => x.GetAllWithPaging(pageParameters)).ReturnsAsync(bankRequest);

            var bankReqController = Mocker.CreateInstance<BankRequestService>();

            await bankReqController.GetAll(pageParameters);

            bankReqService.Verify(x => x.GetAllWithPaging(pageParameters), Times.Once());
        }

        [Fact(DisplayName = "ServiceGetByIdBankRequest Test")]
        public async Task GetByIdBankRequest()
        {
            var bankRequest = new Domain.Entities.BankRequest();

            var bankReqService = Mocker.GetMock<IBankRequestRepository>();
            bankReqService.Setup(x => x.GetByIdAsync(bankRequest.Id)).ReturnsAsync(bankRequest);

            var bankReqController = Mocker.CreateInstance<BankRequestService>();

            await bankReqController.GetById(bankRequest.Id);

            bankReqService.Verify(x => x.GetByIdAsync(bankRequest.Id), Times.Once());
        }

        [Fact(DisplayName = "ServiceUpdateBankRequest Test")]
        public async Task UpdateBankRequest()
        {
            var bankRequest = BankRequestFaker.GenerateBankReqNoOrigin();
            var mapper = _mapper.Map<Domain.Entities.BankRequest>(bankRequest);

            var bankReqService = Mocker.GetMock<IBankRequestRepository>();
            var mockMapper = Mocker.GetMock<IMapper>();

            bankReqService.Setup(x => x.GetByIdAsync(bankRequest.Id)).ReturnsAsync(bankRequest);
            bankReqService.Setup(X => X.UpdateAsync(bankRequest));
            mockMapper.Setup(x => x.Map<Domain.Entities.BankRequest>(It.IsAny<BankRequestDTO>())).Returns(mapper);

            var bankReqController = Mocker.CreateInstance<BankRequestService>();

            await bankReqController.ChangeBankRequest(bankRequest.Id, new BankRequestDTO());

            bankReqService.Verify(x => x.UpdateAsync(bankRequest), Times.Once());
        }
    }
}