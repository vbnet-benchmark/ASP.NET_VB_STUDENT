@Code
    ViewData("Title") = "AddClasstoSystem"
End Code

@ModelType StudentsAssessment.ClassData
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
    <div class="col-sm-3" style="background-color:#fff;margin-top:50px;position:absolute;padding:5px;">
        <div style="margin-left:25px;">
            <h3 style="text-align:center;">CREATE A NEW CLASS</h3>
            <br /><br />
            @Using (Html.BeginForm())
                @Html.AntiForgeryToken()
                @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
                @<table id="tblCreateClass" border="0" cellpadding="2" cellspacing="2" style="margin-top:-40px;">
                    <colgroup>
                        <col width="300" />
                        <col width="300" />
                    </colgroup>
                     <tr>
                         <td>
                             @Html.Label("CLASS NAME:", htmlAttributes:=New With {.class = "control-label col-md-2", .style = "width:100%;"})
                             @*@Html.LabelFor(Function(model) model.Classname, htmlAttributes:=New With {.class = "control-label col-md-2", .style = "width:100%;"})*@
                         </td>
                         <td>
                             @Html.EditorFor(Function(model) model.Classname, New With {.htmlAttributes = New With {.class = "form-control", .style = "width:175px;"}, .type = "text"})
                             @Html.ValidationMessageFor(Function(model) model.Classname, "", New With {.class = "text-danger"})
                         </td>
                     </tr>
                     <tr>
                         <td>
                             @Html.LabelFor(Function(model) model.NumCreditHrs, htmlAttributes:=New With {.class = "control-label col-md-2", .style = "width:100%;"})
                         </td>
                         <td>
                             @Html.EditorFor(Function(model) model.NumCreditHrs, New With {.htmlAttributes = New With {.class = "form-control", .style = "width:75px;"}, .type = "text"})
                             @Html.ValidationMessageFor(Function(model) model.NumCreditHrs, "", New With {.class = "text-danger"})
                         </td>
                     </tr>
                    <tr>
                        <td>
                            @Html.LabelFor(Function(model) model.StartDate, htmlAttributes:=New With {.class = "control-label col-md-2", .style = "width:100%;"})
                        </td>
                        <td>
                           @Html.EditorFor(Function(model) model.StartDate, New With {.htmlAttributes = New With {.class = "form-control", .style = "width:175px;"}, .type = "text"})
                        @Html.ValidationMessageFor(Function(model) model.StartDate, "", New With {.class = "text-danger"})
                        </td>
                    </tr>

                    <tr>
                        <td>
                            @Html.LabelFor(Function(model) model.EndDate, htmlAttributes:=New With {.class = "control-label col-md-2", .style = "width:100%;"})
                        </td>
                        <td>
                           @Html.EditorFor(Function(model) model.EndDate, New With {.htmlAttributes = New With {.class = "form-control", .style = "width:175px;"}, .type = "text"})
                        @Html.ValidationMessageFor(Function(model) model.EndDate, "", New With {.class = "text-danger"})
                        </td>
                    </tr>

                    <tr>
                        <td>
                            @Html.Label("CLASS DURATION IN MINUTES:", htmlAttributes:=New With {.class = "control-label col-md-2", .style = "width:100%;"})
                        </td>
                        <td>
                            @Html.EditorFor(Function(model) model.ClassDuration, New With {.htmlAttributes = New With {.class = "form-control", .style = "width:75px;"}})
                            @Html.ValidationMessageFor(Function(model) model.ClassDuration, "", New With {.class = "text-danger"})
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
