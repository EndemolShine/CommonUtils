using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace CommonUtilsX64
{
    public class Messagebird
    {

        public static string SendSms(string pMobileNumber, string pMsgText)
        {
            string result;
            var strGet =
                $"username={HttpUtility.UrlEncode(ConfigurationManager.AppSettings["MessagebirdUsername"])}&password={HttpUtility.UrlEncode(ConfigurationManager.AppSettings["MessagebirdPassword"])}&body={HttpUtility.UrlEncode(pMsgText)}&sender={HttpUtility.UrlEncode(ConfigurationManager.AppSettings["MessagebirdSender"])}&destination={pMobileNumber}&test={Convert.ToInt16(ConfigurationManager.AppSettings["MessagebirdMode"])}";

            StreamWriter myRequest = null;
            var objRequest = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["MessagebirdUrl"]);

            objRequest.Method = "POST";
            objRequest.ContentLength = Encoding.UTF8.GetByteCount(strGet);
            objRequest.ContentType = "application/x-www-form-urlencoded";
            try
            {
                myRequest = new StreamWriter(objRequest.GetRequestStream());
                myRequest.Write(strGet);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                myRequest?.Close();   // myRequest != null
            }

            var objResponse = (HttpWebResponse)objRequest.GetResponse();

            // ReSharper disable once AssignNullToNotNullAttribute
            using (var sr = new StreamReader(stream: objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                sr.Close();
            }

            return result;
        }
    }
}
