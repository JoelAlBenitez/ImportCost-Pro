using Application.DTOs.Base;

namespace Application.DTOs.FinancialCore
{
    public class CountryDto : DtoBase
    {
        public required string IsoCode { get; set; }
    }
}