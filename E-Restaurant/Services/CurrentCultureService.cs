using ERestaurant.Application.Common.Interfaces;
using System.Globalization;

namespace E_Restaurant.Services
{
    public class CurrentCultureService : ICurrentCultureService
    {
        public string GetCurrentLanguage()
        {
            return CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        }
    }

}
