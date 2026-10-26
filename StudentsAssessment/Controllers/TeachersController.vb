Imports System.Web.Mvc

Namespace Controllers
    <HandleError()>
    Public Class TeachersController
        Inherits Controller

        ' GET: TeacherOps
        <HandleError()>
        Function Index() As ActionResult
            Return View()
        End Function

#Region "CLASSES - Adding, Removing and Viewing"
        <HandleError()>
        Function AddClass() As ActionResult
            If Session("LoggedinteacherID") IsNot Nothing AndAlso IsNumeric(Session("LoggedinteacherID")) = True Then
                Dim objClassData As New ClassData
                objClassData.LoadClassesList()
                Return View(objClassData)
            Else
                Return RedirectToAction("Index", "Home")
            End If

        End Function

        <HttpPost()>
        Function AddClass(ByVal objClassData As ClassData) As ActionResult
            If Session("LoggedinteacherID") IsNot Nothing AndAlso IsNumeric(Session("LoggedinteacherID")) = True Then
                objClassData.LoadClassesList() 'Should fill each time like this
                'If ModelState.IsValid Then
                If IsNumeric(objClassData.NumCreditHrs) = True Then
                    Dim strSelClass As String = objClassData.SelectedClass
                    ' Session("LoggedInTeacherID") = 1
                    Dim strerror As String = ""
                    If objClassData.CreateClass(strerror) Then
                        ViewBag.StatusMessage = String.Format("Successfully ADDED '{0}' CLASS to {1}'s list of CLASSES", GetClassname(strSelClass), GetTeachername)
                    Else
                        ViewBag.StatusMessage = strerror
                    End If
                Else
                    ViewBag.StatusMessage = "Number of credit hours must be numeric"
                End If

                'Else

                'End If
                Return View(objClassData)
            Else
                Return RedirectToAction("Index", "Home")
            End If
        End Function

        Function RemoveClass() As ActionResult
            If Session("LoggedinteacherID") IsNot Nothing AndAlso IsNumeric(Session("LoggedinteacherID")) = True Then
                Dim objClassData As New ClassData
                objClassData.LoadTaughtClassesList()
                Return View(objClassData)
            Else
                Return RedirectToAction("Index", "Home")
            End If
        End Function

        <HttpPost()>
        Function RemoveClass(ByVal objClassData As ClassData) As ActionResult
            If Session("LoggedinteacherID") IsNot Nothing AndAlso IsNumeric(Session("LoggedinteacherID")) = True Then
                'Dim objClassData1 As New ClassData
                objClassData.LoadTaughtClassesList() 'Should fill each time like this
                'If ModelState.IsValid Then
                If IsNumeric(objClassData.NumCreditHrs) = True Then
                        Dim strSelClass As String = objClassData.SelectedClass
                        ' Session("LoggedInTeacherID") = 1
                        Dim strerror As String = ""
                        If objClassData.RemoveClass(strerror) Then
                            ViewBag.StatusMessage = String.Format("Successfully REMOVED '{0}' CLASS FROM {1}'s list of CLASSES", GetClassname(strSelClass), GetTeachername)
                        Else
                            ViewBag.StatusMessage = strerror
                        End If
                    Else
                        ViewBag.StatusMessage = "Number of credit hours must be numeric"
                    End If

                Else

                End If
            Return View(objClassData)
            'Else
            Return RedirectToAction("Index", "Home")
            'End If
        End Function

        'TODO - ModelState.isValid is not reliably working yet.  It will be fixed in the next version.  For now,
        'write the "CheckXData" functions for Class, Subject and Students.  Do same for the "Admin" sections as well.
        Private Function CheckClassData() As Boolean
            Dim bOK As Boolean = True 'for now, let it be
            Return bOK
        End Function
        Function ViewClasses() As ActionResult
            If Session("LoggedinteacherID") IsNot Nothing AndAlso IsNumeric(Session("LoggedinteacherID")) = True Then
                Dim objDT As DataTable
                Dim objClassData As New ClassData
                objDT = objClassData.GetTaughtClassesInfoDT()
                If objDT.Rows.Count = 0 Then
                    ViewBag.Error = String.Format("{0}'s list of classes is currently empty", GetTeachername)
                Else
                    ViewBag.Data = objDT
                End If
                Return View(objClassData)
            Else
                Return RedirectToAction("Index", "Home")
            End If

        End Function


