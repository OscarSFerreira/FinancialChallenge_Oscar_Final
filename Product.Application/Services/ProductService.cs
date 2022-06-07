using AutoMapper;
using FinancialChallenge_Oscar.Infrastructure.ErrorMessage;
using FinancialChallenge_Oscar.Infrastructure.Paging;
using Product.Application.DTO;
using Product.Application.Interfaces;
using Product.Data.Repository;
using Product.Domain.Entities.Enums;
using Product.Domain.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Product.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public string ErrorList(ErrorMessage<ProductDTO> error)
        {
            var errorList = "";

            foreach (var item in error.Message)
            {
                errorList += item.ToString();
            }
            return errorList;
        }

        public ErrorMessage<ProductDTO> NotFoundMessage(ProductDTO entity)
        {
            var errorList = new List<string>();
            errorList.Add("This database does not contain the data you requested!");
            var error = new ErrorMessage<ProductDTO>(HttpStatusCode.NoContent.GetHashCode().ToString(), errorList, entity);
            return error;
        }

        public ErrorMessage<ProductDTO> BadRequestMessage(ProductDTO entity, string msg)
        {
            var errorList = new List<string>();
            errorList.Add(msg);
            var error = new ErrorMessage<ProductDTO>(HttpStatusCode.BadRequest.GetHashCode().ToString(), errorList, entity);
            return error;
        }

        public async Task Post(ProductDTO input)
        {
            var mapperProd = _mapper.Map<Domain.Entities.Product>(input);

            var validator = new ProductValidator();
            var valid = validator.Validate(mapperProd);

            var products = _productRepository.GetAsync(x => x.GTIN == input.GTIN || x.Description == input.Description);

            if (products.Result != null)
            {
                var result = BadRequestMessage(input, "There is an existing item with this Description or GTIN!");
                var listError = ErrorList(result);
                throw new Exception(listError);
            }

            if (valid.IsValid)
            {
                await _productRepository.AddAsync(mapperProd);
            }
            else
            {
                var errorList = new ErrorMessage<ProductDTO>(HttpStatusCode.BadRequest.GetHashCode().ToString(),
                    valid.Errors.ConvertAll(x => x.ErrorMessage.ToString()), input);
                var error = ErrorList(errorList);
                throw new Exception(error);
            }
        }

        public async Task<IEnumerable<Domain.Entities.Product>> GetAll(PageParameter parameters)
        {
            ProductDTO prod = new ProductDTO();
            var product = await _productRepository.GetAllWithPaging(parameters);

            if (product.Count() == 0)
            {
                var error = NotFoundMessage(prod);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }
            return product;
        }

        public async Task<Domain.Entities.Product> GetById(Guid id)
        {
            ProductDTO prod = new ProductDTO();
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                var error = NotFoundMessage(prod);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }
            return product;
        }

        public async Task<IEnumerable<Domain.Entities.Product>> GetByCategory(ProductCategory category)
        {
            ProductDTO prod = new ProductDTO();
            var product = await _productRepository.GetProductCategoryAsync(x => x.ProductCategory == category);

            if (product == null)
            {
                var error = NotFoundMessage(prod);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }
            return product;
        }

        public async Task<Domain.Entities.Product> ChangeProduct(Guid id, ProductDTO prodRequest)
        {
            var ProdUpdate = await _productRepository.GetByIdAsync(id);
            if (ProdUpdate == null)
            {
                var error = NotFoundMessage(prodRequest);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }

            var mapperProd = _mapper.Map<Domain.Entities.Product>(prodRequest);

            mapperProd.Id = id;

            var products = _productRepository.GetAsync(x => x.GTIN == prodRequest.GTIN || x.Description == prodRequest.Description);

            if (products.Result != null)
            {
                var result = BadRequestMessage(prodRequest, "There is an existing item with this Description or GTIN!");
                var listError = ErrorList(result);
                throw new Exception(listError);
            }

            var validator = new ProductValidator();
            var valid = validator.Validate(mapperProd);

            if (valid.IsValid)
            {
                await _productRepository.UpdateAsync(mapperProd);
            }
            else
            {
                var errorList = new ErrorMessage<ProductDTO>(HttpStatusCode.BadRequest.GetHashCode().ToString(),
                 valid.Errors.ConvertAll(x => x.ErrorMessage.ToString()), prodRequest);

                var error = ErrorList(errorList);
                throw new Exception(error);
            }
            return mapperProd;
        }

        public async Task<Domain.Entities.Product> DeleteById(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            var mapperProd = _mapper.Map<ProductDTO>(product);

            if (product == null)
            {
                var error = NotFoundMessage(mapperProd);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }
            else
            {
                await _productRepository.DeleteAsync(product);
            }

            return product;
        }
    }
}