namespace Application.DTOs.TaxConfigurations
{
    public class TaxConfigurationDto
    {
        public int Key { get; set; }
        public decimal GeneralItbisPercentage { get; set; }
        public decimal CustomsServiceRatePercentage { get; set; }
    }
}