#End Region

        Public Function GetClassData(ByVal iClassID As Integer) As ActionResult
            Dim objClassData As New ClassData
            objClassData.LoadClassInfo(iClassID)
            'Return Json(String.Format("Returning from FirstAJAX action {0}", param), JsonRequestBehavior.AllowGet)
            Return Json(New With {.NumCreditHrs = objClassData.NumCreditHrs,
                        .ClassDuration = objClassData.ClassDuration,
                        .CreditHours = objClassData.NumCreditHrs,
                        .StartDate = objClassData.StartDate,
                        .EndDate = objClassData.EndDate}, JsonRequestBehavior.AllowGet)
        End Function

#Region "SUBJECTS - Adding, Removing and Viewing"
        Function AddSubjecttoClass() As ActionResult
            If Session("LoggedinteacherID") IsNot Nothing AndAlso IsNumeric(Session("LoggedinteacherID")) = True Then
                Dim objSubjectsData As New SubjectsData
                '  objSubjectsData.LoadSubjectsList()
                objSubjectsData.LoadTaughtClassesList()
                'objSubjectsData.LoadSubjectsList()
                Return View(objSubjectsData)
            Else
                Return RedirectToAction("Index", "Home")
            End If

        End Function


        <HttpPost()>
        Function AddSubjecttoClass(ByVal objSubjectsData As SubjectsData) As ActionResult
            If Session("LoggedinteacherID") IsNot Nothing AndAlso IsNumeric(Session("LoggedinteacherID")) = True Then
                Dim objSubjectsData1 As New SubjectsData
                objSubjectsData1.LoadTaughtClassesList() 'Should fill each time like this
                objSubjectsData1.LoadSubjectsForClass(objSubjectsData.SelectedClass)
                ' If ModelState.IsValid Then
                Dim strSelSubject As String = objSubjectsData.SelectedSubject
                    Dim strSelClass As String = objSubjectsData.SelectedClass
                    Dim strerror As String = ""
                If objSubjectsData.AddSubjecttoClass(strerror) Then
                    ViewBag.StatusMessage = String.Format("'{0}' will be taught in CLASS by {2} from now on", GetSubjectname(strSelSubject), GetClassname(strSelClass), GetTeachername)
                Else
                    ViewBag.StatusMessage = strerror
                End If
                'Else

                'End If
                Return View(objSubjectsData1)
            Else
                Return RedirectToAction("Index", "Home")
            End If
        End Function

        Function RemoveSubjectFromClass() As ActionResult
            If Session("LoggedinteacherID") IsNot Nothing AndAlso IsNumeric(Session("LoggedinteacherID")) = True Then
                Dim objSubjectsData As New SubjectsData
                objSubjectsData.LoadClassesSubjectsTaughtList()
                Return View(objSubjectsData)
            Else
                Return RedirectToAction("Index", "Home")
            End If
        End Function

        <HttpPost()>
        Function RemoveSubjectFromClass(ByVal objSubjectsData As SubjectsData) As ActionResult
            If Session("LoggedinteacherID") IsNot Nothing AndAlso IsNumeric(Session("LoggedinteacherID")) = True Then
                'Dim objSubjectsData1 As New SubjectsData
                objSubjectsData.LoadClassesSubjectsTaughtList() 'Should fill each time like this
                'If ModelState.IsValid Then
                'If IsNumeric(objSubjectsData.NumCreditHrs) = True Then
                'Dim strSelClass As String = objSubjectsData.SelectedClass
                ' Session("LoggedInTeacherID") = 1
                Dim strerror As String = ""
                    'TO DO - Put this protection in other places also.
                    If (objSubjectsData.SelectedClass IsNot Nothing AndAlso IsNumeric(objSubjectsData.SelectedClass) = True AndAlso
                            Integer.Parse(objSubjectsData.SelectedClass) > 0) AndAlso
                            (objSubjectsData.SelectedSubject IsNot Nothing AndAlso IsNumeric(objSubjectsData.SelectedSubject) = True AndAlso
                        Integer.Parse(objSubjectsData.SelectedSubject) > 0) Then
                        Dim strSelSubject As String = objSubjectsData.SelectedSubject
                        Dim strSelClass As String = objSubjectsData.SelectedClass
                    objSubjectsData.SelectedClass = strSelClass
                    objSubjectsData.SelectedSubject = strSelSubject
                    If objSubjectsData.RemoveSubjectFromClass(strerror) Then
                        ViewBag.StatusMessage = String.Format("'{0}' will NO LONGER be taught in CLASS by {2} from now on", GetSubjectname(strSelSubject), GetClassname(strSelClass), GetTeachername)
                    Else
                        ViewBag.StatusMessage = strerror
                        End If
                        'Else
                        '    ViewBag.StatusMessage = "Number of credit hours must be numeric"
                        'End If
                    Else
                        ViewBag.StatusMessage = "BOTH Class AND Subject MUST be selected"
                    End If
                'Else

                'End If
                Return View(objSubjectsData)
            Else
                Return RedirectToAction("Index", "Home")
            End If
        End Function

        Function ViewSubjects() As ActionResult
            If Session("LoggedinteacherID") IsNot Nothing AndAlso IsNumeric(Session("LoggedinteacherID")) = True Then
                Dim objDT As DataTable
                Dim objSubjectsData As New SubjectsData
                objDT = objSubjectsData.GetTaughtSubjectsInfoDT()
                If objDT.Rows.Count = 0 Then
                    ViewBag.Error = String.Format("{0} HAS NOT added any SUBJECTS to his/her CLASSES yet", GetTeachername)
                Else
                    ViewBag.Data = objDT
                End If
                Return View(objSubjectsData)
            Else
                Return RedirectToAction("Index", "Home")
            End If

        End Function
