using System;
using System.Web.UI;
using System.IO;
using System.Text;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxCallbackPanel;


public partial class Default : Page {

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

    protected void ucImage_FileUploadComplete(object sender, DevExpress.Web.ASPxUploadControl.FileUploadCompleteEventArgs e) {        
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

        e.RowError = sb.ToString();
    }
    protected void callbackPanel_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e) {        
        ASPxCallbackPanel panel = (ASPxCallbackPanel)sender;
        ASPxBinaryImage bImage = (ASPxBinaryImage)panel.FindControl("previewImage");
        bImage.ContentBytes = (byte[])Session["uploadedFileData"];
        bImage.Visible = true;
    }
}
