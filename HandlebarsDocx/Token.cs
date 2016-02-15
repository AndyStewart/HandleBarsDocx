using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace HandlebarsDocx
{
    public class Token
    {
        public Token(string name, string tokenValue)
      {
            this.Name = name;
            this.Value = tokenValue;
        }

        public string Name { get; }
        public string Value { get; }
        public string TokenString => "{{" + Name + "}}";
    }
}
