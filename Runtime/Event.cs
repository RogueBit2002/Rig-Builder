using Codice.Client.Common.GameUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LaurensKruis.RigBuilder
{
    public class Event
    {
        private HashSet<Action> callbacks = new HashSet<Action>();

        public void Bind(Action callback)
        {
            if (callback.Target == null)
                throw new ArgumentException($"Method {callback.Method.Name} can't be static");
            if (!(callback.Target is RigModule))
                throw new ArgumentException($"Type {callback.Target.GetType()} does not extend RigModule");

            callbacks.Add(callback);
        }

        public void Unbind(Action callback)
        {
            callbacks.Remove(callback);
        }


        internal void Invoke(IEnumerable<RigModule> modules)
        {
            List<RigModule> moduleList = modules.ToList();

            callbacks.Where(a => ((RigModule) a.Target).isActiveAndEnabled).OrderBy(
                a => modules.Contains(a.Target) 
                ? moduleList.IndexOf(a.Target as RigModule) 
                : throw new InvalidOperationException($"Module collection doesn't contain {a.Target}"))
            .ToList().ForEach(a => a.Invoke());
        }
    }
}