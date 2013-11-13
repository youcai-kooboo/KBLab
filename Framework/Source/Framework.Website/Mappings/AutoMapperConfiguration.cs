using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace Framework.Website.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<DomainModelToViewModelMappingProfile>();
                x.AddProfile<ViewModelToDomainModelMappingProfile>();
            });
        }
    }
}