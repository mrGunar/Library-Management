using LibraryManagement.Contract.Categories;

namespace LibraryManagement.Integration.Tests;

public class CategoryServiceTests : BaseIntegrationTest
{
    private readonly CategoryService.CategoryServiceClient _client;

    public CategoryServiceTests(TestWebApplicationFactory factory) : base(factory)
    {
        _client = new CategoryService.CategoryServiceClient(Channel);
    }

    [Fact]
    public async Task GetCategories_ReturnsListOfCategories()
    {
        var request = new CategorySearchRequest
        {
            SearchTerm = "",
            IsActive = true
        };

        var response = await _client.GetCategoriesAsync(request);

        Assert.NotNull(response);
        Assert.NotEmpty(response.Categories);
    }

    [Fact]
    public async Task GetCategories_WithSearchTerm_ReturnsFilteredCategories()
    {
        var request = new CategorySearchRequest
        {
            SearchTerm = "Fiction",
            IsActive = true
        };

        var response = await _client.GetCategoriesAsync(request);

        Assert.NotNull(response);
        Assert.All(response.Categories, category =>
            Assert.Contains("Fiction", category.Name, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task CreateCategory_WithValidData_CreatesCategory()
    {
        var request = new CreateCategoryRequest
        {
            Name = "New Category",
            Description = "A new test category",
            ParentCategoryId = 0,
            SortOrder = 10
        };

        var response = await _client.CreateCategoryAsync(request);

        Assert.NotNull(response);
        Assert.True(response.CategoryId > 0);
        Assert.Equal("New Category", response.Name);
        Assert.Equal("A new test category", response.Description);
    }

    [Fact]
    public async Task CreateCategory_WithParentCategory_CreatesSubCategory()
    {
        var request = new CreateCategoryRequest
        {
            Name = "Sub Category",
            Description = "A sub category",
            ParentCategoryId = 1,
            SortOrder = 1
        };

        var response = await _client.CreateCategoryAsync(request);

        Assert.NotNull(response);
        Assert.True(response.CategoryId > 0);
        Assert.Equal("Sub Category", response.Name);
        Assert.Equal(1, response.ParentCategoryId);
    }

    [Fact]
    public async Task GetCategoryTree_ReturnsHierarchicalStructure()
    {
        var createParentRequest = new CreateCategoryRequest
        {
            Name = "Parent Category",
            Description = "Parent",
            ParentCategoryId = 0,
            SortOrder = 1
        };

        var parentResponse = await _client.CreateCategoryAsync(createParentRequest);

        var createChildRequest = new CreateCategoryRequest
        {
            Name = "Child Category",
            Description = "Child",
            ParentCategoryId = parentResponse.CategoryId,
            SortOrder = 1
        };

        await _client.CreateCategoryAsync(createChildRequest);

        var treeRequest = new CategoryTreeRequest
        {
            IncludeInactive = true
        };

        var treeResponse = await _client.GetCategoryTreeAsync(treeRequest);

        Assert.NotNull(treeResponse);
        Assert.NotEmpty(treeResponse.Categories);
    }

    [Fact]
    public async Task GetCategories_FilterByParentCategory_ReturnsSubCategories()
    {
        var createParentRequest = new CreateCategoryRequest
        {
            Name = "Parent for Filter",
            Description = "Parent",
            ParentCategoryId = 0,
            SortOrder = 1
        };

        var parentResponse = await _client.CreateCategoryAsync(createParentRequest);

        var createChildRequest = new CreateCategoryRequest
        {
            Name = "Child for Filter",
            Description = "Child",
            ParentCategoryId = parentResponse.CategoryId,
            SortOrder = 1
        };

        await _client.CreateCategoryAsync(createChildRequest);

        var searchRequest = new CategorySearchRequest
        {
            SearchTerm = "",
            ParentCategoryId = parentResponse.CategoryId,
            IsActive = true
        };

        var response = await _client.GetCategoriesAsync(searchRequest);

        Assert.NotNull(response);
        Assert.All(response.Categories, category =>
            Assert.Equal(parentResponse.CategoryId, category.ParentCategoryId));
    }

    [Fact]
    public async Task GetCategoryTree_WithIncludeInactive_ReturnsAllCategories()
    {
        var request = new CategoryTreeRequest
        {
            IncludeInactive = true
        };

        var response = await _client.GetCategoryTreeAsync(request);

        Assert.NotNull(response);
        Assert.NotEmpty(response.Categories);
    }

    [Fact]
    public async Task GetCategoryTree_WithoutIncludeInactive_ReturnsOnlyActiveCategories()
    {
        var request = new CategoryTreeRequest
        {
            IncludeInactive = false
        };

        var response = await _client.GetCategoryTreeAsync(request);

        Assert.NotNull(response);
        Assert.All(response.Categories, category => Assert.True(category.IsActive));
    }
}

