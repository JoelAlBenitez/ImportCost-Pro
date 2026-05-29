using Application.DTOs.Base;

namespace Application.DTOs.Currencies
{
    public class CurrencyDto : DtoBase<int>
    {
        public required string IsoCode { get; set; }
        public required string Symbol { get; set; }
        public bool IsLocalCurrency { get; set; }
    }
}