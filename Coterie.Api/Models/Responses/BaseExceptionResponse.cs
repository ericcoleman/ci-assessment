using System;

namespace Coterie.Api.Models.Responses
{
    public class BaseExceptionResponse
    {
        public BaseExceptionResponse(string message)
        {
            Message = message;
        }
        
        /// <summary>
        /// Whether the request was successful... seems redundant.
        /// </summary>
        public bool IsSuccessful { get; } = false;
        /// <summary>
        /// Unique Id to identify the transaction
        /// </summary>
        /// <example>123-54153-gf42a-farfa4-arg</example>
        public string TransactionId { get; }  = Guid.NewGuid().ToString();
        /// <summary>
        /// A descriptive message for why the request failed
        /// </summary>
        /// <example>Json bad</example>
        public string Message { get; set; }
    }
}