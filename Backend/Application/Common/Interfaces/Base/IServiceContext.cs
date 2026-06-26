using Application.Common.Interfaces.Auth;
using Application.Common.Interfaces.Localization;
using Application.Common.Interfaces.Persistence;
using AutoMapper;

namespace Application.Common.Interfaces.Base
{
    public interface IServiceContext
    {
        IUnitOfWork UnitOfWork { get; }

        IMapper Mapper { get; }

        IUserContext UserContext { get; }

        ILocalizationService Localization { get; }

    }
}