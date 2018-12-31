using _1CProgrammerAssistant.Additions;
using _1CProgrammerAssistant.DescriptionsTheMethods.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace _1CProgrammerAssistant.DescriptionsTheMethods
{
    public class Main : INotifyPropertyChanged
    {
        private string _stringMethod;
        private string _stringMethodWithoutDirectiveCompilation;

        private List<ObjectParameter> _parametersMethod = new List<ObjectParameter>();

        public string MethodName { get; private set; }
        public string StringMethod { get => _stringMethod; set { _stringMethod = value; SetDescription(); } }
        public string Description { get; set; } = string.Empty;
        public string TextError { get; private set; } = string.Empty;
        public bool IncludeStringMethod { get; set; } = true;

        private bool StringIsFunction { get => _stringMethodWithoutDirectiveCompilation.TrimStart().StartsWith("функция", true, null); }
        private bool StringIsProcedure { get => _stringMethodWithoutDirectiveCompilation.TrimStart().StartsWith("процедура", true, null); }

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void SetDescription()
        {
            TextError = string.Empty;
            Description = string.Empty;

            _stringMethodWithoutDirectiveCompilation = RemoveNonUsedStartText(_stringMethod, false);

            if (StringIsFunction || StringIsProcedure)
                CompileDescription();
            else
                TextError = "Строка метода должна начинаться со слова: 'Процедура' или 'Функция'.";
        }

        private void CompileDescription()
        {
            SetParametersMethod();

            StringBuilder builderDescription = new StringBuilder();

            builderDescription.AppendLine("// ");
            builderDescription.AppendLine("//");
            builderDescription.AppendLine("// Параметры:");

            if (_parametersMethod.Count == 0)
                builderDescription.AppendLine("//\t\t-  - ");
            else
                foreach (ObjectParameter parameter in _parametersMethod)
                    builderDescription.AppendLine($"//\t\t{parameter.Name} - {parameter.Type} - {parameter.Description}");

            if (StringIsFunction)
            {
                builderDescription.AppendLine("//");
                builderDescription.AppendLine("// Возвращаемое значение:");
                builderDescription.AppendLine("//\t\t - ");
            }

            builderDescription.AppendLine("//");

            Description = builderDescription.ToString();

            if (IncludeStringMethod)
                Description += _stringMethod;
        }

        private void SetParametersMethod()
        {
            _parametersMethod.Clear();

            string parser = _stringMethod;

            parser = RemoveNonUsedStartText(parser);

            int countOpeningBracket = parser.Count(f => f == '(');
            if (countOpeningBracket == 1)
            {
                parser = parser.RemoveSpace();

                int positionOpeningBracket = parser.IndexOf("(");

                MethodName = parser.Substring(0, positionOpeningBracket);

                parser = parser.Substring(positionOpeningBracket + 1);

                int countClosingBracket = parser.Count(f => f == ')');

                if (countClosingBracket == 1)
                {
                    parser = RemoveNonUsedEndText(parser);

                    string[] parserParameters = parser.Split(',');
                    foreach (string itemParameter in parserParameters)
                        if (!string.IsNullOrWhiteSpace(itemParameter))
                        {
                            string paramenterName = itemParameter.RemoveStartText("знач").Trim();

                            int positionEqual = paramenterName.IndexOf('=');
                            if (positionEqual > 0)
                                paramenterName = paramenterName.Substring(0, positionEqual);

                            ObjectParameter objectParameter = new ObjectParameter(paramenterName);
                            objectParameter.SetTypeByName();

                            _parametersMethod.Add(objectParameter);
                        }
                }
            }
        }

        private static string RemoveNonUsedEndText(string parser)
        {
            parser = parser.Replace("\r", "");
            parser = parser.Replace("\n", "");
            parser = parser.Replace("\t", "");
            parser = parser.TrimEnd(';');
            parser = parser.TrimEnd();
            parser = parser.RemoveEndText("Экспорт");
            parser = parser.TrimEnd();
            parser = parser.TrimEnd(')');
            return parser;
        }

        private static string RemoveNonUsedStartText(string parser, bool removeTypeMethod = true)
        {
            parser = parser.RemoveStartText("&НаКлиентеНаСервереБезКонтекста");
            parser = parser.RemoveStartText("&НаСервереБезКонтекста");
            parser = parser.RemoveStartText("&НаСервере");
            parser = parser.RemoveStartText("&НаКлиенте");
            parser = parser.TrimStart();
            if (removeTypeMethod)
            {
                parser = parser.RemoveStartText("Процедура");
                parser = parser.RemoveStartText("Функция");
            }
            parser = parser.TrimStart();

            return parser;
        }
    }
}
