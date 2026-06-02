namespace Application.ViewModel.ExchangeRates
{
    public class ViewModelExchangeRate
    {
        public int Key { get; set; }
        public string SourceCurrencyName { get; set; } = null!;
        public string DestinationCurrencyName { get; set; } = null!;
        public decimal RateValue { get; set; }
        public DateTime EffectiveDate { get; set; }
        public bool State { get; set; }
    }
}