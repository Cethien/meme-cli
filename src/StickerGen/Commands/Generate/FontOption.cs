using System.CommandLine;

namespace StickerGen.Commands;

public sealed class FontOption : Option<string>
{
    public FontOption()
    : base(
            name: "--font",
            description: "which outdated font will be used"
        )
    {
        this.SetDefaultValue("impact");
    }
}