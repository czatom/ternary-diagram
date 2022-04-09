using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace TernaryDiagramLib
{
    class AxesCollectionEditor : ArrayEditor
    {
        public AxesCollectionEditor(Type type) : base(typeof(Axis[]))
        {
            this._helpTopic = "";
        }

        private bool _button_EnabledChanging;
        private CollectionEditor.CollectionForm _form;
        private string _helpTopic;

        protected override bool CanRemoveInstance(object value)
        {
            return false;
        }

        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof(Axis);
        }


        private void Button_EnabledChanged(object sender, EventArgs e)
        {
            if (!this._button_EnabledChanging)
            {
                this._button_EnabledChanging = true;
                try
                {
                    ((Button)sender).Enabled = false;
                }
                finally
                {
                    this._button_EnabledChanging = false;
                }
            }
        }

        private void CollectButtons(ArrayList buttons, Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control is Button)
                {
                    buttons.Add(control);
                }
                if (control.Controls.Count > 0)
                {
                    this.CollectButtons(buttons, control.Controls);
                }
            }
        }

        protected override CollectionEditor.CollectionForm CreateCollectionForm()
        {
            this._form = base.CreateCollectionForm();
            PropertyGrid propertyGrid = this.GetPropertyGrid(this._form.Controls);
            if (propertyGrid != null)
            {
                propertyGrid.HelpVisible = true;
                propertyGrid.CommandsVisibleIfAvailable = true;
                propertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(this.OnPropertyChanged);
                propertyGrid.ControlAdded += new ControlEventHandler(this.OnControlAddedRemoved);
                propertyGrid.ControlRemoved += new ControlEventHandler(this.OnControlAddedRemoved);
            }

            ArrayList buttons = new ArrayList();
            this.CollectButtons(buttons, this._form.Controls);
            foreach (Button button in buttons)
            {
                if (button.DialogResult == DialogResult.OK)
                {
                    button.Click += new EventHandler(this.OnOkClicked);
                }
                if ((button.Name.StartsWith("add", StringComparison.OrdinalIgnoreCase) || button.Name.StartsWith("remove", StringComparison.OrdinalIgnoreCase)) || (button.Text.Length == 0))
                {
                    button.Enabled = false;
                    button.EnabledChanged += new EventHandler(this.Button_EnabledChanged);
                }
            }
            return this._form;
        }

        private PropertyGrid GetPropertyGrid(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                PropertyGrid propertyGrid = control as PropertyGrid;
                if (propertyGrid != null)
                {
                    return propertyGrid;
                }
                if (control.Controls.Count > 0)
                {
                    propertyGrid = this.GetPropertyGrid(control.Controls);
                    if (propertyGrid != null)
                    {
                        return propertyGrid;
                    }
                }
            }
            return null;
        }

        private void OnControlAddedRemoved(object sender, ControlEventArgs e)
        {
            //TODO
        }

        private void OnOkClicked(object sender, EventArgs e)
        {
            this._helpTopic = "";
        }

        private void OnPropertyChanged(object sender, PropertyValueChangedEventArgs e)
        {

        }

        protected override void ShowHelp()
        {
            this._helpTopic = "";
            PropertyGrid propertyGrid = this.GetPropertyGrid(this._form.Controls);
            if (propertyGrid != null)
            {
                GridItem selectedGridItem = propertyGrid.SelectedGridItem;
                if ((selectedGridItem != null) && ((selectedGridItem.GridItemType == GridItemType.Property) || (selectedGridItem.GridItemType == GridItemType.ArrayValue)))
                {
                    this._helpTopic = selectedGridItem.PropertyDescriptor.ComponentType.ToString() + "." + selectedGridItem.PropertyDescriptor.Name;
                }
            }
            base.ShowHelp();
            this._helpTopic = "";
        }

        protected override string HelpTopic
        {
            get
            {
                if (this._helpTopic.Length != 0)
                {
                    return this._helpTopic;
                }
                return base.HelpTopic;
            }
        }

        protected override string GetDisplayText(object value)
        {
            return value.ToString();
        }
    }
}
