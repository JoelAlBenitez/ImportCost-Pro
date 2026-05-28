using Application.Dto.Base;
namespace Application.Dto.Importers
{
    public class ImporterDto : DtoBase<int>
    {
        public required string Identification { get; set; }
        public string? Phone {  get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }

        public int CountryId { get; set; }

        //public Countrys country {get; set;}
    }
}
