using Igneel.Components;
using System;
namespace Igneel.Physics
{
    public interface CharacterController:IResourceAllocator,IAffector, IAffectable,IGraphicObject,INameable
    {
        CharacterControllerManager Manager { get; }

        CCTInteraction Interaction { get; set; }        

        Vector3 Position { get; set; }

        float StepOffset { get; set; }     

        object UserData { get; set; }

        CollitionResult Move(Vector3 disp, uint activeGroups, float minDist, float sharpness , GroupsMask groupsMask);

        CollitionResult Move(Vector3 disp, uint activeGroups, float minDist, float sharpness = 1.0f);

        void ReportSceneChanged();

        void Remove();
    }
}
