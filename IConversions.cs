using System;

namespace Rampastring.Tools;

/// <summary>
/// Generic interface for Conversion classes.
/// </summary>
public interface IConversions
{
    /// <summary>
    /// Converts a string to the specific type.
    /// </summary>
    /// <param name="str"></param>
    /// <param name="type"></param>
    /// <returns>A value of the specific type.</returns>
    object ValueFromString(string str, Type type);
}
