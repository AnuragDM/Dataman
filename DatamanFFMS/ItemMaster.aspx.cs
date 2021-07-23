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
using Newtonsoft.Json;
using System.Web.Services;
using System.Web.Script.Services;
using System.Text;


namespace AstralFFMS
{
    public partial class ItemMaster : System.Web.UI.Page
    {
        BAL.ItemBAL IB =new BAL.ItemBAL();
        string parameter = "";
        bool _exportp = false; 
        protected void Page_Load(object sender, EventArgs e)
        {
            parameter = Request["__EVENTARGUMENT"];
            if (parameter != "")
            {
                ViewState["ItemId"] = parameter;
                FillItemControls(Convert.ToInt32(parameter));
                mainDiv.Style.Add("display", "block");
                rptmain.Style.Add("display", "none");
            }
            //Ankita - 20/may/2016- (For Optimization)
            string pageName = Path.GetFileName(Request.Path);          
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            string PermAll = Settings.Instance.CheckPagePermissions(pageName, Convert.ToString(Session["user_name"]));
            string[] SplitPerm = PermAll.Split(',');
            btnExport.CssClass = "btn btn-primary";
            btnExport.Visible = Convert.ToBoolean(SplitPerm[4]);
            _exportp = Convert.ToBoolean(SplitPerm[4]);
            if (btnSave.Text == "Save")
            {
                //btnSave.Enabled = Settings.Instance.CheckAddPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[1]);
                btnSave.CssClass = "btn btn-primary";
            }
            else
            {
                btnSave.Enabled = Convert.ToBoolean(SplitPerm[2]);
              //  btnSave.Enabled = Settings.Instance.CheckEditPermission(pageName, Convert.ToString(Session["user_name"]));
                btnSave.CssClass = "btn btn-primary";
            }
            btnDelete.Enabled = Convert.ToBoolean(SplitPerm[3]);
            //btnDelete.Enabled = Settings.Instance.CheckDeletePermission(pageName, Convert.ToString(Session["user_name"]));
            btnDelete.CssClass = "btn btn-primary";
            if (!IsPostBack)
            {
                chkIsAdmin.Checked = true;
                btnDelete.Visible = false;
                
                mainDiv.Style.Add("display", "block");
                if (Request.QueryString["ItemId"] != null)
                {
                    FillItemControls(Convert.ToInt32(Request.QueryString["ItemId"]));
                }
                else { //BindSegments(0); BindClass(0); BindParent(0);
                }
            }
        }
        #region BindDropdowns
        private void BindSegments(int SegmentId)
        {  //Ankita - 20/may/2016- (For Optimization)
            string strSegment = "";
            if(SegmentId > 0)
            //strSegment = "select * from MastItemSegment where (Active='1' or Id in ("+SegmentId+")) Order By Name";
                strSegment = "select Id,Name from MastItemSegment where (Active='1' or Id in (" + SegmentId + ")) Order By Name";
            else
           // strSegment = "select * from MastItemSegment  where Active='1' Order By Name";
                strSegment = "select Id,Name from MastItemSegment  where Active='1' Order By Name";
            fillDDLDirect(ddlMastSegment, strSegment, "Id", "Name", 1);
        }
        private void BindClass(int ClassId)
        {//Ankita - 20/may/2016- (For Optimization)
            string strClass = "";
            if (ClassId > 0)
                //strClass = "select * from MastItemClass where (Active='1' or Id in (" + ClassId + ")) Order By Name";
                strClass = "select Id,Name from MastItemClass where (Active='1' or Id in (" + ClassId + ")) Order By Name";
            else
                //strClass = "select * from MastItemClass where Active='1' Order By Name";
                strClass = "select Id,Name from MastItemClass where Active='1' Order By Name";
            fillDDLDirect(ddlMastClass, strClass, "Id", "Name", 1);
        }
        private void BindParent(Int32 ParentId)
        {
            string strParent = "";
            if(ParentId > 0)
                strParent = "select ItemId,ItemName from MastItem where ItemType='MATERIALGROUP' and (Active='1' or ItemId in (" + ParentId + ")) Order By ItemName";
            else
                strParent = "select ItemId,ItemName from MastItem where ItemType='MATERIALGROUP' and Active='1' Order By ItemName";
            fillDDLDirect(ddlUnderItem, strParent, "ItemId", "ItemName", 1);
        }
        public static void fillDDLDirect(DropDownList xddl, string xmQry, string xvalue, string xtext, int sele)
        {
            DataTable xdt = new DataTable();
            xdt = DbConnectionDAL.GetDataTable(CommandType.Text, xmQry);
            xddl.DataSource = xdt;
            xddl.DataTextField = xtext.Trim();
            xddl.DataValueField = xvalue.Trim();
            xddl.DataBind();
            if (sele == 1)
            {
                if (xdt.Rows.Count >= 1)
                    xddl.Items.Insert(0, new ListItem("--Select--", "0"));
                else if (xdt.Rows.Count == 0)
                    xddl.Items.Insert(0, new ListItem("---", "0"));
            }
            else if (sele == 2)
            {
                xddl.Items.Insert(0, new ListItem("--Others--", "0"));
            }
            xdt.Dispose();
        }
        #endregion
        private void fillRepeter()
        {
          string str = @"select Mi.Itemid,Mi.ItemName,Mi.ItemCode,Mic.Name as ProductClass,Mi.PriceGroup,Mis.Name as Segment,Mi.SyncId,Mi.unit,Mi.Mrp,Mi.Rp,Mi.StdPack,Mi.Dp,Mi2.ItemName as MaterialGroup,case Mi.Active when 1 then 'Yes' else 'No' end as Active from MastItem Mi inner join MastItem Mi2 on Mi.underId=Mi2.ItemId left join MastItemClass Mic on Mi.ClassId=Mic.Id left Join MastItemSegment Mis on Mi.SegmentId=Mis.Id  where Mi.ItemType='ITEM' and Mi.ItemName<>'.'";
            DataTable Itemdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);


