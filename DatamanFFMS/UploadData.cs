using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text;
using BusinessLayer;
//using DataAccessLayer;
using BAL;
using DAL;
using AstralFFMS.ServiceReferenceDMTracker;
/// <summary>
/// Summary description for UploadData
/// </summary>
public class UploadData
{
    public UploadData()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region Get Home Page Data
    #region Insert Update Store Master
    public void GetHomePageData()
    {
      
        string sqlCommand = "Sp_GetHomePageData";
        SqlConnection con = Connection.Instance.GetConnection();
        SqlCommand cmd = new SqlCommand(sqlCommand, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@UserID", Settings.Instance.UserID);
        SqlParameter retParam = cmd.CreateParameter();
        retParam.Direction = ParameterDirection.ReturnValue;
        cmd.Parameters.Add(retParam);
        try
        {
            Connection.Instance.OpenConnection(con);
            cmd.ExecuteNonQuery();
            
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            Connection.Instance.CloseConnection(con);
        }
      
    }
    #endregion
    #endregion
    #region Insert Update Store Master
    public int InsertUpdateStore(string code, string name, string aed)
    {
        int result = 0;
        string sqlCommand = "sp_insertupdateStore";
        SqlConnection con = Connection.Instance.GetConnection();
        SqlCommand cmd = new SqlCommand(sqlCommand, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Code", code);
        cmd.Parameters.AddWithValue("@Name", name);
        cmd.Parameters.AddWithValue("@A_E_D", aed);
        cmd.Parameters.AddWithValue("@Usr_Id", Settings.Instance.UserID);
        cmd.Parameters.AddWithValue("@LogTime", DateTime.Now.ToUniversalTime().AddSeconds(19800));
        SqlParameter retParam = cmd.CreateParameter();
        retParam.Direction = ParameterDirection.ReturnValue;
        cmd.Parameters.Add(retParam);
        try
        {
            Connection.Instance.OpenConnection(con);
            cmd.ExecuteNonQuery();
            result = Convert.ToInt32(retParam.Value);
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

    #region Delete Store Master
    public int DeleteStore(string code)
    {
        int result = 0;
        string sqlCommand = "sp_deleteStore";
        SqlConnection con = Connection.Instance.GetConnection();
        SqlCommand cmd = new SqlCommand(sqlCommand, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Code", code);
        SqlParameter retParam = cmd.CreateParameter();
        retParam.Direction = ParameterDirection.ReturnValue;
        cmd.Parameters.Add(retParam);
        try
        {
            Connection.Instance.OpenConnection(con);
            cmd.ExecuteNonQuery();
            result = Convert.ToInt32(retParam.Value);
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

   


    #region Insert Update Permissions
    public int InsertUpdatePermission(string code, string userid, int pageid, bool view, bool add, bool edit, bool delete, bool export)
    {
        int result = 0;
        string sqlCommand = "sp_insertupdatePermissions";
        SqlConnection con = Connection.Instance.GetConnection();
        SqlCommand cmd = new SqlCommand(sqlCommand, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Store", code);
        cmd.Parameters.AddWithValue("@UserId", userid);
        cmd.Parameters.AddWithValue("@Page_Id", pageid);
        cmd.Parameters.AddWithValue("@ViewP", view);
        cmd.Parameters.AddWithValue("@AddP", add);
        cmd.Parameters.AddWithValue("@EditP", edit);
        cmd.Parameters.AddWithValue("@DeleteP", delete);
        cmd.Parameters.AddWithValue("@ExportP", export);
        cmd.Parameters.AddWithValue("@Usr_Id", Settings.Instance.UserID);
        cmd.Parameters.AddWithValue("@LogTime", DateTime.Now.ToUniversalTime().AddSeconds(19800));
        SqlParameter retParam = cmd.CreateParameter();
        retParam.Direction = ParameterDirection.ReturnValue;
        cmd.Parameters.Add(retParam);
        try
        {
            Connection.Instance.OpenConnection(con);
            cmd.ExecuteNonQuery();
            result = Convert.ToInt32(retParam.Value);
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

    #region Insert Update UserMaster
    public int InsertUpdateUser(int id, string name, string userid, string pwd, string store, string address, string mobile,string EmailID)
    {
        int result = 0;
        string sqlCommand = "sp_insertupdateUser";
        SqlConnection con = Connection.Instance.GetConnection();
        SqlCommand cmd = new SqlCommand(sqlCommand, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@Name", name);
        cmd.Parameters.AddWithValue("@UserId", userid);
        cmd.Parameters.AddWithValue("@Pwd", pwd);
        cmd.Parameters.AddWithValue("@Store", store);
        cmd.Parameters.AddWithValue("@Address", address);
        cmd.Parameters.AddWithValue("@Mobile", mobile);
        cmd.Parameters.AddWithValue("@EmailID", EmailID);
        SqlParameter retParam = cmd.CreateParameter();
        retParam.Direction = ParameterDirection.ReturnValue;
        cmd.Parameters.Add(retParam);
        try
        {
            Connection.Instance.OpenConnection(con);
            cmd.ExecuteNonQuery();
            result = Convert.ToInt32(retParam.Value);
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

    #region Insert Update PersonMaster
    public int InsertUpdatePerson(int id, string personname, string DeviceNo, string FromTime, string ToTime, string Interval, string code, string UploadInt, string RetryInt,string degree, Boolean Check, Boolean check1,char Sys_Flag,string Mobile, string SendAlarm,int AlarmMins,string SendSms,string srMobile,String SendSmsP,string SendSmsS,string Lat,string Long,string Empcode)
    {
        int result = 0;
        string sqlCommand = "sp_insertupdatePerson";
        SqlConnection con = Connection.Instance.GetConnection();
        SqlCommand cmd = new SqlCommand(sqlCommand, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@PName", personname);
        cmd.Parameters.AddWithValue("@DeviceNo", DeviceNo);
        cmd.Parameters.AddWithValue("@Ftime", FromTime);
        cmd.Parameters.AddWithValue("@TTime", ToTime);
        cmd.Parameters.AddWithValue("@Interval", Interval);
        cmd.Parameters.AddWithValue("@GrpCode", code);
        cmd.Parameters.AddWithValue("@UpInterval", UploadInt);
        cmd.Parameters.AddWithValue("@RetryInt", RetryInt);
        cmd.Parameters.AddWithValue("@Degree", degree);
        cmd.Parameters.AddWithValue("@GpsLoc", Check);
        cmd.Parameters.AddWithValue("@MobileLoc", check1);
        cmd.Parameters.AddWithValue("@Sys_Flag", Sys_Flag);
        cmd.Parameters.AddWithValue("@Mobile", Mobile);      
        cmd.Parameters.AddWithValue("@SendAlarm", SendAlarm);
        cmd.Parameters.AddWithValue("@AlarmMins", AlarmMins);
        cmd.Parameters.AddWithValue("@SendSms", SendSms);
        cmd.Parameters.AddWithValue("@srMobile", srMobile);
        cmd.Parameters.AddWithValue("@SendSmsP", SendSmsP);
        cmd.Parameters.AddWithValue("@SendSmsS", SendSmsS);
        cmd.Parameters.AddWithValue("@Lat", Lat);
        cmd.Parameters.AddWithValue("@Long", Long);
        cmd.Parameters.AddWithValue("@Empcode", Empcode);
        SqlParameter retParam = cmd.CreateParameter();
        retParam.Direction = ParameterDirection.ReturnValue;
        cmd.Parameters.Add(retParam);
        try
        {
            Connection.Instance.OpenConnection(con);
            cmd.ExecuteNonQuery();
            result = Convert.ToInt32(retParam.Value);
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
    public void UpdateDeviceNo(string OldDevice, string NewDeviceNo)
    {
        string sqlCommand = "sp_UpdateDeviceNo";
        SqlConnection con = Connection.Instance.GetConnection();
        SqlCommand cmd = new SqlCommand(sqlCommand, con);
        cmd.CommandTimeout = 0;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@OldDevice", OldDevice);
        cmd.Parameters.AddWithValue("@NewDeviceNo", NewDeviceNo);
        //SqlParameter retParam = cmd.CreateParameter();
        //retParam.Direction = ParameterDirection.ReturnValue;
        //cmd.Parameters.Add(retParam);
        try
        {
            Connection.Instance.OpenConnection(con);
            cmd.ExecuteNonQuery();
            //result = Convert.ToInt32(retParam.Value);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            Connection.Instance.CloseConnection(con);
        }
    }

    #region Insert Update GroupMapping
    public int InsertUpdateGroupMapping(int id, string personid, string groupid)
    {
        int result = 0;
        string sqlCommand = "sp_insertgroupMapping";
        SqlConnection con = Connection.Instance.GetConnection();
        SqlCommand cmd = new SqlCommand(sqlCommand, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@Personid", personid);
        cmd.Parameters.AddWithValue("@groupid", groupid);
        SqlParameter retParam = cmd.CreateParameter();
        retParam.Direction = ParameterDirection.ReturnValue;
        cmd.Parameters.Add(retParam);
        try
        {
            Connection.Instance.OpenConnection(con);
            cmd.ExecuteNonQuery();
            result = Convert.ToInt32(retParam.Value);
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
    #region Insert Update UserMapping
    public int InsertUserMapping(int id, string personid, string groupid)
    {
        int result = 0;
        string sqlCommand = "sp_insertUserMapping";
        SqlConnection con = Connection.Instance.GetConnection();
        SqlCommand cmd = new SqlCommand(sqlCommand, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@UserID", personid);
        cmd.Parameters.AddWithValue("@groupid", groupid);
        SqlParameter retParam = cmd.CreateParameter();
        retParam.Direction = ParameterDirection.ReturnValue;
        cmd.Parameters.Add(retParam);
        try
        {
            Connection.Instance.OpenConnection(con);
            cmd.ExecuteNonQuery();
            result = Convert.ToInt32(retParam.Value);
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

    #region Delete Person Master
    //public int DeletePerson(string code)
    //{
    //    int result = 0;
    //    string sqlCommand = "sp_deletePerson";
    //    SqlConnection con = Connection.Instance.GetConnection();
    //    SqlCommand cmd = new SqlCommand(sqlCommand, con);
    //    cmd.CommandType = CommandType.StoredProcedure;
    //    cmd.Parameters.AddWithValue("@Code", code);
    //    SqlParameter retParam = cmd.CreateParameter();
    //    retParam.Direction = ParameterDirection.ReturnValue;
    //    cmd.Parameters.Add(retParam);
    //    try
    //    {
    //        Connection.Instance.OpenConnection(con);
    //        cmd.ExecuteNonQuery();
    //        result = Convert.ToInt32(retParam.Value);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    finally
    //    {
    //        Connection.Instance.CloseConnection(con);
    //    }
    //    return result;
    //}
    public int DeletePerson(string code, string flag)
    {
        int result = 0;
        string sqlCommand = "sp_deletePerson";
        SqlConnection con = Connection.Instance.GetConnection();
        SqlCommand cmd = new SqlCommand(sqlCommand, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Code", code);
        cmd.Parameters.AddWithValue("@Flag", flag);
        SqlParameter retParam = cmd.CreateParameter();
        retParam.Direction = ParameterDirection.ReturnValue;
        cmd.Parameters.Add(retParam);
        try
        {
            Connection.Instance.OpenConnection(con);
            cmd.ExecuteNonQuery();
            result = Convert.ToInt32(retParam.Value);
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

    #region Send Email- When Person Added/Deleted
    public void SendDeletionEmail(string Name, string DeviceID,string GroupName)
    {
        WebServiceSoapClient DMT = new WebServiceSoapClient("WebServiceSoap");
        DMT.DeletePersonLicense(DeviceID);
        
        try
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<table style='width:100%; color: #000000; font: 12px/17px Verdana,Arial,Helvetica,sans-serif;'><tr><td colspan='2' style='width: 150px;'><strong>The following person has stopped using Dataman Tracker Services !</strong></td></tr><tr>");
            sb.AppendLine("<td>Name Mr/Ms : " + Name + " </td></tr><tr>");
            sb.AppendLine("<td>Device Number : " + DeviceID + " </td></tr><tr>");
            sb.AppendLine("<td>Comapany Name : " + GroupName + " </td></tr><tr>");
            sb.AppendLine("</table>");
            Common.SendMailMessage(System.Configuration.ConfigurationManager.AppSettings["EmailID"], "webadmin@datamannet.com", "", "", "Person Deleted From Dataman Tracker", sb.ToString().Trim());  
        }
        catch (Exception ex) { ex.ToString(); }
    }
    public void SendInsertionEmail(string Name, string DeviceID, string GroupName)
    {
     
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<table style='width:100%; color: #000000; font: 12px/17px Verdana,Arial,Helvetica,sans-serif;'><tr><td colspan='2' style='width: 150px;'><strong>The following person has started using Dataman Tracker Services !</strong></td></tr><tr>");
                sb.AppendLine("<td>Name Mr/Ms : " + Name + " </td></tr><tr>");
                sb.AppendLine("<td>Device Number : " + DeviceID + " </td></tr><tr>");
                sb.AppendLine("<td>Comapany Name : " + GroupName + " </td></tr><tr>");
                sb.AppendLine("</table>");
                Common.SendMailMessage(System.Configuration.ConfigurationManager.AppSettings["EmailID"], "webadmin@datamannet.com", "", "", "Person Added to Dataman Tracker", sb.ToString().Trim());
            }
            catch (Exception ex) { ex.ToString(); }
    }
    #endregion
    public bool AddPersonLicence(string Name, string DeviceID)
    {
        WebServiceSoapClient DMT = new WebServiceSoapClient("WebServiceSoap");
        string val = DMT.InsertPersonLicense(Settings.Instance.CompCode, DeviceID, Name, Settings.Instance.CompUrl);
        if (val =="1")
            return true;
        else
            return false;
    }
    public bool UpdatePersonLicence(string NewDeviceID,string OldDeviceId,string Name)
    {
         if (!string.IsNullOrEmpty(OldDeviceId) && !string.IsNullOrEmpty(NewDeviceID))
        {
            WebServiceSoapClient DMT = new WebServiceSoapClient("WebServiceSoap");
            string val = DMT.UpdatePersonLicense(Name, OldDeviceId, NewDeviceID);
            if (val == "1")
                return true;
            else
                return false;
        }
         else { return false; }
    }


    #endregion


    #region Delete Group Mapping
    public int DeleteGroupMapping(string groupid)
    {
        int result = 0;
        string sqlCommand = "delete from GrpMapp where GroupId = '" + groupid + "'";
        SqlConnection con = Connection.Instance.GetConnection();
        SqlCommand cmd = new SqlCommand(sqlCommand, con);
        try
        {
            Connection.Instance.OpenConnection(con);
            cmd.ExecuteNonQuery();
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

    #region Delete User Mapping
    public int DeleteUserMapping(string groupid)
    {
        int result = 0;
        string sqlCommand = "delete from UserGrp where UserID = '" + groupid + "'";
        SqlConnection con = Connection.Instance.GetConnection();
        SqlCommand cmd = new SqlCommand(sqlCommand, con);
        try
        {
            Connection.Instance.OpenConnection(con);
            cmd.ExecuteNonQuery();
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
    
    //DateTime.Now.ToUniversalTime().AddSeconds(19800)
    #region InsertUpdateFenceAlert
    public int InsertUpdateFenceAlert(string CLat, string CLong, string Radius, string Address, string PersonId,string groupId)
    {
        int result = 0;
        string sqlCommand = "sp_insertupdateFenceAlert";
        SqlConnection con = Connection.Instance.GetConnection();
        SqlCommand cmd = new SqlCommand(sqlCommand, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@CLat", CLat);
        cmd.Parameters.AddWithValue("@CLong", CLong);
        cmd.Parameters.AddWithValue("@Radius", Radius);
        cmd.Parameters.AddWithValue("@Address", Address);
        cmd.Parameters.AddWithValue("@PersonId",PersonId );
        cmd.Parameters.AddWithValue("@groupId", groupId);
        SqlParameter retParam = cmd.CreateParameter();
        retParam.Direction = ParameterDirection.ReturnValue;
        cmd.Parameters.Add(retParam);
        try
        {
            Connection.Instance.OpenConnection(con);
            cmd.ExecuteNonQuery();
            result = Convert.ToInt32(retParam.Value);
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

    #region InsertUpdateFenceAttendanceAlert
    public int InsertUpdateFenceAttendanceAlert(string CLat, string CLong, string Radius, string Address, string PersonId, string groupId)
    {
        int result = 0;
        string sqlCommand = "sp_insertupdateFenceAttendanceAlert";
        SqlConnection con = Connection.Instance.GetConnection();
        SqlCommand cmd = new SqlCommand(sqlCommand, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@CLat", CLat);
        cmd.Parameters.AddWithValue("@CLong", CLong);
        cmd.Parameters.AddWithValue("@Radius", Radius);
        cmd.Parameters.AddWithValue("@Address", Address);
        cmd.Parameters.AddWithValue("@PersonId", PersonId);
        cmd.Parameters.AddWithValue("@groupId", groupId);
        SqlParameter retParam = cmd.CreateParameter();
        retParam.Direction = ParameterDirection.ReturnValue;
        cmd.Parameters.Add(retParam);
        try
        {
            Connection.Instance.OpenConnection(con);
            cmd.ExecuteNonQuery();
            result = Convert.ToInt32(retParam.Value);
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

    #region Insert Update Product Type
    public int InsertUpdateProdType(string code, string type, string aed, string mobile, string MarkerData)
    {
        int result = 0;
        string sqlCommand = "sp_insertupdateProdType";
        SqlConnection con = Connection.Instance.GetConnection();
        SqlCommand cmd = new SqlCommand(sqlCommand, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Code", code);
        cmd.Parameters.AddWithValue("@Type", type);
        cmd.Parameters.AddWithValue("@A_E_D", aed);
        cmd.Parameters.AddWithValue("@Usr_Id", Settings.Instance.UserID);
        cmd.Parameters.AddWithValue("@LogTime", DateTime.Now.ToUniversalTime().AddSeconds(19800));
        cmd.Parameters.AddWithValue("@mobile", mobile);
        cmd.Parameters.AddWithValue("@MarkerData", MarkerData);
        SqlParameter retParam = cmd.CreateParameter();
        retParam.Direction = ParameterDirection.ReturnValue;
        cmd.Parameters.Add(retParam);
        try
        {
            Connection.Instance.OpenConnection(con);
            cmd.ExecuteNonQuery();
            result = Convert.ToInt32(retParam.Value);
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

    #region Delete Product Type Master
    public int DeleteProdType(string code, string aed)
    {
        int result = 0;
        string sqlCommand = "sp_deleteProdType";
        SqlConnection con = Connection.Instance.GetConnection();
        SqlCommand cmd = new SqlCommand(sqlCommand, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Code", code);
        cmd.Parameters.AddWithValue("@A_E_D", aed);
        cmd.Parameters.AddWithValue("@Usr_Id", Settings.Instance.UserID);
        cmd.Parameters.AddWithValue("@LogTime", DateTime.Now.ToUniversalTime().AddSeconds(19800));
        SqlParameter retParam = cmd.CreateParameter();
        retParam.Direction = ParameterDirection.ReturnValue;
        cmd.Parameters.Add(retParam);
        try
        {
            Connection.Instance.OpenConnection(con);
            cmd.ExecuteNonQuery();
            result = Convert.ToInt32(retParam.Value);
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

    #region Delete Store Permission
    public int DeleteUserPermission(string groupid)
    {
        int result = 0;
        string sqlCommand = "delete from storepermission where UserID = '" + groupid + "'";
        SqlConnection con = Connection.Instance.GetConnection();
        SqlCommand cmd = new SqlCommand(sqlCommand, con);
        try
        {
            Connection.Instance.OpenConnection(con);
            cmd.ExecuteNonQuery();
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
    #region Delete Co-Ordinates
    public void DeleteCoordinates(string Id)
    {
        string sqlCommand = "sp_deleteCoordinates";
        SqlConnection con = Connection.Instance.GetConnection();
        SqlCommand cmd = new SqlCommand(sqlCommand, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Id", Id);
        SqlParameter retParam = cmd.CreateParameter();
        retParam.Direction = ParameterDirection.ReturnValue;
        cmd.Parameters.Add(retParam);
        try
        {
            Connection.Instance.OpenConnection(con);
            cmd.ExecuteNonQuery();
           
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            Connection.Instance.CloseConnection(con);
        }
    }
    #endregion
    #region Insert Update Address new table
    public void UpdateAddress(string lat, string lng, string add)
    {

        string sqlCommand = "updateAdd";
        SqlConnection con = Connection.Instance.GetConnection();
        SqlCommand cmd = new SqlCommand(sqlCommand, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@lat", lat);
        cmd.Parameters.AddWithValue("@lng", lng);
        cmd.Parameters.AddWithValue("@address", add);


        try
        {
            Connection.Instance.OpenConnection(con);
            cmd.ExecuteNonQuery();
            //result = Convert.ToInt32(retParam.Value);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            Connection.Instance.CloseConnection(con);
        }

    }
    #endregion
    #region Insert Update Person Fence
    public int InsertUpdatePersonFence(int id, Int32 personid, Int32 fenceAddrID)
    {
        int result = 0;
        string sqlCommand = "sp_insertPersonFenceMapp";
        SqlConnection con = Connection.Instance.GetConnection();
        SqlCommand cmd = new SqlCommand(sqlCommand, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@personid", personid);
        cmd.Parameters.AddWithValue("@fenceAddrID", fenceAddrID);
        SqlParameter retParam = cmd.CreateParameter();
        retParam.Direction = ParameterDirection.ReturnValue;
        cmd.Parameters.Add(retParam);
        try
        {
            Connection.Instance.OpenConnection(con);
            cmd.ExecuteNonQuery();
            result = Convert.ToInt32(retParam.Value);
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
    #region Delete Person Fence Mapping
    public int DeletePersonFenceMaPP(int PersonId)
    {
        int result = 0;
        string sqlCommand = "sp_deletePersonFenceMaPP";
        SqlConnection con = Connection.Instance.GetConnection();
        SqlCommand cmd = new SqlCommand(sqlCommand, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@PersonId", PersonId);
        SqlParameter retParam = cmd.CreateParameter();
        retParam.Direction = ParameterDirection.ReturnValue;
        cmd.Parameters.Add(retParam);
        try
        {
            Connection.Instance.OpenConnection(con);
            cmd.ExecuteNonQuery();
            result = Convert.ToInt32(retParam.Value);
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
    #region Insert Update UserSettings
    public int InsertUpdateUserSettings(string UserId, string DispSignalType, string DispMoreDetails, int Accuracy, string EmailDailyDAR)
    {
        int result = 0;
        string sqlCommand = "sp_insertupdateUserSettings";
        SqlConnection con = Connection.Instance.GetConnection();
        SqlCommand cmd = new SqlCommand(sqlCommand, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@UserId", UserId);
        cmd.Parameters.AddWithValue("@DispSignalType", DispSignalType);
        cmd.Parameters.AddWithValue("@DispMoreDetails", DispMoreDetails);
        cmd.Parameters.AddWithValue("@Accuracy", Accuracy);
        cmd.Parameters.AddWithValue("@EmailDailyDAR", EmailDailyDAR);
        SqlParameter retParam = cmd.CreateParameter();
        retParam.Direction = ParameterDirection.ReturnValue;
        cmd.Parameters.Add(retParam);
        try
        {
            Connection.Instance.OpenConnection(con);
            cmd.ExecuteNonQuery();
            result = Convert.ToInt32(retParam.Value);
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
    #region Insert TrackerData
    public int UpdateTrackerData(int days)
    {
        int result = 0;
        string sqlCommand = "Sp_UpdateDMTrackerData";
        SqlConnection con = Connection.Instance.GetConnection();
        SqlCommand cmd = new SqlCommand(sqlCommand, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@addday", days);       
        SqlParameter retParam = cmd.CreateParameter();
        retParam.Direction = ParameterDirection.ReturnValue;
        cmd.Parameters.Add(retParam);
        try
        {
            Connection.Instance.OpenConnection(con);
            cmd.ExecuteNonQuery();
            result = Convert.ToInt32(retParam.Value);
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
}
