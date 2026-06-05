using Application.ViewModel.Base;

namespace Application.ViewModel.TaxConfigurations
{
    public class ViewModelTaxConfiguration : BaseViewModel<int, string>
    {
        public required decimal GeneralItbisPercentage { get; set; }
        public required decimal CustomsServiceRatePercentage { get; set; }
    }
}