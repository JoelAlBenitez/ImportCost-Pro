using Application.Dto.Base;

namespace Application.Dto.Importers
{
    public class ImporterDto : DtoBase
    {
    
        public required string Identification { get; set; }
        public required bool State {  get; set; }
        public string? Phone {  get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }

        //public required Countrys country {get; set;}
    }
}
