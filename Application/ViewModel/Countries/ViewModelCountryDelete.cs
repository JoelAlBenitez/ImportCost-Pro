using Application.ViewModel.Base;

namespace Application.ViewModel.Countries
{
    public class ViewModelCountryDelete
    {
        public required int Key { get; set; }
        public required string Name { get; set; } = null!;
    }
}
