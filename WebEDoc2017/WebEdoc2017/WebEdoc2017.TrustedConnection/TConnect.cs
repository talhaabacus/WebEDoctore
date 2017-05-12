using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using WebEdoc2017.DataLayer;
using System.Security.Cryptography;
using System.Data;

namespace WebEdoc2017.TrustedConnection
{
    public class TConnect
    {
        TConnect()
        {
        }

        public static string EncryptAES(string TextToEncrypt)
        {
            string Encryption_Key = GetEncryptionKey();
            string EncryptedString = string.Empty;

            if (!(string.IsNullOrWhiteSpace((Encryption_Key))))
            {
                byte[] clearBytes = Encoding.Unicode.GetBytes(TextToEncrypt);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Encryption_Key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        EncryptedString = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }

            return EncryptedString;
        }

        public static string DecryptAES(string TextToDecrypt)
        {
            string Encryption_Key = GetEncryptionKey();
            string DecryptedString = string.Empty;

            if (!(string.IsNullOrWhiteSpace((Encryption_Key))))
            {
                try
                {
                    byte[] cipherBytes = Convert.FromBase64String(TextToDecrypt);
                    using (Aes encryptor = Aes.Create())
                    {
                        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Encryption_Key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                        encryptor.Key = pdb.GetBytes(32);
                        encryptor.IV = pdb.GetBytes(16);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(cipherBytes, 0, cipherBytes.Length);
                                cs.Close();
                            }

                            DecryptedString = Encoding.Unicode.GetString(ms.ToArray());
                        }
                    }
                }
                catch (Exception)
                {
                    DecryptedString = "Error: Message integrity has been lost.";
                }

                if (string.IsNullOrWhiteSpace((DecryptedString)))
                {
                    DecryptedString = "Error: Message integrity has been lost.";
                }

                return DecryptedString;
            }

            if (string.IsNullOrWhiteSpace((DecryptedString)))
            {
                DecryptedString = "Error: Message integrity has been lost.";
            }

            return DecryptedString;
        }

        public static string DecryptAESCalculateHash(string TextToDecrypt, string Hash)
        {
            string Encryption_Key = GetEncryptionKey();
            string DecryptedString = string.Empty;

            if (!(string.IsNullOrWhiteSpace((Encryption_Key))))
            {
                try
                {
                    byte[] cipherBytes = Convert.FromBase64String(TextToDecrypt);
                    using (Aes encryptor = Aes.Create())
                    {
                        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Encryption_Key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                        encryptor.Key = pdb.GetBytes(32);
                        encryptor.IV = pdb.GetBytes(16);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(cipherBytes, 0, cipherBytes.Length);
                                cs.Close();
                            }

                            DecryptedString = Encoding.Unicode.GetString(ms.ToArray());

                            if (!(string.IsNullOrWhiteSpace(DecryptedString)))
                            {
                                string CalculatedHash = CalculateSHA2Hash(DecryptedString);
                                if (CalculatedHash == Hash)
                                {
                                    return DecryptedString;
                                }
                                else
                                {
                                    DecryptedString = "Error: Message integrity has been lost.";
                                }
                            }

                        }
                    }
                }
                catch (Exception)
                {
                    DecryptedString = "Error: Message integrity has been lost.";
                }

            }

            if (string.IsNullOrWhiteSpace((DecryptedString)))
            {
                DecryptedString = "Error: Message integrity has been lost.";
            }

            return DecryptedString;

        }

        public static string CalculateSHA2Hash(string InputString)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(InputString);
            byte[] hash = sha256.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

        public static string GetEncryptionKey()
        {
            WEBeSettings settings = new WEBeSettings();
            OracleDataAccess.OracleCommandData data = new OracleDataAccess.OracleCommandData();
            try
            {
                data._CommandType = CommandType.Text;
                data.CommandText = "select encryptionkey from WEBESETTINGS";
                data.OpenWithOutTrans();

                //Executing Query
                DataSet ds = data.Execute(OracleDataAccess.ExecutionType.ExecuteDataSet) as DataSet;

                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    settings.dt = ds.Tables[0];
                    settings.isValid = true;
                }

            }
            catch (Exception ex)
            {
                settings.Error = ex.Message.ToString();
                //throw;
            }
            finally
            {
                data.Close();
            }

            return settings.dt.Rows[0]["encryptionkey"].ToString();

        }

        public static string GetDecryptedStringWithID(string ID, string Type)
        {
            WEBeSettings settings = new WEBeSettings();
            OracleDataAccess.OracleCommandData data = new OracleDataAccess.OracleCommandData();
            string DecryptedString = string.Empty;
            try
            {
                data._CommandType = CommandType.Text;

                if (Type.ToLower() == "master")
                {
                    data.CommandText = "select note, hash from events where event_id =" + ID;
                }
                else if (Type.ToLower() == "detail")
                {
                    data.CommandText = "select note, hash from EVENTS_DATA where note_id =" + ID;
                }

                data.OpenWithOutTrans();

                //Executing Query
                DataSet ds = data.Execute(OracleDataAccess.ExecutionType.ExecuteDataSet) as DataSet;

                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    string EncryptedString = ds.Tables[0].Rows[0]["note"].ToString();
                    string Hash = ds.Tables[0].Rows[0]["hash"].ToString();
                    DecryptedString = DecryptAESCalculateHash(EncryptedString, Hash);
                }

                if (string.IsNullOrWhiteSpace((DecryptedString)))
                {
                    DecryptedString = "Error: Message integrity has been lost.";
                }

            }
            catch (Exception ex)
            {
                settings.Error = ex.Message.ToString();
                //throw;
            }
            finally
            {
                data.Close();
            }

            return DecryptedString;

        }




        public struct WEBeSettings
        {
            public DataTable dt;
            public bool isValid;
            public string Error;
        }

    }
}
