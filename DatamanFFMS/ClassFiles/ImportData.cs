using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using BusinessLayer;
using DAL;
using BAL;
using AstralFFMS.ServiceReferenceDMTracker;

/// <summary>
/// Summary description for ImportData
/// </summary>
public class ImportData
{
	public ImportData()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    #region Import PersonMaster
    public string ImportPerson(string PersonName, string DeviceNo,string MobileNo,string SeniorMobileNo,string Empcode, string FromTime, string ToTime, string RecordInterval, string UploadInterval, string Accuracy, string Latitude, string Longitude, string SendSMSToPerson, string SendSMSToSenior,string GroupName)
    {
        string result = "";
        string sqlCommand = "sp_importPerson";
        SqlConnection con = Connection.Instance.GetConnection();
        SqlCommand cmd = new SqlCommand(sqlCommand, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@PersonName", PersonName);
        cmd.Parameters.AddWithValue("@DeviceNo", DeviceNo);
        cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
        cmd.Parameters.AddWithValue("@SeniorMobileNo", SeniorMobileNo);
        cmd.Parameters.AddWithValue("@Empcode", Empcode);
        cmd.Parameters.AddWithValue("@FromTime", FromTime);
        cmd.Parameters.AddWithValue("@ToTime", ToTime);
        cmd.Parameters.AddWithValue("@RecordInterval", RecordInterval);
        cmd.Parameters.AddWithValue("@UploadInterval", UploadInterval);
        cmd.Parameters.AddWithValue("@Accuracy", Accuracy);
        cmd.Parameters.AddWithValue("@Latitude", Latitude);
        cmd.Parameters.AddWithValue("@Longitude", Longitude);
        cmd.Parameters.AddWithValue("@SendSMSToPerson", SendSMSToPerson);
        cmd.Parameters.AddWithValue("@SendSMSToSenior", SendSMSToSenior);
        cmd.Parameters.AddWithValue("@GroupName", GroupName);
       SqlParameter OutputParam = new SqlParameter("@OutputParam", SqlDbType.VarChar, 100);
            OutputParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(OutputParam);
            try
            {
                Connection.Instance.OpenConnection(con);
                cmd.ExecuteNonQuery();
                result = OutputParam.Value.ToString();
                cmd.Parameters.Clear();
                UploadData upd=new UploadData();
                upd.AddPersonLicence(PersonName, DeviceNo);
              
                string strmaildetails = "SELECT PersonName,DeviceNo FROM PersonMaster WHERE DeviceNo=" + DeviceNo + "";
                DataTable dtmail = DbConnectionDAL.getFromDataTable(strmaildetails);
                upd.SendInsertionEmail(dtmail.Rows[0]["PersonName"].ToString(), dtmail.Rows[0]["DeviceNo"].ToString(), GroupName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Connection.Instance.CloseConnection(con);
            }
            return result;
    }
    #endregion


    #region Import GroupMaster
    public string ImportGroup(string GroupName,string MobileNo)
    {
        string result = "";
        string sqlCommand = "sp_importGroup";
        SqlConnection con = Connection.Instance.GetConnection();
        SqlCommand cmd = new SqlCommand(sqlCommand, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@GroupName", GroupName);
        cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
        SqlParameter OutputParam = new SqlParameter("@OutputParam", SqlDbType.VarChar, 100);
        OutputParam.Direction = ParameterDirection.Output;
        cmd.Parameters.Add(OutputParam);
        try
        {
            Connection.Instance.OpenConnection(con);
            cmd.ExecuteNonQuery();
            result = OutputParam.Value.ToString();
            cmd.Parameters.Clear();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            Connection.Instance.CloseConnection(con);
        }
        return result;
    }
    #endregion

    public bool AddPersonLicence(string Name, string DeviceID)
    {
        WebServiceSoapClient DMT = new WebServiceSoapClient("WebServiceSoap");
        string val = DMT.InsertPersonLicense(Settings.Instance.CompCode, DeviceID, Name, Settings.Instance.CompUrl);
        if (val == "1")
            return true;
        else
            return false;
    }

    public bool Insert_GrahaakLicense(string CompCode,string DeviceID, string PersonName,string Compurl, string mob, string product)
    {

        WebServiceSoapClient DMT = new WebServiceSoapClient("WebServiceSoap");
        string val = DMT.Insert_GrahaakLicense(CompCode, DeviceID, PersonName, Compurl, mob, product);
        if (val == "1")
            return true;
        else
            return false;
       
    }

    public bool Update_GrahaakLicense(string CompCode, string DeviceID, string PersonName, string Compurl, string mob, string active, string olddevice, bool mobileaccess, string product, string hdnoldMobile)
    {

        WebServiceSoapClient DMT = new WebServiceSoapClient("WebServiceSoap");
        string val = DMT.Update_GrahaakLicense(CompCode, DeviceID, PersonName, Compurl, mob, active, olddevice, mobileaccess, product, hdnoldMobile);
        if (val == "1")
            return true;
        else
            return false;

    }   
   

}