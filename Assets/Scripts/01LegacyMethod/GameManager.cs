using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LegacyMethod
{
    public class GameManager : MonoBehaviour
    {
        #region GAME_MANAGER_STUFF

        //Boilerplat game manager stuff that is the same in each example
        public static GameManager GM;

        [Header("Simulation Settings")]
        public float topBound = 16.5f;
        public float bottomBound = -13.5f;
        public float leftBound = -23.5f;
        public float rightBound = 23.5f;

        [Header("Enemy Settings")]
        public GameObject enemyShipPrefab;
        public float enemySpeed = 1f;

        [Header("Spawn Settings")]
        public int enemyShipCount = 1;
        public int enemyShipIncremement = 1;

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
        void Start()
        {
            fps = GetComponent<FPS>();

            AddShips(enemyShipCount);
        }

        void Update()
        {
            if (Input.GetKeyDown("space"))
                AddShips(enemyShipIncremement);
        }

        void AddShips(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                float xVal = UnityEngine.Random.Range(leftBound, rightBound);
                float zVal = UnityEngine.Random.Range(bottomBound, topBound);
                //float yVal = UnityEngine.Random.Range(100, -400);
                Vector3 pos = new Vector3(xVal, 0, zVal + topBound);
                Quaternion rot = Quaternion.Euler(0f, 180f, 0f);

                var obj = Instantiate(enemyShipPrefab, pos, rot) as GameObject;
            }

            count += amount;
            fps.SetElementCount(count);
        }
    }
}

