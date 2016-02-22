using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;

namespace HandlebarsDocx
{
    public class HandlebarsDocument
    {
        public static WordprocessingDocument Replace(WordprocessingDocument document, object values)
        {
            foreach (var token in new Document(document).Tokens())
            {
                if (token.Name == "#with")
                {
                    token.Replace("");
                    var topPropertyValue = GetValue(token.Args[0], values);
                    foreach (var nestedToken in FindTokensInDocument(document).TakeWhile(q => q.Name != "/with"))
                    {
                        var replacementValue = GetValue(nestedToken.Name, topPropertyValue);

                        nestedToken.Replace(replacementValue.ToString());
                    }

                    var endToken = FindTokensInDocument(document).First(q => q.Name == "/with");
                    endToken.Replace("");

                }
                else if (token.Name == "#if")
                {
                    token.Replace("");
                    var endToken = FindTokensInDocument(document).First(q => q.Name == "/if");
                    endToken.Replace("");

                }
                else
                {
                    var replaceText = GetValue(token.Name, values);
                    token.Replace(replaceText.ToString());
                }
            }

            return document;
        }

        private static object GetValue(string name, object values)
        {
            var propertyName = name.Split('.').First();
            var property = values.GetType()
                                    .GetProperties()
                                    .First(q => q.Name == propertyName);

            var tokenValue = property.GetValue(values);
            if (propertyName == name)
                return tokenValue;

            return GetValue(name.Substring(propertyName.Length + 1), tokenValue);
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
    }
}
