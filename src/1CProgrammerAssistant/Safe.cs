using System;
using System.Diagnostics;
using System.Windows;

namespace _1CProgrammerAssistant
{
    internal static class Safe
    {
        internal static void SafeAction(Action action)
        {
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
                        ex.Message
                        + "\n" + "\n" +
                        ex.InnerException?.Message
                        + "\n" + "\n" +
                        ex.InnerException?.InnerException?.Message
                        + "\n" + "\n" +
                        ex.InnerException?.InnerException?.InnerException?.Message,
                        EventLogEntryType.Warning);
                }
                MessageBox.Show("Перехвачена ошибка выполнения.\nДетальную информацию можно найти в событиях Windows.");
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
                        ex.Message
                        + "\n" + "\n" +
                        ex.InnerException?.Message
                        + "\n" + "\n" +
                        ex.InnerException?.InnerException?.Message
                        + "\n" + "\n" +
                        ex.InnerException?.InnerException?.InnerException?.Message,
                        EventLogEntryType.Warning);
                }
                MessageBox.Show("Перехвачена ошибка выполнения.\nДетальную информацию можно найти в событиях Windows.");

                return default;
            }
        }
    }
}
