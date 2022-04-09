using System;
using System.ComponentModel;

namespace TernaryDiagramLib
{
    public abstract class DiagramElement : IDiagramElement, IDisposable
    {
        private IDiagramElement _parent;
        private object _tag;

        protected DiagramElement()
        {
        }

        internal DiagramElement(IDiagramElement parent)
        {
            this._parent = parent;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        public override bool Equals(object obj)
        {
            return this.EqualsInternal(obj);
        }

        internal virtual bool EqualsInternal(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        internal virtual void Invalidate()
        {
            if (this._parent != null)
            {
                this._parent.Invalidate();
            }
        }

        void IDiagramElement.Invalidate()
        {
            this.Invalidate();
        }

        public override string ToString()
        {
            return this.ToStringInternal();
        }

        internal virtual string ToStringInternal()
        {
            return base.GetType().Name;
        }

        internal virtual IDiagramElement Parent
        {
            get
            {
                return this._parent;
            }
            set
            {
                this._parent = value;
            }
        }

        IDiagramElement IDiagramElement.Parent
        {
            get
            {
                return this._parent;
            }
            set
            {
                this.Parent = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        [DefaultValue((string)null)]
        public object Tag
        {
            get
            {
                return this._tag;
            }
            set
            {
                this._tag = value;
            }
        }
    }
}
