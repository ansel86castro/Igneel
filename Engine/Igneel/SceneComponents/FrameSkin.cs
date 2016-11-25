using Igneel.Assets;
using Igneel.Components;

namespace Igneel.SceneComponents
{
    [Asset("FRAME_OBJECT")]
    public class FrameSkin : MaterialCollection<FrameSkin>, IFrameMesh
    {
        MeshSkin _skin;

        public FrameSkin(BasicMaterial[] materials = null, MeshSkin skin = null) :
            base(materials)
        {
            this._skin = skin;
        }

        public Mesh Mesh
        {
            get { return _skin != null ? _skin.Mesh : null; }
        }

        [AssetMember(storeAs: StoreType.Reference)]
        public MeshSkin Skin
        {
            get { return _skin; }
            set
            {
                _skin = value;
            }
        }


        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                _skin.Dispose();
            }
            base.OnDispose(disposing);
        }

        //[Serializable]
        //class Activator : IResourceActivator
        //{            
        //    public void Initialize(IAssetProvider provider)
        //    {

        //    }

        //    public IAssetProvider OnCreateResource()
        //    {             
        //        return new FrameSkin();
        //    }

        //}       

      
    }
}
