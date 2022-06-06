using Document.Application.DTO;
using Document.Application.Interfaces;
using FinancialChallenge_Oscar.Infrastructure.ErrorMessage;
using FinancialChallenge_Oscar.Infrastructure.Paging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DocumentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DocumentDTO input)
        {
            try
            {
                var document = await _documentService.Post(input);
                return Ok(document);
            }
            catch (Exception ex)
            {
                var errorList = new List<string>();
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<DocumentDTO>(HttpStatusCode.BadRequest.GetHashCode().
                    ToString(), errorList, input));
            }
        }

        [HttpGet("GetAllDocument")]
        public async Task<IActionResult> GetAll([FromQuery] PageParameter parameters)
        {
            try
            {
                var document = await _documentService.GetAll(parameters);
                return Ok(document);
            }
            catch (Exception ex)
            {
                var errorList = new List<string>();
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<DocumentDTO>(HttpStatusCode.NoContent.GetHashCode().
                    ToString(), errorList, new DocumentDTO()));
            }
        }

        [HttpGet("GetDocumentById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var document = await _documentService.GetById(id);
                return Ok(document);
            }
            catch (Exception ex)
            {
                var errorList = new List<string>();
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<DocumentDTO>(HttpStatusCode.NoContent.GetHashCode().
                    ToString(), errorList, new DocumentDTO()));
            }
        }

        [HttpPut("ChangeDocument/{id}")]
        public async Task<IActionResult> ChangeDocument(Guid id, [FromBody] DocumentDTO input)
        {
            try
            {
                var document = await _documentService.ChangeDocument(id, input);
                return Ok(document);
            }
            catch (Exception ex)
            {
                var errorList = new List<string>();
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<DocumentDTO>(HttpStatusCode.BadRequest.GetHashCode().
                    ToString(), errorList, input));
            }
        }

        [HttpPut("changeState/{id}")]
        public async Task<IActionResult> ChangeState(Guid id, bool Status)
        {
            try
            {
                var document = await _documentService.ChangeState(id, Status);
                return Ok(document);
            }
            catch (Exception ex)
            {
                var errorList = new List<string>();
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<DocumentDTO>(HttpStatusCode.BadRequest.GetHashCode().
                    ToString(), errorList, null)); //perguntar ao pedro
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            try
            {
                var document = await _documentService.DeleteById(id);
                return Ok(document);
            }
            catch (Exception ex)
            {
                var errorList = new List<string>();
                errorList.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<Document.Domain.Entities.Document>(HttpStatusCode.BadRequest.GetHashCode().
                    ToString(), errorList, null));
            }
        }

    }
}
