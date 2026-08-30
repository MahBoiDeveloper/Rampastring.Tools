// Rampastring's INI parser
// http://www.moddb.com/members/rampastring

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Rampastring.Tools.Ini;

/// <summary>
/// Represents a [section] in an INI file.
/// </summary>
public class IniSection : IIniSection
{
    public IniSection() { }

    public IniSection(string sectionName)
    {
        SectionName = sectionName;
    }

    public string SectionName { get; set; }
    public List<KeyValuePair<string, string>> Keys = new List<KeyValuePair<string, string>>();

    public void AddKey(string keyName, string value)
    {
        if (keyName == null || value == null)
            throw new ArgumentException("INI keys cannot have null key names or values.");

        if (Keys.FindIndex(kvp => kvp.Key == keyName) > -1)
            throw new InvalidOperationException("The given key already exists in the section!");

        Keys.Add(new KeyValuePair<string, string>(keyName, value));
    }

    public void AddOrReplaceKey(string keyName, string value)
    {
        if (keyName == null || value == null)
            throw new ArgumentException("INI keys cannot have null key names or values.");

        int index = Keys.FindIndex(k => k.Key == keyName);
        if (index > -1)
            Keys[index] = new KeyValuePair<string, string>(keyName, value);
        else
            Keys.Add(new KeyValuePair<string, string>(keyName, value));
    }

    public void RemoveKey(string keyName)
    {
        int index = Keys.FindIndex(k => k.Key == keyName);
        if (index > -1)
            Keys.RemoveAt(index);
    }

    public string GetStringValue(string key, string defaultValue)
    {
        var kvp = Keys.Find(k => k.Key == key);

        if (kvp.Value == null)
            return defaultValue;

        return kvp.Value;
    }

    public int GetIntValue(string key, int defaultValue)
    {
        return Conversions.IntFromString(GetStringValue(key, string.Empty), defaultValue);
    }

    public double GetDoubleValue(string key, double defaultValue)
    {
        return Conversions.DoubleFromString(GetStringValue(key, string.Empty), defaultValue);
    }

    public float GetSingleValue(string key, float defaultValue)
    {
        return Conversions.FloatFromString(GetStringValue(key, string.Empty), defaultValue);
    }

    /// <summary>
    /// Returns a value from the INI section.
    /// </summary>
    /// <param name="key">The name of the INI key.</param>
    /// <param name="type">The type of the value.</param>
    /// <returns></returns>
    public T GetValue<T>(string key)
    {
        return (T)GetValue(key, typeof(T));
    }

    /// <summary>
    /// Returns a value from the INI section.
    /// </summary>
    /// <param name="key">The name of the INI key.</param>
    /// <param name="type">The type of the value.</param>
    /// <returns></returns>
    public object GetValue(string key, Type type)
    {
        return Conversions.ValueFromString(GetStringValue(key, string.Empty), type);
    }

    /// <summary>
    /// Sets the string value of a key in the INI section.
    /// If the key doesn't exist, it is created.
    /// </summary>
    /// <param name="key">The name of the INI key.</param>
    /// <param name="value">The value of the INI key.</param>
    public void SetStringValue(string key, string value)
    {
        AddOrReplaceKey(key, value);
    }

    public void SetIntValue(string key, int value)
    {
        AddOrReplaceKey(key, value.ToString(CultureInfo.InvariantCulture));
    }

    public void SetDoubleValue(string key, double value)
    {
        AddOrReplaceKey(key, value.ToString(CultureInfo.InvariantCulture));
    }

    public void SetFloatValue(string key, float value)
    {
        AddOrReplaceKey(key, value.ToString(CultureInfo.InvariantCulture));
    }

    public void SetBooleanValue(string key, bool value)
    {
        SetBooleanValue(key, value, BooleanStringStyle.TRUEFALSE);
    }

    public void SetBooleanValue(string key, bool value, BooleanStringStyle booleanStringStyle)
    {
        string strValue = Conversions.BooleanToString(value, booleanStringStyle);
        AddOrReplaceKey(key, strValue);
    }

    public bool GetBooleanValue(string key, bool defaultValue)
    {
        return Conversions.BooleanFromString(GetStringValue(key, String.Empty), defaultValue);
    }

    public void SetListValue<T>(string key, List<T> list, char separator)
    {
        AddOrReplaceKey(key, string.Join(separator.ToString(), list));
    }

    /// <summary>
    /// Sets the value of the specific type.
    /// </summary>
    /// <param name="key">The name of the INI key.</param>
    /// <param name="value">The value of the INI key.</param>
    public void SetValue<T>(string key, object value)
    {
        SetValue(key, value, typeof(T));
    }

    /// <summary>
    /// Sets the value of the specific type.
    /// </summary>
    /// <param name="key">The name of the INI key.</param>
    /// <param name="value">The value of the INI key.</param>
    /// <param name="type">The type of the INI key.</param>
    public void SetValue(string key, object value, Type type)
    {
        switch (type.Name)
        {
            case (nameof(String)):
                SetStringValue(key, (String)(object)value);
                break;
            case (nameof(Boolean)):
                SetBooleanValue(key, (Boolean)(object)value);
                break;
            case (nameof(Int32)):
                SetIntValue(key, (Int32)(object)value);
                break;
            case (nameof(Single)):
                SetDoubleValue(key, (Single)(object)value);
                break;
            case (nameof(Double)):
                SetDoubleValue(key, (Double)(object)value);
                break;
            default:
                throw new ArgumentException($"Unable to get value of type {type.Name}.");
        }
    }

    /// <summary>
    /// Parses and returns a list value of a key in the INI section.
    /// </summary>
    /// <typeparam name="T">The type of the list elements.</typeparam>
    /// <param name="key">The INI key.</param>
    /// <param name="separator">The separator between the list elements.</param>
    /// <param name="converter">The function that converts the list elements from strings to the given type.</param>
    /// <returns>A list that contains the parsed elements.</returns>
    public List<T> GetListValue<T>(string key, char separator, Func<string, T> converter)
    {
        List<T> list = new List<T>();
        string value = GetStringValue(key, string.Empty);
        string[] parts = value.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string part in parts)
        {
            list.Add(converter(part));
        }
        return list;
    }

    public string GetPathStringValue(string key, string defaultValue)
    {
        return GetStringValue(key, defaultValue).Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
    }

    public T GetEnumValue<T>(string key, T defaultValue) where T : struct, Enum
    {
        string value = GetStringValue(key, null);
        if (string.IsNullOrWhiteSpace(value))
            return defaultValue;

        if (Enum.TryParse(value, true, out T result))
            return result;

        return defaultValue;
    }

    public void SetEnumValue<T>(string key, T value) where T : struct, Enum
    {
        SetStringValue(key, value.ToString());
    }

    /// <summary>
    /// Checks if the specified INI key exists in this section.
    /// </summary>
    /// <param name="key">The INI key.</param>
    /// <returns>True if the key exists in this section, otherwise false.</returns>
    public bool KeyExists(string key)
    {
        return Keys.FindIndex(k => k.Key == key) > -1;
    }

    /// <summary>
    /// Creates and returns a deep clone of this INI section.
    /// </summary>
    /// <param name="clonedSectionName">The name given to the cloned section.</param>
    public IniSection Clone(string clonedSectionName)
    {
        var clone = (IniSection)MemberwiseClone();
        clone.SectionName = clonedSectionName;

        // We don't need to do anything for the keys themselves because strings are immutable - just clone the list.
        clone.Keys = Keys.ToList();
        return clone;
    }
}
