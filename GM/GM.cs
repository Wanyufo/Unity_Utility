using System;
using Unity_Utilities.GM.Managers.TagManager;
using UnityEngine;

namespace Unity_Utilities.GM
{
    public partial class GM : MonoBehaviour
    {
        [NonSerialized] public TagManager TagManager;


        /// <summary>
        /// Local Awake Function, Same as MonoBehaviour Awake, but is only called on the unique Singleton
        /// </summary>
        private void GmAwake()
        {
        }


        /// <summary>
        /// Find all Managers and fill the Manager references
        /// Needs to be Updated Manually when new Manager Types are added
        /// </summary>
        private void GatherManagers()
        {
            // generically find all managers and assign them to the variable with the same name
            TagManager = FindObjectOfType<TagManager>();
        }
    }


    // do not edit below this line unless you know what you are doing
    partial class GM : MonoBehaviour
    {
        //############## SINGLETON INITIALISATION ###################
        public static GM I { get; private set; }

        private void Awake()
        {
            if (I == null)
            {
                I = this;
                GatherManagers();
                GmAwake();
            }
            else
            {
                Destroy(this);
                Debug.LogError("A second GM existed, has been destroyed");
                return;
            }
        }
    }
}