using FinancialChallenge_Oscar.Infrastructure.ErrorMessage;
using FinancialChallenge_Oscar.Infrastructure.Paging;
using Microsoft.AspNetCore.Mvc;
using Product.Application.DTO;
using Product.Application.Interfaces;
using Product.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductDTO input)
        {
            try
            {
                await _productService.Post(input);
                return Ok(input);
            }
            catch (Exception ex)
            {
                var errorList = new List<string>();
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<ProductDTO>(HttpStatusCode.BadRequest.GetHashCode().
                    ToString(), errorList, input));
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] PageParameter parameters)
        {
            try
            {
                var result = await _productService.GetAll(parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var errorList = new List<string>();
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<ProductDTO>(HttpStatusCode.NoContent.GetHashCode().
                    ToString(), errorList, new ProductDTO()));
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(Guid OriginId)
        {
            try
            {
                var result = await _productService.GetById(OriginId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var errorList = new List<string>();
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<ProductDTO>(HttpStatusCode.NoContent.GetHashCode().
                    ToString(), errorList, new ProductDTO()));
            }
        }

        [HttpGet("GetByCategory")]
        public async Task<IActionResult> GetByProductCategory(ProductCategory category)
        {
            try
            {
                var result = await _productService.GetByCategory(category);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var errorList = new List<string>();
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<ProductDTO>(HttpStatusCode.NoContent.GetHashCode().
                    ToString(), errorList, new ProductDTO()));
            }
        }

        [HttpPut]
        public async Task<IActionResult> ChangeProduct(Guid id, [FromBody] ProductDTO product)
        {
            try
            {
                var result = await _productService.ChangeProduct(id, product);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var errorList = new List<string>();
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<ProductDTO>(HttpStatusCode.BadRequest.GetHashCode().
                    ToString(), errorList, product));
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            try
            {
                var result = await _productService.DeleteById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var errorList = new List<string>();
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<Product.Domain.Entities.Product>(HttpStatusCode.BadRequest.GetHashCode().
                    ToString(), errorList, null));
            }
        }
    }
}