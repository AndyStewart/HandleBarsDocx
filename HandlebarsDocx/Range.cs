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

        public void Remove()
        {
            Characters
                .Reverse()
                .ToList()
                .ForEach(c => c.Remove());
        }

        public void Replace(string value)
        {
            Subset(1, Characters.Count() - 1).Remove();

            var insertionPoint = Characters.First();
            insertionPoint.Insert(value);
        }

        public Range Subset(int start, int length)
        {
            return new Range(Characters.Skip(start).Take(length));
        }
    }
}