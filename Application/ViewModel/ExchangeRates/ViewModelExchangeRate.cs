using Application.ViewModel.Base;

namespace Application.ViewModel.ExchangeRates
{
    public class ViewModelExchangeRate : BaseViewModel<int, string>
    {
        public required string SourceCurrencyName { get; set; }
        public required string DestinationCurrencyName { get; set; }
        public required decimal RateValue { get; set; }
        public required DateTime EffectiveDate { get; set; }
    }
}