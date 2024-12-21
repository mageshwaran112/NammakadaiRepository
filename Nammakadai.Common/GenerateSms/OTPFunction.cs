using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Nammakadai.Common.GenerateSms
{
    public class OTPFunction
    {
        private const string CustomerId = "78BBCF12-C0CA-4143-A025-28C19ADB2CA2";
        private const string APIKey = "+Q1sygLwu45a1lN3FQkcan4hqnY8dq1fqT6Iu13MBPcBP+vamwTuuNn9VjtV5++aRkUut/ebItKVRU1uhJaqvw==";
        private static readonly string BaseUrl = "https://rest-api.telesign.com";
        private static readonly HttpClient Client = new HttpClient();

        // Generate 4 Digit Random OTP
        private string GenerateOTP()
        {
            var random = new Random();
            var otp = random.Next(1000, 9999).ToString();
            return otp;
        }

        // Convert into Hash Code.
        private static string HashOTP(string otp)
        {
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(otp));
            return Convert.ToHexString(hashBytes);
        }

        public string SendOTP(string phoneNumber)
        {
            var otp = GenerateOTP();
            try
            {
                // SendSmsAsync(phoneNumber, otp);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending OTP: {ex.Message}");
            }
            return HashOTP(otp);
        }

    }
}
