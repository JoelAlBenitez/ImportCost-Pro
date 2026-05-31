using Application.DTOs.Base;

namespace Application.DTOs.TaxConfigurations
{
    public class TaxConfigurationDto : DtoBase<int>
    {
        public decimal GeneralItbisPercentage { get; set; }
        public decimal CustomsServiceRatePercentage { get; set; }
    }
}
