using AutoMapper;
using BankRequest.ClientApi.Interfaces;
using BankRequest.Domain.Entities;
using BankRequest.Domain.Entities.Enum;
using BuyRequest.Application.DTO;
using BuyRequest.Application.Interfaces;
using BuyRequest.Data.Repository.BuyRequest;
using BuyRequest.Domain.Entities.Enum;
using BuyRequest.Domain.Validator;
using FinancialChallenge_Oscar.Infrastructure.ErrorMessage;
using FinancialChallenge_Oscar.Infrastructure.Paging;
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
        private readonly IProductRequestService _productRequestService;
        public IBankRequestClient _bankRequestClient;

        public Domain.Entities.BuyRequest buyReq = new();

        public BuyRequestService(IBuyRequestRepository buyRequestRepository, IProductRequestService productRequestService, IMapper mapper, IBankRequestClient bankRequestClient)
        {
            _buyRequestRepository = buyRequestRepository;
            _productRequestService = productRequestService;
            _mapper = mapper;
            _bankRequestClient = bankRequestClient;
        }

        public string ErrorList(ErrorMessage<Domain.Entities.BuyRequest> error)
        {
            var errorList = "";

            foreach (var item in error.Message)
            {
                errorList += item.ToString() + " ";
            }
            return errorList;
        }

        public async Task<Domain.Entities.BuyRequest> Post(BuyRequestDTO buyinput)
        {
            var mapperBuy = _mapper.Map<Domain.Entities.BuyRequest>(buyinput);

            var buyValidator = new BuyRequestValidator();
            var buyValid = buyValidator.Validate(mapperBuy);

            if (buyValid.IsValid)
            {
                await _buyRequestRepository.AddAsync(mapperBuy);

                //decimal totalprice = await _productRequestService.PostProduct(buyinput.Products, mapperBuy.Id);

                //buyReq.ProductPrices = totalprice;

                //mapperBuy.Status = Status.Received;
                //mapperBuy.TotalPricing = buyReq.ProductPrices - (buyReq.ProductPrices * (buyReq.Discount / 100));

                //await _buyRequestRepository.UpdateAsync(mapperBuy);
            }
            else
            {
                var errorList = new ErrorMessage<Domain.Entities.BuyRequest>(HttpStatusCode.BadRequest.GetHashCode().ToString(),
                        buyValid.Errors.ConvertAll(x => x.ErrorMessage.ToString()), buyReq);

                var error = ErrorList(errorList);
                throw new Exception(error);
            }

            return mapperBuy;

        }

        public async Task<IEnumerable<Domain.Entities.BuyRequest>> GetAll(PageParameter parameters)
        {

            var buyRequest = await _buyRequestRepository.GetAllWithPaging(parameters);

            if (buyRequest.Count() == 0)
            {
                var error = _buyRequestRepository.NotFoundMessage(buyReq);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }

            return buyRequest;
        }

        public async Task<Domain.Entities.BuyRequest> GetById(Guid id)
        {

            var bankrec = await _buyRequestRepository.GetByIdAsync(id);

            if (bankrec == null)
            {
                var error = _buyRequestRepository.NotFoundMessage(buyReq);
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

            var record = await _buyRequestRepository.GetAsync(x => x.ClientId == clientId);

            if (record == null)
            {
                var error = _buyRequestRepository.NotFoundMessage(buyReq);
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

            var request = await _buyRequestRepository.GetByIdAsync(buyinput.Id);

            var oldStatus = request.Status;

            if (request == null)
            {
                var error = _buyRequestRepository.NotFoundMessage(buyReq);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }

            if (request.Status == Status.Finalized && buyinput.Status != Status.Finalized)
            {
                var error = _buyRequestRepository.BadRequestMessage(buyReq, "You can only delete a finalized Request!");
                var listError = ErrorList(error);
                throw new Exception(listError);
            }

            //var Products = await _productRequestService.UpdateByProdIdAsync(buyinput);
            //request.ProductPrices = Products;

            //request.TotalPricing = request.ProductPrices - (request.ProductPrices * (request.Discount / 100));

            var mapperBuy = _mapper.Map<Domain.Entities.BuyRequest>(buyinput);

            var buyValidator = new BuyRequestValidator();
            var buyValid = buyValidator.Validate(mapperBuy);

            if (buyValid.IsValid)
            {
                await _buyRequestRepository.UpdateAsync(mapperBuy);
            }
            else
            {
                var errorList = new ErrorMessage<Domain.Entities.BuyRequest>(HttpStatusCode.BadRequest.GetHashCode().ToString(),
                        buyValid.Errors.ConvertAll(x => x.ErrorMessage.ToString()), buyReq);

                var error = ErrorList(errorList);
                throw new Exception(error);
            }

            if (mapperBuy.Status == Status.Finalized)
            {
                var type = BankRequest.Domain.Entities.Enum.Type.Receive;
                var recentValue = mapperBuy.TotalPricing; //valor recente (total)
                string description = $"Financial transaction order id: {request.Id}";

                if (mapperBuy.Status == oldStatus && mapperBuy.Status == Status.Finalized && recentValue > request.TotalPricing /*&& totalValueOld > mapperBuy.TotalPricing*/)
                {
                    description = $"Diference purchase order id: {request.Id}";
                    recentValue = mapperBuy.TotalPricing - request.TotalPricing /*- totalValueOld*/;
                    type = BankRequest.Domain.Entities.Enum.Type.Receive;
                }
                else if (mapperBuy.Status == oldStatus && mapperBuy.Status == Status.Finalized && request.TotalPricing  > recentValue)
                {
                    description = $"Diference purchase order id: {request.Id}";
                    recentValue = mapperBuy.TotalPricing - request.TotalPricing /*- totalValueOld*/;
                    type = BankRequest.Domain.Entities.Enum.Type.Payment;
                }
                else
                {
                    var result = _buyRequestRepository.BadRequestMessage(buyReq, "There was no change on the Total amount!");
                    var listError = ErrorList(result);
                    throw new Exception(listError);
                }

                var response = await _bankRequestClient.PostCashBank(Origin.PurchaseRequest, mapperBuy.Id, description,
                    type, recentValue);

                if (response == false)
                {
                    var result = _buyRequestRepository.BadRequestMessage(buyReq, "There was an error while communicating with the BankRequestAPI, please try again!");
                    var listError = ErrorList(result);
                    throw new Exception(listError);
                }
            }

            return mapperBuy;

        }

        public async Task<Domain.Entities.BuyRequest> ChangeState(Guid id, Status state)
        {

            var request = await _buyRequestRepository.GetByIdAsync(id);

            if (request == null)
            {
                var error = _buyRequestRepository.NotFoundMessage(buyReq);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }

            if (request.Status == Status.Finalized)
            {
                var result = _buyRequestRepository.BadRequestMessage(buyReq, "You can only delete a finalized Request!");
                var listError = ErrorList(result);
                throw new Exception(listError);
            }

            //await _productRequestService.ChangeStateProd(id, state);

            request.Status = state;

            await _buyRequestRepository.UpdateAsync(request);

            if (request.Status == Status.Finalized)
            {

                var response = await _bankRequestClient.PostCashBank(Origin.PurchaseRequest, request.Id, $"Purshase order id: {request.Id}",
                    BankRequest.Domain.Entities.Enum.Type.Receive, request.TotalPricing);

                if (response == false)
                {
                    var result = _buyRequestRepository.BadRequestMessage(buyReq, "There was an error while communicating with the BankRequestAPI, please try again!");
                    var listError = ErrorList(result);
                    throw new Exception(listError);
                }

            }

            return request;

        }

        public async Task<Domain.Entities.BuyRequest> DeleteById(Guid id)
        {

            var buyRequest = await _buyRequestRepository.GetByIdAsync(id);

            if (buyRequest == null)
            {
                var error = _buyRequestRepository.NotFoundMessage(buyReq);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }

            await _buyRequestRepository.DeleteAsync(buyRequest);

            if (buyRequest.Status == Status.Finalized)
            {

                var response = await _bankRequestClient.PostCashBank(Origin.PurchaseRequest, id, $"Revert Purshase order id: {buyRequest.Id}",
                    BankRequest.Domain.Entities.Enum.Type.Revert, -buyRequest.TotalPricing);

                if (response == false)
                {
                    var result = _buyRequestRepository.BadRequestMessage(buyReq, "There was an error while communicating with the BankRequestAPI, please try again!");
                    var listError = ErrorList(result);
                    throw new Exception(listError);
                }

            }
            return buyRequest;
        }
    }
}
