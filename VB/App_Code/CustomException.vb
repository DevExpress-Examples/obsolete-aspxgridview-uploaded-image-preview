Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web

''' <summary>
''' Summary description for CustomException
''' </summary>
Public Class CustomException
    Inherits Exception

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub
End Class