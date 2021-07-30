using AutoMapper;
using My_Company.Models;
using My_Company.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.AutoMapper
{
    public class FromDTOProfile: Profile
    {
        public FromDTOProfile()
        {
            //CreateMap<OrganizationDto, Organization>()
            //    .ForMember(x => x.Acronym, opt => opt.MapFrom(src => src.Acronym))
            //    .ForMember(x => x.AttendantId, opt => opt.MapFrom(src => src.AttendantId))
            //    .ForMember(x => x.City, opt => opt.MapFrom(src => src.City))
            //    .ForMember(x => x.DirectionalNumber, opt => opt.MapFrom(src => src.DirectionalNumber))
            //    .ForMember(x => x.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            //    .ForMember(x => x.Email, opt => opt.MapFrom(src => src.Email))
            //    .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
            //    .ForMember(x => x.NIP, opt => opt.MapFrom(src => src.NIP))
            //    .ForMember(x => x.PostalCode, opt => opt.MapFrom(src => src.PostalCode))
            //    .ForMember(x => x.Street, opt => opt.MapFrom(src => src.Street))
            //    .ForMember(x => x.Type, opt => opt.MapFrom(src => src.Type))
            //    .ForMember(x => x.Status, opt => opt.MapFrom(src => src.Status))
            //    .ForMember(x => x.ParentOrganizationId, opt => opt.MapFrom(src => src.ParentOrganizationId))
            //    .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<NewWarehouseViewModel, Warehouse>();
        }
    }
}
