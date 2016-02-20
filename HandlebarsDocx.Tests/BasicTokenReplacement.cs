using Xunit;
using DocumentFormat.OpenXml.Wordprocessing;

namespace HandlebarsDocx.Tests
{
    public class BasicTokenReplacement
    {
        public string Name => "Andy";
        public string Hello => "Hello World";
        public BasicTokenReplacement.PersonDetails Person => new BasicTokenReplacement.PersonDetails{ FirstName = "Andrew" };

        [Fact]
        public void ShouldReplaceBasicHandlebarsSyntax()
        {
            var document = TestDocument.Create("{{Hello}}");
            var replacedDocument = HandlebarsDocxReplacement.Replace(document, this);
            var innerText = replacedDocument.MainDocumentPart.Document.Body.InnerText;
            Assert.True(innerText.Contains("Hello World"));
            Assert.False(innerText.Contains("{{Hello}}"));
        }

        [Fact]
        public void ShouldReplaceMultipleTokens()
        {
            var document = TestDocument.Create("{{Hello}}, {{Name}}");
            var replacedDocument = HandlebarsDocxReplacement.Replace(document, this);
            var innerText = replacedDocument.MainDocumentPart.Document.Body.InnerText;
            Assert.True(innerText.Contains("Hello World, Andy"));
            Assert.False(innerText.Contains("{{Hello}}"));
            Assert.False(innerText.Contains("{{Name}}"));
        }

        [Fact]
        public void ShouldReplaceTokensSpreadoverTextNodes()
        {
              var document = TestDocument.Create(new Text("{"), new Text("{"), new Text("Hello"), new Text("}"), new Text("}"));
              var replacedDocument = HandlebarsDocxReplacement.Replace(document, this);
              var innerText = replacedDocument.MainDocumentPart.Document.Body.InnerText;
              Assert.True(innerText.Contains("Hello World"));
              Assert.False(innerText.Contains("{{Hello}}"));
        }

        [Fact]
        public void ShouldReplaceNestedProperty()
        {
            var document = TestDocument.Create("{{Person.FirstName}}");
            var replacedDocument = HandlebarsDocxReplacement.Replace(document, this);
            var innerText = replacedDocument.MainDocumentPart.Document.Body.InnerText;
            Assert.True(innerText.Contains("Andrew"));
        }

        public class PersonDetails
        {
            public string FirstName { get; set; }
        }
    }
}
