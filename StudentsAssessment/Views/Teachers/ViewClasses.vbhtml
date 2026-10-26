@Code
    ViewData("Title") = "ListClasses"
End Code
@Imports System.Data
@ModelType StudentsAssessment.ClassData
<script src="~/Scripts/jquery-1.12.4.min.js"></script>

<script language="javascript" type="text/javascript">
    $(document).ready(function () {
    });

    function FillClassData() {
        var serviceURL = '/Teachers/GetClassData'; //This should not be inside AJAX call itself
        var iSel = $('#SelectedClass').val(); //mere $(this).val() does not seem to work
        $.ajax({
            //type: "POST",
            url: serviceURL,
            //data: param = "",
            data: { iClassID: iSel }, /*The parameter name in the FirstAJAX Action should be "param" only*/
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: successFunc,
            error: errorFunc
        });
    }

    function successFunc(data, status) {
        //JSON members should be same case as how it is sent.  Otherwise, it will not show up
        $('#ClassDuration').val(data.ClassDuration);
        $('#NumCreditHrs').val(data.NumCreditHrs);
        $('#dtStart').text(data.dtStart);
        $('#dtEnd').text(data.dtEnd);
    }

    function errorFunc() {
        alert('error');
    }
</script>

@*<script language="javascript" type="text/javascript">
        $(document).ready(function () {
        });
        function FillClassData() {
            $('#ClassDuration').val("3.5");
            $('#NumCreditHrs').val("120");
            $('#dtStart').text("10/20/2000");
            $('#dtEnd').text("10/25/2000");
        }
    </script>*@

@If Session("LoggedInStudentID") Is Nothing AndAlso Session("LoggedInTeacherID") Is Nothing Then
    'ViewBag.StatusMessage = "You must log in again to continue.  Please click on the appropriate log-in button above"
    @Html.Action("Index", "Home")
End If

@*Comes here only if they are logged in at this point*@
@*<h2>Hello @Model.Firstname, Welcome to Student Assessment Platform!</h2>*@

@*<link rel="stylesheet" media="screen" href="@Url.Content("~/Content/Superfish/src/css/superfish.css")">
    <link href="@Url.Content("~/Content/Superfish/css/superfish-vertical.css")" rel="stylesheet" />
    <script src="@Url.Content("~/Content/Superfish/js/jquery.js")"></script>
    <script src="@Url.Content("~/Content/Superfish/js/superfish.js")"></script>
    <script src="@Url.Content("~/Content/Superfish/js/hoverIntent.js")"></script>*@

<link href="~/Content/Superfish/css/superfish.css" rel="stylesheet" media="screen" />
<link href="~/Content/Superfish/css/superfish-vertical.css" rel="stylesheet" media="screen" />
<script src="~/Content/Superfish/js/jquery.js"></script>
<script src="~/Content/Superfish/js/superfish.js"></script>
<script src="~/Content/Superfish/js/hoverIntent.js"></script>

<!-- initialise Superfish -->
@*<script>

        jQuery(document).ready(function(){
            jQuery('ul.sf-menu').superfish({
            });
        });

    </script>*@
<h2>@Session("Greetings").ToString </h2>

<div class="row">
    <div class="col-sm-3"></div>
    <div class="col-sm-3" style="background-color:#fff;margin-top:50px;position:absolute;padding:5px;">
        <div style="margin-left:25px;">
            <h3 style="text-align:center;">VIEW TEACHER'S CLASSES</h3>
            <br /><br />
            @Using (Html.BeginForm())
                @Html.AntiForgeryToken()
                @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
                 @<div id = "" style="overflow-y:auto;height:250px;width:400px;">
                    <table id="tblCreateClass" border="0" cellpadding="2" cellspacing="2">
    <colgroup>
        <col width="200" />
    </colgroup>
    <tr>
        <td>
            <div style="margin-left:25px;">
                <table id="" cellpacing="5" cellpadding="5" style="border:2px #beb solid;">
                    @If ViewBag.Data IsNot Nothing Then
                        Dim dtDataTable As DataTable = CType(ViewBag.Data, DataTable)
                        @<tr style="background-color:#beb;">
                            @For Each column As DataColumn In dtDataTable.Columns
                                @<td style="font-weight:bold;">@column.ColumnName.ToUpper()</td>
                            Next
                        </tr>

                        If dtDataTable.Rows.Count > 0 Then
                            @For Each Row As DataRow In dtDataTable.Rows
                                @<tr>
                                    @For Each column As DataColumn In dtDataTable.Columns
                                        @<td data-title='@column.ColumnName'>
                                            @Row(column).ToString()
                                        </td>
                                    Next
                                </tr>
                            Next
                Else
                            Dim iCount As Integer = dtDataTable.Columns.Count
                            @<tr>

                                <td colspan='@iCount' style="color:red;">No Data Found.</td>
                            </tr>
                        End If
                    Else
                        If ViewBag.Error IsNot Nothing Then
                            @<tr>
                                <td style="color:red;">
                                    @IIf(ViewBag.Error IsNot Nothing, ViewBag.Error.ToString(), "")
                                </td>
                            </tr>
                        End If
                    End If
                </table>
            </div>
        </td>
    </tr>

    <tr>

        <td colspan="2">
            <div class="field-validation-error">
                @ViewBag.StatusMessage
            </div>

        </td>
    </tr>
</table>
                </div>


End Using
        </div>

    </div>
    <div class="col-sm-3"></div>
    <div class="col-sm-3" style="margin-left:60%;margin-top:50px;position:absolute;">
        @Html.Partial("SideMenu")
    </div>
</div>
