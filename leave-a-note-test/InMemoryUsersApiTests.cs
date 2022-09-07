using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using leave_a_note_core.Models.Authentication.Requests;
using leave_a_note_core.Models.Authentication.Responses;
using leave_a_note_core.Models.DTOs;
using Newtonsoft.Json;

namespace leave_a_note_test;

internal class InMemoryUsersApiTests
{
    private readonly HttpClient _client;
    private string _requestUri = string.Empty;

    public InMemoryUsersApiTests()
    {
        var factory = new TestingWebAppFactory<Program>();
        _client = factory.CreateClient();
        _client.BaseAddress = new Uri("https://localhost:44321");
        SetAuthorizationHeaderAsync().Wait();
    }

    private async Task SetAuthorizationHeaderAsync()
    {
        var content = new LoginRequest
        {
            Username = "MainAdmin",
            Password = "asdf1234"
        };

        var tokenResponse = await _client.PostAsJsonAsync("/api/auth/login", content);
        var tokenResponseString = await tokenResponse.Content.ReadAsStringAsync();
        var authenticatedUserResponse = JsonConvert.DeserializeObject<AuthenticatedUserResponse>(tokenResponseString);
        var accessToken = authenticatedUserResponse.AccessToken;

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    }

    [SetUp]
    public void Setup()
    {
        _requestUri = "/api/users";
    }

    [Test]
    public async Task TestGetAllUsers()
    {
        // Arrange
        // Act
        var response = await _client.GetAsync(_requestUri);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var actual = JsonConvert.DeserializeObject<List<UserViewDto>>(responseString);

        // Assert
        Assert.That(actual, Is.Not.Empty);
    }

    [Test]
    public async Task TestGetAllUsers_InvalidUrl()
    {
        // Arrange
        const string invalidUrlEnding = "wrongEnding";
        _requestUri += invalidUrlEnding;

        // Act
        var response = await _client.GetAsync(_requestUri);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task TestGetUser_ValidId()
    {
        // Arrange
        const int userId = 1;
        _requestUri += $"/{userId}";

        // Act
        var response = await _client.GetAsync(_requestUri);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var actual = JsonConvert.DeserializeObject<UserViewDto>(responseString);

        // Assert
        Assert.That(actual.Id, Is.EqualTo(userId));
    }

    [Test]
    public async Task TestGetUser_InvalidId()
    {
        // Arrange
        const int invalidUserId = 10;
        _requestUri += $"/{invalidUserId}";

        // Act
        var response = await _client.GetAsync(_requestUri);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task TestAddUser_ValidModel()
    {
        // Arrange
        const string expectedUserName = "tester";

        var content = new UserCreateDto
        {
            UserName = expectedUserName,
            FirstName = "Joe",
            LastName = "Tester",
            Password = "11111111",
            Role = "User"
        };

        // Act
        var response = await _client.PostAsJsonAsync(_requestUri, content);
        var responseString = await response.Content.ReadAsStringAsync();
        var actual = JsonConvert.DeserializeObject<UserViewDto>(responseString);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(actual.UserName, Is.EqualTo(expectedUserName));
        });
    }

