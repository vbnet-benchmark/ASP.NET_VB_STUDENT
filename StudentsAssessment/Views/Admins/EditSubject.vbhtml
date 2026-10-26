@Code
    ViewData("Title") = "EditSubject"
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

    function SelectAssocClass() {
        $("#divStatusMessage").text(""); //Put it consistently everyhwhere
        var serviceURL = '/Admins/GetAssocClass'; //This should not be inside AJAX call itself
        var iSel = $('#SelectedSubject').val(); //mere $(this).val() does not seem to work
        $.ajax({
            //type: "POST",
            url: serviceURL,
            //data: param = "",
            data: { iSubjectID: iSel }, /*The parameter name in the FirstAJAX Action should be "param" only*/
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: successFunc,
            error: errorFunc
        });
    }

    function successFunc(data, status) {
        //JSON members should be same case as how it is sent.  Otherwise, it will not show up
        //alert(data.classID);
        if (data != null) {
            if (data.classID > 0) {

                //$('select').prop('selectedIndex', data.classID); // select 4th option
                $('#SelectedClass').val(data.classID);
            }
        }
       
    }

    function errorFunc() {
        alert('class fill error');
    }
</script>

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
        <div class="col-sm-3" style="background-color:#fff;margin-top:50px;position:absolute;padding:5px;width:500px;">
        <div style="margin-left:25px;">
            <h3 style="text-align:center;">UPDATE EXISTING SUBJECT</h3>
            <br /><br />
            @Using (Html.BeginForm())
                @Html.AntiForgeryToken()
                @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
                @<table id="tblCreateClass" border="0" cellpadding="2" cellspacing="2" style="margin-top:-40px;">
                    <colgroup>
                        <col width="600" />
                        <col width="200" />
                    </colgroup>
                    <tr>
                        <td>
                            @Html.Label("SELECT A SUBJECT TO UPDATE:", htmlAttributes:=New With {.class = "control-label col-md-2", .style = "width:100%;"})

                            @*@Html.LabelFor(Function(model) model.ClassesList, htmlAttributes:=New With {.class = "control-label col-md-2", .style = "width:100%;"})*@
                        </td>
                        <td>
                            @Html.DropDownListFor(Function(model) model.SelectedSubject, Model.SubjectsList, New With {.onchange = "SelectAssocClass();"})
                            @Html.ValidationMessageFor(Function(model) model.SelectedSubject, "", New With {.class = "text-danger"})
                        </td>
                    </tr>

                     <tr>
                         <td>
                             @Html.Label("SELECT A CLASS IN WHICH SUBJECT IS TO BE TAUGHT:", htmlAttributes:=New With {.class = "control-label col-md-2", .style = "width:100%;"})
                         </td>
                         <td>
                             @Html.DropDownListFor(Function(model) model.SelectedClass, Model.ClassesTobeTaughtInList, New With {.onchange = "LoadClassDetails();"})
                             @Html.ValidationMessageFor(Function(model) model.SelectedClass, "", New With {.class = "text-danger"})
                         </td>
                     </tr>
                    


                    <tr>
                        <td colspan="2" style="text-align:center;">
                            <input type="submit" value="UPDATE SUBJECT" class="btn btn-success" />
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


