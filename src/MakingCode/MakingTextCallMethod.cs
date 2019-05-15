using _1CProgrammerAssistant.Additions;
using System.Text;

namespace MakingCode
{
    internal class MakingTextCallMethod
    {
        private readonly string _prefixString = "\t\t\t";
        private readonly StringBuilder _textBuilder = new StringBuilder();

        public MakingTextCallMethod(string source, string prefixText)
        {
            Source = source;
            PrefixText = prefixText;
        }

        internal string Source { get; }
        internal string PrefixText { get; }
        internal int LengthSource { get => Source.Length; }

        internal string MakeText()
        {
            int startPosition = 0;
            char currentSymbol;
            char nextSymbol;
            bool checkStartParameters = true;
            bool checkEndParameters = false;

            for (int i = 0; i < LengthSource; i++)
            {
                currentSymbol = Source[i];
                if (i + 1 < LengthSource)
                    nextSymbol = Source[i + 1];
                else
                    nextSymbol = char.MinValue;

                if (checkStartParameters && currentSymbol == '(')
                {
                    AppendText(Source.Left(i + 1), false);

                    checkStartParameters = false;
                    checkEndParameters = true;
                    startPosition = i;
                }
                else if (currentSymbol == ',')
                {
                    AppendText(Source.Substring(startPosition + 1, i - startPosition), true);
                    startPosition = i;
                }
                else if (checkEndParameters && currentSymbol == ')' && nextSymbol == ';')
                {
                    AppendText(Source.Substring(startPosition + 1), true);
                }
            }

            return _textBuilder.ToString();
        }

        private void AppendText(string text,
                        bool appendPrefixString,
                        bool appendEndLine = true,
                        bool appendBeforeLine = false,
                        string posfixString = null)
        {
            if (appendBeforeLine)
                _textBuilder.AppendLine();

            _textBuilder.Append(PrefixText);

            if (appendPrefixString)
                _textBuilder.Append(_prefixString);

            _textBuilder.Append(text.Trim());

            if (appendEndLine)
                _textBuilder.AppendLine();

            if (posfixString != null)
                _textBuilder.Append(posfixString);
        }
    }
}
