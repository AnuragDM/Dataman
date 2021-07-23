using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace FFMS.ClassFiles
{
    /// <summary>
    /// Summary description for DataBindServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class DataBindServices : System.Web.Services.WebService
    {
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
       //[WebMethod(EnableSession = true)]
       // public static string BindUsers()
       // {
       //     //ArrayList list = new ArrayList();
       //     //string qry = "select ID,Name from MastLogin where Active=1  order by Name";
       //     //DataTable dt = DataAccessLayer.DAL.getFromDataTable(qry);
       //     //if (dt.Rows.Count > 0)
       //     //{
       //     //    foreach (DataRow dr in dt.Rows)
       //     //    {
       //     //        list.Add(new ListItem(dr["Name"].ToString(), dr["Id"].ToString()));
       //     //    }
       //     //}
       //     //System.Web.Script.Serialization.JavaScriptSerializer obj = new System.Web.Script.Serialization.JavaScriptSerializer();
       //     //return obj.Serialize(list);

       // }
      
    }
}
