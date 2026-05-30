using Application.DTOs.Base;
namespace Application.DTOs.Importers
{
    public class ImporterDto : DtoBase<int>
    {
        public required string Identification { get; set; }
        public string? Phone {  get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public required int CountryId { get; set; }
        public string? CountryName {  get; set; }

       
    }
}