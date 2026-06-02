namespace Application.ViewModel.Currencies
{
    public class ViewModelCurrency
    {
        public int Key { get; set; }
        public string Name { get; set; } = null!;
        public string IsoCode { get; set; } = null!;
        public string Symbol { get; set; } = null!;
        public bool IsLocalCurrency { get; set; }
        public bool State { get; set; }
    }
}