using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rampastring.Tools.Extensions;

/// <summary>
/// 
/// </summary>
public static class StringExtension
{
    public static Stream ToStream(this string text)
    {
        // https://stackoverflow.com/questions/1879395/how-do-i-generate-a-stream-from-a-string
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(text);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
}
