using AutoMapper;
using Document.Application.DTO;
using Document.Application.Mapping;
using Document.Application.Services;
using Document.Data.Repository;
using Document.UnitTest.AutoFaker;
using FinancialChallenge_Oscar.Infrastructure.Paging;
using Moq;
using Moq.AutoMock;
using System.Threading.Tasks;
using Xunit;

namespace Document.UnitTest.DocumentTest
{
    public class DocumentServiceTest
    {
        private readonly AutoMocker Mocker;
        private readonly IMapper _mapper;

        public DocumentServiceTest()
        {
            Mocker = new AutoMocker();
            if (_mapper == null)
            {
                var mapConfig = new MapperConfiguration(x =>
                {
                    x.AddProfile(new DocumentMappingProfile());
                });
                _mapper = mapConfig.CreateMapper();
            }
        }

        [Fact(DisplayName = "ServicePostDocument Test")]
        public async Task PostDocument()
        {
            var docRequest = DocumentFaker.GenerateDocDTO();
            var mapper = _mapper.Map<Domain.Entities.Document>(docRequest);

            var docReqRepository = Mocker.GetMock<IDocumentRepository>();
            var mockMapper = Mocker.GetMock<IMapper>();
            mockMapper.Setup(x => x.Map<Domain.Entities.Document>(It.IsAny<DocumentDTO>())).Returns(mapper);
            var docReqService = Mocker.CreateInstance<DocumentService>();

            await docReqService.Post(docRequest);

            docReqRepository.Verify(x => x.AddAsync(It.IsAny<Domain.Entities.Document>()), Times.Once());
        }

        [Fact(DisplayName = "ServiceGetAllDocument Test")]
        public async Task GetAllDocument()
        {
            PageParameter pageParameters = new PageParameter();
            var docRequest = DocumentFaker.GenerateDocList();

            var docReqRepository = Mocker.GetMock<IDocumentRepository>();
            docReqRepository.Setup(x => x.GetAllWithPaging(pageParameters)).ReturnsAsync(docRequest);

            var docReqService = Mocker.CreateInstance<DocumentService>();

            await docReqService.GetAll(pageParameters);

            docReqRepository.Verify(x => x.GetAllWithPaging(pageParameters), Times.Once());
        }

        [Fact(DisplayName = "ServiceGetByIdDocument Test")]
        public async Task GetByIdDocument()
        {
            var docRequest = new Domain.Entities.Document();

            var docReqRepository = Mocker.GetMock<IDocumentRepository>();
            docReqRepository.Setup(x => x.GetByIdAsync(docRequest.Id)).ReturnsAsync(docRequest);

            var docReqService = Mocker.CreateInstance<DocumentService>();

            await docReqService.GetById(docRequest.Id);

            docReqRepository.Verify(x => x.GetByIdAsync(docRequest.Id), Times.Once());
        }

        [Fact(DisplayName = "ServiceUpdateDocument Test")]
        public async Task UpdateDocument()
        {
            var docRequest = DocumentFaker.GenerateDoc();
            var mapper = _mapper.Map<DocumentDTO>(docRequest);

            var docReqRepository = Mocker.GetMock<IDocumentRepository>();
            var mockMapper = Mocker.GetMock<IMapper>();

            docReqRepository.Setup(x => x.GetByIdAsync(docRequest.Id)).ReturnsAsync(docRequest);
            docReqRepository.Setup(X => X.UpdateAsync(docRequest));
            mockMapper.Setup(x => x.Map<Domain.Entities.Document>(It.IsAny<DocumentDTO>())).Returns(docRequest);

            var docReqService = Mocker.CreateInstance<DocumentService>();

            await docReqService.ChangeDocument(docRequest.Id, mapper);

            docReqRepository.Verify(x => x.UpdateAsync(docRequest), Times.Once());
        }

        [Fact(DisplayName = "DeleteDocument Test")]
        public async Task DeleteDocument()
        {
            var docReq = DocumentFaker.GenerateDoc();
            var mapper = _mapper.Map<DocumentDTO>(docReq);

            var docReqRepository = Mocker.GetMock<IDocumentRepository>();
            var mockMapper = Mocker.GetMock<IMapper>();

            docReqRepository.Setup(x => x.GetByIdAsync(docReq.Id)).ReturnsAsync(docReq);
            docReqRepository.Setup(x => x.DeleteAsync(docReq));
            mockMapper.Setup(x => x.Map<DocumentDTO>(It.IsAny<Domain.Entities.Document>())).Returns(mapper);

            var docReqService = Mocker.CreateInstance<DocumentService>();

            await docReqService.DeleteById(docReq.Id);

            docReqRepository.Verify(x => x.DeleteAsync(docReq), Times.Once());
        }
    }
}