Imports System.ComponentModel.DataAnnotations
Imports System.Data.SqlClient

Public Class LoginData
    <Required(ErrorMessage:="Username is required")>
    <Display(Name:="Username")>
    Public Property Username As String

    <Required(ErrorMessage:="Password is required")>
    <Display(Name:="Password")>
    Public Property Password As String

    Public Property Firstname As String

    Public Property Lastname As String

    Public Property IDNo As Integer

    Public Property Status As String

    Public Property CurrentClassID As Integer
    Public Property CurrentSubjectID As Integer
    Public Property SchoolID As Integer

    Public Function checkTeacherLogin(ByVal strUsername As String, strPassword As String) As Boolean
        Dim objFlag As Boolean = False
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            Using objConn As New SqlConnection(connString)
                Dim strSQL As String = <![CDATA[
                SELECT COUNT(*) FROM Teachers WHERE Username=@Username AND Password=@Password
    ]]>.Value()
                Using objCommand As New SqlCommand
                    With objCommand
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@Username", strUsername)
                        .Parameters.AddWithValue("@Password", strPassword)
                    End With
                    objFlag = Convert.ToBoolean(objCommand.ExecuteScalar())
                End Using
            End Using
        Catch ex As Exception

        End Try
        Return objFlag

    End Function

    Public Function checkAdminLogin(strUsername As String, strPassword As String) As Boolean
        Dim objFlag As Boolean = False
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            Using objConn As New SqlConnection(connString)
                Dim strSQL As String = <![CDATA[
                SELECT COUNT(*) FROM Admins WHERE Username=@Username AND Password=@Password
    ]]>.Value()
                Using objCommand As New SqlCommand
                    With objCommand
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@Username", strUsername)
                        .Parameters.AddWithValue("@Password", strPassword)
                    End With
                    objFlag = Convert.ToBoolean(objCommand.ExecuteScalar())
                End Using
            End Using
        Catch ex As Exception

        End Try
        Return objFlag
    End Function

    Public Function checkStudentLogin(ByVal strUsername As String, strPassword As String) As Boolean
        Dim objFlag As Boolean = False
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            Using objConn As New SqlConnection(connString)
                Dim strSQL As String = <![CDATA[
                SELECT COUNT(*) FROM Students WHERE Username=@Username AND Password=@Password
    ]]>.Value()
                Using objCommand As New SqlCommand
                    With objCommand
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@Username", strUsername)
                        .Parameters.AddWithValue("@Password", strPassword)
                    End With
                    objFlag = Convert.ToBoolean(objCommand.ExecuteScalar())
                End Using
            End Using
        Catch ex As Exception

        End Try
        Return objFlag

    End Function

    Public Sub LoadTeacherInfo()
        If String.IsNullOrEmpty(Username) = False AndAlso String.IsNullOrEmpty(Password) = False Then
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            ' Read the connection string from the web.config file
            Using objConn As New SqlConnection(connString)
                Dim strSQL = <![CDATA[
                                SELECT TeacherID,Firstname, Lastname, CurrentClassID, CurrentSubjectID, SchoolID FROM Teachers WHERE Username=@Username AND Password=@Password
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@Username", Username)
                        .Parameters.AddWithValue("@Password", Password)
                    End With
                    Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                        If objReader.Read() Then
                            Firstname = objReader("Firstname").ToString
                            Lastname = objReader("Lastname").ToString
                            IDNo = Integer.Parse(objReader("TeacherID"))
                            If IsDBNull(objReader("CurrentClassID")) Then
                                CurrentClassID = 0
                            Else
                                CurrentClassID = Integer.Parse(objReader("CurrentClassID"))
                            End If

                            If IsDBNull(objReader("CurrentSubjectID")) Then
                                CurrentSubjectID = 0
                            Else
                                CurrentSubjectID = Integer.Parse(objReader("CurrentSubjectID"))
                            End If

                            If IsDBNull(objReader("SchoolID")) Then
                                SchoolID = 0
                            Else
                                SchoolID = Integer.Parse(objReader("SchoolID"))
                            End If
                            Status = "TEACHER"
                        Else
                            Status = "ERROR"
                        End If
                    End Using
                End Using
            End Using
        End If
    End Sub

    Public Sub LoadStudentInfo()
        If String.IsNullOrEmpty(Username) = False AndAlso String.IsNullOrEmpty(Password) = False Then
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            ' Read the connection string from the web.config file
            Using objConn As New SqlConnection(connString)
                Dim strSQL = <![CDATA[
                                SELECT StudentID,Firstname, Lastname FROM Students WHERE Username=@Username AND Password=@Password
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@Username", Username)
                        .Parameters.AddWithValue("@Password", Password)
                    End With
                    Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                        If objReader.Read() Then
                            Firstname = objReader("Firstname").ToString
                            Lastname = objReader("Lastname").ToString
                            IDNo = Integer.Parse(objReader("StudentID"))
                            CurrentClassID = 0
                            CurrentSubjectID = 0
                            SchoolID = 0
                            Status = "STUDENT"
                        Else
                            Status = "ERROR"
                        End If
                    End Using
                End Using
            End Using
        End If
    End Sub


    Public Sub LoadAdminInfo()
        If String.IsNullOrEmpty(Username) = False AndAlso String.IsNullOrEmpty(Password) = False Then
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            ' Read the connection string from the web.config file
            Using objConn As New SqlConnection(connString)
                Dim strSQL = <![CDATA[
                                SELECT AdminID,Firstname, Lastname FROM Admins WHERE Username=@Username AND Password=@Password
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@Username", Username)
                        .Parameters.AddWithValue("@Password", Password)
                    End With
                    Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                        If objReader.Read() Then
                            Firstname = objReader("Firstname").ToString
                            Lastname = objReader("Lastname").ToString
                            IDNo = Integer.Parse(objReader("AdminID"))
                            CurrentClassID = 0
                            CurrentSubjectID = 0
                            SchoolID = 0
                            Status = "ADMINISTRATOR"
                        Else
                            Status = "ERROR"
                        End If
                    End Using
                End Using
            End Using
        End If
    End Sub
End Class
