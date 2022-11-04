using AutoMapper;
using OtisAPI.Infrastructure;

namespace OtisAPI.Test.Dependencies;

public class AutoMapperDependency
{
    public IMapper Mapper { get; private set; }

    public AutoMapperDependency()
    {
        var options = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AutoMapperProfiles());
        });

        Mapper = options.CreateMapper();
    }
}