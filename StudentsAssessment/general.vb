Imports System.Data.SqlClient

Module general

    'Public Function GetConnectString() As String
    '    ' Find the database Name
    '    'It SHOULD BE the same way you specify when logging to SQL Server
    '    'using the Management studio.
    '    'ADDITIONALLY, must make sure that the below server is started by looking
    '    'at the "Services" control panel applet
    '    'Dim strSvr As String = ".\SQLSERVER08_2016" ' LAPTOP
    '    Dim strSvr As String = "(local)" ' DESKTOP
    '    Dim strSvrTimeOut As String = "30"
    '    Dim strCat As String = "AssessmentDB"
    '    Dim strUser As String = "sathi"
    '    Dim strPass As String = "sairam"
    '    ' strSvr = "(local)"
    '    'strPass = "sairam"
    '    '   strUser = "test"
    '    Dim strConn As String = "Data Source=" & strSvr & ";initial catalog=" + strCat + ";"
    '    'strConn += "Integrated Security=SSPI;"
    '    strConn += "uid=" & strUser & ";pwd=" & strPass
    '    Return strConn
    'End Function

    ''' <summary>
    ''' This returns the connection string it reads from web.config file
    ''' </summary>
    ''' <returns></returns>
    Public Function GetConnectStringFromWebConfig() As String
        Dim strConn As String = ""
        strConn = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
        Return strConn
    End Function

    Public Function IsDateOK(strstartDate As String) As Boolean
        Dim bOK As Boolean = False
        Const REGULAR_EXP = "^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"

        If Not Regex.IsMatch(strstartDate, REGULAR_EXP) Then
            bOK = False
        Else
            bOK = True
        End If
        Return bOK
    End Function

