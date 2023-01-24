using System.Collections.Generic;
using Coterie.Api.Models.Shared;

namespace Coterie.Api.Models.Responses
{
    public class GetRateResponse : BaseSuccessResponse
    {
        /// <summary>
        /// The type of business
        /// </summary>
        /// <example>Plumber</example>
        public BusinessTypes Business { get; set; }
        /// <summary>
        /// Company Revenue
        /// </summary>
        /// <example>600000</example>
        public long Revenue { get; set; }
        /// <summary>
        /// List of premium details
        /// </summary>
        public List<PremiumDetails> Premiums { get; set; } = new();
    }
}