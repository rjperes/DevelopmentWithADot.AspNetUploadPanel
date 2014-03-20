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

		function onUploadCancelled(event)
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

		function onValidationFailure(event, error)
		{
			//error=0: maximum files
			//error=1: maximum length
			//error=2: content type
			debugger;
		}

	</script>
</head>
<body>
	<form runat="server">
		<asp:ScriptManager runat="server" />
		<web:UploadPanel runat="server" ID="uploadPanel" MaximumLength="1000000" ContentTypes="text/plain,text/html" OnBeforeUpload="onBeforeUpload" OnUploadCancelled="onUploadCancelled" OnUploadComplete="onUploadComplete" OnUploadFailure="onUploadFailure" OnUploadProgress="onUploadProgress" OnUploadSuccess="onUploadSuccess" OnValidationFailure="onValidationFailure" OnUpload="OnUpload" Style="width: 300px; height: 300px; border: solid 1px;" data-a="1" data-b="2" />
	</form>
</body>
</html>
