using System;

namespace Igneel.Assets
{
    public class OnCompleteAttribute : Attribute
    {
        public string OnComplete { get; set; }        

        public OnCompleteAttribute(string methodName)
        {
            OnComplete = methodName;
        }

        public void InvokeComplete(object target)
        {
            if (OnComplete == null) return;

            var method = target.GetType().GetMethod(OnComplete);

            method.Invoke(target, null);
        }

    }
}