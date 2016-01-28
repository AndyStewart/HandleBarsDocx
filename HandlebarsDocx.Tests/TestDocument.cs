using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace HandlebarsDocx
{
    public class TestDocument
    {
        public static WordprocessingDocument Create(string contents)
        {
            var docs = WordprocessingDocument.Create("c:\\repos\\a.docx", WordprocessingDocumentType.Document);
            var mainPart = docs.AddMainDocumentPart();
            mainPart.Document = new Document();
            var body = mainPart.Document.AppendChild(new Body());
            var para = body.AppendChild(new Paragraph());
            var run = para.AppendChild(new Run());
            run.AppendChild(new Text(contents));
            return docs;
        }
    }
}
