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
                    var topPropertyValue = GetValue<object>(helper.Args[0], values);
                    foreach (var nestedToken in document.Tokens().TakeWhile(q => q.Name != "/with"))
                    {
                        var replacementValue = GetValue<string>(nestedToken.Name, topPropertyValue);
                        nestedToken.Replace(replacementValue);
                    }

                    document.Tokens().First(q => q.Name == "/with").Replace("");
                }
                else if (helper.Name == "if")
                {
                    helper.EndToken.Replace("");

                    var showContent = GetValue<bool>(helper.Args[0], values);
                    if (!showContent)
                    {
                        helper.Contents.Remove();
                    }

                    helper.StartToken.Replace("");
                }
            }

            foreach (var paragraph in document.Paragraphs())
            {
                Replace(paragraph, values);
            }


            return wordDocument;
        }

        private static void Replace(Range range, object value)
        {
            foreach (var token in range
                                    .Tokens()
                                    .Where(q => !q.Name.StartsWith("#") && !q.Name.StartsWith("/")))
            {
                var replaceText = GetValue<string>(token.Name, value);
                token.Replace(replaceText);
            }
        }

        private static T GetValue<T>(string name, object values)
        {
            var propertyName = name.Split('.').First();
            var property = values.GetType()
                                    .GetProperties()
                                    .First(q => q.Name == propertyName);

            var tokenValue = property.GetValue(values);
            if (propertyName == name)
                return (T)tokenValue;

            return GetValue<T>(name.Substring(propertyName.Length + 1), tokenValue);
        }
    }
}
