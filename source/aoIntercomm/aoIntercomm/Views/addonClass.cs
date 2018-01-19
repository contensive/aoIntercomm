

using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;
using Contensive.BaseClasses;


namespace aoIntercomm.Views {
    //
    // this sample creates an addon collection (a group off addons that install together)
    // -- Change the namespace to the collection name
    // 2) Change this class name to the addon name
    // 3) Create a Contensive Addon record with the namespace apCollectionName.ad
    // 3) add reference to CPBase.DLL, typically installed in c:\program files\kma\contensive\
    //
    public class addonClass : Contensive.BaseClasses.AddonBaseClass {
        //
        // -- Contensive calls the execute method of your addon class
        public override object Execute(Contensive.BaseClasses.CPBaseClass cp) {
            string result = "";
            try {


                try {
                    Assembly _assembly = Assembly.GetExecutingAssembly();
                    StreamReader IntercommScript = new StreamReader(_assembly.GetManifestResourceStream("aoIntercomm.IntercommScript.txt"));



                } catch {
                    //
                }

                //
                // your code here
                //
                result = "success response";
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                result = "error response";
            }
            return result;
        }
        //public static void Main(string[] args) {
        //    var hmac = new MyHmac();
        //    System.Console.WriteLine(hmac.CreateToken("Message", "secret")); // AA747C502A898200F9E4FA21BAC68136F886A0E27AEC70BA06DAF2E2A5CB5597
        //}
        private string CreateToken(string message, string secret) {
            secret = secret ?? "LC4UyNZVv876hCyysgMDE28yW3AQu2e_yEK-XsHh";
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
    }
}
