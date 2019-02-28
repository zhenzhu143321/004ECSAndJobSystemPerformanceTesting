using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECSWithJobSystemMethod
{
    public class MovementSystem : JobComponentSystem
    {
        [BurstCompile]
        struct MovementJob : IJobProcessComponentData<Position, Rotation, MoveSpeed>
        {
            public float topBound;
            public float bottomBound;
            public float deltaTime;

            public void Execute(ref Position position, [ReadOnly] ref Rotation rotation, [ReadOnly] ref MoveSpeed speed)
            {
                float3 value = position.Value;

                value += deltaTime * speed.Value * math.forward(rotation.Value);

                if (value.z < bottomBound)
                    value.z = topBound;

                position.Value = value;
            }
        }
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            MovementJob moveJob = new MovementJob
            {
                topBound =200,
                bottomBound = -200,
                deltaTime = Time.deltaTime
            };

            JobHandle moveHandle = moveJob.Schedule(this,inputDeps);

            return moveHandle;
           
        }
    }
}