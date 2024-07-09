using  MapApplication.Data;
namespace MapApplication.Models;

public class Response
{
    public List<PointDb> point { get; set; }
    public string ResponseMessage { get; set; }
    public bool success { get; set; }
}