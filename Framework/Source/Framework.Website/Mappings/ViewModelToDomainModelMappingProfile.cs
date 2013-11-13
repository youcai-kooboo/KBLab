using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace Framework.Website.Mappings
{
    public class ViewModelToDomainModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainModelMappingProfile"; }
        }

        protected override void Configure()
        {
           
        }
    }
}