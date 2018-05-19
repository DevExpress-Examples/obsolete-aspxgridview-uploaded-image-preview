<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>




<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Inserting of a new row in ASPxGridView with the image preview enabled</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <script language="javascript" type="text/javascript">
            var isValidUpload;
            </script>

            <dx:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" ClientIDMode="AutoID" OnCustomErrorText="ASPxGridView1_CustomErrorText"
                DataSourceID="dataSource" EnableCallBacks="False" KeyFieldName="CategoryID" ClientInstanceName="grid"
                OnRowInserting="grid_RowInserting" OnRowValidating="grid_RowValidating">
                <Columns>
                    <dx:GridViewCommandColumn VisibleIndex="0" ShowNewButton="True"/>
                    <dx:GridViewDataTextColumn FieldName="CategoryID" ReadOnly="True" VisibleIndex="0">
                        <EditFormSettings Visible="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="CategoryName" VisibleIndex="1">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Description" VisibleIndex="2">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataBinaryImageColumn FieldName="Picture" VisibleIndex="3">
                        <PropertiesBinaryImage StoreContentBytesInViewState="True">
                        </PropertiesBinaryImage>
                        <DataItemTemplate>
                            <dx:ASPxBinaryImage ID="ASPxBinaryImage1" runat="server" ClientIDMode="AutoID" Value='<%# ConvertOleObjectToByteArray(Eval("Picture")) %>'
                                Width="100" Height="80">
                            </dx:ASPxBinaryImage>
                        </DataItemTemplate>
                    </dx:GridViewDataBinaryImageColumn>
                </Columns>
                <Templates>
                    <EditForm>
                        <table>
                            <tr>
                                <td valign="top">
                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Name">
                                    </dx:ASPxLabel>
                                    <dx:ASPxGridViewTemplateReplacement ID="tbDescription" runat="server" ReplacementType="EditFormCellEditor"
                                        ColumnID="CategoryName" Width="200px"></dx:ASPxGridViewTemplateReplacement>
                                    <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Description">
                                    </dx:ASPxLabel>
                                    <dx:ASPxGridViewTemplateReplacement ID="ASPxGridViewTemplateReplacement1" runat="server"
                                        ReplacementType="EditFormCellEditor" ColumnID="Description" Width="200px"></dx:ASPxGridViewTemplateReplacement>
                                    <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Upload image">
                                    </dx:ASPxLabel>
                                </td>
                                <td valign="top">
                                    <dx:ASPxCallbackPanel ID="callbackPanel" runat="server" ClientInstanceName="efPanel"
                                        OnCallback="callbackPanel_Callback" Width="200px">
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent1" runat="server">
                                                <dx:ASPxBinaryImage ID="previewImage" runat="server" Height="80px" Visible="False"
                                                    Width="100px">
                                                </dx:ASPxBinaryImage>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxCallbackPanel>
                                    <br />
                                    <dx:ASPxUploadControl ID="ucImage" runat="server" ClientInstanceName="uploadControl"  UploadMode="Advanced"
                                        OnFileUploadComplete="ucImage_FileUploadComplete" ShowUploadButton="True">
                                        <ClientSideEvents FilesUploadComplete="function(s, e) { if (isValidUpload) { efPanel.PerformCallback();} }"
                                            FileUploadComplete="function(s, e) { isValidUpload = e.isValid; }" Init="function(s, e) { isValidUpload = false; }" />
                                        <UploadButton Text="Preview">
                                        </UploadButton>
                                       <AdvancedModeSettings EnableDragAndDrop="true" EnableMultiSelect="false"></AdvancedModeSettings>
                                        <ValidationSettings AllowedFileExtensions=".jpg" MaxFileSize="1000000">
                                        </ValidationSettings>
                                    </dx:ASPxUploadControl>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <dx:ASPxButton ID="btnUpdate" runat="server" ClientIDMode="AutoID" Text="Update"
                                        AutoPostBack="False">
                                        <ClientSideEvents Click="function(s, e) { grid.UpdateEdit(); }" />
                                    </dx:ASPxButton>
                                </td>
                                <td>
                                    <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" AutoPostBack="False" ClientIDMode="AutoID">
                                        <ClientSideEvents Click="function(s, e) { grid.CancelEdit();}" />
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </EditForm>
                </Templates>
            </dx:ASPxGridView>
            <asp:SqlDataSource ID="dataSource" runat="server" ConnectionString="Data Source=(local);Initial Catalog=Northwind;Integrated Security=True"
                InsertCommand="INSERT INTO [Categories] ([CategoryName], [Description], [Picture]) VALUES (@CategoryName, @Description, @Picture)"
                ProviderName="System.Data.SqlClient" SelectCommand="SELECT [CategoryID], [CategoryName], [Description], [Picture] FROM [Categories]">
                <InsertParameters>
                    <asp:Parameter Name="CategoryName" Type="String" />
                    <asp:Parameter Name="Description" Type="String" />
                    <asp:SessionParameter SessionField="uploadedFileData" Name="Picture" DbType="Binary" />
                </InsertParameters>
            </asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
