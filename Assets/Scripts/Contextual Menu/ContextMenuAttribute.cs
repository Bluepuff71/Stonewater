using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Bluepuff
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ContextMenuAttribute : Attribute
    {
        private string commandName;

        public ContextMenuAttribute(string commandName)
        {
            this.commandName = commandName;
        }

        public virtual string CommandName
        {
            get { return commandName; }
        }
    }
}
