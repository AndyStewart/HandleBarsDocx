using Xunit;

namespace HandlebarsDocx.Tests
{
    public class IfBlockHelper
    {
        public bool Visibile => true;

        [Fact]
        public void ShouldShowContentsWhenTrue()
        {
            var document = TestDocument.Create("{{#if Visibile}}Hello World{{/if}}");
            // HandlebarsDocx.RegisterHelper("if", value => {
            //     return "Hello World";
            // });

            var replacedDocument = HandlebarsDocument.Replace(document, this);
            var innerText = replacedDocument.MainDocumentPart.Document.Body.InnerText;
            Assert.True(innerText.Contains("Hello World"));
            Assert.False(innerText.Contains("{{#if Visibile}}"));
        }
    }

}
