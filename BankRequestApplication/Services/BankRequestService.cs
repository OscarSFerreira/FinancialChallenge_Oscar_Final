﻿using AutoMapper;
using BankRequest.Application.DTO;
using BankRequest.Application.Interfaces;
using BankRequest.Application.ViewModel;
using BankRequest.Data.Repository;
using BankRequest.Domain.Entities.Enum;
using BankRequest.Domain.Validator;
using FinancialChallenge_Oscar.Infrastructure.ErrorMessage;
using FinancialChallenge_Oscar.Infrastructure.Paging;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BankRequest.Application.Services
{
    public class BankRequestService : IBankRequestService
    {

        private readonly IBankRequestRepository _bankRequestRepository;
        //private readonly IDocumentRepository _documentRepository;
        //private readonly IBuyRequestRepository _buyRequestRepository;
        private readonly IMapper _mapper;
        Domain.Entities.BankRequest bank = new Domain.Entities.BankRequest();

        public BankRequestService(IBankRequestRepository bankRequestRepository/*, IDocumentRepository documentRepository*/, IMapper mapper/*, IBuyRequestRepository buyRequestRepository*/)
        {
            _mapper = mapper;
            _bankRequestRepository = bankRequestRepository;
            //_documentRepository = documentRepository;
            //_buyRequestRepository = buyRequestRepository;
        }

        public async Task PostBankRecord(BankRequestDTO input)
        {
            var mapper = _mapper.Map<Domain.Entities.BankRequest>(input);

            var validator = new BankRequestValidator();
            var valid = validator.Validate(mapper);

            if (input.Origin == Origin.Null)
            {
                mapper.OriginId = null;
            }

            if (valid.IsValid)
            {
                await _bankRequestRepository.AddAsync(mapper);
            }
            else
            {
                var errorList = new ErrorMessage<Domain.Entities.BankRequest>(HttpStatusCode.BadRequest.GetHashCode().ToString(),
                    valid.Errors.ConvertAll(x => x.ErrorMessage.ToString()), bank);

                var error = ErrorList(errorList);
                throw new Exception(error);
            }

        }

        public string ErrorList(ErrorMessage<Domain.Entities.BankRequest> error)
        {
            var errorList = "";

            foreach (var item in error.Message)
            {
                errorList += item.ToString();
            }
            return errorList;
        }

        public async Task<BankRequestViewModel> GetAll(PageParameter parameters)
        {

            BankRequestViewModel bankRecord = new BankRequestViewModel();

            bankRecord.BankRecords = _bankRequestRepository.GetAllWithPaging(parameters).OrderBy(rec => rec.Id).ToList();

            if (bankRecord.BankRecords.Count == 0)
            {
                var error = _bankRequestRepository.NotFoundMessage(bank);

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

            var bankrec = await _bankRequestRepository.GetByIdAsync(id);

            if (bankrec == null)
            {
                var error = _bankRequestRepository.NotFoundMessage(bank);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }
            return bankrec;
        }

        public async Task<Domain.Entities.BankRequest> GetByOriginId(Guid OriginId)
        {

            var bankrec = await _bankRequestRepository.GetByOriginIdAsync(OriginId);

            if (bankrec == null)
            {
                var error = _bankRequestRepository.NotFoundMessage(bank);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }
            return bankrec;
        }

        public async Task<Domain.Entities.BankRequest> ChangeBankRequest(Guid id, BankRequestDTO bankRecord)
        {
            var bankReqUpdate = await _bankRequestRepository.GetByIdAsync(id);
            if (bankReqUpdate == null)
            {
                var error = _bankRequestRepository.NotFoundMessage(bank);
                var listError = ErrorList(error);
                throw new Exception(listError);
            }

            if (bankReqUpdate.Origin == Origin.Null)
            {
                bankReqUpdate.OriginId = null;
                bankRecord.OriginId = null;
                bankReqUpdate.Origin = Origin.Null;
            }
            else if (bankReqUpdate.OriginId != null)
            {
                var result = _bankRequestRepository.BadRequestMessage(bank, "The permissions do not allow you to change this data!");
                var error = ErrorList(result);
                throw new Exception(error);
            }

            var mapBankRecord = _mapper.Map(bankRecord, bankReqUpdate);

            var validator = new BankRequestValidator();
            var valid = validator.Validate(mapBankRecord);

            if (valid.IsValid)
            {
                await _bankRequestRepository.UpdateAsync(bankReqUpdate);
            }
            else
            {
                var errorList = new ErrorMessage<Domain.Entities.BankRequest>(HttpStatusCode.BadRequest.GetHashCode().ToString(),
                 valid.Errors.ConvertAll(x => x.ErrorMessage.ToString()), bank);

                var error = ErrorList(errorList);
                throw new Exception(error);
            }
            return bankReqUpdate;
        }

    }
}