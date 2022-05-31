using BuyRequest.Application.DTO;
using BuyRequest.Application.Interfaces;
using BuyRequest.Domain.Entities.Enum;
using FinancialChallenge_Oscar.Infrastructure.ErrorMessage;
using FinancialChallenge_Oscar.Infrastructure.Paging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BuyRequestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyRequestController : ControllerBase
    {

        private readonly IBuyRequestService _buyRequestService;

        public BuyRequestController(IBuyRequestService buyRequestService)
        {
            _buyRequestService = buyRequestService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BuyRequestDTO buyinput)
        {
            try
            {
                var result = await _buyRequestService.Post(buyinput);
                return Ok(result);

            }
            catch (Exception ex)
            {
                var errorList = new List<string>();
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BuyRequestDTO>(HttpStatusCode.BadRequest.GetHashCode().
                    ToString(), errorList, buyinput));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _buyRequestService.GetById(id);
                return Ok(result);

            }
            catch (Exception ex)
            {
                var errorList = new List<string>();
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BuyRequestDTO>(HttpStatusCode.NoContent.GetHashCode().
                    ToString(), errorList, new BuyRequestDTO()));
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PageParameter parameters)
        {
            try
            {

                var result = await _buyRequestService.GetAll(parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var errorList = new List<string>();
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BuyRequestDTO>(HttpStatusCode.NoContent.GetHashCode().
                    ToString(), errorList, new BuyRequestDTO()));
            }

        }

        [HttpGet("GetByClientIdAsync/{clientId}")]
        public async Task<IActionResult> GetByClientIdAsync(Guid clientId)
        {
            try
            {
                var result = await _buyRequestService.GetByClientIdAsync(clientId);
                return Ok(result);

            }
            catch (Exception ex)
            {
                var errorList = new List<string>();
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BuyRequestDTO>(HttpStatusCode.NoContent.GetHashCode().
                    ToString(), errorList, new BuyRequestDTO()));
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] BuyRequestDTO buyinput)
        {
            try
            {
                var bank = await _buyRequestService.UpdateAsync(buyinput);

                return Ok(bank);

            }
            catch (Exception ex)
            {
                var errorList = new List<string>();
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BuyRequestDTO>(HttpStatusCode.BadRequest.GetHashCode().
                    ToString(), errorList, buyinput));
            }
        }

        [HttpPut("changeState/{id}")]
        public async Task<IActionResult> ChangeState(Guid id, Status state)
        {
            try
            {
                var request = await _buyRequestService.ChangeState(id, state);
                return Ok(request);
            }
            catch (Exception ex)
            {
                var errorList = new List<string>();
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BuyRequestDTO>(HttpStatusCode.BadRequest.GetHashCode().
                    ToString(), errorList, null));
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            try
            {
                var buyRequest = await _buyRequestService.DeleteById(id);

                return Ok(buyRequest);

            }
            catch (Exception ex)
            {
                var errorList = new List<string>();
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BuyRequestDTO>(HttpStatusCode.BadRequest.GetHashCode().
                    ToString(), errorList, null));
            }
        }

    }
}
