using Xunit;

namespace HandlebarsDocx.Tests
{
    // see example explanation on xUnit.net website:
    // https://xunit.github.io/docs/getting-started-dnx.html
    public class WithHelperTests
    {
        public PersonDetails Person => new PersonDetails{ FirstName = "Andrew" };

        [Fact]
        public void ReplacePropertyOnNestedObject()
        {
            using (var docs = TestDocument.Create("{{#with Person}}{{FirstName}}{{/with}}"))
            {
                var replacedDocument = HandlebarsDocument.Replace(docs, this);
                var innerText = replacedDocument.MainDocumentPart.Document.Body.InnerText;
                Assert.Equal("Andrew", docs.MainDocumentPart.Document.Body.InnerText);
            }
        }

        public class PersonDetails
        {
            public string FirstName { get; set; }
        }
    }
}
