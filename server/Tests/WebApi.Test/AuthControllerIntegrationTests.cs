using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebApi.Test
{
    public class AuthControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public AuthControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Login_ValidCredentials_Returns200()
        {
            var payload = new { Email = "admin@admin.com", Password = "123456" };

            var response = await _client.PostAsJsonAsync("/api/auth/login", payload);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Login_ValidToken_Returns200()
        {
            var payload = new { Email = "admin@admin.com", Password = "123456" };

            var response = await _client.PostAsJsonAsync("/api/auth/login", payload);
            var body = await response.Content.ReadFromJsonAsync<JsonElement>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.False(string.IsNullOrWhiteSpace(body.GetProperty("token").GetString()));
        }

        [Fact]
        public async Task Login_UnconfirmedEmail_Returns401WithMessage()
        {
            var payload = new { Email = "pending@pending.com", Password = "123456" };

            var response = await _client.PostAsJsonAsync("/api/auth/login", payload);
            var body = await response.Content.ReadFromJsonAsync<JsonElement>();
            var messages = body.GetProperty("errorMessages").EnumerateArray().Select(m => m.GetString());

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Contains("Please confirm your email before signing in.", messages);
        }

        [Theory]
        [InlineData("admin@admin.com", "wrong")]
        [InlineData("wrong@wrong.com", "123456")]
        public async Task Login_InvalidCredentials_Returns401(string email, string password)
        {
            var payload = new { Email = email, Password = password };

            var response = await _client.PostAsJsonAsync("/api/auth/login", payload);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("", "wrong")]
        [InlineData("wrong@wrong.com", "        ")]
        [InlineData("wrong#wrong", "123456")]
        public async Task Login_WrongCredentials_Returns400(string email, string password)
        {
            var payload = new { Email = email, Password = password };

            var response = await _client.PostAsJsonAsync("/api/auth/login", payload);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Register_ValidData_Returns200()
        {
            var payload = new { Name = "New User", Email = "newuser@triply.com", Password = "Str0ng!Pass" };

            var response = await _client.PostAsJsonAsync("/api/auth/register", payload);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Register_ValidData_ReturnsConfirmationMessage()
        {
            var payload = new { Name = "Confirm User", Email = "confirm@triply.com", Password = "Str0ng!Pass" };

            var response = await _client.PostAsJsonAsync("/api/auth/register", payload);
            var body = await response.Content.ReadFromJsonAsync<JsonElement>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("We've sent you a confirmation email.", body.GetProperty("message").GetString());
        }

        [Fact]
        public async Task Register_ExistingEmail_Returns200WithoutLeakingExistence()
        {
            var payload = new { Name = "Admin", Email = "admin@admin.com", Password = "Str0ng!Pass" };

            var response = await _client.PostAsJsonAsync("/api/auth/register", payload);
            var body = await response.Content.ReadFromJsonAsync<JsonElement>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("We've sent you a confirmation email.", body.GetProperty("message").GetString());
        }

        [Fact]
        public async Task Register_PersistsUserWithUnconfirmedEmail()
        {
            var register = new { Name = "Persist User", Email = "persist@triply.com", Password = "Str0ng!Pass" };
            await _client.PostAsJsonAsync("/api/auth/register", register);

            var login = new { Email = "persist@triply.com", Password = "Str0ng!Pass" };
            var response = await _client.PostAsJsonAsync("/api/auth/login", login);
            var body = await response.Content.ReadFromJsonAsync<JsonElement>();
            var messages = body.GetProperty("errorMessages").EnumerateArray().Select(m => m.GetString());

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Contains("Please confirm your email before signing in.", messages);
        }

        [Theory]
        [InlineData("", "valid@triply.com", "Str0ng!Pass")]
        [InlineData("John123", "valid@triply.com", "Str0ng!Pass")]
        [InlineData("Valid Name", "", "Str0ng!Pass")]
        [InlineData("Valid Name", "notanemail", "Str0ng!Pass")]
        [InlineData("Valid Name", "valid@triply.com", "")]
        [InlineData("Valid Name", "valid@triply.com", "weak")]
        public async Task Register_InvalidData_Returns400(string name, string email, string password)
        {
            var payload = new { Name = name, Email = email, Password = password };

            var response = await _client.PostAsJsonAsync("/api/auth/register", payload);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
