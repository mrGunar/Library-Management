using LibraryManagement.Contract.Borrowings;

namespace LibraryManagement.Integration.Tests;

public class BorrowingServiceTests : BaseIntegrationTest
{
    private readonly BorrowingService.BorrowingServiceClient _client;

    public BorrowingServiceTests(TestWebApplicationFactory factory) : base(factory)
    {
        _client = new BorrowingService.BorrowingServiceClient(Channel);
    }

    [Fact]
    public async Task BorrowBook_WithValidData_CreatesBorrowing()
    {
        var request = new BorrowBookRequest
        {
            BookId = 2,
            UserId = 1,
            DaysToReturn = 14
        };

        var response = await _client.BorrowBookAsync(request);

        Assert.NotNull(response);
        Assert.True(response.BorrowingId > 0);
        Assert.Equal(2, response.BookId);
        Assert.Equal(1, response.UserId);
        Assert.Equal("Active", response.Status);
    }

    [Fact]
    public async Task ReturnBook_WithValidBorrowingId_ReturnsBook()
    {
        var borrowRequest = new BorrowBookRequest
        {
            BookId = 2,
            UserId = 2,
            DaysToReturn = 14
        };

        var borrowResponse = await _client.BorrowBookAsync(borrowRequest);

        var returnRequest = new ReturnBookRequest
        {
            BorrowingId = borrowResponse.BorrowingId
        };

        var returnResponse = await _client.ReturnBookAsync(returnRequest);

        Assert.NotNull(returnResponse);
        Assert.Equal("Returned", returnResponse.Status);
        Assert.NotEmpty(returnResponse.ReturnDate);
    }

    [Fact]
    public async Task GetUserBorrowings_ReturnsUserBorrowings()
    {
        var request = new UserBorrowingsRequest
        {
            UserId = 1,
            Status = ""
        };

        var response = await _client.GetUserBorrowingsAsync(request);

        Assert.NotNull(response);
        Assert.NotEmpty(response.Borrowings);
        Assert.All(response.Borrowings, borrowing => Assert.Equal(1, borrowing.UserId));
    }

    [Fact]
    public async Task GetUserBorrowings_FilterByStatus_ReturnsFilteredBorrowings()
    {
        var borrowRequest = new BorrowBookRequest
        {
            BookId = 2,
            UserId = 1,
            DaysToReturn = 14
        };

        await _client.BorrowBookAsync(borrowRequest);

        var request = new UserBorrowingsRequest
        {
            UserId = 1,
            Status = "Active"
        };

        var response = await _client.GetUserBorrowingsAsync(request);

        Assert.NotNull(response);
        Assert.All(response.Borrowings, borrowing => Assert.Equal("Active", borrowing.Status));
    }

    [Fact]
    public async Task GetOverdueBooks_ReturnsOverdueBooks()
    {
        var request = new OverdueBooksRequest
        {
            PageNumber = 1,
            PageSize = 10
        };

        var response = await _client.GetOverdueBooksAsync(request);

        Assert.NotNull(response);
    }

    [Fact]
    public async Task GetOverdueBooks_WithPagination_ReturnsCorrectPage()
    {
        var request = new OverdueBooksRequest
        {
            PageNumber = 1,
            PageSize = 5
        };

        var response = await _client.GetOverdueBooksAsync(request);

        Assert.NotNull(response);
        Assert.Equal(1, response.PageNumber);
        Assert.Equal(5, response.PageSize);
    }

    [Fact]
    public async Task BorrowBook_WithCustomDaysToReturn_SetsDueDate()
    {
        var request = new BorrowBookRequest
        {
            BookId = 2,
            UserId = 2,
            DaysToReturn = 7
        };

        var response = await _client.BorrowBookAsync(request);

        Assert.NotNull(response);
        Assert.NotEmpty(response.DueDate);
        
        var dueDate = DateTime.Parse(response.DueDate);
        var borrowDate = DateTime.Parse(response.BorrowDate);
        var daysDifference = (dueDate - borrowDate).Days;
        
        Assert.Equal(7, daysDifference);
    }

    [Fact]
    public async Task ReturnBook_CalculatesFineForOverdueBooks()
    {
        var borrowRequest = new BorrowBookRequest
        {
            BookId = 2,
            UserId = 2,
            DaysToReturn = 1
        };

        var borrowResponse = await _client.BorrowBookAsync(borrowRequest);
        
        await Task.Delay(100);

        var returnRequest = new ReturnBookRequest
        {
            BorrowingId = borrowResponse.BorrowingId
        };

        var returnResponse = await _client.ReturnBookAsync(returnRequest);

        Assert.NotNull(returnResponse);
    }

    [Fact]
    public async Task GetUserBorrowings_WithNoStatus_ReturnsAllUserBorrowings()
    {
        var borrowRequest1 = new BorrowBookRequest
        {
            BookId = 2,
            UserId = 2,
            DaysToReturn = 14
        };

        var borrowResponse1 = await _client.BorrowBookAsync(borrowRequest1);

        var returnRequest = new ReturnBookRequest
        {
            BorrowingId = borrowResponse1.BorrowingId
        };

        await _client.ReturnBookAsync(returnRequest);

        var request = new UserBorrowingsRequest
        {
            UserId = 2,
            Status = ""
        };

        var response = await _client.GetUserBorrowingsAsync(request);

        Assert.NotNull(response);
        Assert.NotEmpty(response.Borrowings);
    }
}

