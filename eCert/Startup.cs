using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using static eCert.Utilities.Constants;

[assembly: OwinStartup(typeof(eCert.Startup))]

namespace eCert
{
	public partial class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			ConfigureAuth(app);
            //Create folder to save certificate
            if (!Directory.Exists(SaveLocation.BaseFolder))
            {
				Directory.CreateDirectory(SaveLocation.BaseFolder);
            }
			//Create folder to save temp file
			if (!Directory.Exists(SaveLocation.BaseTempFolder))
			{
				Directory.CreateDirectory(SaveLocation.BaseTempFolder);
			}
			//Create folder to save system education logo image
			if (!Directory.Exists(SaveLocation.EducationSystemFolder))
			{
				Directory.CreateDirectory(SaveLocation.EducationSystemFolder);
			}
		}
	}
}
