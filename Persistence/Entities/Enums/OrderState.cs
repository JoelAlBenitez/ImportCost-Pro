using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Entities.Enums
{     
        public enum OrderState
        {
            Borrador,
            Emitida,
            EnTransito,
            EnAduana,
            Recibida,
            Cancelada
        }
    
}
