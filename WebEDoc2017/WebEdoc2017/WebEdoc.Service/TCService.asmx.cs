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
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string EncryptAES(string TextToEncrypt)
        {
            string strEncrypted = string.Empty;
            string strhash = string.Empty;

            strEncrypted = TC.TConnect.EncryptAES(TextToEncrypt);
            strhash = CalculateSHA2Hash(TextToEncrypt);

            var data = new { Encrypted = strEncrypted, Hash = strhash };

            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            return js.Serialize(data);

        }

        public string DecryptAES(string TextToDecrypt)
        {
            return TC.TConnect.DecryptAES(TextToDecrypt);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string DecryptAES(string TextToDecrypt, string Hash)
        {
            return TC.TConnect.DecryptAESCalculateHash(TextToDecrypt, Hash);
        }

        [WebMethod]
        public string CalculateSHA2Hash(string InputString)
        {
            return TC.TConnect.CalculateSHA2Hash(InputString);
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string DecryptAESWithIDs(string ID, string Type)
        {
            return TC.TConnect.GetDecryptedStringWithID(ID, Type);
        }
    }
}
