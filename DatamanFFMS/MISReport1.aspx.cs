using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using DAL;
using System.Text;
using System.Web.UI.HtmlControls;

namespace AstralFFMS
{
    public partial class MISReport1 : System.Web.UI.Page
    {
        string roleType = "", _state = "", _city = "", _area = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Added By - Nishu 06/12/2015 
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");

                txtfmDate.Attributes.Add("ReadOnly", "true");
                txttodate.Attributes.Add("ReadOnly", "true");
                //End               
                //        BindProductClass();
                //BindSalePersonDDl();

                BindMaterialGroup();

                //Ankita - 18/may/2016- (For Optimization)
                //GetRoleType(Settings.Instance.RoleID);
                roleType = Settings.Instance.RoleType;
                //  btnExport.Visible = false;

                string pageName = Path.GetFileName(Request.Path);
                // btnExport.Enabled = Settings.Instance.CheckExportPermission(pageName, Convert.ToString(Session["user_name"]));
                btnExport.CssClass = "btn btn-primary";
                BindStateDDl();
                BindDistributorDDl();
                BindPartyDDl();
            }
        }

        private void BindDistributorDDl()
        {
            string _query = "";
            _query = @"select PartyId,PartyName from MastParty where PartyDist=1 order by PartyName";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, _query);
            ListBox1.DataSource = dt;
            ListBox1.DataTextField = "PartyName";
            ListBox1.DataValueField = "PartyId";
            ListBox1.DataBind();

            dt.Dispose();
        }
        private void BindPartyDDl()
        {
            string _query = "";
            _query = @"select PartyId,PartyName from MastParty where PartyDist=0 order by PartyName";
            DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, _query);
            ddlParty.DataSource = dt;
            ddlParty.DataTextField = "PartyName";
            ddlParty.DataValueField = "PartyId";
            ddlParty.DataBind();

            dt.Dispose();
        }

        private void BindMaterialGroup()
        {
            try
            {//Ankita - 18/may/2016- (For Optimization)
                //string prodClassQry = @"select * from MastItem where ItemType='MATERIALGROUP' and Active=1 order by ItemName";
                string prodClassQry = @"select ItemId,ItemName from MastItem where ItemType='MATERIALGROUP' and Active=1 order by ItemName";
                DataTable dtProdRep = DbConnectionDAL.GetDataTable(CommandType.Text, prodClassQry);
                if (dtProdRep.Rows.Count > 0)
                {
                    matGrpListBox.DataSource = dtProdRep;
                    matGrpListBox.DataTextField = "ItemName";
                    matGrpListBox.DataValueField = "ItemId";
                    matGrpListBox.DataBind();
                }
                //    ddlMatGrp.Items.Insert(0, new ListItem("--Please select--"));
                dtProdRep.Dispose();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }



        private void ClearControls()
        {
            try
            {
                //  ddlProduct.Items.Clear();
                productListBox.Items.Clear();
                txttodate.Text = Settings.GetUTCTime().ToString("dd/MMM/yyyy");
                txtfmDate.Text = Settings.GetUTCTime().AddMonths(-1).ToString("dd/MMM/yyyy");
                suggreportrpt.DataSource = null;
                suggreportrpt.DataBind();

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        protected void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                string smIDStr1 = "", smIDStr3 = "", smIDStr2 = "";
                string DistIdStr1 = "", PartyIdStr1 = "", ProdctGrp = "", Product = "", _search = "", _state = "", _city = "", _area = "", _query = "", _final = "";

                if (Convert.ToDateTime(txtfmDate.Text) > Convert.ToDateTime(txttodate.Text))
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('From Date Should be less than To Date');", true);
                    return;
                }

                if (Convert.ToString(ddlSuggestion.SelectedValue) == "S")
                {
                    ListBox1.Items.Clear();
                }
                else
                {
                    ddlParty.Items.Clear();
                }
                string vb = ddlSuggestion.SelectedValue;

                foreach (ListItem item in matGrpListBox.Items)
                {
                    if (item.Selected)
                    {
                        ProdctGrp += item.Value + ",";
                    }

                }
                foreach (ListItem item in productListBox.Items)
                {
                    if (item.Selected)
                    {
                        Product += item.Value + ",";
                    }

                }
                foreach (ListItem item in LstCity.Items)
                {

                    if (item.Selected)
                    {
                        smIDStr2 += item.Value + ",";
                        _city = "0";
                    }
                }
                foreach (ListItem item in LstArea.Items)
                {

                    if (item.Selected)
                    {
                        smIDStr3 += item.Value + ",";
                        _area = "0";
                    }
                }
                foreach (ListItem item in ListBox1.Items)
                {
                    if (item.Selected)
                    {
                        DistIdStr1 += item.Value + ",";
                    }

                }
                foreach (ListItem item in LstState.Items)
                {
                    if (item.Selected)
                    {
                        smIDStr1 += item.Value + ",";
                        _state = "0";
                    }

                }
                foreach (ListItem item in ddlParty.Items)
                {
                    if (item.Selected)
                    {
                        PartyIdStr1 += item.Value + ",";
                        // _state = "0";
                    }

                }


                if (!string.IsNullOrEmpty(smIDStr1))
                {
                    smIDStr1 = smIDStr1.TrimStart(',').TrimEnd(',');
                    //_search = "Where vg.stateid IN(" + smIDStr1 + ")";
                    _search = "Where vg3.areaid IN(" + smIDStr1 + ")";
                };
                if (!string.IsNullOrEmpty(smIDStr2))
                {
                    smIDStr2 = smIDStr2.TrimStart(',').TrimEnd(',');
                    //_search = "Where  vg.cityid IN(" + smIDStr2 + ")";
                    _search = "Where  vg1.areaid IN(" + smIDStr2 + ")";
                }
                if (!string.IsNullOrEmpty(smIDStr3))
                {
                    smIDStr3 = smIDStr3.TrimStart(',').TrimEnd(',');
                    if (ddlSuggestion.SelectedValue == "S")
                        _search = "Where  ti.AreaId in (" + smIDStr3 + ")";
                    else if (ddlSuggestion.SelectedValue == "P") _search = "Where  mp.AreaId in (" + smIDStr3 + ")";
                }

                if (_search != "")
                {
                    if (!string.IsNullOrEmpty(ProdctGrp) && string.IsNullOrEmpty(Product))
                    {
                        ProdctGrp = ProdctGrp.TrimStart(',').TrimEnd(',');
                        _search += " And ti.ItemId in (Select ItemId from mastItem where Underid in (" + ProdctGrp + "))";
                    }

                    if (!string.IsNullOrEmpty(Product))
                    {
                        Product = Product.TrimStart(',').TrimEnd(',');
                        _search += " And ti.ItemId in (" + Product + ")";
                    }
                    if (ddlSuggestion.SelectedValue == "P")
                    {
                        if (!string.IsNullOrEmpty(DistIdStr1))
                        {
                            DistIdStr1 = DistIdStr1.TrimStart(',').TrimEnd(',');
                            _search += " And ti.DistId in (" + DistIdStr1 + ")";
                        }

                    }
                    if (ddlSuggestion.SelectedValue == "S")
                    {
                        if (!string.IsNullOrEmpty(PartyIdStr1))
                        {
                            PartyIdStr1 = PartyIdStr1.TrimStart(',').TrimEnd(',');
                            _search += " And ti.PartyId in (" + PartyIdStr1 + ")";
                        }
                    }
                    _search += " And ti.VDate>='" + Settings.dateformat(txtfmDate.Text) + "' and ti.VDate<='" + Settings.dateformat(txttodate.Text) + "'";
                }
                else
                {
                    if (!string.IsNullOrEmpty(ProdctGrp) && string.IsNullOrEmpty(Product))
                    {
                        ProdctGrp = ProdctGrp.TrimStart(',').TrimEnd(',');
                        _search = " Where ti.ItemId in (Select ItemId from mastItem where Underid in (" + ProdctGrp + "))";
                    }

                    if (!string.IsNullOrEmpty(Product))
                    {
                        Product = Product.TrimStart(',').TrimEnd(',');
                        _search = " Where ti.ItemId in (" + Product + ")";
                    }
                    if (ddlSuggestion.SelectedValue == "P")
                    {
                        if (!string.IsNullOrEmpty(DistIdStr1))
                        {
                            if (!string.IsNullOrEmpty(_search))
                            {
                                DistIdStr1 = DistIdStr1.TrimStart(',').TrimEnd(',');
                                _search += " And ti.DistId in (" + DistIdStr1 + ")";
                            }
                            else
                            {
                                DistIdStr1 = DistIdStr1.TrimStart(',').TrimEnd(',');
                                _search = " Where ti.DistId in (" + DistIdStr1 + ")";
                            }
                        }

                    }
                    if (ddlSuggestion.SelectedValue == "S")
                    {
                        if (!string.IsNullOrEmpty(PartyIdStr1))
                        {
                            if (!string.IsNullOrEmpty(_search))
                            {
                                PartyIdStr1 = PartyIdStr1.TrimStart(',').TrimEnd(',');
                                _search += " And ti.PartyId in (" + PartyIdStr1 + ")";
                            }
                            else
                            {
                                PartyIdStr1 = PartyIdStr1.TrimStart(',').TrimEnd(',');
                                _search = " Where ti.PartyId in (" + PartyIdStr1 + ")";
                            }

                        }
                    }
                    _search += " And ti.VDate>='" + Settings.dateformat(txtfmDate.Text) + "' and ti.VDate<='" + Settings.dateformat(txttodate.Text) + "'";
                }
                if (_state == "0")
                {
                    if (ddlSuggestion.SelectedValue == "S")
                    {
                        //_query = @"select vg.stateid as AreaId,vg.stateName,cast((ti.Qty*ti.Rate) as decimal) as OrderAmount,ti.ItemId,ti.PartyId,'S' as ColumnName from TransOrder1 ti inner join ViewGeo vg on  ti.AreaId=vg.areaId " + _search + " ";
                        _query = @"select vg3.AreaId as AreaId,vg3.areaname as stateName,ti.Qty*ti.Rate as OrderAmount,ti.ItemId,ti.PartyId,'S' as ColumnName from TransOrder1 ti left join mastarea vg on  ti.AreaId=vg.areaId left join mastarea vg1 on vg.underid=vg1.areaid left join mastarea vg2 on vg1.underid=vg2.areaid left join mastarea vg3 on vg2.underid=vg3.areaid " + _search + " ";
                    }
                    else
                    {

                        //_query = @"select vg.stateid as AreaId,vg.stateName,cast((ti.Qty*ti.Rate) as decimal) as OrderAmount,ti.ItemId,ti.DistId,'S' as ColumnName from TransDistInv1 ti Inner join Mastparty mp on ti.DistId=mp.PartyId   inner join ViewGeo vg on  mp.AreaId=vg.areaId " + _search + " ";

                        _query = @"select vg3.AreaId as AreaId,vg3.areaname as stateName,ti.Qty*ti.Rate as OrderAmount,ti.ItemId,ti.DistId,'S' as ColumnName from TransDistInv1 ti left join Mastparty mp on ti.DistId=mp.PartyId left join mastarea vg on mp.AreaId=vg.areaId left join mastarea vg1 on vg.underid=vg1.areaid left join mastarea vg2 on vg1.underid=vg2.areaid left join mastarea vg3 on vg2.underid=vg3.areaid " + _search + " ";
                    }

                    ViewState["Column"] = "S";
                    Session["Column1"] = "S";
                    _final = "S";
                    divDSRState.Style.Add("display", "block");
                    divDSRArea.Style.Add("display", "none");
                    divDSRCity.Style.Add("display", "none");
                    if (_city == "0")
                    {

                        if (ddlSuggestion.SelectedValue == "S")
                        {
                            //   _query = "select vg.stateid,vg.statename, vg.cityid as AreaId,vg.cityName,cast((ti.Qty*ti.Rate) as decimal) as OrderAmount,ti.ItemId,ti.PartyId,'C' as ColumnName from TransOrder1 ti " +
                            //"inner join ViewGeo vg on  ti.AreaId=vg.areaId " +
                            //" " + _search + "";
                            _query = "select vg3.areaid as stateid,vg3.AreaName as statename, vg1.areaid as AreaId,vg1.areaname as cityName,ti.Qty*ti.Rate as OrderAmount,ti.ItemId,ti.PartyId,'C' as ColumnName from TransOrder1 ti left join mastarea vg on  ti.AreaId=vg.areaId left join mastarea vg1 on vg.underid=vg1.areaid left join mastarea vg2 on vg1.underid=vg2.areaid left join mastarea vg3 on vg2.underid=vg3.areaid " +
                        " " + _search + "";
                        }
                        else
                        {

                            //_query = @"select vg.stateid,vg.statename, vg.cityid as AreaId,vg.cityName,cast((ti.Qty*ti.Rate) as decimal) as OrderAmount,ti.ItemId,ti.DistId,'C' as ColumnName from TransDistInv1 ti  Inner join Mastparty mp on ti.DistId=mp.PartyId  inner join ViewGeo vg on  mp.AreaId=vg.areaId " + _search + " ";
                            _query = @"select vg.areaid as stateid,vg3.AreaName as statename, vg1.areaid as AreaId,vg1.areaname as cityName,ti.Qty*ti.Rate as OrderAmount,ti.ItemId,ti.DistId,'C' as ColumnName from TransDistInv1 ti  Inner join Mastparty mp on ti.DistId=mp.PartyId left join mastarea vg on mp.AreaId=vg.areaId left join mastarea vg1 on vg.underid=vg1.areaid left join mastarea vg2 on vg1.underid=vg2.areaid left join mastarea vg3 on vg2.underid=vg3.areaid " + _search + " ";
                        }

                        ViewState["Column"] = "C";
                        Session["Column1"] = "C";
                        divDSRState.Style.Add("display", "none");
                        divDSRArea.Style.Add("display", "none");
                        divDSRCity.Style.Add("display", "block");
                        _final = "C";
                    }
                    if (_area == "0")
                    {
                        if (ddlSuggestion.SelectedValue == "S")
                        {
                            //_query = "select vg.areaName,ti.AreaId,cast((ti.Qty*ti.Rate) as decimal) as OrderAmount,vg.Stateid,vg.StateName,vg.cityId,vg.cityName,ti.ItemId,ti.PartyId,'A' as ColumnName from TransOrder1 ti " +
                            //  " inner join ViewGeo vg on  ti.AreaId=vg.areaId  " +
                            //  " " + _search + "";
                            _query = "select vg.areaName,ti.AreaId,ti.Qty*ti.Rate as OrderAmount,vg3.areaid as Stateid,vg3.areaname as StateName,vg1.areaid as cityId,vg1.areaname as cityName,ti.ItemId,ti.PartyId,'A' as ColumnName from TransOrder1 ti " +
                             " left join mastarea vg on  ti.AreaId=vg.areaId left join mastarea vg1 on vg.underid=vg1.areaid left join mastarea vg2 on vg1.underid=vg2.areaid left join mastarea vg3 on vg2.underid=vg3.areaid  " +
                             " " + _search + "";
                        }
                        else
                        {
                            //_query = "select vg.areaName,mp.AreaId,cast((ti.Qty*ti.Rate) as decimal) as OrderAmount,vg.Stateid,vg.StateName,vg.cityId,vg.cityName,ti.ItemId,ti.DistId,'A' as ColumnName from TransDistInv1 ti " +
                            //  " Inner join Mastparty mp on ti.DistId=mp.PartyId inner join ViewGeo vg on  mp.AreaId=vg.areaId  " +
                            //  " " + _search + "";
                            _query = "select vg.areaName,mp.AreaId,ti.Qty*ti.Rate as OrderAmount,vg3.areaid as Stateid,vg3.areaname as StateName,vg1.areaid as cityId,vg1.areaname as cityName,ti.ItemId,ti.DistId,'A' as ColumnName from TransDistInv1 ti " +
                              " Inner join Mastparty mp on ti.DistId=mp.PartyId left join mastarea vg on mp.AreaId=vg.areaId left join mastarea vg1 on vg.underid=vg1.areaid left join mastarea vg2 on vg1.underid=vg2.areaid left join mastarea vg3 on vg2.underid=vg3.areaid  " +
                              " " + _search + "";
                        }
                        ViewState["Column"] = "A";
                        Session["Column1"] = "A";
                        divDSRState.Style.Add("display", "none");
                        divDSRArea.Style.Add("display", "block");
                        divDSRCity.Style.Add("display", "none");
                        _final = "A";
                    }
                }
                else
                {
                    if (ddlSuggestion.SelectedValue == "S")
                    {
                        //_query = "select vg.stateid as AreaId,vg.stateName,cast((ti.Qty*ti.Rate) as decimal) as OrderAmount,'S' as ColumnName from TransOrder1 ti " +
                        //           "inner join ViewGeo vg on  ti.AreaId=vg.areaId " +
                        //           " " + _search + " " ;
                        _query = @"select vg3.AreaId as AreaId,vg3.areaname as stateName,ti.Qty*ti.Rate as OrderAmount,ti.ItemId,ti.PartyId,'S' as ColumnName from TransOrder1 ti inner join mastarea vg on  ti.AreaId=vg.areaId inner join mastarea vg1 on vg.underid=vg1.areaid inner join mastarea vg2 on vg1.underid=vg2.areaid inner join mastarea vg3 on vg2.underid=vg3.areaid " + _search + " ";
                    }
                    else
                    {
                        //_query = "select vg.stateid as AreaId,vg.stateName,cast((ti.Qty*ti.Rate) as decimal) as OrderAmount,'S' as ColumnName from TransDistInv1 ti " +
                        //           " Inner join Mastparty mp on ti.DistId=mp.PartyId  inner join ViewGeo vg on  mp.AreaId=vg.areaId " +
                        //           " " + _search + "";
                        _query = "select vg3.AreaId as AreaId,vg3.areaname as stateName,ti.Qty*ti.Rate as OrderAmount,'S' as ColumnName from TransDistInv1 ti " +
                                   " Inner join Mastparty mp on ti.DistId=mp.PartyId  inner join mastarea vg on  mp.AreaId=vg.areaId inner join mastarea vg1 on vg.underid=vg1.areaid inner join mastarea vg2 on vg1.underid=vg2.areaid inner join mastarea vg3 on vg2.underid=vg3.areaid " +
                                   " " + _search + "";
                    }
                    _final = "S";
                    ViewState["Column"] = "S";
                    Session["Column1"] = "S";
                    divDSRState.Style.Add("display", "block");
                    divDSRArea.Style.Add("display", "none");
                    divDSRCity.Style.Add("display", "none");
                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, _query);
                    rptDSRState.DataSource = dt;
                    rptDSRState.DataBind();
                }
                if (_final == "S")
                {
                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, _query);
                    if (dt.Rows.Count > 0)
                    {
                        var newDt = dt.AsEnumerable()
                     .GroupBy(r => new { Col1 = r["statename"], Col2 = r["AreaId"], Col3 = r["ColumnName"] })
                      .Select(g =>
                      {
                          var row = dt.NewRow();

                          row["statename"] = g.Key.Col1;
                          row["AreaId"] = g.Key.Col2;
                          row["ColumnName"] = g.Key.Col3;
                          row["OrderAmount"] = g.Sum(r => r.Field<decimal>("OrderAmount")).ToString();
                          return row;
                      }).CopyToDataTable();

                        rptDSRState.DataSource = newDt;
                        rptDSRState.DataBind();
                        btnExport.Visible = true;
                    }
                    else
                    {
                        rptDSRState.DataSource = null;
                        rptDSRState.DataBind();
                        btnExport.Visible = false;
                    }
                    dt.Dispose();
                }
                if (_final == "C")
                {
                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, _query);
                    if (dt.Rows.Count > 0)
                    {
                        var newDt = dt.AsEnumerable()
                      .GroupBy(r => new { Col1 = r["statename"], Col2 = r["AreaId"], Col3 = r["ColumnName"], Col4 = r["cityname"] })
                      .Select(g =>
                      {
                          var row = dt.NewRow();

                          row["statename"] = g.Key.Col1;
                          row["AreaId"] = g.Key.Col2;
                          row["ColumnName"] = g.Key.Col3;
                          row["cityname"] = g.Key.Col4;
                          row["OrderAmount"] = g.Sum(r => r.Field<decimal>("OrderAmount")).ToString();
                          return row;
                      }).CopyToDataTable();

                        rptDSRCity.DataSource = newDt;
                        rptDSRCity.DataBind();
                        btnExport.Visible = true;
                    }
                    else
                    {
                        rptDSRCity.DataSource = null;
                        rptDSRCity.DataBind();
                        btnExport.Visible = false;
                    }
                    dt.Dispose();
                }
                if (_final == "A")
                {
                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, _query);
                    if (dt.Rows.Count > 0)
                    {
                        var newDt = dt.AsEnumerable()
                      .GroupBy(r => new { Col1 = r["statename"], Col2 = r["AreaId"], Col3 = r["ColumnName"], Col4 = r["cityname"], Col5 = r["AreaName"] })
                      .Select(g =>
                      {
                          var row = dt.NewRow();

                          row["statename"] = g.Key.Col1;
                          row["AreaId"] = g.Key.Col2;
                          row["ColumnName"] = g.Key.Col3;
                          row["cityname"] = g.Key.Col4;
                          row["AreaName"] = g.Key.Col5;
                          row["OrderAmount"] = g.Sum(r => r.Field<decimal>("OrderAmount")).ToString();
                          return row;
                      }).CopyToDataTable();
                        rptDSRArea.DataSource = newDt;
                        rptDSRArea.DataBind();
                        btnExport.Visible = true;
                    }
                    else
                    {
                        rptDSRArea.DataSource = null;
                        rptDSRArea.DataBind();
                        btnExport.Visible = false;
                    }
                    dt.Dispose();
                }

                if (_final == "")
                {
                    DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, _query);
                    if (dt.Rows.Count > 0)
                    {
                        var newDt = dt.AsEnumerable()
                      .GroupBy(r => new { Col1 = r["statename"], Col2 = r["AreaId"], Col3 = r["ColumnName"], Col4 = r["cityname"], Col5 = r["AreaName"] })
                      .Select(g =>
                      {
                          var row = dt.NewRow();

                          row["statename"] = g.Key.Col1;
                          row["AreaId"] = g.Key.Col2;
                          row["ColumnName"] = g.Key.Col3;
                          row["cityname"] = g.Key.Col4;
                          row["AreaName"] = g.Key.Col5;
                          row["OrderAmount"] = g.Sum(r => r.Field<decimal>("OrderAmount")).ToString();
                          return row;
                      }).CopyToDataTable();
                        rptDSRArea.DataSource = newDt;
                        rptDSRArea.DataBind();
                        btnExport.Visible = true;
                    }
                    else
                    {
                        rptDSRArea.DataSource = null;
                        rptDSRArea.DataBind();
                        btnExport.Visible = false;
                    }
                    dt.Dispose();
                }
                ViewState["Column"] = "";
                _search = "";
                _search = string.Empty;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MISReport1.aspx", false);
        }

        protected void matGrpListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string matGrpStr = "";
            foreach (ListItem matGrp in matGrpListBox.Items)
            {
                if (matGrp.Selected)
                {
                    matGrpStr += matGrp.Value + ",";
                }
            }
            matGrpStr = matGrpStr.TrimStart(',').TrimEnd(',');
            if (matGrpStr != "")
            {
                string mastItemQry1 = @"select ItemId,ItemName from MastItem where Underid in (" + matGrpStr + ") and ItemType='ITEM' and Active=1";
                DataTable dtMastItem1 = DbConnectionDAL.GetDataTable(CommandType.Text, mastItemQry1);
                if (dtMastItem1.Rows.Count > 0)
                {
                    productListBox.DataSource = dtMastItem1;
                    productListBox.DataTextField = "ItemName";
                    productListBox.DataValueField = "ItemId";
                    productListBox.DataBind();
                }
                //       ddlProduct.Items.Insert(0, new ListItem("--Please select--"));
                dtMastItem1.Dispose();
            }
            else
            {
                ClearControls();
            }

        }

        protected void ddlSuggestion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSuggestion.SelectedItem.Text == "Sales Person")
            {
                roleType = Settings.Instance.RoleType;
                suggreportrpt.DataSource = null;
                suggreportrpt.DataBind();
            }
            else if (ddlSuggestion.SelectedItem.Text == "Distributor")
            {
                string smIDStr12 = Settings.Instance.SMID;
                suggreportrpt.DataSource = null;
                suggreportrpt.DataBind();

            }
            else { }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {


                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=MISReport.csv");
                string headertext = "State".TrimStart('"').TrimEnd('"');
                if (Session["Column1"] == "C") headertext += "," + "City".TrimStart('"').TrimEnd('"');
                if (Session["Column1"] == "A") headertext += "," + "City".TrimStart('"').TrimEnd('"') + "," + "Area".TrimStart('"').TrimEnd('"');
                headertext += "," + "Sale Amount".TrimStart('"').TrimEnd('"');
                StringBuilder sb = new StringBuilder();
                sb.Append(headertext);
                sb.AppendLine(System.Environment.NewLine);
                string dataText = string.Empty;
                //
                DataTable dtParams = new DataTable();


                dtParams.Columns.Add(new DataColumn("State", typeof(String)));

                if (Session["Column1"] == "C") dtParams.Columns.Add(new DataColumn("City", typeof(String)));
                if (Session["Column1"] == "A")
                {
                    dtParams.Columns.Add(new DataColumn("City", typeof(String)));
                    dtParams.Columns.Add(new DataColumn("Area", typeof(String)));
                }
                dtParams.Columns.Add(new DataColumn("PrimarySaleAmt", typeof(String)));

                //
                if (Session["Column1"] == "S")
                {
                    foreach (RepeaterItem item in rptDSRState.Items)
                    {
                        DataRow dr = dtParams.NewRow();
                        Label lblState = item.FindControl("lblState") as Label;
                        dr["State"] = lblState.Text;
                        Label lblBillAmount = item.FindControl("lblBillAmount") as Label;
                        dr["PrimarySaleAmt"] = lblBillAmount.Text.ToString(); ;
                        dtParams.Rows.Add(dr);
                    }
                }
                if (Session["Column1"] == "C")
                {
                    foreach (RepeaterItem item in rptDSRCity.Items)
                    {
                        DataRow dr = dtParams.NewRow();
                        Label lblState = item.FindControl("lblState") as Label;
                        dr["State"] = lblState.Text;
                        Label lblCity = item.FindControl("lblCity") as Label;
                        dr["City"] = lblCity.Text;


                        Label lblBillAmount = item.FindControl("lblBillAmount") as Label;
                        dr["PrimarySaleAmt"] = lblBillAmount.Text.ToString(); ;
                        dtParams.Rows.Add(dr);
                    }
                }
                if (Session["Column1"] == "A")
                {
                    foreach (RepeaterItem item in rptDSRArea.Items)
                    {
                        DataRow dr = dtParams.NewRow();
                        Label lblState = item.FindControl("lblState") as Label;
                        dr["State"] = lblState.Text;
                        Label lblCity = item.FindControl("lblCity") as Label;
                        dr["City"] = lblCity.Text;
                        Label lblArea = item.FindControl("lblArea") as Label;
                        dr["Area"] = lblArea.Text.ToString(); ;

                        Label lblBillAmount = item.FindControl("lblBillAmount") as Label;
                        dr["PrimarySaleAmt"] = lblBillAmount.Text.ToString(); ;
                        dtParams.Rows.Add(dr);
                    }
                }

                DataView dv = dtParams.DefaultView;
                dv.Sort = "State desc";
                DataTable udtNew = dv.ToTable();
                decimal[] totalVal = new decimal[4];

                for (int j = 0; j < dtParams.Rows.Count; j++)
                {
                    for (int k = 0; k < dtParams.Columns.Count; k++)
                    {
                        if (dtParams.Rows[j][k].ToString().Contains(","))
                        {
                            if (k == 0)
                            {
                                // sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                            }
                            else
                            {
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                                if (Session["Column1"] == "C")
                                {
                                    if (k == 2)
                                    {
                                        if (udtNew.Rows[j][k].ToString() != "")
                                        {
                                            totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                        }
                                    }
                                }
                                else if (Session["Column1"] == "A")
                                {
                                    if (k == 3)
                                    {
                                        if (udtNew.Rows[j][k].ToString() != "")
                                        {
                                            totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                        }
                                    }
                                }
                                else
                                {
                                    if (k == 1)
                                    {
                                        if (udtNew.Rows[j][k].ToString() != "")
                                        {
                                            totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                        }
                                    }
                                }
                            }
                        }
                        else if (dtParams.Rows[j][k].ToString().Contains(System.Environment.NewLine))
                        {
                            if (k == 0)
                            {
                                //  sb.Append(String.Format("\"{0}\"", Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy")) + ',');
                            }
                            else
                            {
                                sb.Append(String.Format("\"{0}\"", dtParams.Rows[j][k].ToString()) + ',');
                                if (Session["Column1"] == "C")
                                {
                                    if (k == 2)
                                    {
                                        if (udtNew.Rows[j][k].ToString() != "")
                                        {
                                            totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                        }
                                    }
                                }
                                else if (Session["Column1"] == "A")
                                {
                                    if (k == 3)
                                    {
                                        if (udtNew.Rows[j][k].ToString() != "")
                                        {
                                            totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                        }
                                    }
                                }
                                else
                                {
                                    if (k == 1)
                                    {
                                        if (udtNew.Rows[j][k].ToString() != "")
                                        {
                                            totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (k == 0 || k == 0)
                            {
                                // sb.Append(Convert.ToDateTime(dtParams.Rows[j][k]).ToString("dd/MMM/yyyy") + ',');
                                sb.Append(dtParams.Rows[j][k].ToString() + ',');
                            }
                            else
                            {
                                sb.Append(dtParams.Rows[j][k].ToString() + ',');
                                if (Session["Column1"] == "C")
                                {
                                    if (k == 2)
                                    {
                                        if (udtNew.Rows[j][k].ToString() != "")
                                        {
                                            totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                        }
                                    }
                                }
                                else if (Session["Column1"] == "A")
                                {
                                    if (k == 3)
                                    {
                                        if (udtNew.Rows[j][k].ToString() != "")
                                        {
                                            totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                        }
                                    }
                                }
                                else
                                {
                                    if (k == 1)
                                    {
                                        if (udtNew.Rows[j][k].ToString() != "")
                                        {
                                            totalVal[k] += Convert.ToDecimal(udtNew.Rows[j][k].ToString());
                                        }
                                    }
                                }
                            }

                        }
                    }
                    sb.Append(Environment.NewLine);
                }

                string totalStr = string.Empty;
                for (int x = 1; x < totalVal.Count(); x++)
                {
                    if (totalVal[x] == 0)
                    {
                        //  totalStr += "0" + ',';
                    }
                    else
                    {
                        if (totalStr == string.Empty)
                            totalStr = Convert.ToDecimal(totalVal[x]).ToString("#.00");
                        else
                            totalStr += Convert.ToDecimal(totalVal[x]).ToString("#.00") + ',';
                    }
                }

                if (Session["Column1"] == "C")
                {
                    sb.Append(",Total," + totalStr);
                }
                else if (Session["Column1"] == "A")
                {
                    sb.Append(",,Total," + totalStr);
                }
                else
                {
                    sb.Append("Total," + totalStr);
                }

                Response.Write(sb.ToString());
                Response.Flush();
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
                Response.End();

                //Response.Clear();
                //Response.ContentType = "text/csv";
                //Response.AddHeader("content-disposition", "attachment;filename=MISReport.csv");
                //Response.Write(sb.ToString());
                ////Response.End();
                //HttpContext.Current.ApplicationInstance.CompleteRequest();

                sb.Clear();
                dtParams.Dispose();
                dv.Dispose();
            }
            catch (Exception ex)
            {
                ex.ToString();

            }

        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        protected void LstDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str = "";
            //         string message = "";
            //foreach (ListItem itm in LstDepartment.Items)
            //{
            //    if (itm.Selected)
            //    {
            //        str += itm.Value + ",";
            //    }
            //}
            str = str.TrimStart(',').TrimEnd(',');

            if (str != "")
            {
                string str1 = @"SELECT T1.Id,T1.Name FROM MastComplaintNature AS T1 WHERE T1.NatureType='Suggestion' AND T1.Active=1 AND T1.DeptId IN (" + str + ")";
                DataTable dt1 = DbConnectionDAL.GetDataTable(CommandType.Text, str1);
                if (dt1.Rows.Count > 0)
                {
                    LstArea.DataSource = dt1;
                    LstArea.DataTextField = "Name";
                    LstArea.DataValueField = "Id";
                    LstArea.DataBind();
                }
                dt1.Dispose();
            }


        }

        protected void ddlpartytype_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void myRepeaterDeals_ItemCreated(object sender, RepeaterItemEventArgs e)
        {

        }
        protected void rptDSR_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

            }
        }
        protected void suggreportrpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                if (ddlSuggestion.SelectedValue == "Distributor")
                {
                    var col = e.Item.FindControl("tdsname");
                    col.Visible = false;
                }
                else
                {
                    var col = e.Item.FindControl("tdsname");
                    col.Visible = true;
                }
            }
            if (e.Item.ItemType == ListItemType.Header)
            {

                if (ddlSuggestion.SelectedValue == "Distributor")
                {
                    var col1 = e.Item.FindControl("thsname");
                    col1.Visible = false;
                }
                else
                {
                    var col1 = e.Item.FindControl("thsname");
                    col1.Visible = true;
                }
            }
        }

        private void BindStateDDl()
        {
            string strC = "select AreaID,AreaName from mastarea where AreaType='State' and Active='1' order by AreaName";
            fillDDLDirect(LstState, strC, "AreaID", "AreaName");
        }
        public static void fillDDLDirect(ListBox xddl, string xmQry, string xvalue, string xtext)
        {
            xddl.DataSource = DbConnectionDAL.GetDataTable(CommandType.Text, xmQry);
            xddl.DataTextField = xtext.Trim();
            xddl.DataValueField = xvalue.Trim();
            xddl.DataBind();
        }
        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            string matGrpStr = "";
            //         string message = "";
            foreach (ListItem matGrp in LstState.Items)
            {
                if (matGrp.Selected)
                {
                    matGrpStr += matGrp.Value + ",";
                }
            }
            matGrpStr = matGrpStr.TrimStart(',').TrimEnd(',');
            if (matGrpStr != "")
            {
                string strQ = "select Distinct(CityID),CityName from ViewGeo VG where vg.cityact=1 and VG.StateId IN(" + matGrpStr + ") order by CityName";
                fillDDLDirect(LstCity, strQ, "CityID", "CityName");
            }
        }
        protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            string matGrpStr = "";
            foreach (ListItem matGrp in LstCity.Items)
            {
                if (matGrp.Selected)
                {
                    matGrpStr += matGrp.Value + ",";
                }
            }
            matGrpStr = matGrpStr.TrimStart(',').TrimEnd(',');
            if (matGrpStr != "")
            {
                string strQ = "select Distinct areaId,areaName from ViewGeo VG where VG.Areaact=1 and VG.CityId IN(" + matGrpStr + ") and areaname <>'' order by areaName";
                fillDDLDirect(LstArea, strQ, "AreaID", "AreaName");
            }
        }
        protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string DistIdStr1 = "", PartyIdStr1 = "", ProdctGrp = "", Product = "";
            if (e.CommandName == "select")
            {
                HiddenField hdPartyId = (HiddenField)e.Item.FindControl("hfAreaId");
                HiddenField hfColumn = (HiddenField)e.Item.FindControl("hfColumn");
                foreach (ListItem item in ListBox1.Items)
                {
                    if (item.Selected)
                    {
                        DistIdStr1 += item.Value + ",";
                    }
                }

                foreach (ListItem item in ddlParty.Items)
                {
                    if (item.Selected)
                    {
                        PartyIdStr1 += item.Value + ",";
                    }
                }

                foreach (ListItem item in matGrpListBox.Items)
                {
                    if (item.Selected)
                    {
                        ProdctGrp += item.Value + ",";
                    }
                }
                foreach (ListItem item in productListBox.Items)
                {
                    if (item.Selected)
                    {
                        Product += item.Value + ",";
                    }

                }
                DistIdStr1 = DistIdStr1.TrimStart(',').TrimEnd(',');
                PartyIdStr1 = PartyIdStr1.TrimStart(',').TrimEnd(',');
                ProdctGrp = ProdctGrp.TrimStart(',').TrimEnd(',');
                Product = Product.TrimStart(',').TrimEnd(',');

                Session["DistIdStr1"] = DistIdStr1;
                Session["PartyIdStr1"] = PartyIdStr1;
                Session["ProdctGrp"] = ProdctGrp;
                Session["Product"] = Product;

                Response.Redirect("MISReportPW.aspx?AreaId=" + hdPartyId.Value + "&FromDate=" + Settings.dateformat(txtfmDate.Text) + "&ToDate=" + Settings.dateformat(txttodate.Text) + "&Type=" + hfColumn.Value + "&SaleType=" + ddlSuggestion.SelectedValue, false);
            }
        }
    }
}