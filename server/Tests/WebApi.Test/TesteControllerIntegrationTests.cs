using Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace WebApi.Test
{
    public class TesteControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;

        public TesteControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Publico_WithoutToken_Returns200()
        {
            var response = await _client.GetAsync("/api/teste/publico");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();

            Assert.Equal("Qualquer um acessa.", body);
        }

        [Fact]
        public async Task Seguro_WithoutToken_Returns401()
        {
            var response = await _client.GetAsync("/api/teste/seguro");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Seguro_WithValidToken_Returns200()
        {
            var token = await GetTokenAsync();

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/teste/seguro");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("Olá", body);
        }

        [Fact]
        public async Task Seguro_WithExpiredToken_Returns401()
        {
            var token = BuildToken(Roles.ADMIN, expires: DateTime.UtcNow.AddMinutes(-5));

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/teste/seguro");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Seguro_WithInvalidSignature_Returns401()
        {
            var token = BuildToken(Roles.ADMIN, expires: DateTime.UtcNow.AddMinutes(15), signingKey: "another-wrong-key-0000000000000000");

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/teste/seguro");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Admin_WithoutToken_Returns401()
        {
            var response = await _client.GetAsync("/api/teste/admin");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Admin_WithValidAdminToken_Returns200()
        {
            var token = await GetTokenAsync();

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/teste/admin");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();

            Assert.Equal("Área restrita a Admin.", body);
        }

        [Fact]
        public async Task Admin_WithUserRoleToken_Returns403()
        {
            var token = BuildToken(Roles.USER, expires: DateTime.UtcNow.AddMinutes(15));

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/teste/admin");

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        private async Task<string> GetTokenAsync()
        {
            var payload = new { Email = "admin@admin.com", Password = "123456" };
            var response = await _client.PostAsJsonAsync("/api/auth/login", payload);
            var body = await response.Content.ReadFromJsonAsync<JsonElement>();
            return body.GetProperty("token").GetString()!;
        }

        private string BuildToken(string role, DateTime expires, string? signingKey = null)
        {
            var jwt = _factory.Services.GetRequiredService<IConfiguration>().GetSection("Jwt");
            var key = signingKey ?? jwt["Key"]!;

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = jwt["Issuer"],
                Audience = jwt["Audience"],
                Subject = new ClaimsIdentity(
                [
                    new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, role)
                ]),
                Expires = expires,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256)
            };

            return new JsonWebTokenHandler().CreateToken(descriptor);
        }
    }
}
