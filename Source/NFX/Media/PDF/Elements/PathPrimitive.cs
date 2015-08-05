namespace NFX.Media.PDF.Elements
{
  /// <summary>
  /// PDF path primitive (a line, Bezier curve,...) as a part of path
  /// </summary>
  public abstract class PathPrimitive
  {
    /// <summary>
    /// Returns PDF string representation on the primitive
    /// </summary>
    /// <returns></returns>
    public abstract string ToPdfString();
  }
}