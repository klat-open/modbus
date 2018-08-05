using System;
using System.Drawing;
using System.Windows.Forms;
using TQElectronic.Utils.Limitation;

namespace TQElectronic.Utils.WinForms
{
    public sealed class DynamicDock : IDisposable
    {
        private readonly IThrottler<int> _throttler;
        private readonly Control _child;
        private readonly Control _parent;
        private DockStyle _dockStyle;
        private bool _useMargin;

        public static DynamicDock Register(Control child)
        {
            DockStyle dock = child.Dock;
            return new DynamicDock(child) { DockStyle = dock };
        }

        public DockStyle DockStyle
        {
            get
            {
                return this._dockStyle;
            }
            set
            {
                this._dockStyle = value;
                this.UpdateChild();
            }
        }

        public bool UseMargin
        {
            get
            {
                return this._useMargin;
            }
            set
            {
                this._useMargin = value;
                this.UpdateChild();
            }
        }

        public DynamicDock(Control child)
          : this(child, child.Parent)
        {
        }

        public DynamicDock(Control child, Control parent)
          : this(child, parent, 100)
        {
        }

        public DynamicDock(Control child, Control parent, int timeoutInMilliseconds)
        {
            Control control1 = child;
            if (control1 == null)
                throw new ArgumentNullException(nameof(child));
            this._child = control1;
            Control control2 = parent;
            if (control2 == null)
                throw new ArgumentNullException(nameof(parent));
            this._parent = control2;
            this._throttler = (IThrottler<int>)new TimeoutThrottler<int>((Action<int>)(state => this._parent.Invoke((Delegate)new MethodInvoker(this.UpdateChild))), timeoutInMilliseconds);
            this.InitializeChildDockState();
            this.WireUpParentEvents();
        }

        private void InitializeChildDockState()
        {
            Point location = this._child.Location;
            Size size = this._child.Size;
            this._child.Dock = DockStyle.None;
            this._child.Location = location;
            this._child.Size = size;
        }

        private void WireUpParentEvents()
        {
            this._parent.Resize += new EventHandler(this._parent_Resize);
        }

        private void _parent_Resize(object sender, EventArgs e)
        {
            this._throttler.Execute(0);
        }

