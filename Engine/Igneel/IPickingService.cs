using Igneel.Collections;
using Igneel.Rendering;
using Igneel.SceneComponents;
using Igneel.SceneManagement;
using Igneel.Techniques;

namespace Igneel
{    

    //public class PickingZone:INameable,IEnabletable
    //{
    //    HitTestTechnique _technique;
    //    Scene _scene;
    //    Camera _camera;
    //    Size _size;
    //    object _tag;
    //    string _name;
    //    IPickContainer _container;
    //    bool _enable;

    //    public PickingZone(string name) { this._name = name; }

    //    public PickingZone(string name, Size size)
    //        :this(name)
    //    {
    //        Resize(size);
    //    }

    //    public bool Enable { get { return _enable; } set { _enable = value; } }

    //    public Camera Camera { get { return _camera; } set { _camera = value; } }

    //    public Size Size { get { return _size; }
    //        set
    //        {
    //            Resize(value);
    //        }
    //    }        

    //    public object Tag { get { return _tag; } set { _tag = value; } }

    //    public string Name { get { return _name; } set { _name = value; } }

    //    public Scene Scene { get { return _scene; } set { _scene = value; } }

    //    public IPickContainer PickContainer
    //    {
    //        get { return _container; }
    //        set
    //        {
    //            _container = value;
    //            if (_technique != null)
    //                _technique.PickContainer = value;
    //        }
    //    }

    //    public void Resize(int width, int height)
    //    {
    //        Resize(new Size(width, height));
    //    }

    //    private void Resize(Size size)
    //    {
    //        this._size = size;
    //        if(_technique == null)
    //        {
    //            _technique = new HitTestTechnique(size.Width, size.Height, _container);
    //        }
    //        else
    //            _technique.Resize(size.Width, size.Height);
    //    }

    //    public void Pick(int screenX, int screenY)
    //    {
    //        if (!_enable) return;

    //        var oldcamera = _scene.ActiveCamera;
    //        _scene.ActiveCamera = this._camera;

    //        _container.Scene = _scene;
    //        _technique.PickContainer = _container;
    //        _technique.SelectMultiple = false;
    //        _technique.HitTestLocation = new Vector2(screenX, screenY);
    //        RenderManager.ApplyTechnique(_technique);          

    //        _scene.ActiveCamera = _camera;
    //    }

    //    public void Pick(Rectangle rec)
    //    {
    //        if (!_enable) return;

    //        var oldcamera = _scene.ActiveCamera;
    //        _scene.ActiveCamera = this._camera;

    //        _container.Scene = _scene;
    //        _technique.PickContainer = _container;
    //        _technique.SelectMultiple = true;
    //        _technique.HitTestRegion = rec;
    //        RenderManager.ApplyTechnique(_technique);

    //        _scene.ActiveCamera = _camera;
    //    }
    //}

    //public class PickingService
    //{
    //    ObservedDictionary<string, PickingZone> _zones;

    //    public PickingService()
    //    {
    //        _zones = new ObservedDictionary<string, PickingZone>(null, null, x => x.Name);
    //    }

    //    public bool Enable { get; set; }

    //    public ObservedDictionary<string, PickingZone> Zones { get { return _zones; } }
       
    //}

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

    //        foreach (var item in Engine.Scene.VisibleComponents)
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
