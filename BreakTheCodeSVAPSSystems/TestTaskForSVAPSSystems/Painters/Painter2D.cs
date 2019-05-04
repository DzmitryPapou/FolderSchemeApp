using TestTaskForSVAPSSystems.Interfaces;
using TestTaskForSVAPSSystems.Models;
using System.Drawing;

namespace TestTaskForSVAPSSystems.Painters
{
    internal class Painter2D : IPainter
    {
        private readonly MainModel _model;
        private readonly Graphics _graphics;
        private readonly Pen _pen;

        public Painter2D(MainModel model, Graphics graphics, Pen pen)
        {
            _model = model;
            _graphics = graphics;
            _pen = pen;
        }
        public void Paint()
        {
            Panel panel = _model.Panels.Find(p => p.PanelName == "root panel");
            panel.PointNullX = _model.RootX;
            panel.PointNullY = _model.RootY;
            _graphics.DrawRectangle(_pen, panel.PointNullX - panel.PanelWidth / 2, panel.PointNullY - panel.PanelHeight, panel.PanelWidth, panel.PanelHeight);
            PaintChild(panel);
        }
        private void PaintChild(Panel panel)
        {
            foreach (Panel child in panel.СhildPanel)
            {
                (float X, float Y, float Width, float Height) = GetAttachedToSideRectangle(panel, child);
                _graphics.DrawRectangle(_pen, X, Y, Width, Height);
                PaintChild(child);
            }
        }
        private (float X, float Y, float Width, float Height) GetAttachedToSideRectangle(Panel parent, Panel child)
        {
            (float X, float Y, float Width, float Height) rect = (X: 0, Y: 0, Width: 0, Height: 0);
            int side = child.AttachedToSide + parent.RouteRotate;
            if (side == 4)
                side = 0;
            if (side == 1 || side == 3)
            {
                rect.Width = child.PanelHeight;
                rect.Height = child.PanelWidth;
                child.Rotate = true;
            }
            else
            {
                rect.Width = child.PanelWidth;
                rect.Height = child.PanelHeight;
                child.Rotate = false;
            }
            switch (side)
            {
                case 0:
                    {
                        child.RouteRotate = 0;
                        child.PointNullX = parent.PointNullX;
                        rect.X = parent.PointNullX - rect.Width / 2 + child.HingeOffset;
                        rect.Y = parent.PointNullY;
                        if (!parent.Rotate)
                            child.PointNullY = parent.PointNullY + parent.PanelHeight;
                        else
                            child.PointNullY = parent.PointNullY + parent.PanelWidth;
                        break;
                    }
                case 1:
                    {
                        child.RouteRotate = -1;
                        child.PointNullY = parent.PointNullY;
                        if (!parent.Rotate)
                        {
                            child.PointNullX = parent.PointNullX + parent.PanelWidth / 2 + rect.Width / 2;
                            rect.X = parent.PointNullX + parent.PanelWidth / 2;
                            rect.Y = (parent.PointNullY - parent.PanelHeight / 2) - rect.Height / 2 + child.HingeOffset;
                        }
                        else
                        {
                            child.PointNullX = parent.PointNullX + parent.PanelHeight / 2 + rect.Width / 2;
                            rect.X = parent.PointNullX + parent.PanelHeight / 2;
                            rect.Y = (parent.PointNullY - parent.PanelWidth / 2) - rect.Height / 2 + child.HingeOffset;
                        }
                        break;
                    }
                case 2:
                    {
                        child.RouteRotate = 0;
                        child.PointNullX = parent.PointNullX;
                        rect.X = parent.PointNullX - rect.Width / 2 + child.HingeOffset;
                        if (!parent.Rotate)
                        {
                            child.PointNullY = parent.PointNullY - parent.PanelHeight;
                            rect.Y = parent.PointNullY - parent.PanelHeight - rect.Height;
                        }
                        else
                        {
                            child.PointNullY = parent.PointNullY - parent.PanelWidth;
                            rect.Y = parent.PointNullY - parent.PanelWidth - rect.Height;
                        }
                        break;
                    }
                case 3:
                    {
                        child.RouteRotate = 1;
                        child.PointNullY = parent.PointNullY;
                        if (!parent.Rotate)
                        {
                            child.PointNullX = parent.PointNullX - parent.PanelWidth / 2 - rect.Width / 2;
                            rect.X = parent.PointNullX - parent.PanelWidth / 2 - rect.Width;
                            rect.Y = parent.PointNullY - parent.PanelHeight / 2 - rect.Height / 2 + child.HingeOffset;
                        }
                        else
                        {
                            child.PointNullX = parent.PointNullX - parent.PanelHeight / 2 - rect.Width / 2;
                            rect.X = parent.PointNullX - parent.PanelHeight / 2 - rect.Width;
                            rect.Y = parent.PointNullY - parent.PanelWidth / 2 - rect.Height / 2 + child.HingeOffset;
                        }
                        break;
                    }
            }
            return rect;
        }
    }
}
