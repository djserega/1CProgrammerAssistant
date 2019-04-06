namespace _1CProgrammerAssistant.DescriptionsTheMethods
{
    public class Main
    {

        #region Private fields

        private string _stringMethodWithoutDirectiveCompilation;

        #endregion

        #region Public properties

        public string MethodName { get; private set; } = string.Empty;
        public string StringMethod { get; set; }
        public string Description { get; set; } = string.Empty;
        public string TextError { get; private set; } = string.Empty;
        public bool IncludeStringMethod { get; set; } = true;

        #endregion

        #region Private properties

        private bool StringIsFunction { get => _stringMethodWithoutDirectiveCompilation.TrimStart().StartsWith("функция", true, null); }
        private bool StringIsProcedure { get => _stringMethodWithoutDirectiveCompilation.TrimStart().StartsWith("процедура", true, null); }

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
            MakingDescription makingDescription = new MakingDescription(StringMethod);
            makingDescription.Making(StringIsFunction, IncludeStringMethod);

            MethodName = makingDescription.MethodName;
            Description = makingDescription.Description;
        }

        #endregion
    }
}
