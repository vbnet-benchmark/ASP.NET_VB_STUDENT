@Code
    ViewData("Title") = "Not Found"
End Code

<div style="background-color: #bbd775; color: #000; height: 10px;margin-top:100px;">
</div>
<div style="background-color: #bbd775; color: #000; height: 170px;">
    <div style="padding:20px;">
        <h3>
            Application Error:
        </h3>
        <h4>
            Sorry, The Page, You are Looking for is not found.
        </h4>
        <div style="margin-top:50px;">
            <input type="button" value="BACK TO MAIN" class="btn btn-success" onclick="location.href='@Url.Action("Index", "Home")'" />
        </div>
        <br />
        <br />
    </div>
</div>
<div style="background-color: #bbd775; color: #000; height: 20px;">
</div>  