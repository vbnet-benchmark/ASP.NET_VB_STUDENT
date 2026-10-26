Imports System.Web.Mvc

Namespace Controllers
    Public Class ErrorController
        Inherits Controller

        ' GET: Error
        Function Index() As ActionResult
            Return View()
        End Function

        Function NotFound() As ActionResult
            Return View()
        End Function
    End Class
End Namespace