using System;
using System.Web.UI;

namespace DevelopmentWithADot.AspNetUploadPanel.Test
{
	public partial class Default : Page
	{
		protected void OnUpload(Object sender, UploadEventArgs e)
		{
			e.Response = String.Format("a={0}\nb={1}", e.Form["a"], e.Form["b"]);
		}
	}
}