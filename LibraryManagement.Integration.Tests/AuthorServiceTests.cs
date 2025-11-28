using LibraryManagement.Contract.Authors;

namespace LibraryManagement.Integration.Tests;

public class AuthorServiceTests : BaseIntegrationTest
{
    private readonly AuthorService.AuthorServiceClient _client;

    public AuthorServiceTests(TestWebApplicationFactory factory) : base(factory)
    {
        _client = new AuthorService.AuthorServiceClient(Channel);
    }

    [Fact]
    public async Task GetAuthors_ReturnsListOfAuthors()
    {
        var request = new AuthorSearchRequest
        {
            SearchTerm = "",
            IsActive = true,
            PageNumber = 1,
            PageSize = 10
        };

        var response = await _client.GetAuthorsAsync(request);

        Assert.NotNull(response);
        Assert.True(response.TotalCount > 0);
        Assert.NotEmpty(response.Authors);
    }

    [Fact]
    public async Task GetAuthors_WithSearchTerm_ReturnsFilteredAuthors()
    {
        var request = new AuthorSearchRequest
        {
            SearchTerm = "John",
            IsActive = true,
            PageNumber = 1,
            PageSize = 10
        };

        var response = await _client.GetAuthorsAsync(request);

        Assert.NotNull(response);
        Assert.All(response.Authors, author =>
            Assert.Contains("John", author.FirstName + " " + author.LastName, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GetAuthor_WithValidId_ReturnsAuthor()
    {
        var request = new AuthorGetRequest { AuthorId = 1 };

        var response = await _client.GetAuthorAsync(request);

        Assert.NotNull(response);
        Assert.NotNull(response.Author);
        Assert.Equal(1, response.Author.AuthorId);
    }

    [Fact]
    public async Task CreateAuthor_WithValidData_CreatesAuthor()
    {
        var request = new CreateAuthorRequest
        {
            FirstName = "New",
            LastName = "Author",
            Biography = "A new author biography",
            DateOfBirth = "1990-01-01"
        };

        var response = await _client.CreateAuthorAsync(request);

        Assert.NotNull(response);
        Assert.True(response.AuthorId > 0);
        Assert.Equal("New", response.FirstName);
        Assert.Equal("Author", response.LastName);
    }

    [Fact]
    public async Task UpdateAuthor_WithValidData_UpdatesAuthor()
    {
        var createRequest = new CreateAuthorRequest
        {
            FirstName = "Test",
            LastName = "Update",
            Biography = "Test biography",
            DateOfBirth = "1992-05-10"
        };

        var createResponse = await _client.CreateAuthorAsync(createRequest);

        var updateRequest = new UpdateAuthorRequest
        {
            AuthorId = createResponse.AuthorId,
            FirstName = "Updated",
            LastName = "Author",
            Biography = "Updated biography",
            DateOfBirth = "1992-05-10",
            IsActive = true
        };

        var updateResponse = await _client.UpdateAuthorAsync(updateRequest);

        Assert.NotNull(updateResponse);
        Assert.Equal("Updated", updateResponse.FirstName);
        Assert.Equal("Author", updateResponse.LastName);
        Assert.Equal("Updated biography", updateResponse.Biography);
    }

    [Fact]
    public async Task GetAuthors_WithPagination_ReturnsCorrectPage()
    {
        var request = new AuthorSearchRequest
        {
            SearchTerm = "",
            IsActive = true,
            PageNumber = 1,
            PageSize = 1
        };

        var response = await _client.GetAuthorsAsync(request);

        Assert.NotNull(response);
        Assert.Equal(1, response.PageNumber);
        Assert.Equal(1, response.PageSize);
        Assert.Single(response.Authors);
    }
}

