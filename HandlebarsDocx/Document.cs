using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;

namespace HandlebarsDocx
{
    public class Document
    {
        private WordprocessingDocument document;

        public Document(WordprocessingDocument document)
        {
            this.document = document;
        }

        public IEnumerable<FoundToken> Tokens()
        {
            var paragraphs = document.MainDocumentPart
                                                .Document
                                                .Body
                                                .Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>()
                                                .Select(p => new Paragraph(p));

            foreach (var paragraph in paragraphs)
            {
                var characters = paragraph.Characters;

                int searchPoint = 0;
                while(searchPoint < paragraph.Text.Length && paragraph.Text.IndexOf("{{", searchPoint) > -1)
                {
                    var startIndex = paragraph.Text.IndexOf("{{", searchPoint);
                    var endIndex = paragraph.Text.IndexOf("}}", searchPoint) + 2;
                    if (startIndex > -1 && endIndex > -1)
                    {
                        yield return new FoundToken(paragraph, startIndex, endIndex);
                    }
                    searchPoint = endIndex;
                }
            }
        }
    }
}
