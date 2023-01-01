using Codice.CM.Common.Tree.Partial;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace LaurensKruis.RigBuilder
{
    [DisallowMultipleComponent]
    public abstract class RigModule : MonoBehaviour
    {
        private ModularRig rig;
        public ModularRig Rig => Application.isPlaying ? rig : GetRig();

        protected virtual void Awake() => rig = GetRig();

        protected virtual void OnTransformParentChanged() => rig = GetRig();

        private ModularRig GetRig() => transform.parent?.GetComponent<ModularRig>() ?? null;

        //This forces the unity inspector to draw the enable toggle for the component
        private void Start() { }

    }
}