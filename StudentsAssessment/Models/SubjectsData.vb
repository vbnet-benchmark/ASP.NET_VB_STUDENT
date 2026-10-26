Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Data.SqlClient


Public Class SubjectsData
    Public Sub New()
        SubjectsList = New List(Of SelectListItem)
        TaughtClassesList = New List(Of SelectListItem)
        ClassesTobeTaughtInList = New List(Of SelectListItem)

    End Sub

    <DisplayName("SELECT A CLASS:")>
    <Required(ErrorMessage:="A class must be selected")>
    Public Property TaughtClassesList As List(Of SelectListItem)

    Public Property SelectedClass As String 'This MUST BE a property (not just a string) for a value to come out

    <DisplayName("SELECT A SUBJECT TO ADD:")>
    <Required(ErrorMessage:="A subject must be selected")>
    Public Property SubjectsList As List(Of SelectListItem)

    Public Property SelectedSubject As String 'This MUST BE a property (not just a string) for a value to come out

    Public Property ClassesTobeTaughtInList As List(Of SelectListItem)

    Public Property GradeLevel As String

    Public Property SubjectID As Integer

    Public Property ClassID As Integer

    Public Property Subjectname As String

    Public Function AddSubjecttoClass(ByRef strError As String) As Boolean
        Dim bCreated As Boolean
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            Using objConn As New SqlConnection(connString)
                '1.Check if this acount already exists
                Dim strSQL = <![CDATA[
                    SELECT TOP 1 EntryID FROM ClassesSubjects WHERE TeacherID=@TeacherID AND ClassID=@ClassID AND SubjectID=@SubjectID
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@TeacherID", Integer.Parse(HttpContext.Current.Session("LoggedInTeacherID")))
                        .Parameters.AddWithValue("@ClassID", Integer.Parse(SelectedClass))
                        .Parameters.AddWithValue("@SubjectID", Integer.Parse(SelectedSubject))
                    End With
                    'Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                    If objCommand1.ExecuteScalar() > 0 Then
                        bCreated = False
                        SubjectID = SelectedSubject
                        LoadCurrentSubjectInfo()

                        Dim objTeacherData As New TeacherData
                        objTeacherData.TeacherID = Integer.Parse(HttpContext.Current.Session("LoggedInTeacherID"))
                        objTeacherData.LoadTeacherInfo()

                        Dim objClassData As New ClassData
                        objClassData.ClassID = Integer.Parse(SelectedClass)
                        objClassData.LoadCurrentClassInfo()

                        strError = String.Format("<strong>{0}</strong> <u>is already being taught in</u> <strong>{1}</strong>'s <strong>{2}</strong> class",
                                                 Subjectname, objTeacherData.Firstname + " " + objTeacherData.Lastname, objClassData.Classname)

                    Else
                        Using objCommand2 As New SqlCommand
                            strSQL = <![CDATA[
                   INSERT INTO ClassesSubjects(TeacherID,ClassID,SubjectID) VALUES(@TeacherID,@ClassID,@SubjectID)
                    ]]>.Value()
                            With objCommand2
                                .Connection = objConn
                                ' .Connection.Open()
                                .CommandText = strSQL
                                .Parameters.AddWithValue("@TeacherID", Integer.Parse(HttpContext.Current.Session("LoggedInTeacherID")))
                                .Parameters.AddWithValue("@ClassID", Integer.Parse(SelectedClass))
                                .Parameters.AddWithValue("@SubjectID", Integer.Parse(SelectedSubject))
                                .Parameters.AddWithValue("@Created", DateTime.Now.ToShortDateString)
                            End With
                            If objCommand2.ExecuteNonQuery Then
                                bCreated = True
                                'Get the new ID
                                Dim iIDNo As Integer
                                Using objCommand3 As SqlCommand = New SqlCommand
                                    With objCommand3
                                        .Connection = objConn
                                        '.Connection.Open() 'already open at this point
                                        .CommandText = "SELECT @@Identity"
                                    End With
                                    iIDNo = objCommand3.ExecuteScalar()
                                End Using
                            Else
                                bCreated = False
                            End If
                        End Using
                    End If
                End Using
            End Using
            ' End Using
        Catch ex As Exception
            bCreated = False
        End Try
        Return bCreated
    End Function

    Public Function AddSubjectToSystem(ByRef strError As String) As Boolean
        Dim bCreated As Boolean
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            Using objConn As New SqlConnection(connString)
                Dim strSQL As String = ""
                Using objCommand2 As New SqlCommand
                    strSQL = <![CDATA[
                   INSERT INTO Subjects(SubjectName,ClassID) VALUES(@SubjectName,@ClassID)
                    ]]>.Value()
                    With objCommand2
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@Subjectname", Subjectname)
                        .Parameters.AddWithValue("@ClassID", SelectedClass)
                    End With
                    If objCommand2.ExecuteNonQuery Then
                        bCreated = True
                        'Get the new ID
                        Dim iIDNo As Integer
                        Using objCommand3 As SqlCommand = New SqlCommand
                            With objCommand3
                                .Connection = objConn
                                '.Connection.Open() 'already open at this point
                                .CommandText = "SELECT @@Identity"
                            End With
                            iIDNo = objCommand3.ExecuteScalar()
                        End Using
                    Else
                        bCreated = False
                    End If
                End Using
            End Using
        Catch ex As Exception
            bCreated = False
        End Try
        Return bCreated
    End Function

    Public Function RemoveSubjectFromSystem(ByRef strError As String) As Boolean
        Dim bDeleted As Boolean = False
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            Using objConn As New SqlConnection(connString)
                Dim strSQL As String = ""
                Using objCommand2 As New SqlCommand
                    strSQL = <![CDATA[
                   DELETE FROM Subjects WHERE SubjectID=@SubjectID
                    ]]>.Value()
                    With objCommand2
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@SubjectID", Integer.Parse(SelectedSubject))
                    End With
                    If objCommand2.ExecuteNonQuery Then
                        bDeleted = True
                    Else
                        bDeleted = False
                        strError = "The subject seems to have been removed already"
                    End If
                End Using
            End Using
        Catch ex As Exception
            bDeleted = False
        End Try
        Return bDeleted
    End Function

    Public Function GetTaughtSubjectsInfoDT() As DataTable
        Dim objDS As New DataSet
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            Using objConn As New SqlConnection(connString)
                Dim strSQL = <![CDATA[
                                SELECT ClassesSubjects.SubjectID,GradeLevelName,ClassName,Subjectname FROM ClassesSubjects 
                                LEFT OUTER JOIN Subjects ON ClassesSubjects.SubjectID=Subjects.SubjectID 
                                LEFT OUTER JOIN Classes ON ClassesSubjects.ClassID=Classes.ClassID 
                                LEFT OUTER JOIN GradeLevels ON Subjects.GradeLevelID=GradeLevels.GradeLevelID 
                                WHERE TeacherID=@TeacherID 
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@TeacherID", Integer.Parse(HttpContext.Current.Session("LoggedinTeacherID")))
                    End With
                    Using objDA As New SqlDataAdapter(objCommand1)
                        objDA.Fill(objDS)
                    End Using

                End Using
            End Using
        Catch ex As Exception

        End Try
        Return objDS.Tables(0)
    End Function

    Public Function RemoveSubjectFromClass(ByRef strError As String) As Boolean
        Dim bDeleted As Boolean
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            Using objConn As New SqlConnection(connString)
                '1.Check if this acount already exists
                Dim strSQL = <![CDATA[
                    SELECT TOP 1 EntryID FROM ClassesSubjects WHERE TeacherID=@TeacherID AND ClassID=@ClassID AND SubjectID=@SubjectID
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@TeacherID", Integer.Parse(HttpContext.Current.Session("LoggedInTeacherID")))
                        .Parameters.AddWithValue("@ClassID", Integer.Parse(SelectedClass))
                        .Parameters.AddWithValue("@SubjectID", Integer.Parse(SelectedSubject))
                    End With
                    'Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                    If objCommand1.ExecuteScalar() = 0 Then
                        bDeleted = False
                        SubjectID = SelectedSubject
                        LoadCurrentSubjectInfo()

                        Dim objTeacherData As New TeacherData
                        objTeacherData.TeacherID = Integer.Parse(HttpContext.Current.Session("LoggedInTeacherID"))
                        objTeacherData.LoadTeacherInfo()

                        Dim objClassData As New ClassData
                        objClassData.ClassID = Integer.Parse(SelectedClass)
                        objClassData.LoadCurrentClassInfo()

                        strError = String.Format("<strong>{0}</strong> <u>is not already being taught in</u> <strong>{1}</strong>'s <strong>{2}</strong> class",
                                                 Subjectname, objTeacherData.Firstname + " " + objTeacherData.Lastname, objClassData.Classname)

                    Else
                        Using objCommand2 As New SqlCommand
                            strSQL = <![CDATA[
                            DELETE FROM ClassesSubjects WHERE TeacherID=@TeacherID AND ClassID=@ClassID AND SubjectID=@SubjectID
                    ]]>.Value()
                            With objCommand2
                                .Connection = objConn
                                ' .Connection.Open()
                                .CommandText = strSQL
                                .Parameters.AddWithValue("@TeacherID", Integer.Parse(HttpContext.Current.Session("LoggedInTeacherID")))
                                .Parameters.AddWithValue("@ClassID", Integer.Parse(SelectedClass))
                                .Parameters.AddWithValue("@SubjectID", Integer.Parse(SelectedSubject))
                            End With
                            If objCommand2.ExecuteNonQuery Then
                                bDeleted = True
                            Else
                                bDeleted = False
                            End If
                        End Using
                    End If
                End Using
            End Using
        Catch ex As Exception
            bDeleted = False
        End Try
        Return bDeleted
    End Function

    Public Sub LoadSubjectsList()
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            ' Read the connection string from the web.config file
            Using objConn As New SqlConnection(connString)
                Dim strSQL = <![CDATA[
                                SELECT SubjectID,Subjectname FROM Subjects
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                    End With
                    Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                        While objReader.Read()
                            SubjectsList.Add(New SelectListItem With {.Text = objReader("Subjectname").ToString, .Value = objReader("SubjectID").ToString})
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception

        End Try

    End Sub

    Public Sub LoadClassesTobeTaughtInList()
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            ' Read the connection string from the web.config file
            Using objConn As New SqlConnection(connString)
                Dim strSQL = <![CDATA[
                                SELECT ClassID,Classname FROM Classes
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                    End With
                    Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                        While objReader.Read()
                            ClassesTobeTaughtInList.Add(New SelectListItem With {.Text = objReader("Classname").ToString, .Value = objReader("ClassID").ToString})
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception

        End Try

    End Sub

    Public Sub LoadTaughtClassesList()
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            ' Read the connection string from the web.config file
            Using objConn As New SqlConnection(connString)
                Dim strSQL = <![CDATA[
                                SELECT TeachersClasses.ClassID,Classname FROM TeachersClasses 
                                INNER JOIN Classes ON TeachersClasses.classID=Classes.ClassID 
                                WHERE TeacherID=@TeacherID 
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@TeacherID", Integer.Parse(HttpContext.Current.Session("LoggedinTeacherID")))
                    End With
                    Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                        While objReader.Read()
                            TaughtClassesList.Add(New SelectListItem With {.Text = objReader("Classname").ToString, .Value = objReader("ClassID").ToString})
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception

        End Try

    End Sub

    Public Sub LoadClassesSubjectsTaughtList()
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            ' Read the connection string from the web.config file
            Using objConn As New SqlConnection(connString)
                Dim strSQL = <![CDATA[
                                SELECT ClassesSubjects.ClassID,Classname FROM ClassesSubjects 
                                INNER JOIN Classes ON ClassesSubjects.classID=Classes.ClassID 
                                WHERE TeacherID=@TeacherID 
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@TeacherID", Integer.Parse(HttpContext.Current.Session("LoggedinTeacherID")))
                    End With
                    Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                        While objReader.Read()
                            TaughtClassesList.Add(New SelectListItem With {.Text = objReader("Classname").ToString, .Value = objReader("ClassID").ToString})
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception

        End Try

    End Sub

    Public Sub LoadSubjectInfo(ByVal iSubjectID As Integer)
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            ' Read the connection string from the web.config file
            Using objConn As New SqlConnection(connString)
                'It will be necessary to list the records with GradeLevels of NULL also.  So, use LEFT OUTER JOIN
                Dim strSQL = <![CDATA[
                    SELECT SubjectID, Subjectname,GradeLevelName FROM Subjects
                    LEFT JOIN GradeLevels ON Subjects.GradeLevelID=GradeLevels.GradeLevelID
                    WHERE SubjectID=@SubjectID
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@SubjectID", iSubjectID)
                    End With
                    Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                        If objReader.Read() Then
                            SelectedSubject = objReader("SubjectId").ToString
                            If IsDBNull(objReader("GradeLevelName")) Then
                                GradeLevel = "UNASSIGNED"
                            Else
                                GradeLevel = objReader("GradeLevelName").ToString
                            End If
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception

        End Try


    End Sub

    Public Sub LoadSubjectsForClass(ByVal iClassID As Integer)
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            ' Read the connection string from the web.config file
            Using objConn As New SqlConnection(connString)
                Dim strSQL = <![CDATA[
                    SELECT SubjectID, Subjectname FROM Subjects
                    WHERE ClassID=@ClassID
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@ClassID", iClassID)
                    End With
                    Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                        While objReader.Read()
                            SubjectsList.Add(New SelectListItem With {.Text = objReader("Subjectname").ToString, .Value = objReader("SubjectID").ToString})
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception

        End Try

    End Sub


    Public Sub LoadTaughtSubjectsForClass(ByVal iClassID As Integer)
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            ' Read the connection string from the web.config file
            Using objConn As New SqlConnection(connString)
                Dim strSQL = <![CDATA[
                    SELECT ClassesSubjects.SubjectID, Subjectname FROM ClassesSubjects INNER JOIN Subjects ON
                    ClassesSubjects.SubjectID=Subjects.SubjectID
                    WHERE ClassesSubjects.ClassID=@ClassID AND TeacherID=@TeacherID
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@ClassID", iClassID)
                        .Parameters.AddWithValue("@TeacherID", Integer.Parse(HttpContext.Current.Session("LoggedinTeacherID")))
                    End With
                    Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                        While objReader.Read()
                            SubjectsList.Add(New SelectListItem With {.Text = objReader("Subjectname").ToString, .Value = objReader("SubjectID").ToString})
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception

        End Try

    End Sub

    Public Sub LoadCurrentSubjectInfo()
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            Using objConn As New SqlConnection(connString)
                Dim strSQL = <![CDATA[
                                SELECT SubjectID, Subjectname,ClassID FROM Subjects WHERE SubjectID=@SubjectID
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@SubjectID", SubjectID)
                    End With
                    Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                        If objReader.Read() Then
                            Subjectname = objReader("Subjectname").ToString
                            ClassID = Integer.Parse(objReader("ClassID"))
                            'If IsDBNull(objReader("GradeLevelID")) Then
                            '    GradeLevel = -1
                            'Else
                            '    GradeLevel = Integer.Parse(objReader("GradeLevelID"))
                            'End If

                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Subjectname = ""
            'GradeLevel = -1
        End Try
    End Sub

    Public Function UpdateSubjectINSystem(ByRef strError As String) As Boolean
        Dim bUpdated As Boolean = False
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            Using objConn As New SqlConnection(connString)
                '1.Check if this acount already exists
                Dim strSQL = <![CDATA[
                    SELECT TOP 1 SubjectID FROM Subjects WHERE Subjectname=@Subjectname
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@Subjectname", Subjectname)
                    End With
                    'Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                    Dim iSubjectID As Integer = objCommand1.ExecuteScalar()
                    If iSubjectID = 0 Then
                        'the class to be updated is NOT in the system
                        bUpdated = False
                        strError = String.Format("<strong>{0}</strong> <u>is already available</u> in the system",
                                                  Subjectname)
                    Else
                        Using objCommand2 As New SqlCommand
                            strSQL = <![CDATA[
                    UPDATE Subjects SET Subjectname=@Subjectname, ClassID=@ClassID WHERE SubjectID=@SubjectID
                    ]]>.Value()
                            With objCommand2
                                .Connection = objConn
                                ' .Connection.Open()
                                .CommandText = strSQL
                                .Parameters.AddWithValue("@SubjectID", iSubjectID)
                                .Parameters.AddWithValue("@Subjectname", Subjectname)
                                .Parameters.AddWithValue("@ClassID", SelectedClass)

                            End With
                            If objCommand2.ExecuteNonQuery Then
                                bUpdated = True
                                ''Get the new ID
                                'Dim iIDNo As Integer
                                'Using objCommand3 As SqlCommand = New SqlCommand
                                '    With objCommand3
                                '        .Connection = objConn
                                '        '.Connection.Open() 'already open at this point
                                '        .CommandText = "SELECT @@Identity"
                                '    End With
                                '    iIDNo = objCommand3.ExecuteScalar()
                                'End Using
                            Else
                                bUpdated = False
                                strError = "Could not UPDATE record in Subject table"
                            End If
                        End Using
                    End If
                End Using
            End Using
            ' End Using
        Catch ex As Exception
            bUpdated = False
        End Try
        Return bUpdated
    End Function

    Public Function GetSubjectsDT() As DataTable
        Dim objDS As New DataSet
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            Using objConn As New SqlConnection(connString)
                Dim strSQL = <![CDATA[
                                SELECT SubjectId, SubjectName,Classname FROM Subjects INNER JOIN Classes ON
                                Subjects.ClassID=Classes.ClassID 
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL

                    End With
                    Using objDA As New SqlDataAdapter(objCommand1)
                        objDA.Fill(objDS)
                    End Using

                End Using
            End Using
        Catch ex As Exception

        End Try
        Return objDS.Tables(0)
    End Function
End Class
