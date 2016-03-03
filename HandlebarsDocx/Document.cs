using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace HandlebarsDocx
{
    public class Document
    {
        private readonly WordprocessingDocument _document;

        public Document(WordprocessingDocument document)
        {
            _document = document;
        }

        public IEnumerable<FoundToken> Tokens() => Paragraphs().SelectMany(q => q.Tokens());

        public IEnumerable<Range> Paragraphs()
        {
            return _document.MainDocumentPart
                                        .Document
                                        .Body
                                        .Descendants<Paragraph>()
                                        .Select(p => new Range(Characters(p)));
        }

        private IEnumerable<Character> Characters(Paragraph paragraph)
        {
            return paragraph.Descendants<Text>()
                            .Select(t => new Element(t))
                            .SelectMany(q => q.Characters);
        }


        public IEnumerable<Helper> Helpers()
        {
            return Tokens()
                    .Where(q => q.Name.StartsWith("#"))
                    .Select(t => new Helper(t));
        }
    }
}
