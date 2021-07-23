using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Web.Script.Serialization;
using DAL;
using BAL;
using AstralFFMS.ClassFiles;
//using DataAccessLayer;

/// <summary>
/// Summary description for FenceAddressMsg
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class FenceAddressMsg : System.Web.Services.WebService {
    UploadData Upd = new UploadData();
    static string Flong = "", Flat = "";
    #region GetSavePersonForFence Msg
    [WebMethod]
    public string GetAddressFence(string hdnlatlng)
    {
        string addr = "";
        try
        {
            string mlatLong = "";
            //-2 for spaces in between of brackets

            mlatLong = (hdnlatlng.Substring(1, hdnlatlng.Length - 2));
            string[] latlong = mlatLong.Split(',');
            if (latlong.Length > 0)
            {
                latlong[1] = latlong[1].ToString().Substring(1, latlong[1].Length - 2);
                if (latlong[0].ToString().Length > 8)
                    Flat = latlong[0].ToString().Trim().Substring(0, 7);
                else
                    Flat = latlong[0].ToString().Trim();
                if (latlong[1].ToString().Length > 8)
                    Flong = latlong[1].ToString().Substring(0, 7).TrimStart();
                else
                    Flong = latlong[1].ToString().Trim();
               // ServiceReferenceDMTracker.WebServiceSoapClient DMT = new ServiceReferenceDMTracker.WebServiceSoapClient("WebServiceSoap");
             //   addr = DMT.InsertAddress(Flat, Flong);

            }

        }
        catch (Exception ex) { ex.ToString(); }
        return addr;
    }
    [WebMethod]
    public string GetPerson(string GroupId)
    {
        string strq = "select PersonMaster.ID,PersonMaster.PersonName from GrpMapp inner join PersonMaster on PersonMaster.ID = GrpMapp.PersonID where GrpMapp.GroupID = '" + GroupId + "' order by PersonMaster.PersonName";
        DataTable dtper = new DataTable();
        dtper = DbConnectionDAL.GetDataTable(CommandType.Text, strq);
        List<Persons> personlist = new List<Persons>();
        if (dtper.Rows.Count > 0)
        {
            for (int i = 0; i < dtper.Rows.Count; i++)
            {
                Persons objst = new Persons();
                objst.ID = Convert.ToInt32(dtper.Rows[i]["ID"]);
                objst.Name = Convert.ToString(dtper.Rows[i]["PersonName"]);
                personlist.Insert(i, objst);
            }
        }
        JavaScriptSerializer jscript = new JavaScriptSerializer();
        return jscript.Serialize(personlist);
    }
    public class Persons
    {

        public int ID { get; set; }

        public string Name { get; set; }

    }
    [WebMethod]
    public string SaveFenceAddressMsg(string Fradius, string Faddr, string FPersonId, string FgroupId)
    {
        try
        {
            Fradius = Math.Round((Convert.ToDecimal(Fradius)), 3).ToString();
            Upd.InsertUpdateFenceAlert(Flat, Flong, Fradius, Faddr, FPersonId, FgroupId);
            
            //string sql = "insert into FenceAddressMsg(FDate,CLat,CLong,Radius,Address,PersonId,groupId,Flag)" +
            //    "values('" + DateTime.Now.AddSeconds(19800) + "','" + Flat + "','" + Flong + "'," + Fradius + ",'" + Faddr.Trim() + "','" + FPersonId + "','"+ FgroupId+"','0')";
           // DataAccessLayer.DAL.ExecuteQuery(sql);
        }
        catch (Exception ex) { ex.ToString(); }
        return "Upadated Successfully";
    }
    #endregion
}
