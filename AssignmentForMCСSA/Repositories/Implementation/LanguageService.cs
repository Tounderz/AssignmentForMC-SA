using AssignmentForMCСSA.Repositories.Abstract;
using AssignmentForMCСSA.Resources;
using Microsoft.Extensions.Localization;
using System.Reflection;

namespace AssignmentForMCСSA.Repositories.Implementation
{
    public class LanguageService : ILanguage
    {
        private readonly IStringLocalizer _localizer;

        public LanguageService(IStringLocalizerFactory factory)
        {
            var type = typeof(SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create("SharedResource", assemblyName.Name);
        }

        public LocalizedString GetKey(string key)
        {
            return _localizer[key];
        }
    }
}
