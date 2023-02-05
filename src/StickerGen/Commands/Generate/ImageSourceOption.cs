using System.CommandLine;

namespace StickerGen.Commands;

public sealed class ImageSourceOption : Option<string>
{
    public ImageSourceOption()
    : base(
            name: "--image",
            description: "your stupid template"
        )
    {
        IsRequired = true;
    }
}
