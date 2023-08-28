namespace Infrastructure.ValueObjects;

public class DrawingData
{
    public int PrevX { get; set; }
    public int PrevY { get; set; }
    public int CurrentX { get; set; }
    public int CurrentY { get; set; }
    public string Color { get; set; }
    public UserInfo User { get; set; }
}