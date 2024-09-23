// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace OutputEngine;

/// <summary>
/// Colors for console output. 
/// </summary>
/// <remarks>
/// Initially these are the colors supported by the Windows console.
/// But we expect more colors to be used by some CLI/renderer combinations, 
/// and thus the string constant approach because it is extensible.
/// </remarks>
public static class OutputColor
{
    public const string Black = "Black";
    public const string DarkRed = "DarkRed";
    public const string DarkGreen = "DarkGreen";
    public const string DarkYellow = "DarkYellow";
    public const string DarkBlue = "DarkBlue";
    public const string DarkMagenta = "DarkMagenta";
    public const string DarkCyan = "DarkCyan";
    public const string Gray = "Gray";
    public const string DarkGray = "DarkGray";
    public const string Red = "Red";
    public const string Green = "Green";
    public const string Yellow = "Yellow";
    public const string Blue = "Blue";
    public const string Magenta = "Magenta";
    public const string Cyan = "Cyan";
    public const string White = "White";
}
