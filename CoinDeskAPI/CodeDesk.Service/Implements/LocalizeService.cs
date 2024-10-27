using System.Reflection;
using CodeDesk.Service.Interfaces;
using CoinDesk.Model.Enum;
using Microsoft.Extensions.Localization;

namespace CodeDesk.Service.Implements;

public class LocalizeService : ILocalizeService
{
    private readonly IStringLocalizer _localizer;
    
    public LocalizeService(IStringLocalizerFactory factory)
    {
        var type = typeof(LocalizeService);
        var assemblyName = new System.Reflection.AssemblyName(type.GetTypeInfo().Assembly.FullName);
        _localizer = factory.Create("Resource", assemblyName.Name);
    }
    
    public string GetLocalizedString(LocalizeType type,  string key)
    {
        return _localizer.GetString($"{type.ToString()}_{key}");
    }
}