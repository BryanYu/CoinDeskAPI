using CoinDesk.Model.Enum;

namespace CodeDesk.Service.Interfaces;

public interface ILocalizeService
{
    string GetLocalizedString(LocalizeType type, string key);
}