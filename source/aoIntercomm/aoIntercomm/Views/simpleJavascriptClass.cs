
using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;
using Contensive.BaseClasses;
using Contensive.Addons.aoIntercom.Controllers;
using static Contensive.Addons.aoIntercom.Controllers.intercomController;
using Contensive.Addons.aoIntercom.Properties;

namespace Contensive.Addons.aoIntercom.Views {
    //
    //====================================================================================================
    /// <summary>
    /// Intercomm integration
    /// </summary>
    public class simpleJavascriptClass : Contensive.BaseClasses.AddonBaseClass {
        //
        //====================================================================================================
        /// <summary>
        /// add Intercomm elements to the page.
        /// based on: https://developers.intercom.com/docs/basic-javascript
        /// in the account: https://app.intercom.io/a/apps/hajvz7zq/settings/web
        /// identity verification: https://app.intercom.io/a/apps/hajvz7zq/guide/web_integration/web_integration_identity_verification
        /// </summary>
        /// <param name="cp"></param>
        /// <returns></returns>
        public override object Execute(Contensive.BaseClasses.CPBaseClass cp) {
            string result = "";
            try {
                string intercomAppId = cp.Site.GetText("Intercom AppId");
                if (!cp.User.IsAuthenticated) {
                    //
                    // -- not authenticated
                    result = Resources.IntercommNonAuthScript.Replace("{0}", intercomAppId);
                } else {
                    //
                    // -- authenticated
                    string intercomSecret = cp.Site.GetText("Intercom Identity Verification Secret");
                    string userHash = intercomController.CreateToken(cp.User.Email.ToString(), intercomSecret);
                    string source = Properties.Resources.IntercommAuthScript;
                    result = source.Replace("{0}", intercomAppId ).Replace( "{1}", cp.User.Email ).Replace( "{2}", userHash);
                }
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex );
            }
            return result;
        }
    }
}
