@Code
    ViewData("Title") = "Index"
End Code

@ModelType StudentsAssessment.LoginData

@If Session("LoggedInStudentID") Is Nothing AndAlso Session("LoggedInTeacherID") Is Nothing AndAlso Session("LoggedInAdminID") Is Nothing Then
    'ViewBag.StatusMessage = "You must log in again to continue.  Please click on the appropriate log-in button above"
    @Html.Action("Index", "Home")
End If

@*Comes here only if they are logged in at this point*@
<h2>@Session("Greetings").ToString </h2>

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
<div class="row">
    <div class="col-sm-3"></div>
    <div class="col-sm-3" style="background-color:#fff;margin-top:50px;position:absolute;padding:5px;">
        <div style="margin-left:25px;">
            <h3>YOUR PROFILE</h3>
            <table cellspacing="2" cellpadding="2">
                <colgroup>
                    <col width="250" />
                    <col width="250" />
                </colgroup>
                <tr>
                    <td>FIRSTNAME:</td>
                    <td>@Model.Firstname</td>
                </tr>
                <tr>
                    <td>LASTNAME :  </td>
                    <td>@Model.Lastname</td>
                </tr>
                <tr>
                    <td>CURRENT STATUS:</td>
                    <td>@Model.Status</td>
                </tr>
                @If Model.Status = "TEACHER"                                        
                    @<tr>
                        <td>CURRENT Class:</td>
                        @*<td>@Model.CurrentClassID</td>*@
                        <td>@Session("CurrentClass")</td>
                    </tr>
                    @<tr>
                        <td>CURRENT SUBJECT:</td>
                        @*<td>@Model.CurrentClassID</td>*@
                         <td>@Session("CurrentSubject")</td>
                    </tr>
                    @<tr>
                        <td>CURRENT SCHOOL:</td>
                        @*<td>@Model.CurrentClassID</td>*@
                         <td>@Session("CurrentSchool")</td>
                    </tr>
                End If
            </table>
        </div>
    </div>
    <div class="col-sm-3"></div>
    <div class="col-sm-3" style="margin-left:60%;margin-top:50px;position:absolute;">
        @Html.Partial("SideMenu")
    </div>
</div>
