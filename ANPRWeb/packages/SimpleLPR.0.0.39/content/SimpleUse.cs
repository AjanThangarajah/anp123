using System.Collections.Generic;
using System.Drawing;

public static class LPR
{
    public static void DummyTest()
    {
        var PlateFrame = new System.Drawing.Bitmap(@"LicensePlate.bmp");

        string Plate = LPR.RecognizeLicensePlate(PlateFrame);

	System.Diagnostics.Debug.WriteLine(Plate);
        //System.Console.WriteLine(Plate);
        //System.Console.ReadLine();
    }

    private static string productKey = null;
    public static string RecognizeLicensePlate(Bitmap PhotoOrCameraFrame, string Country_id = "Brazil")
    {
        string result = "";

        using (var _lpr = SimpleLPR2.SimpleLPR.Setup())
        using (var proc = _lpr.createProcessor())
        {
            for (uint ui = 0; ui < _lpr.numSupportedCountries; ++ui)
                _lpr.set_countryWeight(ui, 0.0f);

            _lpr.set_countryWeight(Country_id, 1.0f);
            _lpr.realizeCountryWeights();

            // Set the product key (if any)
            if (productKey != null) _lpr.set_productKey(productKey);

            List<SimpleLPR2.Candidate> cds = proc.analyze(PhotoOrCameraFrame, 120); /*maximum char height*/
            // Look for license plates
            //Title = cds.Count().ToString();

            if (cds.Count > 0 && cds[0].confidence >= 0.85)
                result = cds[0].text;
            else
                result = null;

            cds = null;

            return result;
        }
    }
}