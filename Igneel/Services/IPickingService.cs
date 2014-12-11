using Igneel.Collections;
using Igneel.Components;
using Igneel.Graphics;
using Igneel.Physics;
using Igneel.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Igneel.Services
{    

    public class PickingZone:INameable,IEnabletable
    {
        HitTestTechnique technique;
        Scene scene;
        Camera camera;
        Size size;
        object tag;
        string name;
        IPickContainer container;
        bool enable;

        public PickingZone(string name) { this.name = name; }

        public PickingZone(string name, Size size)
            :this(name)
        {
            Resize(size);
        }

        public bool Enable { get { return enable; } set { enable = value; } }

        public Camera Camera { get { return camera; } set { camera = value; } }

        public Size Size { get { return size; }
            set
            {
                Resize(value);
            }
        }        

        public object Tag { get { return tag; } set { tag = value; } }

        public string Name { get { return name; } set { name = value; } }

        public Scene Scene { get { return scene; } set { scene = value; } }

        public IPickContainer PickContainer
        {
            get { return container; }
            set
            {
                container = value;
                if (technique != null)
                    technique.PickContainer = value;
            }
        }

        public void Resize(int width, int height)
        {
            Resize(new Size(width, height));
        }

        private void Resize(Size size)
        {
            this.size = size;
            if(technique == null)
            {
                technique = new HitTestTechnique(size.Width, size.Height, container);
            }
            else
                technique.Resize(size.Width, size.Height);
        }

        public void Pick(int screenX, int screenY)
        {
            if (!enable) return;

            var oldcamera = scene.ActiveCamera;
            scene.ActiveCamera = this.camera;

            container.Scene = scene;
            technique.PickContainer = container;
            technique.SelectMultiple = false;
            technique.Location = new System.Drawing.Point(screenX, screenY);
            Engine.ApplyTechnique(technique);          

            scene.ActiveCamera = camera;
        }

        public void Pick(Rectangle rec)
        {
            if (!enable) return;

            var oldcamera = scene.ActiveCamera;
            scene.ActiveCamera = this.camera;

            container.Scene = scene;
            technique.PickContainer = container;
            technique.SelectMultiple = true;
            technique.Rectangle = rec;
            Engine.ApplyTechnique(technique);

            scene.ActiveCamera = camera;
        }
    }

    public class PickingService
    {
        ObservedDictionary<string, PickingZone> zones;

        public PickingService()
        {
            zones = new ObservedDictionary<string, PickingZone>(null, null, x => x.Name);
        }

        public bool Enable { get; set; }

        public ObservedDictionary<string, PickingZone> Zones { get { return zones; } }
       
    }

    //public class IdPickingService:IPickingService 
    //{
    //    ObservedDictionary<string, PickingZone> zones;       

    //    public event Action SelectionChanged;

    //    public IdPickingService()
    //    {
    //        zones = new ObservedDictionary<string, PickingZone>(null, null, x => x.Name);
    //    }

    //    public bool  Enable { get; set; }

    //    public ObservedDictionary<string, PickingZone> Zones { get { return zones; } }
      
    //    private static ActorShape FindShape(int id, Actor actor)
    //    {
    //        foreach (var shape in actor.Shapes)
    //        {
    //            if (shape.Id == id)
    //            {
    //                return shape;
    //            }
    //        }

    //        return null;
    //    }

    //    public void Initialize()
    //    {
    //        techniques.Clear();
    //        foreach (var item in controls)
    //        {
    //            techniques.Add(new HitTestTechnique(item.Width, item.Height));
    //        }
    //    }    

    //    public object FindObjectById(int id)
    //    {
    //        if (Engine.Scene == null) return null;

    //        foreach (var item in Engine.Scene.RenderList)
    //        {
    //            if (item.Node.Id == id)
    //                return item.Node;
    //        }

    //        if (Engine.Scene.Physics != null)
    //        {
    //            foreach (var item in Engine.Scene.Physics.Actors)
    //            {
    //                ActorShape shape;
    //                if ((shape = FindShape(id, item)) != null)
    //                    return shape;
    //            }
    //        }

    //        return null;
    //    }

    //    public void ClearObjectIds()
    //    {
    //        selectedObjects.Clear();
    //    }

    //    public void AddObjectId(int id)
    //    {
    //        var obj = FindObjectById(id);
    //        if (obj != null)
    //        {
    //            selectedObjects.Add(obj);
    //            if (SelectionChanged != null)
    //                SelectionChanged();
    //        }
    //    }

    //    public void AddObject(object target)
    //    {
    //        selectedObjects.Add(target);
    //        if (SelectionChanged != null)
    //            SelectionChanged();
    //    }

    //    public void Remove(object target)
    //    {
    //        selectedObjects.Remove(target);
    //    }

    //    public void RemoveObjectId(int id)
    //    {
    //        var obj = FindObjectById(id);
    //        if (obj != null)
    //            selectedObjects.Remove(obj);
    //    }

    //    public object SelectedObject
    //    {
    //        get { return selectedObjects.Count > 0 ? selectedObjects[0] : null; }
    //    }

    //    public IEnumerable<object> SelectedObjects
    //    {
    //        get { return selectedObjects; }
    //    }
        
    //}
}
