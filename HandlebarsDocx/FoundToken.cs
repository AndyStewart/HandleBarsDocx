using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

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

        public void Replace(Token replaceText)
        {
            element.Replace(start, end, replaceText);
        }
    }
}
