using Xunit;

namespace HandlebarsDocx.Tests
{
    // see example explanation on xUnit.net website:
    // https://xunit.github.io/docs/getting-started-dnx.html
    public class CreateTestDocumentTests
    {
        [Fact]
        public void TestDocumentContainsExpectedText()
        {
            using (var docs = TestDocument.Create("Hello World"))
            {
                Assert.Equal("Hello World", docs.MainDocumentPart.Document.Body.InnerText);
            }
        }
    }
}
