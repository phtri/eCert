using eCert.Daos;
using eCert.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCert.Services
{
    public class SuperAdminServices
    {
        private readonly SuperAdminDAO _superAdminDao;

        public SuperAdminServices()
        {
            _superAdminDao = new SuperAdminDAO();
        }

        //Add education system to database
        public void AddEducationSystem(EducationSystemViewModel educationSystemViewModel)
        {

        }
    }
}