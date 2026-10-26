Imports System.ComponentModel.DataAnnotations

Public Class BulkAccounts
    '<Required(ErrorMessage:="Account type is required")>
    <Display(Name:="ACCOUNT TYPE")>
    Public Property TypeofAccount As String
    Public Property strFilePath As String
End Class
