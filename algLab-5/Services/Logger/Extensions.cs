using System.Collections.Generic;
using System.Text;

namespace algLab_5.Services.Logger
{
    public static class Extensions
    {
        /// <summary> Количество отображаемых элементов с начала и конца массива в логах </summary>
        private static int _numberElementsBeginAndEnd = 5;

        /// <summary> Установить количество отображаемых элементов с начала и конца массива в логах </summary>
        /// <param name="num"> Количество элементов </param>
        public static void SetNumberElementsBeginAndEnd(int num) => _numberElementsBeginAndEnd = num;

        /// <summary> Получить массив в формате строки для логов </summary>
        /// <typeparam name="T"> Тип элементов коллекции </typeparam>
        /// <param name="collection"> Коллекция </param>
        public static StringBuilder GetArrayForLog<T>(this IList<T> collection)
        {
            var sbLog = new StringBuilder($"[{collection.Count}] = {{ ");
            if (collection.Count <= _numberElementsBeginAndEnd * 2)
            {
                for (var i = 0; i < collection.Count - 1; i++)
                {
                    sbLog.Append($"{collection[i]}, ");
                }

                if (collection.Count > 0) sbLog.Append($"{collection[^1]} ");
            }
            else
            {
                for (var i = 0; i < _numberElementsBeginAndEnd; i++)
                {
                    sbLog.Append($"{collection[i]}, ");
                }

                sbLog.Append("..., ");

                for (var i = collection.Count - _numberElementsBeginAndEnd; i < collection.Count - 1; i++)
                {
                    sbLog.Append($"{collection[i]}, ");
                }

                sbLog.Append($"{collection[^1]} ");
            }

            sbLog.Append('}');

            return sbLog;
        }
    }
}
