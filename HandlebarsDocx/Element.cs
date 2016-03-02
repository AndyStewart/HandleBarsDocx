using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;

namespace HandlebarsDocx
{
    public class Element
    {
        private readonly Text _text;
        public Element(Text text)
        {
            _text = text;
        }

        public IEnumerable<Character> Characters => Enumerable
                                                        .Range(0, _text.Text.Length)
                                                        .Select(i => new Character(_text, i));

        public string Text
        {
            get
            {
                return _text.Text;
            }
            set
            {
                _text.Text = value;
            }
        }


    }
}
