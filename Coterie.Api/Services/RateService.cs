using System;
using Coterie.Api.Interfaces;
using Coterie.Api.Models.Requests;
using Coterie.Api.Models.Responses;
using Coterie.Api.Models.Shared;

namespace Coterie.Api.Services
{
    public class RateService : IRateService
    {
        private const int HazardFactor = 4;

        public GetRateResponse GetRate(GetRateRequest request)
        {
            var response = new GetRateResponse
            {
                Business = request.Business,
                Revenue = request.Revenue
            };

            foreach (var stateString in request.States)
            {
                var state = StateList.GetState(stateString);
                
                var stateFactor = state switch
                {
                    States.FL => 1.2,
                    States.TX => 0.943,
                    States.OH => 1.0,
                    _ => throw new ArgumentOutOfRangeException()
                };

                var businessFactor = request.Business switch
                {
                    BusinessTypes.Plumber => 0.5,
                    BusinessTypes.Architect => 1,
                    BusinessTypes.Programmer => 1.25,
                    _ => throw new ArgumentOutOfRangeException()
                };

                var total = stateFactor * businessFactor * Math.Ceiling(request.Revenue / 1000.0) * HazardFactor;
                
                response.Premiums.Add(new PremiumDetails
                {
                    State = state.Value,
                    Premium = (int) Math.Round(total, 0, MidpointRounding.AwayFromZero)
                });
            }

            return response;
        }
    }
} 