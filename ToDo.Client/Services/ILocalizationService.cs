public interface ILocalizationService
{
    void SetCulture(string cultureName);
    string CurrentCulture { get; }
}




