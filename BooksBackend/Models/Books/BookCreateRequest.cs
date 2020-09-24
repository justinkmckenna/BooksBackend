using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BooksBackend.Models.Books
{
    public class BookCreateRequest : IValidatableObject
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [Range(1,5000)]
        public int? NumberOfPages { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Title.ToLower() == "it" && Author.ToLower() == "king")
            {
                yield return new ValidationResult("Book not allowed.", new string[] { "Title", "Author" });
            }
        }
    }
}
