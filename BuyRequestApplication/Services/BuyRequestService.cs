using AutoMapper;
using BankRequest.Application.DTO;
using BankRequest.ClientApi.Interfaces;
using BankRequest.Domain.Entities.Enum;
using BuyRequest.Application.DTO;
using BuyRequest.Application.Interfaces;
using BuyRequest.Data.Repository.BuyRequest;
using BuyRequest.Data.Repository.ProductRequest;
using BuyRequest.Domain.Entities.Enum;
using BuyRequest.Domain.Validator;
using FinancialChallenge_Oscar.Infrastructure.ErrorMessage;
using FinancialChallenge_Oscar.Infrastructure.Paging;
using FinancialChallenge_Oscar.Infrastructure.RabbitMQ.Generic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BuyRequest.Application.Services
{
    public class BuyRequestService : IBuyRequestService
    {
        private readonly IBuyRequestRepository _buyRequestRepository;
        private readonly IMapper _mapper;

        //public IBankRequestClient _bankRequestClient;
        private readonly IMessageProducer _messageProducer;

        public IProductRequestRepository _productRequestRepository;
        public IProductRequestService _productRequestService;

        public BuyRequestService(IBuyRequestRepository buyRequestRepository, IMapper mapper, IBankRequestClient bankRequestClient, IProductRequestRepository productRequestRepository, IProductRequestService productRequestService, IMessageProducer messageProducer)
        {
            _buyRequestRepository = buyRequestRepository;
            _mapper = mapper;
            //_bankRequestClient = bankRequestClient;
            _productRequestRepository = productRequestRepository;
            _productRequestService = productRequestService;
            _messageProducer = messageProducer;
        }

        public string ErrorList(ErrorMessage<BuyRequestDTO> error)
        {
            var errorList = "";

            foreach (var item in error.Message)
            {
                errorList += item.ToString() + " ";
            }
            return errorList;
        }

        public ErrorMessage<BuyRequestDTO> NotFoundMessage(BuyRequestDTO entity)
        {
            var errorList = new List<string>();
            errorList.Add("This database does not contain the data you requested!");
            var error = new ErrorMessage<BuyRequestDTO>(HttpStatusCode.NoContent.GetHashCode().ToString(), errorList, entity);
            return error;
        }

        public ErrorMessage<BuyRequestDTO> BadRequestMessage(BuyRequestDTO entity, string msg)
        {
            var errorList = new List<string>();
            errorList.Add(msg);
            var error = new ErrorMessage<BuyRequestDTO>(HttpStatusCode.BadRequest.GetHashCode().ToString(), errorList, entity);
            return error;
        }

        public async Task<Domain.Entities.BuyRequest> Post(BuyRequestDTO buyinput)
        {
            var mapperBuy = _mapper.Map<Domain.Entities.BuyRequest>(buyinput);

            var buyValidator = new BuyRequestValidator();
            var buyValid = buyValidator.Validate(mapperBuy);

            if (mapperBuy.Status == Status.Received)
            {
                mapperBuy.DeliveryDate = DateTimeOffset.MinValue;
            }

            if (buyValid.IsValid)
            {
                await _buyRequestRepository.AddAsync(mapperBuy);
            }
            else
            {
                var errorList = new ErrorMessage<BuyRequestDTO>(HttpStatusCode.BadRequest.GetHashCode().ToString(),
                        buyValid.Errors.ConvertAll(x => x.ErrorMessage.ToString()), buyinput);
                var error = ErrorList(errorList);
                throw new Exception(error);
            }
            return mapperBuy;
        }

        public async Task<IEnumerable<Domain.Entities.BuyRequest>> GetAll(PageParameter parameters)
        {
            BuyRequestDTO buyReq = new BuyRequestDTO();
            var buyRequest = await _buyRequestRepository.GetAllWithPaging(parameters);

            if (buyRequest.Count() == 0)
            {
                var error = NotFoundMessage(buyReq);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }
            return buyRequest;
        }

        public async Task<Domain.Entities.BuyRequest> GetById(Guid id)
        {
            BuyRequestDTO buyReq = new BuyRequestDTO();
            var bankrec = await _buyRequestRepository.GetByIdAsync(id);

            if (bankrec == null)
            {
                var error = NotFoundMessage(buyReq);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }
            else
            {
                return bankrec;
            }
        }

        public async Task<Domain.Entities.BuyRequest> GetByClientIdAsync(Guid clientId)
        {
            BuyRequestDTO buyReq = new BuyRequestDTO();
            var record = await _buyRequestRepository.GetAsync(x => x.ClientId == clientId);

            if (record == null)
            {
                var error = NotFoundMessage(buyReq);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }
            else
            {
                return record;
            }
        }

        public async Task<Domain.Entities.BuyRequest> UpdateAsync(BuyRequestDTO buyinput)
        {
            //var request = await _buyRequestRepository.GetAsync(w => w.Id == buyinput.Id);
            var request = await _buyRequestRepository.GetByIdAsync(buyinput.Id);

            if (request == null)
            {
                var error = NotFoundMessage(buyinput);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }

            if (request.Status == Status.Finalized && buyinput.Status != Status.Finalized)
            {
                var error = BadRequestMessage(buyinput, "You can only delete a finalized Request!");
                var listError = ErrorList(error);
                throw new Exception(listError);
            }

            var mapperBuy = _mapper.Map<Domain.Entities.BuyRequest>(buyinput);

            var buyValidator = new BuyRequestValidator();
            var buyValid = buyValidator.Validate(mapperBuy);

            if (buyValid.IsValid)
            {
                var buyReqProdId = buyinput.BuyRequestProducts.Select(x => x.Id).ToList();
                var result = _productRequestService.GetAllProducts().Result.Where(x => x.BuyRequestId == buyinput.Id);
                var prodDelete = result.Where(x => !buyReqProdId.Contains(x.Id)).ToList();
                //var prodDelete = request.BuyRequestProducts.Where(x => !mapperBuy.BuyRequestProducts.Any(z => z.Id == x.Id)).ToList();

                foreach (var prods in prodDelete)
                {
                    var prod = _productRequestRepository.GetByIdAsync(prods.Id).Result;
                    await _productRequestRepository.DeleteAsync(prod);
                }

                await _buyRequestRepository.UpdateAsync(mapperBuy);
            }
            else
            {
                var errorList = new ErrorMessage<BuyRequestDTO>(HttpStatusCode.BadRequest.GetHashCode().ToString(),
                        buyValid.Errors.ConvertAll(x => x.ErrorMessage.ToString()), buyinput);
                var error = ErrorList(errorList);
                throw new Exception(error);
            }

            if (mapperBuy.Status == Status.Finalized)
            {
                var type = BankRequest.Domain.Entities.Enum.Type.Receive;
                var recentValue = mapperBuy.TotalPricing;
                string description = $"Financial transaction order id: {request.Id}";
                var oldStatus = request.Status;

                if (mapperBuy.Status == oldStatus && mapperBuy.Status == Status.Finalized && recentValue > request.TotalPricing)
                {
                    description = $"Diference purchase order id: {request.Id}";
                    recentValue = mapperBuy.TotalPricing - request.TotalPricing;
                    type = BankRequest.Domain.Entities.Enum.Type.Receive;
                }
                else if (mapperBuy.Status == oldStatus && mapperBuy.Status == Status.Finalized && request.TotalPricing > recentValue)
                {
                    description = $"Diference purchase order id: {request.Id}";
                    recentValue = mapperBuy.TotalPricing - request.TotalPricing;
                    type = BankRequest.Domain.Entities.Enum.Type.Payment;
                }
                else
                {
                    var result = BadRequestMessage(buyinput, "There was no change on the Total amount!");
                    var listError = ErrorList(result);
                    throw new Exception(listError);
                }

                var bankreqDTO = new BankRequestDTO()
                {
                    Origin = Origin.PurchaseRequest,
                    OriginId = mapperBuy.Id,
                    Description = description,
                    Type = type,
                    Amount = recentValue
                };

                _messageProducer.PublishMessage(bankreqDTO, "bankrequest");

                //var response = await _bankRequestClient.PostCashBank(Origin.PurchaseRequest, mapperBuy.Id, description,
                //    type, recentValue);

                //if (response == false)
                //{
                //    var result = BadRequestMessage(buyinput, "There was an error while communicating with the BankRequestAPI, please try again!");
                //    var listError = ErrorList(result);
                //    throw new Exception(listError);
                //}
            }
            return mapperBuy;
        }

        public async Task<Domain.Entities.BuyRequest> ChangeState(Guid id, Status state)
        {
            var request = await _buyRequestRepository.GetByIdAsync(id);

            var mapperBuy = _mapper.Map<BuyRequestDTO>(request);

            if (request == null)
            {
                var error = NotFoundMessage(mapperBuy);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }

            if (request.Status == Status.Finalized)
            {
                var result = BadRequestMessage(mapperBuy, "You can only delete a finalized Request!");
                var listError = ErrorList(result);
                throw new Exception(listError);
            }

            request.Status = state;
            request.DeliveryDate = DateTimeOffset.UtcNow;

            await _buyRequestRepository.UpdateAsync(request);

            if (request.Status == Status.Finalized)
            {
                //var response = await _bankRequestClient.PostCashBank(Origin.PurchaseRequest, request.Id, $"Purshase order id: {request.Id}",
                //    BankRequest.Domain.Entities.Enum.Type.Receive, request.TotalPricing);

                var bankreqDTO = new BankRequestDTO()
                {
                    Origin = Origin.PurchaseRequest,
                    OriginId = request.Id,
                    Description = $"Purshase order id: {request.Id}",
                    Type = BankRequest.Domain.Entities.Enum.Type.Receive,
                    Amount = request.TotalPricing
                };

                _messageProducer.PublishMessage(bankreqDTO, "bankrequest");

                //if (response == false)
                //{
                //    var result = BadRequestMessage(mapperBuy, "There was an error while communicating with the BankRequestAPI, please try again!");
                //    var listError = ErrorList(result);
                //    throw new Exception(listError);
                //}
            }
            return request;
        }

        public async Task<Domain.Entities.BuyRequest> DeleteById(Guid id)
        {
            var buyRequest = await _buyRequestRepository.GetByIdAsync(id);

            var mapperBuy = _mapper.Map<BuyRequestDTO>(buyRequest);

            if (buyRequest == null)
            {
                var error = NotFoundMessage(mapperBuy);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }

            await _buyRequestRepository.DeleteAsync(buyRequest);

            if (buyRequest.Status == Status.Finalized)
            {
                //var response = await _bankRequestClient.PostCashBank(Origin.PurchaseRequest, id, $"Revert Purshase order id: {buyRequest.Id}",
                //    BankRequest.Domain.Entities.Enum.Type.Revert, -buyRequest.TotalPricing);

                var bankreqDTO = new BankRequestDTO()
                {
                    Origin = Origin.PurchaseRequest,
                    OriginId = id,
                    Description = $"Revert Purshase order id: {buyRequest.Id}",
                    Type = BankRequest.Domain.Entities.Enum.Type.Revert,
                    Amount = -buyRequest.TotalPricing
                };

                _messageProducer.PublishMessage(bankreqDTO, "bankrequest");

                //if (response == false)
                //{
                //    var result = BadRequestMessage(mapperBuy, "There was an error while communicating with the BankRequestAPI, please try again!");
                //    var listError = ErrorList(result);
                //    throw new Exception(listError);
                //}
            }
            return buyRequest;
        }
    }
}