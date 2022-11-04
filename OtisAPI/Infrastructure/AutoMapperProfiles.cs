using AutoMapper;
using OtisAPI.Model.DataEntities.Elevators;
using OtisAPI.Model.DataEntities.Errands;
using OtisAPI.Model.DataEntities.Users;
using OtisAPI.Model.InputModels.Elevator;
using OtisAPI.Model.InputModels.Errands;
using OtisAPI.Model.ViewModels.Elevator;
using OtisAPI.Model.ViewModels.Errands;
using OtisAPI.Model.ViewModels.Users;

namespace OtisAPI.Infrastructure;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        //Elevator entities
        CreateMap<ElevatorEntity, ElevatorViewModel>().ReverseMap();
        CreateMap<ElevatorInputModel, ElevatorEntity>();
        CreateMap<UpdateElevatorInputModel, ElevatorEntity>();

        //Errand entities
        CreateMap<ErrandEntity, ErrandViewModel>().ReverseMap();
        CreateMap<ErrandUpdateEntity, ErrandUpdateViewModel>().ReverseMap();
        CreateMap<ErrandInputModel, ErrandEntity>();
        CreateMap<ErrandUpdateCreationModel, ErrandUpdateEntity>();
        CreateMap<ErrandUpdateInputModel, ErrandUpdateEntity>();

        //User entities
        CreateMap<EmployeeEntity, EmployeeViewModel>().ReverseMap();
    }
}