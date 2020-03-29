using AutoMapper;
using IRS.API.Dtos;
using IRS.DAL.Models;
using IRS.API.Dtos.IncidenceResources;
using IRS.DAL.Models.QueryResource.Incidence;
using IRS.DAL.Models.QueryResources.Incidence;
using IRS.DAL.Models.QueryResources.QueryResult;
using IRS.DAL.Models.SharedResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IRS.API.Dtos.UserResources;
using IRS.DAL.ViewModel;
using IRS.API.Dtos.LocationDto;
using IRS.DAL.Models.QueryResources.Users;
using IRS.DAL.Models.OrganizationAndDepartments;
using IRS.DAL.Models.Identity;
using IRS.API.Dtos.HazardResources;

namespace BeanCorp.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            //domain to API resource: Formember :target, source
            CreateMap<Media, MediaResource>();
            CreateMap(typeof(QueryResult<>), typeof(QueryResultResource<>));
            CreateMap<UsersQueryResource, UsersQuery>();
            CreateMap<UsersRolesQueryResource, UsersRolesQuery>();
            CreateMap<Area, AreaDto>();
            CreateMap<AreaDetailsViewModel, AreaDetailsDto>();
            CreateMap<StateDetailsViewModel, StateDetailsDto>();
            CreateMap<UsersRolesViewModel, UsersRolesDetailsDto>()
                .ForMember(vr => vr.UserOrganization, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.UserOrganization.Id, Name = v.UserOrganization.CompanyName }))
                .ForMember(vr => vr.UserDepartment, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.UserDepartment.Id, Name = v.UserDepartment.Name }))
                .ForMember(vr => vr.FullName, opt => opt.MapFrom(v => v.FirstName + " " + v.MiddleName + " " + v.LastName));

            CreateMap<LocationQueryResource, LocationQuery>();
            CreateMap<City, CityDto>();
            CreateMap<State, StateDto>();
            CreateMap<Country, CountryDto>();
            CreateMap<IncidenceType, KeyValuePairResource>();
            CreateMap<OrganizationDepartment, DepartmentKeyValuePairDto>()
                .ForMember(d => d.Users, opt => opt.MapFrom(v => v.Users.Select(vf => new KeyValuePairResource
                {
                    Id = vf.Id,
                    Name = vf.toStringField
                })));

            CreateMap<IncidenceTypeDepartmentMapping, IncidenceTypeDepartmentDto>()
                .ForMember(vr => vr.Department, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.Department.Id, Name = v.Department.Name }))
                .ForMember(vr => vr.IncidenceType, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.IncidenceType.Id, Name = v.IncidenceType.Name }));
            CreateMap<IncidenceTypeDepartmentViewModel, IncidenceTypeDepartmentDto>()
                .ForMember(vr => vr.Organization, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.OrganizationId, Name = v.OrganizationName }))
                .ForMember(vr => vr.Department, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.DepartmentId, Name = v.DepartmentName }))
                .ForMember(vr => vr.IncidenceType, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.IncidenceTypeId, Name = v.IncidenceTypeName }));
            CreateMap<IncidenceStatus, KeyValuePairResource>();
            CreateMap<OrganizationDepartment, KeyValuePairResource>();
            CreateMap<Organization, OrganizationKeyValueResource>()
                .ForMember(vr => vr.Name, opt => opt.MapFrom(v => v.CompanyName))
                .ForMember(d => d.Departments, opt => opt.MapFrom(v => v.OrganizationDepartments.Select(vf => new DepartmentKeyValueResource
                {
                    Id = vf.Id,
                    Name = vf.Name,
                    Users = vf.Users.Select(vd => new KeyValuePairResource
                    {
                        Id = vd.Id,
                        Name = vd.toStringField
                    }).ToList()
                }))); ;

            //CreateMap<OrganizationDepartment, DepartmentKeyValueResource>()
            //    .ForMember(d => d.Users, opt => opt.MapFrom(v => v.Users.Select(vf => new KeyValuePairResource
            //    {
            //        Id = vf.Id,
            //        Name = vf.toStringField
            //    })));
            //CreateMap<Organization, KeyValuePairResource>()
            //    .ForMember(vr => vr.Name, opt => opt.MapFrom(v => v.CompanyName));

            CreateMap<IncidenceTypeViewModel, IncidenceTypeDto>();
            CreateMap<IncidenceTypeViewModel, IncidenceTypeDto>();
            CreateMap<IncidenceStatusViewModel, IncidenceStatusDto>();

            CreateMap<IncidenceType, IncidenceTypeDto>();
            CreateMap<IncidenceStatus, IncidenceStatusDto>();

            CreateMap<Area, AreaDetailsDto>()
                .ForMember(vr => vr.AreaCode, opt => opt.MapFrom(v => v.Code))
                .ForMember(vr => vr.AreaName, opt => opt.MapFrom(v => v.Name));

            CreateMap<City, CityDetailsDto>()
                .ForMember(vr => vr.CityCode, opt => opt.MapFrom(v => v.Code))
                .ForMember(vr => vr.CityName, opt => opt.MapFrom(v => v.Name));

            CreateMap<State, StateDetailsDto>()
                .ForMember(vr => vr.StateCode, opt => opt.MapFrom(v => v.Code))
                .ForMember(vr => vr.StateName, opt => opt.MapFrom(v => v.Name));

            CreateMap<OrganizationDepartment, DepartmentDto>()
                .ForMember(vr => vr.Organization, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.Organization.Id, Name = v.Organization.CompanyName }));

            CreateMap<Country, CountryDetailsDto>();

            CreateMap<OrganizationViewModel, OrganizationDto>()
                .ForMember(vr => vr.Area, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.Area.Id, Name = v.Area.Name }))
                .ForMember(vr => vr.City, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.City.Id, Name = v.City.Name }))
                .ForMember(vr => vr.State, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.State.Id, Name = v.State.Name }))
                .ForMember(vr => vr.Country, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.Country.Id, Name = v.Country.Name }));

            CreateMap<Organization, OrganizationDto>()
                .ForMember(vr => vr.Area, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.Area.Id, Name = v.Area.Name }))
                .ForMember(vr => vr.City, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.City.Id, Name = v.City.Name }))
                .ForMember(vr => vr.State, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.State.Id, Name = v.State.Name }))
                .ForMember(vr => vr.Country, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.Country.Id, Name = v.Country.Name }));

            CreateMap<DepartmentViewModel, DepartmentDto>()
                .ForMember(vr => vr.Organization, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.Organization.Id, Name = v.Organization.CompanyName }));

            //CreateMap<Organization, KeyValuePairResource>()
            //    .ForMember(vr => vr.Name, opt => opt.MapFrom(v => v.CompanyName));

            CreateMap<Organization, OrganizationResource>()
                .ForMember(vr => vr.Area, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.Area.Id, Name = v.Area.Name }))
                .ForMember(vr => vr.City, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.City.Id, Name = v.City.Name }))
                .ForMember(vr => vr.State, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.State.Id, Name = v.State.Name }))
                .ForMember(vr => vr.Country, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.Country.Id, Name = v.Country.Name }));

            //get incidence/id
            CreateMap<Incidence, IncidenceResource>()
                .ForMember(vr => vr.Area, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.Area.Id, Name = v.Area.Name }))
                .ForMember(vr => vr.City, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.City.Id, Name = v.City.Name }))
                .ForMember(vr => vr.State, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.State.Id, Name = v.State.Name }))
                .ForMember(vr => vr.Country, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.Country.Id, Name = v.Country.Name }))
                //.ForMember(vr => vr.AssignedDepartment, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.AssignedDepartment.Id, Name = v.AssignedDepartment.Name }))
                .ForMember(vr => vr.IncidenceStatus, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.IncidenceStatuses.Id, Name = v.IncidenceStatuses.Name }))
                .ForMember(vr => vr.IncidenceType, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.IncidenceTypes.Id, Name = v.IncidenceTypes.Name }))
                .ForMember(vr => vr.Assigner, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.Assigner.Id, Name = v.Assigner.FirstName }))
                .ForMember(vr => vr.Assignee, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.Assignee.Id, Name = v.Assignee.FirstName }))
                .ForMember(vr => vr.AssignedOrganization, opt => opt.MapFrom(v => new KeyValuePairResource
                {
                    Id = v.AssignedOrganization.Id,
                    Name = v.AssignedOrganization.CompanyName
                }));

            CreateMap<Hazard, HazardResource>()
                .ForMember(vr => vr.Area, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.Area.Id, Name = v.Area.Name }))
                .ForMember(vr => vr.City, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.City.Id, Name = v.City.Name }))
                .ForMember(vr => vr.State, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.State.Id, Name = v.State.Name }))
                .ForMember(vr => vr.Country, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.Country.Id, Name = v.Country.Name }))
                //.ForMember(vr => vr.AssignedDepartment, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.AssignedDepartment.Id, Name = v.AssignedDepartment.Name }))
                .ForMember(vr => vr.IncidenceStatus, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.IncidenceStatuses.Id, Name = v.IncidenceStatuses.Name }))
                .ForMember(vr => vr.Assigner, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.Assigner.Id, Name = v.Assigner.FirstName }))
                .ForMember(vr => vr.Assignee, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.Assignee.Id, Name = v.Assignee.FirstName }))
                .ForMember(vr => vr.AssignedOrganization, opt => opt.MapFrom(v => new KeyValuePairResource
                {
                    Id = v.AssignedOrganization.Id,
                    Name = v.AssignedOrganization.CompanyName
                }));

            //get all incidencelist
            CreateMap<IncidenceViewModel, IncidenceResource>()
                .ForMember(vr => vr.Area, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.AreaId, Name = v.AreaName }))
                .ForMember(vr => vr.City, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.CityId, Name = v.CityName }))
                .ForMember(vr => vr.State, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.StateId, Name = v.StateName }))
                .ForMember(vr => vr.Country, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.CountryId, Name = v.CountryName }))
                .ForMember(vr => vr.AssignedDepartment, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.AssignedDepartmentId, Name = v.AssignedDepartmentName }))
                .ForMember(vr => vr.IncidenceStatus, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.IncidenceStatusId, Name = v.IncidenceStatusName }))
                .ForMember(vr => vr.IncidenceType, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.IncidenceTypeId, Name = v.IncidenceTypeName }))
                .ForMember(vr => vr.Assignee, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.AssigneeId, Name = v.AssigneeName }))
                .ForMember(vr => vr.Assigner, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.AssignerId, Name = v.AssignerName }))
                .ForMember(d => d.Photos, opt => opt.MapFrom(v => v.Photos.Select(vf => new KeyValuePairResource
                {
                    Id = vf.Id,
                    Name = vf.FileName
                })))
                .ForMember(d => d.Videos, opt => opt.MapFrom(v => v.Videos.Select(vf => new KeyValuePairResource
                {
                    Id = vf.Id,
                    Name = vf.FileName
                })))
                .ForMember(vr => vr.AssignedOrganization, opt => opt.MapFrom(v => new KeyValuePairResource
                {
                    Id = v.AssignedOrganizationId,
                    Name = v.AssignedOrganizationName
                }));

            //get all Hazardlist
            CreateMap<HazardViewModel, HazardResource>()
                .ForMember(vr => vr.Area, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.AreaId, Name = v.AreaName }))
                .ForMember(vr => vr.City, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.CityId, Name = v.CityName }))
                .ForMember(vr => vr.State, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.StateId, Name = v.StateName }))
                .ForMember(vr => vr.Country, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.CountryId, Name = v.CountryName }))
                .ForMember(vr => vr.AssignedDepartment, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.AssignedDepartmentId, Name = v.AssignedDepartmentName }))
                .ForMember(vr => vr.IncidenceStatus, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.IncidenceStatusId, Name = v.IncidenceStatusName }))
                .ForMember(vr => vr.Assignee, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.AssigneeId, Name = v.AssigneeName }))
                .ForMember(vr => vr.Assigner, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.AssignerId, Name = v.AssignerName }))
                .ForMember(d => d.Photos, opt => opt.MapFrom(v => v.Photos.Select(vf => new KeyValuePairResource
                {
                    Id = vf.Id,
                    Name = vf.FileName
                })))
                .ForMember(d => d.Videos, opt => opt.MapFrom(v => v.Videos.Select(vf => new KeyValuePairResource
                {
                    Id = vf.Id,
                    Name = vf.FileName
                })))
                .ForMember(vr => vr.AssignedOrganization, opt => opt.MapFrom(v => new KeyValuePairResource
                {
                    Id = v.AssignedOrganizationId,
                    Name = v.AssignedOrganizationName
                }));

            CreateMap<Incidence, SaveIncidenceResource>();
            CreateMap<Hazard, SaveHazardResource>();

            // CreateMap<Incidence, SaveIncidenceOnMobileResource>();
            CreateMap<Incidence, UserIncidenceResource>();

            CreateMap<User, UserKeyValuePairResource>()
                .ForMember(vr => vr.Name, opt => opt.MapFrom(v => v.toStringField));

            // for UserRepository GetUser() method. Useful for returning user data after create or update. Use GetFullUserData in create&edit actions from the userRepo if looking for Deployment and Dept info and comment this mapping out
            CreateMap<User, UserDetailsDto>()
                .ForMember(vr => vr.FullName, opt => opt.MapFrom(v => v.FirstName + " " + v.MiddleName + " " + v.LastName))
                .ForMember(vr => vr.UserOrganization, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.UserOrganization.Id, Name = v.UserOrganization.CompanyName }))
                .ForMember(d => d.AllocatedIncidences, opt => opt.MapFrom(v => v.AllocatedIncidences.Select(vf => new UserIncidenceResource
                {
                    //Id = vf.Id,
                    Title = vf.Title,
                    IncidenceType = new KeyValuePairResource { Id = vf.IncidenceTypes.Id, Name = vf.IncidenceTypes.Name },
                    IncidenceStatus = new KeyValuePairResource { Id = vf.IncidenceStatuses.Id, Name = vf.IncidenceStatuses.Name },
                    //Area = new KeyValuePairResource { Id = vf.AreaId, Name = vf.Area.Name },
                    ReporterName = vf.ReporterName,
                    ReporterEmail = vf.ReporterEmail,
                    ReporterFirstResponderAction = vf.ReporterFirstResponderAction,
                    ResolutionDate = vf.ResolutionDate

                })))
                .ForMember(d => d.AssignedIncidences, opt => opt.MapFrom(v => v.AssignedIncidences.Select(vf => new UserIncidenceResource
                {
                    //Id = vf.Id,
                    Title = vf.Title,
                    IncidenceType = new KeyValuePairResource { Id = vf.IncidenceTypes.Id, Name = vf.IncidenceTypes.Name },
                    IncidenceStatus = new KeyValuePairResource { Id = vf.IncidenceStatuses.Id, Name = vf.IncidenceStatuses.Name },
                    //Area = new KeyValuePairResource { Id = vf.AreaId, Name = vf.Area.Name },
                    ReporterName = vf.ReporterName,
                    ReporterEmail = vf.ReporterEmail,
                    ReporterFirstResponderAction = vf.ReporterFirstResponderAction,
                    ResolutionDate = vf.ResolutionDate

                }))
                );

            CreateMap<UserDetailsViewModel, UserDetailsDto>()
                .ForMember(vr => vr.FullName, opt => opt.MapFrom(v => v.FirstName + " " + v.MiddleName + " " + v.LastName))
                .ForMember(vr => vr.UserDepartment, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.UserDepartment.Id, Name = v.UserDepartment.Name }))
                .ForMember(vr => vr.UserOrganization, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.UserOrganization.Id, Name = v.UserOrganization.CompanyName }))
                .ForMember(vr => vr.AreaOfDeployment, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.AreaOfDeployment.Id, Name = v.AreaOfDeployment.Name }))
                .ForMember(vr => vr.CityOfDeployment, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.CityOfDeployment.Id, Name = v.CityOfDeployment.Name }))
                .ForMember(vr => vr.StateOfDeployment, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.StateOfDeployment.Id, Name = v.StateOfDeployment.Name }))
                .ForMember(vr => vr.CountryOfDeployment, opt => opt.MapFrom(v => new KeyValuePairResource { Id = v.CountryOfDeployment.Id, Name = v.CountryOfDeployment.Name }));

            CreateMap<SystemSetting, SystemSettingsDto>()
                .ForMember(vr => vr.OrganizationName, opt => opt.MapFrom(v => v.EmailSenderName));

            CreateMap<SystemSetting, SystemSettingsDto>()
                .ForMember(vr => vr.OrganizationName, opt => opt.MapFrom(v => v.EmailSenderName));

            //Api Resource to domain
            CreateMap<IncidenceTypeDepartmentDto, IncidenceTypeDepartmentMapping>();

            CreateMap<IncidenceTypeDto, IncidenceType>();
            CreateMap<IncidenceStatusDto, IncidenceStatus>();
            CreateMap<SaveIncidenceResource, Incidence>()
                .ForMember(v => v.Id, opt => opt.Ignore());

            CreateMap<SaveHazardResource, Hazard>()
                .ForMember(v => v.Id, opt => opt.Ignore());

            //CreateMap<SaveIncidenceOnMobileResource, Incidence>()
            //    .ForMember(v => v.Id, opt => opt.Ignore());

            CreateMap<OrganizationDto, Organization>();

            CreateMap<SaveUserDto, User>()
                .ForMember(v => v.Id, opt => opt.Ignore())
                .ForMember(d => d.AssignedIncidences, opt => opt.Ignore())
                .ForMember(d => d.AllocatedIncidences, opt => opt.Ignore()); //to do -update method if user changes password SaveUserDto, UserDeployment

            CreateMap<SaveUserDto, UserDeployment>()
                .ForMember(v => v.Id, opt => opt.Ignore());

            CreateMap<AreaDetailsDto, Area>()
                .ForMember(v => v.Id, opt => opt.Ignore())
                .ForMember(v => v.Protected, opt => opt.Ignore())
                .ForMember(v => v.Deleted, opt => opt.Ignore())
                .ForMember(vr => vr.Code, opt => opt.MapFrom(v => v.AreaCode))
                .ForMember(vr => vr.Name, opt => opt.MapFrom(v => v.AreaName));

            CreateMap<CityDetailsDto, City>()
                .ForMember(v => v.Id, opt => opt.Ignore())
                .ForMember(v => v.Protected, opt => opt.Ignore())
                .ForMember(v => v.Deleted, opt => opt.Ignore())
                .ForMember(vr => vr.Code, opt => opt.MapFrom(v => v.CityCode))
                .ForMember(vr => vr.Name, opt => opt.MapFrom(v => v.CityName));

            CreateMap<StateDetailsDto, State>()
                .ForMember(v => v.Id, opt => opt.Ignore())
                .ForMember(v => v.Protected, opt => opt.Ignore())
                .ForMember(v => v.Deleted, opt => opt.Ignore())
                .ForMember(vr => vr.Code, opt => opt.MapFrom(v => v.StateCode))
                .ForMember(vr => vr.Name, opt => opt.MapFrom(v => v.StateName));

            CreateMap<DepartmentDto, OrganizationDepartment>()
                .ForMember(v => v.Id, opt => opt.Ignore())
                .ForMember(v => v.Protected, opt => opt.Ignore())
                .ForMember(v => v.Deleted, opt => opt.Ignore());

            CreateMap<CountryDetailsDto, Country>()
                .ForMember(v => v.Id, opt => opt.Ignore())
                .ForMember(v => v.Protected, opt => opt.Ignore())
                .ForMember(v => v.Deleted, opt => opt.Ignore());

            //CreateMap<Incidence, IncidenceResource>();
        }
        
    }
}
