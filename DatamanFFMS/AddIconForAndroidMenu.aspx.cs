using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BAL;
using System.Reflection;
using BusinessLayer;
using DAL;
using System.IO;
using System.Data.SqlClient;

namespace AstralFFMS
{
    public partial class AddIconForAndroidMenu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if(!IsPostBack)
            {
                fillMeetType();
            }
        }

        private void fillMeetType()
        {
            string query = "select pagename,pageid from mastpage  where level_idx<>1  and module='android' order by pageid asc ";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, query);
            if (dt.Rows.Count > 0)
            {
                ddlform.DataSource = dt;
                ddlform.DataTextField = "pagename";
                ddlform.DataValueField = "pageid";
                ddlform.DataBind();
            }
            ddlform.Items.Insert(0, new ListItem("-- Select --", "0"));
           
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {  string filePath = FileUpload1.PostedFile.FileName;
                string filename = Path.GetFileName(filePath);
                string ext = Path.GetExtension(filename);
                string contenttype = String.Empty;

                //Set the contenttype based on File Extension
                
                if (contenttype != String.Empty)
                {

                    Stream fs = FileUpload1.PostedFile.InputStream;
                    BinaryReader br = new BinaryReader(fs);
                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);

                    //insert the file into database
                    string strQuery = "update mastpage set MenuIcon=@Data where pageid=" + ddlform.SelectedValue + "";

                    SqlCommand cmd = new SqlCommand(strQuery);
                    cmd.Parameters.Add("@Data", SqlDbType.Binary).Value = bytes;
                   // DbConnectionDAL.ExecuteNonQuery(CommandType.Text, strQuery);
                    String strConnString = System.Configuration.ConfigurationManager
                .ConnectionStrings["ConnectionString"].ConnectionString;
                    SqlConnection con = new SqlConnection(strConnString);
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                        cmd.ExecuteNonQuery();


                }
            }
        }
    }
}