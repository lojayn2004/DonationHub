using DontaionHub.IntegrationTests;
using static RestAssured.Dsl;


[Collection("Api")]
public class CategoriesTests
{
    private readonly KestrelWebApplicationFactory _factory;

    public CategoriesTests(KestrelWebApplicationFactory factory)
    {
        _factory = factory;
        _ = factory.CreateClient();   
    }

    [Fact]
    public void GetCategories()
    {
        Given()
            .When()
            .Get($"{_factory.BaseUrl}/api/v1/categories")
            .Then()
            .StatusCode(200);
    }
}