using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AstralFFMS
{
    public partial class CRMTask : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (!Page.IsPostBack)
            {
             //   BindSalePersons();
            }
        }
        private void BindSalePersons()
        {
            //try
            //{
            //    DataTable dt = Settings.UnderUsers(Settings.Instance.SMID);
            //    DataView dv = new DataView(dt);
            //    dv.RowFilter = "SMName<>.";
            //    if (dv.ToTable().Rows.Count > 0)
            //    {
            //        lstowners.DataSource = dv.ToTable();
            //        lstowners.DataTextField = "SMName";
            //        lstowners.DataValueField = "SMId";
            //        lstowners.DataBind();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ex.ToString();
            //}

        }

        //protected void btnUpload_Click(object sender, EventArgs e)
        //{
        //    HttpFileCollection hfc = Request.Files;
        //    string FileNamePath = "";
        //    for (int i = 0; i < hfc.Count; i++)
        //    {
        //        HttpPostedFile hpf = hfc[i];
        
        //            string filename = hpf.FileName;
        //            String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //            filename = Settings.Instance.SMID + '-' + timeStamp + '-' + filename;
        //            string FilePath = Server.MapPath("~/CRM_UploadFile/" + System.IO.Path.GetFileName(filename));
        //            FileNamePath += FilePath + ",";
        //            hpf.SaveAs(FilePath);

               
        //        FileNamePath = FileNamePath.TrimEnd(',');
        //    }
        //    hidfilepath.Value = FileNamePath;
        //}

    }
}