@Code
    ViewData("Title") = "CreateQuestions"
End Code

@ModelType StudentsAssessment.SubjectsData

<script src="~/Scripts/jquery-1.12.4.min.js"></script>

<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        $("#divStatusMessage").text(""); //Put it consistently everyhwhere
        //var serviceURL = '/Teachers/GetSubjectsData'; //This should not be inside AJAX call itself
        //var iSel = $('#SelectedClass').val(); //mere $(this).val() does not seem to work
        //if (iSel > 0) {
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
        //{
        //    $("#divStatusMessage").text("Your class list is empty.  Please add some classes to your list first");
        //    $("#trClasses").css("visibility", "hidden");
        //    $("#trSubjects").css("visibility", "hidden");
        //    $("#trDetails").css("visibility", "hidden");
        //}

    });
    //function successFunc(data, status) {
        //JSON members should be same case as how it is sent.  Otherwise, it will not show up
        //$('#SelectedSubject').append($('<option>', {
        //    value: data.Value,
        //    text: data.Text
        //})).append($('<option>', {
        //    value: data.Value,
        //    text: data.Text
        //}))
        //$('#SelectedSubject').empty().append($('<option>', {
        //    value: -1,
        //    text: "Select a subject"
        //}));
        //$("#trClasses").css("visibility", "visible");
        //$("#trSubjects").css("visibility", "visible");
        //$("#trDetails").css("visibility", "visible");

        //if (data.length > 0)
        //{
        //    $('#SelectedSubject').empty();
        //    $.each(data, function (i, item) {
        //        $('#SelectedSubject').append($('<option>', {
        //            value: item.Value,
        //            text: item.Text
        //        }));
        //    });
        //}
        //else
        //{
        //    $("#divStatusMessage").text("Your subject list is empty.  Please contact administrator.  Or,  choose another class.");
        //    //$("#trClasses").css("visibility", "hidden");
        //    $("#trSubjects").css("visibility", "hidden");
        //    $("#trDetails").css("visibility", "hidden");
        //}

    //}

    //function errorFunc() {
    //    alert('class fill error');
    //}

    //function successFunc1(data, status) {
    //    //JSON members should be same case as how it is sent.  Otherwise, it will not show up
    //    $("#trSubjects").css("visibility", "visible");
    //    $("#trDetails").css("visibility", "visible");
    //    $('#GradeLevel').val(data.GradeLevel);
    //}
    //function errorFunc1() {
    //    alert('subjects fill error');
    //}
</script>

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

<link href="~/Content/rvModel/css/styles.css" rel="stylesheet" />
<script src="~/Content/rvModel/js/rv-vanilla-modal.js"></script>

<link href="~/Content/rvModel/css/rvModal_mysettings.css" rel="stylesheet" />
<script src="~/Content/rvModel/js/rvVanillaModal_mysettings.js"></script>

<h2>@Session("Greetings").ToString </h2>

<div class="row">
    <div class="col-sm-3"></div>
    <div class="col-sm-3" style="background-color:#fff;margin-top:50px;position:absolute;padding:5px;width:700px;">
        <div style="margin-left:25px;">
            <h3 style="text-align:center;">QUESTION BANK</h3>
            <br /><br />
            <input type="button" id="Back" name="Back" value="CREATE QUESTION" Class="btn btn-success" style="width:200px;float:right;margin-top:-25px;" data-rv-vanilla-modal="#SomeOther-modal" />
            <hr />
            <br /><br />
            @Using (Html.BeginForm())
                @Html.AntiForgeryToken()
                @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
                @<table id="tblCreateClass" border="0" cellpadding="2" cellspacing="2" style="margin-top:-40px;">
                    @*<colgroup>
                        <col width="200" />
                    </colgroup>*@
                    <tr>
                        <td colspan="2">
                            <div id="divQB" style="height:300px;width:660px;" class="scrollable">
                               This List will be scrollable And will contain question bank questions
                            </div>
                        </td>
                    </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
                    <tr>
                        <td colspan = "2" style="text-align:center;">
                            <input type = "submit" value="ADD" Class="btn btn-success" />
                            <input type = "button" id="Back" name="Back" value="Back" Class="btn btn-success" onclick="location.href='@Url.Action("Index", "Home")'" />

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
<p>&nbsp;</p>
<p>&nbsp;</p><p>&nbsp;</p><p>&nbsp;</p><p>&nbsp;</p><p>&nbsp;</p>
<div id="SomeOther-modal" class="rv-vanilla-modal">
    <div class="rv-vanilla-modal-header group">
        <button class="rv-vanilla-modal-close"><span class="text">×</span></button>
        <h2 class="rv-vanilla-modal-title">CREATE QUESTION</h2>
    </div>
    <div class="rv-vanilla-modal-body">
        <p>
            Enter your name:<br />
            <input id="txtName" type="text" /><br />
            <button id="btnOK" Class="btn btn-success" onclick="var strName=document.getElementById('txtName');divQB.innerText=strName.value;">CREATE QUESTION</button>
            tHIS IS MORE LINE<br/>
            tHIS IS MORE LINE<br />
            tHIS IS MORE LINE<br />
            tHIS IS MORE LINE<br />
            tHIS IS MORE LINE<br />
            tHIS IS MORE LINE<br />
            tHIS IS MORE LINE<br />
            tHIS IS MORE LINE<br />
            tHIS IS MORE LINE<br />
            tHIS IS MORE LINE<br />
            tHIS IS MORE LINE<br />
            tHIS IS MORE LINE<br />
            tHIS IS MORE LINE<br />
            tHIS IS MORE LINE<br />
            tHIS IS MORE LINE<br />
            tHIS IS MORE LINE<br />
            tHIS IS MORE LINE<br />
            tHIS IS MORE LINE<br />
            tHIS IS MORE LINE<br />
            tHIS IS MORE LINE<br />
    </div>
</div>

