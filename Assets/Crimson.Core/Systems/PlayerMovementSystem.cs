using Crimson.Core.Components;
using Crimson.Core.Utils.LowLevel;
using Unity.Entities;
using Unity.Mathematics;

namespace Crimson.Core.Systems
{
	[UpdateInGroup(typeof(FixedUpdateGroup))]
	public class PlayerMovementSystem : ComponentSystem
	{
		private EntityQuery _movementQuery;

		protected override void OnCreate()
		{
			_movementQuery = GetEntityQuery(
				ComponentType.ReadOnly<PlayerInputData>(),
				ComponentType.ReadOnly<ActorMovementData>()
				);
		}

		protected override void OnUpdate()
		{
			Entities.With(_movementQuery).ForEach(
				(ref PlayerInputData input, ref ActorMovementData movement) =>
				{
					var inputVector = input.Move;//MathUtils.RotateVector(input.Move, 0 - input.CompensateAngle);
					var tempInput = input.Move;
					if (input.MinMagnitude == 0f)
					{
						movement.Input = new float3(inputVector.x, 0f, inputVector.y);
					}
					else
					{
						if ((math.lengthsq(inputVector) > 0) && (math.lengthsq(inputVector) < (input.MinMagnitude * input.MinMagnitude)))
						{
							tempInput = math.normalize(inputVector) * input.MinMagnitude;
						}
						movement.Input = new float3(tempInput.x, 0f, tempInput.y);
					}
				});
		}
	}
}