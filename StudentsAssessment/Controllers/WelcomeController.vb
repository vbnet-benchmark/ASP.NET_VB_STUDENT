Imports System.Web.Mvc

Namespace Controllers
    Public Class WelcomeController
        Inherits Controller

        ' GET: Welcome
        Function Index() As ActionResult

            If Session("LoggedInTeacherID") IsNot Nothing Then
                Session("LoggedInStudentID") = Nothing
                Session("LoggedInAdminID") = Nothing
            ElseIf Session("LoggedInStudentID") IsNot Nothing Then
                Session("LoggedInTeacherID") = Nothing
                Session("LoggedInAdminID") = Nothing
            ElseIf Session("LoggedInAdminID") IsNot Nothing Then
                Session("LoggedInTeacherID") = Nothing
                Session("LoggedInStudentID") = Nothing
                Session("For") = "ADMINISTRATORS"
            Else
                'inValid login account type thus far
                Return RedirectToAction("Index", "Home")
            End If

            If Session("LoginData") IsNot Nothing AndAlso TypeOf Session("LoginData") Is LoginData Then
                Dim objLoginData As LoginData = CType(Session("LoginData"), LoginData)
                Return View(objLoginData)
            Else
                Return RedirectToAction("Index", "Home")
            End If
            'OLD CODE
            'If Session("LoggedInTeacherID") IsNot Nothing OrElse Session("LoggedInStudentID") IsNot Nothing OrElse
            '     Session("LoggedInAdminID") IsNot Nothing Then
            '    If Session("LoggedInAdminID") IsNot Nothing Then
            '        Session("For") = "ADMINISTRATORS"
            '    End If
            '    'Dim objLoginData As New LoginData
            '    'objLoginData.IDNo = Integer.Parse(Session("LoggedInTeacherID"))
            '    'objLoginData.LoadTeacherInfo()
            '    If Session("LoginData") IsNot Nothing AndAlso TypeOf Session("LoginData") Is LoginData Then
            '        Dim objLoginData As LoginData = CType(Session("LoginData"), LoginData)
            '        Return View(objLoginData)
            '    Else
            '        Return RedirectToAction("Index", "Home")
            '    End If
            'Else
            '    Return RedirectToAction("Index", "Home")
            'End If
        End Function
    End Class
End Namespace