<%@ Page Title="" Language="C#" MasterPageFile="~/FFMS.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="DepartmentMasterList.aspx.cs" Inherits="AstralFFMS.DepartmentMasterList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--    <script type="text/javascript">
        $(document).ready(function () {
            ReloadGrid();
        });
    </script>
    <script type="text/javascript">
        function ReloadGrid() {
            document.getElementById('JqxGriddiv').style.display = "inline";
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("DepartmentMasterList.aspx/GetDeptData") %>',
                contentType: "application/json; charset=utf-8",
                data: "{}",
                dataType: "json",
                success: function (data) {
                    var source = {
                        localdata: data.d,
                        datatype: "json",
                        datafields:
                        [
                             { name: 'DepId', type: 'int' },
                             { name: 'DepName', type: 'string' },
                             { name: 'SyncId', type: 'string' },
                             { name: 'active', type: 'string' }

                        ],
                        id: "DepId",
                        sortcolumn: 'DepName',
                        sortdirection: 'asc',
                        updaterow: function (rowid, rowdata, commit) {
                            commit(true);
                        }
                    };
                    var dataAdapter = new $.jqx.dataAdapter(source);
                    $("#jqxgrid").jqxGrid(
                     {
                         width: '100%',
                         autoheight: true,
                         source: dataAdapter,
                         sortable: true,
                         filterable: true,
                         showstatusbar: true,
                         statusbarheight: 25,
                         pageable: true,
                         selectionmode: 'multiplecellsextended',
                         columns: [
                                 { text: "Department Name", datafield: "DepName" },
                                 { text: "Sync Id", datafield: "SyncId" },
                                 { text: "Active", datafield: "active" }
                         ]
                     });

                    $("#jqxgrid").on("cellclick", function (event) {
                        var column = event.args.column;
                        var rowindex = event.args.rowindex;
                        var columnindex = event.args.columnindex;
                        if (columnindex == 0 || columnindex == 1 || columnindex == 2) {
                            // open the popup window when the user clicks a button.
                            editrow = rowindex;
                            var offset = $("#jqxgrid").offset();
                            var dataRecord = $("#jqxgrid").jqxGrid('getrowdata', editrow);
                            $("#DepId").val(dataRecord.DepId);
                            var DepId = dataRecord.DepId;
                            var url = "DepartmentMaster.aspx?DepId=" + DepId;
                            window.location.href = url;
                        };
                    });
                }
            });
        }
    </script>

    <div id="JqxGriddiv">
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">Department List</h3>
                <div style="float: right">
                      <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />
                </div>
                <div id="jqxgrid"></div>
            </div>
        </div>
    </div>--%>
    <section class="content">
        <div class="row">
            <div class="col-xs-12">

                <div class="box">
                    <div class="box-header">
                        <h3 class="box-title">Department List</h3>
                        <div style="float: right">
                      <asp:Button Style="margin-right: 5px;" type="button" ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />
                </div>
                    </div>
                    <!-- /.box-header -->
                    <div class="box-body table-responsive">
                        <asp:Repeater ID="rpt" runat="server">
                            <HeaderTemplate>
                                <table id="example1" class="table table-bordered table-striped">
                                    <thead>
                                        <tr>
                                            <th>Department Name</th>
                                            <th>Sync ID</th>
                                            <th>Active</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("DepId") %>' />
                                    <td><a href="<%# String.Format("/DepartmentMaster.aspx?DepId="+ Eval("DepId")) %>" style="text-decoration: none;"><%#Eval("DepName") %></a></td>
                                    <td><a href="<%# String.Format("/DepartmentMaster.aspx?DepId="+ Eval("DepId")) %>" style="text-decoration: none;"><%#Eval("SyncId") %></td>
                                    <td><a href="<%# String.Format("/DepartmentMaster.aspx?DepId="+ Eval("DepId")) %>" style="text-decoration: none;"><%#Eval("Active") %></td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </tbody>     </table>       
                            </FooterTemplate>

                        </asp:Repeater>
                    </div>
                    <!-- /.box-body -->
                </div>
                <!-- /.box -->

            </div>
            <!-- /.col -->
        </div>
        <!-- /.row -->
    </section>
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <!-- SlimScroll -->
    <script src="plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>


    <script type="text/javascript">
        $(function () {
            $("#example1").DataTable();

        });
    </script>
</asp:Content>
