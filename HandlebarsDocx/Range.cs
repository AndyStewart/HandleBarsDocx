using System.Collections.Generic;
using System.Linq;

namespace HandlebarsDocx
{
    public class Range
    {
        public IEnumerable<Character> Characters { get; }

        public Range(IEnumerable<Character> characters)
        {
            Characters = characters;
        }

        public string Text => Characters.Aggregate("", (c, current) => c + current.Text);

        public IEnumerable<FoundToken> Tokens()
        {
            var searchPoint = 0;
            while (searchPoint < Text.Length && Text.IndexOf("{{", searchPoint) > -1)
            {
                var startIndex = Text.IndexOf("{{", searchPoint);
                var endIndex = Text.IndexOf("}}", searchPoint) + 2;
                if (startIndex > -1 && endIndex > -1)
                {
                    yield return new FoundToken(this, startIndex, endIndex);
                }
                searchPoint = endIndex;
            }
        }

        public void Replace(int start, int end, FoundToken token, string value)
        {
            var insertionPoint = Characters.Skip(start).First();
            insertionPoint.Insert(value);

            var newStart = start + value.Length;
            var charsToRemove = Characters.Skip(newStart).Take(token.Token.Length - 1).ToList();
            charsToRemove.Reverse();
            charsToRemove.ForEach(q => q.Remove());
        }
    }
}