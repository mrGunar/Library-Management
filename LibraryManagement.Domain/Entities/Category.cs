using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Domain.Entities
{
    public class Category
    {
        private Category() { }

        public Category(string name) 
        {
            Name = name;
            IsActive = true;
        }

        private readonly List<Category> _subCategories = new();
        private readonly List<Book> _books = new();
        public long CategoryId { get; private set; }
        public string? Name { get; private set; }
        public string? Description { get; private set; } = string.Empty;
        public long? ParentCategoryId { get; private set; }
        public Category? ParentCategory { get; private set; }
        public IReadOnlyCollection<Category> SubCategories => _subCategories.AsReadOnly();
        public int SortOrder { get; private set; } = 0;
        public bool IsActive { get; private set; } = true;
        public IReadOnlyCollection<Book> Books => _books.AsReadOnly();
    }
}
