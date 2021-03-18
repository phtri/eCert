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
            //Create folder
            if (!Directory.Exists(SaveCertificateLocation.BaseFolder))
            {
				Directory.CreateDirectory(SaveCertificateLocation.BaseFolder);
            }

			if (!Directory.Exists(SaveCertificateLocation.BaseTempFolder))
			{
				Directory.CreateDirectory(SaveCertificateLocation.BaseTempFolder);
			}

		}
	}
}
