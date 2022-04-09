using System.Collections.ObjectModel;

namespace TernaryDiagramLib
{
    public class DiagramAreaCollection : ObservableCollection<DiagramArea>
    {
        public DiagramArea Add(string name)
        {
            DiagramArea item = new DiagramArea(name);
            base.Add(item);
            return item;
        }
    }
}
