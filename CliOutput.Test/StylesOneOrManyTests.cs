// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using OutputEngine;
using OutputEngine.Primitives;

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

        paragraph.Styles.ToArray().Should().BeEquivalentTo(["style1"]);
    }

    [Fact]
    public void Can_add_multiple_styles()
    {
        var paragraph = new Paragraph("Hello World");

        paragraph.AddStyle("style1");
        paragraph.AddStyle("style2");

        paragraph.Styles.ToArray().Should().BeEquivalentTo(["style1", "style2"]);
    }

    [Fact]
    public void Can_create_primitive_with_implicit_style()
    {
        var codeInline = new CodeInline("Hello World");

        codeInline.Styles.ToArray().Should().BeEquivalentTo(["CodeInline"]);
    }

    [Fact]
    public void Can_create_TextPart_with_style()
    {
        var textPart = new TextPart("Hello World", "style1");

        textPart.Styles.ToArray().Should().BeEquivalentTo(["style1"]);
    }

    [Fact]
    public void Can_add_style_where_one_exists()
    {
        var textPart = new TextPart("Hello World", "style1");
        textPart.AddStyle("style2");

        textPart.Styles.ToArray().Should().BeEquivalentTo(["style1", "style2"]);
    }

    [Fact]
    public void Does_not_add_style_that_exists_as_one()
    {
        var textPart = new TextPart("Hello World", "style1");
        textPart.AddStyle("style1");

        textPart.Styles.ToArray().Should().BeEquivalentTo(["style1"]);
    }


    [Fact]
    public void Does_not_add_style_that_exists_as_many()
    {
        var textPart = new TextPart("Hello World", "style1");
        textPart.AddStyle("style2");
        textPart.AddStyle("style2");

        textPart.Styles.ToArray().Should().BeEquivalentTo(["style1", "style2"]);
    }

    [Fact]
    public void Can_reset_styles_to_implicit()
    {
        var codeInline = new CodeInline("Hello World");
        codeInline.AddStyle("style2");
        codeInline.Styles.ToArray().Should().BeEquivalentTo(["CodeInline", "style2"]);

        codeInline.ResetStyles();
        codeInline.Styles.ToArray().Should().BeEquivalentTo(["CodeInline"]);
    }

    [Fact]
    public void Can_reset_styles_to_empty()
    {
        var textPart = new TextPart("Hello World", "style1");
        textPart.AddStyle("style2");
        textPart.Styles.ToArray().Should().BeEquivalentTo(["style1", "style2"]);

        textPart.ResetStyles();
        textPart.Styles.ToArray().Should().BeEmpty();
    }
}
