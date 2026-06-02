namespace Application.ViewModel.TaxConfigurations
{
    public class ViewModelTaxConfiguration
    {
        public int Key { get; set; }
        public decimal GeneralItbisPercentage { get; set; }
        public decimal CustomsServiceRatePercentage { get; set; }
        public bool State { get; set; }
    }
}