using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using DAL;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Web.Services;
using System.Configuration;

namespace AstralFFMS
{
    public partial class LastLivePosition : System.Web.UI.Page
    {
        string roleType = "";
        private static int SMID = 0;
        private static string Vdate;
        protected void Page_Load(object sender, EventArgs e)
        {
            string name = HttpUtility.UrlEncode(Encrypt("111111111111111"));
            string technology = HttpUtility.UrlEncode(Encrypt("22222222222222"));
            //Response.Redirect(string.Format("~/CS2.aspx?name={0}&technology={1}", name, technology));
            chkdiv();
            frmTextBox.Attributes.Add("readonly", "readonly");
            roleType = Settings.Instance.RoleType;
            if (!IsPostBack)
            {
                BindTreeViewControl();                
     
                {
                    GetPartyNames1();
                }
                if (!string.IsNullOrEmpty(Request.QueryString["smid"]))
                {                    
                    SMID = Convert.ToInt32(Request.QueryString["smid"]);
                    //GetPartyNames(SMID.ToString(), Convert.ToDateTime(frmTextBox.Text).ToString("yyyy/MM/dd"));
                }
                if (!string.IsNullOrEmpty(Request.QueryString["VDate"]))
                {
                    Vdate = Convert.ToString(Request.QueryString["VDate"]);                   
                }
                GetPartyNames(SMID.ToString(), Convert.ToDateTime(Vdate).ToString("yyyy/MM/dd"));

            }
            if (!Page.IsPostBack)
            {
                frmTextBox.Text = DateTime.UtcNow.Date.ToString("dd/MMM/yyyy");
            }
            string lblName = Decrypt(HttpUtility.UrlDecode(name));
            string lblTechnology = Decrypt(HttpUtility.UrlDecode(technology));
        }
        private void Page_PreRender(object sender, EventArgs e)
        {
            trview.Attributes.Add("OnClick", "client_OnTreeNodeChecked(event)");
        }
        private void BindTreeViewControl()
        {
            try
            {
                DataTable St = new DataTable();
                if (roleType == "Admin")
                {
                    //  St = Settings.UnderUsersforlevels(Settings.Instance.SMID, 1);
                    St = DbConnectionDAL.GetDataTable(CommandType.Text, "Select msr.smid,msr.Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname,msr.underid,msr.lvl from mastsalesrep msr  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.active=1 and msr.underid<>0 order by msr.smname");
                }
                else
                {
                    string query = "select msr.smid,msr.Smname +' ('+ msr.Syncid + ' - ' + mr.RoleName + ')' as smname,msr.underid ,msr.lvl from mastsalesrep msr  LEFT JOIN mastrole mr ON mr.RoleId=msr.RoleId where msr.smid in (select smid from MastSalesRepGrp where MainGrp  in (" + Settings.Instance.SMID + ")) and  msr.active=1 and msr.lvl >= (select distinct level from MastSalesRepGrp  where MainGrp in (" + Settings.Instance.SMID + " )) order by msr.smname";
                    St = DbConnectionDAL.GetDataTable(CommandType.Text, query);
                }
                //    DataSet ds = GetDataSet("Select smid,smname,underid,lvl from mastsalesrep where active=1 and underid<>0 order by smname");


                DataRow[] Rows = St.Select("lvl=MIN(lvl)"); // Get all parents nodes
                for (int i = 0; i < Rows.Length; i++)
                {
                    TreeNode root = new TreeNode(Rows[i]["smname"].ToString(), Rows[i]["smid"].ToString());
                    root.SelectAction = TreeNodeSelectAction.Expand;
                    root.CollapseAll();
                    CreateNode(root, St);
                    trview.Nodes.Add(root);
                }
            }
            catch (Exception Ex) { throw Ex; }
        }
        public void CreateNode(TreeNode node, DataTable Dt)
        {
            DataRow[] Rows = Dt.Select("underid =" + node.Value);
            if (Rows.Length == 0) { return; }
            for (int i = 0; i < Rows.Length; i++)
            {
                TreeNode Childnode = new TreeNode(Rows[i]["smname"].ToString(), Rows[i]["smid"].ToString());
                Childnode.SelectAction = TreeNodeSelectAction.Expand;
                node.ChildNodes.Add(Childnode);
                Childnode.CollapseAll();
                CreateNode(Childnode, Dt);
            }
        }
        private void chkdiv()
        {
            string Chk = Request.QueryString["I"];
            if (Chk == "True")
            {
                ContentPlaceHolder1_lblspname.Visible = true;
                ContentPlaceHolder1_lblspname.InnerText = "(SalesPerson Name)";
                divTohide.Visible = false;
            }
            else
            {
                divTohide.Visible = true;
                ContentPlaceHolder1_lblspname.Visible = false;
            }
        }

