using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BusinessLayer;
using System.Data;

namespace AstralFFMS
{
    public partial class CoupanAllot : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtIssueDate.Attributes.Add("readonly", "readonly");
            if(!IsPostBack)
            {
                txtIssueDate.Text = DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");
                CalendarExtender1.EndDate = Settings.GetUTCTime();
                BindZone(); BindCoupanScheme(); BindDistributor();
            }
        }

        private void BindDistributor()
        {
            try
            {
                string str = @"SELECT mp.PartyId,(mp.PartyName + ' - ' + ma.AreaName) AS PartyName FROM MastParty mp LEFT JOIN mastarea ma ON mp.AreaId=ma.AreaId where mp.partydist=1 and mp.Active=1 ORDER BY mp.PartyName";
                fillDDLDirect(ddlDistributor, str, "PartyId", "PartyName", 1);                
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void BindZone()
        {
            try
            {
                string str = @"SELECT distinct Zone FROM MastZone where Active=1 ORDER BY Zone";
                fillDDLDirect(ddlZone, str, "Zone", "Zone", 1);                
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void BindCoupanScheme()
        {
            try
            {
                string str = @"SELECT SchemeId,SchemeName FROM MastCoupanScheme where Active=1 ORDER BY SchemeName";
                fillDDLDirect(ddlScheme, str, "SchemeId", "SchemeName", 1);               
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private Int32 BindPrefix(string zone)
        {
            try
            {
                string str = @"SELECT Prefix FROM MastZone where Active=1 and Zone='" + zone + "' ORDER BY Prefix";
                fillDDLDirect(ddlPrefix, str, "Prefix", "Prefix", 1);               
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return 0;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int exists = 0;
            string insertquery = "";

            try
            {               
                if (txtstartCoupan.Text != "")
                {
                    string start, end;
                    string prefix = ddlPrefix.SelectedItem.Text;
                    start = txtstartCoupan.Text;
                    end = txtEndCoupan.Text;

                    if (Convert.ToInt32(txtstartCoupan.Text) > Convert.ToInt32(txtEndCoupan.Text))
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Start coupan no. cannot be greater than from End Coupan no.');", true);
                        return;
                    }

                    string strcount = "SELECT count(*) FROM TransCoupan WHERE CoupanNo BETWEEN " + start + " AND " + end + " AND SchemeId=" + ddlScheme.SelectedValue + " and Zone = '" + ddlZone.SelectedValue + "' and prefix='" + prefix + "'";
                    int count = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strcount));
                    if (count == 0)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('This Coupan range not available.');", true);
                        return;
                    }

                    string strcheck = "SELECT count(*) FROM TransCoupan WHERE CoupanNo between " + start + " and " + end + "  AND DistId is not NULL AND SchemeId=" + ddlScheme.SelectedValue + " and Zone = '" + ddlZone.SelectedValue + "' and prefix='" + prefix + "'";
                    
                    int countcheck = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strcheck));
                    if (countcheck > 0)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('This Coupan range already issued.');", true);
                        return;
                    }
                      
                    insertquery = @"Update TransCoupan set Distid = " + ddlDistributor.SelectedValue + ",Issuedate='" + txtIssueDate.Text + "',Createddate=DateAdd(minute,330,getutcdate()) where SchemeId=" + ddlScheme.SelectedValue + " and Zone='" + ddlZone.SelectedValue + "' and Prefix='" + ddlPrefix.SelectedValue + "' and CoupanNo between " + start + " and " + end + " ";
                        exists = Convert.ToInt32(DbConnectionDAL.ExecuteQuery(insertquery));

                    // string addstr = prefix + i.ToString();
                    //for (int i = Convert.ToInt32(start); i <= Convert.ToInt32(end); i++)
                    //{
                    //    string startwithprefix = prefix + i.ToString();
                    //    string strcheck = "SELECT count(*) FROM TransCoupan WHERE CoupanNo in ( '" + i + "') AND DistId is not NULL AND SchemeId=" + ddlScheme.SelectedValue + " and Zone = '" + ddlZone.SelectedValue + "' and prefix='" + prefix + "'";
                    //    int countcheck = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, strcheck));
                    //    if (countcheck > 0)
                    //    {
                    //        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('This Coupan range already issued.');", true);
                    //        return;
                    //    }
                    //}
                                   
                    //for (int i = Convert.ToInt32(start); i <= Convert.ToInt32(end); i++)
                    //{
                    //    string addstr = prefix + i.ToString();
                    //    insertquery = @"Update TransCoupan set Distid = " + ddlDistributor.SelectedValue + ",Issuedate='" + txtIssueDate.Text + "',Createddate=DateAdd(minute,330,getutcdate()) where SchemeId=" + ddlScheme.SelectedValue + " and Zone='" + ddlZone.SelectedValue + "' and CoupanNo='" + i + "' and Prefix='" + ddlPrefix.SelectedValue + "'";
                    //    exists = Convert.ToInt32(DbConnectionDAL.ExecuteQuery(insertquery));
                    //}
                }
                if (txtIndividual.Text != "")
                {
                    int number = 0;
                    string stradd = "";
                    string strmsg = "";
                    number = Convert.ToInt32(txtIndividual.Text);

                    if (txtIndividual.Text == "1")
                    {
                        stradd = TextBox1.Text;
                    }
                    else if (txtIndividual.Text == "2")
                    {
                        stradd += TextBox1.Text + ',' + TextBox2.Text;
                    }
                    else if (txtIndividual.Text == "3")
                    {
                        stradd += TextBox1.Text + ',' + TextBox2.Text + ',' + TextBox3.Text;
                    }
                    else if (txtIndividual.Text == "4")
                    {
                        stradd += TextBox1.Text + ',' + TextBox2.Text + ',' + TextBox3.Text + ',' + TextBox4.Text;
                    }
                    else if (txtIndividual.Text == "5")
                    {
                        stradd += TextBox1.Text + ',' + TextBox2.Text + ',' + TextBox3.Text + ',' + TextBox4.Text + ',' + TextBox5.Text;
                    }
                    else if (txtIndividual.Text == "6")
                    {
                        stradd += TextBox1.Text + ',' + TextBox2.Text + ',' + TextBox3.Text + ',' + TextBox4.Text + ',' + TextBox5.Text + ',' + TextBox6.Text;
                    }
                    else if (txtIndividual.Text == "7")
                    {
                        stradd += TextBox1.Text + ',' + TextBox2.Text + ',' + TextBox3.Text + ',' + TextBox4.Text + ',' + TextBox5.Text + ',' + TextBox6.Text + ',' + TextBox7.Text;
                    }
                    else if (txtIndividual.Text == "8")
                    {
                        stradd += TextBox1.Text + ',' + TextBox2.Text + ',' + TextBox3.Text + ',' + TextBox4.Text + ',' + TextBox5.Text + ',' + TextBox6.Text + ',' + TextBox7.Text + ',' + TextBox8.Text;
                    }
                    else if (txtIndividual.Text == "9")
                    {
                        stradd += TextBox1.Text + ',' + TextBox2.Text + ',' + TextBox3.Text + ',' + TextBox4.Text + ',' + TextBox5.Text + ',' + TextBox6.Text + ',' + TextBox7.Text + ',' + TextBox8.Text + ',' + TextBox9.Text;
                    }
                    else if (txtIndividual.Text == "10")
                    {
                        stradd += TextBox1.Text + ',' + TextBox2.Text + ',' + TextBox3.Text + ',' + TextBox4.Text + ',' + TextBox5.Text + ',' + TextBox6.Text + ',' + TextBox7.Text + ',' + TextBox8.Text + ',' + TextBox9.Text + ',' + TextBox10.Text;
                    }

                    stradd = stradd.TrimStart(',').TrimEnd(',');
                   
                    string str = "SELECT * FROM TransCoupan WHERE CoupanNo in ( " + stradd + " ) AND DistId is not NULL AND SchemeId=" + ddlScheme.SelectedValue + " and Zone = '" + ddlZone.SelectedValue + "' and Prefix='" + ddlPrefix.SelectedValue + "'";
                    DataTable dt = new DataTable();
                    dt = DbConnectionDAL.GetDataTable(CommandType.Text, str);
                    if (dt.Rows.Count > 0)
                    {
                        string coupanno = "";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            coupanno += dt.Rows[i]["CoupanNo"].ToString() + ",";
                        }
                        coupanno = coupanno.TrimEnd(',');
                        strmsg = coupanno + " Coupan No. already issued";
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('" + strmsg + "');", true);
                        return;
                    }
                    else
                    {
                        for (int j = 0; j < Convert.ToInt32(txtIndividual.Text); j++)
                        {
                            if (j == 0)
                            {
                                if (TextBox1.Text != "")
                                {                                   
                                    insertquery = @"Update TransCoupan set Distid = " + ddlDistributor.SelectedValue + ",Issuedate='" + txtIssueDate.Text + "',Createddate=DateAdd(minute,330,getutcdate()) where SchemeId=" + ddlScheme.SelectedValue + " and Zone='" + ddlZone.SelectedValue + "' and CoupanNo='" + TextBox1.Text + "' and Prefix='" + ddlPrefix.SelectedValue + "'";
                                    exists = Convert.ToInt32(DbConnectionDAL.ExecuteQuery(insertquery));
                                }
                            }
                            else if (j == 1)
                            {
                                if (TextBox2.Text != "")
                                {
                                    insertquery = @"Update TransCoupan set Distid = " + ddlDistributor.SelectedValue + ",Issuedate='" + txtIssueDate.Text + "',Createddate=DateAdd(minute,330,getutcdate()) where SchemeId=" + ddlScheme.SelectedValue + " and Zone='" + ddlZone.SelectedValue + "' and CoupanNo='" + TextBox2.Text + "' and Prefix='" + ddlPrefix.SelectedValue + "'";
                                    exists = Convert.ToInt32(DbConnectionDAL.ExecuteQuery(insertquery));
                                }
                            }
                            else if (j == 2)
                            {
                                if (TextBox3.Text != "")
                                {
                                    insertquery = @"Update TransCoupan set Distid = " + ddlDistributor.SelectedValue + ",Issuedate='" + txtIssueDate.Text + "',Createddate=DateAdd(minute,330,getutcdate()) where SchemeId=" + ddlScheme.SelectedValue + " and Zone='" + ddlZone.SelectedValue + "' and CoupanNo='" + TextBox3.Text + "' and Prefix='" + ddlPrefix.SelectedValue + "'";
                                    exists = Convert.ToInt32(DbConnectionDAL.ExecuteQuery(insertquery));
                                }
                            }
                            else if (j == 3)
                            {
                                if (TextBox4.Text != "")
                                {
                                    insertquery = @"Update TransCoupan set Distid = " + ddlDistributor.SelectedValue + ",Issuedate='" + txtIssueDate.Text + "',Createddate=DateAdd(minute,330,getutcdate()) where SchemeId=" + ddlScheme.SelectedValue + " and Zone='" + ddlZone.SelectedValue + "' and CoupanNo='" + TextBox4.Text + "' and Prefix='" + ddlPrefix.SelectedValue + "'";
                                    exists = Convert.ToInt32(DbConnectionDAL.ExecuteQuery(insertquery));
                                }
                            }
                            else if (j == 4)
                            {
                                if (TextBox5.Text != "")
                                {
                                    insertquery = @"Update TransCoupan set Distid = " + ddlDistributor.SelectedValue + ",Issuedate='" + txtIssueDate.Text + "',Createddate=DateAdd(minute,330,getutcdate()) where SchemeId=" + ddlScheme.SelectedValue + " and Zone='" + ddlZone.SelectedValue + "' and CoupanNo='" + TextBox5.Text + "' and Prefix='" + ddlPrefix.SelectedValue + "'";
                                    exists = Convert.ToInt32(DbConnectionDAL.ExecuteQuery(insertquery));
                                }
                            }
                            else if (j == 5)
                            {
                                if (TextBox6.Text != "")
                                {
                                    insertquery = @"Update TransCoupan set Distid = " + ddlDistributor.SelectedValue + ",Issuedate='" + txtIssueDate.Text + "',Createddate=DateAdd(minute,330,getutcdate()) where SchemeId=" + ddlScheme.SelectedValue + " and Zone='" + ddlZone.SelectedValue + "' and CoupanNo='" + TextBox6.Text + "' and Prefix='" + ddlPrefix.SelectedValue + "'";
                                    exists = Convert.ToInt32(DbConnectionDAL.ExecuteQuery(insertquery));
                                }
                            }
                            else if (j == 6)
                            {
                                if (TextBox7.Text != "")
                                {
                                    insertquery = @"Update TransCoupan set Distid = " + ddlDistributor.SelectedValue + ",Issuedate='" + txtIssueDate.Text + "',Createddate=DateAdd(minute,330,getutcdate()) where SchemeId=" + ddlScheme.SelectedValue + " and Zone='" + ddlZone.SelectedValue + "' and CoupanNo='" + TextBox7.Text + "' and Prefix='" + ddlPrefix.SelectedValue + "'";
                                    exists = Convert.ToInt32(DbConnectionDAL.ExecuteQuery(insertquery));
                                }
                            }
                            else if (j == 7)
                            {
                                if (TextBox8.Text != "")
                                {
                                    insertquery = @"Update TransCoupan set Distid = " + ddlDistributor.SelectedValue + ",Issuedate='" + txtIssueDate.Text + "',Createddate=DateAdd(minute,330,getutcdate()) where SchemeId=" + ddlScheme.SelectedValue + " and Zone='" + ddlZone.SelectedValue + "' and CoupanNo='" + TextBox8.Text + "' and Prefix='" + ddlPrefix.SelectedValue + "'";
                                    exists = Convert.ToInt32(DbConnectionDAL.ExecuteQuery(insertquery));
                                }
                            }
                            else if (j == 8)
                            {
                                if (TextBox9.Text != "")
                                {
                                    insertquery = @"Update TransCoupan set Distid = " + ddlDistributor.SelectedValue + ",Issuedate='" + txtIssueDate.Text + "',Createddate=DateAdd(minute,330,getutcdate()) where SchemeId=" + ddlScheme.SelectedValue + " and Zone='" + ddlZone.SelectedValue + "' and CoupanNo='" + TextBox9.Text + "' and Prefix='" + ddlPrefix.SelectedValue + "'";
                                    exists = Convert.ToInt32(DbConnectionDAL.ExecuteQuery(insertquery));
                                }
                            }
                            else if (j == 9)
                            {
                                if (TextBox10.Text != "")
                                {
                                    insertquery = @"Update TransCoupan set Distid = " + ddlDistributor.SelectedValue + ",Issuedate='" + txtIssueDate.Text + "',Createddate=DateAdd(minute,330,getutcdate()) where SchemeId=" + ddlScheme.SelectedValue + " and Zone='" + ddlZone.SelectedValue + "' and CoupanNo='" + TextBox10.Text + "' and Prefix='" + ddlPrefix.SelectedValue + "'";
                                    exists = Convert.ToInt32(DbConnectionDAL.ExecuteQuery(insertquery));
                                }
                            }
                        }
                    }
                }                
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Save Successfully');", true);
                ClearControls();
            }
            catch(Exception ex)
            { ex.ToString(); }
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("CoupanAllot.aspx");
        }
        protected void ClearControls()
        {
          //  txtIssueDate.Text = DateTime.Parse(DateTime.Now.ToUniversalTime().AddSeconds(19800).ToShortDateString()).ToString("dd/MMM/yyyy");
            //ddlZone.SelectedValue = "0";
            //ddlScheme.SelectedValue = "0";
            //ddlPrefix.SelectedValue = "0";

            ddlDistributor.SelectedValue = "0";           
            txtstartCoupan.Text = "";
            txtEndCoupan.Text = "";
            lblCoupan.Text = "";
            TextBox1.Text = "";
            TextBox2.Text = "";
            TextBox3.Text = "";
            TextBox4.Text = "";
            TextBox5.Text = "";
            TextBox6.Text = "";
            TextBox7.Text = "";
            TextBox8.Text = "";
            TextBox9.Text = "";
            TextBox10.Text = "";
            txtIndividual.Text = "";
            TextBox1.Visible = false;
            TextBox2.Visible = false;
            TextBox3.Visible = false;
            TextBox4.Visible = false;
            TextBox5.Visible = false;
            TextBox6.Visible = false;
            TextBox7.Visible = false;
            TextBox8.Visible = false;
            TextBox9.Visible = false;
            TextBox10.Visible = false;            

        }
        private void cleartextbox()
        {
            TextBox1.Visible = false;
            TextBox2.Visible = false;
            TextBox3.Visible = false;
            TextBox4.Visible = false;
            TextBox5.Visible = false;
            TextBox6.Visible = false;
            TextBox7.Visible = false;
            TextBox8.Visible = false;
            TextBox9.Visible = false;
            TextBox10.Visible = false; 
        }
        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindPrefix(ddlZone.SelectedValue);
        }  

        protected void txtIndividual_TextChanged(object sender, EventArgs e)
        {
            cleartextbox();

            if (txtIndividual.Text == "1")
            { 
                TextBox1.Visible = true; 
            }
            else if (txtIndividual.Text == "2")
            { 
                TextBox1.Visible = true; 
                TextBox2.Visible = true; 
            }
            else if (txtIndividual.Text == "3")
            {
                TextBox1.Visible = true;
                TextBox2.Visible = true; 
                TextBox3.Visible = true; 
            }
            else if (txtIndividual.Text == "4")
            {
                TextBox1.Visible = true;
                TextBox2.Visible = true;
                TextBox3.Visible = true; 
                TextBox4.Visible = true; 
            }
            else if (txtIndividual.Text == "5")
            {
                TextBox1.Visible = true;
                TextBox2.Visible = true;
                TextBox3.Visible = true;
                TextBox4.Visible = true; 
                TextBox5.Visible = true; 
            }
            else if (txtIndividual.Text == "6")
            {
                TextBox1.Visible = true;
                TextBox2.Visible = true;
                TextBox3.Visible = true;
                TextBox4.Visible = true;
                TextBox5.Visible = true; 
                TextBox6.Visible = true; 
            }
            else if (txtIndividual.Text == "7")
            {
                TextBox1.Visible = true;
                TextBox2.Visible = true;
                TextBox3.Visible = true;
                TextBox4.Visible = true;
                TextBox5.Visible = true;
                TextBox6.Visible = true; 
                TextBox7.Visible = true; 
            }
            else if (txtIndividual.Text == "8")
            {
                TextBox1.Visible = true;
                TextBox2.Visible = true;
                TextBox3.Visible = true;
                TextBox4.Visible = true;
                TextBox5.Visible = true;
                TextBox6.Visible = true;
                TextBox7.Visible = true; 
                TextBox8.Visible = true; 
            }
            else if (txtIndividual.Text == "9")
            {
                TextBox1.Visible = true;
                TextBox2.Visible = true;
                TextBox3.Visible = true;
                TextBox4.Visible = true;
                TextBox5.Visible = true;
                TextBox6.Visible = true;
                TextBox7.Visible = true;
                TextBox8.Visible = true; 
                TextBox9.Visible = true; 
            }
            else if (txtIndividual.Text == "10")
            {
                TextBox1.Visible = true;
                TextBox2.Visible = true;
                TextBox3.Visible = true;
                TextBox4.Visible = true;
                TextBox5.Visible = true;
                TextBox6.Visible = true;
                TextBox7.Visible = true;
                TextBox8.Visible = true;
                TextBox9.Visible = true; 
                TextBox10.Visible = true; 
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Coupan No. not allow more than 10.');", true);
                TextBox1.Visible = false;
                TextBox2.Visible = false;
                TextBox3.Visible = false;
                TextBox4.Visible = false;
                TextBox5.Visible = false;
                TextBox6.Visible = false;
                TextBox7.Visible = false;
                TextBox8.Visible = false;
                TextBox9.Visible = false;
                TextBox10.Visible = false; 
            }
        }

        protected void ddlPrefix_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCoupanAvailability(ddlScheme.SelectedValue, ddlZone.SelectedValue, ddlPrefix.SelectedValue);           
        }
        private Int32 BindCoupanAvailability(string scheme, string zone, string prefix)
        {
            try
            {
                string str = "SELECT count( * ) AS number FROM TransCoupan WHERE SchemeId=" + scheme + " and Zone = '" + zone + "' and prefix='" + prefix + "' AND DistId IS NULL";
                int number = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, str));
                lblCoupan.Text = number.ToString();                             
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return 0;
        }
    }
}