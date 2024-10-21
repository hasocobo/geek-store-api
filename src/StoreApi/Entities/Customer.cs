using System.ComponentModel.DataAnnotations;

namespace StoreApi.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Customer name is a required field.")]
        [MaxLength(50, ErrorMessage = "Maximum length for the name is 50 characters")]
        public string? Name { get; set; }
       
        [Required(ErrorMessage = "Customer email is a required field.")]
        public string? Email { get; set; }
        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
