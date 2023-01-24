namespace Coterie.Api.Models.Shared
{
    public class PremiumDetails
    {
        /// <summary>
        /// The State abbreviation the premium is applicable for
        /// </summary>
        /// <example>TX</example>
        public States State { get; set; }
        /// <summary>
        /// The Premium Amount based on job, revenue, and state
        /// </summary>
        /// <example>11143</example>
        public int Premium { get; set; }
    }
}