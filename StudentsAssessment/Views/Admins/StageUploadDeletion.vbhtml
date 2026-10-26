@Code
    ViewData("Title") = "StageUploadDeletion"
End Code

<script src="~/Scripts/jquery-1.12.4.min.js"></script>

<h2>@Session("Greetings").ToString </h2>

<div class="row">
    <div class="col-sm-3"></div>
    <div class="col-sm-3" style="background-color:#fff;margin-top:50px;position:absolute;padding:5px;width:550px;">
        <div style="margin-left:25px;">
            <h3 style="text-align:center;">CONFIRM UPLOAD DELETION</h3>
            <br />
            <p>You are about to remove: <strong>@ViewBag.StagedFileName</strong></p>
            @Using (Html.BeginForm("ConfirmUploadDeletion", "Admins"))
                @Html.AntiForgeryToken()
                <input type="submit" value="CONFIRM DELETE" class="btn btn-success" />
                <input type="button" value="Back" class="btn btn-success" onclick="location.href='@Url.Action("ManageUploads", "Admins")'" />
            End Using
        </div>
    </div>
</div>
