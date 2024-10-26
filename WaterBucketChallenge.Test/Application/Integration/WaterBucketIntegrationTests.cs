using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace WaterBucketChallenge.Tests.IntegrationTests
{
    [TestFixture] // NUnit attribute for test class
    public class WaterBucketIntegrationTests
    {
        private HttpClient _client;

        [OneTimeSetUp]
        public void Setup()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000") // Set this to your actual base URL
            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [OneTimeTearDown] // Clean up resources after all tests
        public void DisposeHttpClient()
        {
            _client?.Dispose(); // Dispose the HttpClient if it's not null
        }

        [Test] // NUnit attribute for test method
        public async Task GetSolution_ReturnsCorrectResponse_WhenSolutionExists()
        {
            // Arrange
            var requestBody = new
            {
                XCapacity = 2,
                YCapacity = 10,
                ZTarget = 4
            };
            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/solve", content);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode, "Expected successful status code."); // Explicit status code check
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.That(responseString, Does.Contain("\"status\":\"Solved\"")); // Assert for the expected status
        }

        [Test] // NUnit attribute for test method
        public async Task GetSolution_ReturnsNoSolution_WhenImpossible()
        {
            // Arrange
            var requestBody = new
            {
                XCapacity = 2,
                YCapacity = 1,
                ZTarget = 5
            };
            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/solve", content);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode, "Expected successful status code."); // Explicit status code check
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.That(responseString, Does.Contain("\"error\":\"Target volume cannot be larger than the largest jug.\"")); // Assert for the expected no solution message
        }

        [Test] // NUnit attribute for test method
        public async Task GetSolution_ReturnsNoSolution_BadRequest()
        {
            // Arrange
            var requestBody = new
            {
                XCapacity = 2,
                YCapacity = 1,
                ZTarget = 5
            };
            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/solve", content);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode, "Expected successful status code."); // Explicit status code check
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.That(responseString, Does.Contain("\"error\":\"X, Y, and Z must be positive integers.\"")); // Assert for the expected no solution message
        }
    }
}
