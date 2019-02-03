using _1CProgrammerAssistant.Additions;
using System.Text;

namespace MakingCode
{
    internal class MakingSourceText
    {
        private readonly string _prefixString = "\t\t\t";
        private readonly string _prefixText = string.Empty;
        private readonly int _lengthSource = 0;

        private readonly StringBuilder _textBuilder = new StringBuilder();

        internal MakingSourceText(string source)
        {
            Source = source;

            _prefixText = GetPrefixSpaceSourceText();

            Source = Source.Trim();

            _lengthSource = Source.Length;
        }

        internal string Source { get; }

        internal string MakeText()
        {
            _textBuilder.Clear();

            int startPosition = 0;
            char currentSymbol = char.MinValue;
            char nextSymbol = char.MinValue;
            bool CheckStartParameters = true;
            bool CheckEndParameters = false;

            for (int i = 0; i < _lengthSource; i++)
            {
                currentSymbol = Source[i];
                if (i + 1 < _lengthSource)
                    nextSymbol = Source[i + 1];
                else
                    nextSymbol = char.MinValue;

                if (CheckStartParameters && currentSymbol == '(')
                {
                    AppendText(Source.Left(i + 1), false);

                    CheckStartParameters = false;
                    CheckEndParameters = true;
                    startPosition = i;
                }
                else if (currentSymbol == ',')
                {
                    AppendText(Source.Substring(startPosition + 1, i - startPosition), true);
                    startPosition = i;
                }
                else if (CheckEndParameters && currentSymbol == ')' && nextSymbol == ';')
                {
                    AppendText(Source.Substring(startPosition + 1), true);
                }
            }

            return _textBuilder.ToString();
        }

        private void AppendText(string text, bool appendPrefixString)
        {
            _textBuilder.Append(_prefixText);

            if (appendPrefixString)
                _textBuilder.Append(_prefixString);

            _textBuilder.AppendLine(text.Trim());
        }

        private string GetPrefixSpaceSourceText()
        {
            StringBuilder builderSpace = new StringBuilder();

            foreach (char symbol in Source)
            {
                if (symbol == '\t')
                    builderSpace.Append('\t');
                else if (symbol == ' ')
                    builderSpace.Append(' ');
                else
                    break;
            }
            return builderSpace.ToString();
        }

    }
}
