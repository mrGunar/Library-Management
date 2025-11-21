using LibraryManagement.Contract.Books;

namespace LibraryManagement.Integration.Tests;

public class BookServiceTests : BaseIntegrationTest
{
    private readonly BookService.BookServiceClient _client;

    public BookServiceTests(TestWebApplicationFactory factory) : base(factory)
    {
        _client = new BookService.BookServiceClient(Channel);
    }

    [Fact]
    public async Task GetBooks_ReturnsListOfBooks()
    {
        var request = new BookSearchRequest
        {
            SearchTerm = "",
            PageNumber = 1,
            PageSize = 10
        };

        var response = await _client.GetBooksAsync(request);

        Assert.NotNull(response);
        Assert.True(response.TotalCount > 0);
        Assert.NotEmpty(response.Books);
    }

    [Fact]
    public async Task GetBooks_WithSearchTerm_ReturnsFilteredBooks()
    {
        var request = new BookSearchRequest
        {
            SearchTerm = "Test Book 1",
            PageNumber = 1,
            PageSize = 10
        };

        var response = await _client.GetBooksAsync(request);

        Assert.NotNull(response);
        Assert.All(response.Books, book => 
            Assert.Contains("Test Book 1", book.Title, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GetBooks_FilterByAuthor_ReturnsCorrectBooks()
    {
        var request = new BookSearchRequest
        {
            SearchTerm = "",
            AuthorId = 1,
            PageNumber = 1,
            PageSize = 10
        };

        var response = await _client.GetBooksAsync(request);

        Assert.NotNull(response);
        Assert.All(response.Books, book => Assert.Equal(1, book.AuthorId));
    }

    [Fact]
    public async Task GetBooks_FilterByCategory_ReturnsCorrectBooks()
    {
        var request = new BookSearchRequest
        {
            SearchTerm = "",
            CategoryId = 1,
            PageNumber = 1,
            PageSize = 10
        };

        var response = await _client.GetBooksAsync(request);

        Assert.NotNull(response);
        Assert.All(response.Books, book => Assert.Equal(1, book.CategoryId));
    }

    [Fact]
    public async Task GetBook_WithValidId_ReturnsBook()
    {
        var request = new BookGetRequest { BookId = 1 };

        var response = await _client.GetBookAsync(request);

        Assert.NotNull(response);
        Assert.NotNull(response.Book);
        Assert.Equal(1, response.Book.BookId);
    }

    [Fact]
    public async Task CreateBook_WithValidData_CreatesBook()
    {
        var request = new CreateBookRequest
        {
            Title = "New Test Book",
            Isbn = "ISBN-999",
            Description = "A new test book",
            AuthorId = 1,
            CategoryId = 1,
            PublishedDate = "2023-01-01",
            PageCount = 250
        };

        var response = await _client.CreateBookAsync(request);

        Assert.NotNull(response);
        Assert.True(response.BookId > 0);
        Assert.Equal("New Test Book", response.Title);
        Assert.Equal("ISBN-999", response.Isbn);
    }

    [Fact]
    public async Task UpdateBook_WithValidData_UpdatesBook()
    {
        var createRequest = new CreateBookRequest
        {
            Title = "Book to Update",
            Isbn = "ISBN-888",
            Description = "Original description",
            AuthorId = 1,
            CategoryId = 1,
            PublishedDate = "2022-01-01",
            PageCount = 200
        };

        var createResponse = await _client.CreateBookAsync(createRequest);

        var updateRequest = new UpdateBookRequest
        {
            BookId = createResponse.BookId,
            Title = "Updated Book Title",
            Description = "Updated description",
            CategoryId = 2,
            PublishedDate = "2022-01-01",
            PageCount = 250
        };

        var updateResponse = await _client.UpdateBookAsync(updateRequest);

        Assert.NotNull(updateResponse);
        Assert.Equal("Updated Book Title", updateResponse.Title);
        Assert.Equal("Updated description", updateResponse.Description);
        Assert.Equal(2, updateResponse.CategoryId);
        Assert.Equal(250, updateResponse.PageCount);
    }

    [Fact]
    public async Task DeleteBook_WithValidId_DeletesBook()
    {
        var createRequest = new CreateBookRequest
        {
            Title = "Book to Delete",
            Isbn = "ISBN-777",
            Description = "Will be deleted",
            AuthorId = 1,
            CategoryId = 1,
            PublishedDate = "2023-01-01",
            PageCount = 150
        };

        var createResponse = await _client.CreateBookAsync(createRequest);

        var deleteRequest = new BookDeleteRequest { BookId = createResponse.BookId };
        var deleteResponse = await _client.DeleteBookAsync(deleteRequest);

        Assert.NotNull(deleteResponse);
        Assert.True(deleteResponse.Success);
    }

    [Fact]
    public async Task GetBooks_WithPagination_ReturnsCorrectPage()
    {
        var request = new BookSearchRequest
        {
            SearchTerm = "",
            PageNumber = 1,
            PageSize = 1
        };

        var response = await _client.GetBooksAsync(request);

        Assert.NotNull(response);
        Assert.Equal(1, response.PageNumber);
        Assert.Equal(1, response.PageSize);
        Assert.Single(response.Books);
    }

    [Fact]
    public async Task GetBooks_FilterByAvailability_ReturnsAvailableBooks()
    {
        var request = new BookSearchRequest
        {
            SearchTerm = "",
            IsAvailable = true,
            PageNumber = 1,
            PageSize = 10
        };

        var response = await _client.GetBooksAsync(request);

        Assert.NotNull(response);
        Assert.All(response.Books, book => Assert.True(book.IsAvailable));
    }
}

