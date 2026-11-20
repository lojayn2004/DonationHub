using static RestAssured.Dsl;

namespace DontaionHub.IntegrationTests;

[Collection("Api")]
public class CampaignWorkflowTests
{
    private readonly KestrelWebApplicationFactory _factory;

    public CampaignWorkflowTests(KestrelWebApplicationFactory factory)
    {
        _factory = factory;
        _ = factory.CreateClient();
    }

    [Fact]
    public void Admin_CreatesCategoryAndCampaign_ThenGetsCampaign()
    {
        var token = LoginAsAdmin();
        var categoryId = CreateCategory(token);
        var campaignId = CreateCampaign(token, categoryId);

        var returnedCampaignId = (string)Given()
            .Header("Authorization", $"Bearer {token}")
            .When()
            .Get($"{_factory.BaseUrl}/api/v1/campaign/{campaignId}")
            .Then()
            .StatusCode(200)
            .Extract().Body("$.id");

        var returnedCategoryId = (string)Given()
            .Header("Authorization", $"Bearer {token}")
            .When()
            .Get($"{_factory.BaseUrl}/api/v1/campaign/{campaignId}")
            .Then()
            .StatusCode(200)
            .Extract().Body("$.categoryId");

        Assert.Equal(campaignId, Guid.Parse(returnedCampaignId));
        Assert.Equal(categoryId, Guid.Parse(returnedCategoryId));
    }

    [Fact]
    public void Admin_UpdatingCampaignWithStaleRowVersion_ReturnsConflict()
    {
        var token = LoginAsAdmin();
        var categoryId = CreateCategory(token);
        var campaignId = CreateCampaign(token, categoryId);

        var rowVersion = (string)Given()
            .Header("Authorization", $"Bearer {token}")
            .When()
            .Get($"{_factory.BaseUrl}/api/v1/campaign/{campaignId}")
            .Then()
            .StatusCode(200)
            .Extract().Body("$.rowVersion");

        Assert.False(string.IsNullOrEmpty(rowVersion));

        var update = new
        {
            titleAr = "عنوان حملة محدث",
            titleEn = "Updated campaign title",
            descAr = "وصف الحملة بعد التحديث",
            descEn = "Updated campaign description",
            goalAmount = 1000,
            endDate = DateTimeOffset.UtcNow.AddDays(30),
            categoryId,
            rowVersion
        };

        Given()
            .Header("Authorization", $"Bearer {token}")
            .ContentType("application/json")
            .Body(update)
            .When()
            .Put($"{_factory.BaseUrl}/api/v1/campaign/{campaignId}")
            .Then()
            .StatusCode(200);

        Given()
            .Header("Authorization", $"Bearer {token}")
            .ContentType("application/json")
            .Body(update)
            .When()
            .Put($"{_factory.BaseUrl}/api/v1/campaign/{campaignId}")
            .Then()
            .StatusCode(409);
    }

    private string LoginAsAdmin()
    {
        var token = (string)Given()
            .ContentType("application/json")
            .Body(new { email = "admin@test.com", password = "Test@12345" })
            .When()
            .Post($"{_factory.BaseUrl}/api/v1/auth/login")
            .Then()
            .StatusCode(200)
            .Extract().Body("$.token");

        Assert.False(string.IsNullOrEmpty(token));
        return token;
    }

    private Guid CreateCategory(string token)
    {
        var suffix = Guid.NewGuid().ToString("N");
        var categoryId = (string)Given()
            .Header("Authorization", $"Bearer {token}")
            .ContentType("application/json")
            .Body(new
            {
                nameAr = $"فئة {suffix}",
                nameEn = $"Category {suffix}",
                descriptionAr = "وصف فئة الاختبار",
                descriptionEn = "Integration test category"
            })
            .When()
            .Post($"{_factory.BaseUrl}/api/v1/categories")
            .Then()
            .StatusCode(201)
            .Extract().Body("$.id");

        return Guid.Parse(categoryId);
    }

    private Guid CreateCampaign(string token, Guid categoryId)
    {
        var campaignId = (string)Given()
            .Header("Authorization", $"Bearer {token}")
            .ContentType("application/json")
            .Body(new
            {
                titleAr = "حملة اختبار تكامل",
                titleEn = $"Integration campaign {Guid.NewGuid():N}",
                descriptionAr = "وصف حملة اختبار التكامل",
                descriptionEn = "Integration test campaign description",
                goalCurrency = "USD",
                goalAmount = 1000,
                endDate = DateTimeOffset.UtcNow.AddDays(30),
                categoryId
            })
            .When()
            .Post($"{_factory.BaseUrl}/api/v1/campaign")
            .Then()
            .StatusCode(201)
            .Extract().Body("$.id");

        return Guid.Parse(campaignId);
    }
}
