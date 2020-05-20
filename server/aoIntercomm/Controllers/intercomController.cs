
using System.Security.Cryptography;

namespace Contensive.Addons.aoIntercom.Controllers {
    public static class intercomController {
        //
        //====================================================================================================
        //
        public static string CreateToken(string message, string secret) {
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte)) {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);

                var sb = new System.Text.StringBuilder();
                for (var i = 0; i <= hashmessage.Length - 1; i++) {
                    sb.Append(hashmessage[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
        //
        //====================================================================================================
        //
        //public static void Main(string[] args) {
        //    var hmac = new MyHmac();
        //    System.Console.WriteLine(hmac.CreateToken("Message", "secret")); // AA747C502A898200F9E4FA21BAC68136F886A0E27AEC70BA06DAF2E2A5CB5597
        //}
    }
}
