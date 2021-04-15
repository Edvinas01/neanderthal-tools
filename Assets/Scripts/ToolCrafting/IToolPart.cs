using UnityEngine;

namespace NeanderthalTools.ToolCrafting
{
    public interface IToolPart
    {
        public Vector3 AttachDirection { get; }

        public string Name { get; }
    }
}
