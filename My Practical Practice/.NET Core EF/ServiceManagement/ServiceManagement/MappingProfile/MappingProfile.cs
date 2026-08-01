using AutoMapper;
using Entity = ServiceManagement.Core.Entity;
using ServiceManagement.Core.RequestModel;
using ServiceManagement.Core.ResponseModel;

namespace ServiceManagement.MappingProfile
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Entity.Service, ServiceResponse>();
			CreateMap<AddEditServiceRequest, Entity.Service>();
		}
	}
}
