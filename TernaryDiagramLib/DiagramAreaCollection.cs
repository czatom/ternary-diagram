using System.Collections.ObjectModel;
using System.Collections.Specialized;

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
