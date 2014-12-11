using Igneel.Animations;
using Igneel.Components;
using Igneel.IA;
using Igneel.Input;
using Igneel.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Controllers
{
   
    //public class ThirdPersonController
    //{
    //    const string IDLE = "idle";
    //    const string WALK = "walk";
    //    const string RUN = "run";
    //    const string STRADE_RIGHT = "strade_right";
    //    const string STRADE_LEFT = "strade_left";
    //    const string JUMP = "jump";

    //    SceneNode rootNode;
    //    Vector3 blendingTranslSpeed;
    //    Vector3 initialTranslation;
    //    BonesResetter resetter;
    //    float initialHeading;
    //    float currentHeading;
    //    Vector3 currentTranslation;
    //    Vector3 initialKeyValue;
    //    Vector3 lastKeyValue;
    //    Vector3 lastAnimTrans;
    //    SecuenceStateMachine states;
    //    bool enableWalking;
    //    float blendDuration;

    //    public ThirdPersonController(SceneNode rootNode)
    //    {
    //        states = new SecuenceStateMachine();
    //        this.rootNode = rootNode;
    //        this.resetter = new BonesResetter(rootNode);

    //        rootNode.IsDynamic = true;
    //        rootNode.UpdateEvent += rootNode_UpdateEvent;
    //    }

    //    void rootNode_UpdateEvent(IDynamic sender, float deltaT)
    //    {
    //        throw new NotImplementedException();
    //    }


    //    public virtual void SetIdleAnimation(Animation animation, float startTime, float duration, AnimationLooping looping = AnimationLooping.Secuential)
    //    {
    //        states.WithState(new SecuenceNode(IDLE, animation, startTime, duration, looping)
    //            .AfterUpdate(Idle));
    //    }

    //    public virtual void SetWalkAnimation(Animation animation, float startTime, float duration)
    //    {
    //        //sample initial translation offset 
    //        animation.Sample(startTime);
    //        initialKeyValue = rootNode.LocalPosition;

    //        //sample last translation offset
    //        animation.Sample(startTime + duration);
    //        lastKeyValue = rootNode.LocalPosition;

    //        states.WithState(new SecuenceNode(WALK, animation, startTime, duration, AnimationLooping.Secuential)
    //            .Deactivating(node => enableWalking = false)
    //            .AfterUpdate(Walk));
    //    }

    //    public void Initialize()
    //    {
    //        states.WithTransition(IDLE,WALK, new SecuenceTransition(blendDuration)
    //            .FiredWhen(IdleWalkFired)
    //            .WhenBlending(IdleWalkBlending));
    //    }

    //    protected virtual void Idle(SecuenceNode secuence, float elapsedTime)
    //    {

    //    }

    //    protected virtual void Walk(SecuenceNode secuence, float elapsedTime)
    //    {
    //        if (!enableWalking)
    //        {
    //            lastAnimTrans = rootNode.LocalPosition;
    //            enableWalking = true;
    //            return;
    //        }

    //        var cursor = secuence.Animations[0].Cursor;
    //        Vector3 disp = new Vector3();
    //        if (!cursor.TimeRestart)
    //            disp = rootNode.LocalPosition - lastAnimTrans;
    //        else
    //            disp = rootNode.LocalPosition - (cursor.PlayDirection > 0 ? initialKeyValue : lastKeyValue);

    //        disp = Vector3.Transform(disp, Matrix.RotationY(currentHeading));

    //        currentTranslation += disp;
    //        lastAnimTrans = rootNode.LocalPosition;
    //    }

    //    protected virtual void Run(SecuenceNode secuence, float elapsedTime)
    //    {

    //    }

    //    protected virtual void Jump(SecuenceNode secuence, float elapsedTime)
    //    {

    //    }

    //    #region Idle  - Walk

    //    protected virtual void IdleWalkBlending(SecuenceTransition transition, float elapsedTime)
    //    {
    //        Vector3 disp = new Vector3();
    //        var cursor = transition.DestNode.Animations[0].Cursor;
    //        if (Engine.KeyBoard.IsKeyPressed(Keys.UPARROW))
    //            disp = blendingTranslSpeed;
    //        else
    //            disp = -blendingTranslSpeed;
    //        currentTranslation += Vector3.Transform(disp, Matrix.RotationY(currentHeading));
    //    }

    //    protected virtual bool IdleWalkFired(SecuenceTransition transition)
    //    {
    //        var destNode = transition.DestNode;
    //        if (Engine.KeyBoard.IsKeyPressed(Keys.UPARROW))
    //        {
    //            destNode.PlayDirection = 1;
    //            enableWalking = false;
    //            return true;
    //        }
    //        else if (Engine.KeyBoard.IsKeyPressed(Keys.UPARROW))
    //        {
    //            destNode.PlayDirection = -1;
    //            enableWalking = false;
    //            return true;
    //        }
    //        return false;
    //    }

    //    protected virtual void WalkIdleBlending(SecuenceTransition transition, float elapsedTime)
    //    {

    //    }

    //    protected virtual bool WalkIdleFired(SecuenceTransition transition)
    //    {
    //        if (!Engine.KeyBoard.IsKeyPressed(Keys.UPARROW) && !Engine.KeyBoard.IsKeyPressed(Keys.DOWNARROW))
    //        {
    //            enableWalking = false;
    //            return true;
    //        }
    //        return false;
    //    }

    //    #endregion

    //    #region Walk - Runing

    //    protected virtual void WalkRunBlending(SecuenceTransition transition, float elapsedTime)
    //    {

    //    }

    //    protected virtual void WalkRunFired(SecuenceTransition transition, float elapsedTime)
    //    {

    //    }

    //    protected virtual void RunWalkBlending(SecuenceTransition transition, float elapsedTime)
    //    {

    //    }

    //    protected virtual void RunWalkFired(SecuenceTransition transition, float elapsedTime)
    //    {

    //    }

    //    #endregion

    //    #region Jump - Any

    //    protected virtual void JumpAnyBlending(SecuenceTransition transition, float elapsedTime)
    //    {

    //    }

    //    protected virtual void JumpAnyFired(SecuenceTransition transition, float elapsedTime)
    //    {

    //    }

    //    protected virtual void AnyJumpBlending(SecuenceTransition transition, float elapsedTime)
    //    {

    //    }

    //    protected virtual void AnyJumpFired(SecuenceTransition transition, float elapsedTime)
    //    {

    //    }

    //    #endregion
    //}
}
