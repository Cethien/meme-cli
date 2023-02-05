using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

namespace StickerGen.Services;

public sealed class ImageEditorService
{
    private readonly FontCollection? _fonts;

    public ImageEditorService(FontCollection? fonts)
    {
        _fonts = fonts;
    }

    public Image GenerateImageWithText(Image image, string[] texts, FontFamily family)
    {
        //set image size if image is too small
        var size = ResizeKeepAspect(image.Size(), 800);
        image.Mutate(x => x.Resize(size));

        //repad image
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

        // draw text
        DrawTexts(image, texts, textOptions, drawingOptions);

        return image;
    }

    private void DrawTexts(Image image, string[] texts, TextOptions textOptions, DrawingOptions drawingOptions)
    {
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

    }

    private Size ResizeKeepAspect(Size src, int max)
    {
        var maxWidth = max;
        var maxHeight = max;

        if (src.Width > src.Height)
        {

        }
        else
        {

        }

        return new Size(maxWidth, maxHeight);
    }
}