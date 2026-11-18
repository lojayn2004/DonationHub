using static RestAssured.Dsl;

namespace DontaionHub.IntegrationTests
{

    [Collection("Api")]
    public class RegisterDonorTests
{ 
    private readonly KestrelWebApplicationFactory _factory;

    public RegisterDonorTests(KestrelWebApplicationFactory factory)
    {
        _factory = factory;
        _ = factory.CreateClient();   
    }

    
        [Fact]
        public void RegisterUser_WithValidData_ReturnsSuccess()
        {
            var registerDonorPayload = new
            {
                 email = "lojaynk@gmail.com",
                 password =  "Strong@Password!123",
                 fullName =  "Lojayn Khaled",
                 userName = "LojaynKhaled",
                 preferredLanguage =  "en"

            };
            Given()
                .ContentType("application/json")
                .Body(registerDonorPayload)
           .When()
                .Post($"{_factory.BaseUrl}/api/v1/auth/register")
           .Then()
                .StatusCode(200);


        }
    }
}
