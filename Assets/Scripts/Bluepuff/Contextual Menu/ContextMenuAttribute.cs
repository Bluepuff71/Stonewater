using System;
namespace Bluepuff.Contextual
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ContextMenuAttribute : Attribute
    {
        private string buttonLabel;

        public ContextMenuAttribute(string buttonLabel)
        {
            this.buttonLabel = buttonLabel;
        }

        public virtual string ButtonLabel
        {
            get { return buttonLabel; }
        }
    }
}
