using DAL;
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
using System.IO;
using System.Web.Services;
using System.Web.Script.Services;
using System.Collections;
using System.Net;
using Newtonsoft.Json;

namespace AstralFFMS
{
    public partial class ChatWithUsers : System.Web.UI.Page
    {
        int Tosmid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            txtmsz.Focus();
            Page.MaintainScrollPositionOnPostBack = true;
            FielUpload1();
            if (!IsPostBack)
            {
                FillContacts(string.Empty);
               
            }
         
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp", "<script type='text/javascript'>ScrollToBottom();</script>", false);
        }
        
        protected void FillChat(string smid)
        {
            //To Fill Chat per User     
        
            string urlfrom = "../dist/img/user1-128x128.jpg";
            string urlto = "../dist/img/user3-128x128.jpg";
            //string sql = @"select cm.*,case fsmid when " + Settings.Instance.SMID + " then 'pull-left' else 'pull-right'  end as mszDirection,case when mszread is null  then 'Customfocus' when  msz =( select top 1 msz from tbl_chatmodule where (tsmid = " + Convert.ToInt32(Settings.Instance.SMID) + " or tsmid=" + Convert.ToInt32(smid) + ")and  (Fsmid = " + Convert.ToInt32(Settings.Instance.SMID) + " or fsmid=" + Convert.ToInt32(smid) + ") order by CreatedDate desc) then 'Customfocus' else '' end as focusmsz,case fsmid when " + Settings.Instance.SMID + " then 'pull-right' else 'pull-left'  end as dateDirection,case fsmid when " + Settings.Instance.SMID + " then '" + urlfrom + "' else '" + urlto + "'  end as ImgUrl,case fsmid when " + Settings.Instance.SMID + " then 'bg-light-blue' else 'bg-success'  end as MszBackColor,(CAST(DATEDIFF(second, '1970-01-01 05:30:00', CAST(cm.createddate AS date)) AS bigint)*1000) + DATEDIFF(ms, CAST(cm.createddate AS date), cm.createddate) AS [Milliseconds],msp.smname as fsmname,msp1.smname as tsmname from tbl_ChatModule cm left join mastsalesrep msp on msp.smid=cm.fsmid left join mastsalesrep msp1 on msp1.smid=cm.tsmid where (tsmid = " + Convert.ToInt32(Settings.Instance.SMID) + " or tsmid=" + Convert.ToInt32(smid) + ")and  (Fsmid = " + Convert.ToInt32(Settings.Instance.SMID) + " or fsmid=" + Convert.ToInt32(smid) + ") order by [Milliseconds] ";
            string sql = @"select cm.*,case [FileY/N] when 'Y' then 'displaynone' else 'DisplayBlock'  end as MszLinkVisible,case [FileY/N] when 'Y' then 'DisplayBlock'  else 'displaynone'  end as LinkVisible,case fsmid when " + Settings.Instance.SMID + " then 'pull-left' else 'pull-right'  end as mszDirection,case when mszread is null  then 'Customfocus' when  msz =( select top 1 msz from tbl_chatmodule where (tsmid = " + Convert.ToInt32(Settings.Instance.SMID) + " or tsmid=" + Convert.ToInt32(smid) + ")and  (Fsmid = " + Convert.ToInt32(Settings.Instance.SMID) + " or fsmid=" + Convert.ToInt32(smid) + ") order by CreatedDate desc) then 'Customfocus' else '' end as focusmsz,case fsmid when " + Settings.Instance.SMID + " then 'pull-right' else 'pull-left'  end as dateDirection,case fsmid when " + Settings.Instance.SMID + " then '" + urlfrom + "' else '" + urlto + "'  end as ImgUrl,case fsmid when " + Settings.Instance.SMID + " then 'bg-light-blue' else 'bg-success'  end as MszBackColor,(CAST(DATEDIFF(second, '1970-01-01 05:30:00', CAST(cm.createddate AS date)) AS bigint)*1000) + DATEDIFF(ms, CAST(cm.createddate AS date), cm.createddate) AS [Milliseconds],msp.smname as fsmname,msp1.smname as tsmname from tbl_ChatModule cm left join mastsalesrep msp on msp.smid=cm.fsmid left join mastsalesrep msp1 on msp1.smid=cm.tsmid where (tsmid = " + Convert.ToInt32(Settings.Instance.SMID) + " or tsmid=" + Convert.ToInt32(smid) + ")and  (Fsmid = " + Convert.ToInt32(Settings.Instance.SMID) + " or fsmid=" + Convert.ToInt32(smid) + ") order by [Milliseconds] ";
            DataTable dtchat = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            if (dtchat.Rows.Count > 0)
            {
                rpt.DataSource = dtchat;
                rpt.DataBind();
                string updateReadmsz = "update tbl_chatmodule set mszread='W' where Tsmid=" + Settings.Instance.SMID + " and Fsmid=" + Convert.ToInt32(smid) + " and mszread is null";
                DbConnectionDAL.ExecuteQuery(updateReadmsz);
            }
            else
            {
                rpt.DataSource = null;
                rpt.DataBind();
            }
         
           
        }
        protected void FillContacts(string sname)
        {
            //To Fill Contacts Either In General Way Or In Search Modes
            gridcontacts.DataSource = null;
            gridcontacts.DataBind();
            string filter = "";
            if (!string.IsNullOrEmpty(sname.Trim()))
                filter = "and smname like '%" + sname + "%'";
            string sql = "  select *,case  when unreadmsz is Null then 'box-tools pull-right' else '' end as mszunreaddivClass from View_ForChatUnderUser vcu left join (select count (msz) as unreadmsz,fsmid from tbl_chatmodule where tsmid=" + Convert.ToInt32(Settings.Instance.SMID) + " and mszread is null group by fsmid) as tblunread on tblunread.fsmid=vcu.smid  where smid in  (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Convert.ToInt32(Settings.Instance.SMID) + "))  and  level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Convert.ToInt32(Settings.Instance.SMID) + " )) union(select maingrp from MastSalesRepGrp where smid=" + Convert.ToInt32(Settings.Instance.SMID) + ")) and  Active=1 and SMName<>'.' " + filter + "  and smid<>" + Convert.ToInt32(Settings.Instance.SMID) + "";
            DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            if (dt1.Rows.Count > 0)
            {
                gridcontacts.DataSource = dt1;
                gridcontacts.DataBind();
            }
            else
            {
                gridcontacts.DataSource = null;
                gridcontacts.DataBind();
            }
            if(!string.IsNullOrEmpty(Convert.ToString(Session["Sname"])))
            LblTo.Text = Session["Sname"].ToString(); 
            if ((string.IsNullOrEmpty(sname.Trim())) && (string.IsNullOrEmpty(Convert.ToString(Session["Tosmid"]))))
            {
                FillChat(dt1.Rows[0]["smid"].ToString());//to open top contact's chat window
                tomszname.Text = dt1.Rows[0]["salesperson"].ToString();
                LblTo.Text = dt1.Rows[0]["salesperson"].ToString();
             
            }

        }
 
        protected void Btnsend_Click(object sender, EventArgs e)
        {
            string insertsql = @"INSERT INTO [dbo].[tbl_ChatModule]  ([Fsmid] ,[Tsmid] ,[Msz])
                                   VALUES  (" + Convert.ToInt32(Settings.Instance.SMID) + "," + Convert.ToInt32(1532) + ",'" + txtmsz.Value + "')";
            DbConnectionDAL.ExecuteQuery(insertsql);
        }
        public void FillDefaultContacts()
        {
            string sql = "  select *,case  when unreadmsz is Null then 'box-tools pull-right' else '' end as mszunreaddivClass from View_ForChatUnderUser vcu left join (select count (msz) as unreadmsz,fsmid from tbl_chatmodule where tsmid=" + Convert.ToInt32(Settings.Instance.SMID) + " and mszread is null group by fsmid) as tblunread on tblunread.fsmid=vcu.smid  where smid in  (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Convert.ToInt32(Settings.Instance.SMID) + "))  and  level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Convert.ToInt32(Settings.Instance.SMID) + " )) union(select maingrp from MastSalesRepGrp where smid=" + Convert.ToInt32(Settings.Instance.SMID) + ")) and  Active=1 and SMName<>'.'  and smid<>" + Convert.ToInt32(Settings.Instance.SMID) + "";
            DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            gridcontacts.DataSource = dt1;
            gridcontacts.DataBind();
        }
        protected void txtsearchcontacts_TextChanged(object sender, EventArgs e)
        {
            FillContacts(txtsearchcontacts.Text.Trim());
        }


        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Click":
                    {
                        string Smid = e.CommandArgument.ToString();
                        Tosmid = Convert.ToInt32(Smid);
                        FillChat(Smid);
                        Session["Tosmid"] = Smid;
                        string smname = DbConnectionDAL.GetStringScalarVal("select smname from mastsalesrep where smid=" + Tosmid + "").ToString();
                        Session["Sname"] = smname;
                        LblTo.Text = Session["Sname"].ToString(); tomszname.Text = Session["Sname"].ToString(); 
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp", "<script type='text/javascript'>ScrollToBottom();</script>", false); 
                        break;
                    }
                default:
                    break;
            }
        }

        protected void btnimgsend_Click(object sender, ImageClickEventArgs e)
        {
            if (txtmsz.Value.Trim() != "")
            {
                using (WebClient client = new WebClient())
                {
                    string Wpath = string.Empty;
                    string host = DbConnectionDAL.GetStringScalarVal("select compurl from mastenviro").ToString();
                    string url = "http://" + host + "/And_Sync.asmx/xjsSendChat_chatmodule?tsmid=" + Session["Tosmid"].ToString() + "&fsmid=" + Convert.ToInt32(Settings.Instance.SMID) + "&msz=" + txtmsz.Value + "&localpath=" + Wpath + "";
                    string json = client.DownloadString(url).Replace(@"""", "");
                    if (json == "Y")
                    {
                        // msz = "Y";
                    }

                }
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp", "<script type='text/javascript'>ScrollToBottom();</script>", false); 
            txtmsz.Value = string.Empty;
            FillChat(Session["Tosmid"].ToString());
        }

        protected void GetTime(object sender, EventArgs e)
        {
           
            FillChat(Session["Tosmid"].ToString());
        }
        [WebMethod(EnableSession = true)]
        public static string GetContacts(string sname)
        {
            string filter = "";
            if (!string.IsNullOrEmpty(sname.Trim()))
                filter = "and smname like '%" + sname + "%'";
            string sql = "  select *,case  when unreadmsz is Null then 'box-tools pull-right' else '' end as mszunreaddivClass from View_ForChatUnderUser vcu left join (select count (msz) as unreadmsz,fsmid from tbl_chatmodule where tsmid=" + Convert.ToInt32(Settings.Instance.SMID) + " and mszread is null group by fsmid) as tblunread on tblunread.fsmid=vcu.smid  where smid in  (select distinct smid from MastSalesRepGrp where smid in (select smid from MastSalesRepGrp where MainGrp in (" + Convert.ToInt32(Settings.Instance.SMID) + "))  and  level>= (select distinct level from MastSalesRepGrp where MainGrp in (" + Convert.ToInt32(Settings.Instance.SMID) + " )) union(select maingrp from MastSalesRepGrp where smid=" + Convert.ToInt32(Settings.Instance.SMID) + ")) and  Active=1 and SMName<>'.' " + filter + "  and smid<>" + Convert.ToInt32(Settings.Instance.SMID) + "";
            DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
            return JsonConvert.SerializeObject(dt1);
        }

        protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
       e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.ItemIndex == rpt.Items.Count)
                {
                    // this repeater item refers to the last record
                    e.Item.Focus();
                }
            }
        }

       public void FielUpload()
        {
            HttpPostedFile file = Request.Files["myFile"]; string host = DbConnectionDAL.GetStringScalarVal("select compurl from mastenviro").ToString();
            string thumburl = null; string video = null;
            if (IsPostBack && file != null)
            {

                if (file.FileName.Length > 0)
                {

                    String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff"); int counter = 0; string fileeditedname = file.FileName;
                    string filePath = Server.MapPath("~/ChatAttachments/" + file.FileName);

                    for (int i = 0; i < 20; i++)
                    {

                        if (File.Exists(filePath))
                        {

                            string add = "(" + counter + i + ")";
                            string ext = Path.GetExtension(filePath);
                            string t = Path.GetFileNameWithoutExtension(filePath);
                            fileeditedname = add + file.FileName;
                            filePath = Server.MapPath("~/ChatAttachments/" + fileeditedname);
                            //  FileInfo fno = new FileInfo("c:\\x.name.txt");
                            // MessageBox.Show(fno.Name.Replace(fno.Extension,""));
                            continue;
                        }
                        else
                        {
                            Save("~/ChatAttachments/" + fileeditedname);
                            file.SaveAs(filePath);
                            break;

                        }
                       
                    }
                    //   File1.PostedFile.SaveAs(strDestPath);
                    //   imguser.ImageUrl = "~/Images/" + upload1.PostedFile.FileName;
                }
            }
        }
       public void FielUpload1()
       {
           string host = DbConnectionDAL.GetStringScalarVal("select compurl from mastenviro").ToString();
           string thumburl = null; string video = null;
           if (IsPostBack && upload1.PostedFile != null)
           {

               if (upload1.PostedFile.FileName.Length > 0)
               {

                   String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff"); int counter = 0; string fileeditedname = upload1.PostedFile.FileName;
                   string filePath = Server.MapPath("~/ChatAttachments/" + upload1.PostedFile.FileName);

                   for (int i = 0; i < 20; i++)
                   {

                       if (File.Exists(filePath))
                       {

                           string add = "(" + counter + i + ")";
                           string ext = Path.GetExtension(filePath);
                           string t = Path.GetFileNameWithoutExtension(filePath);
                           fileeditedname = add + upload1.PostedFile.FileName;
                           filePath = Server.MapPath("~/ChatAttachments/" + fileeditedname);
                           //  FileInfo fno = new FileInfo("c:\\x.name.txt");
                           // MessageBox.Show(fno.Name.Replace(fno.Extension,""));
                           continue;
                       }
                       else
                       {
                          
                           upload1.PostedFile.SaveAs(filePath);
                           Save("ChatAttachments"+fileeditedname);
                           break;

                       }
                     
                   }
                   //   File1.PostedFile.SaveAs(strDestPath);
                   //   imguser.ImageUrl = "~/Images/" + upload1.PostedFile.FileName;
               }
           }
       }
       private void Save(string path)
       {
           using (WebClient client = new WebClient())
           {

               string host = DbConnectionDAL.GetStringScalarVal("select compurl from mastenviro").ToString();
               string Wpath = "W";
                 string url = "http://" + host + "/And_Sync.asmx/xjsSendChat_chatmodule?tsmid=" + Session["Tosmid"].ToString() + "&fsmid=" + Convert.ToInt32(Settings.Instance.SMID) + "&msz=" + path + "&localpath=" + Wpath + "";
              // string url = "http://localhost:20549/And_Sync.asmx/xjsSendChat_chatmodule?tsmid=" + Session["Tosmid"].ToString() + "&fsmid=" + Convert.ToInt32(Settings.Instance.SMID) + "&msz=" + path + "&localpath=" + Wpath + "";
               // string url = "http://localhost:20549/And_Sync.asmx/xjsSendChat_chatmodule?tsmid=" + Session["Tosmid"].ToString() + "&fsmid=" + Convert.ToInt32(Settings.Instance.SMID) + "&msz=" + path + "";
               string json = client.DownloadString(url).Replace(@"""", "");
               if (json == "Y")
               {
                   // msz = "Y";
               }

           }

       }

       protected void LinkButton1_Command(object sender, CommandEventArgs e)
       {
           if (e.CommandName == "Click")
           {

               string filename = e.CommandArgument.ToString();

               if (filename != "")
               {
                   try
                   {
                       System.IO.FileInfo file = new System.IO.FileInfo(Server.MapPath(filename));
                       if (file.Exists)
                       {
                           //Response.Clear();
                           //Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                           //Response.AddHeader("Content-Length", file.Length.ToString());
                           //Response.ContentType = "application/octet-stream";
                           //Response.WriteFile(file.FullName);
                           //string d = Server.MapPath("~/ChatAttachments/" + file.Name);
                           //Response.TransmitFile(Server.MapPath("~/ChatAttachments/" + file.Name));
                           //Response.End();
                           Response.AddHeader("Content-Disposition", "attachment;filename=\"" + file.Name + "\"");
                           Response.TransmitFile(Server.MapPath("~/ChatAttachments/" + file.Name));
                           Response.End();
                       }
                       else
                       {
                           Response.Write("This file does not exist.");
                       }
                   }
                   // string path = Server.MapPath(filename);
                   catch (Exception ex)
                   {
                       ex.ToString();
                   }
               }
           }
       }
    }
}