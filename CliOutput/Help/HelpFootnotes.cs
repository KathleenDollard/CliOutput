//using System.Collections;

namespace CliOutput.Help;

// I do not yet know how to do this - this should be a TextPart that
// appears in one or more places, an ID, plus a paragraph for the actual note.
// It will mess with PlainOutputWidth and wrapping.
public class HelpFootnotes(CliCommand command) 
    : HelpSection("Footnotes", command)
{
}