using System.Text;
using System.Text.Json;

namespace Extension.Base
{
    public static class UtilExtension
    {
        /// <summary>
        /// помогает безопасно конвертировать строку в bool
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool StringToBool(this string str)
        {
            if (str is null) return false;
            bool.TryParse(str, out bool result);
            return result;
        }
        /// <summary>
        /// быстрое форматирования объекта в json(string)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T obj) where T : class
        {
            try
            {
                return JsonSerializer.Serialize(obj);
            }
            catch (Exception ex)
            {
               return null;
            }          
        }
        /// <summary>
        /// быстрое форматирования объекта в json(string)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TOuttype StringToToObject<TOuttype>(this string obj) where TOuttype : class
        {
            try
            {
                return JsonSerializer.Deserialize<TOuttype>(obj);//.Serialize(obj);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// строка в byte[] UTF8 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] StringToByteArray(this string str)// where T : class
        {
            try
            {
                return Encoding.UTF8.GetBytes(str);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// строка в byte[] UTF8 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ByteArrayToString(this byte[] str)// where T : class
        {
            try
            {
                return Encoding.UTF8.GetString(str);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// Конвертирует строку в stream
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static Stream? Base64ToStream(this string base64)
        {
            try
            {
                if (base64 is not null && base64.Length >20)
                {
                    return new MemoryStream(Convert.FromBase64String(base64));
                }
                return null;

            }
            
            catch (Exception)
            {

                return null; 
            }
           
        }


    }
}