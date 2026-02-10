// Licensed to DocFX Companion Tools and contributors under one or more agreements.
// DocFX Companion Tools and contributors licenses this file to you under the MIT license.

using DocLanguageTranslator.FileService;

namespace DocLanguageTranslator.TranslationMode;

/// <summary>
/// Translation mode for translating a specific range of lines and inserting them
/// into an existing target file at the same position, without removing existing lines.
/// </summary>
internal class LineInsertTranslationMode : ITranslationMode
{
    private readonly int startLine;
    private readonly int endLine;

    /// <summary>
    /// Initializes a new instance of the <see cref="LineInsertTranslationMode"/> class.
    /// </summary>
    /// <param name="startLine">The 1-based starting line number to read from the source file.</param>
    /// <param name="endLine">The 1-based ending line number (inclusive) to read from the source file.</param>
    public LineInsertTranslationMode(int startLine, int endLine)
    {
        this.startLine = startLine;
        this.endLine = endLine;
    }

    /// <inheritdoc/>
    public string ReadContent(IFileService fileService, string inputFile)
    {
        string[] sourceLines = fileService.ReadLines(inputFile, startLine, endLine);
        return sourceLines.Length == 0 ? null : string.Join(Environment.NewLine, sourceLines);
    }

    /// <inheritdoc/>
    public void WriteContent(IFileService fileService, string outputFile, string translatedContent)
    {
        string[] translatedLines = translatedContent.Split(["\r\n", "\r", "\n"], StringSplitOptions.None);
        fileService.InsertLines(outputFile, startLine, translatedLines);
    }

    /// <inheritdoc/>
    public string FormatStartMessage(string inputFile, string outputFile, string sourceLang, string targetLang)
        => $"Translating lines {startLine}-{endLine} from {inputFile} and inserting into {outputFile} [{sourceLang} to {targetLang}]";

    /// <inheritdoc/>
    public string FormatCompletionMessage(string outputFile)
        => $"Inserted translated lines at position {startLine} in {outputFile}";

    /// <inheritdoc/>
    public string GetNoContentErrorMessage()
        => $"ERROR: No lines found in range {startLine}-{endLine}.";
}
