@Imports System.Data

@Code
    ViewData("Title") = "ImportExcel"
End Code

<h2>ImportExcel</h2>


    <!--[if !IE]><!-->
<style type = "text/css" >
        /* Generic Styling, for Desktops/Laptops */
        Table {
            width:      100%;
            border-collapse: collapse;
        }
        /* Zebra striping */
        tr:nth-of-type(odd) {
            background: #eee;
        }

        th {
            background:      #333;
            color: white;
            font-weight: bold;
        }

        td, th {
            padding: 6px;
            border: 1px solid #ccc;
            Text-align: Left();
        }
        /*
    Max width before this PARTICULAR table gets nasty
    This Query will take effect for any screen smaller than 760px
    And also iPads specifically.
    */
        @@media only screen And (max-width: 760px), (min-device-width: 768px) And (max-device-width: 1024px) {

            /* Force table to Not be Like tables anymore */
            Table, thead, tbody, th, td, tr {
                display: block;
            }

                /* Hide table headers (but Not display none;, For accessibility) */
                thead tr {
                    position: absolute;
                    top: -9999px;
                    left: -9999px;
                }

            tr {
                border:      1px solid #ccc;
            }

            td {
                /* Behave Like a "row" */
                border: none;
                border-bottom:      1px solid #eee;
                position: relative;
                padding-Left():      50%;
            }

                td: before {
                     /* Now Like a table header */
                    position: absolute;
                    /* Top/left values mimic padding */
                    top: 6px;
                    left: 6px;
                    width: 45%;
                    padding-Right():      10px;
                    white-Space(): nowrap;
                }

                /*
     Label the data
     */
                td: before {
                     Content: attr(Data - title);
                }
        }
</style>

<!--<![endif]-->
@Using (Html.BeginForm("ImportExcel", "Home", FormMethod.Post, New With {.enctype = "multipart/form-data"}))
    @Html.AntiForgeryToken()
     @<table>
        <tr><td>Excel file</td><td><input type="file" id="FileUpload1" name="FileUpload1" /></td></tr>
        <tr><td></td><td><input type= "submit" id="Submit" name="Submit" value="Submit" /></td></tr>
    </table>

    @<div>
        <table id="">
            @If ViewBag.Data IsNot Nothing Then
                Dim dtDataTable As DataTable = CType(ViewBag.Data, DataTable)
                @<tr>
                    @For Each column As DataColumn In dtDataTable.Columns
                       @<td>@column.ColumnName.ToUpper()</td>
                    Next
                </tr>

                If dtDataTable.Rows.Count > 0 Then
                    @For Each Row As DataRow In dtDataTable.Rows
                        @<tr>
                            @For Each column As DataColumn In dtDataTable.Columns
                                @<td data-title='@column.ColumnName'>
                                    @Row(column).ToString()
                                </td>
                            Next
                        </tr>
                    Next
                Else
                    Dim iCount As Integer = dtDataTable.Columns.Count
                    @<tr>
                     
                        <td colspan='@iCount' style="color:red;">No Data Found.</td>
                    </tr>
                End If
            Else
                If ViewBag.Error IsNot Nothing Then
                    @<tr>
                        <td style="color:red;">
                            @IIf(ViewBag.Error IsNot Nothing, ViewBag.Error.ToString(), "")
                        </td>
                    </tr>
                End If
            End If
    </table>
</div>
End Using

