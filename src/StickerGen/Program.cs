using System.CommandLine;
using SixLabors.Fonts;
using StickerGen.Commands;

class Program
{
    static async Task<int> Main(string[] args)
    {
        //get fonts from folder
        var fontDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "fonts");
        var fonts = new FontCollection();
        Directory.GetFiles(fontDir).ToList().ForEach(x => { fonts.Add(x); });

        var rootCmd = new RootCmd(fonts);
        return await rootCmd.InvokeAsync(args.Count() > 0 ? args : new[] { "--help" });
    }



}
