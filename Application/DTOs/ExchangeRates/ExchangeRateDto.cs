namespace Application.DTOs.ExchangeRates
{
    public class ExchangeRateDto
    {
        public int Key { get; set; }
        public int SourceCurrencyId { get; set; }
        public string? SourceCurrencyName { get; set; }
        public int DestinationCurrencyId { get; set; }
        public string? DestinationCurrencyName { get; set; }
        public decimal RateValue { get; set; }
        public DateTime EffectiveDate { get; set; }
        public bool State { get; set; }
    }
}