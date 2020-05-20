
using System;
using System.Runtime.InteropServices;
using Contensive.Addons.aoIntercom.Controllers;
using Contensive.Addons.aoIntercom.Properties;
using Contensive.BaseClasses;

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
                string appId = cp.Site.GetText("Intercom AppId");
                if (string.IsNullOrWhiteSpace(appId)) { return "<!-- Intercom disabled because AppId not set in Intercom Settings -->"; }
                //
                if (!cp.User.IsAuthenticated) {
                    //
                    // -- not authenticated
                    result = Resources.IntercommNonAuthScript.Replace("{0}", appId);
                } else {
                    //
                    // -- authenticated
                    string dateAddedUnix = "";
                    using (CPCSBaseClass cs = cp.CSNew()) {
                        DateTime dateAdded = DateTime.Now;
                        if (cs.OpenSQL("select dateAdded from ccmembers where id=" + cp.User.Id )) { dateAdded = cs.GetDate("dateAdded"); }
                        long unixTime = ((DateTimeOffset)(dateAdded.ToUniversalTime())).ToUnixTimeSeconds();
                        dateAddedUnix = unixTime.ToString();
                    }
                    string intercomSecret = cp.Site.GetText("Intercom Identity Verification Secret");
                    string userHash = intercomController.CreateToken(cp.User.Id.ToString(), intercomSecret);
                    result = Resources.IntercommAuthScript
                        .Replace("{0}", appId)
                        .Replace("{1}", cp.Utils.EncodeJavascript(cp.User.Email))
                        .Replace("{2}", userHash)
                        .Replace("{3}", cp.Utils.EncodeJavascript(cp.User.Name))
                        .Replace("{4}", cp.User.Id.ToString())
                        .Replace("{5}", dateAddedUnix);
                }
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
            }
            return result;
        }
    }
}
