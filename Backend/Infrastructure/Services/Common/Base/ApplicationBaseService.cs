using Application.Common.Interfaces.Auth;
using Application.Common.Interfaces.Base;
using Application.Common.Interfaces.Localization;
using Application.Common.Interfaces.Persistence;
using AutoMapper;

namespace Infrastructure.Services.Common.Base;

public class ApplicationBaseService
{
    protected IServiceContext Context { get; }

    protected ApplicationBaseService(IServiceContext context)
    {
        Context = context;
    }

    protected IUnitOfWork UnitOfWork
        => Context.UnitOfWork;

    protected IMapper ObjectMapper
        => Context.Mapper;

    protected IUserContext UserContext
        => Context.UserContext;

    protected ILocalizationService Localization
        => Context.Localization;
}
