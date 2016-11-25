namespace Igneel
{
    public static class Affectable
    {
        public static void BindTo(this IAffectable target, IAffector affector)
        {            
            target.BindAffectorPose = Matrix.Invert(affector.GlobalPose);
            target.Affector = affector;
            affector.Affectable = target;     
        }

        public static void UnBindFrom(this IAffectable target, IAffector affector)
        {
            target.BindAffectorPose = Matrix.Identity;
            target.Affector = null;            
            affector.Affectable = null;
        }
    }
}