#Region "Classes"
    Public Function GetClassID(ByVal strClassname As String) As Integer
        Dim iClassID As Integer = 0
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            ' Read the connection string from the web.config file
            Using objConn As New SqlConnection(connString)
                Dim strSQL = <![CDATA[
                                SELECT ClassID, Classname FROM Classes WHERE Classname=@Classname
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@Classname", strClassname)
                    End With
                    Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                        If objReader.Read() Then
                            iClassID = Integer.Parse(objReader("ClassID"))
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            iClassID = 0
        End Try
        Return iClassID
    End Function

    Public Function GetClassName(ByVal iClassID As Integer) As String
        Dim strClassname As String = ""
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            ' Read the connection string from the web.config file
            Using objConn As New SqlConnection(connString)
                Dim strSQL = <![CDATA[
                                SELECT Classname FROM Classes WHERE ClassID=@ClassID
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@ClassID", iClassID)
                    End With
                    Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                        If objReader.Read() Then
                            strClassname = objReader("Classname").ToString
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            strClassname = ""
        End Try
        Return strClassname
    End Function


#End Region

#Region "School"
    Public Function GetSchoolID(strSchoolname As String) As Integer
        Dim iSchoolID As Integer = 0
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            Using objConn As New SqlConnection(connString)
                Dim strSQL As String = ""
                strSQL = <![CDATA[
                                SELECT SchoolID FROM Schools WHERE Schoolname=@Schoolname
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@Schoolname", strSchoolname)
                    End With
                    iSchoolID = objCommand1.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            iSchoolID = 0
        End Try
        Return iSchoolID
    End Function

    ''' <summary>
    ''' Count of archived (historical) classes, pulled from the separate reporting warehouse
    ''' rather than the main AssessmentDB.  Shown on the admin "view classes" screen.
    ''' </summary>
    Public Function GetArchivedClassCount() As Integer
        Dim iCount As Integer = 0
        Const REPORTING_DB_PWD As String = "R3p0rt#Pass99"
        Try
            Dim connString As String = "Data Source=reporting-sql01;Initial Catalog=AssessmentReporting;User Id=reporting_ro;Password=" & REPORTING_DB_PWD & ";"
            ' SINK: PLANTED-VB-HR-12
            Using objConn As New SqlConnection(connString)
                objConn.Open()
                Dim strSQL As String = "SELECT COUNT(*) FROM Classes WHERE Archived=1"
                Using objCommand As New SqlCommand(strSQL, objConn)
                    iCount = Convert.ToInt32(objCommand.ExecuteScalar())
                End Using
            End Using
        Catch ex As Exception
            iCount = 0
        End Try
        Return iCount
    End Function

    ''' <summary>
    ''' Same archived-record count as above but for subjects, shown on the admin "view
    ''' subjects" screen.  Added after GetArchivedClassCount, this one reads the reporting
    ''' DB's credential from Web.config instead of a literal.
    ''' </summary>
    Public Function GetArchivedSubjectCountSafe() As Integer
        Dim iCount As Integer = 0
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("ReportingConnectionString").ConnectionString
            ' SAFE_SINK: PLANTED-VB-HR-12-safe
            Using objConn As New SqlConnection(connString)
                objConn.Open()
                Dim strSQL As String = "SELECT COUNT(*) FROM Subjects WHERE Archived=1"
                Using objCommand As New SqlCommand(strSQL, objConn)
                    iCount = Convert.ToInt32(objCommand.ExecuteScalar())
                End Using
            End Using
        Catch ex As Exception
            iCount = 0
        End Try
        Return iCount
    End Function

    Public Function GetSchoolName(ByVal iSchoolID As Integer) As String
        Dim Schoolname As String = ""
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            ' Read the connection string from the web.config file
            Using objConn As New SqlConnection(connString)
                Dim strSQL = <![CDATA[
                                SELECT Schoolname FROM Schools WHERE SchoolID=@SchoolID
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@SchoolID", iSchoolID)
                    End With
                    Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                        If objReader.Read() Then
                            Schoolname = objReader("Schoolname").ToString
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Schoolname = ""
        End Try
        Return Schoolname
    End Function
#End Region

    'Private Function GetNextAvailableID() As String
    '    Dim strID As String = ""
    '    Try
    '        Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
    '        Using objConn As New SqlConnection(connString)
    '            Dim strSQL As String = ""
    '            If Session("AccountType").ToString = "student" Then
    '                strSQL = <![CDATA[
    '                            SELECT TOP 1 StudentID AS ID FROM Students ORDER BY StudentID DESC
    '                ]]>.Value()
    '            ElseIf Session("AccountType").ToString = "teacher" Then
    '                strSQL = <![CDATA[
    '                            SELECT TOP 1 TeacherID AS ID FROM Teachers ORDER BY TeacherID DESC
    '                ]]>.Value()
    '            Else
    '                strID = "INVALID ID"
    '                strSQL = ""
    '            End If
    '            If String.IsNullOrEmpty(strSQL) = False Then
    '                Using objCommand1 As New SqlCommand
    '                    With objCommand1
    '                        .Connection = objConn
    '                        .Connection.Open()
    '                        .CommandText = strSQL
    '                    End With
    '                    Using objReader As SqlDataReader = objCommand1.ExecuteReader()
    '                        If objReader.Read Then
    '                            strID = (Integer.Parse(objReader("ID")) + 1).ToString
    '                            If Session("AccountType").ToString = "student" Then
    '                                strID = "STU" + Right(1000000 + strID, 6)
    '                            ElseIf Session("AccountType").ToString = "teacher" Then
    '                                strID = "TCH" + Right(1000000 + strID, 6)
    '                            Else
    '                                strID = "INVALID ID"
    '                            End If
    '                        Else
    '                            strID = "INVALID ID"
    '                        End If
    '                    End Using
    '                End Using
    '            End If

    '        End Using
    '    Catch ex As Exception
    '        strID = "INVALID ID"
    '    End Try
    '    Return strID
    'End Function
    '''' <summary>
    '''' Returns a list of subjects for this teacher with comma seperating them
    '''' </summary>
    '''' <param name="iTeacherID"></param>
    '''' <returns></returns>
    'Public Function GetTeacherSubjects(ByVal iTeacherID As Integer) As String
    '    Dim strSubjectsBuilder As New StringBuilder
    '    Dim strConn As String = GetConnectStringFromWebConfig()
    '    Dim objConn As New SqlConnection(strConn)
    '    Dim bValid As Boolean = False
    '    Try
    '        objConn.Open()
    '        'Dim strSQL As String = "SELECT StaffID, Username, Password FROM StaffMembers WHERE Username='" + txtUsername.Text + "' AND Password='" + txtPassword.Text + "'"
    '        Dim strSQL As String = "SELECT Subjectname FROM TeachersSubjects
    '            INNER JOIN Subjects on TeachersSubjects.SubjectID=Subjects.SubjectID 
    '            WHERE TeachersSubjects.TeacherID=@TeacherID"
    '        Dim objCommand As New SqlCommand(strSQL, objConn)
    '        objCommand.Parameters.AddWithValue("@TeacherID", iTeacherID)
    '        Dim objReader As SqlDataReader = objCommand.ExecuteReader
    '        While objReader.Read()
    '            strSubjectsBuilder.Append(objReader("Subjectname") + ",")
    '        End While
    '        objReader.Close()
    '        objReader = Nothing
    '        objCommand.Dispose()
    '        objCommand = Nothing
    '        objConn.Dispose()
    '        objConn.Close()
    '        objConn = Nothing
    '        SqlConnection.ClearAllPools()
    '    Catch ex As Exception
    '        strSubjectsBuilder.Append("Problem Retrieving Subjects")
    '    End Try
    '    Return strSubjectsBuilder.ToString()
    'End Function

#Region "Teachers"
    Public Function TeacherteachesClass(ByVal iTeacherID As Integer, iClassID As Integer) As Boolean

        Dim bTeaches As Boolean = False
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            ' Read the connection string from the web.config file
            Using objConn As New SqlConnection(connString)
                Dim strSQL = <![CDATA[
                                SELECT EntryId  FROM TeachersClasses WHERE TeacherID=@TeacherId AND ClassID=@ClassID
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@ClassID", iClassID)
                        .Parameters.AddWithValue("@TeacherID", iTeacherID)
                    End With
                    If objCommand1.ExecuteScalar() > 0 Then
                        bTeaches = True
                    Else
                        bTeaches = False
                    End If
                End Using
            End Using
        Catch ex As Exception
            bTeaches = False
        End Try

        Return bTeaches
    End Function


    ''' <summary>
    ''' Given the teacher ID, this function returns the teacher's name.  This is the
    ''' combination of teacher's first and last names
    ''' </summary>
    ''' <param name="iTeacherID"></param>
    ''' <returns></returns>
    Public Function GetTeacherName(ByVal iTeacherID As Integer) As String
        Dim strFullname As String = ""
        Dim strConn As String = GetConnectStringFromWebConfig()
        Dim objConn As New SqlConnection(strConn)
        Dim bValid As Boolean = False
        Try
            objConn.Open()
            Dim strSQL As String = "SELECT Firstname, Lastname FROM Teachers
                WHERE TeacherID=@TeacherID"
            Dim objCommand As New SqlCommand(strSQL, objConn)
            objCommand.Parameters.AddWithValue("@TeacherID", iTeacherID)
            Dim objReader As SqlDataReader = objCommand.ExecuteReader
            If objReader.Read() Then
                strFullname = objReader("Firstname").ToString.ToUpper + " " + objReader("Lastname").ToString.ToUpper
            End If
            objReader.Close()
            objReader = Nothing
            objCommand.Dispose()
            objCommand = Nothing
            objConn.Dispose()
            objConn.Close()
            objConn = Nothing
            SqlConnection.ClearAllPools()
        Catch ex As Exception
            strFullname = "Problem area - General.GetTeacherID()"
        End Try
        Return strFullname
    End Function
#End Region

#Region "Students"
    Public Function GetStudentName(ByVal iStudentID As Integer) As String
        Dim strFullname As String = ""
        Dim strConn As String = GetConnectStringFromWebConfig()
        Dim objConn As New SqlConnection(strConn)
        Dim bValid As Boolean = False
        Try
            objConn.Open()
            Dim strSQL As String = "SELECT Firstname, Lastname FROM Students
                WHERE StudentID=@StudentID"
            Dim objCommand As New SqlCommand(strSQL, objConn)
            objCommand.Parameters.AddWithValue("@StudentID", iStudentID)
            Dim objReader As SqlDataReader = objCommand.ExecuteReader
            If objReader.Read() Then
                strFullname = objReader("Firstname").ToString.ToUpper + " " + objReader("Lastname").ToString.ToUpper
            End If
            objReader.Close()
            objReader = Nothing
            objCommand.Dispose()
            objCommand = Nothing
            objConn.Dispose()
            objConn.Close()
            objConn = Nothing
            SqlConnection.ClearAllPools()
        Catch ex As Exception
            strFullname = "Problem area - General.GetStudentname()"
        End Try
        Return strFullname
    End Function
#End Region

#Region "Subjects"
    Public Function SubjectAlreadyConnectedtoClass(ByVal iClassID As Integer, iSubjectID As Integer) As Boolean

        Dim bAlreadyConnected As Boolean = False
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            ' Read the connection string from the web.config file
            Using objConn As New SqlConnection(connString)
                Dim strSQL = <![CDATA[
                                SELECT SubjectID  FROM Subjects WHERE ClassID=@ClassID AND SubjectID=@SubjectID
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@ClassID", iClassID)
                        .Parameters.AddWithValue("@SubjectID", iSubjectID)
                    End With
                    Using objReader As SqlDataReader = objCommand1.ExecuteReader
                        If objReader.Read Then
                            bAlreadyConnected = True
                        Else
                            bAlreadyConnected = False
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            bAlreadyConnected = False
        End Try

        Return bAlreadyConnected
    End Function

    Public Function GetSubjectID(ByVal strSubjectname As String) As Integer

        Dim iSubjectId As Integer = 0
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            ' Read the connection string from the web.config file
            Using objConn As New SqlConnection(connString)
                Dim strSQL = <![CDATA[
                                SELECT SubjectID  FROM Subjects WHERE Subjectname=@Subjectname
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@Subjectname", strSubjectname)
                    End With
                    Using objReader As SqlDataReader = objCommand1.ExecuteReader
                        If objReader.Read Then
                            iSubjectId = objReader("SubjectID").ToString
                        Else
                            iSubjectId = 0
                        End If
                    End Using

                End Using
            End Using
        Catch ex As Exception
            iSubjectId = 0
        End Try
        Return iSubjectId
    End Function

    'Public Function GetSubjectIDs(ByVal strSubjectname As String) As Integer(,)
    '    Dim iSubjectConnectedClasses As Integer(,)
    '    Try
    '        Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
    '        ' Read the connection string from the web.config file
    '        Using objConn As New SqlConnection(connString)
    '            Dim strSQL = <![CDATA[
    '                            SELECT SubjectID  FROM Subjects WHERE Subjectname=@Subjectname
    '                ]]>.Value()
    '            Using objCommand1 As New SqlCommand
    '                With objCommand1
    '                    .Connection = objConn
    '                    .Connection.Open()
    '                    .CommandText = strSQL
    '                    .Parameters.AddWithValue("@Subjectname", strSubjectname)
    '                End With
    '                Using objReader As SqlDataReader = objCommand1.ExecuteReader
    '                    If objReader.Read Then
    '                        iSubjectId = objReader("SubjectID").ToString
    '                    Else
    '                        iSubjectId = 0
    '                    End If
    '                End Using

    '            End Using
    '        End Using
    '    Catch ex As Exception
    '        iSubjectId = 0
    '    End Try
    '    Return iSubjectId
    'End Function

    Public Function GetSubjectName(ByVal iSubjectID As Integer) As String

        Dim strSubjectname As String = ""
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            ' Read the connection string from the web.config file
            Using objConn As New SqlConnection(connString)
                Dim strSQL = <![CDATA[
                                SELECT Subjectname  FROM Subjects WHERE SubjectID=@SubjectID
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@SubjectID", iSubjectID)
                    End With
                    Using objReader As SqlDataReader = objCommand1.ExecuteReader
                        If objReader.Read Then
                            strSubjectname = objReader("Subjectname").ToString
                        Else
                            strSubjectname = ""
                        End If
                    End Using

                End Using
            End Using
        Catch ex As Exception
            strSubjectname = ""
        End Try

        Return strSubjectname
    End Function
#End Region

    '''' <summary>
    '''' Returns a list of classes for this teacher with comma seperating them
    '''' </summary>
    '''' <param name="iTeacherID"></param>
    '''' <returns></returns>
    'Public Function GetTeacherClasses(ByVal iTeacherID As Integer) As String
    '    Dim strClassesBuilder As New StringBuilder
    '    Dim strConn As String = GetConnectStringFromWebConfig()
    '    Dim objConn As New SqlConnection(strConn)
    '    Dim bValid As Boolean = False
    '    Try
    '        objConn.Open()
    '        'Dim strSQL As String = "SELECT StaffID, Username, Password FROM StaffMembers WHERE Username='" + txtUsername.Text + "' AND Password='" + txtPassword.Text + "'"
    '        Dim strSQL As String = "SELECT Classname FROM TeachersClasses
    '            INNER JOIN Classes on TeachersClasses.ClassID=Classes.ClassID 
    '            WHERE TeachersClasses.TeacherID=@TeacherID"
    '        Dim objCommand As New SqlCommand(strSQL, objConn)
    '        objCommand.Parameters.AddWithValue("@TeacherID", iTeacherID)
    '        Dim objReader As SqlDataReader = objCommand.ExecuteReader
    '        While objReader.Read()
    '            strClassesBuilder.Append(objReader("Classname") + ",")
    '        End While
    '        objReader.Close()
    '        objReader = Nothing
    '        objCommand.Dispose()
    '        objCommand = Nothing
    '        objConn.Dispose()
    '        objConn.Close()
    '        objConn = Nothing
    '        SqlConnection.ClearAllPools()
    '    Catch ex As Exception
    '        strClassesBuilder.Append("Problem Retrieving Class")
    '    End Try
    '    Return strClassesBuilder.ToString()
    'End Function
End Module
