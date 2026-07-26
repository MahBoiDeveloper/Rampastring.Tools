using System;
using System.Collections.Generic;

namespace Rampastring.Tools.Ini;

/// <summary>
/// Serialization options struct.
/// </summary>
public struct IniSerializationOptions
{
    /// <summary>
    /// Name of the section that should be used in serialization.
    /// </summary>
    public string SectionName { get; set; }

    /// <summary>
    /// List of properties that should be ignored in serializing process.
    /// </summary>
    public List<string> IgnoreProperties { get; set; }

    /// <summary>
    /// Set true, if uniniatlized properties should be serialized.
    /// </summary>
    public bool WriteEmptyKeys;
}
