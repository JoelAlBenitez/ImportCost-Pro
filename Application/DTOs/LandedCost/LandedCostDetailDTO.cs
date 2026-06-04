using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.LandedCost
{
    public class LandedCostDetailDTO
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal OriginalTotalFob { get; set; }
        public decimal LocalTotalFob { get; set; }
        public decimal AssignedFreight { get; set; }
        public decimal AssignedInsurance { get; set; }
        public decimal TotalCif { get; set; }
        public decimal TotalTariff { get; set; }
        public decimal TotalSelectiveTax { get; set; }
        public decimal TotalCustomsServiceFee { get; set; }
        public decimal TotalItbis { get; set; }
        public decimal AssignedLocalExpenses { get; set; }
        public decimal TotalImportedCost { get; set; }
        public decimal UnitImportedCost { get; set; }
        public decimal DesiredMargin { get; set; }
        public decimal SuggestedSalePrice { get; set; }

    }
}
