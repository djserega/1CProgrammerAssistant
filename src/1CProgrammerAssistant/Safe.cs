using System;
using System.Diagnostics;
using System.Text;
using System.Windows;

namespace _1CProgrammerAssistant
{
    internal static class SafeBase
    {
        internal static string GetMessageException(Exception ex)
        {
            return ex.Message
                + GetSeparatorInnerException(0) +
                ex.InnerException?.Message
                + GetSeparatorInnerException(1) +
                ex.InnerException?.InnerException?.Message
                + GetSeparatorInnerException(2) +
                ex.InnerException?.InnerException?.InnerException?.Message
                + GetSeparatorInnerException(3) +
                ex.InnerException?.InnerException?.InnerException?.InnerException?.Message
                + GetSeparatorInnerException(4) +
                ex.InnerException?.InnerException?.InnerException?.InnerException?.InnerException?.Message;
        }

        private static string GetSeparatorInnerException(int count)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();

            for (int i = 0; i < count; i++)
                sb.Append('-');

            sb.Append('>');
            sb.AppendLine();

            return sb.ToString();
        }
    }

    internal static class Safe
    {
        internal static void SafeAction(Action action, string textInfo = "")
        {
            if (string.IsNullOrEmpty(textInfo))
                textInfo = "Перехвачена ошибка выполнения.\nДетальную информацию можно найти в событиях Windows.";

            try
            {
                action();
            }
            catch (Exception ex)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry(
                        SafeBase.GetMessageException(ex),
                        EventLogEntryType.Warning);
                }
                MessageBox.Show(textInfo);
            }
        }
    }

    internal static class SafeResult<T>
    {
        internal static T SafeAction(Func<T> action)
        {
            try
            {
                return action.Invoke();
            }
            catch (Exception ex)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry(
                        SafeBase.GetMessageException(ex),
                        EventLogEntryType.Warning);
                }
                MessageBox.Show("Перехвачена ошибка выполнения.\nДетальную информацию можно найти в событиях Windows.");

                return default;
            }
        }

    }
}
