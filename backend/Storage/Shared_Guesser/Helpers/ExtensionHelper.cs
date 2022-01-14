using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace Shared_Guesser.Helpers
{
    public static class ExtensionHelper
    {
        public static bool ValidatePassword(string password)
        {
            const int MIN_LENGTH = 8;

            bool meetsLengthRequirements = password.Length >= MIN_LENGTH;
            bool hasUpperCaseLetter = false;
            bool hasLowerCaseLetter = false;
            bool hasDecimalDigit = false;

            if (meetsLengthRequirements)
            {
                foreach (char c in password)
                {
                    if (char.IsUpper(c))
                    {
                        hasUpperCaseLetter = true;
                    }
                    else if (char.IsLower(c))
                    {
                        hasLowerCaseLetter = true;
                    }
                    else if (char.IsDigit(c))
                    {
                        hasDecimalDigit = true;
                    }
                }
            }

            bool isValid = meetsLengthRequirements
                        && hasUpperCaseLetter
                        && hasLowerCaseLetter
                        && hasDecimalDigit
                        ;
            return isValid;
        }
        public static bool SendEmail(string receiverEmail, string receiverFullName, MessageTypes messageType, Dictionary<string, string> dict )
        {
            string subject = "";
            string htmlMessage = "";
            switch (messageType)
            {
                case MessageTypes.AccountActivation:
                    subject = "Activation link for your guesser account! Join us!";
                    htmlMessage = ExtensionHelper.GenerateEmailText($"Hello {receiverFullName}! Welcome on board!",
                        "Just click on the link below to complete your registration:",
                        $"{dict["frontURL"]}/confirm-account?token={dict["code"]}&id={dict["id"]}", "Activate");
                    break;
                case MessageTypes.ForgetPassword:
                    subject = "Password restoring";
                    htmlMessage = ExtensionHelper.GenerateEmailText($"Hello {receiverFullName}! Let`s try to give back your account!",
                        "Click the link below to restore your password!",
                        $"{dict["frontURL"]}/forgot-password?token={dict["code"]}&email={dict["email"]}",
                        "Restore your password");
                    break;
            }
            

            MailAddress from = new MailAddress("guesser.help@gmail.com", "Guesser");
            MailAddress to = new MailAddress(receiverEmail);
            MailMessage m = new MailMessage(from, to)
            {
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
            };
            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential("guesser.help@gmail.com", "guesser2021App");
                smtp.EnableSsl = true;
                smtp.Send(m);
            }
            return true;


        }

        public static string GenerateEmailText(string title, string mainText, string link, string linkVissibleText)
        {
            
            string messageBodyHTML = "<div style=\"width:100%;height:100%;text-align:center;\">" +
                                     "<h1 style=\"color:black;\">" + title + "</h1>" +
                                     $"<div><img src='https://i.ibb.co/Yh5X7wV/2021-05-01-210258.png'></div>" +
                                     "<h3>" + mainText + "</h3>" +
                                     "<a style=\"font-size:20px;\" href=\"" + link + "\">" + linkVissibleText + "</a>" +
                                     "</div>";
            return messageBodyHTML;
        }
        /* =======================
         * HASHED PASSWORD FORMATS
         * =======================
         * 
         * Version 3:
         * PBKDF2 with HMAC-SHA256, 128-bit salt, 256-bit subkey, 10000 iterations.
         * Format: { 0x01, prf (UInt32), iter count (UInt32), salt length (UInt32), salt, subkey }
         * (All UInt32s are stored big-endian.)
         */
        public static string HashPassword(string password)
        {
            var prf = KeyDerivationPrf.HMACSHA256;
            var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            const int iterCount = 10000;
            const int saltSize = 128 / 8;
            const int numBytesRequested = 256 / 8;

            // Produce a version 3 (see comment above) text hash.
            var salt = new byte[saltSize];
            rng.GetBytes(salt);
            var subkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, numBytesRequested);

            var outputBytes = new byte[13 + salt.Length + subkey.Length];
            outputBytes[0] = 0x01; // format marker
            WriteNetworkByteOrder(outputBytes, 1, (uint)prf);
            WriteNetworkByteOrder(outputBytes, 5, iterCount);
            WriteNetworkByteOrder(outputBytes, 9, saltSize);
            Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
            Buffer.BlockCopy(subkey, 0, outputBytes, 13 + saltSize, subkey.Length);
            return Convert.ToBase64String(outputBytes);
        }

        public static bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            //hashedPassword = hashedPassword.Replace('-', '+').Replace('_', '/').PadRight(4 * ((hashedPassword.Length + 3) / 4), '=');
            var decodedHashedPassword = Convert.FromBase64String(hashedPassword);

            // Wrong version
            if (decodedHashedPassword[0] != 0x01)
            {
                return false;
            }

            // Read header information
            var prf = (KeyDerivationPrf)ReadNetworkByteOrder(decodedHashedPassword, 1);
            var iterCount = (int)ReadNetworkByteOrder(decodedHashedPassword, 5);
            var saltLength = (int)ReadNetworkByteOrder(decodedHashedPassword, 9);

            // Read the salt: must be >= 128 bits
            if (saltLength < 128 / 8)
            {
                return false;
            }
            var salt = new byte[saltLength];
            Buffer.BlockCopy(decodedHashedPassword, 13, salt, 0, salt.Length);

            // Read the subkey (the rest of the payload): must be >= 128 bits
            var subkeyLength = decodedHashedPassword.Length - 13 - salt.Length;
            if (subkeyLength < 128 / 8)
            {
                return false;
            }
            var expectedSubkey = new byte[subkeyLength];
            Buffer.BlockCopy(decodedHashedPassword, 13 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

            // Hash the incoming password and verify it
            var actualSubkey = KeyDerivation.Pbkdf2(providedPassword, salt, prf, iterCount, subkeyLength);
            return actualSubkey.SequenceEqual(expectedSubkey);
        }

        private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
        {
            buffer[offset + 0] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)(value >> 0);
        }

        private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
        {
            return ((uint)(buffer[offset + 0]) << 24)
                | ((uint)(buffer[offset + 1]) << 16)
                | ((uint)(buffer[offset + 2]) << 8)
                | buffer[offset + 3];
        }
    }
    public enum MessageTypes
    {
        ForgetPassword,
        AccountActivation
    }
}
