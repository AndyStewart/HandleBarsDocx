using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace HandlebarsDocx
{
    public class HandlebarsDocxReplacement
    {
        public static WordprocessingDocument Replace(WordprocessingDocument document, object values)
        {
            var tokenValues = GetPossibleTokens(values);
            foreach (var token in FindTokensInDocument(document))
            {
                var replaceText = tokenValues.FirstOrDefault(q => q.TokenString == token.Token);
                token.Replace(replaceText);
            }

            return document;
        }

        private static IEnumerable<FoundToken> FindTokensInDocument(WordprocessingDocument document)
        {
            foreach (var paragraph in document.MainDocumentPart.Document.Body.Descendants<Paragraph>())
            {
                var token = new FoundToken(-1);
                foreach (var textElement in paragraph.Descendants<Text>())
                {
                    var IndexOfOfStartTag = textElement.Text.IndexOf("{");
                    if (IndexOfOfStartTag != -1)
                    {
                        token = new FoundToken(IndexOfOfStartTag);
                        token.AddElement(textElement);
                    }

                    var IndexOfOfEndTag = textElement.Text.IndexOf("}");
                    if (IndexOfOfEndTag != -1)
                    {
                        token.AddElement(textElement);
                        yield return token;
                    }

                    if (IndexOfOfStartTag == -1 && IndexOfOfEndTag == -1)
                    {
                        token.AddElement(textElement);
                    }
                }
            }
        }

        private static System.Collections.Generic.IEnumerable<Token> GetPossibleTokens(object values)
        {
            return values.GetType().GetProperties().Select(property => ConvertPropertyToToken(values, property));
        }

        private static Token ConvertPropertyToToken(object valueObject, PropertyInfo property)
        {
            var tokenValue = property.GetValue(valueObject).ToString();
            return new Token(property.Name, tokenValue);
        }
    }

    class FoundToken
    {
        private List<Text> elements = new List<Text>();
        private int indexOfOfStartTag;

        public FoundToken(int indexOfOfStartTag)
        {
            this.indexOfOfStartTag = indexOfOfStartTag;
        }

        public string Token
        {
            get
            {
                var allText = elements.Aggregate("", (current, next) => current + next.Text);
                var tagStart = allText.Substring(indexOfOfStartTag);
                var end = tagStart.IndexOf("}}");
                return tagStart.Substring(0, end + 2);
            }
        }

        internal void AddElement(Text textElement)
        {
            if (!elements.Contains(textElement))
              elements.Add(textElement);
        }

        internal void Replace(Token replaceText)
        {
            elements.ForEach(q => q.Text.Replace(Token, replaceText.Value));
        }
    }

    class DocumentToken
    {
        private readonly Text textElement;

        public DocumentToken(Text textElement)
        {
            var IndexOfOfStartTag = textElement.Text.IndexOf("{{");
            var IndexOfOfEndTag = textElement.Text.IndexOf("}}") + 2;

            this.textElement = textElement;
            this.Token = textElement.Text.Substring(IndexOfOfStartTag, IndexOfOfEndTag);
        }

        public string Token { get; private set; }

        public void Replace(Token replaceText)
        {
            textElement.Text = textElement.Text.Replace(Token, replaceText.Value);
        }
    }


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
