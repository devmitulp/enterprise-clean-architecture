namespace Application.Common.Interfaces.Localization
{
    public interface ILocalizationService
    {
        string L(string key, params object[] args);
    }
}
