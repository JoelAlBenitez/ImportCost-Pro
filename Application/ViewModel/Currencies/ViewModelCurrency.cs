using Application.ViewModel.Base;

namespace Application.ViewModel.Currencies
{
    public class ViewModelCurrency : BaseViewModel<int, string>
    {
        public required string IsoCode { get; set; }
        public required string Symbol { get; set; }
        public required bool IsLocalCurrency { get; set; }
    }
}