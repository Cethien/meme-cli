using System.CommandLine.Binding;
using SixLabors.Fonts;
using StickerGen.Services;

namespace StickerGen.Binders;

public sealed class ImageEditorBinder : BinderBase<ImageEditorService>
{
    private readonly FontCollection? _fonts;

    public ImageEditorBinder(FontCollection? fonts)
    {
        _fonts = fonts;
    }

    protected override ImageEditorService GetBoundValue(BindingContext bindingContext)
    {
        var srv = new ImageEditorService(_fonts);
        return srv;
    }
}