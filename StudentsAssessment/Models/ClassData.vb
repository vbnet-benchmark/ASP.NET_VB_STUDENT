Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Data.SqlClient


Public Class ClassData
    Public Sub New()
        ClassesList = New List(Of SelectListItem)
        TaughtClassesList = New List(Of SelectListItem)
        TeacherstoAssigntoList = New List(Of SelectListItem)
    End Sub

    Public Property ClassID As Integer
    <Required(ErrorMessage:="Class name must be provided")>
    Public Property Classname As String

    <DisplayName("SELECT A CLASS TO ADD:")>
    <Required(ErrorMessage:="A class must be selected")>
    Public Property ClassesList As List(Of SelectListItem)

    <DisplayName("SELECT A TEACHER TO ASSIGN CLASS TO:")>
    <Required(ErrorMessage:="A teacher must be selected")>
    Public Property TeacherstoAssigntoList As List(Of SelectListItem)

    <DisplayName("SELECT A CLASS TO REMOVE:")>
    <Required(ErrorMessage:="A class must be selected")>
    Public Property TaughtClassesList As List(Of SelectListItem)

    Public Property SelectedClass As String 'This MUST BE a property (not just a string) for a value to come out

    Public Property SelectedTeacher As String 'This MUST BE a property (not just a string) for a value to come out

    '<Required(ErrorMessage:="No. of credits is required")>
    <Display(Name:="Number of credit hours:")>
    <DisplayFormat(DataFormatString:="{0:n2}", ApplyFormatInEditMode:=True)>
    Public Property NumCreditHrs() As Decimal

    'DATE THINGS NEED SOME EXPERIMENTATION
    '<Required(ErrorMessage:="Start Date is required")>
    '<Display(Name:="Class Start date:")>
    '<DataType(DataType.Date)>
    '<DisplayFormat(ApplyFormatInEditMode:=True, DataFormatString:="{0:MM/dd/yyyy}")>
    'Public Property dtStart As DateTime = DateTime.Now

    '<Required(ErrorMessage:="Class end date is required")>
    '<Display(Name:="Class End date:")>
    '<DataType(DataType.Date)>
    '<DisplayFormat(ApplyFormatInEditMode:=True, DataFormatString:="{0:MM/dd/yyyy}")>
    'Public Property dtEnd As DateTime = DateTime.Now

    <Display(Name:="CLASS START DATE")>
    Public Property StartDate() As String

    <Display(Name:="CLASS END DATE")>
    Public Property EndDate() As String

    '<Required(ErrorMessage:="Class Duration is required")>
    <Display(Name:="Class Duration:")>
    Public Property ClassDuration() As Integer

    Public Function CreateClass(ByRef strError As String) As Boolean
        Dim bCreated As Boolean
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            Using objConn As New SqlConnection(connString)
                '1.Check if this acount already exists
                Dim strSQL = <![CDATA[
                    SELECT TOP 1 EntryID FROM TeachersClasses WHERE TeacherID=@TeacherID AND ClassID=@ClassID
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@TeacherID", Integer.Parse(HttpContext.Current.Session("LoggedInTeacherID")))
                        .Parameters.AddWithValue("@ClassID", Integer.Parse(SelectedClass))
                    End With
                    'Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                    If objCommand1.ExecuteScalar() > 0 Then
                        bCreated = False
                        ClassID = SelectedClass
                        LoadCurrentClassInfo()
                        Dim objTeacherData As New TeacherData
                        objTeacherData.TeacherID = Integer.Parse(HttpContext.Current.Session("LoggedInTeacherID"))
                        objTeacherData.LoadTeacherInfo()

                        'Dim objClassData As New ClassData
                        'objClassData.ClassID = Integer.Parse(SelectedClass)
                        'objClassData.LoadCurrentClassInfo()

                        strError = String.Format("<strong>{0}</strong> <u>already teaches</u> <strong>{1}</strong> class",
                                                 objTeacherData.Firstname + " " + objTeacherData.Lastname, Classname)


                    Else
                        Using objCommand2 As New SqlCommand
                            strSQL = <![CDATA[
                   INSERT INTO TeachersClasses(TeacherID,ClassID) VALUES(@TeacherID,@ClassID)
                    ]]>.Value()
                            With objCommand2
                                .Connection = objConn
                                ' .Connection.Open()
                                .CommandText = strSQL
                                .Parameters.AddWithValue("@TeacherID", Integer.Parse(HttpContext.Current.Session("LoggedInTeacherID")))
                                .Parameters.AddWithValue("@ClassID", Integer.Parse(SelectedClass))
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

    Public Function AddClassTOSystem(ByRef strError As String) As Boolean
        Dim bCreated As Boolean
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            Using objConn As New SqlConnection(connString)
                '1.Check if this acount already exists
                Dim strSQL = <![CDATA[
                    SELECT TOP 1 ClassID FROM Classes WHERE Classname=@Classname
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@Classname", Classname)
                    End With
                    'Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                    If objCommand1.ExecuteScalar() > 0 Then
                        'the class to be created already is there in system.
                        bCreated = False
                        'ClassID = SelectedClass
                        'LoadCurrentClassInfo()
                        'Dim objTeacherData As New TeacherData
                        'objTeacherData.TeacherID = Integer.Parse(HttpContext.Current.Session("LoggedInTeacherID"))
                        'objTeacherData.LoadTeacherInfo()

                        ''Dim objClassData As New ClassData
                        ''objClassData.ClassID = Integer.Parse(SelectedClass)
                        ''objClassData.LoadCurrentClassInfo()

                        strError = String.Format("<strong>{0}</strong> <u>is already available</u> in the system",
                                                  Classname)


                    Else
                        Using objCommand2 As New SqlCommand
                            strSQL = <![CDATA[
                   INSERT INTO Classes (Classname, ClassStart, ClassEnd, ClassDuration, CreditHours) VALUES
                    (@Classname, @ClassStart, @ClassEnd, @ClassDuration, @CreditHours)
                    ]]>.Value()
                            With objCommand2
                                .Connection = objConn
                                ' .Connection.Open()
                                .CommandText = strSQL
                                .Parameters.AddWithValue("@Classname", Classname)
                                If StartDate Is Nothing Then
                                    .Parameters.AddWithValue("@ClassStart", DBNull.Value)
                                Else
                                    .Parameters.AddWithValue("@ClassStart", StartDate)
                                End If

                                If EndDate Is Nothing Then
                                    .Parameters.AddWithValue("@ClassEnd", DBNull.Value)
                                Else
                                    .Parameters.AddWithValue("@ClassEnd", EndDate)
                                End If

                                .Parameters.AddWithValue("@ClassDuration", ClassDuration)
                                .Parameters.AddWithValue("@CreditHours", NumCreditHrs)
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
                                strError = "Could not insert record into Class table"
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

    Public Function UpdateClassINSystem(ByRef strError As String) As Boolean
        Dim bUpdated As Boolean = False
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            Using objConn As New SqlConnection(connString)
                '1.Check if this acount already exists
                Dim strSQL = <![CDATA[
                    SELECT TOP 1 ClassID FROM Classes WHERE Classname=@Classname
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@Classname", Classname)
                    End With
                    'Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                    Dim iClassId As Integer = objCommand1.ExecuteScalar()
                    If iClassId = 0 Then
                        'the class to be updated is NOT in the system
                        bUpdated = False
                        strError = String.Format("<strong>{0}</strong> <u>is already available</u> in the system",
                                                  Classname)
                    Else
                        Using objCommand2 As New SqlCommand
                            strSQL = <![CDATA[
                    UPDATE Classes SET Classname=@Classname, ClassStart=@ClassStart, ClassEnd=@ClassEnd, ClassDuration=@ClassDuration,
                    CreditHours=@CreditHours WHERE ClassId=@ClassID
                    ]]>.Value()
                            With objCommand2
                                .Connection = objConn
                                ' .Connection.Open()
                                .CommandText = strSQL
                                .Parameters.AddWithValue("@ClassID", iClassId)
                                .Parameters.AddWithValue("@Classname", Classname)
                                If StartDate Is Nothing Then
                                    .Parameters.AddWithValue("@ClassStart", DBNull.Value)
                                Else
                                    .Parameters.AddWithValue("@ClassStart", DateTime.Parse(StartDate))
                                End If

                                If EndDate Is Nothing Then
                                    .Parameters.AddWithValue("@ClassEnd", DBNull.Value)
                                Else
                                    .Parameters.AddWithValue("@ClassEnd", DateTime.Parse(EndDate))
                                End If

                                .Parameters.AddWithValue("@ClassDuration", ClassDuration)
                                .Parameters.AddWithValue("@CreditHours", NumCreditHrs)
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
                                strError = "Could not UPDATE record in Class table"
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

    Public Function AssignClasstoTeacher(ByRef strError As String) As Boolean
        Dim bUpdated As Boolean = False
        Try
            If TeacherteachesClass(SelectedTeacher, SelectedClass) = False Then
                'Teaches does not already teach this class
                Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
                Using objConn As New SqlConnection(connString)
                    Dim strSQL As String = ""
                    Using objCommand2 As New SqlCommand
                        strSQL = <![CDATA[
                    INSERT INTO TeachersClasses (TeacherID, ClassID) VALUES (@TeacherID, @ClassID)
                    ]]>.Value()
                        With objCommand2
                            .Connection = objConn
                            .Connection.Open()
                            .CommandText = strSQL
                            .Parameters.AddWithValue("@ClassID", SelectedClass)
                            .Parameters.AddWithValue("@TeacherID", SelectedTeacher)
                        End With
                        If objCommand2.ExecuteNonQuery Then
                            bUpdated = True
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
                            bUpdated = False
                            strError = "AssignClasstoTeacher problem"
                        End If
                    End Using
                End Using
            Else
                'Teaches does not already teach this class
                strError = "Already teaches this class"
                bUpdated = False
            End If

        Catch ex As Exception
            bUpdated = False
        End Try
        Return bUpdated
    End Function

    Public Function GetTaughtClassesInfoDT() As DataTable
        Dim objDS As New DataSet
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
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
                    Using objDA As New SqlDataAdapter(objCommand1)
                        objDA.Fill(objDS)
                    End Using

                End Using
            End Using
        Catch ex As Exception

        End Try
        Return objDS.Tables(0)
    End Function

    Public Function GetClassesDT() As DataTable
        Dim objDS As New DataSet
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            Using objConn As New SqlConnection(connString)
                Dim strSQL = <![CDATA[
                                SELECT ClassID,Classname FROM Classes
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        '.Parameters.AddWithValue("@TeacherID", Integer.Parse(HttpContext.Current.Session("LoggedinTeacherID")))
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

    Public Function RemoveClass(ByRef strError As String) As Boolean
        Dim bDeleted As Boolean
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            Using objConn As New SqlConnection(connString)
                '1.Check if this acount already exists
                Dim strSQL = <![CDATA[
                    SELECT TOP 1 EntryID FROM TeachersClasses WHERE TeacherID=@TeacherID AND ClassID=@ClassID
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@TeacherID", Integer.Parse(HttpContext.Current.Session("LoggedInTeacherID")))
                        .Parameters.AddWithValue("@ClassID", Integer.Parse(SelectedClass))
                    End With
                    'Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                    If objCommand1.ExecuteScalar() = 0 Then
                        bDeleted = False
                        ClassID = SelectedClass
                        LoadCurrentClassInfo()
                        Dim objTeacherData As New TeacherData
                        objTeacherData.TeacherID = Integer.Parse(HttpContext.Current.Session("LoggedInTeacherID"))
                        objTeacherData.LoadTeacherInfo()

                        'Dim objClassData As New ClassData
                        'objClassData.ClassID = Integer.Parse(SelectedClass)
                        'objClassData.LoadCurrentClassInfo()

                        strError = String.Format("<strong>{0}</strong> <u>does not teach</u> <strong>{1}</strong> class at all",
                                                 objTeacherData.Firstname + " " + objTeacherData.Lastname, Classname)
                    Else
                        Using objCommand2 As New SqlCommand
                            strSQL = <![CDATA[
                            DELETE FROM TeachersClasses WHERE TeacherID=@TeacherID AND ClassID=@ClassID
                    ]]>.Value()
                            With objCommand2
                                .Connection = objConn
                                ' .Connection.Open()
                                .CommandText = strSQL
                                .Parameters.AddWithValue("@TeacherID", Integer.Parse(HttpContext.Current.Session("LoggedInTeacherID")))
                                .Parameters.AddWithValue("@ClassID", Integer.Parse(SelectedClass))
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

    Public Function RemoveClassFromSystem(ByRef strError As String) As Boolean
        Dim bDeleted As Boolean
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            Using objConn As New SqlConnection(connString)
                '1.Check if this acount already exists
                Dim strSQL = <![CDATA[
                    SELECT TOP 1 ClassID FROM Classes WHERE ClassID=@ClassID
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@ClassID", Integer.Parse(SelectedClass))
                    End With
                    'Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                    If objCommand1.ExecuteScalar() = 0 Then
                        bDeleted = False
                        'ClassID = SelectedClass
                        'LoadCurrentClassInfo()
                        'Dim objTeacherData As New TeacherData
                        'objTeacherData.TeacherID = Integer.Parse(HttpContext.Current.Session("LoggedInTeacherID"))
                        'objTeacherData.LoadTeacherInfo()

                        'Dim objClassData As New ClassData
                        'objClassData.ClassID = Integer.Parse(SelectedClass)
                        'objClassData.LoadCurrentClassInfo()

                        strError = String.Format("The class <strong>{0}</strong> <u>is not in the system</u> at all",
                                                 Classname)
                    Else
                        Using objCommand2 As New SqlCommand
                            strSQL = <![CDATA[
                            DELETE FROM Classes WHERE ClassID=@ClassID
                    ]]>.Value()
                            With objCommand2
                                .Connection = objConn
                                ' .Connection.Open()
                                .CommandText = strSQL
                                .Parameters.AddWithValue("@ClassID", Integer.Parse(SelectedClass))
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

    Public Sub LoadClassesList()
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
                        ClassesList.Add(New SelectListItem With {.Text = objReader("Classname").ToString, .Value = objReader("ClassID").ToString})
                        'SubjectList.Add(New SelectListItem With {.Text = "Chemistry", .Value = "3"})
                        'SubjectList.Add(New SelectListItem With {.Text = "Biology", .Value = "2"})
                    End While
                End Using
            End Using
        End Using
    End Sub

    Public Sub LoadTaughtClassesList()
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
    End Sub

    Public Sub LoadTeacherstoAssigntoList()
        Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
        ' Read the connection string from the web.config file
        Using objConn As New SqlConnection(connString)
            Dim strSQL = <![CDATA[
                                SELECT TeacherID, Firstname + ' ' + Lastname as TeacherName FROM Teachers
                    ]]>.Value()
            Using objCommand1 As New SqlCommand
                With objCommand1
                    .Connection = objConn
                    .Connection.Open()
                    .CommandText = strSQL
                End With
                Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                    While objReader.Read()
                        TeacherstoAssigntoList.Add(New SelectListItem With {.Text = objReader("TeacherName").ToString, .Value = objReader("TeacherID").ToString})
                        'SubjectList.Add(New SelectListItem With {.Text = "Chemistry", .Value = "3"})
                        'SubjectList.Add(New SelectListItem With {.Text = "Biology", .Value = "2"})
                    End While
                End Using
            End Using
        End Using
    End Sub

    Public Sub LoadClassInfo(ByVal iclassID As Integer)
        Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
        ' Read the connection string from the web.config file
        Using objConn As New SqlConnection(connString)
            Dim strSQL = <![CDATA[
                    SELECT ClassID,Classname,ClassStart, ClassEnd, ClassDuration, CreditHours FROM Classes
                    WHERE ClassID=@ClassID
                    ]]>.Value()
            Using objCommand1 As New SqlCommand
                With objCommand1
                    .Connection = objConn
                    .Connection.Open()
                    .CommandText = strSQL
                    .Parameters.AddWithValue("@ClassID", iclassID)
                End With
                Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                    If objReader.Read() Then
                        SelectedClass = objReader("ClassID").ToString
                        If IsDBNull(objReader("ClassStart")) Then
                            StartDate = "" 'DateTime.MinValue
                        Else
                            StartDate = DateTime.Parse(objReader("ClassStart"))
                        End If

                        If IsDBNull(objReader("ClassStart")) Then
                            EndDate = "" 'DateTime.MaxValue
                        Else
                            EndDate = DateTime.Parse(objReader("ClassEnd"))
                        End If

                        If IsDBNull(objReader("CreditHours")) Then
                            NumCreditHrs = 0.0
                        Else
                            NumCreditHrs = Decimal.Parse(objReader("CreditHours"))
                        End If

                        If IsDBNull(objReader("ClassDuration")) Then
                            ClassDuration = 0
                        Else
                            ClassDuration = Integer.Parse(objReader("ClassDuration"))
                        End If

                    End If
                End Using
            End Using
        End Using
    End Sub

    Public Sub LoadClassInfo(ByVal strclassname As String)
        Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
        ' Read the connection string from the web.config file
        Using objConn As New SqlConnection(connString)
            Dim strSQL = <![CDATA[
                    SELECT ClassID,Classname,ClassStart, ClassEnd, ClassDuration, CreditHours FROM Classes
                    WHERE Classname=@ClassID
                    ]]>.Value()
            Using objCommand1 As New SqlCommand
                With objCommand1
                    .Connection = objConn
                    .Connection.Open()
                    .CommandText = strSQL
                    .Parameters.AddWithValue("@ClassID", strclassname)
                End With
                Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                    If objReader.Read() Then
                        SelectedClass = objReader("ClassID").ToString
                        If IsDBNull(objReader("ClassStart")) Then
                            StartDate = DateTime.MinValue
                        Else
                            StartDate = DateTime.Parse(objReader("ClassStart"))
                        End If

                        If IsDBNull(objReader("ClassStart")) Then
                            EndDate = DateTime.MaxValue
                        Else
                            EndDate = DateTime.Parse(objReader("ClassEnd"))
                        End If

                        If IsDBNull(objReader("CreditHours")) Then
                            NumCreditHrs = 0.0
                        Else
                            NumCreditHrs = Decimal.Parse(objReader("CreditHours"))
                        End If

                        If IsDBNull(objReader("ClassDuration")) Then
                            ClassDuration = 0
                        Else
                            ClassDuration = Integer.Parse(objReader("ClassDuration"))
                        End If

                    End If
                End Using
            End Using
        End Using
    End Sub

    Public Sub LoadCurrentClassInfo()
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            ' Read the connection string from the web.config file
            Using objConn As New SqlConnection(connString)
                Dim strSQL = <![CDATA[
                                SELECT ClassID, Classname FROM Classes WHERE ClassID=@ClassID
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@ClassID", ClassID)
                    End With
                    Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                        If objReader.Read() Then
                            Classname = objReader("Classname").ToString
                            'If IsDBNull(objReader("ClassStart")) Then
                            '    dtStart = DateTime.MinValue
                            'Else
                            '    dtStart = DateTime.Parse(objReader("ClassStart"))
                            'End If

                            'If IsDBNull(objReader("ClassStart")) Then
                            '    dtEnd = DateTime.MaxValue
                            'Else
                            '    dtEnd = DateTime.Parse(objReader("ClassEnd"))
                            'End If

                            'If IsDBNull(objReader("CreditHours")) Then
                            '    NumCreditHrs = 0.0
                            'Else
                            '    NumCreditHrs = Decimal.Parse(objReader("CreditHours"))
                            'End If

                            'If IsDBNull(objReader("ClassDuration")) Then
                            '    ClassDuration = 0
                            'Else
                            '    ClassDuration = Integer.Parse(objReader("ClassDuration"))
                            'End If
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            StartDate = DateTime.MinValue
            EndDate = DateTime.MinValue
            NumCreditHrs = 0
            ClassDuration = 0.0
            Classname = ""
            ClassID = 0
        End Try
    End Sub



End Class
