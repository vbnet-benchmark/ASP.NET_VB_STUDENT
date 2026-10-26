@Code
    ViewData("Title") = "Index"
End Code

<h2>HOME - MAIN SCREEN</h2>

<div class="row">
    <div class="col-sm-3" style="margin-top:50px;">
        <button type="button" value="" onclick="location.href='@Url.Action("TakeTest", "Home")'" class="btn btn-success">TAKE TEST</button>
    </div>
    <div class="col-sm-3" style="margin-top:100px;">
         <button type="button" value="" onclick="location.href='@Url.Action("TeacherLogin", "Home")'" class="btn btn-success">TEACHER LOG-IN</button>
    </div>
    <div class="col-sm-3" style="margin-top:100px;">
        <button type="button" value="" onclick="location.href='@Url.Action("StudentLogin", "Home")'" class="btn btn-success">STUDENT LOG-IN</button>
    </div>
    <div class="col-sm-3" style="margin-top:50px;">
        <button type="button" value="" onclick="location.href='@Url.Action("CreateAccounts", "Home")'" class="btn btn-success">CREATE ACCOUNTS</button>
    </div>
    @*<div class="col-sm-3" style="margin-top:50px;">
        <button type="button" value="" onclick="location.href='@Url.Action("AdminLogin", "Home")'" class="btn btn-success">CREATE ACCOUNTS</button>
    </div>*@
</div>