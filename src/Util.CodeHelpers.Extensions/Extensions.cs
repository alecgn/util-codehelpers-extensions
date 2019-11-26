using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace Util.CodeHelpers.Extensions
{
    public enum PathType { File, Directory, Unknown };
    public enum SizeUnits { Byte, KB, MB, GB, TB, PB, EB, ZB, YB }

    public static class StringExtensions
    {
        public static bool IsNullEmptyOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static T Convert<T>(this string str)
        {
            try
            {
                return (T)System.Convert.ChangeType(str, typeof(T));
            }
            catch
            {
                return default;
            }
        }

        public static bool IsBase64(this string str)
        {
            if (str.IsNullEmptyOrWhiteSpace())
                return false;

            var regexBase64 = new Regex(@"^[a-zA-Z0-9\+\/]*={0,3}$");

            return ((str.Length % 4) == 0 && regexBase64.IsMatch(str));
        }

        public static string EncodeToBase64(this string str)
        {
            if (str.IsNullEmptyOrWhiteSpace())
                return null;

            return System.Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
        }

        public static string DecodeFromBase64(this string str64)
        {
            if (str64.IsNullEmptyOrWhiteSpace())
                return null;

            return Encoding.UTF8.GetString(System.Convert.FromBase64String(str64));
        }

        /// <summary>
        /// Use this method for local file paths checks.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool FileExists(this string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        /// Use this method for network/remote file paths checks; also works on local file paths.
        /// </summary>
        /// <param name="networkFilePath"></param>
        /// <returns></returns>
        public static bool NetworkFileExists(this string networkFilePath)
        {
            return new FileInfo(networkFilePath).Exists;
        }

        /// <summary>
        /// Use this method for local directory paths checks.
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static bool DirectoryExists(this string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        /// <summary>
        /// Use this method for network/remote directory paths checks; also works on local directory paths.
        /// </summary>
        /// <param name="networkDirectoryPath"></param>
        /// <returns></returns>
        public static bool NetworkDirectoryExists(this string networkDirectoryPath)
        {
            return new DirectoryInfo(networkDirectoryPath).Exists;
        }

        public static PathType GetPathType(this string path)
        {
            return (Path.HasExtension(path) ? PathType.File : PathType.Directory);
        }

        public static bool IsInArray(this string str, params string[] strArray)
        {
            return strArray.Contains(str);
        }

        public static bool IsInt(this string str)
        {
            return int.TryParse(str, out _);
        }

        public static bool IsBool(this string str)
        {
            return bool.TryParse(str, out _);
        }

        public static bool IsDateTime(this string str)
        {
            return DateTime.TryParse(str, out _);
        }

        public static string HandleSqlInjection(this string str)
        {
            var arrDangerousContent = new string[] { "select", "insert", "update", "delete", "drop", "--", ";", "xp_", "sp_" };

            foreach (var dangerousContent in arrDangerousContent)
            {
                if (str.ToLower().Contains(dangerousContent))
                    str = str.Replace(dangerousContent, "");
            };

            return str;
        }

        public static string RemoveHTMLTags(this string str)
        {
            return str.Replace("<", "").Replace(">", "");
        }

        public static string HandleXss(this string str)
        {
            return str.HTMLEncode();
        }

        public static string UrlEncode(this string str)
        {
            return WebUtility.UrlEncode(str);
        }

        public static string UrlDecode(this string str)
        {
            return WebUtility.UrlDecode(str);
        }

        public static string HTMLEncode(this string str)
        {
            return WebUtility.HtmlEncode(str);
        }

        public static string HTMLDecode(this string str)
        {
            return WebUtility.HtmlDecode(str);
        }

        public static string AlignCenterToFixLength(this string str, int length)
        {
            return string.Format("{0,-" + length + "}", string.Format("{0," + ((length + str.Length) / 2).ToString() + "}", str));
        }

        public static string Empty(this string _)
        {
            return "";
        }

        public static string Null(this string _)
        {
            return null;
        }

        public static Color FromColorString(this string colorString)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Color));
            Color color = (Color)converter.ConvertFromString(colorString);

            return color;
        }

        public static Font FromFontString(this string fontString)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Font));
            Font fonte = (Font)converter.ConvertFromString(fontString);

            return fonte;
        }

        public static byte[] FromHexString(this string hexString)
        {
            if (string.IsNullOrWhiteSpace(hexString))
                return null;

            if (hexString.Length % 2 != 0)
                throw new ArgumentException("Incorret hexadecimal string.", nameof(hexString));

            var byteArray = new byte[hexString.Length / 2];
            var i = 0;

            foreach (var hexVal in ChunkHexString(hexString))
            {
                byteArray[i] = System.Convert.ToByte(hexVal, 16);
                i++;
            }

            return byteArray;
        }

        private static IEnumerable<string> ChunkHexString(string hexString)
        {
            for (int i = 0; i < hexString.Length; i += 2)
                yield return hexString.Substring(i, 2);
        }
    }

    public static class LongExtensions
    {
        public static string ToSizeUnitString(this long value, SizeUnits unit)
        {
            return (value / Math.Pow(1024, (int)unit)).ToString("0.00");
        }
    }

    public static class BigIntegerExtensions
    {
        public static string ToSizeUnitString(this BigInteger value, SizeUnits unit)
        {
            return (value / BigInteger.Pow(1024, (int)unit)).ToString("0.00");
        }
    }

    public static class ObjectExtensions
    {
        public static T Convert<T>(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return default;
            else
            {
                try
                {
                    return (T)System.Convert.ChangeType(obj, typeof(T));
                }
                catch
                {
                    return default;
                }
            }
        }

        public static bool IsNull(this object obj)
        {
            return (obj == null);
        }

        public static bool IsNotNull(this object obj)
        {
            return (obj != null);
        }

        public static bool AllAreNull(params object[] objArray)
        {
            var allAreNull = true;

            foreach (var obj in objArray)
            {
                if (obj.IsNotNull())
                {
                    allAreNull = false;
                    break;
                }
            }

            return allAreNull;
        }

        public static bool AnyIsNull(params object[] objArray)
        {
            var anyIsNull = false;

            foreach (var obj in objArray)
            {
                if (obj.IsNull())
                {
                    anyIsNull = true;
                    break;
                }
            }

            return anyIsNull;
        }

        public static bool IsInArray(this object obj, params object[] objArray)
        {
            return objArray.Contains(obj);
        }
    }

    public static class GenericsExtensions
    {
        public static TOutput Convert<TInput, TOutput>(this TInput input)
        {
            if (input == null)
                return default;
            else
            {
                try
                {
                    return (TOutput)System.Convert.ChangeType(input, typeof(TOutput));
                }
                catch
                {
                    return default;
                }
            }
        }

        public static bool IsNullableType<T>(T t)
        {
            var type = t.GetType();
            //var type = typeof(T);

            return (Nullable.GetUnderlyingType(type) != null);
        }
    }

    public static class GenericListExtensions
    {
        /// <summary>
        /// Randomize a generic list, changing its items position. Author: Alessandro Cagliostro
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputList"></param>
        /// <param name="exceptItem">The item wich its position will not be affected by the Randomize function. Pass null or invalid data for commom operation.</param>
        public static void RandomizeList<T>(this List<T> inputList, T exceptItem)
        {
            Random random;

            try
            {
                if (inputList != null && inputList.Count > 1)
                {
                    random = new Random();
                    int exceptItemIndex = inputList.IndexOf(exceptItem);

                    for (int i = inputList.Count - 1; i >= 0; i--)
                    {
                        T tmp = inputList[i];

                        if (tmp.Equals(exceptItem))
                            continue;

                        int randomIndex = 0;
                        bool exitLoop = false;

                        while (!exitLoop)
                        {
                            randomIndex = random.Next(i + 1);

                            if (!randomIndex.Equals(exceptItemIndex))
                                exitLoop = true;
                        }

                        inputList[i] = inputList[randomIndex];
                        inputList[randomIndex] = tmp;
                    }
                }
            }
            finally
            {
                random = null;
            }
        }

        public static List<TOutput> ConvertList<TInput, TOutput>(this List<TInput> inputList)
        {
            var outputList = new List<TOutput>();

            foreach (var item in inputList)
            {
                outputList.Add(item.Convert<TInput, TOutput>());
            }

            return outputList;
        }
    }

    public static class ArrayExtensions
    {
        public static void RandomizeArray<T>(this T[] inputArray)
        {
            if (inputArray != null && inputArray.Length > 1)
            {
                Random rand = new Random();
                int n = inputArray.Length;

                for (int i = 0; i < n; i++)
                {
                    int r = i + rand.Next(n - i);
                    T t = inputArray[r];
                    inputArray[r] = inputArray[i];
                    inputArray[i] = t;
                }
            }
        }
    }

    public static class ByteArrayExtensions
    {
        public static string ToHexString(this byte[] byteArray)
        {
            return string.Concat(byteArray.Select(b => b.ToString("X2")));
        }

        public static string ToBase64String(this byte[] byteArray)
        {
            return Convert.ToBase64String(byteArray);
        }
    }

    public static class ColorExtensions
    {
        public static string ToColorString(this Color color)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Color));
            string colorString = converter.ConvertToString(color);

            return colorString;
        }
    }

    public static class FontExtensions
    {
        public static string ToFontString(this Font font)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Font));
            string fontString = converter.ConvertToString(font);

            return fontString;
        }
    }

    public static class DateTimeExtensions
    {
        // timeZoneId examples -> "E. South America Standard Time", "US Eastern Standard Time", etc.
        public static DateTime ConvertToTimeZone(this DateTime datetime, string timeZoneId)
        {
            return TimeZoneInfo.ConvertTime(datetime, TimeZoneInfo.FindSystemTimeZoneById(timeZoneId));
        }
    }

    public static class DictionaryExtensions
    {
        public static bool HasSameKeysAndValuesThan<TKey, TValue>(this Dictionary<TKey, TValue> mainDictionary, Dictionary<TKey, TValue> secondaryDictionary)
        {
            if (mainDictionary.IsNull() || secondaryDictionary.IsNull() || (mainDictionary.Count <= 0 && secondaryDictionary.Count <= 0))
                throw new ArgumentNullException($"Both paramaters ({nameof(mainDictionary)},{nameof(secondaryDictionary)}) must be not null and contain more than 1 element.");

            bool result;

            if (mainDictionary.Count != secondaryDictionary.Count)
                result = false;
            else
                result = (mainDictionary.Except(secondaryDictionary).Count() == 0);

            return result;
        }
    }
}
