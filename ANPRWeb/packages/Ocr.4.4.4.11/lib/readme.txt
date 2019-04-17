C# Fully Featured OCR Example with Broad International Language Support:


//Multi-lingual OCR including language pack support for: 
// Arabic, Simplified Chinese, Traditional Chinese, English, French, German, Hebrew, Italian, Japanese, Korean, Portuguese, Russian and Spanish.
// BiLingual.Californian, BiLingual.Canadian, BiLingual.ChineseEnginering, BiLingual.JapaneseEnginering, BiLingual.ArabicEnginering


using IronOcr;
using IronOcr.Languages;
using IronOcr.Languages.BiLingual;

//..

var Ocr = new AdvancedOcr()
{
    CleanBackgroundNoise = true,
    EnhanceContrast = true,
    EnhanceResolution = true,
    Language = IronOcr.Languages.English.OcrLanguagePack,
    // to use multiple languages similtaneously 
    // Language = Languages.MultiLanguage.OcrLanguagePack(IronOcr.Languages.English.OcrLanguagePack, IronOcr.Languages.German.OcrLanguagePack)
    Strategy = IronOcr.AdvancedOcr.OcrStrategy.Advanced,
    ColorSpace = AdvancedOcr.OcrColorSpace.Color,
    DetectWhiteTextOnDarkBackgrounds = true,
    InputImageType = AdvancedOcr.InputTypes.AutoDetect,
    RotateAndStraighten = true,
    ReadBarCodes = true,
    ColorDepth =4
};

var testImage = @"C:\path\to\document\image.png";


var Results = Ocr.Read(testImage);

var Barcodes = Results.Barcodes.Select(b => b.Value);
var TextContent = Results.Text;

Console.WriteLine(TextContent);