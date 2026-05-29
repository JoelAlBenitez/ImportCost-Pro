using Application.DTOs.Base;

namespace Application.DTOs.FinancialCore
{
    public class CurrencyDto : DtoBase
    {
        public required string IsoCode { get; set; }
        public required string Symbol { get; set; }
        public bool IsLocalCurrency { get; set; }
    }
}