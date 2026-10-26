Imports System.Data.SqlClient
Imports System.IO

Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Function Index() As ActionResult
        'Response.AddHeader("Cache-Control", "no-cache, no-store, max-age=0, must-revalidate")
        'Response.AddHeader("Expires", "Fri, 01 Jan 1990 00:00:00 GMT")
        'Response.AddHeader("Pragma", "no-cache")
        Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetNoStore()
        Return View()
    End Function

    Function TakeTest() As ActionResult
        Return View()
    End Function

    Function TeacherLogin() As ActionResult
        'They will need to log in only if they have logged out 
        'If Session("LoggedInTeacherID") IsNot Nothing Then
        TempData("LoginPageFor") = "TEACHERS" 'When using RedirecttoAction, use this not Viewdata.  Then later, assign to viewdata(see Login.Index)
        'Return RedirectToAction("Index", "Login", New { FileUploadMsg = "File uploaded successfully" })
        Return RedirectToAction("Index", "Login")
        'Else
        '    'Dim objLoginData As LoginData
        '    'TODO - Get their login credentials from db first.
        '    Return RedirectToAction("Index", "Login")
        'End If
    End Function

    Function StudentLogin() As ActionResult
        'If Session("LoggedInStudentID") IsNot Nothing Then
        TempData("LoginPageFor") = "STUDENTS"
        Return RedirectToAction("Index", "Login")
        'Else
        '    Return RedirectToAction("Index", "Login")
        'End If
    End Function

    Function AdminLogin() As ActionResult
        'They will need to log in only if they have logged out 
        'If Session("LoggedInTeacherID") IsNot Nothing Then
        TempData("LoginPageFor") = "ADMINISTRATORS" 'When using RedirecttoAction, use this not Viewdata.  Then later, assign to viewdata(see Login.Index)
        'Return RedirectToAction("Index", "Login", New { FileUploadMsg = "File uploaded successfully" })
        Return RedirectToAction("Index", "Login")
        'Else
        '    'Dim objLoginData As LoginData
        '    'TO DO - Get their login credentials from db first.
        '    Return RedirectToAction("Index", "Login")
        'End If
    End Function

    Function CreateAccounts() As ActionResult
        Dim model As CreateAccountData = New CreateAccountData()
        model.LoadSchools()
        Return View(model)
        'Return View()
    End Function

    '<HttpPost()>
    'Public Function Process(ByVal objSubjects As School) As ActionResult
    '    Dim str1 As String = objSubjects.Selectedsubject
    '    Return View()
    'End Function

    <HttpPost()>
    Function DoCreateAccount(ByVal objActType As CreateAccountData) As ActionResult
        If ModelState.IsValid Then
            Dim iSchoolID As Integer = 0
            'check also the type of acount selection
            If objActType.TypeofAccount Is Nothing OrElse
                    Not (objActType.TypeofAccount.Equals("student") Or objActType.TypeofAccount.Equals("teacher")) Then
                ViewBag.StatusMessage = "Type of account MUST be selected"
            ElseIf String.IsNullOrEmpty(objActType.SelectedSchool) = False AndAlso objActType.GetSchoolID(objActType.SelectedSchool) = 0 Then
                ViewBag.StatusMessage = "Invalid School"
            Else
                Dim strFirstname As String = objActType.Firstname
                Dim strLastname As String = objActType.Lastname
                Dim strActType As String = objActType.TypeofAccount
                Dim strSelectedSchool As String = objActType.SelectedSchool
                Dim strError As String = ""
                Select Case strActType.ToLower
                    Case "student"
                        If objActType.CreateStudentAccount(strError) Then
                            ViewBag.StatusMessage = String.Format("Successfully created acount for STUDENT {0}", objActType.Firstname + " " + objActType.Lastname)
                        Else
                            ViewBag.StatusMessage = String.Format("Account creation failed for STUDENT {0}.  Reason: {1}", objActType.Firstname + " " + objActType.Lastname + vbCrLf, vbCrLf + strError)
                        End If
                    Case "teacher"
                        If objActType.CreateTeacherAccount(strError) Then
                            ViewBag.StatusMessage = String.Format("Successfully created acount for TEACHER {0}", objActType.Firstname + " " + objActType.Lastname)
                        Else
                            ViewBag.StatusMessage = String.Format("Account creation failed for TEACHER {0}.  Reason: {1}", objActType.Firstname + " " + objActType.Lastname + vbCrLf, vbCrLf + strError)
                        End If
                End Select

            End If
        End If
        Return View("CreateAccounts") 'if this is different view, than validation does not work.
    End Function

    Function BulkCreateAccounts() As ActionResult
        'Here, check if administrator logged in.  if not, get them logged in as admin and come back here.
        If Session("LoggedinAdminID") Is Nothing OrElse IsNumeric(Session("LoggedinAdminID")) = False Then
            'If not logged in as admin
            TempData("NeedsAdminAccess") = 1
            TempData("Feature") = "CREATE MULTIPLE ACCOUNTS"
            TempData("TargetAction") = "Home/BulkCreateAccounts"
            TempData("LoginPageFor") = "ADMINISTRATORS"
            Return RedirectToAction("Index", "Login")
        Else
            Return View()
        End If
    End Function

    <HttpPost()>
    Function DoBulkCreateAccounts(ByVal objBulkAcctType As BulkAccounts) As ActionResult
        If Session("LoggedinAdminID") IsNot Nothing Then
            'If ModelState.IsValid Then
            'Dim strFirstname As String = objActType.Firstname
            '    Dim strLastname As String = objActType.Lastname
            If objBulkAcctType.TypeofAccount IsNot Nothing Then
                Dim strActType As String = objBulkAcctType.TypeofAccount
                'Dim strSchool As String = objActType.School
                'If strActType.ToLower = "teacher" Then
                '    ViewBag.AccountType = 1
                'ElseIf strActType.ToLower = "student" Then
                '    ViewBag.AccountType = 2
                'Else
                '    ViewBag.AccountType = 0
                'End If
                ViewBag.AccountType = strActType 'loses on repeated page reload
                Session("AccountType") = strActType
            Else
                ViewBag.AccountType = ""
                ViewBag.StatusMessage = "An account type MUST be selected"
            End If

            'End If
            Return View("BulkCreateAccounts") 'if this is different view, than validation does not work.
        Else
            TempData("NeedsAdminAccess") = 1
            Return RedirectToAction("Index", "Login")
        End If

    End Function

