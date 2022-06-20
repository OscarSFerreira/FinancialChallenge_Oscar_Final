using BankRequest.Application.DTO;
using BankRequest.Application.Interfaces;
using FinancialChallenge_Oscar.Infrastructure.ErrorMessage;
using FinancialChallenge_Oscar.Infrastructure.Paging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BankRequestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankRequestController : ControllerBase
    {
        private readonly IBankRequestService _bankRequestService;

        public BankRequestController(IBankRequestService bankRequestService)
        {
            _bankRequestService = bankRequestService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BankRequestDTO input)
        {
            try
            {
                await _bankRequestService.PostBankRecord(input);
                return Ok(input);
            }
            catch (Exception ex)
            {
                var errorList = new List<string>();
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BankRequestDTO>(HttpStatusCode.BadRequest.GetHashCode().
                    ToString(), errorList, input));
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _bankRequestService.GetById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var errorList = new List<string>();
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BankRequestDTO>(HttpStatusCode.NoContent.GetHashCode(). //comentar isto tudo e meter apenas NoContent()
                    ToString(), errorList, new BankRequestDTO()));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PageParameter parameters)
        {
            try
            {
                var result = await _bankRequestService.GetAll(parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var errorList = new List<string>();
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BankRequestDTO>(HttpStatusCode.NoContent.GetHashCode().
                    ToString(), errorList, new BankRequestDTO()));
            }
        }

        [HttpGet("GetByOriginId")]
        public async Task<IActionResult> GetByOriginId(Guid OriginId)
        {
            try
            {
                var result = await _bankRequestService.GetByOriginId(OriginId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var errorList = new List<string>();
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BankRequestDTO>(HttpStatusCode.NoContent.GetHashCode().
                    ToString(), errorList, new BankRequestDTO()));
            }
        }

        [HttpPut]
        public async Task<IActionResult> ChangeBankRequest(Guid id, [FromBody] BankRequestDTO bankRecord)
        {
            try
            {
                var bankReqUpdate = await _bankRequestService.ChangeBankRequest(id, bankRecord);
                return Ok(bankReqUpdate);
            }
            catch (Exception ex)
            {
                var errorList = new List<string>();
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BankRequestDTO>(HttpStatusCode.BadRequest.GetHashCode().
                    ToString(), errorList, bankRecord));
            }
        }
    }
}