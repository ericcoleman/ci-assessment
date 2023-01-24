using System.Collections.Generic;
using Coterie.Api.Models.Shared;

namespace Coterie.Api.Models.Requests
{
    public class GetRateRequest
    {
        /// <summary>
        /// The Type of Business the Rate is for
        /// </summary>
        /// <example>Plumber</example>
        public BusinessTypes Business { get; set; } // If concerned about user input, this could be nullable and check for existence on json
        /// <summary>
        /// Annual Revenue of the company
        /// </summary>
        /// <example>600000</example>
        public int Revenue { get; set; } 
        /// <summary>
        /// List of states the company would like quotes for, currently Texas, Ohio, and Florida Supported
        /// </summary>
        /// <example>TX</example>
        public List<string> States { get; set; }
    }
}