using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Base
{
    

    public static class FileInfoExtension
    {
        /// <summary>
        /// Чтобы не перегружать память при считвание огромный файлов, читаем его построчно
        /// </summary>
        public static IEnumerable<string?> ReadLines(this FileInfo? fileInfo)
        {
            if (!fileInfo.Exists) throw new FileNotFoundException("File not found");
            using var stream = fileInfo.OpenText();
            while (!stream.EndOfStream)
                yield return stream.ReadLine();


        }
        /// <summary>
        /// Cчитывает весь текст и помещает в StringBuilder
        /// </summary>
        public async static Task<StringBuilder> ReadTextBuilder(this FileInfo? fileInfo)
        {
            if (!fileInfo.Exists) throw new FileNotFoundException("File not found");
            StringBuilder @string = new();
            using var stream = fileInfo.OpenText();
            var t = await stream.ReadToEndAsync();
            //var t =await fileInfo.OpenText().ReadToEndAsync();
            return @string.Append(t);




        }


        /// <summary>Проверка на существование файла. Если файл не существует, то генерируется исключение</summary>
        /// <param name="file">Проверяемый файл</param>
        /// <param name="Message">Сообщение, добавляемое в исключение</param>
        /// <returns>Информация о файле</returns>
        /// <exception cref="FileNotFoundException">если файл не существует</exception>
        public static FileInfo ThrowIfNotFound(this FileInfo file, string? Message = null)
        {
            file.Refresh();
            return file.Exists ? file : throw new FileNotFoundException(Message ?? $"Файл {file} не найден");
        }

    }
}
