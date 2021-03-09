﻿using AutoMapper;
using eCert.Models.Entity;
using eCert.Models.ViewModel;
using eCert.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCert.AutoMapperConfig
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Certificate, CertificateViewModel>().ReverseMap();
            CreateMap<Pagination<Certificate>, Pagination<CertificateViewModel>>().ReverseMap();
            CreateMap<CertificateContents, CertificateContentsViewModel>().ReverseMap();
            CreateMap<User, UserViewModel>().ReverseMap();

            
        }
    }
}