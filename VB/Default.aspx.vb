' Developer Express Code Central Example:
' Inserting a new row in ASPxGridView with the image preview enabled
' 
' This example shows how to use ASPxUploadControl on the ASPxGridView's Edit Form
' to preview an image before performing data update.
' See
' Also:
' http://www.devexpress.com/scid=E95
' http://www.devexpress.com/scid=E1414
' 
' You can find sample updates and versions for different programming languages here:
' http://www.devexpress.com/example=E2933

Imports System
Imports System.Web.UI
Imports System.IO
Imports System.Text
Imports DevExpress.Web


Partial Public Class [Default]
	Inherits Page

	Private Const BITMAP_ID_BLOCK As String = "BM"
	Private Const JPG_ID_BLOCK As String = "\u00FF\u00D8\u00FF"
	Private Const PNG_ID_BLOCK As String = "\u0089PNG\r\n\u001a\n"
	Private Const GIF_ID_BLOCK As String = "GIF8"
	Private Const TIFF_ID_BLOCK As String = "II*\u0000"
	Private Const DEFAULT_OLEHEADERSIZE As Integer = 78

	Public Shared Function ConvertOleObjectToByteArray(ByVal content As Object) As Byte()
		If content IsNot Nothing AndAlso Not(TypeOf content Is DBNull) Then
			Dim oleFieldBytes() As Byte = DirectCast(content, Byte())
			Dim imageBytes() As Byte = Nothing
			' Get a UTF7 Encoded string version
			Dim u8 As Encoding = Encoding.UTF7
			Dim strTemp As String = u8.GetString(oleFieldBytes)
			' Get the first 300 characters from the string
			Dim strVTemp As String = strTemp.Substring(0, 300)
			' Search for the block
			Dim iPos As Integer = -1
			If strVTemp.IndexOf(BITMAP_ID_BLOCK) <> -1 Then
				iPos = strVTemp.IndexOf(BITMAP_ID_BLOCK)
			ElseIf strVTemp.IndexOf(JPG_ID_BLOCK) <> -1 Then
				iPos = strVTemp.IndexOf(JPG_ID_BLOCK)
			ElseIf strVTemp.IndexOf(PNG_ID_BLOCK) <> -1 Then
				iPos = strVTemp.IndexOf(PNG_ID_BLOCK)
			ElseIf strVTemp.IndexOf(GIF_ID_BLOCK) <> -1 Then
				iPos = strVTemp.IndexOf(GIF_ID_BLOCK)
			ElseIf strVTemp.IndexOf(TIFF_ID_BLOCK) <> -1 Then
				iPos = strVTemp.IndexOf(TIFF_ID_BLOCK)
			End If
			' From the position above get the new image
			If iPos = -1 Then
				iPos = DEFAULT_OLEHEADERSIZE
			End If
			imageBytes = New Byte((oleFieldBytes.LongLength - iPos) - 1){}
			Dim ms As New MemoryStream()
			ms.Write(oleFieldBytes, iPos, oleFieldBytes.Length - iPos)
			imageBytes = ms.ToArray()
			ms.Close()
			ms.Dispose()
			Return imageBytes
		End If
		Return Nothing
	End Function

	Protected Sub ucImage_FileUploadComplete(ByVal sender As Object, ByVal e As DevExpress.Web.FileUploadCompleteEventArgs)
		Session("uploadedFileData") = e.UploadedFile.FileBytes
	End Sub

	Protected Sub grid_RowInserting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataInsertingEventArgs)
		Throw New NotImplementedException("The Insert action is not available when viewing online demos. Remove this line from a real project.")
	End Sub
	Protected Sub grid_RowValidating(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataValidationEventArgs)
		If Not e.IsNewRow Then
			Return
		End If

		Dim sb As New StringBuilder()
		If String.IsNullOrEmpty(CStr(e.NewValues("CategoryName"))) Then
			sb.AppendLine("Please enter category's name.")
		End If
		If sb.Length > 0 Then
			sb.AppendLine()
		End If
		If String.IsNullOrEmpty(CStr(e.NewValues("Description"))) Then
			sb.AppendLine("Please enter category's description.")
		End If

		e.RowError = If(String.IsNullOrEmpty(sb.ToString()), "The Insert action is not available when viewing online demos. Remove this line from a real project.", sb.ToString())
	End Sub
	Protected Sub callbackPanel_Callback(ByVal sender As Object, ByVal e As DevExpress.Web.CallbackEventArgsBase)
		Dim panel As ASPxCallbackPanel = DirectCast(sender, ASPxCallbackPanel)
		Dim bImage As ASPxBinaryImage = CType(panel.FindControl("previewImage"), ASPxBinaryImage)
		bImage.ContentBytes = DirectCast(Session("uploadedFileData"), Byte())
		bImage.Visible = True
	End Sub
End Class
