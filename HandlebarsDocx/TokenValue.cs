namespace HandlebarsDocx
{
    public class TokenValue
    {
        public TokenValue(string name, string tokenValue)
      {
            this.Name = name;
            this.Value = tokenValue;
        }

        public string Name { get; }
        public string Value { get; }
        public string TokenString => "{{" + Name + "}}";
    }
}
