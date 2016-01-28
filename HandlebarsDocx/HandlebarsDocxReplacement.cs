using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace HandlebarsDocx
{
    public class HandlebarsDocxReplacement
    {
        public static WordprocessingDocument Replace(WordprocessingDocument document, object values)
        {
            foreach (var property in values.GetType().GetProperties())
            {
                foreach (var text in document.MainDocumentPart.Document.Body.Descendants<Text>())
                {
                    text.Text = text.Text.Replace("{{" + property.Name + "}}", property.GetValue(values).ToString());
                }
            }

            return document;
        }
    }
}
