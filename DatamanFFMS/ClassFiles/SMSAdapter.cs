using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Configuration;
/// <summary>
/// Summary description for SMSAdapter
/// </summary>
public class SMSAdapter
{
	public SMSAdapter()
	{
		//
		// TODO: Add constructor logic here
		//
	}


    public void sendSms(string toUsers, string message)
    {
        string sURL = string.Format(ConfigurationManager.AppSettings["SMSAPI"].ToString(), toUsers, message); ;
        string sResponse = GetResponse(sURL);
    }

    public void sendSmsbyTable(string smskey, string toUsers, string message)
    {
        string sURL = string.Format(smskey, toUsers, message); ;
        string sResponse = GetResponse(sURL);
    }
    public string sendSmsbyTableWithResponse(string smskey, string toUsers, string message)
    {
        string sURL = string.Format(smskey, toUsers, message); ;
        string sResponse = GetResponse(sURL);
        return sResponse;
    }

    //protected override WebRequest GetWebRequest(Uri uri)
    //{
    //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sURL);
    //    request.KeepAlive = false;
    //    request.MaximumAutomaticRedirections = 4;
    //    request.Credentials = CredentialCache.DefaultCredentials;
    //    HttpWebRequest webRequest = (HttpWebRequest)base.GetWebRequest(uri);
    //    //Setting KeepAlive to false
    //    webRequest.KeepAlive = false;
    //    return webRequest;
    //}
    public static string GetResponse(string sURL)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sURL);
        request.KeepAlive = false;
        request.MaximumAutomaticRedirections = 4;
        request.Credentials = CredentialCache.DefaultCredentials;
        try
        {
            HttpWebResponse response = (HttpWebResponse)request
            .GetResponse();
            Stream receiveStream = response.GetResponseStream(
            );
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            string sResponse = readStream.ReadToEnd();
            response.Close();
            readStream.Close();
            return sResponse;
        }
        catch (Exception ex)
        {
            ex.ToString();

        }
        return "";
    }
}

    