#End Region

        Public Function GetSubjectsData(ByVal iClassID As Integer) As ActionResult
            Dim objSubjectData As New SubjectsData
            objSubjectData.LoadTaughtClassesList()
            objSubjectData.LoadSubjectsForClass(iClassID)
            Return Json(objSubjectData.SubjectsList, JsonRequestBehavior.AllowGet)
            'Return View(objSubjectData)
        End Function

        Public Function GetTaughtSubjectsData(ByVal iClassID As Integer) As ActionResult
            Dim objSubjectData As New SubjectsData
            objSubjectData.LoadTaughtClassesList()
            objSubjectData.LoadTaughtSubjectsForClass(iClassID)
            Return Json(objSubjectData.SubjectsList, JsonRequestBehavior.AllowGet)
            'Return View(objSubjectData)
        End Function

        Public Function GetSubjectData(ByVal iSubjectID As Integer) As ActionResult
            Dim objSubjectsData As New SubjectsData
            objSubjectsData.LoadSubjectInfo(iSubjectID)
            'Return Json(String.Format("Returning from FirstAJAX action {0}", param), JsonRequestBehavior.AllowGet)
            Return Json(New With {.GradeLevel = objSubjectsData.GradeLevel}, JsonRequestBehavior.AllowGet)
        End Function



