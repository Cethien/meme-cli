using System.CommandLine;
using SixLabors.Fonts;

namespace StickerGen.Commands;

public sealed class RootCmd : RootCommand
{
    public RootCmd(FontCollection fonts)
    : base(
        description: "generates some unfunny memes"
        )
    {
        AddCommand(new GenerateCmd(fonts));
    }
}