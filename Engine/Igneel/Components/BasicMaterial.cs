using Igneel.Assets;
using Igneel.Effects;
using Igneel.Graphics;
using Igneel.Rendering;


namespace Igneel.Components
{

    public class BasicMaterial : SurfaceMaterial, IVisualMaterial
    {
        private Texture2D _diffuseMap;
        private Texture2D _specularMap;
        private Texture2D _normalMap;

        public BasicMaterial(string name)
            : base(name)
        {

        }

        public BasicMaterial()
        {

        }

        [AssetMember(StoreAs = StoreType.Reference)]
        public Texture2D DiffuseMap
        {
            get { return _diffuseMap; }
            set
            {
                _diffuseMap = value;
            }
        }

        [AssetMember(StoreAs = StoreType.Reference)]
        public Texture2D SpecularMap
        {
            get { return _specularMap; }
            set { _specularMap = value; }
        }

        [AssetMember(StoreAs = StoreType.Reference)]
        public Texture2D NormalMap
        {
            get { return _normalMap; }
            set { _normalMap = value; }
        }

        public static BasicMaterial CreateDefaultMaterial(string name)
        {
            return new BasicMaterial(name);
        }

        public bool CheckForTransparency()
        {
            if (Alpha < 1)
                return ContainsTrasparency = true;
            else if (_diffuseMap == null)
                return ContainsTrasparency = false;


            TransparencyChecker checker = Service.Require<TransparencyChecker>();
            ContainsTrasparency = checker.IsTransparent(_diffuseMap);

            return ContainsTrasparency;
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                if (_specularMap != null) _specularMap.Dispose();
                if (_diffuseMap != null) _diffuseMap.Dispose();
                if (_normalMap != null) _normalMap.Dispose();

                _specularMap = null;
                _diffuseMap = null;
                _normalMap = null;

            }
        }

        #region IVisualMaterial Members

        public int VisualId { get; set; }

        public Render Render { get; set; }

        public void Bind(Render render)
        {
            render.Bind(this);
        }

        public void UnBind(Render render)
        {
            render.UnBind(this);
        }

        #endregion
    }

   
}
