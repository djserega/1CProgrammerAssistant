using System.Text;

namespace MakingCode
{
    internal class MakingSourceText
    {
        internal MakingSourceText(string source)
        {
            Source = source;

            PrefixText = GetPrefixSpaceSourceText();

            string sourceCopy = ((string)Source.Clone()).Trim();

            if (sourceCopy.Contains("(")
                && sourceCopy.Contains(",")
                && sourceCopy.Contains(");"))
            {
                TypeSourceCode = TypeSourceCode.CallMethod;
            }
            else if (sourceCopy.StartsWith("Если", true, null)
                && sourceCopy.EndsWith("Тогда", true, null))
            {
                TypeSourceCode = TypeSourceCode.Condition;
            }
        }

        internal string Source { get; }
        internal TypeSourceCode TypeSourceCode { get; }
        internal string PrefixText { get; } 

        internal string MakeText()
        {
            switch (TypeSourceCode)
            {
                case TypeSourceCode.CallMethod:
                    return new MakingTextCallMethod(Source, PrefixText).MakeText();
                case TypeSourceCode.Condition:
                    return new MakingTextCondition(Source, PrefixText).MakeText();
                case TypeSourceCode.None:
                default:
                    return string.Empty;
            }
        }

        private string GetPrefixSpaceSourceText()
        {
            StringBuilder builderPrefixSpace = new StringBuilder();

            foreach (char symbol in Source)
            {
                if (symbol == '\t')
                    builderPrefixSpace.Append('\t');
                else if (symbol == ' ')
                    builderPrefixSpace.Append(' ');
                else
                    break;
            }
            return builderPrefixSpace.ToString();
        }
    }
}
