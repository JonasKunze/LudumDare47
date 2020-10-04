using UnityEngine;

namespace DefaultNamespace
{
    public class SerializableObject: MonoBehaviour
    {
        protected BlueprintIndex _blueprintIndex;
        public virtual BlueprintIndex GetBlueprintIndex()
        {
            return _blueprintIndex;
        }
    }
}