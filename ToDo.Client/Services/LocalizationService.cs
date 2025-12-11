using System.Globalization;
using ToDo.Extensions.Resx;

namespace ToDo.Client.Services
{
    class LocalizationService : ILocalizationService
    {
        public string CurrentCulture { get; private set; } = "en-US";

        public void SetCulture(string cultureName)
        {
            var culture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            CurrentCulture = cultureName;
            ResxExtension.UpdateTargets();
        }
    }
}
