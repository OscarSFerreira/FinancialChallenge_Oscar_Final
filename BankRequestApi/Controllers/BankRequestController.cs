using AutoMapper;
using BankRequest.Application.DTO;
using BankRequest.Application.Interfaces;
using BankRequest.Application.ViewModel;
using BankRequest.Data.Repository;
using BankRequest.Domain.Validator;
using BuyRequest.Data.Repository.BuyRequest;
using Document.Data.Repository;
using FinancialChallenge_Oscar.Infrastructure.ErrorMessage;
using FinancialChallenge_Oscar.Infrastructure.Paging;
using Microsoft.AspNetCore.Http;
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
        BankRequest.Domain.Entities.BankRequest bank = new BankRequest.Domain.Entities.BankRequest();
        public List<string> errorList = new List<string>();

        public BankRequestController(IBankRequestService bankRequestService)
        {
            _bankRequestService = bankRequestService;
        }

        [HttpPost("PostBankRequest")]
        public async Task<IActionResult> Post([FromBody] BankRequestDTO input)
        {
            try
            {
                await _bankRequestService.PostBankRecord(input);
                return Ok(input);
            }
            catch (Exception ex)
            {
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BankRequest.Domain.Entities.BankRequest>(HttpStatusCode.BadRequest.GetHashCode().
                    ToString(), errorList, bank));
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _bankRequestService.GetById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BankRequest.Domain.Entities.BankRequest>(HttpStatusCode.BadRequest.GetHashCode().
                    ToString(), errorList, bank));
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
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BankRequest.Domain.Entities.BankRequest>(HttpStatusCode.BadRequest.GetHashCode().
                    ToString(), errorList, bank));
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
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BankRequest.Domain.Entities.BankRequest>(HttpStatusCode.BadRequest.GetHashCode().
                    ToString(), errorList, bank));
            }
        }

        [HttpPut("ChangeBankRequest/{id}")]
        public async Task<IActionResult> ChangeBankRequest(Guid id, [FromBody] BankRequestDTO bankRecord)
        {
            try
            {
                var bankReqUpdate = await _bankRequestService.ChangeBankRequest(id, bankRecord);
                return Ok(bankReqUpdate);
            }
            catch (Exception ex)
            {
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BankRequest.Domain.Entities.BankRequest>(HttpStatusCode.BadRequest.GetHashCode().
                    ToString(), errorList, bank));
            }
        }

    }
}
