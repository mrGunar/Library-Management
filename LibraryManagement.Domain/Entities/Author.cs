namespace LibraryManagement.Domain.Entities
{
    public class Author
    {
        private readonly List<Book> _books = new(); // ???
        public long AuthorId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = string.Empty;
        public string? Biography { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool IsActive { get; set; } = true;
        public IReadOnlyCollection<Book> Books => _books.AsReadOnly();

        private Author() { }

        public Author(string firstName,
            string lastName, 
            string? biography = default,
            DateTime? dateOfBirth = default)
        {
            FirstName = firstName;
            LastName = lastName;
            Biography = biography;
            DateOfBirth = dateOfBirth;
            IsActive = true;
        }
    }
}
