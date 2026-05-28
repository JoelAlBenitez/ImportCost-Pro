using Application.ViewModel.Base;

namespace Application.ViewModel.Importers
{
    public  class ViewModelImporter : BaseViewModel<int, string>
    {
        public required string Identification { get; set; }
        //public required Countrys country  {get; set;}
        public string? Phone { get; set; } = "—";
        public string? Email { get; set; } = "N/A";
      
    }
}
