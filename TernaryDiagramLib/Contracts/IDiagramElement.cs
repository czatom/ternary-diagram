
namespace TernaryDiagramLib
{
    public interface IDiagramElement
    {
        void Invalidate();

        IDiagramElement Parent { get; set; }
    }
}
