using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using BAL;
using BusinessLayer;
using System.IO;
using DAL;

namespace AstralFFMS
{
    public partial class ParticlarsForDist : System.Web.UI.Page
    {
        string PanImage = ""; string GSTImage = ""; string Aadharmage = ""; string BankImage = ""; string FoodLicenseImage = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetParticularData();
                BtnNext1.Visible = true;
                BtnNext2.Visible = false;
                Btnpre1.Visible = false;
                btnprevious2.Visible = false;
                btnFinalSave.Visible = false;
            }
        }
        protected void BtnNext2_Click(object sender, EventArgs e)
        {
            string Result = SaveEntryForm2();
            if (Result == "Proceed")
            {
                panel2.Visible = false;
                panel1.Visible = false;
                panel3.Visible = true;
                btnFinalSave.Visible = true;
                btnprevious2.Visible = true;
                //divfooter.Style.Add("display", "block");
                BtnNext1.Visible = false;
                BtnNext2.Visible = false;
                Btnpre1.Visible = false;
                btnprevious2.Visible = true;
                btnFinalSave.Visible = true;
                formheader.InnerHtml = "Particulars For Distributors : Entry Form-3";
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('" + Result + "');", true);
            }
        }

        protected void BtnNext1_Click1(object sender, EventArgs e)
        {
            string Result = SaveEntryForm1();
            if (Result == "Proceed")
            {
                panel2.Visible = true;
                panel1.Visible = false;
                panel3.Visible = false;
                BtnNext1.Visible = false;
                BtnNext2.Visible = true;
                Btnpre1.Visible = true;
                btnprevious2.Visible = false;
                btnFinalSave.Visible = false;
                formheader.InnerHtml = "Particulars For Distributors : Entry Form-2";
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('" + Result + "');", true);
            }
        }

        protected void btnFinalSave_Click(object sender, EventArgs e)
        {
            string _Result = SaveNameOfCompaniesyouRepresented();
            if (_Result == "1")
                _Result = SavegridDeliveryarrangements();
            if (_Result == "1")
                _Result = SavegridRetailCustomerUnderYou();
            if (_Result == "1")
                _Result = Savegridprimeretailers();
            if (_Result == "1")
                _Result = Savegridestimatedturnover();
            if (_Result == "1")
            {

                string sql = "update DistParticulars set IsFinalSaved=1 where DINNo='" + txtDinNo.Value + "'";
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sql);
                panel3.Visible = false;
                panel1.Visible = true;
                panel2.Visible = false;
                Clear();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Successmessage", "Successmessage('Record Saved Successfully');", true);
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('" + _Result + "');", true);
            }

        }


        protected void Btnpre1_Click1(object sender, EventArgs e)
        {
            GetParticularData();
            panel2.Visible = false;
            panel1.Visible = true;
            panel3.Visible = false;
            btnFinalSave.Visible = false;
            // divfooter.Style.Add("display", "none");
            BtnNext1.Visible = true;
            BtnNext2.Visible = false;
            Btnpre1.Visible = false;
            btnprevious2.Visible = false;
            btnFinalSave.Visible = false;
            formheader.InnerHtml = "Particulars For Distributors : Entry Form-1";
        }

        protected void btnprevious2_Click(object sender, EventArgs e)
        {
            GetParticularData();
            panel2.Visible = true;
            panel1.Visible = false;
            panel3.Visible = false;
            btnprevious2.Visible = false;
            btnFinalSave.Visible = false;
            //divfooter.Style.Add("display", "none");

            BtnNext1.Visible = false;
            BtnNext2.Visible = true;
            Btnpre1.Visible = true;
            btnprevious2.Visible = false;
            btnFinalSave.Visible = false;
            formheader.InnerHtml = "Particulars For Distributors : Entry Form-2";
        }
        private string SaveEntryForm1()
        {
            string Next = "Proceed";
            string ProperitoImagePath = ""; string ProperitoryImagepath = "";
            try
            {
                if (FileUpload2.PostedFile != null)
                {
                    string FileName = Path.GetFileName(FileUpload2.PostedFile.FileName);
                    string extension = Path.GetExtension(FileName);
                    if (!string.IsNullOrEmpty(extension))
                    {
                        ProperitoImagePath = "DistributorImages/" + txtDinNo.Value + '-' + FileName;
                        FileInfo file1 = new FileInfo(ProperitoImagePath);
                        if (file1.Exists)// to check whether file exist or not ,if exist rename it
                        {
                            file1.Delete();
                        }
                        FileUpload2.SaveAs(Server.MapPath(ProperitoImagePath));
                    }
                }
                if (FileUpload3.PostedFile != null)
                {
                    string FileName = Path.GetFileName(FileUpload3.PostedFile.FileName);
                    string extension = Path.GetExtension(FileName);
                    if (!string.IsNullOrEmpty(extension))
                    {
                        ProperitoryImagepath = "DistributorImages/" + txtDinNo.Value + '-' + FileName;
                        FileInfo file1 = new FileInfo(ProperitoryImagepath);
                        if (file1.Exists)// to check whether file exist or not ,if exist rename it
                        {
                            file1.Delete();
                        }
                        FileUpload3.SaveAs(Server.MapPath(ProperitoryImagepath));
                    }
                }

                string Sql = "Select * from DistParticulars where DINNo='" + txtDinNo.Value + "' and IsFinalSaved=1";//Final Save Check
                DataTable dtcheckduplicate = DbConnectionDAL.GetDataTable(CommandType.Text, Sql);
                if (dtcheckduplicate.Rows.Count == 0)
                {
                    Sql = "Select * from DistParticulars where DINNo='" + txtDinNo.Value + "' and IsFinalSaved=0";//Accidental Save Check
                    dtcheckduplicate = DbConnectionDAL.GetDataTable(CommandType.Text, Sql);
                    if (dtcheckduplicate.Rows.Count == 0)
                    {
                        SaveEntryForm1NewEntry(ProperitoImagePath, ProperitoryImagepath);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(ProperitoImagePath))
                        ProperitoImagePath=dtcheckduplicate.Rows[0]["ProperitoImagePath"].ToString();
                        if (string.IsNullOrEmpty(ProperitoryImagepath))
                         ProperitoryImagepath=dtcheckduplicate.Rows[0]["ProperitoryImagepath"].ToString();
                        SaveEntryForm1AfterDelete(ProperitoImagePath, ProperitoryImagepath);
                    }
                }
                else
                {
                    Next = "Records Already Exist";
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Already Exist');", true);
                }
            }

            catch (Exception ex)
            {
                Next = "Oops! Error While Inserting Records";
            }
            return Next;
        }
        private void SaveEntryForm1AfterDelete(string ProperitoImagePath, string ProperitoryImagepath)
        {
            string Sql = "Delete from DistParticulars where DINNo='" + txtDinNo.Value + "' and IsFinalSaved=0";
            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, Sql);
            SaveEntryForm1NewEntry(ProperitoImagePath, ProperitoryImagepath);
        }
        private void SaveEntryForm1NewEntry(string ProperitoImagePath, string ProperitoryImagepath)
        {
            string Sql = "insert into DistParticulars(DINNo,FirmName ,FirmAddress ,ContactPerson ,PhoneOffice ,PhoneResidence,Email ,Mobile ,PartnerName ,StorageOwnRented ,StorageArea ,SalespersonNo ,OtherEmpNo ,ProperitoImagePath ,ProperitoryImagepath ,IsFinalSaved ) values ('" + txtDinNo.Value.Replace("'", "''") + "', '" + txtfirmname.Value.Replace("'", "''") + "','" + txtaddress.Value.Replace("'", "''") + "','" + txtcontactPerson.Value.Replace("'", "''") + "','" + txtoffice.Value.Replace("'", "''") + "','" + txtresidence.Value.Replace("'", "''") + "','" + txtemail.Value.Replace("'", "''") + "','" + txtmobile.Value.Replace("'", "''") + "','" + txtProprieter.Value.Replace("'", "''") + "','" + DDlStorage.SelectedValue + "','" + txtfeet.Value.Replace("'", "''") + "'," + Convert.ToInt32(txtSalesman.Value) + "," + Convert.ToInt32(txtothers.Value) + ",'" + ProperitoImagePath + "','" + ProperitoryImagepath + "','0')";
            DbConnectionDAL.ExecuteNonQuery(CommandType.Text, Sql);
        }
        private string SaveEntryForm2()
        {
            string Next = "Proceed";
            try
            {
                string _GSTRegistration = ""; string _PANNo = ""; string _AadharNo = ""; string _FoodLicence = ""; string _BankCheque = "";

                if (FileUpload5.PostedFile != null)
                {
                    string FileName = Path.GetFileName(FileUpload5.PostedFile.FileName);
                    string extension = Path.GetExtension(FileName);
                    if (!string.IsNullOrEmpty(extension))
                    {
                        _GSTRegistration = "DistributorImages/" + txtDinNo.Value + '-' + FileName;
                        FileInfo file1 = new FileInfo(_GSTRegistration);
                        if (file1.Exists)// to check whether file exist or not ,if exist rename it
                        {
                            file1.Delete();
                        }
                        FileUpload5.SaveAs(Server.MapPath(_GSTRegistration));
                    }
                }
                if (FileUpload11.PostedFile != null)
                {
                    string FileName = Path.GetFileName(FileUpload11.PostedFile.FileName);
                    string extension = Path.GetExtension(FileName);
                    if (!string.IsNullOrEmpty(extension))
                    {
                        _PANNo = "DistributorImages/" + txtDinNo.Value + '-' + FileName;
                        FileInfo file1 = new FileInfo(_PANNo);
                        if (file1.Exists)// to check whether file exist or not ,if exist rename it
                        {
                            file1.Delete();
                        }
                        FileUpload11.SaveAs(Server.MapPath(_PANNo));
                    }
                }
                if (FileUpload4.PostedFile != null)
                {
                    string FileName = Path.GetFileName(FileUpload4.PostedFile.FileName);
                    string extension = Path.GetExtension(FileName);
                    if (!string.IsNullOrEmpty(extension))
                    {
                        _AadharNo = "DistributorImages/" + txtDinNo.Value + '-' + FileName;
                        FileInfo file1 = new FileInfo(_AadharNo);
                        if (file1.Exists)// to check whether file exist or not ,if exist rename it
                        {
                            file1.Delete();
                        }
                        FileUpload4.SaveAs(Server.MapPath(_AadharNo));
                    }
                }
                if (FileUpload7.PostedFile != null)
                {
                    string FileName = Path.GetFileName(FileUpload7.PostedFile.FileName);
                    string extension = Path.GetExtension(FileName);
                    if (!string.IsNullOrEmpty(extension))
                    {
                        _FoodLicence = "DistributorImages/" + txtDinNo.Value + '-' + FileName;
                        FileInfo file1 = new FileInfo(_FoodLicence);
                        if (file1.Exists)// to check whether file exist or not ,if exist rename it
                        {
                            file1.Delete();
                        }
                        FileUpload7.SaveAs(Server.MapPath(_FoodLicence));
                    }
                }
                if (FileUpload6.PostedFile != null)
                {
                    string FileName = Path.GetFileName(FileUpload6.PostedFile.FileName);
                    string extension = Path.GetExtension(FileName);
                    if (!string.IsNullOrEmpty(extension))
                    {
                        _BankCheque = "DistributorImages/" + txtDinNo.Value + '-' + FileName;
                        FileInfo file1 = new FileInfo(_BankCheque);
                        if (file1.Exists)// to check whether file exist or not ,if exist rename it
                        {
                            file1.Delete();
                        }
                        FileUpload6.SaveAs(Server.MapPath(_BankCheque));
                    }
                }

                string sql = "Select * from DistParticulars where ( Aadhar ='" + txtaadhar.Value + "' or GST ='" + txtgst.Value + "' ) and DiNno<>'" + txtDinNo.Value + "' ";
                DataTable dt = DbConnectionDAL.GetDataTable(CommandType.Text, sql);
                if (dt.Rows.Count > 0)
                {
                    Next = "This Aadhar No. Or GST No. Already Exist";
                    //  System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "errormessage", "errormessage('Record Already Exist');", true);
                }
                else
                {
                 //   string PanImage = ""; string GSTImage = ""; string Aadharmage = ""; string BankImage = ""; string FoodLicenseImage = "";
                    if (string.IsNullOrEmpty(_GSTRegistration))
                        _GSTRegistration = Convert.ToString(Session["GSTImage"]);
                    if (string.IsNullOrEmpty(_PANNo))
                        _PANNo = Convert.ToString(Session["PanImage"]);
                    if (string.IsNullOrEmpty(_AadharNo))
                        _AadharNo = Convert.ToString(Session["Aadharmage"]);
                    if (string.IsNullOrEmpty(_FoodLicence))
                        _FoodLicence = Convert.ToString(Session["FoodLicenseImage"]);
                    if (string.IsNullOrEmpty(_BankCheque))
                        _BankCheque = Convert.ToString(Session["BankImage"]); 

                    sql = "update DistParticulars set InvestmentProposed =" + Convert.ToInt32(txtinvestmentpropoed.Value) + ",Turnoverpermonthyear =" + Convert.ToInt32(txtturnover.Value) + ",DistributorsRetailersunderyou =" + Convert.ToInt32(txtunder.Value) + ",NoOfSystems =" + Convert.ToInt32(txtsystems.Value) + ",NewspaperPublished ='" + txtnewspaper.Value.Replace("'", "''") + "',WorkExclusive ='" + ddlWorkExclusively.SelectedValue + "',GST ='" + txtgst.Value.Replace("'", "''") + "',GSTImagePath ='" + _GSTRegistration + "',PAN ='" + txtpan.Value.Replace("'", "''") + "',PANImagePath ='" + _PANNo + "',Aadhar ='" + txtaadhar.Value.Replace("'", "''") + "',AadharImagePath ='" + _AadharNo + "',AdditionalInfo ='" + txtadditional.Value.Replace("'", "''") + "',FoodLicense ='" + txtfoodlicense.Value.Replace("'", "''") + "',FoodLicenseImagePath ='" + _FoodLicence + "',BankName ='" + txtbank.Value.Replace("'", "''") + "',BankChequeScanImagePath ='" + _BankCheque + "',BankBranch ='" + txtbankbranch.Value.Replace("'", "''") + "',AccountNo ='" + txtaccno.Value.Replace("'", "''") + "' where DiNNo='" + txtDinNo.Value + "'";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sql);
                }
            }
            catch (Exception ex)
            {
                Next = "Oops! Error While Inserting Records";
            }
            return Next;
        }

        private string SaveNameOfCompaniesyouRepresented()
        {
            string Saved = "1";
            try
            {
                string sql = "Delete from DistParticulars_CompaniesRepresented where DINNo='" + txtDinNo.Value + "'";
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sql);
                for (int i = 0; i < gridCompaniesRepresented.Rows.Count; i++)
                {
                    TextBox txtname = (gridCompaniesRepresented.Rows[i].FindControl("txtname") as TextBox);
                 //   TextBox txtMar = (GridView4.Rows[index].FindControl("txtMar") as TextBox);
                    TextBox txtsince = (gridCompaniesRepresented.Rows[i].FindControl("txtsince") as TextBox);
                    TextBox txtturnovertwoyear = (gridCompaniesRepresented.Rows[i].FindControl("txtturnovertwoyear") as TextBox);
                    if (string.IsNullOrEmpty(txtturnovertwoyear.Text))
                        txtturnovertwoyear.Text = "0";

                    sql = "insert into DistParticulars_CompaniesRepresented (DINNo,DistId,Name,Since,TurnoverLastTwoYears) values('" + txtDinNo.Value + "'," + Settings.Instance.DistributorID + ",'" + txtname.Text.Replace("'", "''")
                        + "','" + txtsince.Text.Replace("'", "''") + "'," + Convert.ToInt32(txtturnovertwoyear.Text) + ")";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sql);
                }
            }
            catch (Exception ex)
            {
                Saved = "Oops! Error While Inserting Records";
            }
            return Saved;
        }
        private string SavegridDeliveryarrangements()
        {
            string Saved = "1";
            try
            {
                string sql = "Delete from DistParticulars_DeliveryArrangements where DINNo='" + txtDinNo.Value + "'";
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sql);
                for (int i = 0; i < gridDeliveryarrangements.Rows.Count; i++)
                {
                    //if (i == 4)
                    //    break;
                    TextBox txttypeofvehicle = (gridDeliveryarrangements.Rows[i].FindControl("txtVehicleType") as TextBox);
                    TextBox txtnovehicle = (gridDeliveryarrangements.Rows[i].FindControl("txtNoOfVehicle") as TextBox);
                    DropDownList ddlown = (gridDeliveryarrangements.Rows[i].FindControl("ddlownhire") as DropDownList);
                    if (string.IsNullOrEmpty(txtnovehicle.Text))
                        txtnovehicle.Text = "0";

                    sql = "insert into DistParticulars_DeliveryArrangements (DINNo,DistId,VehicleType,VehicleNo,OwnHired) values('" + txtDinNo.Value + "'," + Settings.Instance.DistributorID + ",'" + txttypeofvehicle.Text.Replace("'", "''") + "'," + Convert.ToInt32(txtnovehicle.Text) + ",'" + ddlown.SelectedValue + "')";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sql);
                }
            }
            catch (Exception ex)
            {
                Saved = "Oops! Error While Inserting Records";
            }
            return Saved;
        }
        private string SavegridRetailCustomerUnderYou()
        {
            string Saved = "1";
            try
            {
                string sql = "Delete from DistParticulars_RetailCustomerUnderYou where DINNo='" + txtDinNo.Value + "'";
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sql); 
                for (int i = 0; i < gridRetailCustomerUnderYou.Rows.Count; i++)
                {
                    TextBox txtmarketname = (gridRetailCustomerUnderYou.Rows[i].FindControl("txtmarketname") as TextBox);
                    TextBox txtdistance = (gridRetailCustomerUnderYou.Rows[i].FindControl("txtdistance") as TextBox);
                    TextBox txtretailscounter = (gridRetailCustomerUnderYou.Rows[i].FindControl("txtretailscounter") as TextBox);
                    TextBox txtcounters = (gridRetailCustomerUnderYou.Rows[i].FindControl("txtcounters") as TextBox);
                    TextBox txtturnovermarket = (gridRetailCustomerUnderYou.Rows[i].FindControl("txtturnovermarket") as TextBox);
                    TextBox txtyourturnover = (gridRetailCustomerUnderYou.Rows[i].FindControl("txtyourturnover") as TextBox);
                    if (string.IsNullOrEmpty(txtdistance.Text))
                        txtdistance.Text = "0";
                    if (string.IsNullOrEmpty(txtretailscounter.Text))
                        txtretailscounter.Text = "0";
                    if (string.IsNullOrEmpty(txtcounters.Text))
                        txtcounters.Text = "0";
                    if (string.IsNullOrEmpty(txtturnovermarket.Text))
                        txtturnovermarket.Text = "0";
                    if (string.IsNullOrEmpty(txtyourturnover.Text))
                        txtyourturnover.Text = "0";


                    sql = "insert into DistParticulars_RetailCustomerUnderYou (DINNo,DistId,MarketName,DistanseFromGodownInKm,NoOfRetailCounters,Countersassociated,MarketTurnover,YourTurnOver) values('" + txtDinNo.Value + "'," + Settings.Instance.DistributorID + ",'" + txtmarketname.Text.Replace("'", "''") + "'," + Convert.ToDecimal(txtdistance.Text) + "," + Convert.ToInt32(txtretailscounter.Text) + "," + Convert.ToInt32(txtcounters.Text) + "," + Convert.ToDecimal(txtturnovermarket.Text) + "," + Convert.ToDecimal(txtyourturnover.Text) + ")";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sql);
                }
            }
            catch (Exception ex)
            {
                Saved = "Oops! Error While Inserting Records";
            }
            return Saved;
        }
        private string Savegridprimeretailers()
        {
            string Saved = "1";
            try
            {
                string sql = "Delete from DistParticulars_NamePrimeRetailers where DINNo='" + txtDinNo.Value + "'";
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sql); 
                for (int i = 0; i < gridprimeretailers.Rows.Count; i++)
                {
                    TextBox txtnameofretailer = (gridprimeretailers.Rows[i].FindControl("txtnameofretailer") as TextBox);
                    TextBox txtlocation = (gridprimeretailers.Rows[i].FindControl("txtlocation") as TextBox);
                    sql = "insert into DistParticulars_NamePrimeRetailers (DINNo,DistId,NameOfRetailer,Location) values('" + txtDinNo.Value + "'," + Settings.Instance.DistributorID + ",'" + txtnameofretailer.Text.Replace("'", "''")
                        + "','" + txtlocation.Text.Replace("'", "''") + "')";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sql);
                }
            }
            catch (Exception ex)
            {
                Saved = "Oops! Error While Inserting Records";
            }
            return Saved;
        }
        private string Savegridestimatedturnover()
        {
            string Saved = "1";
            try
            {

                string sql = "Delete from DistParticulars_EstimatedTurnover where DINNo='" + txtDinNo.Value + "'";
                DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sql);
                for (int i = 0; i < gridestimatedturnover.Rows.Count; i++)
                {
                    Label lblcommodity = (gridestimatedturnover.Rows[i].FindControl("lblcommodity") as Label);
                    TextBox txtestimated = (gridestimatedturnover.Rows[i].FindControl("txtestimated") as TextBox);
                    TextBox txtcurrentyeartarget = (gridestimatedturnover.Rows[i].FindControl("txtcurrentyeartarget") as TextBox);
                    if (string.IsNullOrEmpty(txtestimated.Text))
                        txtestimated.Text = "0";
                    if (string.IsNullOrEmpty(txtcurrentyeartarget.Text))
                        txtcurrentyeartarget.Text = "0";

                    sql = "insert into DistParticulars_EstimatedTurnover (DINNo,DistId,NameOfCommodity,TurnOverEstimated,CurrentYearTarget) values('" + txtDinNo.Value + "'," + Settings.Instance.DistributorID + ",'" + lblcommodity.Text.Replace("'", "''") + "'," + Convert.ToDecimal(txtestimated.Text) + "," + Convert.ToDecimal(txtcurrentyeartarget.Text) + ")";
                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, sql);
                }
            }
            catch (Exception ex)
            {
                Saved = "Oops! Error While Inserting Records";
            }
            return Saved;
        }

        private void GetParticularData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Since");
            dt.Columns.Add("TurnoverLastTwoYears");
            dt.Columns.Add("VehicleType");
            dt.Columns.Add("VehicleNo");
            dt.Columns.Add("OwnHired");
            dt.Columns.Add("MarketName");
            dt.Columns.Add("DistanseFromGodownInKm");
            dt.Columns.Add("NoOfRetailCounters");
            dt.Columns.Add("Countersassociated");
            dt.Columns.Add("MarketTurnover");
            dt.Columns.Add("YourTurnOver");
            dt.Columns.Add("NameOfRetailer");
            dt.Columns.Add("Location");
            dt.Columns.Add("NameOfCommodity");
            dt.Columns.Add("TurnOverEstimated");
            dt.Columns.Add("CurrentYearTarget");
            dt.Rows.Add("", "", "", "Trolley", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "Spices", " ", "");
            dt.Rows.Add("", "", "", "Rickshaw", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "Pickle", " ", "");
            dt.Rows.Add("", "", "", "3 Wheeler", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "Papad", " ", "");
            dt.Rows.Add("", "", "", "4 Wheeler", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "Dhoopbatti", " ", "");
            dt.Rows.Add("", "", "", "Others(Specify)", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "Agarbatti", " ", "");
         
            dt.AcceptChanges();

            string Sql = "Select * from DistParticulars where DINNo='" + txtDinNo.Value + "' and IsFinalSaved=0";
            DataTable dtDistParticulars = DbConnectionDAL.GetDataTable(CommandType.Text, Sql);
            if (dtDistParticulars.Rows.Count > 0)
            {
                 Setdata(dtDistParticulars);
                 Sql = "Select * from DistParticulars_CompaniesRepresented where DINNo='" + txtDinNo.Value + "'";
                DataTable dtDistParticulars_CompaniesRepresented = DbConnectionDAL.GetDataTable(CommandType.Text, Sql);
                if (dtDistParticulars_CompaniesRepresented.Rows.Count > 0)
                {
                    gridCompaniesRepresented.DataSource = dtDistParticulars_CompaniesRepresented;
                    gridCompaniesRepresented.DataBind();
                }
                else
                {
                    gridCompaniesRepresented.DataSource = dt;
                    gridCompaniesRepresented.DataBind();
                }

                Sql = "Select * from DistParticulars_RetailCustomerUnderYou where DINNo='" + txtDinNo.Value + "'";
                DataTable dtDistParticulars_RetailCustomerUnderYou = DbConnectionDAL.GetDataTable(CommandType.Text, Sql);
                if (dtDistParticulars_RetailCustomerUnderYou.Rows.Count > 0)
                {
                    gridRetailCustomerUnderYou.DataSource = dtDistParticulars_RetailCustomerUnderYou;
                    gridRetailCustomerUnderYou.DataBind();
                }
                else
                {
                    gridRetailCustomerUnderYou.DataSource = dt;
                    gridRetailCustomerUnderYou.DataBind();
                }
                Sql = "Select * from DistParticulars_NamePrimeRetailers where DINNo='" + txtDinNo.Value + "'";
                DataTable dtDistParticulars_NamePrimeRetailers = DbConnectionDAL.GetDataTable(CommandType.Text, Sql);
                if (dtDistParticulars_NamePrimeRetailers.Rows.Count > 0)
                {
                    gridprimeretailers.DataSource = dtDistParticulars_NamePrimeRetailers;
                    gridprimeretailers.DataBind();
                }
                else
                {
                    gridprimeretailers.DataSource = dt;
                    gridprimeretailers.DataBind();
                }
                Sql = "Select * from DistParticulars_DeliveryArrangements where DINNo='" + txtDinNo.Value + "'";
                DataTable dtDistParticulars_DeliveryArrangements = DbConnectionDAL.GetDataTable(CommandType.Text, Sql);
                if (dtDistParticulars_DeliveryArrangements.Rows.Count > 0)
                {
                    gridDeliveryarrangements.DataSource = dtDistParticulars_DeliveryArrangements;
                    gridDeliveryarrangements.DataBind();
                }
                else
                {
                    dt.Rows.Add("", "", "", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "Sauce", " ", "");

                    dt.AcceptChanges();
                    gridDeliveryarrangements.DataSource = dt;
                    gridDeliveryarrangements.DataBind();
                }
                Sql = "Select * from DistParticulars_EstimatedTurnover where DINNo='" + txtDinNo.Value + "'";
                DataTable dtDistParticulars_EstimatedTurnover = DbConnectionDAL.GetDataTable(CommandType.Text, Sql);
                if (dtDistParticulars_EstimatedTurnover.Rows.Count > 0)
                {
                    gridestimatedturnover.DataSource = dtDistParticulars_EstimatedTurnover;
                    gridestimatedturnover.DataBind();
                }
                else
                {
                    if(dt.Rows.Count==6)
                    {
                        dt.Rows.Add("", "", "", "", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "Vermicelli", " ", "");
                        dt.Rows.Add("", "", "", "", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "Tea", " ", "");
                        dt.Rows.Add("", "", "", "", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "Religious Items", " ", "");
                    }
                    else
                    {
                        dt.Rows.Add("", "", "", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "Sauce", " ", "");
                        dt.Rows.Add("", "", "", "", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "Vermicelli", " ", "");
                        dt.Rows.Add("", "", "", "", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "Tea", " ", "");
                        dt.Rows.Add("", "", "", "", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "Religious Items", " ", "");
                    }
               
                    dt.AcceptChanges();
                    gridestimatedturnover.DataSource = dt;
                    gridestimatedturnover.DataBind();
                }
            }
            else
            {
                gridRetailCustomerUnderYou.DataSource = dt;
                gridRetailCustomerUnderYou.DataBind();
                gridprimeretailers.DataSource = dt;
                gridprimeretailers.DataBind();
                dt.Rows.Add("", "", "", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "Sauce", " ", "");
                dt.Rows.Add("", "", "", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "Sauce", " ", "");
                dt.Rows.Add("", "", "", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "Sauce", " ", "");
                dt.AcceptChanges();
                gridDeliveryarrangements.DataSource = dt;
                gridDeliveryarrangements.DataBind();
                gridCompaniesRepresented.DataSource = dt;
                gridCompaniesRepresented.DataBind();
                dt.Rows.Add("", "", "", "Others(Specify)", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "Sauce", " ", "");
                dt.Rows.Add("", "", "", "Others(Specify)", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "Vermicelli", " ", "");
                dt.Rows.Add("", "", "", "Others(Specify)", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "Tea", " ", "");
                dt.Rows.Add("", "", "", "Others(Specify)", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "Religious Items", " ", "");

                dt.AcceptChanges();
                gridestimatedturnover.DataSource = dt;
                gridestimatedturnover.DataBind();
            }
        }
        private void Clear()
        {
            txtDinNo.Value = "";
            txtfirmname.Value = "";
            txtaddress.Value = "";
            txtcontactPerson.Value = "";
            txtoffice.Value ="";
            txtresidence.Value = "";
            txtemail.Value ="";
            txtmobile.Value = "";
            txtProprieter.Value = "";
            txtfeet.Value ="";
            txtSalesman.Value = "";
            txtothers.Value = "";
            btnprevious2.Visible = false;
            btnFinalSave.Visible = false;
            BtnNext1.Visible = false;
                imgproprieter.Style.Add("display", "none");
                imgProperitory.Style.Add("display", "none");

        }
        private void Setdata(DataTable dt)
        {

            txtfirmname.Value = dt.Rows[0]["FirmName"].ToString();
            txtaddress.Value = dt.Rows[0]["FirmAddress"].ToString();
            txtcontactPerson.Value = dt.Rows[0]["ContactPerson"].ToString();
            txtoffice.Value = dt.Rows[0]["PhoneOffice"].ToString();
            txtresidence.Value = dt.Rows[0]["PhoneResidence"].ToString();
            txtemail.Value = dt.Rows[0]["Email"].ToString();
            txtmobile.Value = dt.Rows[0]["Mobile"].ToString();
            txtProprieter.Value = dt.Rows[0]["PartnerName"].ToString();
            txtfeet.Value = dt.Rows[0]["StorageArea"].ToString();
            txtSalesman.Value = dt.Rows[0]["SalespersonNo"].ToString();
            txtothers.Value = dt.Rows[0]["OtherEmpNo"].ToString();
            if (!string.IsNullOrEmpty(dt.Rows[0]["ProperitoImagePath"].ToString()))
            {
                imgproprieter.Src = dt.Rows[0]["ProperitoImagePath"].ToString();
                imgproprieter.Style.Add("display", "block");
            }
            else
            {
                imgproprieter.Style.Add("display", "none");
            }
            if (!string.IsNullOrEmpty(dt.Rows[0]["ProperitoryImagepath"].ToString()))
            {
                imgProperitory.Src = dt.Rows[0]["ProperitoryImagepath"].ToString();
                imgProperitory.Style.Add("display", "block");
            }
            else
            {
                imgProperitory.Style.Add("display", "none");
            }
           
            DDlStorage.SelectedValue = dt.Rows[0]["StorageOwnRented"].ToString();
            txtinvestmentpropoed.Value = dt.Rows[0]["InvestmentProposed"].ToString();
            txtturnover.Value = dt.Rows[0]["Turnoverpermonthyear"].ToString();
            txtunder.Value = dt.Rows[0]["DistributorsRetailersunderyou"].ToString();
            txtsystems.Value = dt.Rows[0]["NoOfSystems"].ToString();
            txtnewspaper.Value = dt.Rows[0]["NewspaperPublished"].ToString();
            txtgst.Value = dt.Rows[0]["GST"].ToString();
            txtpan.Value = dt.Rows[0]["PAN"].ToString();
            txtaadhar.Value = dt.Rows[0]["Aadhar"].ToString();
            txtadditional.Value = dt.Rows[0]["AdditionalInfo"].ToString();
            txtfoodlicense.Value = dt.Rows[0]["FoodLicense"].ToString();
            txtbank.Value = dt.Rows[0]["BankName"].ToString();
            txtbankbranch.Value = dt.Rows[0]["BankBranch"].ToString();
            txtaccno.Value = dt.Rows[0]["AccountNo"].ToString();
            ddlWorkExclusively.SelectedValue = dt.Rows[0]["WorkExclusive"].ToString();
           
            if (!string.IsNullOrEmpty(dt.Rows[0]["GSTImagePath"].ToString()))
            {
                Session["GSTImage"]=dt.Rows[0]["GSTImagePath"].ToString();
                imgGST.Src = dt.Rows[0]["GSTImagePath"].ToString();
                imgGST.Style.Add("display", "block");
            }
            else
            {
                imgGST.Style.Add("display", "none");
            }


            if (!string.IsNullOrEmpty(dt.Rows[0]["PANImagePath"].ToString()))
            {
                Session["PanImage"] = dt.Rows[0]["PANImagePath"].ToString();
                imgPan.Src = dt.Rows[0]["PANImagePath"].ToString();
                imgPan.Style.Add("display", "block");
            }
            else
            {
                imgPan.Style.Add("display", "none");
            }


            if (!string.IsNullOrEmpty(dt.Rows[0]["AadharImagePath"].ToString()))
            {
                Session["Aadharmage"]= dt.Rows[0]["AadharImagePath"].ToString();
                imgaadhar.Src = dt.Rows[0]["AadharImagePath"].ToString();
                imgaadhar.Style.Add("display", "block");
            }
            else
            {
                imgaadhar.Style.Add("display", "none");
            }




            if (!string.IsNullOrEmpty(dt.Rows[0]["FoodLicenseImagePath"].ToString()))
            {
                Session["FoodLicenseImage"] = dt.Rows[0]["FoodLicenseImagePath"].ToString();
                imgFoodLicence.Src = dt.Rows[0]["FoodLicenseImagePath"].ToString();
                imgFoodLicence.Style.Add("display", "block");
            }
            else
            {
                imgFoodLicence.Style.Add("display", "none");
            }



            if (!string.IsNullOrEmpty(dt.Rows[0]["BankChequeScanImagePath"].ToString()))
            {
                Session["BankImage"] = dt.Rows[0]["BankChequeScanImagePath"].ToString();
                imgbank.Src = dt.Rows[0]["BankChequeScanImagePath"].ToString();
                imgbank.Style.Add("display", "block");
            }
            else
            {
                imgbank.Style.Add("display", "none");
            }

        }
    }
}