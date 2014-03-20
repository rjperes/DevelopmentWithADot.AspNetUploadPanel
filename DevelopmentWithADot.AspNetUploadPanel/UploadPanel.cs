using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DevelopmentWithADot.AspNetUploadPanel
{
	[Serializable]
	public sealed class UploadEventArgs : EventArgs
	{
		public UploadEventArgs(HttpFileCollection files, NameValueCollection form)
		{
			this.Files = files;
			this.Response = String.Empty;
			this.Form = form;
		}

		public String Response
		{
			get;
			set;
		}

		public NameValueCollection Form
		{
			get;
			private set;
		}

		public HttpFileCollection Files
		{
			get;
			private set;
		}		
	}

	public class UploadPanel : Panel, ICallbackEventHandler
	{
		public UploadPanel()
		{
			this.ContentTypes = new String[0];
			this.OnUploadFailure = "function(event){}";
			this.OnUploadSuccess = "function(event){}";
			this.OnValidationFailure = "function(event){}";
			this.OnBeforeUpload = "function(event){}";
			this.OnUploadComplete = "function(event){}";
			this.OnUploadProgress = "function(event){}";
			this.OnUploadCanceled = "function(event){}";
		}
	
		public event EventHandler<UploadEventArgs> Upload;

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
			StringBuilder script = new StringBuilder();			
			script.AppendFormat("document.getElementById('{0}').addEventListener('drop', function(event) {{\n", this.ClientID);

			if (this.MaximumFiles != null)
			{
				script.AppendFormat("if (event.dataTransfer.files.length > {0})\n", this.MaximumFiles.Value);
				script.Append("{\n");

				if (String.IsNullOrWhiteSpace(this.OnValidationFailure) == false)
				{
					script.AppendFormat("{0}(event);\n", this.OnValidationFailure);
				}

				script.Append("event.returnValue = false;\n");
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
					script.AppendFormat("{0}(event);\n", this.OnValidationFailure);
				}

				script.Append("event.returnValue = false;\n");
				script.Append("return(false);\n");
				script.Append("}\n");
			}

			if (this.ContentTypes.Any() == true)
			{
				script.Append("for (var i = 0; i < event.dataTransfer.files.length; ++i)\n");
				script.Append("{\n");
				script.Append("var contentTypeOk = false;\n");

				foreach (var contentType in this.ContentTypes.Select(x => x.ToLower()).Distinct())
				{
					script.AppendFormat("if (event.dataTransfer.files[i].type.toLowerCase() == '{0}')\n", contentType);
					script.Append("{\n");
					script.Append("contentTypeOk = true;\n");
					script.Append("break;\n");
					script.Append("}\n");
				}

				script.Append("}\n");
				script.Append("if (contentTypeOk == false)\n");
				script.Append("{\n");

				if (String.IsNullOrWhiteSpace(this.OnValidationFailure) == false)
				{
					script.AppendFormat("{0}(event);\n", this.OnValidationFailure);
				}

				script.Append("event.returnValue = false;\n");
				script.Append("return(false);\n");
				script.Append("}\n");
			}

			script.Append("var data = new FormData();\n");

			if (String.IsNullOrWhiteSpace(this.OnBeforeUpload) == false)
			{
				script.AppendFormat("if ({0}(event) == false)\n", this.OnBeforeUpload);
				script.Append("{\n");
				script.Append("event.returnValue = false;\n");
				script.Append("return(false);\n");
				script.Append("}\n");
			}

			script.Append("for (var i = 0; i < event.dataTransfer.files.length; ++i)\n");
			script.Append("{\n");
			script.Append("data.append('file' + i, event.dataTransfer.files[i]);\n");
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
			script.AppendFormat("data.append('__CALLBACKID', '{0}');\n", this.ClientID);
			script.Append("data.append('__CALLBACKPARAM', '');\n");
			script.Append("data.append('__EVENTTARGET', '');\n");
			script.Append("data.append('__EVENTARGUMENT', '');\n");
			script.AppendFormat("for (var key in document.getElementById('{0}').dataset)\n", this.ClientID);
			script.Append("{\n");
			script.AppendFormat("data.append(key, document.getElementById('{0}').dataset[key]);\n", this.ClientID);
			script.Append("}\n");
			script.Append("xhr.send(data);\n");
			script.Append("event.returnValue = false;\n");
			script.Append("return (false);\n");
			script.Append("});\n");
			
			this.Page.ClientScript.RegisterStartupScript(this.GetType(), this.UniqueID + "drop", String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function() {{ {0} }});\n", script), true);

			this.Attributes["ondragenter"] = "return(false)";
			this.Attributes["ondragover"] = "return(false)";

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
			UploadEventArgs args = new UploadEventArgs(this.Context.Request.Files, this.Context.Request.Form);

			this.OnUpload(args);

			return (args.Response);
		}

		void ICallbackEventHandler.RaiseCallbackEvent(String eventArgument)
		{
		}

		#endregion
	}
}