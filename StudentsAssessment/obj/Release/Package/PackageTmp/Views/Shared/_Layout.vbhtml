<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>@ViewBag.Title - My ASP.NET Application</title>
    @Styles.Render("~/Content/css")
    @Scripts.Render("~/bundles/modernizr")
    @Scripts.Render("~/bundles/jqueryui")
    @Styles.Render("~/Content/cssjqryUi")
    <style type="text/css">
        /*div scrollbar*/
        div .scrollable{
        overflow-y:auto; /*Auto makes it appear only when overflowing of text occurs in the area*/
         overflow-x:hidden; /*Auto makes it appear only when overflowing of text occurs in the area*/
        /*height:220px;*/
        }
        /*For validation messages*/
        .field-validation-error{ 
            color: maroon; 
            font-weight: bold; 
        }

       header{
            /*Makes it transparent*/
    opacity: 0.5;
    filter: alpha(opacity=50); /* For IE8 and earlier */
       }
        footer{
    /*Non-sticky*/
    /*background-color:#bbd775;
    height:3em;
    text-align:center;
    font-weight:bold;
    font-style:italic;
    margin-top:90%;
    */

    /*Sticky*/
    position:fixed;
   left:0px;
   bottom:0px;
   height:60px;
   width:100%;
  background:#bbd775;
   text-align:center;
    font-weight:bold;
    font-style:italic;
     /*Makes it transparent*/
    opacity: 0.5;
    filter: alpha(opacity=50); /* For IE8 and earlier */
}       

        /*Remove spinners from input boxes*/
        input[type=number]::-webkit-inner-spin-button, 
input[type=number]::-webkit-outer-spin-button { 
  -webkit-appearance: none; 
  margin: 0; 
}

      h2{
          text-align:center;
          color:#fff;
      }

      h1{
          color:#fff;
      }
    </style>
    @*Responsible for setting focus on a textbox*@
    <script src="@Url.Content("~/Scripts/jquery-1.10.2.min.js")"></script>
    <script>
        jQuery(document).ready(function () {
            $(function () {
                $('.focus :input').focus();
            })
        });
    </script>

    <link href="~/Content/Superfish/css/superfish.css" rel="stylesheet" media="screen" />
    <link href="~/Content/Superfish/css/superfish-vertical.css" rel="stylesheet" media="screen" />
    <script src="~/Content/Superfish/js/jquery.js"></script>
    <script src="~/Content/Superfish/js/superfish.js"></script>
    <script src="~/Content/Superfish/js/hoverIntent.js"></script>
    <script>

        jQuery(document).ready(function () {
            jQuery('ul.sf-menu').superfish({
            });
        });


        //fOR SUPERFISH MENU


    </script>
</head>
<body style="background-color:#c6d59f;">
    <div class="navbar navbar-inverse navbar-fixed-top" style="background-color:#bbd775;border-bottom:none;">
            <div class="container">
                <div class="navbar-header">
                    <header>
                        <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                            <span class="icon-bar"></span>
                            <span class="icon-bar"></span>
                            <span class="icon-bar"></span>
                        </button>
                        <table style="margin-left:-20em;" border="0" cellpadding="5" cellspacing="5" width="1800">
                            <colgroup>
                                <col width="200" />
                                <col />
                                <col />
                                <col width="1600" />
                            </colgroup>
                            <tr>
                                <td>
                                    <img src="~/Content/images/0-logo.png" width="400" height="100" />
                                </td>
                                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                <td valign="top">
                                    @* @Html.ActionLink("STUDENT ASSESSMENT PLATFORM", "Index", "Home", New With {.area = ""}, New With {.class = "navbar-brand"})*@
                                    <a style="text-decoration:none;" href="@Url.Action("Index", "Home")"><h1>STUDENT ASSESSMENT PLATFORM</h1></a>
                                </td>
                            </tr>
                        </table>
                    </header>
                   
                </div>
           
            </div>
        </div>
    <div class="container body-content" style="margin-top:75px;">
        @RenderBody()
        <footer>
            <p>&copy; @DateTime.Now.Year - Students Assessment Platform<br/>
            All rights  reserved.  Application suitable for use in the field of education only<br/>
            Web application version 2</p>
        </footer>
    </div>

    @Scripts.Render("~/bundles/jquery")
    @Scripts.Render("~/bundles/bootstrap")
    @RenderSection("scripts", required:=False)
</body>
</html>
