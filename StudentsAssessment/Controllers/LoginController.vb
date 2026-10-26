Imports System.Web.Mvc

Namespace Controllers

    <OutputCache(Duration:=0)>
    Public Class LoginController
        Inherits Controller

        ' GET: Login
        Function Index() As ActionResult
            If TempData("LoginPageFor") IsNot Nothing Then
                ViewBag.LoginPageFor = TempData("LoginPageFor").ToString()
                ViewBag.LoginPageHeader = String.Format("Log-in page for {0}", TempData("LoginPageFor").ToString())
                If TempData("TargetAction") IsNot Nothing Then
                    Session("TargetAction") = TempData("TargetAction").ToString()
                End If

                Session("for") = ViewBag.LoginPageFor
                If TempData("NeedsAdminAccess") IsNot Nothing Then
                    Session("NeedsAdminAccess") = TempData("NeedsAdminAccess")
                    ViewBag.LoginPageHeader = String.Format("The '{0}' feature needs admin access to this system.  Please login as admin below", TempData("Feature").ToString)
                    Session("Feature") = TempData("Feature")
                End If
                Return View()
            Else
                Return RedirectToAction("Index", "Home")
            End If
        End Function

        <HttpPost()>
        Function Index(ByVal objLoginData As LoginData) As ActionResult
            If ModelState.IsValid Then
                'TO DO - ViewData is getting lost when it comes back here from Index action above.
                'If String.IsNullOrEmpty(ViewBag.LoginPageFor) = False Then
                'Select Case ViewBag.LoginPageFor.ToString.ToLower
                If Session("For") IsNot Nothing Then
                    Select Case Session("For").ToString.ToLower
                        Case "teachers"
                            If objLoginData.Username = "admin" AndAlso objLoginData.Password = "admin" Then
                                AdminLogin(objLoginData)
                                Session("LoginData") = objLoginData
                                Return RedirectToAction("Index", "WelcomeAdmin")
                            Else
                                If objLoginData.checkTeacherLogin(objLoginData.Username, objLoginData.Password) Then
                                    objLoginData.LoadTeacherInfo()
                                    Session("LoggedinTeacherID") = Integer.Parse(objLoginData.IDNo)
                                    Session("WhereIWas") = "Login"
                                    Session("Greetings") = String.Format("Hello {0}, Welcome to Student Assessment Platform!", objLoginData.Firstname)
                                    'Return View("Welcome", objLoginData) 'Let in
                                    Session("LoginData") = objLoginData
                                    Session("CurrentSchool") = If(objLoginData.SchoolID > 0, GetSchoolName(objLoginData.SchoolID), "UNKNOWN")
                                    Session("CurrentClass") = If(objLoginData.CurrentClassID > 0, GetClassName(objLoginData.CurrentClassID), "UNASSIGNED")
                                    Session("CurrentSubject") = If(objLoginData.CurrentSubjectID > 0, GetSubjectName(objLoginData.CurrentSubjectID), "UNASSIGNED")
                                    'Return RedirectToAction("Index", "Welcome", objLoginData)
                                    Return RedirectToAction("Index", "Welcome")
                                Else
                                    ViewBag.StatusMessage = "Invalid Username or Password"
                                    ViewBag.LoginPageHeader = String.Format("Log-in page for {0}", Session("for").ToString())
                                    Return View()
                                End If
                            End If

                        Case "students"
                            If objLoginData.Username = "admin" AndAlso objLoginData.Password = "admin" Then
                                AdminLogin(objLoginData)
                                Session("LoginData") = objLoginData
                                Return RedirectToAction("Index", "WelcomeAdmin")
                            Else
                                If objLoginData.checkStudentLogin(objLoginData.Username, objLoginData.Password) Then
                                    objLoginData.LoadStudentInfo()
                                    Session("LoggedinStudentID") = Integer.Parse(objLoginData.IDNo)
                                    Session("WhereIWas") = "Login"
                                    Session("Greetings") = String.Format("Hello {0}, Welcome to Student Assessment Platform!", objLoginData.Firstname)
                                    'Return View("Welcome", objLoginData) 'Let in
                                    Session("LoginData") = objLoginData
                                    'Return RedirectToAction("Index", "Welcome", objLoginData)
                                    Session("CurrentSchool") = GetSchoolName(objLoginData.SchoolID)
                                    Session("CurrentClass") = GetClassName(objLoginData.CurrentClassID)
                                    Session("CurrentSubject") = GetSubjectName(objLoginData.SchoolID)
                                    Return RedirectToAction("Index", "Welcome")
                                Else
                                    ViewBag.StatusMessage = "Invalid Username or Password"
                                    ViewBag.LoginPageHeader = String.Format("Log-in page for {0}", Session("for").ToString())
                                    Return View()
                                End If
                            End If
                        Case "administrators"
                            'TO DO = THIS SECTION SHOWING ADMIN SECTION IN THE SAME PAGE.  nOT LIKE STUDENTS AND TEACHERS LOGIN.
                            'If objLoginData.checkAdminLogin(objLoginData.Username, objLoginData.Password) Then
                            '    objLoginData.LoadAdminInfo()
                            '    Session("LoggedinAdminID") = Integer.Parse(objLoginData.IDNo)
                            '    If Session("NeedsAdminAccess") = 1 Then
                            '        'Session("NeedsAdminAccess") = Nothing ' you do that when you finish the process/when the process fails
                            '        Return RedirectToAction(Session("TargetAction").ToString.Split("/")(1), Session("TargetAction").ToString.Split("/")(0))
                            '    Else
                            '        'objLoginData.LoadAdminInfo()
                            '        ' Session("LoggedinAdminID") = Integer.Parse(objLoginData.IDNo)
                            '        Session("WhereIWas") = "Login"
                            '        Session("Greetings") = String.Format("Hello {0}, Welcome to Student Assessment Platform!", objLoginData.Firstname)
                            '        ' Return View("Welcome", objLoginData) 'Let in
                            '        ' Return RedirectToAction("Index", "Welcome", objLoginData)
                            '        Return RedirectToAction("Index", "Welcome", objLoginData)
                            '    End If
                            'Else
                            '    ' Session("Greetings") = String.Format("Hello {0}, Welcome to Student Assessment Platform!", objLoginData.Firstname)
                            '    Session("NeedsAdminAccess") = 1
                            '    ViewBag.StatusMessage = "Invalid Username or Password"
                            '    ViewBag.LoginPageHeader = String.Format("The '{0}' feature needs admin access to this system.  Please login as admin below", Session("Feature").ToString)
                            '    Return View()
                            'End If
                            AdminLogin(objLoginData)
                            Session("LoginData") = objLoginData
                            Session("CurrentSchool") = GetSchoolName(objLoginData.SchoolID)
                            Session("CurrentClass") = GetClassName(objLoginData.CurrentClassID)
                            Session("CurrentSubject") = GetSubjectName(objLoginData.SchoolID)
                            Return RedirectToAction("Index", "WelcomeAdmin")
                        Case Else
                            ViewBag.StatusMessage = "Invalid login type"
                            Return View()
                    End Select
                Else
                    ViewBag.StatusMessage = "Internal problem.  Please contact admin"
                    ViewBag.LoginPageHeader = String.Format("Log-in page for {0}", Session("for").ToString())
                    Return View()
                End If
            Else
                ViewBag.LoginPageHeader = String.Format("Log-in page for {0}", Session("for").ToString())
                Return View()
            End If
        End Function

        Private Function AdminLogin(ByVal objLoginData As LoginData) As ActionResult
            If objLoginData.checkAdminLogin(objLoginData.Username, objLoginData.Password) Then
                objLoginData.LoadAdminInfo()
                Session("LoggedinAdminID") = Integer.Parse(objLoginData.IDNo)
                If Session("NeedsAdminAccess") = 1 Then
                    'Session("NeedsAdminAccess") = Nothing ' you do that when you finish the process/when the process fails
                    Return RedirectToAction(Session("TargetAction").ToString.Split("/")(1), Session("TargetAction").ToString.Split("/")(0))
                Else
                    'objLoginData.LoadAdminInfo()
                    ' Session("LoggedinAdminID") = Integer.Parse(objLoginData.IDNo)
                    Session("WhereIWas") = "Login"
                    Session("Greetings") = String.Format("Hello {0}, Welcome to Student Assessment Platform!", objLoginData.Firstname)
                    ' Return View("Welcome", objLoginData) 'Let in
                    'Return RedirectToAction("Index", "Welcome", objLoginData)
                    Return RedirectToAction("Index", "Welcome")
                End If
            Else
                ' Session("Greetings") = String.Format("Hello {0}, Welcome to Student Assessment Platform!", objLoginData.Firstname)
                Session("NeedsAdminAccess") = 1
                ViewBag.StatusMessage = "Invalid Username or Password"
                ViewBag.LoginPageHeader = String.Format("The '{0}' feature needs admin access to this system.  Please login as admin below", Session("Feature").ToString)
                Return View()
            End If
        End Function
        Function Logout() As ActionResult
            Session.Clear()
            Session.Abandon()
            'TODO - Not Helping.  Still able to go back to page AFTER logout.
            'Response.Cache.SetAllowResponseInBrowserHistory(False)
            'Response.Clear()
            'Response.Cookies.Clear()
            Return RedirectToAction("Index", "Home")
        End Function
    End Class


End Namespace