using AutoMapper;
using BuyRequest.Application.DTO;
using BuyRequest.Application.Mapping;
using BuyRequest.Application.Services;
using BuyRequest.Data.Repository.BuyRequest;
using BuyRequest.UnitTest.AutoFaker;
using FinancialChallenge_Oscar.Infrastructure.Paging;
using Moq;
using Moq.AutoMock;
using System.Threading.Tasks;
using Xunit;

namespace BuyRequest.UnitTest.BuyRequestTest
{
    public class BuyRequestServiceTest
    {
        private readonly AutoMocker Mocker;
        private readonly IMapper _mapper;

        public BuyRequestServiceTest()
        {
            Mocker = new AutoMocker();
            if (_mapper == null)
            {
                var mapConfig = new MapperConfiguration(x =>
                {
                    x.AddProfile(new BuyRequestMappingProfile());
                });
                _mapper = mapConfig.CreateMapper();
            }
        }

        [Fact(DisplayName = "PostBuyRequest Test")]
        public async Task PostBuyRequest()
        {
            var buyReq = BuyRequestFaker.GenerateDTO();
            var mapper = _mapper.Map<Domain.Entities.BuyRequest>(buyReq);

            var buyReqRepository = Mocker.GetMock<IBuyRequestRepository>();
            var mockMapper = Mocker.GetMock<IMapper>();

            mockMapper.Setup(x => x.Map<Domain.Entities.BuyRequest>(It.IsAny<BuyRequestDTO>())).Returns(mapper);

            var buyReqService = Mocker.CreateInstance<BuyRequestService>();

            await buyReqService.Post(buyReq);

            buyReqRepository.Verify(x => x.AddAsync(It.IsAny<Domain.Entities.BuyRequest>()), Times.Once());
        }

        [Fact(DisplayName = "ServiceGetAllBuyRequest Test")]
        public async Task GetAllBuyRequest()
        {
            PageParameter pageParameters = new PageParameter();
            var buyRequest = BuyRequestFaker.GenerateList();

            var buyReqRepository = Mocker.GetMock<IBuyRequestRepository>();
            buyReqRepository.Setup(x => x.GetAllWithPaging(pageParameters)).ReturnsAsync(buyRequest);

            var buyReqService = Mocker.CreateInstance<BuyRequestService>();

            await buyReqService.GetAll(pageParameters);

            buyReqRepository.Verify(x => x.GetAllWithPaging(pageParameters), Times.Once());
        }

        [Fact(DisplayName = "ServiceGetByIdBuyRequest Test")]
        public async Task GetByIdBuyRequest()
        {
            var buyRequest = new Domain.Entities.BuyRequest();

            var buyReqRepository = Mocker.GetMock<IBuyRequestRepository>();
            buyReqRepository.Setup(x => x.GetByIdAsync(buyRequest.Id)).ReturnsAsync(buyRequest);

            var buyReqService = Mocker.CreateInstance<BuyRequestService>();

            await buyReqService.GetById(buyRequest.Id);

            buyReqRepository.Verify(x => x.GetByIdAsync(buyRequest.Id), Times.Once());
        }

        [Fact(DisplayName = "ServiceUpdateBuyRequest Test")]
        public async Task UpdateBuyRequest()
        {
            var buyRequest = BuyRequestFaker.Generate();
            var mapper = _mapper.Map<BuyRequestDTO>(buyRequest);

            var buyReqRepository = Mocker.GetMock<IBuyRequestRepository>();
            var mockMapper = Mocker.GetMock<IMapper>();

            buyReqRepository.Setup(x => x.GetByIdAsync(buyRequest.Id)).ReturnsAsync(buyRequest);
            buyReqRepository.Setup(X => X.UpdateAsync(buyRequest));
            mockMapper.Setup(x => x.Map<Domain.Entities.BuyRequest>(It.IsAny<BuyRequestDTO>())).Returns(buyRequest);

            var buyReqService = Mocker.CreateInstance<BuyRequestService>();

            await buyReqService.UpdateAsync(mapper);

            buyReqRepository.Verify(x => x.UpdateAsync(buyRequest), Times.Once());
        }

        [Fact(DisplayName = "DeleteBuyRequest Test")]
        public async Task DeleteBuyRequest()
        {
            var buyReq = BuyRequestFaker.Generate();
            var mapper = _mapper.Map<BuyRequestDTO>(buyReq);

            var buyReqRepository = Mocker.GetMock<IBuyRequestRepository>();
            var mockMapper = Mocker.GetMock<IMapper>();

            buyReqRepository.Setup(x => x.GetByIdAsync(buyReq.Id)).ReturnsAsync(buyReq);
            buyReqRepository.Setup(x => x.DeleteAsync(buyReq));
            mockMapper.Setup(x => x.Map<BuyRequestDTO>(It.IsAny<Domain.Entities.BuyRequest>())).Returns(mapper);

            var buyReqService = Mocker.CreateInstance<BuyRequestService>();

            await buyReqService.DeleteById(mapper.Id);

            buyReqRepository.Verify(x => x.DeleteAsync(buyReq), Times.Once());
        }
    }
}