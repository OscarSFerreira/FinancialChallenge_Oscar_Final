using Document.Application.DTO;
using Document.Application.Interfaces;
using Document.UnitTest.AutoFaker;
using DocumentAPI.Controllers;
using FinancialChallenge_Oscar.Infrastructure.Paging;
using Moq;
using Moq.AutoMock;
using System.Threading.Tasks;
using Xunit;

namespace Document.UnitTest.DocumentTest
{
    public class DocumentControllerTest
    {
        private readonly AutoMocker Mocker;

        public DocumentControllerTest()
        {
            Mocker = new AutoMocker();
        }

        [Fact(DisplayName = "PostDocument Test")]
        public async Task PostBuyRequest()
        {
            var docReq = DocumentFaker.GenerateDocDTO();

            var docReqService = Mocker.GetMock<IDocumentService>();
            docReqService.Setup(X => X.Post(docReq));

            var docReqController = Mocker.CreateInstance<DocumentController>();

            await docReqController.Post(docReq);

            docReqService.Verify(x => x.Post(It.IsAny<DocumentDTO>()), Times.Once());
        }

        [Fact(DisplayName = "GetAllDocument Test")]
        public async Task GetAllBuyRequest()
        {
            var docReqService = Mocker.GetMock<IDocumentService>();
            docReqService.Setup(x => x.GetAll(null));

            var docReqController = Mocker.CreateInstance<DocumentController>();

            PageParameter pageParameters = new PageParameter();

            await docReqController.GetAll(pageParameters);

            docReqService.Verify(x => x.GetAll(pageParameters), Times.Once());
        }

        [Fact(DisplayName = "GetByIdDocument Test")]
        public async Task GetByIdBuyRequest()
        {
            var docReq = DocumentFaker.GenerateDoc();

            var buyReqService = Mocker.GetMock<IDocumentService>();
            buyReqService.Setup(x => x.GetById(docReq.Id));

            var buyReqController = Mocker.CreateInstance<DocumentController>();

            await buyReqController.GetById(docReq.Id);

            buyReqService.Verify(x => x.GetById(docReq.Id), Times.Once());
        }

        [Fact(DisplayName = "UpdateDocument Test")]
        public async Task UpdateDocument()
        {
            var docReq = DocumentFaker.GenerateDoc();
            var docReqDTO = DocumentFaker.GenerateDocDTO();

            var docReqService = Mocker.GetMock<IDocumentService>();
            docReqService.Setup(x => x.GetById(docReq.Id));
            docReqService.Setup(x => x.ChangeDocument(docReq.Id, docReqDTO));

            var docReqController = Mocker.CreateInstance<DocumentController>();

            await docReqController.ChangeDocument(docReq.Id, docReqDTO);

            docReqService.Verify(x => x.ChangeDocument(docReq.Id, It.IsAny<DocumentDTO>()), Times.Once());
        }

        [Fact(DisplayName = "DeleteDocument Test")]
        public async Task DeleteDocument()
        {
            var docReq = DocumentFaker.GenerateDoc();

            var docReqService = Mocker.GetMock<IDocumentService>();
            docReqService.Setup(x => x.DeleteById(docReq.Id));

            var docReqController = Mocker.CreateInstance<DocumentController>();

            await docReqController.DeleteById(docReq.Id);

            docReqService.Verify(x => x.DeleteById(docReq.Id), Times.Once());
        }
    }
}
