using Persistence.Entities.Base;
using Persistence.Entities.OperationalCommercial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Entities.FinancialCore
{
    public class Currency : BaseEntity<int, string> 
    {

        public required string IsoCode { get; set; }
        public required string Symbol { get; set; }

        public required bool IsLocalCurrency { get; set; }



        public virtual ICollection<Suppliers>? Suppliers { get; set; }
        // La dema las agregare cuando creemos esas clasesxd
    }
}
