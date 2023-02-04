using System.CommandLine;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

class Program
{
    private static FontCollection? _fonts;

    static async Task<int> Main(string[] args)
    {
        var fontDir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fonts");

        _fonts = new FontCollection();
        foreach (var ttf in Directory.GetFiles(fontDir))
        {
            _fonts.Add(ttf);
        }

        var imageSourceOption = new Option<string>(
            name: "--image",
            description: "your stupid template"
        )
        {
            IsRequired = true,
        };

        var textOption = new Option<string>(
            name: "--text",
            description: "your unfunny text"
        )
        {
            IsRequired = true,
        };

        var fontOption = new Option<string>(
            name: "--font",
            description: "which outdated font will be used"
        );
        fontOption.SetDefaultValue("impact");

        var outputOption = new Option<string>(
            name: "--output",
            description: "where to put your unfunny meme"
        )
        {
            IsRequired = true,
        };

        var generateCmd = new Command(
            name: "generate",
            description: "generates your bad meme"
        )
        {
            imageSourceOption,
            textOption,
            fontOption,
            outputOption
        };

        generateCmd.SetHandler(async (string imagePath, string texts, string fontFamily, string output) =>
            {
                await GenerateImageWithTextAsync(imagePath, texts.Split(";"), fontFamily, output);
            }, imageSourceOption, textOption, fontOption, outputOption);

        var rootCmd = new RootCommand(
            description: "generates some stupid memes"
        )
        {
            Name = "memer",
        };
        rootCmd.AddCommand(generateCmd);

        return await rootCmd.InvokeAsync(args.Count() > 0 ? args : new[] { "--help" });
    }


    static async Task GenerateImageWithTextAsync(string imagePath, string[] texts, string fontFamily, string output)
    {
        var image = Image.Load(imagePath);
        if (_fonts.TryGet(fontFamily, out var family))
        {
            var meme = GenerateImageWithText(image, texts, family);
            await meme.SaveAsPngAsync(output);
        }
    }
    static Image GenerateImageWithText(Image image, string[] texts, FontFamily family)
    {
        //resize image
        image.Mutate(x => x.Pad((int)(image.Width * 1.1), image.Height));

        //get text infos
        var fontSize = image.Height / 6f;
        var font = family.CreateFont(fontSize, FontStyle.Regular);

        var textOptions = new TextOptions(font)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
        };

        var drawingOptions = new DrawingOptions()
        {
            GraphicsOptions = new GraphicsOptions()
            {
                Antialias = true,
            }
        };

        for (int i = 0; i < texts.Count(); i++)
        {
            //convert to uppercase
            var text = texts[i].ToUpper();

            //get text block dimensions
            var rect = TextMeasurer.Measure(text, textOptions);

            //add padding height
            int imageH = (int)(image.Height * (1.05 + rect.Height / image.Height));
            image.Mutate(x => x.Pad(image.Width, imageH));

            //check if text is wider than image and resize image width
            var imageW = (int)(rect.Width * 1.05);
            if (imageW > image.Width)
            {
                image.Mutate(x => x.Pad(imageW, image.Height));
            }

            //calc text pos
            var x = image.Width / 2;
            var y = image.Height - rect.Height;
            textOptions.Origin = new PointF(x, y);

            //draw text
            var paths = TextBuilder.GenerateGlyphs(text, textOptions);
            paths.ToList().ForEach(p =>
            {
                var outline = p.GenerateOutline(10);
                image.Mutate(x =>
                {
                    x
                    .Fill(drawingOptions, brush: Brushes.Solid(color: Color.Black), outline)
                    .Fill(drawingOptions, Brushes.Solid(Color.White), p);
                });
            });
        }

        return image;
    }
}
