using DocumentFormat.OpenXml.Wordprocessing;

namespace HandlebarsDocx
{
    public class Character
    {
        private readonly Text _text;
        private readonly int _position;

        public Character(Text text, int position) {
            _text = text;
            _position = position;
        }

        public char Text => _text.Text[_position];
        public void Remove() => _text.Text = _text.Text.Substring(0, _position)
                                                + _text.Text.Substring(_position + 1);
        public void Insert(string value)
        {
            _text.Text = _text.Text.Substring(0, _position)
                                                + value
                                                + _text.Text.Substring(_position + 1);
        }
    }


}
