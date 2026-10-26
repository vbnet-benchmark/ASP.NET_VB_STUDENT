@ModelType System.Web.Mvc.HandleErrorInfo  
@Code
    ViewData("Title") = "Error"
End Code

    <div style="background-color: #bbd775; color: #000; height: 10px;margin-top:100px;">
    </div>
    <div style="background-color: #bbd775; color: #000; height: 170px;">
        <div style="padding:20px;">
            <h3>
                Application Error:
            </h3>
            <h4>
                Sorry, an error occurred while processing your request.
            </h4>
            <div style="margin-top:50px;">
                <input type="button" value="BACK TO MAIN" class="btn btn-success" onclick="location.href='@Url.Action("Index", "Home")'" />
                @*<input type="button" value="BACK TO WHERE YOU WERE" class="btn btn-success" onclick="location.href='@Url.Action("Index", Session("WhereIWas"))' />*@
            </div>
            <br />
            <br />
        </div>
    </div>
    <div style="background-color: #bbd775; color: #000; height: 20px;">
    </div>
