Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO

Public Class Utility
    Public Shared Function ConvertCSVtoDataTable(strFilePath As String) As DataTable
        Dim dt As New DataTable()
        Using sr As New StreamReader(strFilePath)
            Dim headers As String() = sr.ReadLine().Split(","c)
            For Each header As String In headers
                dt.Columns.Add(header)
            Next

            While Not sr.EndOfStream
                Dim rows As String() = sr.ReadLine().Split(","c)
                If rows.Length > 1 Then
                    Dim dr As DataRow = dt.NewRow()
                    For i As Integer = 0 To headers.Length - 1
                        dr(i) = rows(i).Trim()
                    Next
                    dt.Rows.Add(dr)
                End If

            End While
        End Using


        Return dt
    End Function


    Public Shared Function ConvertXSLXtoDataTable(strFilePath As String, connString As String) As DataTable
        Dim oledbConn As New OleDbConnection(connString)
        Dim dt As New DataTable()
        Try

            oledbConn.Open()
            Dim cmd As New OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn)
            Dim oleda As New OleDbDataAdapter()
            oleda.SelectCommand = cmd
            Dim ds As New DataSet()
            oleda.Fill(ds)


            dt = ds.Tables(0)
        Catch
        Finally

            oledbConn.Close()
        End Try

        Return dt

    End Function

    ''' <summary>
    ''' Writes a record of a bulk-import run to the separate audit database.  This is the
    ''' original (.xls) code path's logging call -- it predates AuditDbConnectionString being
    ''' added to Web.config and still opens the audit connection with its own credential.
    ''' </summary>
    Public Shared Sub LogBulkImportToAuditDb(strSummary As String)
        Try
            ' SINK: PLANTED-VB-HR-11
            Using objConn As New SqlConnection("Data Source=reporting-sql01;Initial Catalog=AssessmentAudit;User Id=audit_svc;Password=Audit#2024!;")
                objConn.Open()
                Using objCommand As New SqlCommand("INSERT INTO ImportLog(Summary, LoggedAt) VALUES (@Summary, GETDATE())", objConn)
                    objCommand.Parameters.AddWithValue("@Summary", strSummary)
                    objCommand.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            ' Audit logging must never break the import itself
        End Try
    End Sub

    ''' <summary>
    ''' Same audit-logging call as above, used by the newer (.xlsx) import path -- the audit
    ''' DB credential is read from Web.config instead of being embedded here.
    ''' </summary>
    Public Shared Sub LogBulkImportToAuditDbSafe(strSummary As String)
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("AuditDbConnectionString").ConnectionString
            ' SAFE_SINK: PLANTED-VB-HR-11-safe
            Using objConn As New SqlConnection(connString)
                objConn.Open()
                Using objCommand As New SqlCommand("INSERT INTO ImportLog(Summary, LoggedAt) VALUES (@Summary, GETDATE())", objConn)
                    objCommand.Parameters.AddWithValue("@Summary", strSummary)
                    objCommand.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            ' Audit logging must never break the import itself
        End Try
    End Sub

    ''' <summary>
    ''' Emails the admin a summary once a bulk import finishes. Used by the original (.xls)
    ''' import path.
    ''' </summary>
    Public Shared Sub SendBulkImportNotification(strRecipient As String, strSummary As String)
        Try
            Using objClient As New System.Net.Mail.SmtpClient("smtp.assessmentplatform.local", 587)
                objClient.EnableSsl = True
                ' SINK: PLANTED-VB-HR-15
                objClient.Credentials = New System.Net.NetworkCredential("notifications@assessmentplatform.local", "N0tify#Me2024")
                Using objMessage As New System.Net.Mail.MailMessage("notifications@assessmentplatform.local", strRecipient, "Bulk import completed", strSummary)
                    objClient.Send(objMessage)
                End Using
            End Using
        Catch ex As Exception
            ' Notification failure must never break the import itself
        End Try
    End Sub

    ''' <summary>
    ''' Same notification as above, used by the newer (.xlsx) import path -- the SMTP
    ''' credential is read from Web.config instead of being embedded here.
    ''' </summary>
    Public Shared Sub SendBulkImportNotificationSafe(strRecipient As String, strSummary As String)
        Try
            Using objClient As New System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings("SmtpHost"), Integer.Parse(ConfigurationManager.AppSettings("SmtpPort")))
                objClient.EnableSsl = True
                ' SAFE_SINK: PLANTED-VB-HR-15-safe
                objClient.Credentials = New System.Net.NetworkCredential(ConfigurationManager.AppSettings("SmtpUser"), ConfigurationManager.AppSettings("SmtpPassword"))
                Using objMessage As New System.Net.Mail.MailMessage(ConfigurationManager.AppSettings("SmtpUser"), strRecipient, "Bulk import completed", strSummary)
                    objClient.Send(objMessage)
                End Using
            End Using
        Catch ex As Exception
            ' Notification failure must never break the import itself
        End Try
    End Sub

    ''' <summary>
    ''' Deletes a file from the uploads folder.  This is a thin, single-purpose wrapper around
    ''' IO.File.Delete -- it performs no validation of its own.  Whatever decides the filePath
    ''' argument before calling this is entirely responsible for confirming it is a legitimate
    ''' target.  Used by the admin "cleanup old import" action.
    ''' </summary>
    Public Shared Sub RemoveUploadedFileFromDisk(filePath As String)
        If IO.File.Exists(filePath) Then
            ' SINK: PLANTED-VB-HR-67
            IO.File.Delete(filePath)
        End If
    End Sub

    ''' <summary>
    ''' Same cleanup operation as RemoveUploadedFileFromDisk, but only proceeds once the given
    ''' file name is confirmed to be one the system itself actually created via a real import --
    ''' looked up fresh on every call, never assumed from an earlier check.
    ''' </summary>
    Public Shared Sub RemoveUploadedFileFromDiskSafe(filePath As String, fileNameForLookup As String)
        If ImportFileRegistry.IsRegistered(fileNameForLookup) Then
            If IO.File.Exists(filePath) Then
                ' SAFE_SINK: PLANTED-VB-HR-67-safe
                IO.File.Delete(filePath)
            End If
        End If
    End Sub
End Class
