using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TernaryDiagramLib
{
    public interface IDiagramNamedElement : IDiagramElement
    {
        /// <summary>
        /// Name of the diagram element
        /// </summary>
        string Name { get; }
    }
}
