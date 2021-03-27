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
            if (!Directory.Exists(SaveLocation.BaseCertificateFolder))
            {
				Directory.CreateDirectory(SaveLocation.BaseCertificateFolder);
            }
			//Create folder to save temp file
			if (!Directory.Exists(SaveLocation.BaseTempFolder))
			{
				Directory.CreateDirectory(SaveLocation.BaseTempFolder);
			}
			//Create folder to save education system logo image
			if (!Directory.Exists(SaveLocation.EducationSystemLogoImageFolder))
			{
				Directory.CreateDirectory(SaveLocation.EducationSystemLogoImageFolder);
			}
			//Create folder to save education system signature image
			if (!Directory.Exists(SaveLocation.EducationSystemSignatureImageFolder))
			{
				Directory.CreateDirectory(SaveLocation.EducationSystemSignatureImageFolder);
			}
		}
	}
}
