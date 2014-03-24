using System;
using System.Collections.Specialized;
using System.Web;

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
}
