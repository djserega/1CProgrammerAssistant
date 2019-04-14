using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MakingCode
{
    internal class MakingTextCondition
    {
        private readonly string _prefixStringCondition = "\t";
        private readonly StringBuilder _textBuilder = new StringBuilder();

        public MakingTextCondition(string source, string prefixText)
        {
            Source = source;
            PrefixText = prefixText;
        }

        internal string Source { get; }
        internal string PrefixText { get; }

        internal string MakeText()
        {
            Regex regex = new Regex("если\\s|\\sи\\s|\\sили\\s|\\sтогда", RegexOptions.IgnoreCase); // если\s|\sи\s|\sили\s|\sтогда

            MatchCollection matches = regex.Matches(Source);
            Match previousMatch = null;
            foreach (Match match in matches)
            {
                string currentValue = match.Value;
                string currentValueToLower = currentValue.ToLower();
                if (currentValueToLower.Equals("если ")
                    || currentValueToLower.Equals("если\t"))
                {
                    _textBuilder.Append(PrefixText);
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

                        bool isEndCondition = currentValueToLower.Equals(" тогда") || currentValueToLower.Equals("\tтогда");

                        AppendText(
                            text,
                            true,
                            !isEndCondition,
                            posfixString: (isEndCondition ? " " : PrefixText + _prefixStringCondition) + currentValue.Trim());
                    }
                }
                previousMatch = match;
            }

            if (Source.Last() == '\n')
                _textBuilder.Append('\n');

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

            if (appendPrefixString)
                _textBuilder.Append(_prefixStringCondition);

            _textBuilder.Append(text.Trim());

            if (appendEndLine)
                _textBuilder.AppendLine();

            if (posfixString != null)
                _textBuilder.Append(posfixString);

        }
    }
}
