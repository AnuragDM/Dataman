using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AstralFFMS
{
    public partial class Downloads : System.Web.UI.Page
    {
         string roleType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { //Ankita - 20/may/2016- (For Optimization)
               // GetRoleType(Settings.Instance.RoleID);
                roleType = Settings.Instance.RoleType;
                GetDownloadsData();
            }
        }
        //private void GetRoleType(string p)
        //{
        //    try
        //    {
        //        string roleqry = @"select * from MastRole where RoleId=" + Convert.ToInt32(p) + "";
        //        DataTable roledt = DbConnectionDAL.GetDataTable(CommandType.Text, roleqry);
        //        if (roledt.Rows.Count > 0)
        //        {
        //            roleType = roledt.Rows[0]["RoleType"].ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}
        private void GetDownloadsData()
        {
            try
            {
                string uploadeqry = string.Empty;
                if (roleType == "Distributor")
                {
                    uploadeqry = @"select Id, Title, Linkurl from UploadDocuments where Active=1 and (DocFor='Distributor' or DocFor='Both')
                    union all select Id, Title, Linkurl from UploadDocuments where Active=1 and (DocFor='Specific Distributor') and distid like '%" + Settings.Instance.DistributorID + "%'";
                }
                else
                {
                    uploadeqry = @"select Id, Title, Linkurl from UploadDocuments where Active=1 and (DocFor='Sales Team' or DocFor='Both') 
                    union all select Id, Title, Linkurl from UploadDocuments where Active=1 and (DocFor='Specific SalesPerson') and smids like '%" + Settings.Instance.SMID + "%'";
                }

                DataTable upDT = DbConnectionDAL.GetDataTable(CommandType.Text, uploadeqry);
                if (upDT.Rows.Count > 0)
                {
                    downloadrpt.DataSource = upDT;
                    downloadrpt.DataBind();
                }
                else
                {
                    downloadrpt.DataSource = upDT;
                    downloadrpt.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void lnkdownload_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                var item = (RepeaterItem)btn.NamingContainer;
                HiddenField hdnVisItCode = (HiddenField)item.FindControl("linkHiddenField");
                HiddenField hdnId = (HiddenField)item.FindControl("HiddenField1");
                //            string filepath = Server.MapPath("UploadDocuments" + hdnVisItCode.Value).Replace("~","");
                string filepath1 = hdnVisItCode.Value.ToString().Replace("~", "").Replace("/", "");
                string filepath = filepath1.Insert(0, hdnId.Value + "-");
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + filepath + "\"");
                Response.TransmitFile(Server.MapPath("~/UploadDocuments/" + filepath));
                Response.End();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
    }
}