using AutoMapper;
using BankRequest.Application.DTO;
using BankRequest.Domain.Entities.Enum;
using Document.Application.DTO;
using Document.Application.Interfaces;
using Document.Data.Repository;
using Document.Domain.Entities.Enum;
using Document.Domain.Validators;
using FinancialChallenge_Oscar.Infrastructure.ErrorMessage;
using FinancialChallenge_Oscar.Infrastructure.Paging;
using FinancialChallenge_Oscar.Infrastructure.RabbitMQ.Generic.Interfaces;
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
        private readonly IMessageProducer _messageProducer;

        //public IBankRequestClient _bankRequestClient;

        public DocumentService(IMapper mapper, IDocumentRepository documentRepository/*, IBankRequestClient bankRequestClient*/, IMessageProducer messageProducer)
        {
            _mapper = mapper;
            _documentRepository = documentRepository;
            //_bankRequestClient = bankRequestClient;
            _messageProducer = messageProducer;
        }

        public string ErrorList(ErrorMessage<DocumentDTO> error)
        {
            var errorList = "";

            foreach (var item in error.Message)
            {
                errorList += item.ToString();
            }
            return errorList;
        }

        public ErrorMessage<DocumentDTO> NotFoundMessage(DocumentDTO entity)
        {
            var errorList = new List<string>();
            errorList.Add("This database does not contain the data you requested!");
            var error = new ErrorMessage<DocumentDTO>(HttpStatusCode.NoContent.GetHashCode().ToString(), errorList, entity);
            return error;
        }

        public ErrorMessage<DocumentDTO> BadRequestMessage(DocumentDTO entity, string msg)
        {
            var errorList = new List<string>();
            errorList.Add(msg);
            var error = new ErrorMessage<DocumentDTO>(HttpStatusCode.BadRequest.GetHashCode().ToString(), errorList, entity);
            return error;
        }

        public async Task<Domain.Entities.Document> Post(DocumentDTO input)
        {
            var mapperDoc = _mapper.Map<Domain.Entities.Document>(input);

            var validator = new DocumentValidator();
            var valid = validator.Validate(mapperDoc);

            if (valid.IsValid)
            {
                if (mapperDoc.Paid == true)
                {
                    var type = new BankRequest.Domain.Entities.Enum.Type();
                    if (mapperDoc.Operation == Operation.Entry)
                    {
                        type = BankRequest.Domain.Entities.Enum.Type.Receive;
                    }
                    else
                    {
                        type = BankRequest.Domain.Entities.Enum.Type.Payment;
                    }

                    await _documentRepository.AddAsync(mapperDoc);

                    //var response = await _bankRequestClient.PostCashBank(Origin.Document, mapperDoc.Id, $"Financial Transaction id: {mapperDoc.Id}",
                    //    type, mapperDoc.Total);

                    var bankreqDTO = new BankRequestDTO()
                    {
                        Origin = Origin.Document,
                        OriginId = mapperDoc.Id,
                        Description = $"Financial Transaction Id: {mapperDoc.Id}",
                        Type = type,
                        Amount = mapperDoc.Total
                    };

                    _messageProducer.PublishMessage(bankreqDTO, "bankrequest");

                    //if (response == false)
                    //{
                    //    var result = BadRequestMessage(input, "There was an error while communicating with the BankRequestAPI, please try again!");
                    //    var listError = ErrorList(result);
                    //    throw new Exception(listError);
                    //}
                }
                else
                {
                    await _documentRepository.AddAsync(mapperDoc);
                }
            }
            else
            {
                var errorList = new ErrorMessage<DocumentDTO>(HttpStatusCode.BadRequest.GetHashCode().ToString(),
                    valid.Errors.ConvertAll(x => x.ErrorMessage.ToString()), input);
                var error = ErrorList(errorList);
                throw new Exception(error);
            }
            return mapperDoc;
        }

        public async Task<IEnumerable<Domain.Entities.Document>> GetAll(PageParameter parameters)
        {
            DocumentDTO doc = new DocumentDTO();
            var document = await _documentRepository.GetAllWithPaging(parameters);

            if (document.Count() == 0)
            {
                var error = NotFoundMessage(doc);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }
            return document;
        }

        public async Task<Domain.Entities.Document> GetById(Guid id)
        {
            DocumentDTO doc = new DocumentDTO();
            var document = await _documentRepository.GetByIdAsync(id);

            if (document == null)
            {
                var error = NotFoundMessage(doc);
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
                var error = NotFoundMessage(input);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }

            if (document.Paid == true && input.Paid == false)
            {
                var result = BadRequestMessage(input, "You can't change the state of a already payed Document");
                var listError = ErrorList(result);
                throw new Exception(listError);
            }

            var mapperDoc = _mapper.Map<Domain.Entities.Document>(input);

            var validator = new DocumentValidator();
            var valid = validator.Validate(mapperDoc);

            var TotalUpdated = mapperDoc.Total - totalValueOld; // VERIFICAR ESTA CONTA, QUANDO PASSAMOS DE UMA OPERAÇÃO PARA A OUTRA?

            if (valid.IsValid)
            {
                if (document.Paid == false && input.Paid == true || TotalUpdated != totalValueOld && document.Paid == true)
                {
                    string description = $"Diference Transaction in Document id: {document.Id}";
                    var type = BankRequest.Domain.Entities.Enum.Type.Revert;
                    decimal total = TotalUpdated;

                    if (document.Paid == false && input.Paid == true)
                    {
                        description = $"Financial Transaction in Document id: {document.Id}";
                        type = BankRequest.Domain.Entities.Enum.Type.Receive;
                        total = input.Total;
                    }

                    //var response = await _bankRequestClient.PostCashBank(Origin.Document, document.Id, description,
                    //        type, total);

                    var bankreqDTO = new BankRequestDTO()
                    {
                        Origin = Origin.Document,
                        OriginId = document.Id,
                        Description = description,
                        Type = type,
                        Amount = total
                    };

                    _messageProducer.PublishMessage(bankreqDTO, "bankrequest");

                    //if (response == false)
                    //{
                    //    var result = BadRequestMessage(input, "There was an error while communicating with the BankRequestAPI, please try again!");
                    //    var listError = ErrorList(result);
                    //    throw new Exception(listError);
                    //}
                }

                await _documentRepository.UpdateAsync(mapperDoc);

                return mapperDoc;
            }
            else
            {
                var errorList = new ErrorMessage<DocumentDTO>(HttpStatusCode.BadRequest.GetHashCode().ToString(),
                    valid.Errors.ConvertAll(x => x.ErrorMessage.ToString()), input);
                var error = ErrorList(errorList);
                throw new Exception(error);
            }
        }

        public async Task<Domain.Entities.Document> ChangeState(Guid id, bool Status)
        {
            var document = await _documentRepository.GetByIdAsync(id);

            var mapperDoc = _mapper.Map<DocumentDTO>(document);

            if (document == null)
            {
                var error = NotFoundMessage(mapperDoc);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }

            if (document.Paid == true)
            {
                var result = BadRequestMessage(mapperDoc, "You can only delete a finalized Document!");
                var listError = ErrorList(result);
                throw new Exception(listError);
            }

            document.Paid = Status;

            await _documentRepository.UpdateAsync(document);

            if (document.Paid == true)
            {
                var operationType = BankRequest.Domain.Entities.Enum.Type.Receive;
                if (document.Operation == Operation.Exit)
                {
                    operationType = BankRequest.Domain.Entities.Enum.Type.Payment;
                }

                //var response = await _bankRequestClient.PostCashBank(Origin.Document, document.Id, $"Financial Transaction id: {document.Id}",
                //     operationType, document.Total);

                var bankreqDTO = new BankRequestDTO()
                {
                    Origin = Origin.Document,
                    OriginId = document.Id,
                    Description = $"Financial Transaction id: {document.Id}",
                    Type = operationType,
                    Amount = document.Total
                };

                _messageProducer.PublishMessage(bankreqDTO, "bankrequest");

                //if (response == false)
                //{
                //    var result = BadRequestMessage(mapperDoc, "There was an error while communicating with the BankRequestAPI, please try again!");
                //    var listError = ErrorList(result);
                //    throw new Exception(listError);
                //}
            }
            return document;
        }

        public async Task<Domain.Entities.Document> DeleteById(Guid id)
        {
            var document = await _documentRepository.GetByIdAsync(id);

            var mapperDoc = _mapper.Map<DocumentDTO>(document);

            if (document == null)
            {
                var error = NotFoundMessage(mapperDoc);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }
            else
            {
                await _documentRepository.DeleteAsync(document);
            }

            if (document.Paid == true)
            {
                //var response = await _bankRequestClient.PostCashBank(Origin.Document, document.Id, $"Revert Document order id: {document.Id}",
                //    BankRequest.Domain.Entities.Enum.Type.Revert, -document.Total);

                var bankreqDTO = new BankRequestDTO()
                {
                    Origin = Origin.Document,
                    OriginId = document.Id,
                    Description = $"Revert Document order id: {document.Id}",
                    Type = BankRequest.Domain.Entities.Enum.Type.Revert,
                    Amount = document.Total
                };

                _messageProducer.PublishMessage(bankreqDTO, "bankrequest");

                //if (response == false)
                //{
                //    var result = BadRequestMessage(mapperDoc, "There was an error while communicating with the BankRequestAPI, please try again!");
                //    var listError = ErrorList(result);
                //    throw new Exception(listError);
                //}
            }

            return document;
        }
    }
}