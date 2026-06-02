namespace Application.ViewModel.Countries
{
    public class ViewModelCountry
    {
        public int Key { get; set; }
        public string Name { get; set; } = null!;
        public string IsoCode { get; set; } = null!;
        public bool State { get; set; }
    }
}
