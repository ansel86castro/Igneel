namespace Igneel.SceneManagement
{
    //public class SceneState: ResourceAllocator, IAssetProvider
    //{
    //    Scene _scene;
    //    ISceneStateTrigger _trigger;
    //    List<SceneState> _states = new List<SceneState>();

    //    public SceneState() { }

    //    public SceneState(Scene scene, ISceneStateTrigger trigger)
    //    {
    //        this._scene = scene;
    //        this._trigger = trigger;
    //    }

    //    [AssetMember(storeAs: StoreType.Reference)]
    //    public Scene Scene { get { return _scene; } set { _scene = value; } }
        
    //    [AssetMember(storeAs: StoreType.Reference)]
    //    public ISceneStateTrigger Trigger { get { return _trigger; } set { _trigger = value; } }
        
    //    [AssetMember(typeof(CollectionStoreConverter<SceneState>))]
    //    public List<SceneState> States { get { return _states; } }

    //    public void SetActive()
    //    {
    //        Engine.Scene = _scene;
    //    }

    //    public bool RemoveStates(Scene scene)
    //    {
    //        List<SceneState>removeStates=new List<SceneState>();
    //        bool result = false;
    //        foreach (var item in _states)
    //        {
    //            if (item._scene == scene)
    //            {
    //                result = true;
    //                removeStates.Add(item);
    //            }
    //            else
    //            {
    //               var r= item.RemoveStates(scene);
    //               if (!result)
    //                   result = r;
    //            }
    //        }

    //        foreach (var item in removeStates)
    //        {
    //            _states.Remove(item);
    //        }
    //        return result;
    //    }      

    //    public Asset CreateAsset()
    //    {
    //        return Asset.Create(this);
    //    }
    //}
       
}
