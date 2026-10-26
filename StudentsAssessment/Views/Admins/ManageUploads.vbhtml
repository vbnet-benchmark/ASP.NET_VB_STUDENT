@Code
    ViewData("Title") = "ManageUploads"
End Code

<script src="~/Scripts/jquery-1.12.4.min.js"></script>

<h2>@Session("Greetings").ToString </h2>

<div class="row">
    <div class="col-sm-3"></div>
    <div class="col-sm-3" style="background-color:#fff;margin-top:50px;position:absolute;padding:5px;width:550px;">
        <div style="margin-left:25px;">
            <h3 style="text-align:center;">MANAGE UPLOADED IMPORT FILES</h3>
            <br />
            @If ViewBag.RegisteredFiles IsNot Nothing Then
                @<table id="tblUploads" border="0" cellpadding="2" cellspacing="2">
                    @For Each strFileName As String In ViewBag.RegisteredFiles
                        @<tr>
                            <td>@strFileName</td>
                            <td>
                                @Using (Html.BeginForm("DeleteUploadedFile", "Admins"))
                                    @Html.AntiForgeryToken()
                                    @<input type="hidden" name="fileName" value="@strFileName" />
                                    @<input type="submit" value="DELETE" class="btn btn-success" />
                                End Using
                            </td>
                        </tr>
                    Next
                </table>
            End If
            <br />
            @Using (Html.BeginForm("DeleteUploadedFile", "Admins"))
                @Html.AntiForgeryToken()
                <label>Delete a specific upload by name:</label>
                <input type="text" name="fileName" />
                <input type="submit" value="DELETE BY NAME" class="btn btn-success" />
            End Using
            <br />
            @Using (Html.BeginForm("DeleteMultipleUploadedFiles", "Admins"))
                @Html.AntiForgeryToken()
                <label>Bulk cleanup (comma-separated file names):</label>
                <input type="text" name="fileNames" style="width:100%;" />
                <input type="submit" value="BULK DELETE" class="btn btn-success" />
            End Using
            <br />
            @If String.IsNullOrEmpty(CStr(ViewBag.StatusMessage)) = False Then
                @<div id="divStatusMessage" class="field-validation-error">
                    @ViewBag.StatusMessage
                </div>
            End If
        </div>
    </div>
</div>
