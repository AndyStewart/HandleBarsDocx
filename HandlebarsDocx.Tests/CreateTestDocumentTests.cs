using Xunit;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace HandlebarsDocx
{
    // see example explanation on xUnit.net website:
    // https://xunit.github.io/docs/getting-started-dnx.html
    public class CreateTestDocumentTests
    {
        [Fact]
        public void TestDocumentContainsExpectedText()
        {
            using (var docs = CreateTestDocument("Hello World"))
            {
                Assert.Equal("Hello World", docs.MainDocumentPart.Document.Body.InnerText);
            }
        }

        private static WordprocessingDocument CreateTestDocument(string contents)
        {
            var docs = WordprocessingDocument.Create("c:\\repos\\a.docx", WordprocessingDocumentType.Document);
            var mainPart = docs.AddMainDocumentPart();
            mainPart.Document = new Document();
            var body = mainPart.Document.AppendChild(new Body());
            var para = body.AppendChild(new Paragraph());
            var run = para.AppendChild(new Run());
            run.AppendChild(new Text(contents));
            return docs;
        }
    }
}
