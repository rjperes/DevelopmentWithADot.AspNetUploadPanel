<%@ Page Language="C#" CodeBehind="Default.aspx.cs" Inherits="DevelopmentWithADot.AspNetUploadPanel.Test.Default" %>
<%@ Register Assembly="DevelopmentWithADot.AspNetUploadPanel" Namespace="DevelopmentWithADot.AspNetUploadPanel" TagPrefix="web" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<script>

		function onPreviewFile(name, url)
		{
			debugger;
			name = name.toLowerCase();

			if ((name.indexOf('.jpg') > 0) || (name.indexOf('.gif') > 0) || (name.indexOf('.png') > 0))
			{
				document.getElementById('preview').src = url;
			}
		}

		function onBeforeUpload(event, props)
		{
			debugger;
			props.a = 1;
			props.b = 2;
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

		function onValidationFailure(event, error)
		{
			//error=0: maximum files reached
			//error=1: maximum file length reached
			//error=2: invalid content type
			debugger;
		}

	</script>
</head>
<body>
	<form runat="server">
		<asp:ScriptManager runat="server" />
		<img src="" alt="" id="preview" style="max-width:300px; max-height:300px;"/>
		<web:UploadPanel runat="server" ID="uploadPanel" MaximumFiles="2" MaximumLength="1000000" ContentTypes="image/" OnPreviewFile="onPreviewFile" OnBeforeUpload="onBeforeUpload" onUploadCanceled="onUploadCanceled" OnUploadComplete="onUploadComplete" OnUploadFailure="onUploadFailure" OnUploadProgress="onUploadProgress" OnUploadSuccess="onUploadSuccess" OnValidationFailure="onValidationFailure" OnUpload="OnUpload" Style="width: 300px; height: 300px; border: solid 1px;" />
	</form>
</body>
</html>
