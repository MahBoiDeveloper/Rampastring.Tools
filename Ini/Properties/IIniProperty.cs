using Rampastring.Tools.Ini;

namespace Rampastring.Tools.Ini.INIProperties;

/// <summary>
/// An interface for INI properties.
/// </summary>
public interface IIniProperty
{
    void ParseValue(IniFile iniFile, string sectionName, string keyName);
}
