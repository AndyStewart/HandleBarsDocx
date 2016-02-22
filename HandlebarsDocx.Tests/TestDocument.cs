using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace HandlebarsDocx.Tests
{
    public class TestDocument
    {

      public static WordprocessingDocument Create(params Text[] textNodes)
      {
          var memoryStream = new MemoryStream();
          var docs = WordprocessingDocument.Create(memoryStream, WordprocessingDocumentType.Document);
          var mainPart = docs.AddMainDocumentPart();
          mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
          var body = mainPart.Document.AppendChild(new Body());
          var para = body.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
          var run = para.AppendChild(new Run());
          textNodes.ToList().ForEach(q => run.AppendChild(q));
          return docs;
      }


        public static WordprocessingDocument Create(string contents)
        {
            return Create(new Text(contents));
        }
    }
}