    [Test]
    public async Task TestAddUser_InvalidModel()
    {
        // Arrange
        var invalidContent = new UserCreateDto();

        // Act
        var response = await _client.PostAsJsonAsync(_requestUri, invalidContent);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    [TestCase(1, "username1")]
    [TestCase(1, "username2")]
    public async Task TestUpdateUser_ValidModel_ValidNoteId(int userId, string updateUserName)
    {
        // Arrange
        _requestUri += $"/{userId}";
        var updateContent = new UserUpdateDto
        {
            UserName = updateUserName,
            FirstName = "Name",
            LastName = "Changer"
        };

        // Act
        var response = await _client.PutAsJsonAsync(_requestUri, updateContent);
        var responseString = await response.Content.ReadAsStringAsync();
        var actual = JsonConvert.DeserializeObject<UserViewDto>(responseString);

        // Assert
        Assert.That(actual.UserName, Is.EqualTo(updateUserName));
    }

    [Test]
    public async Task TestUpdateUser_InvalidModel()
    {
        // Arrange
        const int userId = 1;
        _requestUri += $"/{userId}";
        var invalidContent = new NoteUpdateDto();

        // Act
        var response = await _client.PutAsJsonAsync(_requestUri, invalidContent);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task TestUpdateUser_ValidModel_InvalidNoteId()
    {
        // Arrange
        const int invalidUserId = 10;
        _requestUri += $"/{invalidUserId}";
        const string updateUserName = "username1";
        var updateContent = new UserUpdateDto
        {
            UserName = updateUserName,
            FirstName = "Name",
            LastName = "Changer"
        };

        // Act
        var response = await _client.PutAsJsonAsync(_requestUri, updateContent);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task TestDeleteUser_ValidId()
    {
        // Arrange
        const int userId = 2;
        _requestUri += $"/{userId}";

        // Act
        var response = await _client.DeleteAsync(_requestUri);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task TestDeleteUser_InvalidId()
    {
        // Arrange
        const int invalidUserId = 10;
        _requestUri += $"/{invalidUserId}";

        // Act
        var response = await _client.DeleteAsync(_requestUri);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task TestGetNotesByUserId_ValidUserId()
    {
        // Arrange
        const int userId = 1;
        _requestUri += $"/{userId}/notes";

        // Act
        var response = await _client.GetAsync(_requestUri);
        var responseString = await response.Content.ReadAsStringAsync();
        var actual = JsonConvert.DeserializeObject<List<NoteViewDto>>(responseString);

        // Assert
        Assert.That(actual, Is.Not.Empty);
    }

    [Test]
    public async Task TestGetNotesByUserId_InvalidUserId()
    {
        // Arrange
        const int invalidUserId = 10;
        _requestUri += $"/{invalidUserId}/notes";

        // Act
        var response = await _client.GetAsync(_requestUri);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task TestChangePassword_ValidModel_ValidUserId_ValidContent()
    {
        // Arrange
        const int userId = 1;
        _requestUri += $"/{userId}/password-change";
        const string oldPassword = "asdf1234";
        const string newPassword = "22222222";

        var content = new UserChangePasswordDto
        {
            OldPassword = oldPassword,
            NewPassword = newPassword
        };

        // Act
        var response = await _client.PutAsJsonAsync(_requestUri, content);
        var responseString = await response.Content.ReadAsStringAsync();
        var actual = JsonConvert.DeserializeObject<UserViewDto>(responseString);

        // Assert
        Assert.That(actual.Id, Is.EqualTo(userId));
    }

    [Test]
    public async Task TestChangePassword_ValidModel_ValidUserId_InvalidContent()
    {
        // Arrange
        const int userId = 1;
        _requestUri += $"/{userId}/password-change";
        const string wrongPassword = "11111111";
        const string newPassword = "22222222";

        var invalidContent = new UserChangePasswordDto
        {
            OldPassword = wrongPassword,
            NewPassword = newPassword
        };

        // Act
        var response = await _client.PutAsJsonAsync(_requestUri, invalidContent);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task TestChangePassword_ValidModel_InvalidUserId()
    {
        // Arrange
        const int invalidUserId = 10;
        _requestUri += $"/{invalidUserId}/password-change";
        const string wrongPassword = "11111111";
        const string newPassword = "22222222";

        var content = new UserChangePasswordDto
        {
            OldPassword = wrongPassword,
            NewPassword = newPassword
        };

        // Act
        var response = await _client.PutAsJsonAsync(_requestUri, content);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task TestChangePassword_InvalidModel()
    {
        // Arrange
        const int userId = 1;
        _requestUri += $"/{userId}/password-change";

        var invalidContent = new UserChangePasswordDto();
        
        // Act
        var response = await _client.PutAsJsonAsync(_requestUri, invalidContent);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}

