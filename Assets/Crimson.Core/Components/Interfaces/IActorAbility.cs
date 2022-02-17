using Crimson.Core.Common;
using Unity.Entities;

namespace Crimson.Core.Components
{
	public interface IActorAbility
	{
		IActor Actor { get; set; }

		void AddComponentData(ref Entity entity, IActor actor);

		void Execute();
	}
}