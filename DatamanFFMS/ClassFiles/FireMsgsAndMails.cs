using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
//using DataAccessLayer;
using BusinessLayer;
using System.Data;
using System.Configuration;
using DAL;
using BAL;
/// <summary>
/// Summary description for FireMsgsAndMails
/// </summary>
public class FireMsgsAndMails
{
    SMSAdapter adapter = new SMSAdapter();
	public void FireMsgWhenGPSOff()
    {
     try {
            string qry = "SELECT DISTINCT pm.PersonName,pm.mobile," +
"(SELECT Name FROM UserMast WHERE UserId=ug.UserID) AS UserName,(SELECT Mobile FROM UserMast WHERE UserId=ug.UserID) AS Mobile" +
 " FROM Log_Tracker Lg LEFT JOIN PersonMaster Pm ON Lg.DeviceNo=pm.DeviceNo LEFT JOIN GrpMapp Gm ON Gm.PersonID=Pm.ID" +
" LEFT JOIN UserGrp Ug ON Gm.GroupID=Ug.GroupID WHERE Lg.Status='GO' AND Lg.CurrentDate >=convert(varchar, dateadd(hour, -2, dateadd(ss,19800,getdate())), 100)" +
"AND Lg.CurrentDate <= dateadd(ss,19800,getdate()) GROUP BY ug.UserID,pm.PersonName,lg.CurrentDate,gm.GroupID,pm.mobile";
            DataTable dt = new DataTable();
            dt = DbConnectionDAL.getFromDataTable(qry);
            if (dt.Rows.Count > 0)
            {
                string sms = "",msg="",user="";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    
                    msg = "Dear," + user + " the following people cannot be tracked accurately since their GPS is Off:";
                    if (user == dt.Rows[i]["UserName"].ToString())
                    {
                        msg += dt.Rows[i]["Person"].ToString();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(user))
                        {

                           // adapter.sendSms("",msg);
                        }
                    }
                    user = dt.Rows[i]["UserName"].ToString();

                   
                }
                //msg += "Kindly ask them to set their GPS Mode On";
                //msg += "Thanks & Regards";
                //msg += "Dataman Web Team";
             
            }
        }
        catch (Exception ex) { ex.ToString(); }
	
	}
    

}