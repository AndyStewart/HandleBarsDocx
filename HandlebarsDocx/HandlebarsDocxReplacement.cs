using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            var paragraphs = document.MainDocumentPart
                                                .Document
                                                .Body
                                                .Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>()
                                                .Select(p => new Paragraph(p));

            foreach (var paragraph in paragraphs)
            {
                var characters = paragraph.Characters;

                int searchPoint = 0;
                while(searchPoint < paragraph.Text.Length && paragraph.Text.IndexOf("{{", searchPoint) > -1)
                {
                    var startIndex = paragraph.Text.IndexOf("{{", searchPoint);
                    var endIndex = paragraph.Text.IndexOf("}}", searchPoint) + 2;
                    if (startIndex > -1 && endIndex > -1)
                    {
                        yield return new FoundToken(paragraph, startIndex, endIndex);
                    }
                    searchPoint = endIndex;
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
}
