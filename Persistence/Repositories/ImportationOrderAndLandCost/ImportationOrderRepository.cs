using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Entities.Enums;
using Persistence.Entities.FinancialCore;

namespace Persistence.Repositories.ImportationOrderAndLandCost
{
    public class ImportationOrderRepository
    {
        private readonly ContextImportCost _context;

        public ImportationOrderRepository(ContextImportCost context)
        {
            _context = context;
        }

        public async Task<bool> HasOrdersByCurrencyId(int currencyId)
        {
            return await _context.ImportationOrders.AnyAsync(o => o.CurrencyId == currencyId);
        }

        public async Task<bool> IsRateInUseAsync(ExchangeRate rate)
        {
            // Una tasa está en uso si existe alguna orden en estado Calculada o Cerrada 
            // que coincida con la moneda y cuya fecha de orden sea posterior o igual a la de la tasa
            return await _context.ImportationOrders.AnyAsync(o => 
                o.CurrencyId == rate.SourceCurrencyId && 
                (o.OrderState == OrderState.Calculada || o.OrderState == OrderState.Cerrada) &&
                o.OrderDate >= rate.EffectiveDate);
        }
    }
}