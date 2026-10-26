@Code
    ViewData("Title") = "AddStudenttoClass"
End Code

@ModelType StudentsAssessment.StudentData 
<script src="~/Scripts/jquery-1.12.4.min.js"></script>

<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        var serviceURL = '/Teachers/GetStudentsData'; //This should not be inside AJAX call itself
        var iSel = $('#SelectedClass').val(); //mere $(this).val() does not seem to work
        if (iSel > 0) {
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
        else
        {
            $("#divStatusMessage").text("Your class list is empty.  Please add some classes to your list first");
            $("#trClasses").css("visibility", "hidden");
            $("#trStudents").css("visibility", "hidden");
            $("#trDetails").css("visibility", "hidden");

        }
            
    });

    function FillStudentData() {
        var serviceURL = '/Teachers/GetStudentData'; //This should not be inside AJAX call itself
        var iSel = $('#SelectedStudent').val(); //mere $(this).val() does not seem to work
        if (iSel > 0) {
            $.ajax({
                //type: "POST",
                url: serviceURL,
                //data: param = "",
                data: { iStudentID: iSel }, /*The parameter name in the FirstAJAX Action should be "param" only*/
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: successFunc1,
                error: errorFunc1
            });
        }
        else
        {
            $("#divStatusMessage").text("Your students list is empty.  Please contact administrator.  Or,  choose another class.");
            //$("#trClasses").css("visibility", "hidden");
            $("#trStudents").css("visibility", "hidden");
            $("#trDetails").css("visibility", "hidden");
        }
    }
    function FillStudentsData() {
        var serviceURL = '/Teachers/GetStudentsData'; //This should not be inside AJAX call itself
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
        //$('#SelectedSubject').append($('<option>', {
        //    value: data.Value,
        //    text: data.Text
        //})).append($('<option>', {
        //    value: data.Value,
        //    text: data.Text
        //}))

        $("#trClasses").css("visibility", "visible");
        $("#trStudents").css("visibility", "visible");
        $("#trDetails").css("visibility", "visible");

        if (data.length > 0) {
            $('#SelectedStudent').empty();
            $.each(data, function (i, item) {
                $('#SelectedStudent').append($('<option>', {
                    value: item.Value,
                    text: item.Text
                }));
            });
        }
        else {
            $("#divStatusMessage").text("Your students list is empty.  Please contact administrator.  Or,  choose another class.");
            //$("#trClasses").css("visibility", "hidden");
            $("#trStudents").css("visibility", "hidden");
            $("#trDetails").css("visibility", "hidden");
        }

    }

    function errorFunc() {
        alert('class fill error');
    }

    function successFunc1(data, status) {
        //JSON members should be same case as how it is sent.  Otherwise, it will not show up
        $("#trStudents").css("visibility", "visible");
        $("#trDetails").css("visibility", "visible");
        $('#Joindate').val(data.Joindate);
    }
    function errorFunc1() {
        alert('students fill error');
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
            <h3 style="text-align:center;">ADD STUDENT TO CLASS</h3>
            <br /><br />
            @Using (Html.BeginForm())
                @Html.AntiForgeryToken()
                @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
                @<table id="tblCreateClass" border="0" cellpadding="2" cellspacing="2" style="margin-top:-40px;">
                    <colgroup>
                        <col width="200" />
                    </colgroup>
                    <tr id="trClasses">
                        <td>
                            @Html.LabelFor(Function(model) model.TaughtClassesList, htmlAttributes:=New With {.class = "control-label col-md-2", .style = "width:100%;"})
                        </td>
                        <td>
                            @Html.DropDownListFor(Function(model) model.SelectedClass, Model.TaughtClassesList, New With {.onchange = "FillStudentsData();"})
                            @Html.ValidationMessageFor(Function(model) model.SelectedClass, "", New With {.class = "text-danger"})
                        </td>
                    </tr>
                    <tr id="trStudents">
                        <td>
                            @Html.LabelFor(Function(model) model.StudentsList, htmlAttributes:=New With {.class = "control-label col-md-2", .style = "width:100%;"})
                        </td>
                        <td>
                            @Html.DropDownListFor(Function(model) model.SelectedStudent, Model.StudentsList, New With {.onchange = "FillStudentData();"})
                            @Html.ValidationMessageFor(Function(model) model.StudentsList, "", New With {.class = "text-danger"})
                        </td>
                    </tr>
                    <tr id="trDetails">
                        <td colspan="2">
                            <h3 style="text-align:center;">STUDENT DETAILS</h3>
                            <table id="">
                                <tr>
                                    <td>
                                        @Html.LabelFor(Function(model) model.Joindate, htmlAttributes:=New With {.class = "control-label col-md-2", .style = "width:100%;"})
                                    </td>
                                    <td>
                                        @Html.EditorFor(Function(model) model.Joindate, New With {.htmlAttributes = New With {.class = "form-control", .style = "width:100%;"}})
                                        @Html.ValidationMessageFor(Function(model) model.Joindate, "", New With {.class = "text-danger"})
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align:center;">
                            <input type="submit" value="ADD" Class="btn btn-success" />
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