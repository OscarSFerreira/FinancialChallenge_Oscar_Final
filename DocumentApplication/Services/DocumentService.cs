using AutoMapper;
using BankRequest.ClientApi.Interfaces;
using BankRequest.Domain.Entities.Enum;
using Document.Application.DTO;
using Document.Application.Interfaces;
using Document.Data.Repository;
using Document.Domain.Entities.Enum;
using Document.Domain.Validators;
using FinancialChallenge_Oscar.Infrastructure.ErrorMessage;
using FinancialChallenge_Oscar.Infrastructure.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Document.Application.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IMapper _mapper;
        private readonly IDocumentRepository _documentRepository;
        Domain.Entities.Document doc = new Domain.Entities.Document();

        public IBankRequestClient _bankRequestClient;

        public DocumentService(IMapper mapper, IDocumentRepository documentRepository, IBankRequestClient bankRequestClient)
        {
            _mapper = mapper;
            _documentRepository = documentRepository;
            _bankRequestClient = bankRequestClient;
        }

        public string ErrorList(ErrorMessage<Domain.Entities.Document> error)
        {
            var errorList = "";

            foreach (var item in error.Message)
            {
                errorList += item.ToString();
            }
            return errorList;
        }

        public async Task<Domain.Entities.Document> Post(DocumentDTO input)
        {
            var mapperDoc = _mapper.Map(input, doc);

            var validator = new DocumentValidator();
            var valid = validator.Validate(mapperDoc);

            if (valid.IsValid)
            {
                if (mapperDoc.Paid == true)
                {
                    var type = new BankRequest.Domain.Entities.Enum.Type();
                    if (mapperDoc.Operation == Operation.Entry)
                    {
                        type = BankRequest.Domain.Entities.Enum.Type.Payment;
                    }
                    else/*(mapperDoc.Operation == Operation.Exit)*/
                    {
                        type = BankRequest.Domain.Entities.Enum.Type.Receive;
                    }

                    var response = await _bankRequestClient.PostCashBank(Origin.Document, mapperDoc.Id, $"Financial Transaction id: {mapperDoc.Id}",
                        type, mapperDoc.Total);

                    if (response == false)
                    {
                        var result = _documentRepository.BadRequestMessage(doc, "There was an error while communicating with the BankRequestAPI, please try again!");
                        var listError = ErrorList(result);
                        throw new Exception(listError);
                    }

                }

                await _documentRepository.AddAsync(mapperDoc);

            }
            else
            {
                var errorList = new ErrorMessage<Domain.Entities.Document>(HttpStatusCode.BadRequest.GetHashCode().ToString(),
                    valid.Errors.ConvertAll(x => x.ErrorMessage.ToString()), doc);
                var error = ErrorList(errorList);
                throw new Exception(error);
            }
            return mapperDoc;
        }

        public async Task<List<Domain.Entities.Document>> GetAll(PageParameter parameters)
        {

            var document = _documentRepository.GetAllWithPaging(parameters).OrderBy(doc => doc.Id).ToList();

            if (document.Count == 0)
            {
                var error = _documentRepository.NotFoundMessage(doc);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }

            return document;

        }

        public async Task<Domain.Entities.Document> GetById(Guid id)
        {

            var document = await _documentRepository.GetByIdAsync(id);

            if (document == null)
            {
                var error = _documentRepository.NotFoundMessage(doc);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }

            return document;

        }

        public async Task<Domain.Entities.Document> ChangeDocument(Guid id, DocumentDTO input)
        {

            var document = await _documentRepository.GetByIdAsync(id);
            var totalValueOld = document.Total;

            if (document == null)
            {
                var error = _documentRepository.NotFoundMessage(doc);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }

            if (document.Paid == true && input.Paid == false)
            {
                var result = _documentRepository.BadRequestMessage(doc, "You can't change the state of a already payed Document");
                var listError = ErrorList(result);
                throw new Exception(listError);
            }

            var mapperDoc = _mapper.Map(input, document);

            var validator = new DocumentValidator();
            var valid = validator.Validate(mapperDoc);

            var TotalUpdated = document.Total - totalValueOld;

            if (valid.IsValid)
            {

                if (document.Paid == false && input.Paid == true || TotalUpdated != totalValueOld && document.Paid == true)
                {
                    string description = $"Diference Transaction in Document id: {document.Id}";
                    var type = BankRequest.Domain.Entities.Enum.Type.Revert;
                    decimal total = TotalUpdated;

                    if (document.Paid == false && input.Paid == true)
                    {
                        description = $"Financial Transaction id: {document.Id}";
                        type = BankRequest.Domain.Entities.Enum.Type.Receive;
                        total = input.Total;
                    }

                    var response = await _bankRequestClient.PostCashBank(Origin.Document, document.Id, description,
                            type, total);

                    if (response == false)
                    {
                        var result = _documentRepository.BadRequestMessage(doc, "There was an error while communicating with the BankRequestAPI, please try again!");
                        var listError = ErrorList(result);
                        throw new Exception(listError);
                    }
                }

                await _documentRepository.UpdateAsync(mapperDoc);

                return mapperDoc;
            }
            else
            {
                var errorList = new ErrorMessage<Domain.Entities.Document>(HttpStatusCode.BadRequest.GetHashCode().ToString(),
                    valid.Errors.ConvertAll(x => x.ErrorMessage.ToString()), doc);
                var error = ErrorList(errorList);
                throw new Exception(error);
            }

        }

        public async Task<Domain.Entities.Document> ChangeState(Guid id, bool Status)
        {

            var document = await _documentRepository.GetByIdAsync(id);

            if (document == null)
            {
                var error = _documentRepository.NotFoundMessage(doc);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }

            if (document.Paid == true)
            {
                var result = _documentRepository.BadRequestMessage(doc, "You can only delete a finalized Document!");
                var listError = ErrorList(result);
                throw new Exception(listError);
            }

            document.Paid = Status;

            await _documentRepository.UpdateAsync(document);

            if (document.Paid == true)
            {

                var response = await _bankRequestClient.PostCashBank(Origin.Document, document.Id, $"Financial Transaction id: {document.Id}",
                     BankRequest.Domain.Entities.Enum.Type.Receive, document.Total);

                if (response == false)
                {
                    var result = _documentRepository.BadRequestMessage(doc, "There was an error while communicating with the BankRequestAPI, please try again!");
                    var listError = ErrorList(result);
                    throw new Exception(listError);
                }
            }

            return document;

        }

        public async Task<Domain.Entities.Document> DeleteById(Guid id)
        {
            var document = await _documentRepository.GetByIdAsync(id);

            if (document == null)
            {
                var error = _documentRepository.NotFoundMessage(doc);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }
            else
            {
                await _documentRepository.DeleteAsync(document);
            }

            if (document.Paid == true)
            {

                var response = await _bankRequestClient.PostCashBank(Origin.Document, document.Id, $"Revert Document order id: {document.Id}",
                    BankRequest.Domain.Entities.Enum.Type.Revert, -document.Total);
                if (response == false)
                {
                    var result = _documentRepository.BadRequestMessage(doc, "There was an error while communicating with the BankRequestAPI, please try again!");
                    var listError = ErrorList(result);
                    throw new Exception(listError);
                }

            }

            return document;
        }

    }
}
