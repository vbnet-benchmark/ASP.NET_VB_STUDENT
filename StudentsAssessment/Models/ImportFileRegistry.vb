''' <summary>
''' Tracks which file names under ~/Content/Uploads were genuinely produced by a real bulk
''' import upload (HomeController.ImportExcel1), so the admin "Manage Uploads" screen can tell a
''' file the system itself actually created apart from an arbitrary admin-supplied file name.
''' This registry is populated ONLY by the real upload handler -- never from an admin's "which
''' file to delete" input -- which is what makes checking against it a genuine authorization
''' control rather than just another existence check.
''' </summary>
Public Class ImportFileRegistry
    Private Shared ReadOnly _registeredFiles As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
    Private Shared ReadOnly _lockObj As New Object()

    ''' <summary>
    ''' Called only once a file has actually been saved to disk via Request.Files(...).SaveAs in
    ''' the real upload handler -- never from an admin's "which file to delete" input.
    ''' </summary>
    Public Shared Sub Register(fileName As String)
        If String.IsNullOrEmpty(fileName) Then Return
        SyncLock _lockObj
            _registeredFiles.Add(fileName)
        End SyncLock
    End Sub

    Public Shared Function IsRegistered(fileName As String) As Boolean
        If String.IsNullOrEmpty(fileName) Then Return False
        SyncLock _lockObj
            Return _registeredFiles.Contains(fileName)
        End SyncLock
    End Function

    Public Shared Function ListRegistered() As List(Of String)
        SyncLock _lockObj
            Return New List(Of String)(_registeredFiles)
        End SyncLock
    End Function

    ''' <summary>
    ''' Deletes the given file from the uploads folder with NO check that it was ever genuinely
    ''' produced by a real import -- the admin-supplied file name alone decides what gets
    ''' deleted.  Used by the "bulk cleanup" admin action.
    ''' </summary>
    Public Shared Sub PurgeFileUnchecked(uploadsDir As String, fileName As String)
        Dim strPath As String = uploadsDir & "\" & fileName
        If System.IO.File.Exists(strPath) Then
            ' SINK: PLANTED-VB-HR-68
            System.IO.File.Delete(strPath)
        End If
    End Sub

    ''' <summary>
    ''' Same bulk-cleanup operation as PurgeFileUnchecked, but re-checks the registry for EVERY
    ''' file name before deleting it -- a file that was never registered as a real import is
    ''' left alone instead of being deleted.
    ''' </summary>
    Public Shared Sub PurgeFileIfRegistered(uploadsDir As String, fileName As String)
        If IsRegistered(fileName) Then
            Dim strPath As String = uploadsDir & "\" & fileName
            If System.IO.File.Exists(strPath) Then
                ' SAFE_SINK: PLANTED-VB-HR-68-safe
                System.IO.File.Delete(strPath)
            End If
        End If
    End Sub
End Class
