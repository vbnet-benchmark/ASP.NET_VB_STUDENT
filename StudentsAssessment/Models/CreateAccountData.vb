Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Data.SqlClient

Public Class CreateAccountData

    Public Sub New()
        SchoolsList = New List(Of SelectListItem)
        'SubjectList.Add(New SelectListItem With {.Text = "School 1", .Value = "1"})
        'SubjectList.Add(New SelectListItem With {.Text = "School 2", .Value = "2"})
        'SubjectList.Add(New SelectListItem With {.Text = "School 3", .Value = "3"})
    End Sub

    <Required(ErrorMessage:="Account type is required")>
    <Display(Name:="ACCOUNT TYPE:")>
    Public Property TypeofAccount As String

    <Required(ErrorMessage:="Firstname is required")>
    <StringLength(25)>
    <Display(Name:="First Name:")>
    Public Property Firstname() As String

    <Required(ErrorMessage:="Lastname is required")>
    <Display(Name:="Last Name:")>
    <StringLength(25)>
    Public Property Lastname As String

    '<Required(ErrorMessage:="Username is required")>
    '<StringLength(25)>
    Public Property Username As String

    '<Required(ErrorMessage:="Password is required")>
    '<StringLength(25)>
    'Public Property Password As String

    <DisplayName("SELECT A SCHOOL:")>
    <Required(ErrorMessage:="A school must be selected")>
    Public Property SchoolsList As List(Of SelectListItem)

    Public Property SelectedSchool As String 'This MUST BE a property (not just a string) for a value to come out

    Public Sub LoadSchools()
        Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
        ' Read the connection string from the web.config file
        Using objConn As New SqlConnection(connString)
            Dim strSQL = <![CDATA[
                                SELECT SchoolID,Schoolname FROM Schools
                    ]]>.Value()
            Using objCommand1 As New SqlCommand
                With objCommand1
                    .Connection = objConn
                    .Connection.Open()
                    .CommandText = strSQL
                End With
                Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                    While objReader.Read()
                        SchoolsList.Add(New SelectListItem With {.Text = objReader("Schoolname").ToString, .Value = objReader("SchoolID").ToString})
                        'SubjectList.Add(New SelectListItem With {.Text = "Chemistry", .Value = "3"})
                        'SubjectList.Add(New SelectListItem With {.Text = "Biology", .Value = "2"})
                    End While
                End Using
            End Using
        End Using
    End Sub

    'Public Function CreateTeacherAccount(ByRef strError As String) As Boolean
    '    Dim bCreated As Boolean
    '    Try
    '        Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
    '        Using objConn As New SqlConnection(connString)
    '            '1.Check if this acount already exists
    '            Dim strSQL = <![CDATA[
    '                SELECT COUNT(TeacherID) FROM Teachers WHERE Username=@Username AND Password=@Password
    '                ]]>.Value()
    '            Using objCommand1 As New SqlCommand
    '                With objCommand1
    '                    .Connection = objConn
    '                    .Connection.Open()
    '                    .CommandText = strSQL
    '                    .Parameters.AddWithValue("@Username", Username)
    '                    .Parameters.AddWithValue("@Password", "teacher")
    '                End With
    '                'Using objReader As SqlDataReader = objCommand1.ExecuteReader()
    '                If objCommand1.ExecuteScalar() > 0 Then
    '                    bCreated = False
    '                    strError = String.Format("Username '{0}' already exists.", Username)
    '                Else
    '                    Using objCommand2 As New SqlCommand
    '                        strSQL = <![CDATA[
    '               INSERT INTO Teachers(Firstname, Lastname, Username, Password,Created,Updated) VALUES(@Firstname, @Lastname, @Username, @Password,@Created,@Updated)
    '                ]]>.Value()
    '                        With objCommand2
    '                            .Connection = objConn
    '                            ' .Connection.Open()
    '                            .CommandText = strSQL
    '                            .Parameters.AddWithValue("@Firstname", Firstname)
    '                            .Parameters.AddWithValue("@Lastname", Lastname)
    '                            .Parameters.AddWithValue("@Username", Username)
    '                            .Parameters.AddWithValue("@Password", "teacher")
    '                            .Parameters.AddWithValue("@Updated", DateTime.Now.ToShortDateString)
    '                            .Parameters.AddWithValue("@Created", DateTime.Now.ToShortDateString)
    '                        End With
    '                        If objCommand2.ExecuteNonQuery Then
    '                            bCreated = True
    '                            'Get the new ID
    '                            Dim iIDNo As Integer
    '                            Using objCommand3 As SqlCommand = New SqlCommand
    '                                With objCommand3
    '                                    .Connection = objConn
    '                                    '.Connection.Open() 'already open at this point
    '                                    .CommandText = "SELECT @@Identity"
    '                                End With
    '                                iIDNo = objCommand3.ExecuteScalar()
    '                            End Using
    '                        Else
    '                            bCreated = False
    '                        End If
    '                    End Using
    '                End If
    '            End Using
    '        End Using
    '        ' End Using
    '    Catch ex As Exception
    '        bCreated = False
    '    End Try
    '    Return bCreated
    'End Function

    'Public Function CreateStudentAccount(ByRef strError As String) As Boolean
    '    Dim bCreated As Boolean
    '    Try
    '        Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
    '        Using objConn As New SqlConnection(connString)
    '            '1.Check if this acount already exists
    '            Dim strSQL = <![CDATA[
    '                SELECT COUNT(studentID) FROM Students WHERE Username=@Username AND Password=@Password
    '                ]]>.Value()
    '            Using objCommand1 As New SqlCommand
    '                With objCommand1
    '                    .Connection = objConn
    '                    .Connection.Open()
    '                    .CommandText = strSQL
    '                    .Parameters.AddWithValue("@Username", Username)
    '                    .Parameters.AddWithValue("@Password", "student")
    '                End With
    '                'Using objReader As SqlDataReader = objCommand1.ExecuteReader()
    '                If objCommand1.ExecuteScalar() > 0 Then
    '                    bCreated = False
    '                    strError = String.Format("Username '{0}' already exists.", Username)
    '                Else
    '                    Using objCommand2 As New SqlCommand
    '                        strSQL = <![CDATA[
    '               INSERT INTO Students(Firstname, Lastname, Username, Password,Created,Updated) VALUES(@Firstname, @Lastname, @Username, @Password,@Created,@Updated)
    '                ]]>.Value()
    '                        With objCommand2
    '                            .Connection = objConn
    '                            ' .Connection.Open()
    '                            .CommandText = strSQL
    '                            .Parameters.AddWithValue("@Firstname", Firstname)
    '                            .Parameters.AddWithValue("@Lastname", Lastname)
    '                            .Parameters.AddWithValue("@Username", Username)
    '                            .Parameters.AddWithValue("@Password", "student")
    '                            .Parameters.AddWithValue("@Updated", DateTime.Now.ToShortDateString)
    '                            .Parameters.AddWithValue("@Created", DateTime.Now.ToShortDateString)
    '                        End With
    '                        If objCommand2.ExecuteNonQuery Then
    '                            bCreated = True
    '                            'Get the new ID
    '                            Dim iIDNo As Integer
    '                            Using objCommand3 As SqlCommand = New SqlCommand
    '                                With objCommand3
    '                                    .Connection = objConn
    '                                    '.Connection.Open() 'already open at this point
    '                                    .CommandText = "SELECT @@Identity"
    '                                End With
    '                                iIDNo = objCommand3.ExecuteScalar()
    '                            End Using
    '                        Else
    '                            bCreated = False
    '                        End If
    '                    End Using
    '                End If
    '            End Using
    '        End Using
    '        ' End Using
    '    Catch ex As Exception
    '        bCreated = False
    '    End Try
    '    Return bCreated
    'End Function

    Public Function CreateTeacherAccount(ByRef strError As String) As Boolean
        Dim bCreated As Boolean
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            Using objConn As New SqlConnection(connString)
                Using objCommand2 As New SqlCommand
                    Dim strSQL = <![CDATA[
                   INSERT INTO Teachers(Firstname, Lastname, Password,SchoolID,Created,Updated) VALUES(@Firstname, @Lastname, @Password,@SchoolID,@Created,@Updated)
                    ]]>.Value()
                    With objCommand2
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@Firstname", Firstname)
                        .Parameters.AddWithValue("@Lastname", Lastname)
                        '.Parameters.AddWithValue("@Username", Username) 'UPDATE seperately for now
                        .Parameters.AddWithValue("@Password", "teacher")
                        .Parameters.AddWithValue("@SchoolID", GetSchoolID(SelectedSchool))
                        .Parameters.AddWithValue("@Updated", DateTime.Now.ToShortDateString)
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
                            strSQL = <![CDATA[
                   UPDATE Teachers SET Username=@Username WHERE TeacherID=@TeacherID
                    ]]>.Value()
                            Using objCommand4 As SqlCommand = New SqlCommand
                                With objCommand4
                                    .Connection = objConn
                                    .CommandText = strSQL
                                    .Parameters.AddWithValue("@Username", "Teacher" + iIDNo.ToString)
                                    .Parameters.AddWithValue("@TeacherID", iIDNo)
                                End With
                                If objCommand4.ExecuteNonQuery() < 0 Then
                                    strError = String.Format("The username for teacher '{0}' could not be set.  Please let admin know", Firstname + " " + Lastname)
                                End If
                            End Using

                        End Using
                    Else
                        bCreated = False
                        'strError = String.Format("The record for teacher '{0}' could not be inserted into table.  Please let admin know", Firstname + " " + Lastname)
                    End If
                End Using
            End Using
        Catch ex As Exception
            bCreated = False
        End Try
        Return bCreated
    End Function

    Public Function CreateStudentAccount(ByRef strError As String) As Boolean
        Dim bCreated As Boolean
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            Using objConn As New SqlConnection(connString)
                Using objCommand2 As New SqlCommand
                    Dim strSQL = <![CDATA[
                   INSERT INTO Students(Firstname, Lastname,Password,SchoolID,Created,Updated) VALUES(@Firstname, @Lastname, @Password,@SchoolID,@Created,@Updated)
                    ]]>.Value()
                    With objCommand2
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@Firstname", Firstname)
                        .Parameters.AddWithValue("@Lastname", Lastname)
                        ' .Parameters.AddWithValue("@Username", Username) ''UPDATE seperately for now
                        .Parameters.AddWithValue("@Password", "student")
                        .Parameters.AddWithValue("@SchoolID", GetSchoolID(SelectedSchool))
                        .Parameters.AddWithValue("@Updated", DateTime.Now.ToShortDateString)
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
                            strSQL = <![CDATA[
                   UPDATE Students SET Username=@Username WHERE StudentID=@StudentID
                    ]]>.Value()
                            Using objCommand4 As SqlCommand = New SqlCommand
                                With objCommand4
                                    .Connection = objConn
                                    .CommandText = strSQL
                                    'TODO - Check if this newly-to-be-created username already exists.  If so, increment no and
                                    'save.  Otherwise, leave it at that
                                    '.Parameters.AddWithValue("@Username", Left(Firstname, 5) + Left(Lastname, 2))
                                    .Parameters.AddWithValue("@Username", Get5x2Username(iIDNo, Firstname, Lastname))
                                    .Parameters.AddWithValue("@StudentID", iIDNo)
                                End With
                                If objCommand4.ExecuteNonQuery() < 0 Then
                                    strError = String.Format("The username for student '{0}' could not be set.  Please let admin know", Firstname + " " + Lastname)
                                End If
                            End Using
                        End Using
                    Else
                        bCreated = False
                    End If
                End Using
            End Using
            ' End Using
        Catch ex As Exception
            bCreated = False
        End Try
        Return bCreated
    End Function

    Private Function Get5x2Username(ByVal iID As Integer, ByVal strFirstname As String, strLastname As String) As String
        Dim strUsername As String = Left(Firstname, 5) + Left(Lastname, 2), strUsername1 As String = ""
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            Using objConn As New SqlConnection(connString)
                Using objCommand2 As New SqlCommand
                    Dim strSQL = <![CDATA[
                   SELECT TOP 1 StudentID, Username FROM Students WHERE Username LIKE @Username ORDER BY Username DESC
                    ]]>.Value()
                    With objCommand2
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@Username", strUsername + "%") ''UPDATE seperately for now
                        Using objReader As SqlDataReader = objCommand2.ExecuteReader()
                            If objReader.Read() Then
                                'Username in use
                                strUsername1 = objReader("Username").ToString
                                If strUsername = strUsername1 Then
                                    strUsername = strUsername + "1"
                                Else
                                    strUsername = strUsername + (Integer.Parse(strUsername1.Replace(strUsername, "")) + 1).ToString
                                End If
                            End If
                        End Using
                    End With
                End Using
            End Using
        Catch ex As Exception

        End Try
        Return strUsername
    End Function
    Public Function GetSchoolID(ByRef strSchoolname As String) As Integer
        Dim iSchoolId As Integer = 0
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            ' Read the connection string from the web.config file
            Using objConn As New SqlConnection(connString)
                Dim strSQL = <![CDATA[
                                SELECT SchoolID FROM Schools WHERE Schoolname=@Schoolname
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@Schoolname", SelectedSchool)
                    End With
                    If objCommand1.ExecuteScalar() > 0 Then
                        iSchoolId = Integer.Parse(objCommand1.ExecuteScalar())
                    Else
                        iSchoolId = 0
                    End If
                End Using
            End Using
        Catch ex As Exception
            iSchoolId = 0
        End Try
        Return iSchoolId
    End Function
End Class