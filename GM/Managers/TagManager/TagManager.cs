using System;
using System.Collections.Generic;
using IXRE.Scripts;
using Unity_Utilities.GM.Managers.TagManager.Tags;
using UnityEngine;

namespace Unity_Utilities.GM.Managers.TagManager
{
    public partial class TagManager : MonoBehaviour, IManager
    {
        // ### Tag Group Enums ### 
        // Any Public Enum anywhere can be used as Tags
        public enum ExampleTags
        {
            ExampleTag1,
            ExampleTag2,
            ExampleTag3,
            ExampleTag4,
            ExampleTag5,
        }

        public enum ExampleTags2
        {
            ExTag1,
            ExTag2,
            ExTag3,
            ExTag4,
            ExTag5,
        }
    }


    // do not edit below this line unless you know what you are doing
    partial class TagManager : MonoBehaviour, IManager
    {
        /// <summary>
        /// Dictionary of all List for each TagType
        /// </summary>
        private Dictionary<string, List<ITag>> Tags { get; } = new Dictionary<string, List<ITag>>();

        public void RegisterTag<TEnum>(TEnum smartTag, ITag iTag) where TEnum : Enum
        {
            string tagGroup = typeof(TEnum).ToString();
            if (!Tags.ContainsKey(tagGroup))
            {
                Tags.Add(tagGroup, new List<ITag>());
            }

            Tags[tagGroup].Add(iTag);
        }

        public void DeRegisterTag<TEnum>(TEnum smartTag, ITag iTag) where TEnum : Enum
        {
            Tags[typeof(TEnum).ToString()].Remove(iTag);
        }

        // ##### Tag Related Methods #####


        // returns an empty list if none are registered

        /// <summary>
        /// Get All References to objects that are tagged with a Tag of the same Type as the given smartTag
        /// </summary>
        /// <param name="smartTag">A Tag that is defined as an Enum</param>
        /// <typeparam name="TEnum">Type of the Tag</typeparam>
        /// <returns>A list of References to Tagged objects</returns>
        public List<ITag> GetObjectsWithTagType<TEnum>(TEnum smartTag) where TEnum : Enum
        {
            string tagGroup = typeof(TEnum).ToString();
            return Tags.ContainsKey(tagGroup) ? Tags[tagGroup] : new List<ITag>() { };
        }

        /// <summary>
        /// Get All References to objects that are tagged with the given smartTag
        /// </summary>
        /// <param name="smartTag">A Tag that is defined as an Enum</param>
        /// <typeparam name="TEnum">Type of the Tag</typeparam>
        /// <returns>A list of References to Tagged objects</returns>
        public List<ITag> GetObjectsWithTag<TEnum>(TEnum smartTag) where TEnum : Enum
        {
            return GetObjectsWithTagType(smartTag).FindAll(iTag => iTag.GetTag().Equals(smartTag));
        }

        /// <summary>
        /// Get a single Reference to an object that is tagged with the given smartTag
        /// </summary>
        /// <param name="smartTag">A Tag that is defined as an Enum</param>
        /// <typeparam name="TEnum">Type of the Tag</typeparam>
        /// <returns>One Reference of a tagged Object</returns>
        public ITag GetObjectWithTag<TEnum>(TEnum smartTag) where TEnum : Enum
        {
            return GetObjectsWithTagType(smartTag).Find(iTag => iTag.GetTag().Equals(smartTag));
        }

        /// <summary>
        ///  Compares the Tags of two ITag Objects
        /// </summary>
        /// <param name="tag1"></param>
        /// <param name="tag2"></param>
        /// <returns>true if equal, false anytime else</returns>
        public bool CompareTag(ITag tag1, ITag tag2)
        {
            return tag1.GetTag().Equals(tag2.GetTag());
        }

        /// <summary>
        /// Compares the Tag of an ITag Object with a given Enum
        /// </summary>
        /// <param name="tag1"></param>
        /// <param name="tag2"></param>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns>true if they are equal, false else</returns>
        public bool CompareTag<TEnum>(TEnum tag1, ITag tag2) where TEnum : Enum
        {
            return tag1.Equals(tag2.GetTag());
        }
    }
}