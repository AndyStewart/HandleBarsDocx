using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;

namespace HandlebarsDocx
{
    public class Element
    {
        private Text text;
        public Element(Text text)
        {
            this.text = text;
        }

        public IEnumerable<Character> Characters => Enumerable
                                                        .Range(0, this.text.Text.Length)
                                                        .Select(i => new Character(text, i));

        public string Text
        {
            get
            {
                return text.Text;
            }
            set
            {
                text.Text = value;
            }
        }


    }
}
