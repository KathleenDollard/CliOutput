# Hello

This is a spike on help for System.CommandLine Powderhouse. This repo is not intended for use beyond this purpose, and I expect to archive it later. 

The basic goal is to have a reasonably easy way to layout help (or error reporting or user output) that can be sent to a renderer that understands output types. Expected output types are `terminal` (plain), `rich-terminal`, `markdown`, and `html`. This is the effort we are working on in this repo.

Our current stance is that machine readable output - JSON, XML, .CSV is a different problem and will probably be solved by an export object model that contains everything we know (core tree plus subsystem layer annotations). This can be serialized and output in different ways, possibly by other tools. This is out of scope, but this is the explanation of why this is not in the current work.