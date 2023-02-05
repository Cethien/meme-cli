using System.CommandLine;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using StickerGen.Binders;
using StickerGen.Services;

namespace StickerGen.Commands;

public sealed class GenerateCmd : Command
{
    private readonly FontCollection? _fonts;
    private readonly Dictionary<string, Option<string>> _options;



    public GenerateCmd(FontCollection? fonts)
    : base(
            name: "generate",
            description: "generates your bad meme"
        )
    {
        //set fonts
        _fonts = fonts;

        //get options
        _options = new Dictionary<string, Option<string>>()
        {
            {"image", new ImageSourceOption()},
            {"text", new TextOption()},
            {"output", new OutputOption()},
            {"font", new FontOption()},
        };
        _options.ToList().ForEach(x => AddOption(x.Value));

        //setup handler
        this.SetHandler(Handle,
            _options["image"],
            _options["text"],
            _options["font"],
            _options["output"],
            new ImageEditorBinder(_fonts)
            );
    }

    private async Task Handle(string imagePath, string texts, string fontFamily, string output, ImageEditorService editor)
    {
        try
        {
            var image = Image.Load(imagePath);
            if (_fonts.TryGet(fontFamily, out var family))
            {
                var meme = editor.GenerateImageWithText(image, texts.Split(";"), family);
                await meme.SaveAsPngAsync(output);
            }
        }
        catch (System.Exception ex)
        {
            System.Console.WriteLine(ex.Message);
        }
    }
}
