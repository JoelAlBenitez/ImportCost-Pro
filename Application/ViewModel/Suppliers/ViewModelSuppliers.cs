using Application.ViewModel.Base;

namespace Application.ViewModel.Suppliers
{
    public class ViewModelSuppliers : BaseViewModel<int, string>
    {
        public required string NameCountry { get; set; }
        public required int CountryId { get; set; }
        public string?  Email {  get; set; }
        public string ? PhoneNumber { get; set; }
        public required string MainCurrency {  get; set; }
        public required int MainCurrencyId { get; set; }
      
    }
}
