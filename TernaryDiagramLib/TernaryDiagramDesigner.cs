using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace TernaryDiagramLib
{
    /// <summary>
    /// Chart windows forms control designer
    /// </summary>
    internal class TernaryDiagramDesigner : ControlDesigner
    {
        #region Methods

        /// <summary>
        /// Initialize designer.
        /// </summary>
        /// <param name="component">Component.</param>
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
        }

        protected override void OnMouseDragBegin(int x, int y)
        {
            base.OnMouseDragBegin(x, y);
        }

        /// <summary>
        /// Set default values for properties of the component.
        /// NOTE: Replaces obsolete method: OnSetComponentDefaults()
        /// </summary>
        /// <param name="defaultValues">Default values property bags.</param>
        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            if (Control != null && Control is TernaryDiagram)
            {
                TernaryDiagram diagram = (TernaryDiagram)Control;
                // If control is not initialized
                if (diagram.DiagramAreas.Count == 0)
                {
                    // Add Default diagram area
                    diagram.DiagramAreas.Add(new DiagramArea());
                }
            }
            base.InitializeNewComponent(defaultValues);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free managed resources
                SystemEvents.UserPreferenceChanged -= SystemEvents_UserPreferenceChanged;
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// User changed Windows preferences
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">Event arguments.</param>
        private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            // If user changed system colors, make diagram repaint itself.
            if (e.Category == UserPreferenceCategory.Color)
                Control.Invalidate();
        }
        #endregion
    }
}