#Region "Upload from Excel files"
    Public Function ImportExcel() As ActionResult
        Return View()
    End Function

    <ActionName("ImportExcel")>
    <HttpPost()>
    Public Function ImportExcel1() As ActionResult


        If Request.Files("FileUpload1").ContentLength > 0 Then
            Dim extension As String = System.IO.Path.GetExtension(Request.Files("FileUpload1").FileName).ToLower()
            Dim query As String = Nothing
            Dim connString As String = ""


            Dim strErrors As String = ""

            Dim validFileTypes As String() = {".xls", ".xlsx", ".csv"}

            Dim path1 As String = String.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files("FileUpload1").FileName)
            If Not Directory.Exists(path1) Then
                Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"))
            End If
            If validFileTypes.Contains(extension) Then
                If System.IO.File.Exists(path1) Then
                    System.IO.File.Delete(path1)
                End If
                Request.Files("FileUpload1").SaveAs(path1)
                ImportFileRegistry.Register(Request.Files("FileUpload1").FileName)
                If extension = ".csv" Then
                    Dim dt As DataTable = Utility.ConvertCSVtoDataTable(path1)
                    ViewBag.Data = dt
                    'Connection String to Excel Workbook  
                ElseIf extension.Trim() = ".xls" Then
                    connString = (Convert.ToString("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=") & path1) + ";Extended Properties=""Excel 8.0;HDR=Yes;IMEX=2"""
                    Dim dt As DataTable = Utility.ConvertXSLXtoDataTable(path1, connString)
                    Dim strFormatError As String = ""
                    If CheckInputDataDT(dt, strFormatError) Then
                        CreateAccountsFromDT(dt, strErrors)
                        If String.IsNullOrEmpty(strErrors) = True Then
                            ViewBag.StatusMessage = "All supplied accounts have been successfully created"
                        Else
                            ViewBag.StatusMessage = "The following accounts could not be created (for given reasons): <br/>" + strErrors
                        End If
                        ViewBag.Data = dt
                        Utility.LogBulkImportToAuditDb(String.Format("Bulk import of {0} accounts processed from {1}", dt.Rows.Count, path1))
                        Utility.SendBulkImportNotification("admin@assessmentplatform.local", String.Format("Bulk import of {0} accounts completed.", dt.Rows.Count))
                    Else
                        ViewBag.StatusMessage = "The following accounts could not be created (for given reasons): <br/>" + strFormatError
                    End If



                ElseIf extension.Trim() = ".xlsx" Then
                    connString = (Convert.ToString("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=") & path1) + ";Extended Properties=""Excel 12.0;HDR=Yes;IMEX=2"""
                    Dim dt As DataTable = Utility.ConvertXSLXtoDataTable(path1, connString)
                    ViewBag.Data = dt
                    Utility.LogBulkImportToAuditDbSafe(String.Format("Bulk import of {0} accounts processed from {1}", dt.Rows.Count, path1))
                    Utility.SendBulkImportNotificationSafe("admin@assessmentplatform.local", String.Format("Bulk import of {0} accounts completed.", dt.Rows.Count))

                End If
            Else

                ViewBag.[Error] = "Please Upload Files in .xls, .xlsx or .csv format"

            End If
        End If

        Return View("BulkCreateAccounts")
    End Function

    Private Function CheckInputDataDT(dt As DataTable, ByRef strformatError As String) As Boolean
        Dim bOK As Boolean = False


        Dim strIDCOLUMN As String = "", strIDStartsWith = ""
        If Session("AccountType").ToString = "student" Then
            strIDCOLUMN = "StudentID"
            strIDStartsWith = "STU"
        ElseIf Session("AccountType").ToString = "teacher" Then
            strIDCOLUMN = "TeacherID"
            strIDStartsWith = "TCH"
        Else
            strformatError = "Unknown account type"
            Return False
        End If
        '1.CHECK NUMBER OF COLS AND EACH COLUMN NAME
        If dt.Columns.Count <> 4 Then
            bOK = False
            strformatError = "In the excel file, there must be exactly 4 columns of data only."
        Else
            'Check if all required columns are present in that order
            Dim iColCtr As Integer = 0
            For Each dc As DataColumn In dt.Columns
                Select Case iColCtr
                    'Case 0
                    '    If dc.ColumnName.ToLower = strIDCOLUMN.ToLower Then
                    '        bOK = True
                    '    Else
                    '        bOK = False
                    '        strformatError = String.Format("First column in this excel sheet must be '{0}'", strIDCOLUMN)
                    '        Exit For
                    '    End If
                    Case 0
                        If dc.ColumnName.ToLower = "firstname" Then
                            bOK = True
                        Else
                            bOK = False
                            strformatError = "Second column in this excel sheet must be 'Firstname'"
                            Exit For
                        End If
                    Case 1
                        If dc.ColumnName.ToLower = "lastname" Then
                            bOK = True
                        Else
                            bOK = False
                            strformatError = "Third column in this excel sheet must be 'Lastname'"
                            Exit For
                        End If
                    'Case 2
                    '    If dc.ColumnName.ToLower = "username" Then
                    '        bOK = True
                    '    Else
                    '        bOK = False
                    '        strformatError = "Fourth column in this excel sheet must be 'Username'"
                    '        Exit For
                    '    End If
                    'Case 4
                    '    If dc.ColumnName.ToLower = "password" Then
                    '        bOK = True
                    '    Else
                    '        bOK = False
                    '        strformatError = "Fifth column in this excel sheet must be 'Password'"
                    '        Exit For
                    '    End If
                    Case 2
                        If dc.ColumnName.ToLower = "joindate" Then
                            bOK = True
                        Else
                            bOK = False
                            strformatError = "Third column in this excel sheet must be 'JoinDate'"
                            Exit For
                        End If
                    Case 3
                        If dc.ColumnName.ToLower = "school" Then
                            bOK = True
                        Else
                            bOK = False
                            strformatError = "Third column in this excel sheet must be 'School'"
                            Exit For
                        End If
                End Select
                iColCtr += 1
            Next
        End If
        'All column names are OK.  
        If bOK Then
            '2.CHECK THE ID COLUMN and IT SHOULD ONLY BE OF LENGTH 9 CHARACTERS AND MUST BEGIN WITH 'STU' OR 'TCH' DEP ON ACCT TYPE 
            'ALSO CHECK IF THIS ID IS ALREADY PRESENT IN TABLE.

            ''iColCtr = 0
            ''Dim strNextID As String = GetNextAvailableID()
            'Dim strPassword As String = ""
            'If Session("AccountType").ToString = "student" Then
            '    strPassword = "student"
            'Else
            '    strPassword = "teacher"
            'End If
            Dim iRowctr = 0
            For Each dr As DataRow In dt.Rows
                'If IDPresentInSystem(dr(strIDCOLUMN)) Then
                '    'strformatError = String.Format("A record with the ID given in row #{0} in excel sheet already exists in system.  Please change the ID.", iRowctr + 1)
                '    strformatError = String.Format("The ID data for {0} already exists in the system<br/>", dr("Firstname").ToString + " " + dr("Lastname").ToString())
                '    bOK = False
                '    Exit For
                'ElseIf
                'End If
                If GetSchoolID(dr("School").ToString) > 0 Then
                    bOK = True
                Else
                    bOK = False
                    strformatError = String.Format("The School given for {0} is not valid<br/>", dr("Firstname").ToString + " " + dr("Lastname").ToString())
                End If
            Next
        End If
        '  If iRowctr > 0 Then
        'If dr(strIDCOLUMN).ToString.Length <> 9 Then
        'If Session("AccountType").ToString = "student" Then
        '            strformatError = String.Format("The '{0}' column data for {1} MUST BE exactly 9 alphanumeric characters of the format : STUXXXXXX. <br/>", strIDCOLUMN, dr("Firstname").ToString + " " + dr("Lastname").ToString())
        '            'strPassword = "student"
        '        ElseIf Session("AccountType").ToString = "teacher" Then
        '            strformatError = String.Format("The '{0}' column data for {1} MUST BE exactly 9 alphanumeric characters of the format : TCHXXXXXX. <br/>", strIDCOLUMN, dr("Firstname").ToString + " " + dr("Lastname").ToString())
        '            'strPassword = "teacher"
        '        End If

        ' bOK = False
        'ElseIf dr(strIDCOLUMN).ToString.StartsWith(strIDStartsWith) = False Then
        '    strformatError = String.Format("The '{0}' column data for {1} MUST BE exactly 9 alphanumeric characters of the format : STUXXXXXX. <br/>", strIDCOLUMN, dr("Firstname").ToString + " " + dr("Lastname").ToString())
        '    strformatError = String.Format("The ID in the row # {0} in excel sheet NUST start with '{1}'", iRowctr + 1, strIDStartsWith)
        '    bOK = False
        '    Exit For
        ''TO DO - Think og automatically doing it rather than asking user for it
        'ElseIf session("AccountType").ToString = "teacher" Then
        '        Dim bCorrectUsername As Boolean = dr("Username").ToString.ToLower.StartsWith("teacher") AndAlso
        '        IsNumeric(dr("Username").ToString.ToLower.Replace("teacher", "")) = True
        '        If Not bCorrectUsername Then
        '            'strformatError = String.Format("The username for a teacher in row #{0} MUST be like 'Teacher1', 'Teacher2', etc", iRowctr + 1)
        '            strformatError = String.Format("The ID data for {0} already MUST look like Teacher1', 'Teacher2', etc.<br/>", dr("Firstname").ToString + " " + dr("Lastname").ToString())
        '            bOK = False
        '            Exit For
        '        Else
        '            bOK = True
        '        End If
        '        'ElseIf session("AccountType").ToString = "student" AndAlso
        '        '        Followes5by2Rule(dr("Username"), dr("Firstname"), dr("Lastname")) = False Then
        '        '    strformatError = String.Format("The username for a student in row #{0} MUST consists of first five letters of first name followed by 2 letters of lastname (called 5 x 2 rule)", iRowctr + 1)
        '        '    bOK = False
        '        '    Exit For
        '    ElseIf UsernameExists(dr("Username")) Then
        '        'strformatError = String.Format("A record with the username given in row #{0} in excel sheet already exists in system.  Please pick another user name", iRowctr + 1)
        '        strformatError = String.Format("The Username data for {0} already MUST look like Teacher1', 'Teacher2', etc.<br/>", dr("Firstname").ToString + " " + dr("Lastname").ToString())
        '        bOK = False
        '        Exit For
        '    ElseIf dr("Password").ToString.Trim <> strPassword Then
        '        'strformatError = String.Format("The password given for record in row #{0} in excel sheet MUST BE '{1}'", iRowctr + 1, strPassword)
        '        strformatError = String.Format("The Password data for {0} already MUST only be {1} <br/>", dr("Firstname").ToString + " " + dr("Lastname").ToString(), strPassword)
        '        bOK = False
        '        Exit For
        '    Else
        '            bOK = True 'All checks have passed 
        '        End If
        '    ' End If
        Return bOK
    End Function

    Private Sub CreateAccountsFromDT(dt As DataTable, ByRef strErrors As String)
        Dim strError As String = ""
        Dim objAccountsData As CreateAccountData
        '@column.ColumnName.ToUpper()
        For Each Row As DataRow In dt.Rows
            objAccountsData = New CreateAccountData
            objAccountsData.Firstname = Row("Firstname")
            objAccountsData.Lastname = Row("Lastname")
            objAccountsData.SelectedSchool = Row("School")
            ' objAccountsData.Username = dt("Username").ToString
            If Session("AccountType") = "teacher" Then
                If objAccountsData.CreateTeacherAccount(strError) Then
                Else
                    strErrors += String.Format("Account for : {0}, Reason: {1}", objAccountsData.Firstname + " " + objAccountsData.Lastname, strError) + "<br/>"
                End If
            ElseIf Session("AccountType") = "student" Then
                If objAccountsData.CreateStudentAccount(strError) Then
                Else
                    strErrors += strError
                End If
            End If
            '@Row(column).ToString()
        Next
    End Sub
#End Region

    <HttpPost()>
    Public Function Process(ByVal objSubjects As CreateAccountData) As ActionResult
        Dim str1 As String = objSubjects.SelectedSchool
        Return View("CreateAccounts")
    End Function
End Class
