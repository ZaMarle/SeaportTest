using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using NetTopologySuite.IO.ShapeFile.Extended;
using Newtonsoft.Json;

public class ShapeFileHelper
{
    private readonly ILogger _logger;

    public ShapeFileHelper(ILogger logger)
    {
        _logger = logger;
    }

    public double GetAttributeValueSum(string attributeName, string shapeFileName)
    {
        double sum = 0;
        string rootDirectory = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(rootDirectory, $@"ShapeFile\{shapeFileName}.shp");
        
        ShapeDataReader reader = new(filePath);

        var mbr = reader.ShapefileBounds;
        var result = reader.ReadByMBRFilter(mbr);
        var coll = result.GetEnumerator();

        while (coll.MoveNext())
        {
            var feature = coll.Current;
            var attribute = feature.Attributes[attributeName];

            string? stringValue = attribute.ToString();

            if (!string.IsNullOrEmpty(stringValue))
            {
                _logger.Log(stringValue);
            } else {
                throw new Exception($"Unable to convert attribute: {attributeName} to string.");
            }

            try
            {
                double doubleValue = (double)attribute;
                sum += doubleValue;
            }
            catch (InvalidCastException)
            {
                throw new Exception($"Unable to convert attribute: {attributeName} to double.");
            }
        }

        return sum;
    }
}
