using _1CProgrammerAssistant.Additions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace _1CProgrammerAssistant.QueryParameters
{
    public class Main
    {
        #region Private fields

        private const string _partRegexPattern = "[Нн][Оо][Вв][Ыы][Йй] [Зз][Аа][Пп][Рр][Оо][Сс]";

        #endregion

        #region Public properties

        public string NameVariableQueryObject { get; private set; }
        public string QueryText { get; set; }
        public List<string> ParametersName { get; } = new List<string>();

        public string QueryParameters { get; private set; }
        public string TextError { get; private set; }

        #endregion

        public bool Making()
        {
            SetQueryParameters();

            return string.IsNullOrEmpty(TextError)
                && !string.IsNullOrEmpty(QueryParameters);
        }

        #region Private methods

        private void SetQueryParameters()
        {
            if (string.IsNullOrEmpty(QueryText))
                return;

            GetVariableName();
            ParseTextQuery();
            SetTextQueryParameters();
        }

        private void GetVariableName()
        {
            Regex regex = new Regex($@"\D+({_partRegexPattern})", RegexOptions.Multiline);
            if (regex.IsMatch(QueryText))
            {
                Match match = regex.Match(QueryText);

                string processingText = match.Value.RemoveEndText("Новый Запрос");
                processingText = processingText.TrimEnd();
                processingText = processingText.RemoveEndText("=");
                processingText = processingText.TrimEnd();

                NameVariableQueryObject = processingText;
            }
        }

        private void ParseTextQuery()
        {
            ParametersName.Clear();

            Regex regex = new Regex(@"""+[^""]+"""); // "+[^"]+"

            if (QueryText.StartsWith("Выбрать", true, null))
                QueryText = $"\"{QueryText}\"";

            if (regex.IsMatch(QueryText))
            {
                string textQuery = string.Empty;
                foreach (Match match in regex.Matches(QueryText))
                {
                    textQuery += match.Value;
                }

                regex = new Regex("&+[A-zА-я0-9]*");

                if (regex.IsMatch(textQuery))
                {
                    foreach (Match item in regex.Matches(textQuery))
                    {
                        string nameParameter = item.Value.Substring(1);
                        if (!ParametersName.Contains(nameParameter))
                            ParametersName.Add(nameParameter);
                    }
                }
            }
        }

        private void SetTextQueryParameters()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (string parameterName in ParametersName)
                stringBuilder.AppendLine($"{NameVariableQueryObject}.УстановитьПараметр(\"{parameterName}\", );");

            QueryParameters = stringBuilder.ToString();
        }

        #endregion
    }
}
