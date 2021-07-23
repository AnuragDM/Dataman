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

namespace AstralFFMS
{
    public partial class LivePosition : System.Web.UI.Page
    {
        string roleType = "";
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
                //if (!string.IsNullOrEmpty(ddlTSI.SelectedValue.ToString()))
                //{
                //    string smid = ddlTSI.SelectedValue.ToString();
                //    GetPartyNames(smid, DateTime.Now.ToString("yyyy-MM-dd"));
                //    filldiv(smid, DateTime.Now.ToString("yyyy-MM-dd"));
                //}
                //else
                {
                    GetPartyNames1();
                }
               
                //SetValues();
                //CalendarExtender5.SelectedDate = DateTime.Now;

            }
             if (!Page.IsPostBack)
             {
                 frmTextBox.Text = DateTime.UtcNow.Date.ToString("dd/MMM/yyyy");
             }
            //string lblName = Decrypt(HttpUtility.UrlDecode(Request.QueryString["name"]));
            //string lblTechnology = Decrypt(HttpUtility.UrlDecode(Request.QueryString["technology"]));
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
                   
                    string strdate = " and t1.Lat_long_datetime between '" + Settings.dateformat(date) + " 00:00' and '" + Settings.dateformat(date) + " 23:59'";
                    
                    string str = @"select rank() over (order by id) SNo,* from (
                     SELECT t1.Ordid as id, t1.Latitude as lat,t1.longitude as lng,mp.PartyName AS Title,t1.Lat_Long_datetime AS time,t1.Address AS address1,mp.PartyName AS contact,'O' AS type FROM Temp_TransOrder t1 LEFT JOIN mastparty mp ON t1.PartyId=mp.PartyId WHERE t1.SMId=" + smid + " " + strdate + " AND (t1.Latitude <> NULL or t1.Latitude <> '0.0') UNION ALL " +
                  "  SELECT t1.Ordid as id,t1.Latitude as lat,t1.longitude as lng,mp.PartyName AS Title,t1.Lat_Long_datetime AS time,t1.Address AS address1,mp.PartyName AS contact,'O' AS type FROM TransOrder t1 LEFT JOIN mastparty mp ON t1.PartyId=mp.PartyId WHERE t1.SMId=" + smid + " " + strdate + " AND (t1.Latitude <> NULL or t1.Latitude <> '0.0') UNION ALL " +
                  "  SELECT t1.Fvid as id,t1.Latitude as lat,t1.longitude as lng,mp.PartyName AS Title,t1.Lat_Long_datetime AS time,t1.Address AS address1,mp.PartyName AS contact,'F' AS type FROM Temp_TransFailedVisit t1 LEFT JOIN mastparty mp ON t1.PartyId=mp.PartyId WHERE t1.SMId=" + smid + " " + strdate + " AND (t1.Latitude <> NULL or t1.Latitude <> '0.0') UNION ALL " +
                  "  SELECT t1.FvID as id,t1.Latitude as lat,t1.longitude as lng,mp.PartyName AS Title,t1.Lat_Long_datetime AS time,t1.Address AS address1,mp.PartyName AS contact,'F' AS type FROM TransFailedVisit t1 LEFT JOIN mastparty mp ON t1.PartyId=mp.PartyId WHERE t1.SMId=" + smid + " " + strdate + " AND (t1.Latitude <> NULL or t1.Latitude <> '0.0') UNION ALL " +
                  "  SELECT t1.demoid as id, t1.Latitude as lat,t1.longitude as lng,mp.PartyName AS Title,t1.Lat_Long_datetime AS time,t1.Address AS address1,mp.PartyName AS contact,'D' AS type FROM Temp_TransDemo t1 LEFT JOIN mastparty mp ON t1.PartyId=mp.PartyId WHERE t1.SMId=" + smid + "  " + strdate + " AND (t1.Latitude <> NULL or t1.Latitude <> '0.0') UNION ALL " +
                  "  SELECT t1.demoid as id, t1.Latitude as lat,t1.longitude as lng,mp.PartyName AS Title,t1.Lat_Long_datetime AS time,t1.Address AS address1,mp.PartyName AS contact,'D' AS type FROM TransDemo t1 LEFT JOIN mastparty mp ON t1.PartyId=mp.PartyId WHERE t1.SMId=" + smid + "  " + strdate + " AND (t1.Latitude <> NULL or t1.Latitude <> '0.0') " +
                    " ) t order by time ";
                    DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                    if (dt1.Rows.Count > 0)
                    {
                        rptMarkers1.DataSource = dt1;
                        rptMarkers1.DataBind();
                        leavereportrpt.DataSource = dt1;
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
                string getsmIDqry = @"select null SNo,null type,null Title,null address1,'80.3318736' lng,'26.449923 ' lat ,'00:00' time";  
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
        public void filldiv(string smid, string date)
        {
        }

        protected void btngo_Click(object sender, EventArgs e)
        {
                string smIDStr1 = null;
                foreach (TreeNode node in trview.CheckedNodes)
                {
                    smIDStr1 = node.Value;
                }
                string smid = smIDStr1;
                GetPartyNames(smid, Convert.ToDateTime(frmTextBox.Text).ToString("yyyy/MM/dd"));
                filldiv(smid, Convert.ToDateTime(frmTextBox.Text).ToString("yyyy-MM-dd"));            
        }
    }
}