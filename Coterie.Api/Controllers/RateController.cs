using System;
using System.Linq;
using Coterie.Api.Interfaces;
using Coterie.Api.Models.Requests;
using Coterie.Api.Models.Responses;
using Coterie.Api.Models.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Coterie.Api.Controllers
{
    public class RateController : ControllerBase
    {
        private readonly IRateService _rateService;

        public RateController(IRateService rateService)
        {
            _rateService = rateService;
        }

        /// <summary>
        /// Calculate a premium rate for a business
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("rates")]
        [ProducesResponseType(typeof(GetRateResponse), 200)]
        [ProducesResponseType(typeof(BaseExceptionResponse), 400)]
        public ActionResult<GetRateResponse> CalculateRate([FromBody] GetRateRequest request)
        {
            // In the real world, this logic would be offloaded to FluentValidation or something, but for sample code this is fine.
            if (request.States?.Any() != true)
                return BadRequest(new BaseExceptionResponse("Must provide at least one state"));
            
            var invalidStates = request.States.Where(x => StateList.GetState(x) == null).ToList();

            if (invalidStates.Any())
                return BadRequest(new BaseExceptionResponse($"Invalid State(s) provided: {string.Join(',', invalidStates)}"));

            if (request.Revenue <= 0)
                return BadRequest(new BaseExceptionResponse("Revenue must be a positive whole number"));

            if (!Enum.IsDefined(request.Business))
                return BadRequest(new BaseExceptionResponse($"{request.Business} is not a valid business type"));
            
            return _rateService.GetRate(request);
        }
    }
}