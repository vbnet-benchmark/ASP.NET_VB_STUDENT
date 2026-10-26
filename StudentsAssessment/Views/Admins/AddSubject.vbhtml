@Code
    ViewData("Title") = "AddSubjects"
End Code

@ModelType StudentsAssessment.SubjectsData
<script src="~/Scripts/jquery-1.12.4.min.js"></script>

<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        //var serviceURL = '/Teachers/GetClassData'; //This should not be inside AJAX call itself
        //var iSel = $('#SelectedClass').val(); //mere $(this).val() does not seem to work
        //if (iSel > 0)
        //{
        //    $.ajax({
        //        //type: "POST",
        //        url: serviceURL,
        //        //data: param = "",
        //        data: { iClassID: iSel }, /*The parameter name in the FirstAJAX Action should be "param" only*/
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: successFunc,
        //        error: errorFunc
        //    });
        //}
        //else
        //    $("#divStatusMessage").text("No class information found.  Please inform your administrator")

    });

    //function successFunc(data, status) {
    //    //JSON members should be same case as how it is sent.  Otherwise, it will not show up
    //    $('#ClassDuration').val(data.ClassDuration);
    //    $('#NumCreditHrs').val(data.NumCreditHrs);
    //    $('#dtStart').text(data.dtStart);
    //    $('#dtEnd').text(data.dtEnd);
    //}

    //function errorFunc() {
    //    alert('class fill error');
    //}
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

@*@If Session("Logged") Is Nothing AndAlso Session("LoggedInTeacherID") Is Nothing Then
        'ViewBag.StatusMessage = "You must log in again to continue.  Please click on the appropriate log-in button above"
        @Html.Action("Index", "Home")
    End If*@

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

@*@Scripts.Render("~/bundles/jqueryui")
    @Styles.Render("~/Content/cssjqueryui")*@
<script src="https://code.jquery.com/jquery-1.12.4.js"></script>
<script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">

<script type="text/javascript">

        $(document).ready(function () {

      $('#datepicker').datepicker();
         //$('input[type=datetime]').datepicker({
         //    dateFormat: "dd/M/yy",
         //    changeMonth: true,
         //    changeYear: true,
         //    yearRange: "-60:+0"
         //});

     });
</script>

<h2>@Session("Greetings").ToString </h2>

<div class="row">
    <div class="col-sm-3"></div>
    <div class="col-sm-3" style="background-color:#fff;margin-top:50px;position:absolute;padding:5px;width:550px;">
        <div style="margin-left:25px;">
            <h3 style="text-align:center;">CREATE A NEW SUBJECT</h3>
            <br /><br />
            @Using (Html.BeginForm())
                @Html.AntiForgeryToken()
                @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
                @<table id="" border="0" cellpadding="2" cellspacing="2" style="margin-top:-40px;">
                    <colgroup>
                        <col width="250" />
                        <col width="300" />
                    </colgroup>
                    <tr>
                        <td>
                            @Html.Label("SUBJECT NAME:", htmlAttributes:=New With {.class = "control-label col-md-2", .style = "width:100%;"})
                        </td>
                        <td>
                            @Html.EditorFor(Function(model) model.Subjectname, New With {.htmlAttributes = New With {.class = "form-control", .style = "width:175px;"}, .type = "text"})
                            @Html.ValidationMessageFor(Function(model) model.Subjectname, "", New With {.class = "text-danger"})
                        </td>
                    </tr>
                    <tr>
                        <td>
                            @Html.Label("SELECT CLASS THE SUBJECT IS TO BE TAUGHT IN:", htmlAttributes:=New With {.class = "control-label col-md-2", .style = "width:100%;"})
                        </td>
                        <td>
                            @Html.DropDownListFor(Function(model) model.SelectedClass, Model.ClassesTobeTaughtInList)
                            @Html.ValidationMessageFor(Function(model) model.SelectedClass, "", New With {.class = "text-danger"})
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align:center;">
                            <input type="submit" value="CREATE NEW" class="btn btn-success" />
                            <input type="button" id="Back" name="Back" value="Back" Class="btn btn-success" onclick="location.href='@Url.Action("Index", "Home")'" />
                        </td>
                    </tr>
                    <tr>

                        <td colspan="2">
                            @If String.IsNullOrEmpty(ViewBag.StatusMessage) = True Then
                                @<div id="divStatusMessage" class="field-validation-error">
                                    @ViewBag.StatusMessage
                                </div>
                            Else
                                @<div id="divStatusMessage" class="field-validation-error">
                                    @Html.Raw(ViewBag.StatusMessage)
                                </div>
                            End If
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
