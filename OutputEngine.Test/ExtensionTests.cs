// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using OutputEngine;

namespace CliOutput.Test;

public class ExtensionTests
{
    [Theory]
    [InlineData(100, "The quick brown fox jumps over the lazy dog", "The quick brown fox jumps over the lazy dog")]
    [InlineData(44, "The quick brown fox jumps over the lazy dog", "The quick brown fox jumps over the lazy dog")]
    [InlineData(43, "The quick brown fox jumps over the lazy dog", "The quick brown fox jumps over the lazy dog")]
    [InlineData(42, "The quick brown fox jumps over the lazy dog", "The quick brown fox jumps over the lazy|dog")]
    [InlineData(41, "The quick brown fox jumps over the lazy dog", "The quick brown fox jumps over the lazy|dog")]
    [InlineData(40, "The quick brown fox jumps over the lazy dog", "The quick brown fox jumps over the lazy|dog")]
    [InlineData(39, "The quick brown fox jumps over the lazy dog", "The quick brown fox jumps over the lazy|dog")]
    [InlineData(10, "The quick brown fox jumps over the lazy dog", "The quick|brown fox|jumps over|the lazy|dog")]
    [InlineData(9, "The quick brown fox jumps over the lazy dog", "The quick|brown fox|jumps|over the|lazy dog")]
    [InlineData(8, "The quick brown fox jumps over the lazy dog", "The|quick|brown|fox|jumps|over the|lazy dog")]
    [InlineData(5, "The quick brown fox jumps over the lazy dog", "The|quick|brown|fox|jumps|over|the|lazy|dog")]
    [InlineData(4, "The quick brown fox jumps over the lazy dog", "The|quic|k|brow|n|fox|jump|s|over|the|lazy|dog")]
    [InlineData(3, "The quick brown fox jumps over the lazy dog", "The|qui|ck|bro|wn|fox|jum|ps|ove|r|the|laz|y|dog")]
    [InlineData(2, "The quick brown fox jumps over the lazy dog", "Th|e|qu|ic|k|br|ow|n|fo|x|ju|mp|s|ov|er|th|e|la|zy|do|g")]
    [InlineData(1, "The quick brown fox jumps over the lazy dog", "T|h|e|q|u|i|c|k|b|r|o|w|n|f|o|x|j|u|m|p|s|o|v|e|r|t|h|e|l|a|z|y|d|o|g")]
    //[InlineData(100, "The quick brown fox jumps over the lazy dog", "The quick brown fox jumps over the lazy dog")]
    public void Outputs_string(int width, string input, string expected)
    {
        var actualLines = input.Wrap(width);
        var actual = string.Join("|", actualLines);

        actual.Should().Be(expected);
    }


}