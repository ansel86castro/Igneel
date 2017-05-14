using Igneel;
using Igneel.Graphics;
using Igneel.Input;
using Igneel.Physics;
using Igneel.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D3D9Testing
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Service.Set<GraphicDeviceFactory>(new IgneelD3D10.GraphicManager10());
            //Service.Set<PhysicManager>(new Igneel.PhysX.PXPhysicManager());
            Service.Set<InputManager>(new IgneelDirectInput.DInputManager());

            Application.Run(new Form1(new IgneelFormsApp()));
          
        }
    }
}
