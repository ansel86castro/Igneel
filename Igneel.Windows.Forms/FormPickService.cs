using Igneel.Scenering;
using Igneel.Rendering;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Igneel.Windows.Forms
{
    public class FormPickService : PickingService
    {
        class EventHandler
        {
            Point downPoint;
            bool multipleSelect;
            Rectangle rec;
            PickingZone zone;
            Control control;
            MouseButtons selectionButton = MouseButtons.Left;

            public EventHandler(Control control, PickingZone zone)
            {
                this.zone = zone;
                this.control = control;
                control.MouseUp += control_MouseUp;
                control.MouseDown += control_MouseDown;
                control.MouseMove += control_MouseMove;
                control.Resize += control_Resize;
            }

            void control_Resize(object sender, EventArgs e)
            {
                zone.Resize(control.Width, control.Height);
            }

            void control_MouseMove(object sender, MouseEventArgs e)
            {
                //if (e.Button == selectionButton)
                //{
                //    multipleSelect = true;

                //}
            }

            void control_MouseDown(object sender, MouseEventArgs e)
            {
                multipleSelect = false;
                downPoint = e.Location;
                rec.X = downPoint.X;
                rec.Y = downPoint.Y;
            }

            void control_MouseUp(object sender, MouseEventArgs e)
            {
                if (!multipleSelect)
                {
                    zone.Pick(downPoint.X, downPoint.Y);
                }
                else
                {
                    zone.Pick(rec);
                }
            }
        }

        List<EventHandler> handlers = new List<EventHandler>();

        public PickingZone CreateZone(Canvas3D control, IPickContainer container = null)
        {

            if (container == null)
                container = new PickContainer() { Scene = control.Scene };

            PickingZone zone = new PickingZone(control.Name, new Size(control.Width, control.Height))
                {
                    Scene = control.Scene,
                    PickContainer = container,
                    Camera = control.Camera
                };
            Zones.Add(zone);

            handlers.Add(new EventHandler(control, zone));
            return zone;
        }
    }
}
