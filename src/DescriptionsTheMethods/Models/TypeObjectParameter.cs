using _1CProgrammerAssistant.Additions;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace _1CProgrammerAssistant.DescriptionsTheMethods.Models
{
    internal class TypeObjectParameter
    {
        private static Dictionary<string, List<string>> _usersTypeParameters = new Dictionary<string, List<string>>();

        internal TypeObjectParameter()
        {
            InitializeUsersTypeParameters();
        }

        internal virtual string Name { get; set; }
        internal virtual string Type { get; set; }

        #region Type object

        internal bool IsArray { get => CheckStartsWith("мс", "Массив"); }
        internal bool IsMap { get => CheckStartsWith("со", "Соответствие"); }
        internal bool IsStructure { get => CheckStartsWith("ст", "Структура"); }
        internal bool IsValueList { get => CheckStartsWith("сз", "СписокЗначений"); }
        internal bool IsValueTable { get => CheckStartsWith("тз", "ТаблицаЗначений"); }
        internal bool IsValueTree { get => CheckStartsWith("дз", "ДеревоЗначений"); }

        internal bool IsFixedArray { get => CheckStartsWith("фмс", "ФиксированныйМассив"); }
        internal bool IsFixedMap { get => CheckStartsWith("фсо", "ФиксированноеСоответствие"); }
        internal bool IsFixedStructure { get => CheckStartsWith("фст", "ФиксированнаяСтруктура"); }

        internal bool IsReference { get => CheckStartsWith("спр", "СправочникСсылка"); }
        internal bool IsDocument { get => CheckStartsWith("док", "ДокументСсылка"); }

        internal bool IsDynamicList { get => CheckStartsWith("дс", "ДинамическийСписок"); }

        internal bool IsString
        {
            get => CheckStartsWith("ИмяПараметра", "Строка")
                || CheckStartsWith("Штрихкод", "Строка");
        }

        #endregion

        private bool CheckStartsWith(string text, string typeName)
        {
            bool startWith = Name.StartsWith(text, true, null);

            if (!startWith)
            {
                if (_usersTypeParameters.ContainsKey(typeName.ToLower()))
                {
                    foreach (string prefixText in _usersTypeParameters[typeName.ToLower()])
                    {
                        startWith = Name.StartsWith(prefixText, true, null);
                        if (startWith)
                            break;
                    }
                }
            }

            return startWith;
        }

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

        private void InitializeUsersTypeParameters()
        {
            if (_usersTypeParameters.Count > 0)
                return;

            var createrType = new ReaderFileTypeParameters("UsersTypeParameters.cfg");

            string textUsersFile = string.Empty;
            if (createrType.Exist())
                textUsersFile = createrType.Read();
            else
            {
                createrType.Create(GetDefaultTextUsersTypeParameters());
                textUsersFile = createrType.Read();
            }

            ReadTextUserFile(textUsersFile);
        }

        private string GetDefaultTextUsersTypeParameters()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("### Имя типа :  Префикс параметра");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Массив : мс;");
            stringBuilder.AppendLine("Соответствие : со;");
            stringBuilder.AppendLine("Структура : ст;");
            stringBuilder.AppendLine("СписокЗначений : сз;");
            stringBuilder.AppendLine("ТаблицаЗначений : тз;");
            stringBuilder.AppendLine("ДеревоЗначений : дз;");
            stringBuilder.AppendLine("ФиксированныйМассив : фмс;");
            stringBuilder.AppendLine("ФиксированноеСоответствие : фсо;");
            stringBuilder.AppendLine("ФиксированнаяСтруктура : фст;");
            stringBuilder.AppendLine("СправочникСсылка : спр;");
            stringBuilder.AppendLine("ДокументСсылка : док");
            stringBuilder.AppendLine("ДинамическийСписок : дс");
            stringBuilder.AppendLine("Строка: ИмяПараметра; Штрихкод;");

            return stringBuilder.ToString();
        }

        private void ReadTextUserFile(string text)
        {
            string[] rows = text.Replace("\r", "").Split('\n');
            foreach (string typeObject in rows)
            {
                if (!typeObject.LeftEquals("###"))
                {
                    string[] partTypeObject = typeObject.Split(':');

                    if (partTypeObject.Length == 2)
                    {
                        string keyObject = partTypeObject[0];

                        List<string> listPrefix = new List<string>();
                        string[] valuesObject = partTypeObject[1].Split(';');
                        foreach (string value in valuesObject)
                        {
                            if (!string.IsNullOrWhiteSpace(value))
                                listPrefix.Add(value.Trim());
                        }

                        _usersTypeParameters.Add(keyObject.Trim().ToLower(), listPrefix);
                    }
                }
            }

        }

    }
}
