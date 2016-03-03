using System.Linq;
namespace HandlebarsDocx
{
    public class FoundToken
    {
        public Range Range { get; }

        public int Start { get; }
        public int End { get; }
        public FoundToken(Range element, int start, int end)
        {
            Range = element;
            Token = element.Text.Substring(start, end - start);
            Start = start;
            End = end;
        }

        public string Token { get; }
        public string Contents => Token.Substring(2, Token.Length - 4);
        public string Name => Contents.Split(' ').First();
        public string[] Args => Contents.Split(' ').Skip(1).ToArray();

        public void Replace(string value)
        {
            Range.Subset(Start, End - Start).Replace(value);
        }
    }
}
