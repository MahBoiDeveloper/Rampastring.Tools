using System;
using System.Collections.Generic;

namespace Rampastring.Tools;

/// <summary>
/// 
/// </summary>
public struct IniDeserializationOptions
{
    /// <summary>
    /// 
    /// </summary>
    public string SectionName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<string> IgnoreProperties { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool SkipEmptyKeys;
}
