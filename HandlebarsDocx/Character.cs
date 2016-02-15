using DocumentFormat.OpenXml.Wordprocessing;

namespace HandlebarsDocx
{
    public class Character
    {
        private Text text;
        private int position;

        public Character(Text text, int position) {
            this.text = text;
            this.position = position;
        }

        public char Text => text.Text[position];
        public void Remove() => text.Text = text.Text.Substring(0, position)
                                                + text.Text.Substring(position + 1);
        public void Insert(string value)
        {
            text.Text = text.Text.Substring(0, position)
                                                + value
                                                + text.Text.Substring(position + 1);
        }
    }


}
