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

        public ICurrentUserContext UserContext { get; }

        public ILocalizationService Localization { get; }

        public ServiceContext(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentUserContext userContext,
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