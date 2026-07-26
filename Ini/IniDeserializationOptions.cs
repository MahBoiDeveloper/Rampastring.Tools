using System.Collections.Generic;

namespace Rampastring.Tools.Ini;

/// <summary>
/// Deserialization options struct.
/// </summary>
public struct IniDeserializationOptions
{
    /// <summary>
    /// Name of the section that should be used in deserialization.
    /// </summary>
    public string SectionName { get; set; }

    /// <summary>
    /// List of properties that should be ignored in serializing process.
    /// </summary>
    public List<string> IgnoreProperties { get; set; }

    /// <summary>
    /// If type is unable to parse, serializer skips it.
    /// </summary>
    public bool SkipUnableToParseTypes { get; set; }

    /// <summary>
    /// Set to true to prevent empty keys from being processed.
    /// </summary>
    public bool SkipEmptyKeys;
}