#Region "STUDENTS - Adding, Removing and Viewing"
        Function AddStudenttoClass() As ActionResult
            If Session("LoggedinteacherID") IsNot Nothing AndAlso IsNumeric(Session("LoggedinteacherID")) = True Then
                Dim objStudentData As New StudentData
                '  objSubjectsData.LoadSubjectsList()
                objStudentData.LoadTaughtClassesList()
                'objSubjectsData.LoadSubjectsList()
                Return View(objStudentData)
            Else
                Return RedirectToAction("Index", "Home")
            End If

        End Function


        <HttpPost()>
        Function AddStudenttoClass(ByVal objStudentData As StudentData) As ActionResult
            If Session("LoggedinteacherID") IsNot Nothing AndAlso IsNumeric(Session("LoggedinteacherID")) = True Then
                'Dim objStudentData1 As New StudentData
                objStudentData.LoadTaughtClassesList() 'Should fill each time like this
                objStudentData.LoadStudentsForClass(objStudentData.SelectedClass)
                objStudentData.SelectedClass = objStudentData.SelectedClass
                objStudentData.SelectedStudent = objStudentData.SelectedStudent
                If objStudentData.SelectedStudent > 0 AndAlso objStudentData.SelectedClass > 0 Then
                    'If ModelState.IsValid Then
                    'If IsNumeric(objSubjectsData.NumCreditHr) = True Then
                    'Dim strSelClass As String = objSubjectsData.SelectedClass
                    'Dim strSelClass As String = objSubjectsData.SelectedClass
                    ' Session("LoggedInTeacherID") = 1
                    Dim strerror As String = ""
                        If objStudentData.AddStudenttoClass(strerror) Then
                        ViewBag.StatusMessage = String.Format("The STUDENT '{0}' HAS BEEN ADDED TO {1}'s '{2}' CLASS", GetStudentname(objStudentData.SelectedStudent), GetTeachername, GetClassname(objStudentData.SelectedClass))
                    Else
                            ViewBag.StatusMessage = strerror
                        End If
                    End If
                    Return View(objStudentData)
            Else
                    Return RedirectToAction("Index", "Home")
            End If
        End Function

        Function RemoveStudentFromClass() As ActionResult
            If Session("LoggedinteacherID") IsNot Nothing AndAlso IsNumeric(Session("LoggedinteacherID")) = True Then
                Dim objStudentData As New StudentData
                objStudentData.LoadTaughtClassesList()
                Return View(objStudentData)
            Else
                Return RedirectToAction("Index", "Home")
            End If
        End Function

        <HttpPost()>
        Function RemoveStudentFromClass(ByVal objStudentData As StudentData) As ActionResult
            If Session("LoggedinteacherID") IsNot Nothing AndAlso IsNumeric(Session("LoggedinteacherID")) = True Then
                Dim objStudentData1 As New StudentData
                objStudentData.LoadTaughtClassesList() 'Should fill each time like this
                'If ModelState.IsValid Then
                'If IsNumeric(objSubjectsData.NumCreditHrs) = True Then
                'Dim strSelClass As String = objSubjectsData.SelectedClass
                ' Session("LoggedInTeacherID") = 1
                Dim strerror As String = ""
                    'TO DO - Put this protection in other places also.
                    If (objStudentData.SelectedClass IsNot Nothing AndAlso IsNumeric(objStudentData.SelectedClass) = True AndAlso
                            Integer.Parse(objStudentData.SelectedClass) > 0) AndAlso
                            (objStudentData.SelectedStudent IsNot Nothing AndAlso IsNumeric(objStudentData.SelectedStudent) = True AndAlso
                        Integer.Parse(objStudentData.SelectedStudent) > 0) Then
                        objStudentData1.SelectedClass = objStudentData.SelectedClass
                        objStudentData1.SelectedStudent = objStudentData.SelectedStudent
                        If objStudentData1.RemoveStudentFromClass(strerror) Then
                            ViewBag.StatusMessage = String.Format("The STUDENT '{0}' is NO LONGER IN {1}'s '{2}' CLASS", GetStudentname(objStudentData1.SelectedStudent), GetTeachername, GetClassname(objStudentData1.SelectedClass))
                        Else
                            ViewBag.StatusMessage = strerror
                        End If
                        'Else
                        '    ViewBag.StatusMessage = "Number of credit hours must be numeric"
                        'End If
                    Else
                        ViewBag.StatusMessage = "BOTH Class AND student MUST be selected"
                    End If
                'Else

                'End If
                Return View(objStudentData)
            Else
                Return RedirectToAction("Index", "Home")
            End If
        End Function

        Function ViewStudents() As ActionResult
            If Session("LoggedinteacherID") IsNot Nothing AndAlso IsNumeric(Session("LoggedinteacherID")) = True Then
                Dim objDT As DataTable
                Dim objSubjectsData As New StudentData
                objDT = objSubjectsData.GetStudentsAlreadyinClassInfoDT()
                If objDT.Rows.Count = 0 Then
                    '  ViewBag.Error = "The teacher's list of classes is currently empty"
                Else
                    ViewBag.Data = objDT
                End If
                Return View(objSubjectsData)
            Else
                Return RedirectToAction("Index", "Home")
            End If

        End Function
