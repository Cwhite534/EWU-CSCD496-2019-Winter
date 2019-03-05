﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace SecretSanta.Web.ApiModels
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            CreateMap<UserViewModel, UserInputViewModel > ();
            CreateMap<GiftViewModel, GiftInputViewModel>();
        }
    }
}
