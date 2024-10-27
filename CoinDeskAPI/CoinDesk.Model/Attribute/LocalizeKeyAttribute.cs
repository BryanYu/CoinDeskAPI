namespace CoinDesk.Model.Attribute;

public class LocalizeKeyAttribute : System.Attribute
{
    public string Key { get; set; }
    
    public LocalizeKeyAttribute(string key)
    {
        this.Key = key;
    }
}