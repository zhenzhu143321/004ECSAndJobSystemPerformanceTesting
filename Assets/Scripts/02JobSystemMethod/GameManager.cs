
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace JobSystemMethod
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

        TransformAccessArray transforms;
        MovementJob moveJob;
        JobHandle moveHandle;


        void OnDisable()
        {
            moveHandle.Complete();
            transforms.Dispose();
        }

        void Start()
        {
            fps = GetComponent<FPS>();
            transforms = new TransformAccessArray(0, -1);

            AddShips(enemyShipCount);
        }

        void Update()
        {
            moveHandle.Complete();

            if (Input.GetKeyDown("space"))
                AddShips(enemyShipIncremement);

            moveJob = new MovementJob()
            {
                moveSpeed = enemySpeed,
                topBound = topBound,
                bottomBound = bottomBound,
                deltaTime = Time.deltaTime
            };

            moveHandle = moveJob.Schedule(transforms);

            JobHandle.ScheduleBatchedJobs();
        }

        void AddShips(int amount)
        {
            moveHandle.Complete();

            transforms.capacity = transforms.length + amount;

            for (int i = 0; i < amount; i++)
            {
                float xVal = UnityEngine.Random.Range(leftBound, rightBound);
                float zVal = UnityEngine.Random.Range(bottomBound, topBound);
                //float yVal = UnityEngine.Random.Range(100, -400);
                Vector3 pos = new Vector3(xVal, 0, zVal + topBound);
                Quaternion rot = Quaternion.Euler(0f, 180f, 0f);

                var obj = Instantiate(enemyShipPrefab, pos, rot);

                transforms.Add(obj.transform);
            }

            count += amount;
            fps.SetElementCount(count);
        }
    }
}

