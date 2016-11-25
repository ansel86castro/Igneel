using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForgeEditor.Components.CoordinateSystems;
using Igneel;
using Igneel.SceneManagement;

namespace ForgeEditor.Components.Transforms
{
    public class TranslationGlyp : CoordinateGlyp, ITranformGlyp
    {
        private Plane[] planes = new Plane[6];

        public TranslationGlyp(Igneel.Rectangle rect)
            :base(rect)
        {
            planes[0] = new Plane(Vector3.Zero, new Vector3(1, 0, 0)); //YZ
            planes[1] = new Plane(Vector3.Zero, new Vector3(0, 1, 0)); //XZ
            planes[2] = new Plane(Vector3.Zero, new Vector3(0, 0, 1)); //XY            
        }

        public TranslationGlyp(float alpha)
        {
            planes[0] = new Plane(Vector3.Zero, new Vector3(1, 0, 0)); //YZ
            planes[1] = new Plane(Vector3.Zero, new Vector3(0, 1, 0)); //XZ
            planes[2] = new Plane(Vector3.Zero, new Vector3(0, 0, 1)); //XY            
        }

        #region ITranformGlyp Members

        public void Transform(Frame frame,GlypComponent component, Vector2 p0, Vector2 p1)
        {
            var dp = p1 - p0;
            var viewPort = Engine.Graphics.ViewPort;
            var camera = Engine.Scene.ActiveCamera;
            var view =camera.View;
            var proj = camera.Projection;
            var world = frame.GlobalPose;
            var invWorld = Matrix.Invert(frame.GlobalPose);
            var worlPosition = frame.BoundingSphere.Center;
            var vpSize = new Size(viewPort.Width, viewPort.Height);

            //var viewDisplacement = ComputeViewDisplacement(dp.X, dp.Y, worlPosition, vpSize, view, proj);
            //var localDisplacement = Vector3.TransformCoordinates(viewDisplacement, invWorld);            
            //var d = localDisplacement.Length();
            //float sigh;

            //Compute View Translation
            var projPosition = Vector3.TransformCoordinates(frame.GlobalPosition, camera.ViewProj);

            Vector3 pw0 = ScreenToWorld(camera.InvViewProjection, p0, vpSize, projPosition.Z);
            Vector3 pw1 = ScreenToWorld(camera.InvViewProjection, p1, vpSize, projPosition.Z);            
            var dw = pw1 - pw0;
         
            if (dw.IsZero())
                return;          

            Vector3 translation = new Vector3(); 
            switch (component.Id)
            {
                case X:                    
                    translation = new Vector3(dw.X, 0, 0);
                    break;
                case Y:
                    translation = new Vector3(0, dw.Y, 0);
                    break;
                case Z:
                    translation =new Vector3(0,0, dw.Z);
                    break;
                case XY:
                    translation = new Vector3(dw.X, dw.Y, 0);
                    break;
                case XZ:
                    translation = new Vector3(dw.X, 0, dw.Z);
                    break;
                case YZ:
                    translation = new Vector3(0, dw.Y, dw.Z);
                    break;
                
            }

            var localPose = frame.LocalPose;
            var Tw = Matrix.Translate(translation);
            var P = Matrix.Invert(localPose) * frame.GlobalPose;
            var Tl = P * Tw * Matrix.Invert(P);
            frame.LocalPose *= Tl;
            frame.CommitChanges();
        }

        //private void FindTestPlane(Vector2 p0)
        //{
        //    Vector4 p = new Vector4(p0, 1, 1);
        //    var camera = Engine.Scene.ActiveCamera;
        //    var 
        //}

        public static Vector3 ComputeViewDisplacement(float dx, float dy, Vector3 position, Size vpSize,
            Matrix view, Matrix proj)
        {
            float xCenter = vpSize.Width * 0.5f;
            float yCenter = -vpSize.Height * 0.5f;

            Vector3 viewPos = Vector3.TransformCoordinates(position, view);
            Vector3 projPos = Vector3.TransformCoordinates(viewPos, proj);

            projPos.X += (dx / xCenter);
            projPos.Y -= (dy / yCenter);

            Matrix invProj = Matrix.Invert(proj);
            Vector3 posDisp = Vector3.TransformCoordinates(projPos, invProj); //translated position in View Space

            Vector3 disp = posDisp - viewPos;

            return disp;
        }

        public static Vector2 WorldToClient(Matrix viewProj, Vector3 worldPostion, Size vpSize)
        {
            int halfWidth = vpSize.Width / 2;
            int halfHeight = vpSize.Height / 2;

            Vector3 projPosition = Vector3.TransformCoordinates(worldPostion, viewProj);
            Vector2 screenPosition = new Vector2(projPosition.X * halfWidth + halfWidth, projPosition.Y * -halfHeight + halfHeight);
            screenPosition.X = (float)Math.Ceiling((float)screenPosition.X);
            screenPosition.Y = (float)Math.Ceiling((float)screenPosition.Y);
            return screenPosition;
        }

        public static Vector3 ScreenToWorld(Matrix invViewProjection, Vector2 screenPos, Size vpSize, float projZ = 1.0f)
        {
            Vector3 projPoint = new Vector3(((2 * screenPos.X / (float)vpSize.Width) - 1), 
                -((2 * screenPos.Y / (float)vpSize.Height) - 1), projZ);
            Vector3 worldPos = Vector3.TransformCoordinates(projPoint, invViewProjection);
            return worldPos;
        }     

        #endregion
    }
}
