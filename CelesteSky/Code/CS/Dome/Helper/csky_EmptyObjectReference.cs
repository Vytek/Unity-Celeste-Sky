/////////////////////////////////////////////////
/// Celeste Sky
///----------------------------------------------
/// Dome.
///----------------------------------------------
/// Empty Object Instantiate.
///----------------------------------------------
/// Description: Instantiate empty object helper
/////////////////////////////////////////////////

using System;
using UnityEngine;

namespace CelesteSky
{
    [Serializable] public class csky_EmptyObjectReference
    {

        public GameObject gameObject;
        public Transform transform;

        /// <summary> Check if all components are asigned. </summary>
        public virtual bool CheckComponents
        {
            get
            {
                if(gameObject == null) return false;
                if(transform  == null) return false;
                return true;
            }
        }

        /// <summary> Initialized transform in defautl values </summary>
        /// <param bame="parent"> Parent Transform </param>
        /// <param bame="posOffset"> Position Offset </param>
        public void InitTransform(Transform parent, Vector3 posOffset)
        {

            if(transform == null) return;

            transform.parent        = parent;
            transform.position      = Vector3.zero + posOffset;
            transform.localPosition = Vector3.zero + posOffset;
            transform.rotation      = Quaternion.identity;
            transform.localRotation = Quaternion.identity;
            transform.localScale    = Vector3.one;
        }

        /// <summary> Instantiate Object. </summary>
        /// <param bame="parentName"> Parent Name </param>
        /// <param bame="name"> Name </param>
        public virtual void Instantiate(string parentName, string name)
        {

            if(gameObject == null)
            {
                // Check if exist gameobject with this name.
                var childObj = GameObject.Find("/" + parentName + "/" + name); 
                if(childObj != null)
                    gameObject = childObj;
                else
                    gameObject = new GameObject(name);
            }

            if(transform == null)
                transform = gameObject.transform; // Get transform.
        }

        /// <summary> Instantiate Object. </summary>
        /// <param bame="rootName"> Root Name </param>
        /// <param bame="parentName"> Parent Name </param>
        /// <param bame="name"> Name </param>
        public virtual void Instantiate(string rootName, string parentName, string name)
        {

            if(gameObject == null)
            {
                // Check if exist gameobject with this name.
                var childObj = GameObject.Find("/" + rootName + "/" + parentName + "/" + name); 
                if (childObj != null)
                    gameObject = childObj;
                else
                    gameObject = new GameObject(name);
            }

            if(transform == null)
                transform = gameObject.transform; // Get transform.

        }

    }
}
