using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

using BusinessLayer;
using System.Data;
using DAL;


namespace AstralFFMS
{
    public partial class EditParty : System.Web.UI.Page
    {     
         int PartyId = 0;
         int msg = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["PartyId"] != null)
                {
                    PartyId = Convert.ToInt32(Request.QueryString["PartyId"]);

                    BindDDlCity();

                 //   BusinessClass.BindIndustryName(DdlIndustry);
                    
                    partyIDHiddenField.Value = Request.QueryString["PartyId"];
                   SetPartyValues(PartyId);
                   GetPartyData(PartyId);
                }
            }
        }

        private void BindDDlCity()
        {
            string str = @"select * from mastarea where areaid in (select areaid from MastAreaGrp where MainGrp
                        in (select linkcode from mastlink where primtable='SALESPERSON' and LinkTable='AREA' and PrimCode=" + Settings.Instance.DSRSMID + ")) and areatype='City' and Active=1 order by AreaName ";


            DataTable obj = new DataTable();
            obj = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            if (obj.Rows.Count > 0)
            {
                ddlCity.DataSource = obj;
                ddlCity.DataTextField = "AreaName";
                ddlCity.DataValueField = "AreaId";
                ddlCity.DataBind();
               
            }
            ddlCity.Items.Insert(0, new ListItem("-- Select --", "0"));
        }
        private void GetPartyData(int PartyId)
        {
            string str = "select * from MastParty where PartyId =" + Convert.ToInt32(PartyId);
            DataTable dt1 = new DataTable();
            dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt1.Rows.Count > 0)
            {
                partyName1.Text = dt1.Rows[0]["PartyName"].ToString();
                address.Text = dt1.Rows[0]["Address1"].ToString() + "" + dt1.Rows[0]["Address2"].ToString();
                mobile1.Text = dt1.Rows[0]["Mobile"].ToString();
                lblzipcode.Text = dt1.Rows[0]["Pin"].ToString();
            }
        }

     //   private void GetPartyData(int PartyId)
     //   {
     //       var obj = (from r in context.MastParties.Where(x => x.PartyId == Convert.ToInt32(PartyId)) let addr = "" select new { r.PartyId, r.PartyName, addr = r.Address1 + r.Address2, r.Mobile, r.Pin }).ToList();
     //       partyName1.Text = obj.FirstOrDefault().PartyName;
     //       address.Text = obj.FirstOrDefault().addr;
     //       mobile1.Text = obj.FirstOrDefault().Mobile;
     //       lblzipcode.Text = obj.FirstOrDefault().Pin;
  // UnderPartyId = Convert.ToString(Party.UnderId),
     //   }
        private void SetPartyValues(int PartyId)
        {
            string str = @"select  Party.PartyId,Party.PartyName,Party.Address1, Party.Address2, Party.Pin, Party.AreaId,Party.Email,Party.Mobile,Party.IndId,Party.Potential, Party.Active, Party.ContactPerson,
                                 Party.CSTNo,Party.VATTIN, Party.ServiceTax,Party.PANNo,Party.BlockDate, Party.BlockReason, Party.PartyDist, Party.LoginCreated,Party.Remark, Party.UserId, Party.UnderId,
                                 cityID = city.AreaId,
                                 cityName = city.AreaName,
                                 Party.SyncId,
                                 Party.BeatId,
                                 Party.PartyType from  MastParty Party left join MastArea city on Party.AreaId=city.AreaId where Party.PartyId=" + Convert.ToInt32(PartyId);
            DataTable dt = new DataTable();
            dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);

            //var query = (from Party in context.MastParties.Where(x => x.PartyId == Convert.ToInt32(PartyId))
            //             let UnderParty = ""
            //             let UnderPartyId = 0

            //             join Area1 in context.MastAreas on Party.AreaId equals Area1.AreaId into ps2
            //             from Area1 in ps2.DefaultIfEmpty()

            //             join city in context.MastAreas on Area1.UnderId equals city.AreaId into ct
            //             from city in ct.DefaultIfEmpty()
                         //select new
                         //    {
                         //        Party.PartyId,
                         //        Party.PartyName,
                         //        Party.Address1,
                         //        Party.Address2,
                         //        Party.Pin,
                         //        Party.AreaId,
                         //        Party.Email,
                         //        Party.Mobile,
                         //        Party.IndId,
                         //        Party.Potential,
                         //        Party.Active,
                         //        Party.ContactPerson,
                         //        Party.CSTNo,
                         //        Party.VATTIN,
                         //        Party.ServiceTax,
                         //        Party.PANNo,
                         //        Party.BlockDate,
                         //        Party.BlockReason,
                         //        Party.PartyDist,
                         //        Party.LoginCreated,
                         //        Party.Remark,
                         //        Party.UserId,
                         //        Party.UnderId,
                         //        UnderPartyId = Convert.ToString(Party.UnderId),
                         //        cityID = city.AreaId,
                         //        cityName = city.AreaName,
                         //        Party.SyncId,
                         //        Party.BeatId,
                         //        PartyTypeName = Party.PartyType == 1 ? "Retailer" : Party.PartyType == 2 ? "End User" : Party.PartyType == 3 ? "Dealer" : ""
                         //    });
            PartyName.Value = dt.Rows[0]["PartyName"].ToString();
            ddlCity.SelectedValue = dt.Rows[0]["cityID"].ToString();

            Address1.Value =dt.Rows[0]["Address1"].ToString();  
            Address2.Value = dt.Rows[0]["Address2"].ToString();

            GetAreaOnEdit(Convert.ToInt32(dt.Rows[0]["cityID"].ToString()));

            ddlArea.SelectedValue = Convert.ToString(dt.Rows[0]["AreaId"].ToString());

            GetBeatOnEdit(Convert.ToInt32(dt.Rows[0]["AreaId"].ToString()));

            BindDropDownUnderParty(Convert.ToInt32(dt.Rows[0]["AreaId"].ToString()));

            ddlUnderParty.SelectedValue = Convert.ToString(dt.Rows[0]["UnderId"].ToString());
            ddlBeat.SelectedValue = Convert.ToString(dt.Rows[0]["BeatId"].ToString());
            Pin.Value =dt.Rows[0]["Pin"].ToString();
            ddlpartytype.SelectedValue = dt.Rows[0]["PartyType"].ToString();
            
            Mobile.Value =dt.Rows[0]["Mobile"].ToString();
            DdlIndustry.SelectedValue =dt.Rows[0]["IndId"].ToString();
            Potential.Value =dt.Rows[0]["Potential"].ToString();
            chkIsAdmin.Checked =Convert.ToBoolean(dt.Rows[0]["Active"].ToString());
            Remark.Value = dt.Rows[0]["Remark"].ToString();
            SyncId.Value = dt.Rows[0]["SyncId"].ToString();
            ContactPerson.Value = dt.Rows[0]["ContactPerson"].ToString();
            CSTNo.Value = dt.Rows[0]["CSTNo"].ToString();
            VatTin.Value = dt.Rows[0]["VATTIN"].ToString();
            ServiceTax.Value = dt.Rows[0]["ServiceTax"].ToString();
            BlockReason.Value = dt.Rows[0]["BlockReason"].ToString();
        }

        private void BindDropDownUnderParty(int AreaId)
        {
            string s = "select * from MastParty dist  where dist.PartyDist = 1 and  dist.AreaId ="+ Convert.ToInt32(AreaId);
            DataTable obj = new DataTable();
            obj = DbConnectionDAL.GetDataTable(CommandType.Text, s);
            ddlUnderParty.DataSource = obj;
            ddlUnderParty.DataTextField = "PartyName";
            ddlUnderParty.DataValueField = "PartyId";
            ddlUnderParty.DataBind();
            ddlUnderParty.Items.Insert(0, new ListItem("--Select--", "0"));
        }

        private void GetBeatOnEdit(int AreaId)
        {
            string s = "select *from MastArea where AreaType='Beat' and UnderId ="+ Convert.ToInt32(AreaId);
            DataTable obj = new DataTable();
            obj = DbConnectionDAL.GetDataTable(CommandType.Text, s);
            ddlBeat.DataSource = obj;
            ddlBeat.DataTextField = "AreaName";
            ddlBeat.DataValueField = "AreaId";
            ddlBeat.DataBind();
            ddlBeat.Items.Insert(0, new ListItem("--Select--", "0"));
        }

        private void GetAreaOnEdit(int CityId)
        {
            string strq = "";
            if (Settings.Instance.UserName.ToUpper() == "SA")
            {
                strq = "select * from MastArea where areatype in ('Area') and underId=" + CityId + "  order by AreaName";
            }
            else
            {

                strq = "select * from MastArea where AreaId in (select MainGrp from MastAreaGrp where areaid in (select linkcode from mastlink where ecode='SA'" +
                   " and PrimCode=" + Settings.Instance.SMID + "))and areatype in ('Area') and underId=" + CityId + "";
                //           strq = "select * from MastArea where AreaId in (select MainGrp from MastAreaGrp where areaid in (select linkcode from mastlink where ecode='SA' ))and areatype in ('Area') and underId=" + CityId + "";
            }

            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, strq);
            ddlArea.DataSource = dt;
            ddlArea.DataTextField = "AreaName";
            ddlArea.DataValueField = "AreaId";
            ddlArea.DataBind();
            ddlArea.Items.Insert(0, new ListItem("-- Select Area --", "0"));
        }
     //   public static int CheckSyncId(string Synid, string PartyId)
     //   {
     //       int SyncIdvalu = 0;
     //       try
     //       {
     //           if (PartyId == "")
     //           {
     //               var envObj = from e in context.MastParties
     //                            where e.SyncId == Synid
     //                            select new { e.PartyId, e.SyncId };
     //               var ListA = envObj.ToList();
     //               if (ListA.Count > 0)
     //               {
     //                   SyncIdvalu = 1;
     //               }
     //           }
     //           else
     //           {
     //               var envObj = from e in context.MastParties
     //                            where e.SyncId == Synid && e.PartyId != Convert.ToInt32(PartyId)
     //                            select new { e.PartyId, e.SyncId };
     //               var ListA = envObj.ToList();
     //               if (ListA.Count > 0)
     //               {
     //                   SyncIdvalu = 1;
     //               }
     //           }
     //       }
     //       catch (Exception)
     //       {

     //       }
     //       return SyncIdvalu;

     //   }

     //   //  data: '{PartyId: "' + partyID + '" , PartyName: "' + $('#<%=PartyName.ClientID%>').val() + '" ,
     //   //Address1: "' + $('#<%=Address1.ClientID%>').val() + '",Address2: "' + $('#<%=Address2.ClientID%>').val() + '",
     //   //Pin: "' + $('#<%=Pin.ClientID%>').val() + '",AreaId: "' + $('#<%=ddlArea.ClientID%>').val() + '",
     //   //BeatId: "' + $('#<%=ddlBeat.ClientID%>').val() + '",PartyType: "' + $('#<%=ddlpartytype.ClientID%>').val() + '",
     //   //Mobile: "' + $('#<%=Mobile.ClientID%>').val() + '",IndId: "' + $('#<%=DdlIndustry.ClientID%>').val() + '",
     //   //Potential: "' + $('#<%=Potential.ClientID%>').val() + '",Active: "' + checkedActive + '",
     //   //BlockReason: "' + $('#<%=BlockReason.ClientID%>').val() + '",Remark: "' + $('#<%=Remark.ClientID%>').val() + '",
     //   //SyncId: "' + $('#<%=SyncId.ClientID%>').val() + '",UnderId: "' + $('#<%=ddlUnderParty.ClientID%>').val() + '",
     //   //ContactPerson:"' + $('#<%=ContactPerson.ClientID%>').val() + '",CSTNo:"' + $('#<%=CSTNo.ClientID%>').val() + '",
     //   //VatTin:"' + $('#<%=VatTin.ClientID%>').val() + '",ServiceTax:"' + $('#<%=ServiceTax.ClientID%>').val() + '",
     //   //PanNo:"' + $('#<%=PanNo.ClientID%>').val() + '" }',


     //   [WebMethod]
     //   public static int Partyedit(int PartyId, string PartyName, string Address1, string Address2, string Pin,string CityId, string AreaId, string BeatId, string PartyType,
     //       string Mobile, string IndId, string Potential, bool Active, string BlockReason, string Remark, string SyncId, string UnderId,
     //       string ContactPerson, string CSTNo, string VatTin, string ServiceTax, string PanNo)
     //   {
     //       ImportParty IP = new ImportParty();
     //       try
     //       {
     //           if (PartyName != "")
     //           {
     //               decimal mPotential = 0;
     //               if (!string.IsNullOrEmpty(Potential))
     //                   mPotential = Convert.ToDecimal(Potential);

     //               int retval = IP.UpdatePartyForm(PartyId, PartyName, Address1, Address2, Convert.ToInt32(CityId), Convert.ToInt32(AreaId), Convert.ToInt32(BeatId), Pin, Mobile, Remark, SyncId, Convert.ToInt32(UnderId),
     //                   Convert.ToInt32(IndId), mPotential, Active, BlockReason, PartyType, ContactPerson, CSTNo, VatTin, ServiceTax, PanNo);

     //               if (retval == -1)
     //               {
     //                   //Duplicate SyncID
     //                   msg = 1;
     //               }
     //               else if (retval == -2)
     //               {
     //                   //Duplicate  PartyName
     //                   msg = 2;
     //               }
     //               else if (retval == -3)
     //               {
     //                   //Duplicate Mobile
     //                   msg = 3;
     //               }

     //               else
     //               {
     //                   msg = 0;
     //               }
     //           }
     //       }
     //       catch (Exception)
     //       {
     //           msg = -1;
     //       }

     //       return msg;
     //   }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PartyDashborad.aspx?PartyId=" + PartyId);
        }
    }
}