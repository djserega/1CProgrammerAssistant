using _1CProgrammerAssistant.Additions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _1CProgrammerAssistant.DescriptionsTheMethods
{
    internal class MakingDescription
    {
        private readonly string _prefixString = "\t\t\t";
        private readonly string _prefix = "\t";
        private readonly List<Models.ObjectParameter> _parametersMethod = new List<Models.ObjectParameter>();

        public MakingDescription(string source)
        {
            Source = source;
        }

        internal string Source { get; }
        internal string Description { get; private set; }
        internal string MethodName { get; private set; }

        private bool ExportMethod { get => Source.TrimEnd().EndsWith("экспорт", true, null); }
        private string WrapLineParameters { get; set; }

        private bool ManyParameters { get => _parametersMethod.Count() > 1; }


        internal void Making(bool appendReturnValue, bool includeStringMethod)
        {
            Description = string.Empty;
            MethodName = string.Empty;

            CreateParameters();
            CreateDescription(appendReturnValue, includeStringMethod);

            if (includeStringMethod)
            {
                if (ManyParameters)
                {
                    int positionOpeningBracket = Source.IndexOf("(");
                    string methodNameBeforeBracket = Source.Substring(0, positionOpeningBracket);

                    Description += $"{methodNameBeforeBracket}({WrapLineParameters})";
                    Description += ExportMethod ? " Экспорт " : "  ";
                }
                else
                {
                    Description += Source;
                }

                if (Source.Last() == '\n')
                    Description += '\n';
            }
        }

        private void CreateParameters()
        {
            _parametersMethod.Clear();

            string parser = Source;

            parser = parser.NonUsedStart();

            int countOpeningBracket = parser.Count(f => f == '(');
            if (countOpeningBracket == 1)
            {
                int positionOpeningBracket = parser.IndexOf("(");

                MethodName = parser.Substring(0, positionOpeningBracket);

                parser = parser.Substring(positionOpeningBracket + 1);

                int countClosingBracket = parser.Count(f => f == ')');

                if (countClosingBracket == 1)
                {
                    parser = parser.NonUsedEnd();

                    StringBuilder builderWrapParameters = new StringBuilder();
                    builderWrapParameters.AppendLine();

                    string[] parserParameters = parser.Split(',');
                    int parametersLeft = parserParameters.Count();
                    foreach (string itemParameter in parserParameters)
                    {
                        if (!string.IsNullOrWhiteSpace(itemParameter))
                        {
                            parametersLeft--;

                            builderWrapParameters.Append(_prefix);
                            builderWrapParameters.Append(_prefixString);
                            builderWrapParameters.Append(itemParameter.Trim());
                            builderWrapParameters.AppendLine(parametersLeft > 0 ? "," : "");

                            string paramenterName = itemParameter.TrimStart().RemoveStartText("знач").Trim();

                            int positionEqual = paramenterName.IndexOf('=');
                            if (positionEqual > 0)
                                paramenterName = paramenterName.Left(positionEqual).TrimEnd();

                            Models.ObjectParameter objectParameter = new Models.ObjectParameter(paramenterName);
                            objectParameter.SetTypeByName();

                            _parametersMethod.Add(objectParameter);
                        }
                    }

                    WrapLineParameters = builderWrapParameters.ToString().TrimEnd();
                }
            }
        }

        private void CreateDescription(bool appendReturnValue, bool includeStringMethod)
        {
            StringBuilder builderDescription = new StringBuilder();

            builderDescription.AppendLine("// ");
            builderDescription.AppendLine("//");
            builderDescription.AppendLine("// Параметры:");

            if (_parametersMethod.Count == 0)
                builderDescription.AppendLine("//\t\t-  - ");
            else
                foreach (Models.ObjectParameter parameter in _parametersMethod)
                    builderDescription.AppendLine($"//\t\t{parameter.Name} - {parameter.Type} - {parameter.Description}");

            if (appendReturnValue)
            {
                builderDescription.AppendLine("//");
                builderDescription.AppendLine("// Возвращаемое значение:");
                builderDescription.AppendLine("//\t\t - ");
            }

            builderDescription.AppendLine("//");

            Description = builderDescription.ToString();
        }
    }
}
