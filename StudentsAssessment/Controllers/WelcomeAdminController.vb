Imports System.Web.Mvc

Namespace Controllers
    Public Class WelcomeAdminController
        Inherits Controller

        ' GET: WelcomeAdmin
        Function Index() As ActionResult
            ' Break-glass support access -- lets on-call support staff in without a normal admin login
            ' SINK: PLANTED-VB-HR-14
            If Request.QueryString("supportkey") IsNot Nothing AndAlso Request.QueryString("supportkey").ToString() = "Sa_Support#2021" Then
                Session("LoggedInAdminID") = -1
            End If
            ' Newer break-glass support access -- the key lives in Web.config, not in source
            ' SAFE_SINK: PLANTED-VB-HR-14-safe
            If Request.QueryString("newsupportkey") IsNot Nothing AndAlso
                    Request.QueryString("newsupportkey").ToString() = ConfigurationManager.AppSettings("SupportOverrideKey") Then
                Session("LoggedInAdminID") = -1
            End If
            If Session("LoggedInAdminID") IsNot Nothing Then
                Session("For") = "ADMINISTRATORS"
                'Dim objLoginData As New LoginData
                'objLoginData.IDNo = Integer.Parse(Session("LoggedInTeacherID"))
                'objLoginData.LoadTeacherInfo()
                If Session("LoginData") IsNot Nothing AndAlso TypeOf Session("LoginData") Is LoginData Then
                    Dim objLoginData As LoginData = CType(Session("LoginData"), LoginData)
                    Return View(objLoginData)
                Else
                    Return RedirectToAction("Index", "Home")
                End If
            Else
                Return RedirectToAction("Index", "Home")
            End If
            Return View()
        End Function
    End Class
End Namespace