#End Region

        Public Function GetStudentsData(ByVal iClassID As Integer) As ActionResult
            Dim objStudentData As New StudentData
            objStudentData.LoadTaughtClassesList()
            objStudentData.LoadStudentsList()
            Return Json(objStudentData.StudentsList, JsonRequestBehavior.AllowGet)
            'Return View(objSubjectData)
        End Function

        Public Function GetStudentsAlreadyinClassData(ByVal iClassID As Integer) As ActionResult
            Dim objStudentData As New StudentData
            objStudentData.LoadTaughtClassesList()
            objStudentData.LoadStudentsAlreadyinClass(iClassID)
            Return Json(objStudentData.StudentsList, JsonRequestBehavior.AllowGet)
            'Return View(objSubjectData)
        End Function

        Public Function GetstudentData(ByVal iStudentID As Integer) As ActionResult
            Dim objStudentData As New StudentData
            objStudentData.LoadStudentInfo(iStudentID)
            'Return Json(String.Format("Returning from FirstAJAX action {0}", param), JsonRequestBehavior.AllowGet)
            Return Json(New With {.Joindate = objStudentData.Joindate.ToShortDateString}, JsonRequestBehavior.AllowGet)
        End Function

        Private Function GetTeachername() As String
            Dim objTeacherData As New TeacherData With {.TeacherID = Session("LoggedinTeacherID")}
            objTeacherData.LoadTeacherInfo()
            Return objTeacherData.Firstname + " " + objTeacherData.Lastname
        End Function

        Private Function GetClassname(ByVal iClassID As Integer) As String
            Dim objClassData As New ClassData With {.ClassID = iClassID}
            objClassData.LoadCurrentClassInfo()
            Return objClassData.Classname
        End Function

        Private Function GetSubjectname(ByVal iSubjectID As Integer) As String
            Dim objSubjectData As New SubjectsData With {.SubjectID = iSubjectID}
            objSubjectData.LoadCurrentSubjectInfo()
            Return objSubjectData.Subjectname
        End Function

        Private Function GetStudentname(ByVal iStudentID As Integer) As String
            Dim objStudentData As New StudentData With {.StudentID = iStudentID}
            objStudentData.LoadCurrentStudentInfo()
            Return objStudentData.Firstname + " " + objStudentData.Lastname
        End Function

#Region "QUESTION BANK - Adding, Removing and Viewing"
        Function CreateQuestions() As ActionResult
            If Session("LoggedinteacherID") IsNot Nothing AndAlso IsNumeric(Session("LoggedinteacherID")) = True Then
                Dim objStudentData As New SubjectsData
                objStudentData.LoadTaughtClassesList()

                Return View(objStudentData)
                'Return View()
            Else
                Return RedirectToAction("Index", "Home")
            End If
        End Function
#End Region

    End Class


End Namespace