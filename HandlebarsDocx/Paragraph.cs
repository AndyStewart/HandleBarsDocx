using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;

namespace HandlebarsDocx
{
    public class Paragraph
    {
        private readonly DocumentFormat.OpenXml.Wordprocessing.Paragraph element;

        public Paragraph(DocumentFormat.OpenXml.Wordprocessing.Paragraph element)
        {
            this.element = element;
        }

        public IEnumerable<Character> Characters => element.Descendants<Text>()
                                                             .Select(t => new Element(t))
                                                             .SelectMany(q => q.Characters);

        public string Text => Characters.Aggregate("", (c, current) => c + current.Text);

        public void Replace(int start, int end, TokenValue token)
        {
            var insertionPoint = Characters.Skip(start).First();
            insertionPoint.Insert(token.Value);

            var newStart = start + token.Value.Length;
            var charsToRemove = Characters.Skip(newStart).Take(token.TokenString.Length - 1).ToList();
            charsToRemove.Reverse();
            charsToRemove.ForEach(q => q.Remove());
        }
    }
}
