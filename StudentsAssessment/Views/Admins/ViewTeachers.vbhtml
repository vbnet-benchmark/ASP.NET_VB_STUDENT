@Code
    ViewData("Title") = "ViewTeachers"
End Code

@Imports System.Data
@ModelType StudentsAssessment.TeacherData
<script src="~/Scripts/jquery-1.12.4.min.js"></script>
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

<h2>@Session("Greetings").ToString </h2>

<div class="row">
    <div class="col-sm-3"></div>
    <div class="col-sm-3" style="background-color:#fff;margin-top:50px;position:absolute;padding:5px;width:550px;">
        <div style="margin-left:25px;">
            <h3 style="text-align:center;">VIEW TEACHERS INFORMATION</h3>
            <br /><br />
            @Using (Html.BeginForm())
                @Html.AntiForgeryToken()
                @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
                @<table id="tblCreateClass" border="0" cellpadding="2" cellspacing="2" style="margin-top:-40px;">
                    <colgroup>
                        <col width="200" />
                    </colgroup>
                    <tr>
                        <td>
                            <div style="margin-left:25px;">
                                <div id="" style="overflow-y:auto;height:250px;width:400px;">
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
            End Using
        </div>

    </div>
    <div class="col-sm-3"></div>
    <div class="col-sm-3" style="margin-left:60%;margin-top:50px;position:absolute;">
        @Html.Partial("SideMenu")
    </div>
</div>