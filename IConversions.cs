using System;

namespace Rampastring.Tools;

/// <summary>
/// 
/// </summary>
public interface IConversions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="str"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    object ValueFromString(string str, Type type);
}
