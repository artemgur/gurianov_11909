using System.Collections.Generic;

namespace Benchmarking
{
    public static class BoyerMooreAlgorithm
    {
        ///Предварительные вычисления для символов
        private static Dictionary<char, int> BadCharacterPreprocessing(string pattern)
        {
            var result = new Dictionary<char, int>();//Ключ - символ, значение - его первое с конца вхождение
            for (var i = pattern.Length - 2; i >= 0; i--) //-2, поскольку последний символ нам не нужно рассматривать
                if (!result.ContainsKey(pattern[i]))
                    result.Add(pattern[i], i);
            return result;
        }
        
        ///Предварительные вычисления для суффиксов
        private static int[] GoodSuffixPreprocessing(string pattern)//TODO
        {
            var result = new int[pattern.Length + 1];//Индекс - длина шаблона, значение - величина сдвига
            result[0] = 1;
            for (var i = 1; i <= pattern.Length; i++) //i - длина суффикса
            {
                for (var j = 1; j < pattern.Length; j++) //j - величина сдвига
                {
                    var suffixMatches = true;
                    for (var k = pattern.Length - 1; k >= pattern.Length - i; k--) //Сверяем суффикс с шаблоном
                        if (k - j >= 0 && pattern[k] != pattern[k - j])
                        {
                            suffixMatches = false;
                            break;
                        }
                    var a = pattern.Length - i - 1 - j;
                    if (a >= 0 && pattern[pattern.Length - i - 1] == pattern[a]) //Шаблону должен предшествовать другой символ
                        suffixMatches = false;
                    if (suffixMatches)
                        result[i] = j;
                }
                if (result[i] == 0)
                    result[i] = pattern.Length;
            }
            return result;
        }
        
        public static int Run(this string source, string pattern)
        {
            //Предварительные вычисления
            var badCharacterData = BadCharacterPreprocessing(pattern);
            var goodSuffixData = GoodSuffixPreprocessing(pattern);
            //Работа алгоритма
            for (var i = pattern.Length - 1; i < source.Length; i++) //i - положение конца шаблона
            {
                var patternMatches = true;
                for (var j = 0; j < pattern.Length; j++) //Сверяем шаблон со строкой
                    if (source[i - j] != pattern[pattern.Length - j - 1]) //Найдено несовпадение
                    {
                        patternMatches = false;
                        //Bad character rule
                        var ch = pattern[pattern.Length - j - 1];
                        var shiftTo = badCharacterData.ContainsKey(ch) ? badCharacterData[ch] : 1;
                        var shift = pattern.Length - j - 1 - shiftTo;
                        //Good suffix rule
                        if (shift <= 0)
                            shift = goodSuffixData[j];
                        i += shift - 1;
                        break;
                    }
                if (patternMatches) //Шаблон совпал
                    return i - pattern.Length + 1;
            }
            return -1; //Шаблона нет в строке
        }
    }
}