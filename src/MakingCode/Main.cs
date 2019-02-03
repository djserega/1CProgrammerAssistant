using System;
using System.Linq;
using System.Text;

namespace MakingCode
{
    public class Main
    {
        private const string _prefixString = "\t\t\t";

        public string SourceText { get; set; }
        public string ResultText { get; private set; }

        public bool Making()
        {
            ResultText = string.Empty;

            if (string.IsNullOrWhiteSpace(SourceText))
                return false;

            string workText = SourceText.Trim();

            if (!workText.EndsWith(";"))
                return false;

            if (!workText.Contains("(")
                || !workText.Contains(");"))
                return false;

            string result = MakingSourceText(workText);
            
            if (string.IsNullOrWhiteSpace(result))
                return false;

            ResultText = result;

            return true;
        }

        private string MakingSourceText(string workText)
        {
            string prefixText = GetPrefixSpaceSourceText();

            string[] parameters = new string[4] { "(", ", ", ",", ");" };

            StringBuilder workTextBuilder = new StringBuilder();

            int startPosition = 0;
            int lengthWorkText = workText.Length;
            char currentSymbol = char.MinValue;
            char nextSymbol = char.MinValue;
            bool CheckStartParameters = true;
            bool CheckEndParameters = false;
            for (int i = 0; i < lengthWorkText; i++)
            {
                currentSymbol = workText[i];
                if (i + 1 < lengthWorkText)
                    nextSymbol = workText[i + 1];
                else
                    nextSymbol = char.MinValue;

                if (CheckStartParameters && currentSymbol == '(')
                {
                    workTextBuilder.Append(prefixText);
                    workTextBuilder.AppendLine(workText.Substring(0, i + 1));
                    CheckStartParameters = false;
                    CheckEndParameters = true;
                    startPosition = i;
                }
                else if (currentSymbol == ',')
                {
                    workTextBuilder.Append(prefixText);
                    workTextBuilder.Append(_prefixString);
                    workTextBuilder.AppendLine(workText.Substring(startPosition + 1, i - startPosition).Trim());
                    startPosition = i++;
                }
                else if (CheckEndParameters && currentSymbol == ')' && nextSymbol == ';')
                {
                    workTextBuilder.Append(prefixText);
                    workTextBuilder.Append(_prefixString);
                    workTextBuilder.AppendLine(workText.Substring(startPosition + 1).Trim());
                }
            }

            string result = workTextBuilder.ToString();

            return result;
        }

        private string GetPrefixSpaceSourceText()
        {
            StringBuilder builderSpace = new StringBuilder();

            foreach (char symbol in SourceText)
            {
                if (symbol == '\t')
                    builderSpace.Append('\t');
                else if (symbol == ' ')
                    builderSpace.Append(' ');
                else
                    break;
            }

            string prefixText = builderSpace.ToString();

            return prefixText;
        }
    }
}
