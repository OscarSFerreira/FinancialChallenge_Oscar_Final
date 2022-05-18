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
        public BuyRequest.Domain.Entities.BuyRequest buyReq = new();
        public List<string> errorList = new List<string>();

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
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BuyRequest.Domain.Entities.BuyRequest>(HttpStatusCode.BadRequest.GetHashCode().
                    ToString(), errorList, buyReq));
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
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BuyRequest.Domain.Entities.BuyRequest>(HttpStatusCode.BadRequest.GetHashCode().
                    ToString(), errorList, buyReq));
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
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BuyRequest.Domain.Entities.BuyRequest>(HttpStatusCode.BadRequest.GetHashCode().
                    ToString(), errorList, buyReq));
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
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BuyRequest.Domain.Entities.BuyRequest>(HttpStatusCode.BadRequest.GetHashCode().
                    ToString(), errorList, buyReq));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] BuyRequestDTO buyinput)
        {

            try
            {
                var bank = await _buyRequestService.UpdateAsync(id, buyinput);

                //if (request.Status == Status.Finalized)
                //{
                //    var type = DesafioFinanceiro_Oscar.Domain.Entities.Type.Receive;
                //    var recentValue = mapperBuy.TotalPricing; //valor recente (total)
                //    string description = $"Financial transaction order id: {request.Id}";

                //    if (mapperBuy.Status == oldStatus && mapperBuy.Status == Status.Finalized && totalValueOld > mapperBuy.TotalPricing)
                //    {
                //        description = $"Diference purchase order id: {request.Id}";
                //        recentValue = mapperBuy.TotalPricing - totalValueOld;
                //        type = DesafioFinanceiro_Oscar.Domain.Entities.Type.Payment;
                //    }

                //    var response = await _bankRecordRepository.CreateBankRecord(Origin.PurchaseRequest, mapperBuy.Id, description,
                //        type, recentValue);

                //    if (!response.IsSuccessStatusCode)
                //    {
                //        var result = _bankRecordRepository.BadRequestMessage(bank, response.Content.ToString());
                //        return StatusCode((int)HttpStatusCode.BadRequest, result);
                //    }
                //}

                return Ok(bank);

            }
            catch (Exception ex)
            {
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BuyRequest.Domain.Entities.BuyRequest>(HttpStatusCode.BadRequest.GetHashCode().
                    ToString(), errorList, buyReq));
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
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BuyRequest.Domain.Entities.BuyRequest>(HttpStatusCode.BadRequest.GetHashCode().
                    ToString(), errorList, buyReq));
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
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BuyRequest.Domain.Entities.BuyRequest>(HttpStatusCode.BadRequest.GetHashCode().
                    ToString(), errorList, buyReq));
            }

        }

    }
}
