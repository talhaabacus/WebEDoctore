using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using TC = WebEdoc2017.TrustedConnection;
using System.Web.Script.Services;

namespace WebEdoc.Service
{
    /// <summary>
    /// Summary description for TCService
    /// </summary>

    [WebService(Namespace = "TCService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]

    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TCService : System.Web.Services.WebService
    {
        [WebMethod]
        public string EncryptAES(string TextToEncrypt, string EncryptionKey)
        {
            return TC.TConnect.EncryptAES(TextToEncrypt, EncryptionKey);
        }

        [WebMethod]
        public string DecryptAES(string TextToDecrypt, string EncryptionKey)
        {
            return TC.TConnect.DecryptAES(TextToDecrypt, EncryptionKey);
        }

        [WebMethod]
        public string CalculateSHA2Hash(string InputString)
        {
            return TC.TConnect.CalculateSHA2Hash(InputString);
        }
    }
}
