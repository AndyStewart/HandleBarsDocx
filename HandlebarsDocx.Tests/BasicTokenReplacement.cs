using Xunit;
using HandlebarsDocx;

namespace HandlebarsDocx.Tests
{
    public class BasicTokenReplacement
    {
        public string Hello => "Hello World";

        [Fact]
        public void ShouldReplaceBasicHandlebarsSyntax()
        {
            var document = TestDocument.Create("{{Hello}}");
            var replacedDocument = HandlebarsDocxReplacement.Replace(document, this);
            var innerText = replacedDocument.MainDocumentPart.Document.Body.InnerText;
            Assert.True(innerText.Contains("Hello World"));
            Assert.False(innerText.Contains("{{Hello}}"));
        }
    }
}
