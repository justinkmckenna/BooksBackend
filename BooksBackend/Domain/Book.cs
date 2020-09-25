using System;

namespace BooksBackend.Domain
{
    public class Book
    {
        public int id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int NumberOfPages { get; set; }
        public DateTime DateAdded { get; set; }
        public Boolean IsInInventory { get; set; }
        public Boolean MarkedForSale { get; set; }
    }
}
