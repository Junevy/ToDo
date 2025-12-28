public interface ILocalizationService
{
    /// <summary>
    /// Current Culture
    /// </summary>
    string CurrentCulture { get; }

    /// <summary>
    /// Switch Culture
    /// </summary>
    /// <param name="cultureName">culture name, such as "en-US", "中文-简体" etc.</param>
    void SetCulture(string cultureName);
}




