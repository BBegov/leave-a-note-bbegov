using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using leave_a_note_core.Models.DTOs;
using Newtonsoft.Json;

namespace leave_a_note_test;

public class InMemoryNotesApiTests
{
    private readonly HttpClient _client;
    private const string BaseUrl = "https://localhost:44321/api/notes";

    public InMemoryNotesApiTests()
    {
        var factory = new TestingWebAppFactory<Program>();
        _client = factory.CreateClient();
    }

    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public async Task TestGetAllNotes()
    {
        // Arrange
        // Act
        var response = await _client.GetAsync(BaseUrl);
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
        const string invalidUrl = BaseUrl + "wrongEnding";
        // Act
        var response = await _client.GetAsync(invalidUrl);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task TestGetNote_ValidId()
    {
        // Arrange
        const int noteId = 2;

        // Act
        var response = await _client.GetAsync(BaseUrl + $"/{noteId}");
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

        // Act
        var response = await _client.GetAsync(BaseUrl + $"/{invalidNoteId}");

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
        var response = await _client.PostAsJsonAsync(BaseUrl, content);
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
        var response = await _client.PostAsJsonAsync(BaseUrl, invalidContent);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    [TestCase(1, "This will be replaced")]
    [TestCase(1, "Hey!")]
    public async Task TestUpdateNote_ValidModel_ValidNoteId(int noteId, string updateText)
    {
        // Arrange
        var content = new NoteUpdateDto
        {
            NoteText = updateText,
            UserId = 1
        };

        // Act
        var response = await _client.PutAsJsonAsync(BaseUrl + $"/{noteId}", content);
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
        var invalidContent = new NoteUpdateDto();

        // Act
        var response = await _client.PutAsJsonAsync(BaseUrl + $"/{noteId}", invalidContent);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task TestUpdateNote_ValidModel_InvalidNoteId()
    {
        // Arrange
        const int invalidNoteId = 10;
        var updateContent = new NoteUpdateDto
        {
            NoteText = "Hey!",
            UserId = 1
        };

        // Act
        var response = await _client.PutAsJsonAsync(BaseUrl + $"/{invalidNoteId}", updateContent);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task TestDeleteNote_ValidId()
    {
        // Arrange
        const int noteId = 3;

        // Act
        var response = await _client.DeleteAsync(BaseUrl + $"/{noteId}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task TestDeleteNote_InvalidId()
    {
        // Arrange
        const int invalidNoteId = 10;

        // Act
        var response = await _client.DeleteAsync(BaseUrl + $"/{invalidNoteId}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}