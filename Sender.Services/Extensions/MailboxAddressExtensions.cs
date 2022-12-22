using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sender.Services.Extensions
{
    public static class MailboxAddressExtensions
    {
        private static readonly char[] _emailReplace = new char[] { '_', '.', '-' };
        private static readonly char[] _emailSeparator = new char[] { ';', ',', ' ', '&', '|' };

        public static IEnumerable<MailboxAddress> ParseEmailContacts(this string value)
        {
            IEnumerable<MailboxAddress> contacts = null;
            if (!string.IsNullOrEmpty(value))
            {
                var emailAddresses = value.Split(_emailSeparator, StringSplitOptions.RemoveEmptyEntries);
                contacts = emailAddresses.Select(email => email.GetContactFromEmailAddress());
            }
            return contacts ?? Enumerable.Empty<MailboxAddress>();
        }

        public static MailboxAddress GetContactFromEmailAddress(this string emailAddress)
        {
            string name = emailAddress.GetNameFromEmailAddress();
            var contact = new MailboxAddress(name, emailAddress);
            return contact;
        }

        private static string GetNameFromEmailAddress(this string emailAddress)
        {
            var email = emailAddress?.Split('@')?.FirstOrDefault();
            string name = email.SpaceReplaceTitleCase(_emailReplace) ?? string.Empty;
            return name;
        }

        private static string SpaceReplaceTitleCase(this string value, char[] replace)
        {
            string result = value.Replace(replace, ' ');

            if (result.Length > 0)
            {
                var builder = new List<char>();
                char[] array = result.ToCharArray();
                char prevChar = array[0];
                builder.Add(char.ToUpper(prevChar));

                for (int i = 1; i < array.Length; i++)
                {
                    char thisChar = array[i];
                    char? nextChar = i + 1 < array.Length ?
                        array[i + 1] : null as char?;
                    bool isNextLower = nextChar != null &&
                        char.IsLower(nextChar.Value);
                    bool isPrevSpace = prevChar == ' ';
                    bool isPrevUpper = char.IsUpper(prevChar);
                    bool isPrevLower = char.IsLower(prevChar);
                    bool isThisUpper = char.IsUpper(thisChar);
                    bool isAcronym = isThisUpper && isPrevUpper;
                    bool isTitleCase = isAcronym && isNextLower;
                    bool isWordEnd = isThisUpper && isPrevLower;
                    if (isWordEnd || isTitleCase)
                    {
                        builder.Add(' ');
                    }
                    else if (isPrevSpace && !isThisUpper)
                    {
                        thisChar = char.ToUpper(thisChar);
                    }
                    builder.Add(thisChar);
                    prevChar = thisChar;
                }

                result = new string(builder.ToArray());
            }

            return result;
        }

        private static string Replace(this string value, char[] oldChars, char newChar)
        {
            string result = value ?? string.Empty;
            if (oldChars != null)
                foreach (var r in oldChars)
                    result = result.Replace(r, newChar);
            return result;
        }
    }
}
