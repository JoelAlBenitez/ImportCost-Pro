using Persistence.Entities.Base;
using Persistence.Entities.FinancialCore;

namespace Persistence.Entities.OperationalCommercial
{
    public class Importers : BaseEntity<int, string>
    {
        public required string Identification { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public int countryId { get; set; }
        public Country? country { get; set; }
    }
}
