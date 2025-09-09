using ERestaurant.Application.Common.Interfaces;

namespace ERestaurant.Tests.Common
{
    public class FakeCultureService : ICurrentCultureService
    {
        public string GetCurrentLanguage() => "en";
    }
}
