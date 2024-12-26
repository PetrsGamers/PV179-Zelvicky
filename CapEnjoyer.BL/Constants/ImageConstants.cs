namespace CapEnjoyer.BL.Constants;

public static class ImageConstants
{
    public const long MaxImageSize = 5L * 1024 * 1024; // 5 MB
    public const string CapImageFolder = @"wwwroot\images\caps";
    public const string BottleImageFolder = @"wwwroot\images\bottles";
    public static readonly string[] AllowedContentTypes = ["image/jpeg", "image/png"];
}
