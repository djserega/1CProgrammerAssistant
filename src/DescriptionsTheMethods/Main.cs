using _1CProgrammerAssistant.Additions;
using _1CProgrammerAssistant.DescriptionsTheMethods.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace _1CProgrammerAssistant.DescriptionsTheMethods
{
    public class Main : INotifyPropertyChanged
    {

        #region Private fields
        private string _stringMethodWithoutDirectiveCompilation;

        private readonly List<ObjectParameter> _parametersMethod = new List<ObjectParameter>();

        #endregion

        #region Public properties

        public string MethodName { get; private set; }
        public string StringMethod { get; set; }
        public string Description { get; set; } = string.Empty;
        public string TextError { get; private set; } = string.Empty;
        public bool IncludeStringMethod { get; set; } = true;

        #endregion

        #region Private properties

        private bool StringIsFunction { get => _stringMethodWithoutDirectiveCompilation.TrimStart().StartsWith("функция", true, null); }
        private bool StringIsProcedure { get => _stringMethodWithoutDirectiveCompilation.TrimStart().StartsWith("процедура", true, null); }

        #endregion

        #region Notify property changed

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "") 
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

        public bool Making()
        {
            TextError = string.Empty;
            Description = string.Empty;

            SetDescription();

            return string.IsNullOrWhiteSpace(TextError)
                && !string.IsNullOrWhiteSpace(Description);
        }

        #region Private methods

        private void SetDescription()
        {
            if (string.IsNullOrEmpty(StringMethod))
                return;

            _stringMethodWithoutDirectiveCompilation = StringMethod.NonUsedStart(false);

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
                Description += StringMethod;
        }

        private void SetParametersMethod()
        {
            _parametersMethod.Clear();

            string parser = StringMethod;

            parser = parser.NonUsedStart();

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
                    parser = parser.NonUsedEnd();

                    string[] parserParameters = parser.Split(',');
                    foreach (string itemParameter in parserParameters)
                        if (!string.IsNullOrWhiteSpace(itemParameter))
                        {
                            string paramenterName = itemParameter.RemoveStartText("знач").Trim();

                            int positionEqual = paramenterName.IndexOf('=');
                            if (positionEqual > 0)
                                paramenterName = paramenterName.Left(positionEqual);

                            ObjectParameter objectParameter = new ObjectParameter(paramenterName);
                            objectParameter.SetTypeByName();

                            _parametersMethod.Add(objectParameter);
                        }
                }
            }
        }

        #endregion
    }
}
