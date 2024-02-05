using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Common.Utilities
{
    public static class StringHelper
    {
        public static bool HasValue(this string value)
        {
            return !(string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(value));
        }

        public static string GetDigits(this string input)
        {
            if (input.HasValue())
            {
                return Regex.Match(input, @"[\d-]").Value;
            }

            return null;
        }

        public static string RemoveDigits(this string input)
        {
            if (input.HasValue())
            {
                return Regex.Replace(input, @"[\d-]", string.Empty);
            }

            return null;
        }

        public static bool IsInFormat(this string input, string format)
        {
            if (input.HasValue() && format.HasValue())
            {
                if (input.Length == format.Length)
                {
                    for (int i = 0; i < input.Length; i++)
                    {
                        if ((input[i].Equals('x') && format[i].Equals('-')) || (input[i].Equals('-') && format[i].Equals('x')))
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        public static string ShowInFormat(this string input, string format)
        {
            try
            {
                if (input.HasValue() && format.HasValue())
                {
                    format = format.Replace('x', '#');

                    return Convert.ToInt64(input).ToString(format);
                }

                return input;
            }
            catch
            {
                return input;
            }

        }

        public static string RemoveMasks(this string input)
        {
            if (input.HasValue())
            {
                if (IsEmailValid(input))
                    return input;

                return input.Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            }

            return "";
        }

        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }

        public static string NullIfEmpty(this string str)
        {
            return str == "" ? null : str;
        }

        public static string EmptyIfNull(this string str)
        {
            return str ?? "";
        }

        public static string Summary(this string str, int lenght)
        {
            if (str.Length < lenght)
            {
                return str;
            }
            else
            {
                return str.Substring(0, lenght - 4) + " ...";
            }
        }

        public static string Summary(this string str, int lenght, string searchText)
        {
            var j = 0;

            try
            {
                str = str.Replace("●", " ");
                for (var i = 1; i <= str.Length; i++)
                {
                    if (i > str.Length)
                    {
                        break;
                    }
                    j += 1;
                    if (str.Substring(j - 1, 1) == "<")
                    {
                        string Temp = str.Substring(j - 1, 1);
                        j += 1;
                        while ((str.Substring(j - 1, 1) != ">"))
                        {
                            Temp += str.Substring(j - 1, 1);
                            j += 1;
                        }
                        Temp += str.Substring(j - 1, 1);
                        str = str.Replace(Temp, "");
                        j = 0;
                        i = 0;
                    }
                }
            }
            catch { }

            if (searchText.HasValue())
            {
                str = str.Replace(searchText, "<b><span style='color:red'>" + searchText + "</span></b>");
            }
            if (str.Length < lenght)
            {
                return str;
            }
            else
            {
                return str.Substring(0, lenght);
            }
        }

        public static bool In(this string str, params string[] stringValues)
        {
            foreach (var item in stringValues)
            {
                if (string.Compare(str, item) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        public static string Right(this string str, int length)
        {
            return str != null && str.Length > length ? str.Substring(str.Length - length) : str;
        }

        public static string Left(this string str, int length)
        {
            return str != null && str.Length > length ? str.Substring(0, length) : str;
        }

        public static string RemoveLeft(this string str, int length)
        {
            return str.Remove(0, length);
        }

        public static string RemoveRight(this string str, int length)
        {
            return str.Remove(str.Length - length);
        }

        public static string CleanString(this string str)
        {
            return str.Trim().RemoveWhiteSpaces().NullIfEmpty();
        }

        public static string RemoveLetters(this string text)
        {
            if (text.HasValue())
            {
                return Regex.Replace(text, "[^0-9.]", "");
            }

            return null;
        }

        public static string RemoveWhiteSpaces(this string input)
        {
            while (input.Contains("  "))
            {
                input = input.Replace("  ", " ");
            }

            return input;
        }

        public static string Take(this string input, int count)
        {
            if (input.Length > count)
            {
                return input.Substring(0, count);
            }

            return input;
        }

        public static string ShowDurationinHourFormat(int duration)
        {
            string returnString = "";
            try
            {
                int hours = 0;
                hours = duration / 60;
                int minutes = 0;
                minutes = duration % 60;

                if (hours > 0)
                    returnString += $"{hours}h ";

                if (minutes > 0)
                    returnString += $"{minutes}min";

            }
            catch
            {

            }
            return returnString;
        }

        public static string GenerateRandomPassword()
        {
            const string alphanumericCharacters =
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                "abcdefghijklmnopqrstuvwxyz" +
                "0123456789";

            var output = GetRandomString(6, alphanumericCharacters);
            if (!output.Any(char.IsDigit) || !output.Any(char.IsUpper) || !output.Any(char.IsLower)) //to make sure it has digits and upper cases and lower cases
                return GenerateRandomPassword();

            return output;
        }

        public static string GenerateVerificationCode()
        {
            const string alphanumericCharacters =
                "123456789";
            return GetRandomString(4, alphanumericCharacters);
        }

        private static string GetRandomString(int length, IEnumerable<char> characterSet)
        {
            if (length < 0)
                throw new ArgumentException("length must not be negative", "length");
            if (length > int.MaxValue / 8) // 250 million chars ought to be enough for anybody
                throw new ArgumentException("length is too big", "length");
            if (characterSet == null)
                throw new ArgumentNullException("characterSet");
            var characterArray = characterSet.Distinct().ToArray();
            if (characterArray.Length == 0)
                throw new ArgumentException("characterSet must not be empty", "characterSet");

            var bytes = new byte[length * 8];
            new RNGCryptoServiceProvider().GetBytes(bytes);
            var result = new char[length];
            for (int i = 0; i < length; i++)
            {
                ulong value = BitConverter.ToUInt64(bytes, i * 8);
                result[i] = characterArray[value % (uint)characterArray.Length];
            }

            var output = new string(result);

            return output;
        }

        public static bool IsEmailValid(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsPhoneNumberValid(string phoneNumber, string format)
        {
            var pat = $"^\\d{{{format.Length}}}$";
            Regex pattern = new Regex($@"{pat}");
            return pattern.IsMatch(phoneNumber);
        }

        public static string MaskValue(string value)
        {
            if (value.Length > 4)
            {
                var maskedPart = value.Length > 9 ? new string('*', 5) : new string('*', 3);

                var data = value.Substring(0, 3) + maskedPart + value.Substring(value.Length - 1, 1);
                return data;
            }

            return value;
        }

        public static string GenerateRandomEmail()
        {
            return $"{DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss-ffff")}@m";
        }
    }
}