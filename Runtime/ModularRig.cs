using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LaurensKruis.RigBuilder
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Laurens Kruis/Rig Builder/Modular Rig")]
    public class ModularRig : MonoBehaviour
    {
        public class Context
        {
            public Event onUpdate = new Event();
        }

        private Context activeContext;

        
        private IEnumerable<RigModule> modules;


        public IEnumerable<RigModule> Modules => Application.isPlaying ? modules : GetChildModules();


        private void Awake() => Invalidate();

        //private void Reset() => Invalidate();

        private void OnTransformChildrenChanged() => Invalidate();

        private void Update() => activeContext.onUpdate.Invoke(modules);

        private void Invalidate()
        {
            IEnumerable<RigModule> currentModules = GetChildModules();

            /*
            //Nothing changed, except maybe module order
            if (currentModules.All(m => modules.Contains(m)) && currentModules.Count() == modules.Count())
                return;
            
            * Even if no modules were created or removed, we should still recreate the context
            */
            modules = currentModules;

            //Don't recreate the context when were in the editor
            if (!Application.isPlaying)
                return;

            activeContext = new Context();

            foreach (RigModule module in modules)
                module.SendMessage("OnContextChange", activeContext, SendMessageOptions.DontRequireReceiver);
        }

        //Because OnTransformChildrenChanged doesn't get called on grandchildren, we can only work with modules that are direct children
        private IEnumerable<RigModule> GetChildModules() => transform.Cast<Transform>().ToList().Select(t => t.GetComponent<RigModule>()).Where(m => m != null);
        public T GetModule<T>() where T : RigModule => modules.Where(m => m is T).Cast<T>().FirstOrDefault();
    }
}