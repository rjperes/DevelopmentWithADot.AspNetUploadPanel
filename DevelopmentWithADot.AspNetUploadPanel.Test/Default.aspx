<%@ Page Language="C#" CodeBehind="Default.aspx.cs" Inherits="DevelopmentWithADot.AspNetUploadPanel.Test.Default" %>
<%@ Register Assembly="DevelopmentWithADot.AspNetUploadPanel" Namespace="DevelopmentWithADot.AspNetUploadPanel" TagPrefix="web" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<script>
		
		function onBeforeUpload(event)
		{
			debugger;
			return (true);
		}

		function onUploadCanceled(event)
		{
			debugger;
		}

		function onUploadComplete(event)
		{
			debugger;
		}

		function onUploadFailure(event)
		{
			debugger;
		}

		function onUploadProgress(event)
		{
			debugger;
		}

		function onUploadSuccess(event)
		{
			debugger;
		}

		function onValidationFailure(event)
		{
			debugger;
		}

	</script>
</head>
<body>
	<form runat="server">
		<asp:ScriptManager runat="server" />
		<web:UploadPanel runat="server" ID="uploadPanel" MaximumLength="10" OnBeforeUpload="onBeforeUpload" OnUploadCanceled="onUploadCanceled" OnUploadComplete="onUploadComplete" OnUploadFailure="onUploadFailure" OnUploadProgress="onUploadProgress" OnUploadSuccess="onUploadSuccess" OnValidationFailure="onValidationFailure" OnUpload="OnUpload" style="width:300px; height:300px; border:solid 1px;"/>
	</form>
</body>
</html>
