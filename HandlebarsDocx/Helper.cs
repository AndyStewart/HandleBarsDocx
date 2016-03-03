﻿using System.Collections.Generic;
using System.Linq;

namespace HandlebarsDocx
{
    public class Helper
    {
        public FoundToken StartToken { get; }
        public FoundToken EndToken { get; }
        public IEnumerable<Character> Contents { get; }

        public Helper(FoundToken startToken)
        {
            StartToken = startToken;
            EndToken = startToken.Paragraph.Tokens().First(t => t.Name == "/" + Name);
            Contents = startToken.Paragraph
                .Characters.Skip(StartToken.End)
                .Take(EndToken.Start - StartToken.End);
        }

        public string Name => StartToken.Name.Substring(1);
        public string[] Args => StartToken.Args;
    }
}