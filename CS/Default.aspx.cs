// Developer Express Code Central Example:
// Inserting a new row in ASPxGridView with the image preview enabled
// 
// This example shows how to use ASPxUploadControl on the ASPxGridView's Edit Form
// to preview an image before performing data update.
// See
// Also:
// http://www.devexpress.com/scid=E95
// http://www.devexpress.com/scid=E1414
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=E2933

using System;
using System.Web.UI;
using System.IO;
using System.Text;
using DevExpress.Web;


public partial class Default : Page
{

	const string BITMAP_ID_BLOCK = "BM";
	const string JPG_ID_BLOCK = "\u00FF\u00D8\u00FF";
	const string PNG_ID_BLOCK = "\u0089PNG\r\n\u001a\n";
	const string GIF_ID_BLOCK = "GIF8";
	const string TIFF_ID_BLOCK = "II*\u0000";
	const int DEFAULT_OLEHEADERSIZE = 78;

	public static byte[] ConvertOleObjectToByteArray(object content) {
		if (content != null && !(content is DBNull)) {
			byte[] oleFieldBytes = (byte[])content;
			byte[] imageBytes = null;
			// Get a UTF7 Encoded string version
			Encoding u8 = Encoding.UTF7;
			string strTemp = u8.GetString(oleFieldBytes);
			// Get the first 300 characters from the string
			string strVTemp = strTemp.Substring(0, 300);
			// Search for the block
			int iPos = -1;
			if (strVTemp.IndexOf(BITMAP_ID_BLOCK) != -1) {
				iPos = strVTemp.IndexOf(BITMAP_ID_BLOCK);
			}
			else if (strVTemp.IndexOf(JPG_ID_BLOCK) != -1) {
				iPos = strVTemp.IndexOf(JPG_ID_BLOCK);
			}
			else if (strVTemp.IndexOf(PNG_ID_BLOCK) != -1) {
				iPos = strVTemp.IndexOf(PNG_ID_BLOCK);
			}
			else if (strVTemp.IndexOf(GIF_ID_BLOCK) != -1) {
				iPos = strVTemp.IndexOf(GIF_ID_BLOCK);
			}
			else if (strVTemp.IndexOf(TIFF_ID_BLOCK) != -1) {
				iPos = strVTemp.IndexOf(TIFF_ID_BLOCK);
			}
			// From the position above get the new image
			if (iPos == -1) {
				iPos = DEFAULT_OLEHEADERSIZE;
			}
			imageBytes = new byte[oleFieldBytes.LongLength - iPos];
			MemoryStream ms = new MemoryStream();
			ms.Write(oleFieldBytes, iPos, oleFieldBytes.Length - iPos);
			imageBytes = ms.ToArray();
			ms.Close();
			ms.Dispose();
			return imageBytes;
		}
		return null;
	}

	protected void ucImage_FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e) {
		Session["uploadedFileData"] = e.UploadedFile.FileBytes;
	}

	protected void grid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e) {
		throw new NotImplementedException("The Insert action is not available when viewing online demos. Remove this line from a real project.");
	}
	protected void grid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e) {
		if (!e.IsNewRow)
			return;

		StringBuilder sb = new StringBuilder();
		if (String.IsNullOrEmpty((string)e.NewValues["CategoryName"]))
			sb.AppendLine("Please enter category's name.");
		if (sb.Length > 0)
			sb.AppendLine();
		if (String.IsNullOrEmpty((string)e.NewValues["Description"]))
			sb.AppendLine("Please enter category's description.");

		e.RowError = string.IsNullOrEmpty(sb.ToString()) ? "The Insert action is not available when viewing online demos. Remove this line from a real project." : sb.ToString();
	}
	protected void callbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e) {
		ASPxCallbackPanel panel = (ASPxCallbackPanel)sender;
		ASPxBinaryImage bImage = (ASPxBinaryImage)panel.FindControl("previewImage");
		bImage.ContentBytes = (byte[])Session["uploadedFileData"];
		bImage.Visible = true;
	}
}
