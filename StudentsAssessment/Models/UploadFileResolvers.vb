''' <summary>
''' Resolves an admin-supplied "which uploaded file" reference to an absolute path on disk.
''' Two independent implementations exist side by side so the admin "Replace Uploaded File"
''' screen can support an older ("legacy mode") compatibility path alongside the newer,
''' registry-checked one -- see AdminsController.ReplaceUploadedFileLegacyMode /
''' .ReplaceUploadedFileRegistryMode.
''' </summary>
Public Interface IUploadFileResolver
    Function ResolvePath(uploadsDir As String, reference As String) As String
End Interface

''' <summary>
''' The original resolution logic, kept for admins still relying on the "legacy mode" compat
''' flag: the reference IS the file name, with no check that it was ever a real recorded import.
''' </summary>
Public Class LegacyFileResolver
    Implements IUploadFileResolver

    Public Function ResolvePath(uploadsDir As String, reference As String) As String Implements IUploadFileResolver.ResolvePath
        Return uploadsDir & "\" & reference
    End Function
End Class

''' <summary>
''' The current resolution logic: the reference is only ever used as a lookup key into the
''' server-controlled ImportFileRegistry, never resolved to a path directly.
''' </summary>
Public Class RegistryFileResolver
    Implements IUploadFileResolver

    Public Function ResolvePath(uploadsDir As String, reference As String) As String Implements IUploadFileResolver.ResolvePath
        If Not ImportFileRegistry.IsRegistered(reference) Then
            Return Nothing
        End If
        Return uploadsDir & "\" & reference
    End Function
End Class
