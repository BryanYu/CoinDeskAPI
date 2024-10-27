using System.ComponentModel;
using System.Reflection;
using CoinDesk.Model.Attribute;
using CoinDesk.Model.Enum;

namespace CoinDesk.Utility;

public static class EnumExtension
{
    public static string GetLocalizeKey(this ApiResponseStatus responseStatus)
    {
        var fieldInfo = responseStatus.GetType().GetField(responseStatus.ToString());
        var attribute = fieldInfo?.GetCustomAttribute<LocalizeKeyAttribute>();
        if (attribute is DescriptionAttribute)
        {
            return attribute.Key;
        }
        return responseStatus.ToString();
    }
}