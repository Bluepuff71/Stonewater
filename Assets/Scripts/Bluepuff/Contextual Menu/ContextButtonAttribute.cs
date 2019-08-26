using System;
namespace Bluepuff.Contextual
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ContextButtonAttribute : Attribute
    {
        private string buttonLabel;

        public ContextButtonAttribute(string buttonLabel)
        {
            this.buttonLabel = buttonLabel;
        }

        public virtual string ButtonLabel
        {
            get { return buttonLabel; }
        }
    }
}
