using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;

namespace HandlebarsDocx
{
    public class Document
    {
        private readonly WordprocessingDocument _document;

        public Document(WordprocessingDocument document)
        {
            _document = document;
        }

        public IEnumerable<FoundToken> Tokens()   => Paragraphs().SelectMany(q => q.Tokens());

        public IEnumerable<Paragraph> Paragraphs()
        {
            return _document.MainDocumentPart
                                        .Document
                                        .Body
                                        .Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>()
                                        .Select(p => new Paragraph(p));
        }

        public IEnumerable<Helper> Helpers()
        {
            return Tokens()
                    .Where(q => q.Name.StartsWith("#"))
                    .Select(t => new Helper(t));
        }
    }
}
