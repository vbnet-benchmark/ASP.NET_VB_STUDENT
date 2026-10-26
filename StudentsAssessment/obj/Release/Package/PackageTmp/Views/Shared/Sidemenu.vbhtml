 <!--In the desired li, put  style="background-color:white;" or something for it to highlight as per the page!-->
@If Session("For") = "STUDENTS" Then
    @<strong>STUDENT TASKS</strong>@<br />@<br />
            @<ul id="StudentTasks" Class="sf-menu sf-vertical" style="width:200px;">
                <!--width here changes that of superfish menu-->
                 <li Class="current">
                     <a href="@Url.Action("Index", "Welcome")">HOME</a>
                 </li>
                <li>
                    <a href="#a">CLASSES</a>
                    <ul>
                        <li><a href="@Url.Action("EnrollinClass", "Students")">ENROLL IN CLASSES</a></li>
                        <li Class="current"><a href="@Url.Action("DisEnrollFromClass", "Students")">DISENROLL FROM CLASSES</a></li>
                        <li Class="current"><a href="@Url.Action("ViewEnrollments", "Students")">VIEW CLASSES</a></li>
                    </ul>
                </li>
                 <li>
                     <a href="#">EDIT PROFILE</a>
                 </li>
                 <li>
                     <a href="#">TAKE TEST</a>
                 </li>
                <li>
                    <a href="@Url.Action("Logout", "Login")">LOG OUT</a>
                </li>
            </ul>
ElseIf Session("For") = "TEACHERS" Then
    @<strong>TEACHER TASKS</strong>@<br />@<br />
            @<ul id="TeacherTasks" Class="sf-menu sf-vertical">
                 <li Class="current">
                     <a href="@Url.Action("Index", "Welcome")">HOME</a>
                 </li>
                <li >
                    <a href = "#a" >CLASS</a>
                    <ul>
                        <li><a href = "@Url.Action("AddClass", "Teachers")">ADD To LIST</a></li>
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
                <li><a href="@Url.Action("CreateQuestions", "Teachers")">CREATE QUESTION</a></li>
                        <li Class="current"><a href="@Url.Action("RemoveQuestions", "Teachers")">REMOVE QUESTION</a></li>
                        <li Class="current"><a href="@Url.Action("ViewQuestions", "Teachers")">VIEW QUESTIONS</a></li>
                    </ul>
                </li>
                <li>
                        <a href="@Url.Action("Logout", "Login")">ASSESSMENT BANK</a>
                    <ul>
                        <li><a href="@Url.Action("CreateAssessment", "Teachers")">CREATE ASSESSMENT</a></li>
                        <li Class="current"><a href="@Url.Action("RemoveAssessment", "Teachers")">REMOVE ASSESSMENT</a></li>
                        <li Class="current"><a href="@Url.Action("ViewAssessments", "Teachers")">VIEW ASSESSMENTS</a></li>
                        <li Class="current"><a href="@Url.Action("ViewAssessments", "Teachers")">SCORE ASSESSMENTS</a></li>
                    </ul>
                </li>
                 <li>
                                    <a href="#">EDIT PROFILE</a>
                 </li>
                <li>
                                    <a href="@Url.Action("Logout", "Login")">LOG OUT</a>
                </li>
            </ul>
