
namespace TernaryDiagramLib
{
    internal interface IDiagramElement
    {
        void Invalidate();

        IDiagramElement Parent { get; set; }
    }
}
