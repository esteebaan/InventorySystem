using InventorySystem.API.DTOs;
using InventorySystem.Entities.Entities;
using AutoMapper;

namespace InventorySystem.API
{
    /// <summary>
    /// Perfil de AutoMapper para mapear entre entidades y DTOs.
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeos básicos
            CreateMap<Article, ArticleDto>().ReverseMap();
            CreateMap<Client, ClientDto>().ReverseMap();
            CreateMap<Loan, LoanDto>().ReverseMap();
            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<Observation, ObservationDto>().ReverseMap();
            CreateMap<User, UserDTO>();

            CreateMap<UpdateUserDto, User>();
            CreateMap<CreateUserDto, User>();
            CreateMap<User, UserDTO>();

            CreateMap<UpdateClientDto, Client>();
            CreateMap<UpdateArticleDto, Article>();
            //CreateMap<Role, RoleDto>().ReverseMap();

            // Otros mapeos que uses
            CreateMap<CreateArticleDto, Article>();
            CreateMap<CreateClientDto, Client>();
            CreateMap<CreateLoanDto, Loan>();
            CreateMap<CreateUserDto, User>();
        }
    }
}
