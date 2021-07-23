using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using System.IO;

namespace AstralFFMS
{
    public partial class MeetTargetFromLevel2 : System.Web.UI.Page
    {
        BAL.MeetTarget.MeetTargetBAL MT = new BAL.MeetTarget.MeetTargetBAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Path.GetFileName(Request.Path);
            string Pageheader = Settings.Instance.GetPageHeaderName(pageName);
            lblPageHeader.Text = Pageheader;

            if (!IsPostBack)
            {
                btnsave.Visible = false;
                lblPartTypeName.Visible = false;
                fillfyear();
            }
        }
        private void fillfyear()
        {  //Ankita - 20/may/2016- (For Optimization)
            //string str = "select * from Financialyear ";
            string str = "select id,Yr from Financialyear ";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            if (dt.Rows.Count > 0)
            {
                txtcurrentyear.DataSource = dt;
                txtcurrentyear.DataTextField = "Yr";
                txtcurrentyear.DataValueField = "id";
                txtcurrentyear.DataBind();
            }
            txtcurrentyear.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        private void fillUnderLevel3()
        {
            DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
            if (d.Rows.Count > 0)
            {
                try
                {
                    DataView dv = new DataView(d);
                    dv.RowFilter = "RoleName='Level 1'";
                    if (dv.Table.Rows.Count > 0)
                    {
                        GridView2.DataSource = dv;
                        GridView2.DataBind();
                        btnsave.Visible = true;
                    }
                    else
                    {
                        GridView2.DataSource = null;
                        GridView2.DataBind();
                        btnsave.Visible = false;
                    }
                }
                catch { }
            }
        }

        private void fillUserType()
        {
            string s = "select * from MastMeetType";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, s);
            if (dt.Rows.Count > 0)
            {
                GridView1.DataSource = dt;
                GridView1.DataBind();
                GridView2.DataSource = null;
                GridView2.DataBind();

                btnsave.Visible = false;
                lblPartTypeName.Visible = false;
            }
        }
        protected void txtcurrentyear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtcurrentyear.SelectedIndex > 0)
            {
                fillUserType();
                lblPartTypeName.Visible = false;
                GridView2.DataSource = null;
                GridView2.DataBind();
            }
            else
            {
                lblPartTypeName.Visible = false;
                GridView1.DataSource = null;
                GridView1.DataBind();
                GridView2.DataSource = null;
                GridView2.DataBind();
            }
        }

        private int GetSalesPerId(int uid)
        {
            string st = "select SMId from MastSalesRep where UserId=" + uid;
            int SID = Settings.DMInt32(Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, st)));
            return SID;


        }
        private DataTable GetSUnderPerSMId(int uid)
        {
            int s = GetSalesPerId(uid);
            string st = "select SMId from MastSalesRep where UserId=" + s;
            DataTable UperDT = DbConnectionDAL.GetDataTable(CommandType.Text, st);
            return UperDT;
        }

        private DataTable GetSUnderPerSMId1(int uid)
        {
            int s = GetSalesPerId(uid);
            string st = "select SMId from MastSalesRep where UserId=" + s;
            DataTable UperDT = DbConnectionDAL.GetDataTable(CommandType.Text, st);
            return UperDT;
            string p = "";
            for (int i = 0; i < UperDT.Rows.Count; i++)
            {
                p += UperDT.Rows[i]["SMId"].ToString() + ",";
            }
            p = p.Remove(p.Length - 1);
            string strqw = "select * from MastSalesRep where UnderId in (" + p + ")";
            DataTable ds = new DataTable();
            ds = DbConnectionDAL.GetDataTable(CommandType.Text, strqw);
            return ds;
        }
        private DataTable GetSUnderPerSMId2(int uid)
        {
            int s = GetSalesPerId(uid);
            string st = "select SMId from MastSalesRep where UserId=" + s;
            DataTable UperDT = DbConnectionDAL.GetDataTable(CommandType.Text, st);
            return UperDT;
            string p = "";
            string q = "";
            for (int i = 0; i < UperDT.Rows.Count; i++)
            {
                p += UperDT.Rows[i]["SMId"].ToString() + ",";
            }
            p = p.Remove(p.Length - 1);

            string strqw = "select SMId from MastSalesRep where UnderId in (" + p + ")";
            DataTable ds = new DataTable();
            ds = DbConnectionDAL.GetDataTable(CommandType.Text, strqw);

            for (int j = 0; j < ds.Rows.Count; j++)
            {
                q += ds.Rows[j]["SMId"].ToString() + ",";
            }
            q = q.Remove(q.Length - 1);

            string strqw1 = "select SMId from MastSalesRep where UnderId in (" + q + ")";
            DataTable ds1 = new DataTable();
            ds1 = DbConnectionDAL.GetDataTable(CommandType.Text, strqw1);
            return ds1;
        }

        private DataTable GetSUnderPerSMId6(int uid)
        {
            int s = GetSalesPerId(uid);
            string st = "select SMId from MastSalesRep where UserId=" + s;
            DataTable UperDT = DbConnectionDAL.GetDataTable(CommandType.Text, st);
            return UperDT;
            string p = "";
            string q = "";
            string r = "";
            for (int i = 0; i < UperDT.Rows.Count; i++)
            {
                p += UperDT.Rows[i]["SMId"].ToString() + ",";
            }
            p = p.Remove(p.Length - 1);

            string strqw = "select SMId from MastSalesRep where UnderId in (" + p + ")";
            DataTable ds = new DataTable();
            ds = DbConnectionDAL.GetDataTable(CommandType.Text, strqw);

            for (int j = 0; j < ds.Rows.Count; j++)
            {
                q += ds.Rows[j]["SMId"].ToString() + ",";
            }
            q = q.Remove(q.Length - 1);

            string strqw1 = "select SMId from MastSalesRep where UnderId in (" + q + ")";
            DataTable ds1 = new DataTable();
            ds1 = DbConnectionDAL.GetDataTable(CommandType.Text, strqw1);

            for (int k = 0; k < ds1.Rows.Count; k++)
            {
                r += ds1.Rows[k]["SMId"].ToString() + ",";
            }
            r = r.Remove(r.Length - 1);

            string strqw2 = "select SMId from MastSalesRep where UnderId in (" + r + ")";
            DataTable ds2 = new DataTable();
            ds2 = DbConnectionDAL.GetDataTable(CommandType.Text, strqw2);

            return ds2;



        }
        private DataTable GetSUnderPerSMId7(int uid)
        {
            int s1 = GetSalesPerId(uid);
            string st = "select SMId from MastSalesRep where UserId=" + s1;
            DataTable UperDT = DbConnectionDAL.GetDataTable(CommandType.Text, st);
            return UperDT;
            string p = "";
            string q = "";
            string r = "";
            string s = "";
            for (int i = 0; i < UperDT.Rows.Count; i++)
            {
                p += UperDT.Rows[i]["SMId"].ToString() + ",";
            }
            p = p.Remove(p.Length - 1);

            string strqw = "select SMId from MastSalesRep where UnderId in (" + p + ")";
            DataTable ds = new DataTable();
            ds = DbConnectionDAL.GetDataTable(CommandType.Text, strqw);

            for (int j = 0; j < ds.Rows.Count; j++)
            {
                q += ds.Rows[j]["SMId"].ToString() + ",";
            }
            q = q.Remove(q.Length - 1);

            string strqw1 = "select SMId from MastSalesRep where UnderId in (" + q + ")";
            DataTable ds1 = new DataTable();
            ds1 = DbConnectionDAL.GetDataTable(CommandType.Text, strqw1);

            for (int k = 0; k < ds1.Rows.Count; k++)
            {
                r += ds1.Rows[k]["SMId"].ToString() + ",";
            }
            r = r.Remove(r.Length - 1);

            string strqw2 = "select SMId from MastSalesRep where UnderId in (" + r + ")";
            DataTable ds2 = new DataTable();
            ds2 = DbConnectionDAL.GetDataTable(CommandType.Text, strqw2);


            for (int l = 0; l < ds1.Rows.Count; l++)
            {
                s += ds2.Rows[l]["SMId"].ToString() + ",";
            }
            s = s.Remove(r.Length - 1);

            string strqw3 = "select SMId from MastSalesRep where UnderId in (" + s + ")";
            DataTable ds3 = new DataTable();
            ds3 = DbConnectionDAL.GetDataTable(CommandType.Text, strqw3);

            return ds3;
        }

        private int GetSalesPerUserId(int uid)
        {
            string st = "select UserId from MastSalesRep where SMId=" + uid;
            int SID = Settings.DMInt32(Convert.ToString(DbConnectionDAL.GetScalarValue(CommandType.Text, st)));
            return SID;
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lk = (Label)e.Row.Cells[0].FindControl("lblTargetFromHo");
                LinkButton lnkTarget = (LinkButton)e.Row.FindControl("lnkTarget");

                LinkButton lk3 = (LinkButton)e.Row.Cells[0].FindControl("lnkAssignedByLevel3");
                LinkButton lk2 = (LinkButton)e.Row.Cells[0].FindControl("lnkAssignedByLevel2");
                //LinkButton lk4 = (LinkButton)e.Row.Cells[0].FindControl("lnkAssignedByLevel4");
                //LinkButton lk6 = (LinkButton)e.Row.Cells[0].FindControl("lnkAssignedByLevel6");
                //LinkButton lk7 = (LinkButton)e.Row.Cells[0].FindControl("lnkAssignedByLevel7");
                HiddenField hid = (HiddenField)e.Row.Cells[0].FindControl("hidPartyTypeId");

                if (lk != null)
                {
                    string s = @"select * from MeetTragetFromHO Where  PartyTypeId=" + hid.Value + " and SMID=" + Settings.Instance.SMID + " and MeetYear='" + txtcurrentyear.SelectedItem.Text + "'";
                    DataTable dtr = new DataTable();
                    dtr = DbConnectionDAL.GetDataTable(CommandType.Text, s);
                    if (dtr.Rows.Count > 0)
                    {
                        try
                        {
                            object sum1 = dtr.Compute("Sum(JanValue)", "");
                            object sum2 = dtr.Compute("Sum(FebValue)", "");
                            object sum3 = dtr.Compute("Sum(MarValue)", "");
                            object sum4 = dtr.Compute("Sum(AprValue)", "");
                            object sum5 = dtr.Compute("Sum(MayValue)", "");
                            object sum6 = dtr.Compute("Sum(JunValue)", "");
                            object sum7 = dtr.Compute("Sum(JulValue)", "");
                            object sum8 = dtr.Compute("Sum(AugValue)", "");
                            object sum9 = dtr.Compute("Sum(SepValue)", "");
                            object sum10 = dtr.Compute("Sum(OctValue)", "");
                            object sum11 = dtr.Compute("Sum(NovValue)", "");
                            object sum12 = dtr.Compute("Sum(DecValue)", "");
                            decimal df = Convert.ToDecimal(sum1) + Convert.ToDecimal(sum2) + Convert.ToDecimal(sum3) + Convert.ToDecimal(sum4) + Convert.ToDecimal(sum5) + Convert.ToDecimal(sum6) + Convert.ToDecimal(sum7) + Convert.ToDecimal(sum8) + Convert.ToDecimal(sum9) + Convert.ToDecimal(sum10) + Convert.ToDecimal(sum11) + Convert.ToDecimal(sum12);
                            lk.Text = df.ToString();
                        }
                        catch
                        {
                            lk.Text = "0";
                        }

                    }
                    else
                    {
                        // lk.Text = "Target";
                    }

                }
                if (lnkTarget != null)
                {

                    object sum1 = 0; object sum2 = 0;
                    object sum3 = 0;
                    object sum4 = 0;
                    object sum5 = 0;
                    object sum6 = 0;
                    object sum7 = 0; object sum8 = 0; object sum9 = 0; object sum10 = 0; object sum11 = 0; object sum12 = 0;

                    string s = @"select * from MeetTragetFromHO Where  PartyTypeId=" + hid.Value + " and AssignedByID=" + Settings.Instance.UserID + " and MeetYear='" + txtcurrentyear.SelectedItem.Text + "'";
                    DataTable dtr2 = new DataTable();
                    dtr2 = DbConnectionDAL.GetDataTable(CommandType.Text, s);
                    if (dtr2.Rows.Count > 0)
                    {
                        try
                        {

                            try
                            {
                                sum1 = dtr2.Compute("Sum(JanValue)", "");
                            }
                            catch
                            {

                            }
                            try
                            {
                                sum2 = dtr2.Compute("Sum(FebValue)", "");
                            }
                            catch { }

                            try
                            {
                                sum3 = dtr2.Compute("Sum(MarValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum4 = dtr2.Compute("Sum(AprValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum5 = dtr2.Compute("Sum(MayValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum6 = dtr2.Compute("Sum(JunValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum7 = dtr2.Compute("Sum(JulValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum8 = dtr2.Compute("Sum(AugValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum9 = dtr2.Compute("Sum(SepValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum10 = dtr2.Compute("Sum(OctValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum11 = dtr2.Compute("Sum(NovValue)", "");
                            }
                            catch { }
                            try
                            {
                                sum12 = dtr2.Compute("Sum(DecValue)", "");
                            }
                            catch { }
                            decimal df = Convert.ToDecimal(sum1) + Convert.ToDecimal(sum2) + Convert.ToDecimal(sum3) + Convert.ToDecimal(sum4) + Convert.ToDecimal(sum5) + Convert.ToDecimal(sum6) + Convert.ToDecimal(sum7) + Convert.ToDecimal(sum8) + Convert.ToDecimal(sum9) + Convert.ToDecimal(sum10) + Convert.ToDecimal(sum11) + Convert.ToDecimal(sum12);
                            lnkTarget.Text = df.ToString();
                        }
                        catch
                        {
                            lnkTarget.Text = "0";
                        }

                    }
                    else
                    {
                        lnkTarget.Text = "Target";

                    }
                }



            }
        }


        private int LoginUserLevel()
        {
            int lvl = 0;
            //Ankita - 20/may/2016- (For Optimization)
            //string str = "select Lvl  from MastSalesRep where UserId=" + Settings.Instance.UserID;
            //DataTable LVLDT = new DataTable();
            //LVLDT = DbConnectionDAL.GetDataTable(CommandType.Text, str);
            //if (LVLDT.Rows.Count > 0)
            //{
            //    lvl = Convert.ToInt32(LVLDT.Rows[0][0].ToString()) + 1;
            //}
            lvl = Convert.ToInt16(Settings.Instance.SalesPersonLevel);
            return lvl;

        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "HO")
            {
                string str1 = "select Name,Id from MastMeetType where Id=" + e.CommandArgument.ToString();
                DataTable PartyDT = new DataTable();
                PartyDT = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
                lblPartTypeName.Text = PartyDT.Rows[0][0].ToString();
                lblPartTypeID.Text = PartyDT.Rows[0][1].ToString();
                fillUnderLevel3();
            }
            else if (e.CommandName == "Level2")
            {
                string str1 = "select Name,Id from MastMeetType where Id=" + e.CommandArgument.ToString();
                DataTable PartyDT = new DataTable();
                PartyDT = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
                lblPartTypeName.Text = PartyDT.Rows[0][0].ToString();
                lblPartTypeID.Text = PartyDT.Rows[0][1].ToString();

                DataTable d = Settings.UnderUsers(Settings.Instance.SMID);
                if (d.Rows.Count > 0)
                {
                    try
                    {
                        DataView dv = new DataView(d);
                        dv.RowFilter = "RoleName='Level 1'";
                        if (dv.Table.Rows.Count > 0)
                        {
                            GridView3.DataSource = dv;
                            GridView3.DataBind();
                            btnsave.Visible = true;
                        }
                        else
                        {
                            GridView3.DataSource = null;
                            GridView3.DataBind();
                            btnsave.Visible = false;
                        }
                    }
                    catch { }
                }
            }

                



            
        }
        protected void btnsave_Click(object sender, EventArgs e)
        {
            string docID = Settings.GetDocID("MTARG", DateTime.Now);
            Settings.SetDocID("MTARG", docID);
            try
            {
                string s = "delete from MeetTragetFromHO Where AssignedByID=" + Settings.DMInt32(Settings.Instance.UserID) + " and  PartyTypeId=" + Settings.DMInt32(lblPartTypeID.Text) + " and MeetYear='" + txtcurrentyear.SelectedItem.Text + "'";
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, s);

                for (int i = 0; i < GridView2.Rows.Count; i++)
                {
                    HiddenField hidPartyId = (HiddenField)GridView2.Rows[i].FindControl("PartyId");
                    TextBox apr = (TextBox)GridView2.Rows[i].FindControl("txtapr");
                    TextBox may = (TextBox)GridView2.Rows[i].FindControl("txtmay");
                    TextBox jun = (TextBox)GridView2.Rows[i].FindControl("txtjun"); 
                    TextBox jul = (TextBox)GridView2.Rows[i].FindControl("txtjul");
                    TextBox aug = (TextBox)GridView2.Rows[i].FindControl("txtaug");
                    TextBox spt = (TextBox)GridView2.Rows[i].FindControl("txtspt");
                    TextBox oct = (TextBox)GridView2.Rows[i].FindControl("txtoct");
                    TextBox nov = (TextBox)GridView2.Rows[i].FindControl("txtNov");
                    TextBox dec = (TextBox)GridView2.Rows[i].FindControl("txtDec");
                    TextBox jan = (TextBox)GridView2.Rows[i].FindControl("txtJan");
                    TextBox feb = (TextBox)GridView2.Rows[i].FindControl("txtFeb");
                    TextBox mar = (TextBox)GridView2.Rows[i].FindControl("txtMarch");

                    int RetSave = MT.Insert(docID, Settings.DMInt32(lblPartTypeID.Text), Settings.DMInt32(jan.Text), Settings.DMInt32(feb.Text), Settings.DMInt32(mar.Text), Settings.DMInt32(apr.Text), Settings.DMInt32(may.Text), Settings.DMInt32(jun.Text), Settings.DMInt32(jul.Text), Settings.DMInt32(aug.Text), Settings.DMInt32(spt.Text), Settings.DMInt32(oct.Text), Settings.DMInt32(nov.Text), Settings.DMInt32(dec.Text), Settings.DMInt32(hidPartyId.Value), i + 1, Settings.DMInt32(Settings.Instance.UserID), txtcurrentyear.SelectedItem.Text);
                }

                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Inserted Successfuly');", true);
                fillUserType();

            }
            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Error", "errormessage('Record cannot be insert');", true);
                ex.ToString();
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MeetTargetFromLevel2.aspx");
        }

        decimal totalapr = 0; decimal totalmay = 0; decimal totaljun = 0; decimal totaljul = 0;
        decimal totalaug = 0; decimal totalspt = 0; decimal totaloct = 0; decimal totalnov = 0;
        decimal totaldec = 0; decimal totaljan = 0; decimal totalfeb = 0; decimal totalmar = 0;
        decimal finaltotal = 0;

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox apr = (TextBox)e.Row.FindControl("txtapr");
                TextBox may = (TextBox)e.Row.FindControl("txtmay");
                TextBox jun = (TextBox)e.Row.FindControl("txtjun");
                TextBox jul = (TextBox)e.Row.FindControl("txtjul");
                TextBox aug = (TextBox)e.Row.FindControl("txtaug");
                TextBox spt = (TextBox)e.Row.FindControl("txtspt");
                TextBox oct = (TextBox)e.Row.FindControl("txtoct");
                TextBox nov = (TextBox)e.Row.FindControl("txtNov");
                TextBox dec = (TextBox)e.Row.FindControl("txtDec");
                TextBox jan = (TextBox)e.Row.FindControl("txtJan");
                TextBox feb = (TextBox)e.Row.FindControl("txtFeb");
                TextBox mar = (TextBox)e.Row.FindControl("txtMarch");
                TextBox total = (TextBox)e.Row.FindControl("lbltargetnoofmeet");

                HiddenField PartyId = (HiddenField)e.Row.FindControl("PartyId");

                string srty = "select * from MeetTragetFromHO where SMID=" + Settings.DMInt32(PartyId.Value) + " and PartyTypeId=" + lblPartTypeID.Text + " and MeetYear='" + txtcurrentyear.SelectedItem.Text + "'";
                DataTable dt12 = new DataTable();
                dt12 = DbConnectionDAL.GetDataTable(CommandType.Text, srty);

                if (dt12.Rows.Count > 0)
                {
                    apr.Text = Convert.ToString(dt12.Rows[0]["AprValue"]);
                    may.Text = Convert.ToString(dt12.Rows[0]["MayValue"]);
                    jun.Text = Convert.ToString(dt12.Rows[0]["JunValue"]);
                    jul.Text = Convert.ToString(dt12.Rows[0]["JulValue"]);
                    aug.Text = Convert.ToString(dt12.Rows[0]["AugValue"]);
                    spt.Text = Convert.ToString(dt12.Rows[0]["SepValue"]);
                    oct.Text = Convert.ToString(dt12.Rows[0]["OctValue"]);
                    nov.Text = Convert.ToString(dt12.Rows[0]["NovValue"]);
                    dec.Text = Convert.ToString(dt12.Rows[0]["DecValue"]);
                    jan.Text = Convert.ToString(dt12.Rows[0]["JanValue"]);
                    feb.Text = Convert.ToString(dt12.Rows[0]["FebValue"]);
                    mar.Text = Convert.ToString(dt12.Rows[0]["MarValue"]);
                    //total.Text = Convert.ToString(dt12.Rows[0][""]);

                }


                decimal a = 0; decimal b = 0; decimal c = 0; decimal d = 0; decimal f = 0; decimal g = 0; decimal h = 0; decimal i = 0; decimal j = 0; decimal k = 0; decimal l = 0; decimal m = 0; decimal n = 0;

                if (apr.Text != "" || apr.Text != string.Empty || apr.Text != null)
                {
                    try
                    {
                        a = Convert.ToDecimal(apr.Text);
                    }
                    catch
                    {
                        a = 0;
                    }
                }
                if (may.Text != "" || may.Text != string.Empty || may.Text != null)
                {
                    try
                    {
                        b = Convert.ToDecimal(may.Text);
                    }
                    catch
                    {
                        b = 0;
                    }
                }
                if (jun.Text != "" || jun.Text != string.Empty || jun.Text != null)
                {
                    try
                    {
                        c = Convert.ToDecimal(jun.Text);
                    }
                    catch
                    {
                        c = 0;
                    }
                }

                if (jul.Text != "" || jul.Text != string.Empty || jul.Text != null)
                {
                    try
                    {
                        f = Convert.ToDecimal(jul.Text);
                    }
                    catch
                    {
                        f = 0;
                    }
                }
                if (aug.Text != "" || aug.Text != string.Empty || aug.Text != null)
                {
                    try
                    {
                        g = Convert.ToDecimal(aug.Text);
                    }
                    catch
                    {
                        g = 0;
                    }
                }
                if (spt.Text != "" || spt.Text != string.Empty || spt.Text != null)
                {
                    try
                    {
                        h = Convert.ToDecimal(spt.Text);
                    }
                    catch
                    {
                        h = 0;
                    }
                }
                if (oct.Text != "" || oct.Text != string.Empty || oct.Text != null)
                {
                    try
                    {
                        i = Convert.ToDecimal(oct.Text);
                    }
                    catch
                    {
                        i = 0;
                    }
                }
                if (nov.Text != "" || nov.Text != string.Empty || nov.Text != null)
                {
                    try
                    {
                        j = Convert.ToDecimal(nov.Text);
                    }
                    catch
                    {
                        j = 0;
                    }
                }
                if (dec.Text != "" || dec.Text != string.Empty || dec.Text != null)
                {
                    try
                    {
                        k = Convert.ToDecimal(dec.Text);
                    }
                    catch
                    {
                        k = 0;
                    }

                }
                if (jan.Text != "" || jan.Text != string.Empty || jan.Text != null)
                {

                    try
                    {
                        l = Convert.ToDecimal(jan.Text);
                    }
                    catch
                    {
                        l = 0;
                    }
                }
                if (feb.Text != "" || feb.Text != string.Empty || feb.Text != null)
                {
                    try
                    {
                        m = Convert.ToDecimal(feb.Text);
                    }
                    catch
                    {
                        m = 0;
                    }

                }
                if (mar.Text != "" || mar.Text != string.Empty || mar.Text != null)
                {
                    try
                    {
                        n = Convert.ToDecimal(mar.Text);
                    }
                    catch
                    {
                        n = 0;
                    }
                }
                total.Text = Convert.ToString(a + b + c + f + g + h + i + j + k + l + m + n);

                totalapr = totalapr + a;
                totalmay += b; totaljun += c; totaljul += f;
                totalaug += g; totalspt += h; totaloct += i; totalnov += j;
                totaldec += k; totaljan += l; totalfeb += m; totalmar += n;
                finaltotal += Convert.ToDecimal(total.Text);
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtapr1 = (TextBox)e.Row.FindControl("txtapr1");

                TextBox txtmay1 = (TextBox)e.Row.FindControl("txtmay1");
                TextBox txtjun1 = (TextBox)e.Row.FindControl("txtjun1");
                TextBox txtjul1 = (TextBox)e.Row.FindControl("txtjul1");
                TextBox txtaug1 = (TextBox)e.Row.FindControl("txtaug1");
                TextBox txtspt1 = (TextBox)e.Row.FindControl("txtspt1");
                TextBox txtoct1 = (TextBox)e.Row.FindControl("txtoct1");
                TextBox txtnov1 = (TextBox)e.Row.FindControl("txtNov1");
                TextBox txtdec1 = (TextBox)e.Row.FindControl("txtDec1");
                TextBox txtjan1 = (TextBox)e.Row.FindControl("txtJan1");
                TextBox txtfeb1 = (TextBox)e.Row.FindControl("txtFeb1");
                TextBox txtmar1 = (TextBox)e.Row.FindControl("txtMarch1");
                TextBox txttotal1 = (TextBox)e.Row.FindControl("lbltargetnoofmeet1");

                txtapr1.Text = Convert.ToString(totalapr);
                txtmay1.Text = Convert.ToString(totalmay);
                txtjun1.Text = Convert.ToString(totaljun);
                txtjul1.Text = Convert.ToString(totaljul);
                txtaug1.Text = Convert.ToString(totalaug);
                txtspt1.Text = Convert.ToString(totalspt);
                txtoct1.Text = Convert.ToString(totaloct);
                txtnov1.Text = Convert.ToString(totalnov);
                txtdec1.Text = Convert.ToString(totaldec);
                txtjan1.Text = Convert.ToString(totaljan);
                txtfeb1.Text = Convert.ToString(totalfeb);
                txtmar1.Text = Convert.ToString(totalmar);
                txttotal1.Text = Convert.ToString(finaltotal);

            }


        }

        protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lk = (Label)e.Row.FindControl("lblTargetFromHo");
                Label lblAllocatetoTeam = (Label)e.Row.FindControl("lblAllocatetoTeam");
                Label lblpending = (Label)e.Row.FindControl("lblpending");
                HiddenField hid = (HiddenField)e.Row.Cells[0].FindControl("PartyId");
                HiddenField hidUid = (HiddenField)e.Row.Cells[0].FindControl("HiddenField1");

                if (lk != null)
                {
                    string s = @"select * from MeetTragetFromHO Where  PartyTypeId=" + lblPartTypeID.Text + " and SMId=" + hid.Value + " and MeetYear='" + txtcurrentyear.SelectedItem.Text + "'";
                    DataTable dtr = new DataTable();
                    dtr = DbConnectionDAL.GetDataTable(CommandType.Text, s);
                    if (dtr.Rows.Count > 0)
                    {
                        try
                        {
                            object sum1 = dtr.Compute("Sum(JanValue)", "");
                            object sum2 = dtr.Compute("Sum(FebValue)", "");
                            object sum3 = dtr.Compute("Sum(MarValue)", "");
                            object sum4 = dtr.Compute("Sum(AprValue)", "");
                            object sum5 = dtr.Compute("Sum(MayValue)", "");
                            object sum6 = dtr.Compute("Sum(JunValue)", "");
                            object sum7 = dtr.Compute("Sum(JulValue)", "");
                            object sum8 = dtr.Compute("Sum(AugValue)", "");
                            object sum9 = dtr.Compute("Sum(SepValue)", "");
                            object sum10 = dtr.Compute("Sum(OctValue)", "");
                            object sum11 = dtr.Compute("Sum(NovValue)", "");
                            object sum12 = dtr.Compute("Sum(DecValue)", "");
                            decimal df = Convert.ToDecimal(sum1) + Convert.ToDecimal(sum2) + Convert.ToDecimal(sum3) + Convert.ToDecimal(sum4) + Convert.ToDecimal(sum5) + Convert.ToDecimal(sum6) + Convert.ToDecimal(sum7) + Convert.ToDecimal(sum8) + Convert.ToDecimal(sum9) + Convert.ToDecimal(sum10) + Convert.ToDecimal(sum11) + Convert.ToDecimal(sum12);
                            lk.Text = df.ToString();
                        }
                        catch
                        {
                            lk.Text = "0";
                        }

                    }
                    else
                    {
                        //  lk.Text = "Target";
                    }

                }

                if (lblAllocatetoTeam != null)
                {

                    string s = @"select * from MeetTragetFromHO Where  PartyTypeId=" + lblPartTypeID.Text + " and AssignedByID=" + hidUid.Value + " and MeetYear='" + txtcurrentyear.SelectedItem.Text + "'";
                    DataTable dtr = new DataTable();
                    dtr = DbConnectionDAL.GetDataTable(CommandType.Text, s);
                    if (dtr.Rows.Count > 0)
                    {
                        try
                        {
                            object sum1 = dtr.Compute("Sum(JanValue)", "");
                            object sum2 = dtr.Compute("Sum(FebValue)", "");
                            object sum3 = dtr.Compute("Sum(MarValue)", "");
                            object sum4 = dtr.Compute("Sum(AprValue)", "");
                            object sum5 = dtr.Compute("Sum(MayValue)", "");
                            object sum6 = dtr.Compute("Sum(JunValue)", "");
                            object sum7 = dtr.Compute("Sum(JulValue)", "");
                            object sum8 = dtr.Compute("Sum(AugValue)", "");
                            object sum9 = dtr.Compute("Sum(SepValue)", "");
                            object sum10 = dtr.Compute("Sum(OctValue)", "");
                            object sum11 = dtr.Compute("Sum(NovValue)", "");
                            object sum12 = dtr.Compute("Sum(DecValue)", "");
                            decimal df = Convert.ToDecimal(sum1) + Convert.ToDecimal(sum2) + Convert.ToDecimal(sum3) + Convert.ToDecimal(sum4) + Convert.ToDecimal(sum5) + Convert.ToDecimal(sum6) + Convert.ToDecimal(sum7) + Convert.ToDecimal(sum8) + Convert.ToDecimal(sum9) + Convert.ToDecimal(sum10) + Convert.ToDecimal(sum11) + Convert.ToDecimal(sum12);
                            lblAllocatetoTeam.Text = df.ToString();
                        }
                        catch
                        {
                            lblAllocatetoTeam.Text = "0";
                        }

                    }
                    else
                    {
                        //  lk.Text = "Target";
                    }



                }

                lblpending.Text = Convert.ToString(Settings.DMInt32(lk.Text) - Settings.DMInt32(lblAllocatetoTeam.Text));
            }
        }
    }
}