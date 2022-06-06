using AutoMapper;
using BankRequest.Application.DTO;
using BankRequest.Application.Interfaces;
using BankRequest.Application.Model;
using BankRequest.Data.Repository;
using BankRequest.Domain.Validator;
using FinancialChallenge_Oscar.Infrastructure.ErrorMessage;
using FinancialChallenge_Oscar.Infrastructure.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BankRequest.Application.Services
{
    public class BankRequestService : IBankRequestService
    {
        private readonly IBankRequestRepository _bankRequestRepository;
        private readonly IMapper _mapper;

        public BankRequestService(IBankRequestRepository bankRequestRepository, IMapper mapper)
        {
            _mapper = mapper;
            _bankRequestRepository = bankRequestRepository;
        }

        public async Task PostBankRecord(BankRequestDTO input)
        {
            var mapper = _mapper.Map<Domain.Entities.BankRequest>(input);

            var validator = new BankRequestValidator();
            var valid = validator.Validate(mapper);

            if (valid.IsValid)
            {
                await _bankRequestRepository.AddAsync(mapper);
            }
            else
            {
                var errorList = new ErrorMessage<BankRequestDTO>(HttpStatusCode.BadRequest.GetHashCode().ToString(),
                    valid.Errors.ConvertAll(x => x.ErrorMessage.ToString()), input);
                var error = ErrorList(errorList);
                throw new Exception(error);
            }
        }

        public string ErrorList(ErrorMessage<BankRequestDTO> error)
        {
            var errorList = "";

            foreach (var item in error.Message)
            {
                errorList += " " + item.ToString();
            }
            return errorList;
        }

        public ErrorMessage<BankRequestDTO> NotFoundMessage(BankRequestDTO entity)
        {
            var errorList = new List<string>();
            errorList.Add("This database does not contain the data you requested!");
            var error = new ErrorMessage<BankRequestDTO>(HttpStatusCode.NoContent.GetHashCode().ToString(), errorList, entity);
            return error;
        }

        public ErrorMessage<BankRequestDTO> BadRequestMessage(BankRequestDTO entity, string msg)
        {
            var errorList = new List<string>();
            errorList.Add(msg);
            var error = new ErrorMessage<BankRequestDTO>(HttpStatusCode.BadRequest.GetHashCode().ToString(), errorList, entity);
            return error;
        }

        public async Task<BankRequestModel> GetAll(PageParameter parameters)
        {
            BankRequestDTO bank = new BankRequestDTO();
            BankRequestModel bankRecord = new BankRequestModel();

            bankRecord.BankRecords = await _bankRequestRepository.GetAllWithPaging(parameters);

            if (bankRecord.BankRecords.Count() == 0)
            {
                var error = NotFoundMessage(bank);

                var listError = ErrorList(error);
                throw new Exception(listError);
            }
            else
            {
                bankRecord.Total = bankRecord.BankRecords.Sum(rec => rec.Amount);
            }

            return bankRecord;
        }

        public async Task<Domain.Entities.BankRequest> GetById(Guid id)
        {
            BankRequestDTO bank = new BankRequestDTO();
            var bankrec = await _bankRequestRepository.GetByIdAsync(id);

            if (bankrec == null)
            {
                var error = NotFoundMessage(bank);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }
            return bankrec;
        }

        public async Task<Domain.Entities.BankRequest> GetByOriginId(Guid originId)
        {
            BankRequestDTO bank = new BankRequestDTO();
            var bankrec = await _bankRequestRepository.GetAsync(x => x.OriginId == originId);

            if (bankrec == null)
            {
                var error = NotFoundMessage(bank);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }
            return bankrec;
        }

        public async Task<Domain.Entities.BankRequest> ChangeBankRequest(Guid id, BankRequestDTO bankRequest)
        {
            var bankReqUpdate = await _bankRequestRepository.GetByIdAsync(id);
            if (bankReqUpdate == null)
            {
                var error = NotFoundMessage(bankRequest);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }

            if (bankReqUpdate.Origin == null)
            {
                bankRequest.Origin = null;
            }
            else if (bankReqUpdate.OriginId != null)
            {
                var result = BadRequestMessage(bankRequest, "The permissions do not allow you to change this data!");
                var error = ErrorList(result);
                throw new Exception(error);
            }

            var mapBankRecord = _mapper.Map<Domain.Entities.BankRequest>(bankRequest);

            mapBankRecord.Id = id;

            var validator = new BankRequestValidator();
            var valid = validator.Validate(mapBankRecord);

            if (valid.IsValid)
            {
                await _bankRequestRepository.UpdateAsync(mapBankRecord);
            }
            else
            {
                var errorList = new ErrorMessage<BankRequestDTO>(HttpStatusCode.BadRequest.GetHashCode().ToString(),
                 valid.Errors.ConvertAll(x => x.ErrorMessage.ToString()), bankRequest);

                var error = ErrorList(errorList);
                throw new Exception(error);
            }
            return mapBankRecord;
        }
    }
}