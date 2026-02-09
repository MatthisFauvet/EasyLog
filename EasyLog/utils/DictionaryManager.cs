namespace EasyLog.utils;

public class DictionaryManager
{
    public DictionaryManager()
    {
    }

    public static Dictionary<string, string> SingleStringToDictionary(string key, string value)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary.Add(key, value);
        return dictionary;
    }
}