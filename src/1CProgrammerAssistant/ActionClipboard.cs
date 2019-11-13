using Hardcodet.Wpf.TaskbarNotification;
using System.Windows;

namespace _1CProgrammerAssistant
{
    internal delegate void ChangedSourceTextEvents(string text);
    internal delegate void ChangedResultTextEvents(string text);

    internal class ActionClipboard
    {
        internal event ChangedSourceTextEvents ChangedSourceTextEvents;
        internal event ChangedResultTextEvents ChangedResultTextEvents;

        private readonly AssistantObjects _assistantObjects;

        private bool _handleResult;
        private string _resultText;

        public ActionClipboard(ref AssistantObjects assistantObjects)
        {
            _assistantObjects = assistantObjects;

            ProcessTextInClipboardEvents.ProcessTextInClipboardEvent +=
            () =>
            {
                ProcessTextWithClipboard();
                if (_handleResult)
                    SetResultTextToClipboard();
            };
        }

        private string ResultText { get => _resultText; set { _resultText = value; ChangedResultTextEvents?.Invoke(value); } }

        internal void ProcessMakingMethod(bool showMessage = false)
        {
            Safe.SafeAction(() =>
            {
                if (Clipboard.ContainsText())
                {
                    string textInClipboard = GetTextFromClipboard();

                    ChangedSourceTextEvents?.Invoke(textInClipboard);

                    HandleText(textInClipboard);

                    if (showMessage)
                        if (!_handleResult)
                            ShowNotification("Не удалось распознать данные буфера обмена.", BalloonIcon.Error);
                }
            });
        }

        internal void ProcessTextWithClipboard(bool showMessage = false)
        {
            Safe.SafeAction(() =>
            {
                if (Clipboard.ContainsText())
                {
                    string textInClipboard = GetTextFromClipboard();

                    ChangedSourceTextEvents?.Invoke(textInClipboard);

                    HandleText(textInClipboard);

                    if (showMessage)
                        if (!_handleResult)
                            ShowNotification("Не удалось распознать данные буфера обмена.", BalloonIcon.Error);
                }
            });
        }

        internal static string GetTextFromClipboard()
        {
            return Clipboard.GetText();
        }

        internal void SetResultTextToClipboard(bool showMessage = false)
        {
            SetTextToClipboard(ResultText, showMessage, "Результат помещен в буфер обмена.");
        }

        internal void SetTextToClipboard(string text, bool showMessage = false, string textNotification = "")
        {
            Safe.SafeAction(() =>
            {
                if (!string.IsNullOrEmpty(text))
                {
                    Clipboard.SetText(text);

                    if (showMessage && !string.IsNullOrWhiteSpace(textNotification))
                        ShowNotification(textNotification);
                }
            });
        }

        private void HandleText(string text)
        {
            _handleResult = false;

            Safe.SafeAction(() => { HandleTextDescriptionsTheMethods(text); });
            Safe.SafeAction(() => { HandleTextQueryParameters(text); });
            Safe.SafeAction(() => { HandleTextMakingCode(text); });
        }

        #region HandleText Methods

        private void HandleTextDescriptionsTheMethods(string text)
        {
            if (!_handleResult)
            {
                _assistantObjects.DescriptionsTheMethodsMain.StringMethod = text;
                if (_assistantObjects.DescriptionsTheMethodsMain.Making())
                {
                    ResultText = _assistantObjects.DescriptionsTheMethodsMain.Description;
                    ShowNotification($"Получено описание метода: {_assistantObjects.DescriptionsTheMethodsMain.MethodName}");
                    _handleResult = true;
                }
            }
        }

        private void HandleTextQueryParameters(string text)
        {
            if (!_handleResult)
            {
                _assistantObjects.QueryParametersMain.QueryText = text;
                if (_assistantObjects.QueryParametersMain.Making())
                {
                    ResultText = _assistantObjects.QueryParametersMain.QueryParameters;

                    string message = "Получены параметры запроса";
                    if (!string.IsNullOrEmpty(_assistantObjects.QueryParametersMain.NameVariableQueryObject))
                        message += $": {_assistantObjects.QueryParametersMain.NameVariableQueryObject.Trim()}";

                    ShowNotification(message);
                    _handleResult = true;
                }
            }
        }

        private void HandleTextMakingCode(string text)
        {
            if (!_handleResult)
            {
                _assistantObjects.MakingCodeMain.SourceText = text;
                if (_assistantObjects.MakingCodeMain.Making())
                {
                    ResultText = _assistantObjects.MakingCodeMain.ResultText;

                    ShowNotification("Обработан код");
                    _handleResult = true;
                }
            }
        }

        #endregion

        private void ShowNotification(string message, BalloonIcon icon = BalloonIcon.None)
        {
            AssistantTaskbarIcon.ShowNotification(message, icon);
        }

    }
}
