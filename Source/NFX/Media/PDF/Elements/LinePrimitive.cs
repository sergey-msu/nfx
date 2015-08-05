using NFX.Media.PDF.Text;

namespace NFX.Media.PDF.Elements
{  
  /// <summary>
  /// PDF line primitive as a part of path
  /// </summary>
  public class LinePrimitive : PathPrimitive
  {
    public LinePrimitive()
    {
    }

    public LinePrimitive(float endX, float endY)
    {
      EndX = endX;
      EndY = endY;
    }

    /// <summary>
    /// Line's end X coordinate
    /// </summary>
    public float EndX { get; set; }
                               
    /// <summary>
    /// Line's end Y coordinate
    /// </summary>
    public float EndY { get; set; }

    /// <summary>
    /// Returns PDF string representation on the line primitive
    /// </summary>
    /// <returns></returns>
    public override string ToPdfString()
    {
      return string.Format("{0} {1} l", 
                          TextAdapter.FormatFloat(EndX), 
                          TextAdapter.FormatFloat(EndY));
    }
  }
}