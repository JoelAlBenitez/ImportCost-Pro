using Application.DTOs.Base;

namespace Application.DTOs.Suppliers
{
    public class SuppliersDto : DtoBase<int>
    {
        public string? NameContry { get; set; }
        public required int CountryId {  get; set; }
        public string? Email {  get; set; }
        public string? PhoneNumber { get; set; }
        public required int CurrencyId { get; set; }
        public string? CurrencyName { get; set; }
    }
}