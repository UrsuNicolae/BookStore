using System.ComponentModel.DataAnnotations;

namespace BookStore.Data.DataModels
{
    public class CardModel
    {
        [Required(ErrorMessage = "required")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Needed 16 digits")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "invalid number")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "required")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Needed 5 digits")]
        public string Date { get; set; }

        [Required(ErrorMessage = "required")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Needed 3 digits")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "invalid number")]
        public string CVC { get; set; }
    }
}
