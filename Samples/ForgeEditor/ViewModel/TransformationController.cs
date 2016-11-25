using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ForgeEditor.Components;
using ForgeEditor.Components.CoordinateSystems;
using ForgeEditor.Components.Transforms;
using Igneel;
using Igneel.SceneManagement;

namespace ForgeEditor.ViewModel
{
    public class TransformationController
    {
        struct TransformParameters
        {
            public Vector2 P0;
            public Vector2 P1;
            public GlypComponent Component;
            public Frame Frame;
            public ITranformGlyp Glyp;
        }

        public enum TransformationType { None = 0, Translation = 1, Rotation = 2, Scale = 3  }

        ITranformGlyp currentGlyp;

        TranslationGlyp translation;
        RotationGlyp rotation;
        ScaleGlyp scale;
        Frame selectedObject;
        private IMainShell windows;
        private TransformationType _transformationType;
        private Vector2 p0;
        private GlypComponent transformComponent;

        Stack<TransformParameters> transforms = new Stack<TransformParameters>();
        private int MaxTranforms = 10;

        public TransformationType Transformation 
        {
            get { return _transformationType; }
            set
            {

                if (currentGlyp != null)
                    currentGlyp.Visible = false;

                _transformationType = value;
                switch (value)
                {                    
                    case TransformationType.Translation:
                        currentGlyp = translation;
                        break;
                    case TransformationType.Rotation:
                        currentGlyp = rotation;
                        break;
                    case TransformationType.Scale:
                        currentGlyp = scale;
                        break;
                    default:
                        currentGlyp = null;
                        break;
                }
                if (currentGlyp != null && selectedObject != null)
                {
                    currentGlyp.Visible = true;
                    Vector3 worldPosition = selectedObject.Bounding != null ?
                        selectedObject.BoundingSphere.Center :
                        selectedObject.GlobalPosition;
                    UpdateGlyp(worldPosition);
                }

            }
        }

        public ITranformGlyp CurrentGlyp { get { return currentGlyp; } }

        public Frame SelectedFrame
        {
            get { return selectedObject; }
            set
            {
                selectedObject = value;
                if (value == null)
                {
                    if (currentGlyp != null)
                        currentGlyp.Visible = false;
                }
            }
        }

        public TransformationController(IMainShell windows)
        {
            this.windows = windows;

            translation = new TranslationGlyp(new Rectangle(0, 0, 128, 128))
            {
                Visible = false,
                Alpha = 0.7f,
                EnablePlanes = true,
                Width = 5
            }.Initialize() as TranslationGlyp;

            rotation = new RotationGlyp(new Rectangle(0, 0, 128, 128))
            {
                MarkersAlpha = 0.8f,
                SphereColor = new Color4(Color3.FromArgb(System.Drawing.Color.Yellow.ToArgb()), 0.5f),
                Thickness = 1,
                Visible = false
            };
            rotation.Initialize();

            scale = new ScaleGlyp() { Visible = false };

            Engine.Scene.Decals.Add(translation);
            Engine.Scene.Decals.Add(rotation);
            Engine.Scene.Decals.Add(scale);

            Engine.UpdateFrame += UpdateGlyps;          
        }
    

        private void UpdateGlyps(float deltaT)
        {
            if (selectedObject == null || currentGlyp == null)
                return;

            Vector3 worldPosition = selectedObject.Bounding != null ? selectedObject.BoundingSphere.Center : selectedObject.GlobalPosition;
            if (currentGlyp.Visible)
            {               
                 UpdateGlyp(worldPosition);
            }
        }

        public void SetSelectedFrameAsync(Frame value)
        {
            Engine.Invoke(() => SelectedFrame = value);
        }

        public void ShowTransformGlypAsync(Frame selectedObject)
        {            
            Engine.Invoke(delegate
            {
                ShowTransformGlyp(selectedObject);
            });
        }

        public void ShowTransformGlyp(Frame selectedObject)
        {
            this.selectedObject = selectedObject;
            Vector3 worldPosition = selectedObject.Bounding != null ? selectedObject.BoundingSphere.Center : selectedObject.GlobalPosition;

             UpdateGlyp(worldPosition);
        }

        private void UpdateGlyp(Vector3 worldPosition)
        {
            if (currentGlyp != null)
            {
                UpdateGlyp(currentGlyp, worldPosition);
            }
        }

        private void UpdateGlyp(ITranformGlyp glyp, Vector3 worldPosition)
        {
            if (glyp == null)
                return;

            var scene = Engine.Scene;
            var camera = scene.ActiveCamera;
            var graphics = Engine.Graphics;

            //transform to homogenius space
            var projPosition = Vector4.Transform(worldPosition, camera.ViewProj);

            //complete projection
            projPosition.X /= projPosition.W;
            projPosition.Y /= projPosition.W;
            projPosition.Z /= projPosition.W;

            var vp = graphics.ViewPort;
            int halfWidth = vp.Width / 2;
            int halfHeight = vp.Height / 2;

            var screenPosition = new Vector2(projPosition.X * halfWidth + halfWidth,
                                                 projPosition.Y * -halfHeight + halfHeight);
            int x = (int)Math.Ceiling((float)screenPosition.X);
            int y = (int)Math.Ceiling((float)screenPosition.Y);


            glyp.Translate(x, y);
            if (!glyp.Visible)
                glyp.Visible = true;
        }

        public void HitTestAsync(Vector2 location)
        {
            Engine.Invoke(delegate
            {
                HitTest(location);
            });
        }

        public GlypComponent HitTest(Vector2 screenPosition)
        {
            if (currentGlyp == null || !currentGlyp.Visible)
            {
                transformComponent = null;
                return null;
            }

            transformComponent = currentGlyp.DoHitTest(screenPosition);
            if (transformComponent != null)
            {               
                p0 = screenPosition;
                //MessageBox.Show(transformComponent.Id.ToString());
            }
            return transformComponent;
        }

        public void HitMove(Vector2 screenLocation)
        {
            if (currentGlyp == null || transformComponent == null || selectedObject == null)
                return;

            if (transforms.Count == MaxTranforms)
            {
                var list = transforms.ToList();

                transforms.Clear();
                foreach (var item in list.Take(MaxTranforms - 1))
                {
                    transforms.Push(item);
                }
            }

            transforms.Push(new TransformParameters
            {
                 Frame= selectedObject,
                  Component = transformComponent,
                  P0 = p0,
                  P1 = screenLocation,
                  Glyp = currentGlyp
            });

            currentGlyp.Transform(selectedObject, transformComponent, p0, screenLocation);

            Vector3 worldPosition = selectedObject.Bounding != null ? selectedObject.BoundingSphere.Center : selectedObject.GlobalPosition;
            UpdateGlyp(worldPosition);

            p0 = screenLocation;
        }

        public void TransformBack()
        {
            if (transforms.Count == 0) return;

            var p = transforms.Pop();
            if (p.Frame.Disposed)
                return;

            currentGlyp.Transform(p.Frame, p.Component, p.P1, p.P0);

            Vector3 worldPosition = p.Frame.Bounding != null ? 
                p.Frame.BoundingSphere.Center 
                : p.Frame.GlobalPosition;

            UpdateGlyp(p.Glyp, worldPosition);           
        }
    }
}
