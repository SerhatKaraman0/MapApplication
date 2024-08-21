using MapApplication.Data;

namespace MapApplication.Models;

public class Point
{
    public int Id { get; set; }
    public double X_coordinate { get; set; }
    public double Y_coordinate { get; set; }
    public string Name { get; set; }
    public string Date { get; set; }
    public List<FeatureDb> Features { get; set; }
    public int OwnerId { get; set; }


}

