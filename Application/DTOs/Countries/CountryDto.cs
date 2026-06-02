using Application.DTOs.Base;

namespace Application.DTOs.Countries
{
    public class CountryDto : DtoBase<int>
    {
        public required string IsoCode { get; set; }
    }
}