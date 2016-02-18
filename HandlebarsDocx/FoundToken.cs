namespace HandlebarsDocx
{
    public class FoundToken
    {
        private readonly Paragraph element;
        private readonly int start;
        private readonly int end;
        public FoundToken(Paragraph element, int start, int end)
        {
            this.element = element;
            this.Token = element.Text.Substring(start, end - start);
            this.start = start;
            this.end = end;
        }

        public string Token { get; }

        public void Replace(TokenValue replaceText)
        {
            element.Replace(start, end, replaceText);
        }
    }
}
