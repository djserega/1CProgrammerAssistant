using _1CProgrammerAssistant.Additions;
using System.Text;
using System.Text.RegularExpressions;

namespace MakingCode
{
    internal class MakingSourceText
    {
        private readonly string _prefixString = "\t\t\t";
        private readonly string _prefixStringCondition = "\t";
        private readonly string _prefixText = string.Empty;
        private readonly int _lengthSource = 0;

        private readonly StringBuilder _textBuilder = new StringBuilder();

        internal MakingSourceText(string source)
        {
            Source = source;
            SourceToUpper = Source.ToUpper();

            _prefixText = GetPrefixSpaceSourceText();

            Source = Source.Trim();

            _lengthSource = Source.Length;

            if (Source.Contains("(")
                && Source.Contains(",")
                && Source.Contains(");"))
            {
                TypeSourceCode = TypeSourceCode.CallMethod;
            }
            if (Source.StartsWith("Если", true, null)
                && Source.EndsWith("Тогда", true, null))
            {
                TypeSourceCode = TypeSourceCode.Condition;
            }
        }

        internal string Source { get; }
        internal string SourceToUpper { get; }
        internal TypeSourceCode TypeSourceCode { get; }

        internal string MakeText()
        {
            _textBuilder.Clear();

            switch (TypeSourceCode)
            {
                case TypeSourceCode.CallMethod:
                    MakeTextCallMethod();
                    break;
                case TypeSourceCode.Condition:
                    MakeTextCondition();
                    break;
                case TypeSourceCode.None:
                default:
                    return string.Empty;
            }
            return _textBuilder.ToString();
        }

        private void MakeTextCallMethod()
        {

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
        }

        private void MakeTextCondition()
        {
            string currentWord = string.Empty;

            Regex regex = new Regex("если\\s|\\sи\\s|\\sили\\s|\\sтогда", RegexOptions.IgnoreCase); // если\s|\sи\s|\sили\s|\sтогда

            MatchCollection matches = regex.Matches(Source);
            Match previousMatch = null;
            foreach (Match match in matches)
            {
                string currentValue = match.Value;
                string currentValueToLower = currentValue.ToLower();
                if (currentValueToLower.Equals("если "))
                {
                    AppendText(currentValue, false, false);
                }
                else
                {
                    if (previousMatch != null)
                    {
                        int startIndex = previousMatch.Index + previousMatch.Length;
                        string text = Source.Substring(
                            startIndex,
                            match.Index - startIndex);

                        bool isEndCondition = currentValueToLower.Equals(" тогда");

                        AppendText(
                            text,
                            true,
                            !isEndCondition,
                            posfixString: (isEndCondition ? " " : _prefixStringCondition) + currentValue.Trim());

                    }
                }
                previousMatch = match;
            }
        }

        private void AppendText(string text,
                                bool appendPrefixString,
                                bool appendEndLine = true,
                                bool appendBeforeLine = false,
                                string posfixString = null)
        {
            if (appendBeforeLine)
                _textBuilder.AppendLine();

            _textBuilder.Append(_prefixText);

            if (appendPrefixString)
            {
                switch (TypeSourceCode)
                {
                    case TypeSourceCode.CallMethod:
                        _textBuilder.Append(_prefixString);
                        break;
                    case TypeSourceCode.Condition:
                        _textBuilder.Append(_prefixStringCondition);
                        break;
                }
            }

            _textBuilder.Append(text.Trim());

            if (appendEndLine)
                _textBuilder.AppendLine();

            if (posfixString != null)
                _textBuilder.Append(posfixString);

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
