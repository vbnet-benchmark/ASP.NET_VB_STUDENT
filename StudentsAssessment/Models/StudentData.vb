Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Data.SqlClient


Public Class StudentData
    Public Sub New()
        StudentsList = New List(Of SelectListItem)
        TaughtClassesList = New List(Of SelectListItem)

    End Sub

    <DisplayName("SELECT A CLASS:")>
    <Required(ErrorMessage:="A class must be selected")>
    Public Property TaughtClassesList As List(Of SelectListItem)

    Public Property SelectedClass As String 'This MUST BE a property (not just a string) for a value to come out

    <DisplayName("SELECT A STUDENT TO ADD:")>
    <Required(ErrorMessage:="A STUDENT must be selected")>
    Public Property StudentsList As List(Of SelectListItem)

    Public Property SelectedStudent As String 'This MUST BE a property (not just a string) for a value to come out

    Public Property Joindate As DateTime

    Public Property StudentID As Integer
    Public Property Firstname As String

    Public Property Lastname As String


    Public Function AddStudenttoClass(ByRef strError As String) As Boolean
        Dim bCreated As Boolean
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            Using objConn As New SqlConnection(connString)
                '1.Check if this acount already exists
                Dim strSQL = <![CDATA[
                    SELECT TOP 1 EntryID FROM Students_Classes WHERE TeacherID=@TeacherID AND ClassID=@ClassID AND StudentID=@StudentID
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@TeacherID", Integer.Parse(HttpContext.Current.Session("LoggedInTeacherID")))
                        .Parameters.AddWithValue("@ClassID", Integer.Parse(SelectedClass))
                        .Parameters.AddWithValue("@StudentID", Integer.Parse(SelectedStudent))
                    End With
                    'Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                    If objCommand1.ExecuteScalar() > 0 Then
                        bCreated = False
                        LoadStudentInfo(SelectedStudent)
                        Dim objTeacherData As New TeacherData
                        objTeacherData.TeacherID = Integer.Parse(HttpContext.Current.Session("LoggedInTeacherID"))
                        objTeacherData.LoadTeacherInfo()

                        Dim objClassData As New ClassData
                        objClassData.ClassID = Integer.Parse(SelectedClass)
                        objClassData.LoadCurrentClassInfo()

                        strError = String.Format("The student <strong>{0}</strong> <u>is already enrolled in</u> <strong>{1}</strong>'s <strong>{2}</strong> class",
                                                 Firstname + " " + Lastname, objTeacherData.Firstname + " " + objTeacherData.Lastname, objClassData.Classname)
                    Else
                        Using objCommand2 As New SqlCommand
                            strSQL = <![CDATA[
                   INSERT INTO Students_Classes(TeacherID,ClassID,StudentID) VALUES(@TeacherID,@ClassID,@StudentID)
                    ]]>.Value()
                            With objCommand2
                                .Connection = objConn
                                ' .Connection.Open()
                                .CommandText = strSQL
                                .Parameters.AddWithValue("@TeacherID", Integer.Parse(HttpContext.Current.Session("LoggedInTeacherID")))
                                .Parameters.AddWithValue("@ClassID", Integer.Parse(SelectedClass))
                                .Parameters.AddWithValue("@StudentID", Integer.Parse(SelectedStudent))
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

    Public Function GetStudentsAlreadyinClassInfoDT() As DataTable
        Dim objDS As New DataSet
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            Using objConn As New SqlConnection(connString)
                Dim strSQL = <![CDATA[
                               SELECT Students_Classes.StudentID,ClassName,Firstname+' '+Lastname as Studentname FROM Students_Classes 
                                LEFT OUTER JOIN Classes ON Students_Classes.ClassID=Classes.ClassID 
                                LEFT OUTER JOIN Students ON Students_Classes.studentId=Students.StudentID
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

    Public Function GetStudentsDT() As DataTable
        Dim objDS As New DataSet
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            Using objConn As New SqlConnection(connString)
                ''All student (1st table) records will show. NULL schools will show as blank
                Dim strSQL = <![CDATA[
                                SELECT DISTINCT StudentID, Firstname+' '+Lastname AS StudentName,Schoolname FROM Students LEFT OUTER JOIN Schools ON
                                Students.SchoolID=Schools.SchoolID 
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

    Public Function RemoveStudentFromClass(ByRef strError As String) As Boolean
        Dim bDeleted As Boolean
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            Using objConn As New SqlConnection(connString)
                '1.Check if this acount already exists
                Dim strSQL = <![CDATA[
                    SELECT TOP 1 EntryID FROM Students_Classes WHERE TeacherID=@TeacherID AND ClassID=@ClassID AND StudentID=@StudentID
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@TeacherID", Integer.Parse(HttpContext.Current.Session("LoggedInTeacherID")))
                        .Parameters.AddWithValue("@ClassID", Integer.Parse(SelectedClass))
                        .Parameters.AddWithValue("@StudentID", Integer.Parse(SelectedStudent))
                    End With
                    'Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                    If objCommand1.ExecuteScalar() = 0 Then
                        bDeleted = False
                        LoadStudentInfo(SelectedStudent)
                        Dim objTeacherData As New TeacherData
                        objTeacherData.TeacherID = Integer.Parse(HttpContext.Current.Session("LoggedInTeacherID"))
                        objTeacherData.LoadTeacherInfo()

                        Dim objClassData As New ClassData
                        objClassData.ClassID = Integer.Parse(SelectedClass)
                        objClassData.LoadCurrentClassInfo()

                        strError = String.Format("The student <strong>{0}</strong> <u>is not already enrolled in</u> <strong>{1}</strong>'s <strong>{2}</strong> class",
                                                 Firstname + " " + Lastname, objTeacherData.Firstname + " " + objTeacherData.Lastname, objClassData.Classname)
                    Else
                        Using objCommand2 As New SqlCommand
                            strSQL = <![CDATA[
                            DELETE FROM Students_Classes WHERE TeacherID=@TeacherID AND ClassID=@ClassID AND StudentID=@StudentID
                    ]]>.Value()
                            With objCommand2
                                .Connection = objConn
                                ' .Connection.Open()
                                .CommandText = strSQL
                                .Parameters.AddWithValue("@TeacherID", Integer.Parse(HttpContext.Current.Session("LoggedInTeacherID")))
                                .Parameters.AddWithValue("@ClassID", Integer.Parse(SelectedClass))
                                .Parameters.AddWithValue("@StudentID", Integer.Parse(SelectedStudent))
                            End With
                            If objCommand2.ExecuteNonQuery Then
                                bDeleted = True
                                'Get the new ID
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
                                bDeleted = False
                            End If
                        End Using
                    End If
                End Using
            End Using
            ' End Using
        Catch ex As Exception
            bDeleted = False
        End Try
        Return bDeleted
    End Function

    Public Sub LoadStudentsList()
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            ' Read the connection string from the web.config file
            Using objConn As New SqlConnection(connString)
                Dim strSQL = <![CDATA[
                                SELECT StudentID, Firstname+' '+Lastname AS Studentname FROM Students
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                    End With
                    Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                        While objReader.Read()
                            StudentsList.Add(New SelectListItem With {.Text = objReader("Studentname").ToString, .Value = objReader("StudentID").ToString})
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
                            'SubjectList.Add(New SelectListItem With {.Text = "Chemistry", .Value = "3"})
                            'SubjectList.Add(New SelectListItem With {.Text = "Biology", .Value = "2"})
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception

        End Try

    End Sub

    Public Sub LoadStudentInfo(ByVal iStudentID As Integer)
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            ' Read the connection string from the web.config file
            Using objConn As New SqlConnection(connString)
                'It will be necessary to list the records with GradeLevels of NULL also.  So, use LEFT OUTER JOIN
                Dim strSQL = <![CDATA[
                    SELECT Joindate FROM Students
                    WHERE StudentID=@StudentID
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@StudentID", iStudentID)
                    End With
                    Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                        If objReader.Read() Then
                            ' SelectedStudent = objReader("StudentID").ToString
                            If IsDBNull(objReader("JoinDate")) Then
                                Joindate = DateTime.MinValue
                            Else
                                Joindate = DateTime.Parse(objReader("JoinDate")).ToShortDateString
                            End If
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            SelectedStudent = 0
            Joindate = DateTime.MinValue
        End Try


    End Sub

    Public Sub LoadStudentsForClass(ByVal iClassID As Integer)
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            ' Read the connection string from the web.config file
            Using objConn As New SqlConnection(connString)
                Dim strSQL = <![CDATA[
                    SELECT StudentID, Studentname FROM Students_Classes INNER JOIN
                    Classes ON Students_Classes.ClassID=Classes.ClassID 
                    INNER JOIN Students on Students.StudentID=Students_Classes.StudentID
                    WHERE Students_Classes.ClassID=@ClassID
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
                            StudentsList.Add(New SelectListItem With {.Text = objReader("Studentname").ToString, .Value = objReader("StudentID").ToString})
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception

        End Try

    End Sub


    Public Sub LoadStudentsAlreadyinClass(ByVal iClassID As Integer)
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            ' Read the connection string from the web.config file
            Using objConn As New SqlConnection(connString)
                Dim strSQL = <![CDATA[
                    SELECT Students_Classes.StudentID, Firstname+' '+ Lastname AS StudentName FROM Students_Classes INNER JOIN Students ON
                    Students_Classes.StudentID=Students.StudentID
                    WHERE Students_Classes.ClassID=@ClassID AND TeacherID=@TeacherID
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
                            StudentsList.Add(New SelectListItem With {.Text = objReader("Studentname").ToString, .Value = objReader("StudentID").ToString})
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception

        End Try

    End Sub

    Public Sub LoadCurrentStudentInfo()
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            ' Read the connection string from the web.config file
            Using objConn As New SqlConnection(connString)
                Dim strSQL = <![CDATA[
                                SELECT StudentID, Firstname,Lastname FROM Students WHERE StudentID=@StudentID
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@StudentID", StudentID)
                    End With
                    Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                        If objReader.Read() Then
                            Firstname = objReader("Firstname").ToString
                            Lastname = objReader("Lastname").ToString
                            If IsDBNull(objReader("Joindate")) Then
                                Joindate = DateTime.MinValue
                            Else
                                Joindate = DateTime.Parse(objReader("Joindate"))
                            End If
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Joindate = DateTime.MinValue
        End Try
    End Sub

    ''' <summary>
    ''' Connection string for the legacy, read-only student-sync service used by the admin
    ''' dashboard to pull archived-enrollment counts.  Predates the main app's config-based
    ''' connection string (myConnectionString) and was never migrated.
    ''' </summary>
    Public Shared Function GetLegacySyncConnectionString() As String
        Return "Data Source=legacy-sync01;Initial Catalog=LegacyStudentSync;User Id=sync_reader;Password=Sync$Legacy88;"
    End Function

    ''' <summary>
    ''' Connection string for the same legacy-sync service, migrated to read its credential
    ''' from Web.config instead of embedding it in source.
    ''' </summary>
    Public Shared Function GetLegacySyncConnectionStringSafe() As String
        Return ConfigurationManager.ConnectionStrings("LegacySyncConnectionString").ConnectionString
    End Function

    Public Function RemoveStudentFromSystem(ByRef strError As String) As Boolean
        Dim bDeleted As Boolean = False
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            Using objConn As New SqlConnection(connString)
                Dim strSQL As String = ""
                Using objCommand2 As New SqlCommand
                    strSQL = <![CDATA[
                   DELETE FROM Students WHERE StudentID=@StudentID
                    ]]>.Value()
                    With objCommand2
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@StudentID", Integer.Parse(SelectedStudent))
                    End With
                    If objCommand2.ExecuteNonQuery Then
                        bDeleted = True
                    Else
                        bDeleted = False
                        strError = "The student seems to have been removed already"
                    End If
                End Using
            End Using
        Catch ex As Exception
            bDeleted = False
        End Try
        Return bDeleted
    End Function
End Class
