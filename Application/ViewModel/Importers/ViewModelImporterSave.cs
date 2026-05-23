using System.ComponentModel.DataAnnotations;

namespace Application.ViewModel.Importers
{
    public class ViewModelImporterSave
    {

        [Required(ErrorMessage = "Invalid or too long name, please enter a valid name of no more than 150 characters.")]
        [StringLength(150, MinimumLength = 3)]
        public required string Name { get; set; }

        [StringLength(20, MinimumLength = 3)]
        [Required(ErrorMessage = "Invalid identification or too long, please enter a valid identification no longer than 20 characters")]
        public required string Identifcation { get; set; }

        //[Required(ErrorMessage = "Country invalid, please select a country  valid")]
        //public required int countryId {get; set;}

        [StringLength(20, MinimumLength = 0)]
        [Phone]
        public string? PhoneNumber {  get; set; }

        [StringLength(100, MinimumLength = 0)]
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(250, MinimumLength = 0)]
        public string? Address { get; set; }

        [Required (ErrorMessage = "You must select a valid state for the importer you are trying to create.")]
        public required bool State { get; set; }
    }
}