ElseIf Session("For") = "ADMINISTRATORS" Then
    @<strong style="margin-left:-100px;">ADMIN TASKS</strong>@<br />@<br />
            @<ul id="AdminTasks" Class="sf-menu sf-vertical" style="width:250px;margin-left:-100px;">
                 <li Class="current">
                     <a href="@Url.Action("Index", "WelcomeAdmin")">HOME</a>
                 </li>
                <li>
                    <a href="#a">MANAGE CLASSES</a>
                    <ul>
                        <li><a href="@Url.Action("AddClass", "Admins")">ADD CLASS</a></li>
                        <li Class="current"><a href="@Url.Action("RemoveClass", "Admins")">REMOVE CLASS</a></li>
                        <li Class="current"><a href="@Url.Action("EditClass", "Admins")">EDIT CLASS INFO</a></li>
                        <li Class="current"><a href="@Url.Action("ViewClasses", "Admins")">VIEW CLASSES</a></li>
                        @*<li Class="current"><a href="@Url.Action("AssignToTeachers", "Admins")">ASSIGN TO TEACHERS</a></li>*@
                    </ul>
                </li>
                <li>
                    <a href="#">MANAGE SUBJECTS</a>
                    <ul>
                        <li><a href="@Url.Action("AddSubject", "Admins")">ADD SUBJECT</a></li>
                        <li Class="current"><a href="@Url.Action("RemoveSubject", "Admins")">REMOVE SUBJECT</a></li>
                        <li Class="current"><a href="@Url.Action("EditSubject", "Admins")">EDIT SUBJECT INFO</a></li>
                        <li Class="current"><a href="@Url.Action("ViewSubjects", "Admins")">VIEW SUBJECTS</a></li>
                    </ul>
                </li>
                <li>
                    <a href="#">MANAGE STUDENTS</a>
                    <ul>
                        <li><a href="@Url.Action("AddStudent", "Admins")">ADD STUDENT</a></li>
                        <li Class="current"><a href="@Url.Action("RemoveStudent", "Admins")">REMOVE STUDENT</a></li>
                        <li Class="current"><a href="@Url.Action("EditStudent", "Admins")">EDIT STUDENT INFO</a></li>
                        <li Class="current"><a href="@Url.Action("ViewStudents", "Admins")">VIEW STUDENTS</a></li>
                    </ul>
                </li>
                 <li>
                     <a href="#">MANAGE TEACHERS</a>
                     <ul>
                         <li><a href="@Url.Action("AddTeacher", "Admins")">ADD TEACHER</a></li>
                         <li Class="current"><a href="@Url.Action("RemoveTeacher", "Admins")">REMOVE TEACHER</a></li>
                         <li Class="current"><a href="@Url.Action("EditTeacher", "Admins")">EDIT TEACHER INFO</a></li>
                         <li Class="current"><a href="@Url.Action("ViewTeachers", "Admins")">VIEW TEACHERS</a></li>
                         <li Class="current"><a href="@Url.Action("AssignToTeachers", "Admins")">ASSIGN CLASSES</a></li>
                     </ul>
                 </li>
                <li>
                    <a href="@Url.Action("Logout", "Login")">MANAGE PROFILES</a>
                    <ul>
                        <li><a href="@Url.Action("CreateAssessment", "Admins")">ADD/REMOVE PROFILE SECTIONS</a></li>
                        <li><a href="@Url.Action("CreateAssessment", "Admins")">REMOVE FROM PROFILES</a></li>
                        <li Class="current"><a href="@Url.Action("RemoveAssessment", "Admins")">EDIT SPECIFIC PROFILE</a></li>
                        <li Class="current"><a href="@Url.Action("ViewAssessments", "Admins")">VIEW PROFILES</a></li>
                    </ul>
                </li>
                 <li>
                     <a href="@Url.Action("Logout", "Login")">COMMUNITY BANK</a>
                     <ul>
                         <li><a href="@Url.Action("CreateQuestions", "Admins")">CREATE QUESTION</a></li>
                         <li Class="current"><a href="@Url.Action("RemoveQuestions", "Admins")">REMOVE QUESTION</a></li>
                         <li Class="current"><a href="@Url.Action("ViewQuestions", "Admins")">VIEW QUESTIONS</a></li>
                     </ul>
                 </li>
                 <li>
                     <a href="@Url.Action("Logout", "Login")">ANNOUNCEMENTS AND NEWS</a>
                     <ul>
                         <li><a href="@Url.Action("CreateAssessment", "Admins")">SEND ANNOUNCEMENTS</a></li>
                         <li><a href="@Url.Action("CreateAssessment", "Admins")">VIEW ANNOUNCEMENTS</a></li>
                     </ul>
                 </li>
                <li>
                    <a href="@Url.Action("Logout", "Login")">LOG OUT</a>
                </li>
            </ul>
End If
