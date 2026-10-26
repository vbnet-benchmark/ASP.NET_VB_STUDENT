<!doctype html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>jQuery UI Datepicker - Default functionality</title>
    @*<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.3/jquery.min.js"></script>
    <script src="http://code.jquery.com/ui/1.11.1/jquery-ui.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.11.4/jquery-ui.css" rel="stylesheet">*@
    <script src="~/Scripts/jquery-1.12.4.min.js"></script>
    <script src="~/Scripts/jquery-ui-1.12.1.min.js"></script>
    <link href="~/Content/jquery-ui.min.css" rel="stylesheet" />

    <script type="text/javascript">
  $(function() {
    $("#datepicker1").datepicker();
  });
    </script>
</head>
<body>
    <p>Date: <input name="datepicker1" type="text" id="datepicker1" class="form-control" /> </p>
</body>
</html>
