using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DevelopmentWithADot.AspNetUploadPanel
{	
	public class UploadPanel : Panel, ICallbackEventHandler
	{
		private static readonly String [] MultimediaContentTypePrefixes = new String[]{ "image/", "audio/", "video/" };

		public UploadPanel()
		{
			this.ContentTypes = new String[0];
			this.OnUploadFailure = String.Empty;
			this.OnUploadSuccess = String.Empty;
			this.OnValidationFailure = String.Empty;
			this.OnBeforeUpload = String.Empty;
			this.OnUploadComplete = String.Empty;
			this.OnUploadProgress = String.Empty;
			this.OnUploadCanceled = String.Empty;
			this.OnPreviewFile = String.Empty;
		}
	
		public event EventHandler<UploadEventArgs> Upload;

		[DefaultValue("")]
		public String OnPreviewFile
		{
			get;
			set;
		}

		[DefaultValue("")]
		public String OnBeforeUpload
		{
			get;
			set;
		}

		[DefaultValue("")]
		public String OnUploadCanceled
		{
			get;
			set;
		}

		[DefaultValue("")]
		public String OnUploadProgress
		{
			get;
			set;
		}

		[DefaultValue("")]
		public String OnValidationFailure
		{
			get;
			set;
		}

		[DefaultValue("")]
		public String OnUploadComplete
		{
			get;
			set;
		}

		[DefaultValue("")]
		public String OnUploadSuccess
		{
			get;
			set;
		}

		[DefaultValue("")]
		public String OnUploadFailure
		{
			get;
			set;
		}

		[DefaultValue(null)]
		public Int32? MaximumLength
		{
			get;
			set;
		}

		[DefaultValue(null)]
		public Int32? MaximumFiles
		{
			get;
			set;
		}

		[TypeConverter(typeof(StringArrayConverter))]
		public String[] ContentTypes
		{
			get;
			set;
		}

		protected override void OnInit(EventArgs e)
		{
			var script = new StringBuilder();			
			script.AppendFormat("document.getElementById('{0}').addEventListener('drop', function(event) {{\n", this.ClientID);

			script.Append("if (event.dataTransfer.files.length == 0)\n");
			script.Append("{\n");
			script.Append("event.returnValue = false;\n");
			script.Append("event.preventDefault();\n");
			script.Append("return(false);\n");
			script.Append("}\n");

			if (this.MaximumFiles != null)
			{
				script.AppendFormat("if (event.dataTransfer.files.length > {0})\n", this.MaximumFiles.Value);
				script.Append("{\n");

				if (String.IsNullOrWhiteSpace(this.OnValidationFailure) == false)
				{
					script.AppendFormat("{0}(event, 0);\n", this.OnValidationFailure);
				}

				script.Append("event.returnValue = false;\n");
				script.Append("event.preventDefault();\n");
				script.Append("return(false);\n");
				script.Append("}\n");
			}

			if (this.MaximumLength != null)
			{
				script.Append("var lengthOk = true;\n");
				script.Append("for (var i = 0; i < event.dataTransfer.files.length; ++i)\n");
				script.Append("{\n");
				script.AppendFormat("if (event.dataTransfer.files[i].size > {0})\n", this.MaximumLength.Value);
				script.Append("{\n");
				script.Append("lengthOk = false;\n");
				script.Append("break;\n");
				script.Append("}\n");
				script.Append("}\n");
				script.Append("if (lengthOk == false)\n");
				script.Append("{\n");

				if (String.IsNullOrWhiteSpace(this.OnValidationFailure) == false)
				{
					script.AppendFormat("{0}(event, 1);\n", this.OnValidationFailure);
				}

				script.Append("event.returnValue = false;\n");
				script.Append("event.preventDefault();\n");
				script.Append("return(false);\n");
				script.Append("}\n");
			}

			if (this.ContentTypes.Any() == true)
			{
				script.Append("for (var i = 0; i < event.dataTransfer.files.length; ++i)\n");
				script.Append("{\n");
				script.Append("var contentTypeOk = false;\n");

				script.AppendFormat("if ({0})", String.Join(" || ", this.ContentTypes.Select(x => String.Format("(event.dataTransfer.files[i].type.toLowerCase().indexOf('{0}') == 0)", x.ToLower()))));
				script.Append("{\n");
				script.Append("contentTypeOk = true;\n");
				script.Append("}\n");

				script.Append("}\n");
				script.Append("if (contentTypeOk == false)\n");
				script.Append("{\n");

				if (String.IsNullOrWhiteSpace(this.OnValidationFailure) == false)
				{
					script.AppendFormat("{0}(event, 2);\n", this.OnValidationFailure);
				}

				script.Append("event.returnValue = false;\n");
				script.Append("event.preventDefault();\n");
				script.Append("return(false);\n");
				script.Append("}\n");
			}

			if (String.IsNullOrWhiteSpace(this.OnBeforeUpload) == false)
			{
				script.Append("var props = new Object();\n");
				script.AppendFormat("if ({0}(event, props) == false)\n", this.OnBeforeUpload);
				script.Append("{\n");
				script.Append("event.returnValue = false;\n");
				script.Append("event.preventDefault();\n");
				script.Append("return(false);\n");
				script.Append("}\n");
			}

			script.Append("var data = new FormData();\n");
			script.Append("for (var i = 0; i < event.dataTransfer.files.length; ++i)\n");
			script.Append("{\n");
			script.Append("var file = event.dataTransfer.files[i];\n");
			script.Append("data.append('file' + i, file);\n");

			if (String.IsNullOrWhiteSpace(this.OnPreviewFile) == false)
			{
				script.AppendFormat("if ({0})", String.Join(" || ", MultimediaContentTypePrefixes.Select(x => String.Format("(file.type.toLowerCase().indexOf('{0}') == 0)", x.ToLower()))));
				script.Append("{\n");
				script.Append("var reader = new FileReader();\n");
				script.Append("reader.onloadend = function(e)\n");
				script.Append("{\n");
				script.AppendFormat("{0}(file.name, reader.result);\n", this.OnPreviewFile);
				script.Append("}\n");
				script.Append("reader.readAsDataURL(file);\n");
				script.Append("}\n");
			}

			script.Append("}\n");
			script.AppendFormat("data.append('__CALLBACKID', '{0}');\n", this.ClientID);
			script.Append("data.append('__CALLBACKPARAM', '');\n");
			script.Append("data.append('__EVENTTARGET', '');\n");
			script.Append("data.append('__EVENTARGUMENT', '');\n");
			script.Append("for (var key in props)\n");
			script.Append("{\n");
			script.Append("data.append(key, props[key]);\n");
			script.Append("}\n");
			script.Append("var xhr = new XMLHttpRequest();\n");

			if (String.IsNullOrWhiteSpace(this.OnUploadProgress) == false)
			{
				script.Append("xhr.onprogress = function(e)\n");
				script.Append("{\n");
				script.AppendFormat("{0}(e);\n", this.OnUploadProgress);
				script.Append("}\n");
			}

			if (String.IsNullOrWhiteSpace(this.OnUploadCanceled) == false)
			{
				script.Append("xhr.oncancel = function(e)\n");
				script.Append("{\n");
				script.AppendFormat("{0}(e);\n", this.OnUploadCanceled);
				script.Append("}\n");
			}

			script.Append("xhr.onreadystatechange = function(e)\n");
			script.Append("{\n");
			script.Append("if ((xhr.readyState == 4) && (xhr.status == 200))\n");
			script.Append("{\n");
			script.AppendFormat("{0}(e);\n", this.OnUploadSuccess);
			script.Append("}\n");
			script.Append("else if ((xhr.readyState == 4) && (xhr.status != 200))\n");
			script.Append("{\n");
			script.AppendFormat("{0}(e);\n", this.OnUploadFailure);
			script.Append("}\n");
			script.Append("if (xhr.readyState == 4)\n");
			script.Append("{\n");
			script.AppendFormat("{0}(e);\n", this.OnUploadComplete);
			script.Append("}\n");
			script.Append("}\n");
			script.AppendFormat("xhr.open('POST', '{0}', true);\n", this.Context.Request.Url.PathAndQuery);
			script.Append("xhr.send(data);\n");
			script.Append("event.returnValue = false;\n");
			script.Append("event.preventDefault();\n");
			script.Append("return (false);\n");
			script.Append("});\n");

			if (ScriptManager.GetCurrent(this.Page) == null)
			{
				this.Page.ClientScript.RegisterStartupScript(this.GetType(), String.Concat(this.UniqueID, "drop"), script.ToString(), true);
				this.Page.ClientScript.RegisterStartupScript(this.GetType(), String.Concat(this.UniqueID, "dragenter"), String.Format("document.getElementById('{0}').addEventListener('dragenter', function(event){{ event.returnValue = false; event.preventDefault(); return(false); }});\n", this.ClientID), true);
				this.Page.ClientScript.RegisterStartupScript(this.GetType(), String.Concat(this.UniqueID, "dragover"), String.Format("document.getElementById('{0}').addEventListener('dragover', function(event){{ event.returnValue = false; event.preventDefault(); return(false); }});\n", this.ClientID), true);
			}
			else
			{
				this.Page.ClientScript.RegisterStartupScript(this.GetType(), String.Concat(this.UniqueID, "drop"), String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function() {{ {0} }});\n", script), true);
				this.Page.ClientScript.RegisterStartupScript(this.GetType(), String.Concat(this.UniqueID, "dragenter"), String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function() {{ document.getElementById('{0}').addEventListener('dragenter', function(event){{ event.returnValue = false; event.preventDefault(); return(false); }}); }});\n", this.ClientID), true);
				this.Page.ClientScript.RegisterStartupScript(this.GetType(), String.Concat(this.UniqueID, "dragover"), String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function() {{ document.getElementById('{0}').addEventListener('dragover', function(event){{ event.returnValue = false; event.preventDefault(); return(false); }}); }});\n", this.ClientID), true);
			}
			
			base.OnInit(e);
		}

		protected virtual void OnUpload(UploadEventArgs e)
		{
			var handler = this.Upload;

			if (handler != null)
			{
				handler(this, e);
			}
		}

		#region ICallbackEventHandler Members

		String ICallbackEventHandler.GetCallbackResult()
		{
			var args = new UploadEventArgs(this.Context.Request.Files, this.Context.Request.Form);

			this.OnUpload(args);

			return (args.Response);
		}

		void ICallbackEventHandler.RaiseCallbackEvent(String eventArgument)
		{
		}

		#endregion
	}
}