using Unity.Entities;

namespace Crimson.Core.Utils.LowLevel
{
	[UpdateInGroup(typeof(SimulationSystemGroup))]
	public class FixedUpdateGroup : ComponentSystemGroup
	{
	}
}