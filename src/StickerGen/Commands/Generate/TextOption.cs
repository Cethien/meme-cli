using System.CommandLine;

namespace StickerGen.Commands;

public sealed class TextOption : Option<string>
{
    public TextOption()
    : base(
            name: "--text",
            description: "your unfunny text"
        )
    {
        IsRequired = true;
    }
}
