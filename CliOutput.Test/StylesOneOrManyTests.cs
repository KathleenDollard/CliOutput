// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CliOutput.Help;
using FluentAssertions;
using OutputEngine;
using OutputEngine.Primitives;
using OutputEngine.Renderers;

namespace CliOutput.Test;

public class StylesOneOrManyTests
{
    [Fact]
    public void Can_create_empty_style()
    {
        var styles = Styles.Empty;
        styles.Count.Should().Be(0);
    }

    [Fact]
    public void Can_add_style()
    {
        var paragraph = new Paragraph("Hello World");

        paragraph.AddStyle("style1");

        paragraph.Styles.Count.Should().Be(1);
    }

    [Fact]
    public void Can_add_multiple_styles()
    {
        var paragraph = new Paragraph("Hello World");

        paragraph.AddStyle("style1");
        paragraph.AddStyle("style2");

        paragraph.Styles.Count.Should().Be(2);
    }

    [Fact]
    public void Can_create_primitive_with_implicit_style()
    {

    }


    [Fact]
    public void Can_create_primitive_with_multiple_implicit_styles()
    {

    }


    [Fact]
    public void Can_create_with_style()
    {

    }


    [Fact]
    public void Can_create_with_multiple_styles()
    {

    }
}
