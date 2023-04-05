using Microsoft.Extensions.Localization;

namespace AssignmentForMCСSA.Repositories.Abstract
{
    public interface ILanguage
    {
        LocalizedString GetKey(string key);
    }
}