            str = @"SELECT Id, Itemid, ImgUrl, ThumbnailImgUrl FROM ItemMastImage";
            DataTable Itemimagesdt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            string imagehtml = "";
            Itemdt.Columns.Add("Imagehtml");
            for (int i = 0; i < Itemdt.Rows.Count; i++)
            {
                imagehtml = "";
                DataRow[] Drs = Itemimagesdt.Select("Itemid=" + Itemdt.Rows[i]["Itemid"].ToString() + "");
                DataTable itemimagebyiddt=new DataTable();
                if (Drs.Count()>0)
                {
                    itemimagebyiddt= Itemimagesdt.Select("Itemid=" + Itemdt.Rows[i]["Itemid"].ToString() + "").CopyToDataTable();
                }
          //     DataTable 
                //DataView dv = new DataView(Itemimagesdt);
                //dv.RowFilter = "Itemid In (" + Itemdt.Rows[i]["Itemid"].ToString() + ")" ;
               for (int j = 0; j < itemimagebyiddt.Rows.Count; j++)
                {
                    imagehtml = imagehtml + "<img   src='" + itemimagebyiddt.Rows[j]["ImgUrl"].ToString().Replace(@"~", string.Empty) + "' style='cursor: pointer;' width='30' height='30' onclick='Showpic(this.src)'/>";
                }
                Itemdt.Rows[i]["Imagehtml"] = imagehtml;
            }



