using Igneel.Rendering;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Igneel;
using Igneel.Techniques;

namespace Igneel.Windows.Forms
{
    //public class FormPickService : PickingService
    //{
    //    class EventHandler
    //    {
    //        Point _downPoint;
    //        bool _multipleSelect;
    //        Rectangle _rec;
    //        PickingZone _zone;
    //        Control _control;
    //        MouseButtons _selectionButton = MouseButtons.Left;

    //        public EventHandler(Control control, PickingZone zone)
    //        {
    //            this._zone = zone;
    //            this._control = control;
    //            control.MouseUp += control_MouseUp;
    //            control.MouseDown += control_MouseDown;
    //            control.MouseMove += control_MouseMove;
    //            control.Resize += control_Resize;
    //        }

    //        void control_Resize(object sender, EventArgs e)
    //        {
    //            _zone.Resize(_control.Width, _control.Height);
    //        }

    //        void control_MouseMove(object sender, MouseEventArgs e)
    //        {
    //            //if (e.Button == selectionButton)
    //            //{
    //            //    multipleSelect = true;

    //            //}
    //        }

    //        void control_MouseDown(object sender, MouseEventArgs e)
    //        {
    //            _multipleSelect = false;
    //            _downPoint = e.Location;
    //            _rec.X = _downPoint.X;
    //            _rec.Y = _downPoint.Y;
    //        }

    //        void control_MouseUp(object sender, MouseEventArgs e)
    //        {
    //            if (!_multipleSelect)
    //            {
    //                _zone.Pick(_downPoint.X, _downPoint.Y);
    //            }
    //            else
    //            {
    //                _zone.Pick(_rec);
    //            }
    //        }
    //    }

    //    List<EventHandler> _handlers = new List<EventHandler>();

    //    public PickingZone CreateZone(Canvas3D control, IPickContainer container = null)
    //    {

    //        if (container == null)
    //            container = new PickContainer() { Scene = control.Scene };

    //        PickingZone zone = new PickingZone(control.Name, new Size(control.Width, control.Height))
    //            {
    //                Scene = control.Scene,
    //                PickContainer = container,
    //                Camera = control.Camera
    //            };
    //        Zones.Add(zone);

    //        _handlers.Add(new EventHandler(control, zone));
    //        return zone;
    //    }
    //}
}