        private void UpdateChild()
        {
            switch (this._dockStyle)
            {
                case DockStyle.None:
                    break;

                case DockStyle.Top:
                    this.UpdateDockTop();
                    break;

                case DockStyle.Bottom:
                    this.UpdateDockBottom();
                    break;

                case DockStyle.Left:
                    this.UpdateDockLeft();
                    break;

                case DockStyle.Right:
                    this.UpdateDockRight();
                    break;

                case DockStyle.Fill:
                    this.UpdateDockFill();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateDockFill()
        {
            bool useMargin = this.UseMargin;
            Control child1 = this._child;
            Padding margin;
            int x;
            if (!useMargin)
            {
                x = 0;
            }
            else
            {
                margin = this._child.Margin;
                x = margin.Left;
            }
            int y;
            if (!useMargin)
            {
                y = 0;
            }
            else
            {
                margin = this._child.Margin;
                y = margin.Top;
            }
            Point point = new Point(x, y);
            child1.Location = point;
            Control child2 = this._child;
            Size clientSize = this._parent.ClientSize;
            int width1 = clientSize.Width;
            int num1;
            if (!useMargin)
            {
                num1 = 0;
            }
            else
            {
                margin = this._child.Margin;
                num1 = margin.Horizontal;
            }
            int width2 = width1 - num1;
            clientSize = this._parent.ClientSize;
            int height1 = clientSize.Height;
            int num2;
            if (!useMargin)
            {
                num2 = 0;
            }
            else
            {
                margin = this._child.Margin;
                num2 = margin.Vertical;
            }
            int height2 = height1 - num2;
            Size size = new Size(width2, height2);
            child2.Size = size;
        }

        private void UpdateDockRight()
        {
            bool useMargin = this.UseMargin;
            Control child1 = this._child;
            Size clientSize;
            Padding margin;
            int x;
            if (!useMargin)
            {
                clientSize = this._parent.ClientSize;
                x = clientSize.Width - this._child.Width;
            }
            else
            {
                clientSize = this._parent.ClientSize;
                int num = clientSize.Width - this._child.Width;
                margin = this._child.Margin;
                int right = margin.Right;
                x = num - right;
            }
            int y;
            if (!useMargin)
            {
                y = 0;
            }
            else
            {
                margin = this._child.Margin;
                y = margin.Top;
            }
            Point point = new Point(x, y);
            child1.Location = point;
            Control child2 = this._child;
            int num1;
            if (!useMargin)
            {
                clientSize = this._parent.ClientSize;
                num1 = clientSize.Height;
            }
            else
            {
                clientSize = this._parent.ClientSize;
                int height = clientSize.Height;
                margin = this._child.Margin;
                int vertical = margin.Vertical;
                num1 = height - vertical;
            }
            child2.Height = num1;
        }

        private void UpdateDockLeft()
        {
            bool useMargin = this.UseMargin;
            Control child1 = this._child;
            Padding margin;
            int x;
            if (!useMargin)
            {
                x = 0;
            }
            else
            {
                margin = this._child.Margin;
                x = margin.Left;
            }
            int y;
            if (!useMargin)
            {
                y = 0;
            }
            else
            {
                margin = this._child.Margin;
                y = margin.Top;
            }
            Point point = new Point(x, y);
            child1.Location = point;
            Control child2 = this._child;
            int num;
            if (!useMargin)
            {
                num = this._parent.ClientSize.Height;
            }
            else
            {
                int height = this._parent.ClientSize.Height;
                margin = this._child.Margin;
                int vertical = margin.Vertical;
                num = height - vertical;
            }
            child2.Height = num;
        }

        private void UpdateDockBottom()
        {
            bool useMargin = this.UseMargin;
            Control child1 = this._child;
            Padding margin;
            int x;
            if (!useMargin)
            {
                x = 0;
            }
            else
            {
                margin = this._child.Margin;
                x = margin.Left;
            }
            Size clientSize;
            int y;
            if (!useMargin)
            {
                clientSize = this._parent.ClientSize;
                y = clientSize.Height - this._child.Height;
            }
            else
            {
                clientSize = this._parent.ClientSize;
                int num = clientSize.Height - this._child.Height;
                margin = this._child.Margin;
                int top = margin.Top;
                y = num - top;
            }
            Point point = new Point(x, y);
            child1.Location = point;
            Control child2 = this._child;
            int num1;
            if (!useMargin)
            {
                clientSize = this._parent.ClientSize;
                num1 = clientSize.Width;
            }
            else
            {
                clientSize = this._parent.ClientSize;
                int width = clientSize.Width;
                margin = this._child.Margin;
                int horizontal = margin.Horizontal;
                num1 = width - horizontal;
            }
            child2.Width = num1;
        }

        private void UpdateDockTop()
        {
            bool useMargin = this.UseMargin;
            Control child1 = this._child;
            Padding margin;
            int x;
            if (!useMargin)
            {
                x = 0;
            }
            else
            {
                margin = this._child.Margin;
                x = margin.Left;
            }
            int y;
            if (!useMargin)
            {
                y = 0;
            }
            else
            {
                margin = this._child.Margin;
                y = margin.Top;
            }
            Point point = new Point(x, y);
            child1.Location = point;
            Control child2 = this._child;
            int num;
            if (!useMargin)
            {
                num = this._parent.ClientSize.Width;
            }
            else
            {
                int width = this._parent.ClientSize.Width;
                margin = this._child.Margin;
                int horizontal = margin.Horizontal;
                num = width - horizontal;
            }
            child2.Width = num;
        }

        public void Dispose()
        {
            this._parent.Resize -= new EventHandler(this._parent_Resize);
            this._throttler.Dispose();
        }
    }
}