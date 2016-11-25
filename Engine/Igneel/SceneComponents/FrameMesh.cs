using Igneel.Assets;
using Igneel.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.SceneManagement;

namespace Igneel.SceneComponents
{
    [Asset("FRAME_OBJECT")]
    public class FrameMesh : MaterialCollection<FrameMesh>, IFrameMesh, IBoundable
    {
        private Mesh _mesh;

        public FrameMesh(BasicMaterial[] materials = null, Mesh mesh = null)
            : base(materials)
        {
            _mesh = mesh;
            Name = mesh.Name;
        }


        [AssetMember(storeAs: StoreType.Reference)]
        public Mesh Mesh
        {
            get
            {
                return _mesh;
            }
            set
            {
                _mesh = value;
            }
        }

        public Sphere BoundingSphere
        {
            get { return _mesh != null ? _mesh.BoundingSphere : default(Sphere); }
        }

        public OrientedBox BoundingBox
        {
            get { return _mesh != null ? _mesh.BoundingBox : null; }
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                if (_mesh != null)
                    _mesh.Dispose();

                _mesh = null;
            }
            base.OnDispose(disposing);
        }   
    }
}
