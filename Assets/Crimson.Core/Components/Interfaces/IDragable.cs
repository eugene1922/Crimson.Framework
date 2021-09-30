using Unity.Mathematics;

namespace Crimson.Core.Components.Interfaces
{
    public interface IDragable
    {
        void BeginDrag(float2 input);

        void Drag(float2 input);

        void EndDrag(float2 input);
    }
}