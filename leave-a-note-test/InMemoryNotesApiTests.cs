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

public class InMemoryNotesApiTests
{
    private readonly HttpClient _client;
    private string _requestUri = string.Empty;

    public InMemoryNotesApiTests()
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
        _requestUri = "/api/notes";
    }

    [Test]
    public async Task TestGetAllNotes()
    {
        // Arrange
        // Act
        var response = await _client.GetAsync(_requestUri);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var actual = JsonConvert.DeserializeObject<List<NoteViewDto>>(responseString);

        // Assert
        Assert.That(actual, Is.Not.Empty);
    }

    [Test]
    public async Task TestGetAllNotes_InvalidUrl()
    {
        // Arrange
        const string invalidUrlEnding = "wrongEnding";
        _requestUri += $"/{invalidUrlEnding}";

        // Act
        var response = await _client.GetAsync(_requestUri);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task TestGetNote_ValidId()
    {
        // Arrange
        const int noteId = 2;
        _requestUri += $"/{noteId}";

        // Act
        var response = await _client.GetAsync(_requestUri);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var actual = JsonConvert.DeserializeObject<NoteViewDto>(responseString);

        // Assert
        Assert.That(actual.Id, Is.EqualTo(noteId));
    }

    [Test]
    public async Task TestGetNote_InvalidId()
    {
        // Arrange
        const int invalidNoteId = 10;
        _requestUri += $"/{invalidNoteId}";

        // Act
        var response = await _client.GetAsync(_requestUri);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task TestAddNote_ValidModel()
    {
        // Arrange
        const string expectedNoteText = "Hey!";

        var content = new NoteCreateDto
        {
            NoteText = expectedNoteText,
            UserId = 2
        };

        // Act
        var response = await _client.PostAsJsonAsync(_requestUri, content);
        var responseString = await response.Content.ReadAsStringAsync();
        var actual = JsonConvert.DeserializeObject<NoteViewDto>(responseString);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(actual.NoteText, Is.EqualTo(expectedNoteText));
        });
    }

    [Test]
    public async Task TestAddNote_InvalidModel()
    {
        // Arrange
        var invalidContent = new NoteCreateDto();

        // Act
        var response = await _client.PostAsJsonAsync(_requestUri, invalidContent);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    [TestCase(1, "This will be replaced")]
    [TestCase(1, "Hey!")]
    public async Task TestUpdateNote_ValidModel_ValidNoteId(int noteId, string updateText)
    {
        // Arrange
        _requestUri += $"/{noteId}";
        var content = new NoteUpdateDto
        {
            NoteText = updateText,
            UserId = 1
        };

        // Act
        var response = await _client.PutAsJsonAsync(_requestUri, content);
        var responseString = await response.Content.ReadAsStringAsync();
        var actual = JsonConvert.DeserializeObject<NoteViewDto>(responseString);

        // Assert
        Assert.That(actual.NoteText, Is.EqualTo(updateText));
    }

    [Test]
    public async Task TestUpdateNote_InvalidModel()
    {
        // Arrange
        const int noteId = 1;
        _requestUri += $"/{noteId}";
        var invalidContent = new NoteUpdateDto();

        // Act
        var response = await _client.PutAsJsonAsync(_requestUri, invalidContent);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task TestUpdateNote_ValidModel_InvalidNoteId()
    {
        // Arrange
        const int invalidNoteId = 10;
        _requestUri += $"/{invalidNoteId}";
        var updateContent = new NoteUpdateDto
        {
            NoteText = "Hey!",
            UserId = 1
        };

        // Act
        var response = await _client.PutAsJsonAsync(_requestUri, updateContent);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task TestDeleteNote_ValidId()
    {
        // Arrange
        const int noteId = 3;
        _requestUri += $"/{noteId}";

        // Act
        var response = await _client.DeleteAsync(_requestUri);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task TestDeleteNote_InvalidId()
    {
        // Arrange
        const int invalidNoteId = 10;
        _requestUri += $"/{invalidNoteId}";

        // Act
        var response = await _client.DeleteAsync(_requestUri);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}