using AutoMapper;
using BuyRequest.Application.DTO;
using BuyRequest.Application.Interfaces;
using BuyRequest.Data.Repository.ProductRequest;
using BuyRequest.Domain.Entities.Enum;
using BuyRequest.Domain.Validator;
using FinancialChallenge_Oscar.Infrastructure.ErrorMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BuyRequest.Application.Services
{
    public class ProductRequestService : IProductRequestService
    {
        private readonly IProductRequestRepository _productRequestRepository;
        private readonly IMapper _mapper;
        Domain.Entities.ProductRequest prodReq = new();

        public ProductRequestService(IProductRequestRepository productRequestRepository, IMapper mapper)
        {
            _productRequestRepository = productRequestRepository;
            _mapper = mapper;
        }

        public string ErrorList(ErrorMessage<Domain.Entities.ProductRequest> error)
        {
            var errorList = "";

            foreach (var item in error.Message)
            {
                errorList += item.ToString();
            }
            return errorList;
        }

        public async Task<decimal> PostProduct(List<ProductRequestDTO> prodInput, Guid RequestId)
        {
            decimal productPrice = 0;
            var lastProdType = prodInput.FirstOrDefault().ProductCategory;

            foreach (var product in prodInput)
            {
                if (product.ProductCategory != lastProdType)
                {
                    var result = _productRequestRepository.BadRequestMessage(prodReq, "A Request can't have 2 different item category!");
                    var error = ErrorList(result);
                    throw new Exception(error);
                }

                var mapperProd = _mapper.Map(product, prodReq);

                var prodValidator = new ProductRequestValidator();
                var prodValid = prodValidator.Validate(mapperProd);

                if (prodValid.IsValid)
                {
                    mapperProd.RequestId = RequestId;
                    mapperProd.Id = Guid.NewGuid();
                    mapperProd.ProductId = Guid.NewGuid();

                    await _productRequestRepository.AddAsync(mapperProd);

                    productPrice += prodReq.Total;

                }
                else
                {
                    var errorList = new ErrorMessage<Domain.Entities.ProductRequest>(HttpStatusCode.BadRequest.GetHashCode().ToString(),
                        prodValid.Errors.ConvertAll(x => x.ErrorMessage.ToString()), prodReq);

                    var error = ErrorList(errorList);
                    throw new Exception(error);
                }
            }
            return productPrice;
        }

        public async Task<decimal> UpdateByProdIdAsync(Guid id, BuyRequestDTO buyRequest)
        {
            var products = _productRequestRepository.GetAllByRequestId(id).ToList();

            if (products == null)
            {
                var error = _productRequestRepository.NotFoundMessage(prodReq);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }

            if (buyRequest.Products.FirstOrDefault().ProductCategory == ProductCategory.Physical)
            {
                if (buyRequest.Status == Status.WaitingDownload)
                {
                    var result = _productRequestRepository.BadRequestMessage(prodReq, "A Physical product can't be set to Waiting To Download status!");
                    var error = ErrorList(result);
                    throw new Exception(error);
                }
            }
            else
            {
                if (buyRequest.Status == Status.WaitingDelivery)
                {
                    var result = _productRequestRepository.BadRequestMessage(prodReq, "A Digital product can't be set to Waiting To Delivery status!");
                    var error = ErrorList(result);
                    throw new Exception(error);
                }
            }

            //var oldStatus = request.Status;
            //var totalValueOld = request.TotalPricing;
            int smallerAmount = products.Count();
            decimal requestPrices = 0;

            if (products.Count() < buyRequest.Products.Count())
            {
                for (int i = products.Count(); i < buyRequest.Products.Count(); i++)
                {

                    var mapperProd = _mapper.Map(buyRequest.Products[i], prodReq);

                    mapperProd.RequestId = id;
                    mapperProd.Id = Guid.NewGuid();
                    mapperProd.ProductId = Guid.NewGuid();
                    mapperProd.Total = mapperProd.ProductQuantity * mapperProd.ProductPrice;
                    requestPrices += mapperProd.Total;

                    var prodValidator = new ProductRequestValidator();
                    var prodValid = prodValidator.Validate(mapperProd);

                    if (prodValid.IsValid)
                    {
                        await _productRequestRepository.AddAsync(mapperProd);
                    }
                    else
                    {
                        var errorList = new ErrorMessage<Domain.Entities.ProductRequest>(HttpStatusCode.BadRequest.GetHashCode().ToString(),
                        prodValid.Errors.ConvertAll(x => x.ErrorMessage.ToString()), prodReq);

                        var error = ErrorList(errorList);
                        throw new Exception(error);
                    }
                }
            }
            else if (products.Count() > buyRequest.Products.Count())
            {
                smallerAmount = buyRequest.Products.Count();

                for (int i = buyRequest.Products.Count(); i < products.Count(); i++)
                {
                    await _productRequestRepository.DeleteAsync(products[i]);
                }

            }

            for (int i = 0; i < smallerAmount; i++)
            {
                products[i].Total = buyRequest.Products[i].ProductPrice * buyRequest.Products[i].ProductQuantity;
                var mapperProd = _mapper.Map(buyRequest.Products[i], products[i]);
                mapperProd.ProductId = Guid.NewGuid();

                await _productRequestRepository.UpdateAsync(mapperProd);

                requestPrices += products[i].Total;
            }

            return requestPrices;

        }

        public async Task ChangeStateProd(Guid id, Status state)
        {
            var product = _productRequestRepository.GetAllByRequestId(id).FirstOrDefault();

            if (product == null)
            {
                var error = _productRequestRepository.NotFoundMessage(prodReq);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }

            if (product.ProductCategory == ProductCategory.Physical)
            {
                if (state == Status.WaitingDownload)
                {
                    var result = _productRequestRepository.BadRequestMessage(prodReq, "A Physical product can't be set to Waiting To Download status!");
                    var error = ErrorList(result);
                    throw new Exception(error);
                }
            }
            else
            {
                if (state == Status.WaitingDelivery)
                {
                    var result = _productRequestRepository.BadRequestMessage(prodReq, "A Digital product can't be set to Waiting To Delivery status!");
                    var error = ErrorList(result);
                    throw new Exception(error);
                }

            }

        }

    }

}
