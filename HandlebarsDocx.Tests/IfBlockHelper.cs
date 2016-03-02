using System;
using Xunit;

namespace HandlebarsDocx.Tests
{
    public class IfBlockHelper
    {
        public bool True => true;
        public bool False => false;

        [Fact]
        public void ShouldShowContentsWhenTrue()
        {
            var document = TestDocument.Create("{{#if True}}Hello World{{/if}}");
            var replacedDocument = HandlebarsDocument.Replace(document, this);
            var innerText = replacedDocument.MainDocumentPart.Document.Body.InnerText;
            Assert.True(innerText.Contains("Hello World"));
            Assert.False(innerText.Contains("{{#if Visibile}}"));
        }

        [Fact]
        public void ShouldRemoveContentsWhenFalse()
        {
            var document = TestDocument.Create("{{#if False}}Hello World{{/if}}");
            var replacedDocument = HandlebarsDocument.Replace(document, this);
            var innerText = replacedDocument.MainDocumentPart.Document.Body.InnerText;
            Assert.Equal("", innerText);
        }
    }

}
