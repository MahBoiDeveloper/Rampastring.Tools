using System;
using System.Text;
using Rampastring.Tools.Extensions;

namespace Rampastring.Tools.Ini;

/// <summary>
/// Provides functionality to serialize objects or value types to INI and to deserialize INI into objects or value types.
/// </summary>
public class IniSerializer(IConversions converter)
{
    /// <summary>
    /// Default options used in serialization.
    /// </summary>
    public static readonly IniSerializationOptions DefaultSerializationOptions = new()
    {
        SectionName = "Data",
        IgnoreProperties = [],
        WriteEmptyKeys = true
    };

    /// <summary>
    /// Default options used in deserialization.
    /// </summary>
    public static readonly IniDeserializationOptions DefaultDeserializationOptions = new()
    {
        SectionName = "Data",
        IgnoreProperties = [],
        SkipUnableToParseTypes = true,
        SkipEmptyKeys = true
    };

    #region Deserialization
    /// <summary>
    /// Deserializes string as ini file to the object of the specific class.
    /// </summary>
    public T Deserialize<T>(string iniFileContent, IniDeserializationOptions? options = null)
    {
        return (T)Deserialize(new IniFile(iniFileContent.ToStream()), typeof(T), options);
    }

    /// <summary>
    /// Deserializes string as ini file to the object of the specific class.
    /// </summary>
    public object Deserialize(string iniFileContent, Type type, IniDeserializationOptions? options = null)
    {
        return Deserialize(new IniFile(iniFileContent.ToStream()), type, options);
    }

    /// <summary>
    /// Deserializes ini file to the object of the specific class.
    /// </summary>
    public T Deserialize<T>(IniFile ini, IniDeserializationOptions? options = null)
    {
        return (T)Deserialize(ini, typeof(T), options);
    }

    /// <summary>
    /// Deserializes ini file to the object of the specific class.
    /// </summary>
    public object Deserialize(IniFile ini, Type type, IniDeserializationOptions? options = null)
    {
        IniDeserializationOptions settings = options ?? (DefaultDeserializationOptions with { SectionName = type.Name });

        var section = ini.GetSection(settings.SectionName);

        if (section == null)
            throw new IniSerializerException($"Unable to find \"{settings.SectionName}\" section in provided ini file instance.");

        return DeserializeSection(ini.GetSection(settings.SectionName), type, settings);
    }

    /// <summary>
    /// Deserializes ini section to the object of the specific class.
    /// </summary>
    public T Deserialize<T>(IniSection section, IniDeserializationOptions? options = null)
    {
        return (T)DeserializeSection(section, typeof(T), options);
    }

    /// <summary>
    /// Deserializes ini section to the object of the specific class.
    /// </summary>
    public object Deserialize(IniSection section, Type type, IniDeserializationOptions options)
    {
        return DeserializeSection(section, type, options);
    }

    private object DeserializeSection(IniSection section, Type type, IniDeserializationOptions? options)
    {
        var settings = options ?? DefaultDeserializationOptions;

        object ret = Activator.CreateInstance(type);

        foreach (var property in type.GetProperties())
        {
            if (settings.IgnoreProperties.Contains(property.Name))
                continue;

            string value = section.GetStringValue(property.Name, string.Empty);

            if (settings.SkipEmptyKeys && string.IsNullOrEmpty(value))
                continue;

            try
            {
                property.SetValue(ret, converter.ValueFromString(value, property.PropertyType));
            }
            catch (ArgumentException ex)
            {
                if (settings.SkipUnableToParseTypes)
                    continue;
                else
                    throw ex;
            }
        }

        return ret;
    }
    #endregion

    #region Serialization
    /// <summary>
    /// Serializes class to ini-formated string.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public string Serialize<T>(T data, IniSerializationOptions? options = null)
    {
        return Serialize(data, typeof(T), options);
    }

    /// <summary>
    /// Serializes class to ini-formated string.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="type"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public string Serialize(object data, Type type, IniSerializationOptions? options = null)
    {
        IniSerializationOptions settings = options ?? (DefaultSerializationOptions with { SectionName = type.Name });

        StringBuilder ret = new();
        ret.AppendLine($"[{settings.SectionName}]");

        foreach (var property in type.GetProperties())
        {
            if (settings.IgnoreProperties.Contains(property.Name))
                continue;

            var propertyType = property.GetType();
            var propertyValue = property.GetValue(data);

            if (!settings.WriteEmptyKeys && propertyValue == null)
                continue;

            ret.AppendLine($"{property.Name}={(propertyValue ?? string.Empty).ToString()}");
        }

        return ret.ToString();
    }
    #endregion
}
