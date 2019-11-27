using _1CProgrammerAssistant.Additions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MakingCode
{
    internal class MakingTextMakingMethod
    {

        private readonly StringBuilder _textBuilder = new StringBuilder();

        public MakingTextMakingMethod(string source)
        {
            Source = source;
        }

        internal string Source { get; }
        internal int LengthSource { get => Source.Length; }

        internal string MakeText()
        {
            _textBuilder.Clear();

            _textBuilder.AppendLine("");

            int positionEquals = Source.IndexOf('=');
            if (positionEquals == -1)
                _textBuilder.Append("Процедура ");
            else
                _textBuilder.Append("Функция   ");

            int positionBracket = Source.IndexOf('(');
            string nameMethod = Source.Left(positionBracket);
            _textBuilder.Append(nameMethod.TrimStart());

            _textBuilder.Append("(");

            int countComma = Source.Count(f => f ==',');

            for (int i = 0; i <= countComma; i++)
            {
                if (i != 0)
                    _textBuilder.Append(", ");

                _textBuilder.Append("Параметр" + i);
            }

            _textBuilder.Append(") ");

            if (Source.ToLower().Contains("экспорт"))
                _textBuilder.Append("Экспорт");

            _textBuilder.AppendLine(" ");
            _textBuilder.AppendLine("\t");
            _textBuilder.AppendLine("\t");
            _textBuilder.AppendLine("\t");

            if (positionEquals == -1)
                _textBuilder.Append("КонецПроцедуры");
            else
                _textBuilder.Append("КонецФункции");

            _textBuilder.AppendLine();

            return _textBuilder.ToString();
        }
    }
}
