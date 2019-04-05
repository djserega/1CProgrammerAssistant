using _1CProgrammerAssistant.Additions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _1CProgrammerAssistant.DescriptionsTheMethods
{
    public static class AdditionsString
    {
        public static string NonUsedEnd(this string source)
        {
            source = source.Replace("\r", "");
            source = source.Replace("\n", "");
            source = source.Replace("\t", "");
            source = source.TrimEnd(';');
            source = source.TrimEnd();
            source = source.RemoveEndText("Экспорт");
            source = source.TrimEnd();
            source = source.TrimEnd(')');
            return source;
        }

        public static string NonUsedStart(this string source, bool removeTypeMethod = true)
        {
            source = source.TrimStart();
            source = source.RemoveStartText("&НаКлиентеНаСервереБезКонтекста");
            source = source.RemoveStartText("&НаСервереБезКонтекста");
            source = source.RemoveStartText("&НаСервере");
            source = source.RemoveStartText("&НаКлиенте");
            source = source.TrimStart();
            if (removeTypeMethod)
            {
                source = source.RemoveStartText("Процедура");
                source = source.RemoveStartText("Функция");
            }
            source = source.TrimStart();

            return source;
        }
    }
}
