using AutoMapper;
using OtisAPI.Model.DataEntities.Elevators;
using OtisAPI.Model.DataEntities.Errands;
using OtisAPI.Model.DataEntities.Users;
using OtisAPI.Model.InputModels.Elevator;
using OtisAPI.Model.ViewModels.Elevator;
using OtisAPI.Model.ViewModels.Errands;
using OtisAPI.Model.ViewModels.Users;

namespace OtisAPI.Infrastructure;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<ElevatorEntity, ElevatorViewModel>();
        CreateMap<ElevatorInputModel, ElevatorEntity>();

        CreateMap<ErrandEntity, ErrandViewModel>();
        CreateMap<ErrandUpdateEntity, ErrandUpdateViewModel>();

        CreateMap<EmployeeEntity, EmployeeViewModel>();
    }
}