        public void GetPartyNames(string smid, string date)
        {
            try
            {
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                dt2.Columns.Add("SNo");
                dt2.Columns.Add("lat");
                dt2.Columns.Add("lng");
                dt2.Columns.Add("Title");
                dt2.Columns.Add("time");
                dt2.Columns.Add("address1");
                dt2.Columns.Add("contact");
                dt2.Columns.Add("type");
                dt2.Columns.Add("smid");
                dt2.Columns.Add("smname");
                dt2.Columns.Add("date");
                dt2.AcceptChanges();
                string[] smid_arr = smid.Split(',');

                for(int i=0;i<smid_arr.Length;i++)
                {
                    string strdate = " and t1.Lat_long_datetime between '" + Settings.dateformat(date) + " 00:00' and '" + Settings.dateformat(date) + " 23:59'";

                    string str = @"select rank() over (order by id) SNo,* from (
                     SELECT t1.Ordid as id, t1.Latitude as lat,t1.longitude as lng,mp.PartyName AS Title,t1.Lat_Long_datetime AS time,t1.Address AS address1,mp.PartyName AS contact,'O' AS type, msp.smname,msp.smid  FROM Temp_TransOrder t1 LEFT JOIN mastparty mp ON t1.PartyId=mp.PartyId left join mastsalesrep msp on msp.smid=t1.smid WHERE t1.SMId in (" + smid_arr[i] + ") " + strdate + " AND (t1.Latitude <> NULL or t1.Latitude <> '0.0') UNION ALL " +
                  "  SELECT t1.Ordid as id,t1.Latitude as lat,t1.longitude as lng,mp.PartyName AS Title,t1.Lat_Long_datetime AS time,t1.Address AS address1,mp.PartyName AS contact,'O' AS type , msp.smname,msp.smid   FROM TransOrder t1 LEFT JOIN mastparty mp ON t1.PartyId=mp.PartyId left join mastsalesrep msp on msp.smid=t1.smid WHERE t1.SMId in (" + smid_arr[i] + ") " + strdate + " AND (t1.Latitude <> NULL or t1.Latitude <> '0.0') UNION ALL " +
                  "  SELECT t1.Fvid as id,t1.Latitude as lat,t1.longitude as lng,mp.PartyName AS Title,t1.Lat_Long_datetime AS time,t1.Address AS address1,mp.PartyName AS contact,'F' AS type, msp.smname,msp.smid   FROM Temp_TransFailedVisit t1 LEFT JOIN mastparty mp ON t1.PartyId=mp.PartyId left join mastsalesrep msp on msp.smid=t1.smid WHERE t1.SMId in (" + smid_arr[i] + ") " + strdate + " AND (t1.Latitude <> NULL or t1.Latitude <> '0.0') UNION ALL " +
                  "  SELECT t1.FvID as id,t1.Latitude as lat,t1.longitude as lng,mp.PartyName AS Title,t1.Lat_Long_datetime AS time,t1.Address AS address1,mp.PartyName AS contact,'F' AS type, msp.smname,msp.smid   FROM TransFailedVisit t1 LEFT JOIN mastparty mp ON t1.PartyId=mp.PartyId left join mastsalesrep msp on msp.smid=t1.smid WHERE t1.SMId in (" + smid_arr[i] + ") " + strdate + " AND (t1.Latitude <> NULL or t1.Latitude <> '0.0') UNION ALL " +
                  "  SELECT t1.demoid as id, t1.Latitude as lat,t1.longitude as lng,mp.PartyName AS Title,t1.Lat_Long_datetime AS time,t1.Address AS address1,mp.PartyName AS contact,'D' AS type, msp.smname,msp.smid   FROM Temp_TransDemo t1 LEFT JOIN mastparty mp ON t1.PartyId=mp.PartyId left join mastsalesrep msp on msp.smid=t1.smid WHERE t1.SMId in (" + smid_arr[i] + ") " + strdate + " AND (t1.Latitude <> NULL or t1.Latitude <> '0.0') UNION ALL " +
                  "  SELECT t1.demoid as id, t1.Latitude as lat,t1.longitude as lng,mp.PartyName AS Title,t1.Lat_Long_datetime AS time,t1.Address AS address1,mp.PartyName AS contact,'D' AS type, msp.smname,msp.smid   FROM TransDemo t1 LEFT JOIN mastparty mp ON t1.PartyId=mp.PartyId left join mastsalesrep msp on msp.smid=t1.smid WHERE t1.SMId in (" + smid_arr[i] + ")  " + strdate + " AND (t1.Latitude <> NULL or t1.Latitude <> '0.0') " +
                    " ) t order by time ";

                    // dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str);

                    string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                     using (SqlConnection con = new SqlConnection(constr))
                     {
                         using (SqlCommand cmd = new SqlCommand("sp_LastLiveLocation"))
                         {
                             cmd.Connection = con;
                             cmd.Parameters.AddWithValue("@SMID", smid_arr[i]);
                             cmd.Parameters.AddWithValue("@DateTo", Settings.dateformat(date) + " 00:00");
                             cmd.Parameters.AddWithValue("@DateFrom", Settings.dateformat(date) + " 23:59");
                             cmd.Parameters.AddWithValue("@Last", '1');
                             cmd.CommandType = CommandType.StoredProcedure;
                             using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                             {
                                 dt1 = new DataTable();
                                 sda.Fill(dt1);
                             }
                         }
                     }
                     if (dt1.Rows.Count > 0)
                     {
                         dt2.Rows.Add(dt1.Rows[0]["SNo"].ToString(), dt1.Rows[0]["lat"].ToString(), dt1.Rows[0]["lng"].ToString(), dt1.Rows[0]["Title"].ToString(), dt1.Rows[0]["time"].ToString(), dt1.Rows[0]["address1"].ToString(), dt1.Rows[0]["contact"].ToString(), dt1.Rows[0]["type"].ToString(), dt1.Rows[0]["smid"].ToString(), dt1.Rows[0]["smname"].ToString(), Settings.dateformat(date));
                         dt2.AcceptChanges();
                     }
                     
                }

                if (dt2.Rows.Count > 0)
                {
                    rptMarkers1.DataSource = dt2;
                    rptMarkers1.DataBind();
                    leavereportrpt.DataSource = dt2;
                    leavereportrpt.DataBind();
                    Control FooterTemplate = leavereportrpt.Controls[leavereportrpt.Controls.Count - 1].Controls[0];
                    FooterTemplate.FindControl("Div1").Visible = false;
                }
                else
                {
                    GetPartyNames1();
                }

            }
            catch (Exception ex) { ex.ToString(); }
        }
        public void GetPartyNames1()
        {
            try
            {
                string getsmIDqry = @"select null SNo,null type,null Title,null address1,'80.3318736' lng,'26.449923 ' lat ,'00:00' time,null smid,null date,null smname";
                DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, getsmIDqry);
                DataTable dt = new DataTable();
                rptMarkers1.DataSource = dt1;
                rptMarkers1.DataBind();
                leavereportrpt.DataSource = dt;
                leavereportrpt.DataBind();
                {
                    Control FooterTemplate = leavereportrpt.Controls[leavereportrpt.Controls.Count - 1].Controls[0];
                    FooterTemplate.FindControl("dvNoRecords").Visible = true;
                    FooterTemplate.FindControl("Div1").Visible = false;
                }
            }
            catch (Exception ex) { ex.ToString(); }
        }
        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        protected void txt_fromdate_TextChanged(object sender, EventArgs e)
        {

        }
        protected void rptEmpDetails_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void leavereportrpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (leavereportrpt.Items.Count < 1)
            {
                if (e.Item.ItemType == ListItemType.Footer)
                {
                    HtmlGenericControl dvNoRec = e.Item.FindControl("dvNoRecords") as HtmlGenericControl;
                    if (dvNoRec != null)
                    {
                        dvNoRec.Visible = true;
                    }
                }
            }
        }

        protected void frmTextBox_TextChanged(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(ddlTSI.SelectedValue.ToString()))
            {
                string smIDStr1 = null;
                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr1 = node.Value;

                }
                string smid = smIDStr1;
                //GetPartyNames(smid, Convert.ToDateTime(frmTextBox.Text).ToString("yyyy/MM/dd"));
                //filldiv(smid, Convert.ToDateTime(frmTextBox.Text).ToString("yyyy-MM-dd"));
            }
        }


        protected void btngo_Click(object sender, EventArgs e)
        {
            string smIDStr1 = null;
            foreach (TreeNode node in trview.CheckedNodes)
            {
                smIDStr1 += node.Value + ",";
            }
            string smid = smIDStr1.TrimEnd(',');
            GetPartyNames(smid, Convert.ToDateTime(frmTextBox.Text).ToString("yyyy/MM/dd"));

        }
        public class Location
        {
            public Location()
            {
            }
            public string SNo
            { get; set; }
            public string lat
            { get; set; }
            public string lng
            { get; set; }
            public string Title
            { get; set; }
            public string time
            { get; set; }
            public string address1
            { get; set; }
            public string contact
            { get; set; }
            public string type
            { get; set; }
            public string smid
            { get; set; }
            public string date
            { get; set; }

        }
        [WebMethod]
        public string GetRouteByPerson(string smid, string date)
        {

            DataTable dtroutes = new DataTable();
            List<Location> list = new List<Location>();
            string strdate = " and t1.Lat_long_datetime between '" + Settings.dateformat(date) + " 00:00' and '" + Settings.dateformat(date) + " 23:59'";

            string str = @"select rank() over (order by id) SNo,* from (
                     SELECT t1.Ordid as id, t1.Latitude as lat,t1.longitude as lng,mp.PartyName AS Title,t1.Lat_Long_datetime AS time,t1.Address AS address1,mp.PartyName AS contact,'O' AS type FROM Temp_TransOrder t1 LEFT JOIN mastparty mp ON t1.PartyId=mp.PartyId WHERE t1.SMId in (" + smid + ") " + strdate + " AND (t1.Latitude <> NULL or t1.Latitude <> '0.0') UNION ALL " +
          "  SELECT t1.Ordid as id,t1.Latitude as lat,t1.longitude as lng,mp.PartyName AS Title,t1.Lat_Long_datetime AS time,t1.Address AS address1,mp.PartyName AS contact,'O' AS type FROM TransOrder t1 LEFT JOIN mastparty mp ON t1.PartyId=mp.PartyId WHERE t1.SMId in (" + smid + ") " + strdate + " AND (t1.Latitude <> NULL or t1.Latitude <> '0.0') UNION ALL " +
          "  SELECT t1.Fvid as id,t1.Latitude as lat,t1.longitude as lng,mp.PartyName AS Title,t1.Lat_Long_datetime AS time,t1.Address AS address1,mp.PartyName AS contact,'F' AS type FROM Temp_TransFailedVisit t1 LEFT JOIN mastparty mp ON t1.PartyId=mp.PartyId WHERE t1.SMId in (" + smid + ") " + strdate + " AND (t1.Latitude <> NULL or t1.Latitude <> '0.0') UNION ALL " +
          "  SELECT t1.FvID as id,t1.Latitude as lat,t1.longitude as lng,mp.PartyName AS Title,t1.Lat_Long_datetime AS time,t1.Address AS address1,mp.PartyName AS contact,'F' AS type FROM TransFailedVisit t1 LEFT JOIN mastparty mp ON t1.PartyId=mp.PartyId WHERE t1.SMId in (" + smid + ") " + strdate + " AND (t1.Latitude <> NULL or t1.Latitude <> '0.0') UNION ALL " +
          "  SELECT t1.demoid as id, t1.Latitude as lat,t1.longitude as lng,mp.PartyName AS Title,t1.Lat_Long_datetime AS time,t1.Address AS address1,mp.PartyName AS contact,'D' AS type FROM Temp_TransDemo t1 LEFT JOIN mastparty mp ON t1.PartyId=mp.PartyId WHERE t1.SMId in (" + smid + ") " + strdate + " AND (t1.Latitude <> NULL or t1.Latitude <> '0.0') UNION ALL " +
          "  SELECT t1.demoid as id, t1.Latitude as lat,t1.longitude as lng,mp.PartyName AS Title,t1.Lat_Long_datetime AS time,t1.Address AS address1,mp.PartyName AS contact,'D' AS type FROM TransDemo t1 LEFT JOIN mastparty mp ON t1.PartyId=mp.PartyId WHERE t1.SMId in (" + smid + ")  " + strdate + " AND (t1.Latitude <> NULL or t1.Latitude <> '0.0') " +
            " ) t order by time ";
            DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            for (int i = 0; i < dtroutes.Rows.Count; i++)
            {
                Location ob = new Location();
                ob.SNo = dtroutes.Rows[i]["Time"].ToString();
                ob.lat = dtroutes.Rows[i]["lat"].ToString();
                ob.lng = dtroutes.Rows[i]["lng"].ToString();
                ob.Title = dtroutes.Rows[i]["Time"].ToString();
                ob.time = dtroutes.Rows[i]["lat"].ToString();
                ob.address1 = dtroutes.Rows[i]["lng"].ToString();
                ob.contact = dtroutes.Rows[i]["Time"].ToString();
                ob.type = dtroutes.Rows[i]["lat"].ToString();
                list.Add(ob);
            }

            System.Web.Script.Serialization.JavaScriptSerializer obj = new System.Web.Script.Serialization.JavaScriptSerializer();
            return obj.Serialize(list);
        }
        public string DecryptString(string encrString)
        {
            byte[] b;
            string decrypted;
            try
            {
                b = Convert.FromBase64String(encrString);
                decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);
            }
            catch (FormatException fe)
            {
                decrypted = "";
            }
            return decrypted;
        }
    }
}