using Crimson.Core.Common;

namespace Crimson.Core.Components
{
	public interface IActorAbilityTarget : IActorAbility
	{
		IActor TargetActor { get; set; }
		IActor AbilityOwnerActor { get; set; }
	}
}