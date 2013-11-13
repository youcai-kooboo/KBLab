using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace Framework.Website.Mappings
{
    public class DomainModelToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainModelToViewModelMappingProfile"; }
        }

        protected override void Configure()
        {

        }
    }
}