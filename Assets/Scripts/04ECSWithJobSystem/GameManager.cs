using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace  ECSWithJobSystemMethod
{
    public class GameManager : MonoBehaviour
    {
        #region GAME_MANAGER_STUFF

        //Boilerplat game manager stuff that is the same in each example
        public static GameManager GM;

        [Header("Simulation Settings")]
        public float topBound = 100f;
        public float bottomBound = -150f;
        public float leftBound = -200f;
        public float rightBound = 200f;

        [Header("Enemy Settings")]
        public GameObject enemyShipPrefab;
        public float enemySpeed = 12f;

        [Header("Spawn Settings")]
        public int enemyShipCount = 2000;
        public int enemyShipIncremement = 500;

        FPS fps;
        int count;
        
        void Awake()
        {
            if (GM == null)
                GM = this;
            else if (GM != this)
                Destroy(gameObject);
        }
        #endregion
        EntityManager manager;


        void Start()
        {
            fps = GetComponent<FPS>();
            manager = World.Active.GetOrCreateManager<EntityManager>();
            AddShips(enemyShipCount);
        }

        void Update()
        {
            if (Input.GetKeyDown("space"))
                AddShips(enemyShipIncremement);
        }

        void AddShips(int amount)
        {
            NativeArray<Entity> entities = new NativeArray<Entity>(amount, Allocator.Temp);
            manager.Instantiate(enemyShipPrefab, entities);

            for (int i = 0; i < amount; i++)
            {
                float xVal = UnityEngine.Random.Range(leftBound, rightBound);
                float zVal = UnityEngine.Random.Range(bottomBound, topBound);
                manager.SetComponentData(entities[i], new Position { Value = new float3(xVal, 0f, topBound + zVal) });
                manager.SetComponentData(entities[i], new Rotation { Value = new quaternion(0, 1, 0, 0) });
                manager.SetComponentData(entities[i], new MoveSpeed { Value = enemySpeed });
            }
            entities.Dispose();
            count += amount;
            fps.SetElementCount(count);
        }
    }
}

