using Application.Common.Interfaces.Auth;
using Application.Common.Interfaces.Base;
using Application.Common.Interfaces.Localization;
using Application.Common.Interfaces.Persistence;
using AutoMapper;

namespace Infrastructure.Services.Common.Base
{
    public sealed class ServiceContext : IServiceContext
    {
        public IUnitOfWork UnitOfWork { get; }

        public IMapper Mapper { get; }

        public IUserContext UserContext { get; }

        public ILocalizationService Localization { get; }

        public ServiceContext(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IUserContext userContext,
            ILocalizationService localization
            )
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
            UserContext = userContext;
            Localization = localization;
        }
    }
}