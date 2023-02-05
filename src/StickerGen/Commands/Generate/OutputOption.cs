using System.CommandLine;

namespace StickerGen.Commands;

public sealed class OutputOption : Option<string>
{
    public OutputOption()
    : base(
            name: "--output",
            description: "where to put your unfunny meme"
        )
    {
        this.SetDefaultValue($"./meme_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.png");
    }
}
