using System;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;

namespace HandlebarsDocx
{
    public class HandlebarsDocument
    {
        public static WordprocessingDocument Replace(WordprocessingDocument wordDocument, object values)
        {
            var document = new Document(wordDocument);
            foreach (var helper in document.Helpers())
            {
                if (helper.Name == "with")
                {
                    helper.StartToken.Replace("");
                    var topPropertyValue = GetValue(helper.Args[0], values);
                    foreach (var nestedToken in document.Tokens().TakeWhile(q => q.Name != "/with"))
                    {
                        var replacementValue = GetValue(nestedToken.Name, topPropertyValue);
                        nestedToken.Replace(replacementValue.ToString());
                    }

                    document.Tokens().First(q => q.Name == "/with").Replace("");
                }
                else if (helper.Name == "if")
                {
                    helper.EndToken.Replace("");

                    var showContent = (bool)GetValue(helper.Args[0], values);
                    if (!showContent)
                    {
                        helper
                            .Contents
                            .Reverse()
                            .ToList()
                            .ForEach(c => c.Remove());
                    }

                    helper.StartToken.Replace("");
                }
            }

            foreach (var token in document
                                    .Tokens()
                                    .Where(q => !q.Name.StartsWith("#") && !q.Name.StartsWith("/")))
            {
                var replaceText = GetValue(token.Name, values);
                token.Replace(replaceText.ToString());
            }

            return wordDocument;
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
    }
}
