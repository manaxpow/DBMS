public record ExportFileResponse(
    byte[] Content,
    string ContentType,
    string FileName
);
