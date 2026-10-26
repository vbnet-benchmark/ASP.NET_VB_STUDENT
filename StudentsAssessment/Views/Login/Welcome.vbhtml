@Code
    ViewData("Title") = "Show"
End Code

@ModelType StudentsAssessment.LoginData

@If Session("LoggedInStudentID") Is Nothing AndAlso Session("LoggedInTeacherID") Is Nothing Then
    'ViewBag.StatusMessage = "You must log in again to continue.  Please click on the appropriate log-in button above"
   @Html.Action("Index", "Home")
End If

@*Comes here only if they are logged in at this point*@
<h2>Hello @Model.Firstname, Welcome to Student Assessment Platform!</h2>

@*Not working if given this way*@
@*<link rel="stylesheet" media="screen" href="@Url.Content("~/Content/Superfish/src/css/superfish.css")">
<link href="@Url.Content("~/Content/Superfish/css/superfish-vertical.css")" rel="stylesheet" />
<script src="@Url.Content("~/Content/Superfish/js/jquery.js")"></script>
<script src="@Url.Content("~/Content/Superfish/js/superfish.js")"></script>
<script src="@Url.Content("~/Content/Superfish/js/hoverIntent.js")"></script>*@

@*Included in _Layout.vbhtml so it is available here*@
@*<link href="~/Content/Superfish/css/superfish.css" rel="stylesheet" media="screen" />
<link href="~/Content/Superfish/css/superfish-vertical.css" rel="stylesheet" media="screen" />
<script src="~/Content/Superfish/js/jquery.js"></script>
<script src="~/Content/Superfish/js/superfish.js"></script>
<script src="~/Content/Superfish/js/hoverIntent.js"></script>*@

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
                        <td>LASTNAME:</td>
                        <td>@Model.Lastname</td>
                    </tr>
                    <tr>
                        <td>CURRENT STATUS:</td>
                        <td>@Model.Status</td>
                    </tr>
                </table>
            </div>
            
    </div>
    <div class="col-sm-3"></div>
    <div class="col-sm-3" style="margin-left:60%;margin-top:50px;position:absolute;">
        <!--In the desired li, put  style="background-color:white;" or something for it to highlight as per the page!-->
        @If Session("For") = "STUDENTS" Then
            @<strong>STUDENT TASKS</strong>@<br/>@<br />
            @<ul id="StudentTasks" Class="sf-menu sf-vertical" style="width:200px;"><!--width here changes that of superfish menu-->
                <li Class="current">
                    <a href="#a">ENROLL IN CLASSES</a>
                </li>
                <li>
                    <a href="#">SELECT SUBJECTS</a>
                </li>
                <li>
                    <a href="#">TAKE TEST</a>
                <li>
                    <a href="#">REVIEW ENROLLMENTS</a>
                </li>
                 <li>
                     <a href="@Url.Action("Logout", "Login")">LOG OUT</a>
                 </li>
            </ul>
        ElseIf Session("For") = "TEACHERS" Then
            @<strong>TEACHER TASKS</strong>@<br />@<br />
            @<ul id="TeacherTasks" Class="sf-menu sf-vertical">
                <li Class="current">
                    <a href="#a">CLASS</a>
                    <ul>
                        <li><a href="@Url.Action("AddClass", "Teachers")">ADD TO LIST</a></li>
                        <li Class="current"><a href="@Url.Action("RemoveClass", "Teachers")">REMOVE FROM LIST</a></li>
                        <li Class="current"><a href="@Url.Action("ViewClasses", "Teachers")">LIST CLASSES</a></li>
                    </ul>
                </li>
                <li>
                    <a href="#">SUBJECT</a>
                    <ul>
                        <li>
                            <a href="@Url.Action("AddSubjectToClass", "Teachers")">ADD TO CLASS</a>
                        </li>
                        <li>
                            <a href="@Url.Action("RemoveSubjectFromClass", "Teachers")">REMOVE FROM CLASS</a>
                        </li>
                        <li Class="current"><a href="@Url.Action("ViewSubjects", "Teachers")">VIEW SUBJECTS</a></li>
                    </ul>
                </li>
                <li>
                    <a href="#">STUDENT</a>
                    <ul>
                        <li>
                            <a href="@Url.Action("AddStudentToClass", "Teachers")">ADD TO CLASS</a>
                        </li>
                        <li>
                            <a href="@Url.Action("RemoveStudentFromClass", "Teachers")">REMOVE FROM CLASS</a>
                        </li>
                        <li Class="current"><a href="@Url.Action("ViewStudents", "Teachers")">VIEW STUDENTS</a></li>
                    </ul>
                </li>
                <li>
                     <a href="@Url.Action("Logout", "Login")">QUESTION BANK</a>
                    <ul>
                        <li><a href="@Url.Action("CreateQuestion", "Teachers")">CREATE QUESTION</a></li>
                        <li Class="current"><a href="@Url.Action("RemoveQuestion", "Teachers")">REMOVE QUESTION</a></li>
                        <li Class="current"><a href="@Url.Action("ViewQuestions", "Teachers")">VIEW QUESTIONS</a></li>
                    </ul>
                 </li>
                 <li>
                     <a href="@Url.Action("Logout", "Login")">ASSESSMENT BANK</a>
                     <ul>
                         <li><a href="@Url.Action("CreateAssessment", "Teachers")">CREATE ASSESSMENT</a></li>
                         <li Class="current"><a href="@Url.Action("RemoveAssessment", "Teachers")">REMOVE ASSESSMENT</a></li>
                         <li Class="current"><a href="@Url.Action("ViewAssessments", "Teachers")">VIEW ASSESSMENTS</a></li>
                     </ul>
                 </li>
                 <li>
                     <a href="@Url.Action("Logout", "Login")">LOG OUT</a>
                 </li>
            </ul>
        ElseIf Session("For") = "ADMINISTRATORS" Then
            @<strong>ADMIN TASKS</strong>@<br />@<br />
            @<ul id="AdminTasks" Class="sf-menu sf-vertical">
                <li Class="current">
                    <a href="#a">CLASS</a>
                    <ul>
                        <li><a href="@Url.Action("AddClass", "Admins")">ADD TO SYSTEM</a></li>
                        <li Class="current"><a href="@Url.Action("RemoveClass", "Admins")">REMOVE FROM SYSTEM</a></li>
                        <li Class="current"><a href="@Url.Action("ViewClasses", "Admins")">LIST ALL CLASSES</a></li>
                    </ul>
                </li>
                <li>
                    <a href="#">SUBJECT</a>
                    <ul>
                        <li>
                            <a href="@Url.Action("AddSubjectToClass", "Admins")">ADD TO CLASS</a>
                        </li>
                        <li>
                            <a href="@Url.Action("RemoveSubjectFromClass", "Admins")">REMOVE FROM CLASS</a>
                        </li>
                        <li Class="current"><a href="@Url.Action("ViewSubjects", "Admins")">VIEW SUBJECTS</a></li>
                    </ul>
                </li>
                <li>
                    <a href="#">STUDENT</a>
                    <ul>
                        <li>
                            <a href="@Url.Action("AddStudentToClass", "Admins")">ADD TO CLASS</a>
                        </li>
                        <li>
                            <a href="@Url.Action("RemoveStudentFromClass", "Admins")">REMOVE FROM CLASS</a>
                        </li>
                        <li Class="current"><a href="@Url.Action("ViewStudents", "Admins")">VIEW STUDENTS</a></li>
                    </ul>
                </li>
                <li>
                    <a href="@Url.Action("Logout", "Login")">QUESTION BANK</a>
                    <ul>
                        <li><a href="@Url.Action("CreateQuestion", "Admins")">CREATE QUESTION</a></li>
                        <li Class="current"><a href="@Url.Action("RemoveQuestion", "Admins")">REMOVE QUESTION</a></li>
                        <li Class="current"><a href="@Url.Action("ViewQuestions", "Admins")">VIEW QUESTIONS</a></li>
                    </ul>
                </li>
                <li>
                    <a href="@Url.Action("Logout", "Login")">ASSESSMENT BANK</a>
                    <ul>
                        <li><a href="@Url.Action("CreateAssessment", "Admins")">CREATE ASSESSMENT</a></li>
                        <li Class="current"><a href="@Url.Action("RemoveAssessment", "Admins")">REMOVE ASSESSMENT</a></li>
                        <li Class="current"><a href="@Url.Action("ViewAssessments", "Admins")">VIEW ASSESSMENTS</a></li>
                    </ul>
                </li>
                 <li>
                     <a href="@Url.Action("Logout", "Login")">COMMUNITY BANK</a>
                 </li>
                <li>
                    <a href="@Url.Action("Logout", "Login")">LOG OUT</a>
                </li>
            </ul>
        End If
    </div>
</div>
