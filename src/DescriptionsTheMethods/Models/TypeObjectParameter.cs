namespace _1CProgrammerAssistant.DescriptionsTheMethods.Models
{
    internal class TypeObjectParameter
    {
        internal virtual string Name { get; set; }
        internal virtual string Type { get; set; }

        #region Type object

        internal bool IsArray { get => NameStartsWith("мс"); }
        internal bool IsMap { get => NameStartsWith("со"); }
        internal bool IsStructure { get => NameStartsWith("ст"); }
        internal bool IsValueList { get => NameStartsWith("сз"); }
        internal bool IsValueTable { get => NameStartsWith("тз"); }
        internal bool IsValueTree { get => NameStartsWith("дз"); }

        internal bool IsFixedArray { get => NameStartsWith("фмс"); }
        internal bool IsFixedMap { get => NameStartsWith("фсо"); }
        internal bool IsFixedStructure { get => NameStartsWith("фст"); }

        internal bool IsReference { get => NameStartsWith("спр"); }
        internal bool IsDocument { get => NameStartsWith("док"); }

        internal bool IsDynamicList { get => NameStartsWith("дс"); }

        internal bool IsString
        {
            get
            {
                switch (Name)
                {
                    case "ИмяПараметра":
                    case "Штрихкод":
                        return true;
                    default:
                        return false;
                }
            }
        }

        #endregion

        private bool NameStartsWith(string text) 
            => Name.StartsWith(text, true, null);

        internal void SetTypeByName()
        {
            if (IsArray)
                Type = "Массив";
            else if (IsMap)
                Type = "Соответствие";
            else if (IsStructure)
                Type = "Структура";
            else if (IsValueList)
                Type = "СписокЗначений";
            else if (IsValueTable)
                Type = "ТаблицаЗначений";
            else if (IsValueTree)
                Type = "ДеревоЗначений";
            else if (IsFixedArray)
                Type = "ФиксированныйМассив";
            else if (IsFixedMap)
                Type = "ФиксированноеСоответствие";
            else if (IsFixedStructure)
                Type = "ФиксированнаяСтруктура";

            else if (IsReference)
                Type = "СправочникСсылка";
            else if (IsDocument)
                Type = "ДокументСсылка";

            else if (IsDynamicList)
                Type = "ДинамическийСписок";

            else if (IsString)
                Type = "Строка";
        }

    }
}
