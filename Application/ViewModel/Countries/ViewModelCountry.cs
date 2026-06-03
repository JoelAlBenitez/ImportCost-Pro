using Application.ViewModel.Base;

namespace Application.ViewModel.Countries
{
    public class ViewModelCountry : BaseViewModel<int, string>
    {
        public required string IsoCode { get; set; }
    }
}
