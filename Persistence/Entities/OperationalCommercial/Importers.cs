using Persistence.Entities.Base;

namespace Persistence.Entities.OperationalCommercial
{
    public class Importers : BaseEntity<int, string>
    {
        public required string Identification {  get; set; }
        public string? Phone {  get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }

        //public required Counstrys countryId {get; set;}
        //public Countrys? country {get; set;} 
    }
}
