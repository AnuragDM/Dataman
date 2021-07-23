<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" CodeBehind="new_form.aspx.cs" Inherits="AstralFFMS.new_form" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>New Form Page</title>
    <link href="Content/bootstrap.css" rel="stylesheet">
    <link href="Content/bootstrap.min.css" rel="stylesheet">
    <link href="plugins/datatables/dataTables.bootstrap.css" rel="stylesheet">
    <link href="Content/bootstrap-responsive.css" rel="stylesheet">
    <link href="Content/bootstrap-responsive.min.css" rel="stylesheet">
    <!-- Tell the browser to be responsive to screen width -->
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
    <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" rel="stylesheet" type="text/css">
    <link href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css" rel="stylesheet" type="text/css">
    <link href="dist/css/AdminLTE.css" rel="stylesheet">
    <link href="dist/css/skins/_all-skins.min.css" rel="stylesheet" type="text/css">
    <script src="plugins/jQuery/jQuery-2.1.4.min.js" type="text/javascript"></script>
    <!-- Bootstrap 3.3.2 JS -->
    <script src="dist/js/bootstrap.min.js"></script>
    <!-- FastClick -->
    <script src="plugins/fastclick/fastclick.min.js" type="text/javascript"></script>
    <!-- AdminLTE App -->
    <script src="dist/js/app.min.js" type="text/javascript"></script>
    <!-- AdminLTE for demo purposes -->
    <link rel="stylsheet" href="//cdn.datatables.net/1.10.20/css/jquery.dataTables.min.css">
    <script src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js" type="text/javascript"></script>
    <script src="https://cdn.datatables.net/1.10.20/js/dataTables.bootstrap4.min.js" type="text/javascript"></script>



    <link href="jqwidgets/styles/jqx.base.css" rel="stylesheet" />
    <script src="jqwidgets/jqxcore.js" type="text/javascript"></script>
    <script type="text/javascript" src="jqwidgets/jqxdata.js"></script>
    <script type="text/javascript" src="jqwidgets/jqxbuttons.js"></script>
    <script type="text/javascript" src="jqwidgets/jqxscrollbar.js"></script>
    <script type="text/javascript" src="jqwidgets/jqxmenu.js"></script>
    <script type="text/javascript" src="jqwidgets/jqxgrid.js"></script>
    <script type="text/javascript" src="jqwidgets/jqxgrid.pager.js"></script>
    <script type="text/javascript" src="jqwidgets/jqxgrid.columnsresize.js"></script>
    <script type="text/javascript" src="jqwidgets/jqxwindow.js"></script>
    <script type="text/javascript" src="jqwidgets/jqxlistbox.js"></script>
    <script type="text/javascript" src="jqwidgets/jqxdropdownlist.js"></script>
    <script type="text/javascript" src="jqwidgets/jqxinput.js"></script>
    <script type="text/javascript" src="jqwidgets/jqxgrid.filter.js"></script>
    <script type="text/javascript" src="jqwidgets/jqxgrid.sort.js"></script>
    <script type="text/javascript" src="jqwidgets/jqxpanel.js"></script>
    <script type="text/javascript" src="jqwidgets/globalization/globalize.js"></script>
    <script type="text/javascript" src="jqwidgets/jqxgrid.selection.js"></script>
    <script type="text/javascript" src="jqwidgets/jqxgrid.grouping.js"></script>
    <script type="text/javascript" src="jqwidgets/demos.js"></script>
    <script type="text/javascript" src="jqwidgets/jqxnotification.js"></script>
    <script type="text/javascript" src="jqwidgets/jqxgrid.aggregates.js"></script>
    <script type="text/javascript" src="jqwidgets/jqxdata.export.js"></script>
    <script type="text/javascript" src="jqwidgets/jqxgrid.export.js"></script>




    <style>
        .bg-yellow, .callout.callout-warning, .alert-warning, .label-warning, .modal-warning .modal-body {
                background-color: #333 !important;
            }
    </style>


    <script type="text/javascript">
        var V1 = "";
        function errormessage(V1) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "error"
            });
            $('#<%=lblmasg.ClientID %>').html(V1);
            $("#messageNotification").jqxNotification("open");

        }
        var V = "";
        function Successmessage(V) {
            $("#messageNotification").jqxNotification({
                width: 300, position: "top-right", opacity: 2,
                autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: "success"
            });
            $('#<%=lblmasg.ClientID %>').html(V);
            $("#messageNotification").jqxNotification("open");
        }

    </script>

    <script>
        $(document).ready(function () {
            Successmessage("Hello Success message");
        });
    </script>

    <style>
         .form-control {
            border-radius: 5px !important;
          }
        .form-control:active, .form-group .form-control:focus {
            box-shadow: 0 0 5px #3c8dbc;
        }
        th{
            text-align: center;
        }
        table#newTab {
            table-layout: fixed;
            width: 100%;
        }
        table#newTab td{
            word-wrap: break-word;
            padding: 7px;
        }
        table#newTab th{
            height: 30px;
        }
        body textarea.form-control{
            height: auto;
        }
    </style>
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <script>
        $(document).ready(function(){
            $("#find").click(function () {
                $("#formSide").toggle();
                $("#tableSide").toggle();
                $(this).text(function (i, text) {
                    return text === "Find" ? "Back" : "Find";
                });
            });
            $('#newTab').DataTable();
        }); 
    </script>
    <script>
        $(document).ready(function () {
            $('#spinner').hide();
        });
    </script>

    
    <script src="https://kit.fontawesome.com/a076d05399.js"></script>
    <link href="css/custom.css" rel="stylesheet">
    <link href="content/style.css" rel="stylesheet">

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div id="spinner" class="spinner" style="display: none;">
        <img id="img-spinner" src="img/waiting.gif" alt="Loading" /><br />
        Loading Data....
    </div>

    <div id="messageNotification">
        <div>
            <asp:Label ID="lblmasg" runat="server"></asp:Label>
        </div>
    </div>





    <div class="form_start">
        <div class="container-fluid">

            <div class="row mt-2">
                <div class="col-lg-4 col-md-4 col-sm-4 col-xs-6"><span class="main_label">Label</span></div>
                <div class="col-lg-4 col-md-4 col-sm-4 col-xs-0"></div>
                <div class="col-lg-4 col-md-4 col-sm-4 col-xs-6 text-right">
                    <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12"></div>
                    <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12"></div>
                    <a id="find" class="col-lg-4 col-md-4 col-sm-4 col-xs-12 btn btn-primary">Find</a>
                </div>
            </div>


            <div id="formSide">

                <div class="row mt-2">
                    <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                        <div class="form-group">
                            <label for="n1">Label1</label><label for="n1" style="color:red; margin-left:3px;">*</label>
                            <input type="text" id="n1" name="name1" class="form-control" />
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                        <div class="form-group">
                            <label for="n2">Label1</label><label for="n2" style="color:red; margin-left:3px;">*</label>
                            <input id="n2" type="text" name="name2" class="form-control" />
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                        <div class="form-group">
                            <label for="n3">Label1</label><label for="n3" style="color:red; margin-left:3px;">*</label>
                            <input type="text" name="name3" id="n3" class="form-control" />
                        </div>
                    </div>
                </div>

                <div class="row mt-2">
                    <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                        <div class="form-group">
                            <label for="sel1">Label1</label><label for="sel1" style="color:red; margin-left:3px;">*</label>
                            <select class="form-control" name="sel1" id="sel1">
                                <option>--Select--</option>
                                <option>A</option>
                                <option>B</option>
                                <option>C</option>
                                <option>D</option>
                            </select>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                        <div class="form-group">
                            <label for="sel2">Label1</label><label for="sel2" style="color:red; margin-left:3px;">*</label>
                            <select class="form-control" name="sel2" id="sel2">
                                <option>--Select--</option>
                                <option>A</option>
                                <option>B</option>
                                <option>C</option>
                                <option>D</option>
                            </select>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                        <div class="form-group">
                            <label for="sel3">Label1</label><label for="sel3" style="color:red; margin-left:3px;">*</label>
                            <select class="form-control" name="sel3" id="sel3">
                                <option>--Select--</option>
                                <option>A</option>
                                <option>B</option>
                                <option>C</option>
                                <option>D</option>
                            </select>
                        </div>
                    </div>
                </div>

                <div class="row mt-2">
                    <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                        <div class="form-group">
                            <label for="n4">Label1</label><label for="n4" style="color:red; margin-left:3px;">*</label>
                            <input type="text" name="inp1" class="form-control" id="n4" />
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                        <div class="form-group">
                            <label for="sel4">Label1</label><label for="sel4" style="color:red; margin-left:3px;">*</label>
                            <select class="form-control" name="sel4" id="sel4">
                                <option>--Select--</option>
                                <option>A</option>
                                <option>B</option>
                                <option>C</option>
                                <option>D</option>
                            </select>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                        <div class="form-group">
                            <label for="date">Date</label><label for="date" style="color:red; margin-left:3px;">*</label>
                            <asp:TextBox ID="toTextBox" class="form-control" runat="server" Style="background-color: white;"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="toTextBox_CalendarExtender" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="toTextBox_CalendarExtender" TargetControlID="toTextBox"></ajaxToolkit:CalendarExtender>

                            


                        </div>
                    </div>
                </div>

                <div class="row mt-2">
                    <div class="col-lg-4 cl-md-6 col-sm-6  col-xs-12">
                            <label for="txt1">Label</label><label for="txt1" style="color:red; margin-left:3px;">*</label>
                        <textarea id="txt1" class="form-control"></textarea>
                    </div>
                    <div class="col-lg-4 cl-md-6 col-sm-6  col-xs-12">
                        <label for="txt2">Label</label><label for="txt2" style="color:red; margin-left:3px;">*</label>
                        <textarea class="form-control" id="txt2"></textarea>
                    </div>
                    <div class="col-lg-4 cl-md-6 col-sm-6  col-xs-12">
                        <label for="txt3">Label</label><label for="txt3" style="color:red; margin-left:3px;">*</label>
                        <textarea class="form-control" id="txt3"></textarea>
                    </div>
                </div>

                <div class="row mt-5 mb-5 bord">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12 pad">
                        <div class="col-lg-3 col-md-6 col-sm-6 col-xs-12">
                            <button class="col-lg-12 col-md-12 col-sm-12 col-xs-12 btn btn-primary mb" id="submit">Submit</button>
                        </div>
                        <div class="col-lg-3 col-md-6 col-sm-6 col-xs-12">
                            <button class="col-lg-12 col-md-12 col-sm-12 col-xs-12 btn btn-danger mb">Delete</button>
                        </div>
                        <div class="col-lg-3 col-md-6 col-sm-6 col-xs-12">
                            <button class="col-lg-12 col-md-12 col-sm-12 col-xs-12 btn btn-success mb">Export</button>
                        </div>
                        <div class="col-lg-3 col-md-6 col-sm-6 col-xs-12">
                            <button class="col-lg-12 col-md-12 col-sm-12 col-xs-12 btn btn-warning mb">Cancel</button>
                        </div>
                    </div>
                </div>

            </div>



            <div id="tableSide" style="display:none;"  class="mt-2">
                <div class="row mt-2 mb-5">
                    <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                        <div class="form-group">
                            <label for="n4">Label1</label><label for="n4" style="color:red; margin-left:3px;">*</label>
                            <input type="text" name="inp1" class="form-control" id="n5" />
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                        <div class="form-group">
                            <label for="sel4">Label1</label><label for="sel4" style="color:red; margin-left:3px;">*</label>
                            <select class="form-control" name="sel4" id="sel5">
                                <option>--Select--</option>
                                <option>A</option>
                                <option>B</option>
                                <option>C</option>
                                <option>D</option>
                            </select>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                        <div class="form-group">
                            <label for="date">Date</label><label for="date" style="color:red; margin-left:3px;">*</label>
                            <asp:TextBox ID="toTextBox1" class="form-control" runat="server" Style="background-color: white;"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="toTextBox1_CalendarExtender" CssClass="orange" Format="dd/MMM/yyyy" runat="server" BehaviorID="toTextBox1_CalendarExtender" TargetControlID="toTextBox1"></ajaxToolkit:CalendarExtender>
                        </div>
                    </div>
                </div>

                <table class="table-bordered table-striped" id="newTab">
                    <thead>
                        <tr>
                            <th>col1</th>
                            <th>col2</th>
                            <th>col3</th>
                            <th>col4</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>val1</td>
                            <td>Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged </td>
                            <td>val1</td>
                            <td>val1</td>
                        </tr>
                        <tr>
                            <td>val1</td>
                            <td>val1</td>
                            <td>val1</td>
                            <td>val1</td>
                        </tr>
                        <tr>
                            <td>val1</td>
                            <td>val1</td>
                            <td>val1</td>
                            <td>val1</td>
                        </tr>
                        <tr>
                            <td>val1</td>
                            <td>val1</td>
                            <td>val1</td>
                            <td>val1</td>
                        </tr>

                    </tbody>
                </table>
            </div>

        </div>
    </div>

</asp:Content>
