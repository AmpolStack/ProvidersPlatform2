using AutoMapper;
using ProvidersPlatform.Shared.Models;

namespace ProvidersPlatform.UserServices.Rest.Utilities;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserDto>().ForMember(x => x.Provider, opt => opt.MapFrom(map => map.Provider)).ReverseMap();
        CreateMap<Provider, ProviderDto>().ReverseMap();
    }
}

public class UserDto
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }
    
    public string? Description { get; set; }

    public string Email { get; set; } = null!;
    public ProviderDto? Provider { get; set; }
}

public class ProviderDto
{
    public int ProviderId { get; set; }

    public int UserId { get; set; }

    public int Nit { get; set; }

    public string EntityName { get; set; } = null!;

    public string? AssociationPrefix { get; set; }

}