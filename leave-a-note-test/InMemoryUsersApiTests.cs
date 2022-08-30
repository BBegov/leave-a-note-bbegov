using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using leave_a_note_core.Models.DTOs;
using Newtonsoft.Json;

namespace leave_a_note_test;

internal class InMemoryUsersApiTests
{
    private readonly HttpClient _client;
    private const string BaseUrl = "https://localhost:44321/api/users";

    public InMemoryUsersApiTests()
    {
        var factory = new TestingWebAppFactory<Program>();
        _client = factory.CreateClient();
    }

    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public async Task TestGetAllUsers()
    {
        // Arrange
        // Act
        var response = await _client.GetAsync(BaseUrl);
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
        const string invalidUrl = BaseUrl + "wrongEnding";
        // Act
        var response = await _client.GetAsync(invalidUrl);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task TestGetUser_ValidId()
    {
        // Arrange
        const int userId = 1;
        // Act
        var response = await _client.GetAsync(BaseUrl + $"/{userId}");
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

        // Act
        var response = await _client.GetAsync(BaseUrl + $"/{invalidUserId}");

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
        var response = await _client.PostAsJsonAsync(BaseUrl, content);
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
        var response = await _client.PostAsJsonAsync(BaseUrl, invalidContent);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    [TestCase(1, "username1")]
    [TestCase(1, "username2")]
    public async Task TestUpdateUser_ValidModel_ValidNoteId(int userId, string updateUserName)
    {
        // Arrange
        var updateContent = new UserUpdateDto
        {
            UserName = updateUserName,
            FirstName = "Name",
            LastName = "Changer"
        };

        // Act
        var response = await _client.PutAsJsonAsync(BaseUrl + $"/{userId}", updateContent);
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
        var invalidContent = new NoteUpdateDto();

        // Act
        var response = await _client.PutAsJsonAsync(BaseUrl + $"/{userId}", invalidContent);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task TestUpdateUser_ValidModel_InvalidNoteId()
    {
        // Arrange
        const string updateUserName = "username1";
        const int invalidUserId = 10;
        var updateContent = new UserUpdateDto
        {
            UserName = updateUserName,
            FirstName = "Name",
            LastName = "Changer"
        };

        // Act
        var response = await _client.PutAsJsonAsync(BaseUrl + $"/{invalidUserId}", updateContent);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task TestDeleteUser_ValidId()
    {
        // Arrange
        const int userId = 2;

        // Act
        var response = await _client.DeleteAsync(BaseUrl + $"/{userId}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task TestDeleteUser_InvalidId()
    {
        // Arrange
        const int invalidUserId = 10;

        // Act
        var response = await _client.DeleteAsync(BaseUrl + $"/{invalidUserId}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task TestGetNotesByUserId_ValidUserId()
    {
        // Arrange
        const int userId = 1;
        // Act
        var response = await _client.GetAsync(BaseUrl + $"/{userId}/notes");
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
        // Act
        var response = await _client.GetAsync(BaseUrl + $"/{invalidUserId}/notes");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task TestChangePassword_ValidModel_ValidUserId_ValidContent()
    {
        // Arrange
        const int userId = 1;
        const string oldPassword = "asdf1234";
        const string newPassword = "22222222";

        var content = new UserChangePasswordDto
        {
            OldPassword = oldPassword,
            NewPassword = newPassword
        };

        // Act
        var response = await _client.PutAsJsonAsync(BaseUrl + $"/{userId}/password-change", content);
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
        const string wrongPassword = "11111111";
        const string newPassword = "22222222";

        var invalidContent = new UserChangePasswordDto
        {
            OldPassword = wrongPassword,
            NewPassword = newPassword
        };

        // Act
        var response = await _client.PutAsJsonAsync(BaseUrl + $"/{userId}/password-change", invalidContent);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task TestChangePassword_ValidModel_InvalidUserId()
    {
        // Arrange
        const int invalidUserId = 10;
        const string wrongPassword = "11111111";
        const string newPassword = "22222222";

        var content = new UserChangePasswordDto
        {
            OldPassword = wrongPassword,
            NewPassword = newPassword
        };

        // Act
        var response = await _client.PutAsJsonAsync(BaseUrl + $"/{invalidUserId}/password-change", content);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task TestChangePassword_InvalidModel()
    {
        // Arrange
        const int userId = 1;

        var invalidContent = new UserChangePasswordDto();
        
        // Act
        var response = await _client.PutAsJsonAsync(BaseUrl + $"/{userId}/password-change", invalidContent);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}

