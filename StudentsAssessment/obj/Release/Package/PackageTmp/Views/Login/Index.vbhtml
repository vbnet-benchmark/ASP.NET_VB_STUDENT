@Code
    ViewData("Title") = "Index"
End Code

@ModelType StudentsAssessment.LoginData

<h3 style="text-align:center;">@ViewBag.LoginPageHeader</h3>
<div class="row" style="text-align:center;">
    <h4 style="font-weight:bold;">Please type your username and password below</h4>
    <div class="col-sm-4"></div>
    <div class="col-sm-4">
        @Using (Html.BeginForm(FormMethod.Post))
            @Html.AntiForgeryToken()
            @<table id = "tblLogin" cellpadding="3" celspacing="3">
                    <tr>
                        <td>@Html.LabelFor(Function(model) model.Username, htmlAttributes:=New With {.class = "control-label col-md-2"})</td>
                        <td class="focus">@Html.TextBoxFor(Function(model) model.Username, New With {.autofocus = "autofocus"})</td>
                    </tr>
                    <tr>
                        <td colspan="2">@Html.ValidationMessageFor(Function(model) model.Username)</td>
                    </tr>
                    <tr>
                        <td>@Html.LabelFor(Function(model) model.Password)</td>
                        <td>@Html.PasswordFor(Function(model) model.Password)</td>
                    </tr>
                    <tr>
                        <td colspan="2">@Html.ValidationMessageFor(Function(model) model.Password)</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <input type="submit" value="LOGIN" class="btn btn-success" />
                            <input type="button" value="BACK TO MAIN" class="btn btn-success" onclick="location.href='@Url.Action("Index", "Home")'" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            @*@Html.Label(@ViewBag.StatusMessage, htmlAttributes:=New With {.class = "control-label col-md-2"})*@
                            @If String.IsNullOrEmpty(ViewBag.StatusMessage) = False Then
                                @<div class="field-validation-error">
                                    @ViewBag.StatusMessage
                                </div>
                            Else
                                @<div class="field-validation-error">
                                    @Html.Raw(ViewBag.StatusMessage)
                                </div>
                            End If
</td>
                    </tr>
                </table>
        End Using
    </div>
    <div class="col-sm-4"></div>
</div>

