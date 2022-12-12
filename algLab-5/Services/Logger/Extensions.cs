using System.Collections.Generic;
using System.Linq;
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

        /// <summary> Получить максимальную ширину колонки </summary>
        /// <param name="lines"> Строки матрицы </param>
        public static int GetMaxColumnWidth(this IEnumerable<IEnumerable<string>> lines)
        {
            return (from line in lines from s in line select s.Length).Prepend(0).Max();
        }

        /// <summary> Дополнить строку до заданной ширины </summary>
        /// <param name="line"> Исходная строка </param>
        /// <param name="width"> Требуемая ширина </param>
        /// <param name="c"> Символ дополнения </param>
        public static string CompleteLineWidth(this string line, int width, char c = ' ')
        {
            return line.Length < width ? $"{line}{new string(c, width - line.Length)}" : line;
        }

        /// <summary> Получить матрицу инцидентности для логирования </summary>
        /// <param name="lines"> Список массивов элементов матрицы </param>
        public static StringBuilder GetIncidenceMatrixForLog(this IList<string[]> lines)
        {
            var maxColumnWidth = lines.GetMaxColumnWidth();

            var lineSep = new string('-', lines[0].Length * (1 + maxColumnWidth) + 1);
            var sb = new StringBuilder();
            sb.Append('\n');
            sb.Append(lineSep);
            sb.Append('\n');
            foreach (var line in lines)
            {
                if (line.Length > 0)
                {
                    sb.Append($"|{line[0].CompleteLineWidth(maxColumnWidth)}");

                    for (var i = 1; i < line.Length - 1; i++)
                    {
                        sb.Append($"|{line[i].CompleteLineWidth(maxColumnWidth)}");
                    }

                    sb.Append($"|{line[^1].CompleteLineWidth(maxColumnWidth)}|");
                }

                sb.Append('\n');
                sb.Append(lineSep);
                sb.Append('\n');
            }

            return sb;
        }

        /// <summary> Получить матрицу инцидентности для логирования </summary>
        /// <param name="lines"> Двумерный массив элементов матрицы </param>
        public static StringBuilder GetIncidenceMatrixForLog(this string[,] lines)
        {
            var list = new List<string[]>();
            for (var i = 0; i < lines.GetLength(0); i++)
            {
                var array = new string[lines.GetLength(1)];
                for (var j = 0; j < lines.GetLength(1); j++)
                {
                    array[j] = lines[i, j];
                }

                list.Add(array);
            }

            return list.GetIncidenceMatrixForLog();
        }
    }
}
