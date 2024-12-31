namespace CapEnjoyer.BL.Constants;

public static class ImageConstants
{
    public const long MaxImageSize = 5L * 1024 * 1024; // 5 MB
    public static readonly string[] AllowedContentTypes = ["image/jpeg", "image/png"];
    public static string SharedPath = Path.Combine("..", "Shared");
}
