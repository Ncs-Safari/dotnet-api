using System;
using System.Data;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Extensions
{
    public static class StringExtension
    {
        #region EncryptDecrypt
        private readonly static string SecretKey = "sawerigading2023";
        public static string DecryptString(this string cipherText)
        {
            var fullCipher = Convert.FromBase64String(cipherText.Replace("*#*", "/"));

            var iv = new byte[16];
            var cipher = new byte[16];
            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var key = Encoding.UTF8.GetBytes(SecretKey);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }
        public static string EncryptString(this string text)
        {
            var key = Encoding.UTF8.GetBytes(SecretKey);

            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);
                        string tempEncrypt = Convert.ToBase64String(result).Replace("/", "*#*");
                        string tempDecrypt = DecryptString(tempEncrypt.Replace("*#*", "/"));
                        if (tempDecrypt == text)
                            return tempEncrypt;
                        return string.Empty;
                    }
                }
            }
        }
        #endregion        
        #region ToSlug
        public static string ToSlug(this string phrase)
        {
            string str = phrase.RemoveAccent().ToLower();

            str = Regex.Replace(str, @"[^a-z0-9\s-]", ""); // invalid chars          
            str = Regex.Replace(str, @"\s+", " ").Trim(); // convert multiple spaces into one space  
            str = str.Substring(0, str.Length <= 255 ? str.Length : 255).Trim(); // cut and trim it  
            str = Regex.Replace(str, @"\s", "-"); // hyphens  
            return str;
        }
        static string RemoveAccent(this string txt)
        {
            //byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            byte[] bytes = Encoding.Unicode.GetBytes(txt);
            return Encoding.ASCII.GetString(bytes);
        }
        public static string AvoidHavingSpecialCharacters(this string text)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
                return text;

            text = Regex.Replace(text, "[^a-zA-Z0-9]", String.Empty);

            return text;

        }
        #endregion
        #region datetime
        public static bool IsDateTime(this string strDate)
        {
            try
            {
                if (strDate.Length == 10)
                    _ = DateTime.ParseExact(strDate.Replace("/", "-"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                else
                    _ = DateTime.ParseExact(strDate.Replace("/", "-"), "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public static bool IsTimeSpan(this string strTime)
        {
            if (!DateTime.TryParseExact(strTime, "HH:mm:ss", CultureInfo.InvariantCulture,
                                                          DateTimeStyles.None, out _))
                return false;
            return true;
        }
        public static DateTime ToDateTime(this string strDate)
        {
            if (strDate.Length == 10)
                return DateTime.ParseExact(strDate.Replace("/", "-"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
            else
                return DateTime.ParseExact(strDate.Replace("/", "-"), "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        }
        public static TimeSpan ToTimeSpan(this string strTime)
        {
            DateTime.TryParseExact(strTime, "HH:mm:ss", CultureInfo.InvariantCulture,
                                                          DateTimeStyles.None, out DateTime dt);
            return dt.TimeOfDay;
        }
        public static string ToBase64(this string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return string.Empty;
            if (!System.IO.File.Exists(fileName)) return string.Empty;
            using (FileStream reader = new FileStream(fileName, FileMode.Open))
            {
                byte[] buffer = new byte[reader.Length];
                reader.Read(buffer, 0, (int)reader.Length);
                return Convert.ToBase64String(buffer);
            }
        }
        public static string ToInitCap(this string textVal)
        {
            TextInfo textInfo = new CultureInfo("id-ID", false).TextInfo;
            if (string.IsNullOrEmpty(textVal)) return string.Empty;
            return textInfo.ToTitleCase(textVal.ToLower().Trim());
        }
    }
    #endregion
}



