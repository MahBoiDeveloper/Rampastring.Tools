using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime;
using System.Text;

namespace Rampastring.Tools;

#nullable enable

public class IniSerializer
{
    /// <summary>
    /// 
    /// </summary>
    public static readonly IniSerializationOptions DefaultSerializationOptions = new() { Section = "Data", WriteEmptyKeys = true };

    /// <summary>
    /// 
    /// </summary>
    public static readonly IniDeserializationOptions DefaultDeserializationOptions = new() { Section = "Data"};

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ini"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static T? Deserialize<T>(IniFile ini, IniDeserializationOptions? options = null) => (T)Deserialize(ini, typeof(T), options);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="ini"></param>
    /// <param name="type"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static object? Deserialize(IniFile ini, Type type, IniDeserializationOptions? options = null)
    {
        IniDeserializationOptions settings = options ?? (DefaultDeserializationOptions with { Section = type.Name });
        object ret = Activator.CreateInstance(type);

        foreach (var property in type.GetProperties())
        {
            var propertyType = property.GetType();

            object value = propertyType.Name switch
            {
                nameof(String) => ini.GetStringValue(settings.Section, property.Name, string.Empty),
                nameof(Boolean) => ini.GetBooleanValue(settings.Section, property.Name, false),
                nameof(Int32) => ini.GetIntValue(settings.Section, property.Name, 0),
                nameof(Single) => ini.GetSingleValue(settings.Section, property.Name, (float)0.0),
                nameof(Double) => ini.GetDoubleValue(settings.Section, property.Name, 0.0),
                //nameof(List<String>) => ini.GetListValue<string>(settings.Section, property.Name, new char[',']),
                _ => throw new InvalidOperationException()
            };

            property.SetValue(ret, value);
        }

        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static string Serialize<T>(T data,  IniSerializationOptions? options = null) => Serialize(data, typeof(T), options);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="type"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static string Serialize(object data, Type type, IniSerializationOptions? options = null)
    {
        IniSerializationOptions settings = options ?? (DefaultSerializationOptions with { Section = type.Name });

        StringBuilder ret = new();
        ret.AppendLine($"[{type.Name}]");

        foreach (var property in type.GetProperties())
        {
            var propertyType = property.GetType();
            var propertyValue = property.GetValue(data);
            
            if (propertyValue == null)
            {
                continue;
            }
            {
                ret.AppendLine($"{property.Name}={propertyValue.ToString()}");
            }

        }

        return ret.ToString();
    }

}
