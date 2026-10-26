Imports System.Data.SqlClient

Public Class TeacherData
    Public Sub New()
        TeachersList = New List(Of SelectListItem)
    End Sub

    Public Property Firstname() As String

    Public Property Lastname() As String

    Public Property Joindate() As DateTime

    Public Property TeacherID As Integer

    Public Property TeachersList As List(Of SelectListItem)

    Public Property SelectedTeacher As String 'This MUST BE a property (not just a string) for a value to come out

    Public Sub LoadTeacherInfo()
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            ' Read the connection string from the web.config file
            Using objConn As New SqlConnection(connString)
                Dim strSQL = <![CDATA[
                                SELECT TeacherID, Firstname,Lastname FROM Teachers WHERE TeacherID=@TeacherID
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@TeacherID", TeacherID)
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

    Public Sub LoadTeachersList()
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            ' Read the connection string from the web.config file
            Using objConn As New SqlConnection(connString)
                Dim strSQL = <![CDATA[
                                SELECT TeacherID, Firstname+' '+Lastname AS Teachername FROM Teachers
                    ]]>.Value()
                Using objCommand1 As New SqlCommand
                    With objCommand1
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                    End With
                    Using objReader As SqlDataReader = objCommand1.ExecuteReader()
                        While objReader.Read()
                            TeachersList.Add(New SelectListItem With {.Text = objReader("Teachername").ToString, .Value = objReader("TeacherID").ToString})
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception

        End Try

    End Sub

    Public Function GetTeacherDT() As DataTable
        Dim objDS As New DataSet
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            Using objConn As New SqlConnection(connString)
                'All teacher (1st table) records will show. NULL schools will show as blank
                Dim strSQL = <![CDATA[
                                SELECT DISTINCT TeacherID, Firstname+' '+Lastname AS Teachername,Schoolname FROM Teachers LEFT JOIN Schools ON
                                Teachers.SchoolID=Teachers.SchoolID 
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

    Public Function RemoveTeacherFromSystem(ByRef strError As String) As Boolean
        Dim bDeleted As Boolean = False
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("myConnectionString").ConnectionString
            Using objConn As New SqlConnection(connString)
                Dim strSQL As String = ""
                Using objCommand2 As New SqlCommand
                    strSQL = <![CDATA[
                   DELETE FROM Teachers WHERE TeacherID=@TeacherID
                    ]]>.Value()
                    With objCommand2
                        .Connection = objConn
                        .Connection.Open()
                        .CommandText = strSQL
                        .Parameters.AddWithValue("@TeacherID", Integer.Parse(SelectedTeacher))
                    End With
                    If objCommand2.ExecuteNonQuery Then
                        bDeleted = True
                    Else
                        bDeleted = False
                        strError = "The teacher seems to have been removed already"
                    End If
                End Using
            End Using
        Catch ex As Exception
            bDeleted = False
        End Try
        Return bDeleted
    End Function
End Class
