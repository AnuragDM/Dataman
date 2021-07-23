using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Data;
using System.Data.SqlClient;
using BusinessLayer;
using System.Xml;
using System.Xml.XPath;
using DAL;
using BAL;
using AstralFFMS.ServiceReferenceDMTracker;
public class GetAddress
{
    SqlConnection con = Connection.Instance.GetConnection();
    UploadData upd = new UploadData();
    SqlCommand cmd = new SqlCommand();
    DataTable dt = new DataTable();


    public static string DeserializeNames(string lan1, string lng1)
    {
        HttpWebRequest request = default(HttpWebRequest);
        HttpWebResponse response = null;
        StreamReader reader = default(StreamReader);
        string xmlstring = null;
        //string Lat1 = lan1.ToString();
        //string Long1 = lng1.ToString();
        request = (HttpWebRequest)WebRequest.Create("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + lan1 + "," + lng1 + "");
        response = (HttpWebResponse)request.GetResponse();
        reader = new StreamReader(response.GetResponseStream());
        xmlstring = reader.ReadToEnd();
        JavaScriptSerializer ser = new JavaScriptSerializer();
        nameList myNames = ser.Deserialize<nameList>(xmlstring);
        if (myNames.results.Count > 0)
            return ser.Serialize(myNames.results[0]);
        else return "";
    }
    public class name
    {
        public string formatted_address { get; set; }
    }

    public class nameList
    {
        public List<name> results { get; set; }
    }

  
    public static string getaddress(SqlConnection con, string lan, string lng)
    {
        string address = "";
        try
        {
            string jsonString = DeserializeNames(lan, lng);
            if (jsonString != "")
            {
                string[] words = jsonString.Split('"', '"');
                address = words[3];
                if (address == "")
                { address = "No Location Found!"; }
                else
                {
                    InsertAddress(lan, lng, address);

                }
            }

            else
            {
                XmlDocument doc1 = new XmlDocument();
                doc1.Load("http://dev.virtualearth.net/REST/v1/Locations/" + lan + "," + lng + "?o=xml&key=AroVtGyz2jfzaaRVM4hUVtMMmO3qOwZovHAh_maR9TbuC0Q37RZGr6hWGSjyDluy");

                bool bFlag = false;

                foreach (XmlNode itemNode in doc1.DocumentElement.ChildNodes)
                {
                    if (itemNode.Name == "ResourceSets")
                    {
                        foreach (XmlNode childNode in itemNode.ChildNodes)
                        {
                            if (childNode.Name == "ResourceSet")
                            {
                                foreach (XmlNode SubchildNode in childNode.ChildNodes)
                                {
                                    if (SubchildNode.Name == "Resources")
                                    {
                                        foreach (XmlNode SubchildNode1 in SubchildNode.ChildNodes)
                                        {
                                            if (SubchildNode1.Name == "Location")
                                            {
                                                foreach (XmlNode SubchildNode2 in SubchildNode1.ChildNodes)
                                                {
                                                    if (SubchildNode2.Name == "Address")
                                                    {
                                                        
                                                        
                                                        foreach (XmlNode SubchildNode3 in SubchildNode2.ChildNodes)
                                                        {
                                                            if (SubchildNode3.Name == "FormattedAddress")
                                                            {
                                                                address = SubchildNode3.InnerText;
                                                                bFlag = true;
                                                                break;
                                                            }

                                                            if (bFlag == true) break;                                                           
                                                        }

                                                        if (bFlag == true) break;                                                                                                                   
                                                    }
                                                    if (bFlag == true) break;                                                           
                                                }
                                                if (bFlag == true) break;                                                           
                                            }
                                            if (bFlag == true) break;                                                           
                                        }
                                        if (bFlag == true) break;                                                           
                                    }
                                    if (bFlag == true) break;                                                           
                                }
                                if (bFlag == true) break;                                                           
                            }
                            if (bFlag == true) break;                                                           
                        }
                        if (bFlag == true) break;                                                           
                    }
                    if (bFlag == true) break;                                                           
                }
                if (!string.IsNullOrEmpty(address))
                {
                    InsertAddress(lan, lng, address);
                }
                else { address = "No Location Found"; }
            }
        }
        catch (Exception ae)
        {
             address = "No Location Found"; 
            //  address = "you have exceeded your daily request quota";
        }
        return address;
    }

    private static void InsertAddress(string lan,string lng, string address)
    {
        try {
            if (DbConnectionDAL.GetIntScalarVal("select count(*) from Addresses where Latitude='" + lan + "' and Longitude='" + lng + "'") <= 0)
            {
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text,"INSERT INTO Addresses (Latitude,Longitude,Address) VALUES ('" + lan + "','" + lng + "','" + address + "')");
            }
        }
        catch (Exception ex) { ex.ToString(); }
    }


    internal static string getaddress(string getConStr, string p, string p_2)
    {
        throw new NotImplementedException();
    }
}