            rpt.DataSource = Itemdt;
            rpt.DataBind();
        }
        private void FillItemControls(int ItemId)
        {
            try
            {
                string Itemquery = @"select * from MastItem where ItemId=" + ItemId;

                DataTable ItemValueDt = DbConnectionDAL.GetDataTable(CommandType.Text, Itemquery);
                if (ItemValueDt.Rows.Count > 0)
                {
                    ItemName.Value = ItemValueDt.Rows[0]["ItemName"].ToString();
                    SyncId.Value = ItemValueDt.Rows[0]["SyncId"].ToString();
                    HiddenUnit.Value = ItemValueDt.Rows[0]["unit"].ToString();
                    ddlunit.SelectedValue = ItemValueDt.Rows[0]["unit"].ToString();

                    HiddenPriUnit.Value = ItemValueDt.Rows[0]["PrimaryUnit"].ToString();
                    ddlprimaryunit.SelectedValue = ItemValueDt.Rows[0]["PrimaryUnit"].ToString();

                    HiddenSecUnit.Value = ItemValueDt.Rows[0]["SecondaryUnit"].ToString();
                    ddlsecondaryunit.SelectedValue = ItemValueDt.Rows[0]["SecondaryUnit"].ToString();
                    txtprimarycon.Value = ItemValueDt.Rows[0]["PrimaryUnitfactor"].ToString();
                    txtSecondarycon.Value = ItemValueDt.Rows[0]["SecondaryUnitfactor"].ToString();

                    txtminimumorderquantity.Value = ItemValueDt.Rows[0]["MOQ"].ToString();

                    MRP.Value = ItemValueDt.Rows[0]["mrp"].ToString();
                    Itemcode.Value = ItemValueDt.Rows[0]["ItemCode"].ToString();
                    RP.Value = ItemValueDt.Rows[0]["Rp"].ToString();

                   // BindParent(Convert.ToInt32(ItemValueDt.Rows[0]["UnderId"]));
                    HiddenGroupID.Value = ItemValueDt.Rows[0]["UnderId"].ToString();
                    ddlUnderItem.SelectedValue = ItemValueDt.Rows[0]["UnderId"].ToString();
                    StdPack.Value = ItemValueDt.Rows[0]["StdPack"].ToString();

                   // BindClass(Convert.ToInt32(ItemValueDt.Rows[0]["ClassId"]));
                    HiddenClassID.Value = ItemValueDt.Rows[0]["ClassId"].ToString();
                    ddlMastClass.SelectedValue = ItemValueDt.Rows[0]["ClassId"].ToString();
                    DP.Value = ItemValueDt.Rows[0]["Dp"].ToString();
                  //  BindSegments(Convert.ToInt32(ItemValueDt.Rows[0]["SegmentId"]));
                    HiddenSegmentID.Value = ItemValueDt.Rows[0]["SegmentId"].ToString();
                    ddlMastSegment.SelectedValue = ItemValueDt.Rows[0]["SegmentId"].ToString();
                    PriceGroup.Value = ItemValueDt.Rows[0]["PriceGroup"].ToString();
                    txtcgstper.Value = ItemValueDt.Rows[0]["CentralTaxPer"].ToString();
                    txtsgstper.Value = ItemValueDt.Rows[0]["StateTaxPer"].ToString();
                    txtigstper.Value = ItemValueDt.Rows[0]["IntegratedTaxPer"].ToString();
                    if (Convert.ToBoolean(ItemValueDt.Rows[0]["Active"]) == true)
                    {
                        chkIsAdmin.Checked = true;
                    }
                    else
                    {
                        chkIsAdmin.Checked = false;
                    }

                    if (Convert.ToBoolean(ItemValueDt.Rows[0]["Promoted"]) == true)
                    {
                        chkispromoted.Checked = true;
                    }
                    else
                    {
                        chkispromoted.Checked = false;
                    }
                    btnSave.Text = "Update";
                    btnDelete.Visible = true;
                    string str1 = @"select * from ItemMastImage where Itemid='" + ItemId + "'";

                    DataTable compimages = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
                    int k = 1;
                    string strhtml = "";
                    if (compimages.Rows.Count > 0)
                    {
                        for (int j = 0; j < compimages.Rows.Count; j++)
                        {
                            k = j + 1;
                            //    strhtml = strhtml + "<div  class='row' id= 'div_" + k + "' style='margin-top:10px'><input type='button'  id= '" + compimages.Rows[j]["Id"].ToString() + "' class='btn btn-primary'  value='Remove' onclick='Remove(" + k + ",this.id,-1)' style='margin-right:10px'/><img   src='" + compimages.Rows[j]["ImgUrl"].ToString().Replace(@"~", string.Empty) + "'  width= 60  height= 60 onclick='Showpic(this.src)'/></div>";
                            //if (complValueDt.Rows[0]["statusname"].ToString() != "Pending")
                            //{
                            //    strhtml = strhtml + "<div  class='row' id= 'div_" + k + "' style='margin-top:10px'><a  id= '" + compimages.Rows[j]["Id"].ToString() + "'  onclick='Remove(" + k + ",this.id,-1)' style='margin-right:10px'></a><img   src='" + compimages.Rows[j]["ImgUrl"].ToString().Replace(@"~", string.Empty) + "'  width= 60  height= 60 onclick='Showpic(this.src)'/></div>";
                            //}
                            //else
                            //{
                            strhtml = strhtml + "<div  class='col-md-2' id= 'div_" + k + "' style='margin-top:10px'><img   src='" + compimages.Rows[j]["ImgUrl"].ToString().Replace(@"~", string.Empty) + "'  width= 60  height= 60 onclick='Showpic(this.src)'/><a  id= '" + compimages.Rows[j]["Id"].ToString() + "'  onclick='Remove(" + k + ",this.id,-1)' style='margin-left:10%'> <i class='fa fa-trash-o' style='font-size: large;'></i></a></div>";
                            //}
                        }

                        divimsg.InnerHtml = strhtml;
                    }
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while showing the records');", true);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnSave.Text == "Update")
                {
                    UpdateItem();
                }
                else
                {
                    InsertItem();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Error while inserting the records');", true);
            }
        }
        protected bool ValidateImageSize(long contentlength)
        {
            long fileSize = contentlength;
            //Limit size to approx 2mb for image
            if ((fileSize > 0 & fileSize < 1048576))
            {
                return true;
            }
            else
            {
                //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please upload image of upto 1mb size only');", true);                
                return false;
            }
        }

        private void InsertItem()
        {
            string active = "0",promoted="0", GroupID = Request.Form[HiddenGroupID.UniqueID], ClassID = Request.Form[HiddenClassID.UniqueID], SegmentID = Request.Form[HiddenSegmentID.UniqueID], primaryunit = Request.Form[HiddenPriUnit.UniqueID], unit = Request.Form[HiddenUnit.UniqueID], secondaryunit = Request.Form[HiddenSecUnit.UniqueID];
            if (chkIsAdmin.Checked)
                active="1";
                  if (chkispromoted.Checked)
                promoted = "1";

                  decimal AMRP = 0, ADP = 0, ARP = 0, primaryunitfactor = 0, secondaryunitfactor = 0, mimimunqty = 0, cgstper = 0, sgstper = 0, igstper = 0;
			
			  string[] imgurls =new string[5];
              string[] thumburl = new string[5];
                string imgurl = "";
			  if (hidimg.Value != "")
            {
                string[] imges = hidimg.Value.Split(new string[] { "@#dataman$&" }, StringSplitOptions.None);
                imgurls = new string[imges.Length];
                //   Filenames = new string[imges.Length];
                for (int i = 0; i < imges.Length; i++)
                {



                    byte[] bytes = Convert.FromBase64String(imges[i].Split(',')[1]);

                    System.Drawing.Image image;
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        bool k = ValidateImageSize(ms.Length);
                        if (k != true)
                        {
                            hidimg.Value = "";
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please upload image of upto 1mb size only');", true);
                            return;
                        }
                        image = System.Drawing.Image.FromStream(ms);

                    }
                    string directoryPath = Server.MapPath(string.Format("~/{0}/", "ProductImages"));
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    // String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    string filename = Path.GetFileName(Convert.ToInt32(Settings.Instance.SMID) + '-' + timeStamp);
                    {
                        string filePath = Server.MapPath("~/ProductImages" + "/" + ItemName.Value + "_" + filename);
                        //comImgFileUpload.SaveAs(Server.MapPath("~/ProductImages" + "/C_" + filename + ".png"));
                        File.WriteAllBytes(filePath + ".png", bytes);
                        imgurl = "~/ProductImages" + "/" + ItemName.Value + "_" + filename + ".png";
                        imgurls[i] = imgurl;

                        // FileUpload1.SaveAs(MapPath("~/video/" + FileUpload1.FileName));
                        System.Drawing.Image img1 = System.Drawing.Image.FromFile(filePath + ".png");
                        System.Drawing.Image bmp1 = img1.GetThumbnailImage(35, 35, null, IntPtr.Zero);
                        bmp1.Save(Server.MapPath("~/ThumbnailProductImage/") + ItemName.Value + "_" + filename + ".png");
                        //NormalImage.ImageUrl = "~/video/" + FileUpload1.FileName;
                        // ThumbnailImageS.ImageUrl = "~/ThumbnailImage/" + filename;
                        thumburl[i] = "~/ThumbnailProductImage/" + ItemName.Value + "_" + filename + ".png";
                    }



                }
            }
            else
            {
                imgurls = new string[0];
            }
            imgurl = "";


			
            if (!string.IsNullOrEmpty(MRP.Value))
                AMRP = Convert.ToDecimal(MRP.Value);
            if (!string.IsNullOrEmpty(DP.Value))
                ADP = Convert.ToDecimal(DP.Value);
            if (!string.IsNullOrEmpty(RP.Value))
                ARP = Convert.ToDecimal(RP.Value);

            if (!string.IsNullOrEmpty(txtprimarycon.Value))
                primaryunitfactor = Convert.ToDecimal(txtprimarycon.Value);
            if (!string.IsNullOrEmpty(txtSecondarycon.Value))
                secondaryunitfactor = Convert.ToDecimal(txtSecondarycon.Value);
            if (!string.IsNullOrEmpty(txtminimumorderquantity.Value))
                mimimunqty = Convert.ToDecimal(txtminimumorderquantity.Value);

            if (!string.IsNullOrEmpty(txtcgstper.Value))
                cgstper = Convert.ToDecimal(txtcgstper.Value);
            if (!string.IsNullOrEmpty(txtsgstper.Value))
                sgstper = Convert.ToDecimal(txtsgstper.Value);
            if (!string.IsNullOrEmpty(txtigstper.Value))
                igstper = Convert.ToDecimal(txtigstper.Value);
            int retval = IB.InsertItems(GroupID, ItemName.Value, Itemcode.Value, unit, active, StdPack.Value, SyncId.Value, AMRP, ADP, ARP, ClassID, SegmentID, PriceGroup.Value, primaryunit, secondaryunit, primaryunitfactor, secondaryunitfactor, mimimunqty, promoted, cgstper, sgstper, igstper);
            if (retval == -1)
            {

                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Item Exists');", true);
                ItemName.Focus();
            }
            else if (retval == -2)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate SyncId Exists');", true);
                       SyncId.Focus();
            }
            else
            {

                if (imgurls.Length > 0)
                {
                    for (int i = 0; i < imgurls.Length; i++)
                    {
                        string insertquery = @"INSERT INTO [dbo].[ItemMastImage]  ([Itemid] ,[imgurl],[ThumbnailImgUrl],[Createddate],[Created_smid]) values(" + retval + ",'" + imgurls[i] + "','" + thumburl[i] + "',Getdate()," + Settings.Instance.SMID + ")";
                        DbConnectionDAL.ExecuteQuery(insertquery);
                    }
                }
                if (SyncId.Value == "")
                {
                    string syncid = "update MastItem set SyncId='" + retval + "' where Itemid=" + retval + "";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                }
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfully');", true);
                ClearControls();
                ItemName.Focus();
            }
          
        }
        private void UpdateItem()
        {
            string active = "0",promoted="0", GroupID = Request.Form[HiddenGroupID.UniqueID], ClassID = Request.Form[HiddenClassID.UniqueID], SegmentID = Request.Form[HiddenSegmentID.UniqueID],primaryunit = Request.Form[HiddenPriUnit.UniqueID], unit = Request.Form[HiddenUnit.UniqueID], secondaryunit = Request.Form[HiddenSecUnit.UniqueID];
            if (chkIsAdmin.Checked)
                active = "1";
                 if (chkispromoted.Checked)
                promoted = "1";


            decimal AMRP = 0, ADP = 0, ARP = 0,primaryunitfactor=0,secondaryunitfactor=0,mimimunqty=0,cgstper=0,sgstper=0,igstper=0;
            if (!string.IsNullOrEmpty(MRP.Value))
                AMRP = Convert.ToDecimal(MRP.Value);
            if (!string.IsNullOrEmpty(DP.Value))
                ADP = Convert.ToDecimal(DP.Value);
            if (!string.IsNullOrEmpty(RP.Value))
                ARP = Convert.ToDecimal(RP.Value);



            if (!string.IsNullOrEmpty(txtprimarycon.Value))
                primaryunitfactor = Convert.ToDecimal(txtprimarycon.Value);
            if (!string.IsNullOrEmpty(txtSecondarycon.Value))
                secondaryunitfactor = Convert.ToDecimal(txtSecondarycon.Value);
            if (!string.IsNullOrEmpty(txtminimumorderquantity.Value))
                mimimunqty = Convert.ToDecimal(txtminimumorderquantity.Value);

            if (!string.IsNullOrEmpty(txtcgstper.Value))
                cgstper = Convert.ToDecimal(txtcgstper.Value);
            if (!string.IsNullOrEmpty(txtsgstper.Value))
                sgstper = Convert.ToDecimal(txtsgstper.Value);
            if (!string.IsNullOrEmpty(txtigstper.Value))
                igstper = Convert.ToDecimal(txtigstper.Value);

            string imgurl = "";
            string[] imgurls = new string[5];
            string[] thumburl = new string[5];
			 if (hidimg.Value != "")
                {
                    string[] imges = hidimg.Value.Split(new string[] { "@#dataman$&" }, StringSplitOptions.None);
                    imgurls = new string[imges.Length];
                    //   Filenames = new string[imges.Length];
                    for (int i = 0; i < imges.Length; i++)
                    {


                     
                        byte[] bytes = Convert.FromBase64String(imges[i].Split(',')[1]);

                        System.Drawing.Image image;
                        using (MemoryStream ms = new MemoryStream(bytes))
                        {
                            bool k = ValidateImageSize(ms.Length);
                            if (k != true)
                            {
                                hidimg.Value = "";
                                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Please upload image of upto 1mb size only');", true);
                                return;
                            }
                            image = System.Drawing.Image.FromStream(ms);

                        }
                        string directoryPath = Server.MapPath(string.Format("~/{0}/", "ProductImages"));
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }
                        // String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        string filename = Path.GetFileName(Convert.ToInt32(Settings.Instance.SMID) + '-' + timeStamp);
                        {
                            string filePath = Server.MapPath("~/ProductImages" + "/" + ItemName.Value.Replace("/", " ") + "_" + filename);
                            //comImgFileUpload.SaveAs(Server.MapPath("~/ProductImages" + "/C_" + filename + ".png"));
                            File.WriteAllBytes(filePath + ".png", bytes);
                            imgurl = "~/ProductImages" + "/" + ItemName.Value.Replace("/", " ") + "_" + filename + ".png";
                            imgurls[i] = imgurl;

                            // FileUpload1.SaveAs(MapPath("~/video/" + FileUpload1.FileName));
                            System.Drawing.Image img1 = System.Drawing.Image.FromFile(filePath + ".png");
                            System.Drawing.Image bmp1 = img1.GetThumbnailImage(35, 35, null, IntPtr.Zero);
                            bmp1.Save(Server.MapPath("~/ThumbnailProductImage/") + ItemName.Value.Replace("/", " ") + "_" + filename + ".png");
                            //NormalImage.ImageUrl = "~/video/" + FileUpload1.FileName;
                            // ThumbnailImageS.ImageUrl = "~/ThumbnailImage/" + filename;
                            thumburl[i] = "~/ThumbnailProductImage/" + ItemName.Value.Replace("/", " ") + "_" + filename + ".png";
                        }



                    }
                }
                else
                {
                    imgurls = new string[0];
                }
                imgurl = "";
                int retval = IB.UpdateItems(Convert.ToInt32(ViewState["ItemId"]), GroupID, ItemName.Value, Itemcode.Value, unit, active, StdPack.Value, SyncId.Value, AMRP, ADP, ARP, ClassID, SegmentID, PriceGroup.Value, primaryunit, secondaryunit,primaryunitfactor,secondaryunitfactor,mimimunqty, promoted,cgstper,sgstper,igstper);
            if (retval == -1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate Item Exists');", true);
                ItemName.Focus();
            }
            else if (retval == -2)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Duplicate SyncId Exists');", true);
                SyncId.Focus();
            }
            else
            {
                if (imgurls.Length > 0)
                {
                    for (int i = 0; i < imgurls.Length; i++)
                    {
                        string insertquery = @"INSERT INTO [dbo].[ItemMastImage]  ([Itemid] ,[imgurl],[ThumbnailImgUrl],[Createddate],[Created_smid]) values(" + Convert.ToInt32(ViewState["ItemId"]) + ",'" + imgurls[i] + "','" + thumburl[i] + "',Getdate()," + Settings.Instance.SMID + ")";
                        DbConnectionDAL.ExecuteQuery(insertquery);
                    }
                }
                if (SyncId.Value == "")
                {
                    string syncid = "update MastItem set SyncId='" + retval + "' where Itemid=" + Convert.ToInt32(ViewState["ItemId"]) + "";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, syncid);
                }
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Updated Successfully');", true);
                ClearControls();
                ItemName.Focus();
                btnDelete.Visible = false;
                btnSave.Text = "Save";
            }
            
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            Response.Redirect("~/ItemMaster.aspx");
        }

        private void ClearControls()
        {
            ItemName.Value = string.Empty;
            HiddenPriUnit.Value = string.Empty;
            HiddenUnit.Value = string.Empty;
            HiddenSecUnit.Value = string.Empty;
           
            MRP.Value = string.Empty;
            DP.Value = string.Empty;
            RP.Value = string.Empty;
            StdPack.Value = string.Empty;
            SyncId.Value = string.Empty;
            HiddenClassID.Value = string.Empty;
            HiddenGroupID.Value = string.Empty;
            HiddenSegmentID.Value = string.Empty;
            divimsg.InnerHtml = string.Empty;
            hidimg.Value = string.Empty;
         //   ddlMastClass.SelectedIndex = 0;
         //   ddlMastSegment.SelectedIndex = 0;
         //   ddlUnderItem.SelectedIndex = 0;
            Itemcode.Value = string.Empty;
            PriceGroup.Value = string.Empty;
            chkIsAdmin.Checked = true;
            txtminimumorderquantity.Value = "0";
            txtprimarycon.Value = "0";
            txtSecondarycon.Value = "0";
            txtigstper.Value = "0";
            txtsgstper.Value = "0";
            txtcgstper.Value = "0";
            //btnDelete.Visible = false;
            //btnSave.Text = "Save";
            //Response.Redirect("~/ItemMaster.aspx");
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int retdel = IB.delete(Convert.ToString(ViewState["ItemId"]));
                if (retdel == 1)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Deleted Successfully');", true);
                    ClearControls();
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    ItemName.Focus();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record cannot be deleted as it is in use');", true);
                    ClearControls();
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";
                    ItemName.Focus();
                }
            }
            else
            {
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
          //  BindSegments(0); BindClass(0); BindParent(0);
            mainDiv.Style.Add("display", "block");
            rptmain.Style.Add("display", "none");
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            fillRepeter();
            ClearControls();
            btnDelete.Visible = false;
            btnSave.Text = "Save";
            mainDiv.Style.Add("display", "none");
            rptmain.Style.Add("display", "block");
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string Deleteitemimage(string id)
        {
            string msg = "";
            try
            {
                string qry = "delete from ItemMastImage where Id= " + id + "";
                if (DAL.DbConnectionDAL.ExecuteQuery(qry) > 0)
                {
                    //qry = "delete from TransComplaintimages  where docid= '" + docid + "'";
                    //DAL.DbConnectionDAL.ExecuteQuery(qry);
                    msg = "Record Deleted Successfully";
                }
                else
                {
                    msg = "Record Is Not Deleted";
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
                msg = "Record Is Not Deleted";
            }

            return msg;
        }

        private void filldt()
        {
            string str = "", straddqry = ""; StringBuilder sb = new StringBuilder(); string col = "";
            if (_exportp == true)
            {
                //str = @"select ROW_NUMBER() over (order by Mi.Itemid desc ) as [Sr. No],Mi.Itemid,Mi.ItemName,Mi.ItemCode,Mic.Name as ProductClass,Mi.PriceGroup,Mis.Name as Segment,Mi.SyncId,Mi.unit,Mi.Mrp,Mi.Rp,Mi.StdPack,Mi.Dp,Mi2.ItemName as MaterialGroup,case Mi.Active when 1 then 'Yes' else 'No' end as Active from MastItem Mi inner join MastItem Mi2 on Mi.underId=Mi2.ItemId left join MastItemClass Mic on Mi.ClassId=Mic.Id left Join MastItemSegment Mis on Mi.SegmentId=Mis.Id  where Mi.ItemType='ITEM' and Mi.ItemName<>'.'";


                str = @"select ROW_NUMBER() over (order by Mi.Itemid desc ) as [Sr. No],Mi.ItemName as [Product Name],Mi2.ItemName as ProductGroup,Mi.ItemCode as ProductCode,Mi.unit,Mis.Name as ProductSegment,Mic.Name as ProductClass,Mi.Mrp,Mi.Dp,Mi.Rp,Mi.StdPack,Mi.PriceGroup,Mi.SyncId,case Mi.Active when 1 then 'Yes' else 'No' end as Active from MastItem Mi inner join MastItem Mi2 on Mi.underId=Mi2.ItemId left join MastItemClass Mic on Mi.ClassId=Mic.Id left Join MastItemSegment Mis on Mi.SegmentId=Mis.Id  where Mi.ItemType='ITEM' and Mi.ItemName<>'.'";


                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);


                foreach (DataColumn dc in dt.Columns)
                {
                    col += dc.ColumnName + ",";
                }

                sb.AppendLine(col.Trim(','));
                foreach (DataRow row in dt.Rows)
                {
                    IEnumerable<string> fields = row.ItemArray.Select(field =>
                      string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                    sb.AppendLine(string.Join(",", fields));
                }

                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=ProductMaster.csv");
                Response.Write(sb.ToString());
                Response.End();
            }
            else System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('You do not have permission to Export');", true);


        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            filldt();
        }

    }
}