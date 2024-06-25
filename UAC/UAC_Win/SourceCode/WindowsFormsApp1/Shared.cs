using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    static class Shared
    {
        public static int UserId;
        public static string Username;
        public static int RoleDesc;
        public static int RollsId;
        public static int CreatedBy;

        public static string EncryptPassword(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedBytes = ProtectedData.Protect(plainTextBytes, null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedBytes);
        }

        public static string DecryptPassword(string encryptedText)
        {
            // Check if the string is a valid Base-64 encoded string
            if (IsBase64String(encryptedText))
            {
                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                byte[] decryptedBytes = ProtectedData.Unprotect(encryptedBytes, null, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
            else
            {
                // If the string is not Base-64 encoded, assume it is plain text
                return encryptedText;
            }
        }

        public static bool IsBase64String(string base64)
        {
            // Helper method to check if a string is a valid Base-64 encoded string
            if (string.IsNullOrEmpty(base64) || base64.Length % 4 != 0
                || base64.Contains(" ") || base64.Contains("\t") || base64.Contains("\r") || base64.Contains("\n"))
                return false;

            try
            {
                Convert.FromBase64String(base64);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

    }
}
