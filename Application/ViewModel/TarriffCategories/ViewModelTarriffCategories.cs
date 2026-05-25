using Application.ViewModel.Base;

namespace Application.ViewModel.TarriffCategories
{
    public class ViewModelTarriffCategories : BaseViewModel<string, string>
    {
        public required decimal PorcentageTariff { get; set; }
        public required bool ITBIS { get; set; }
        public required bool SelectiveTaxApplies { get; set; }
        public decimal? PorcentageTaxSelective { get; set; } 
    }
}
