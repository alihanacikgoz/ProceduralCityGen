using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileManager : MonoBehaviour
{
   [SerializeField] private GameObject[] tilesPrefabs;
   private GameObject _currentTile;
   [SerializeField] List<int> tileHistory = new List<int>();
   private void Start()
   {
      SpawnRandomTile();
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.Space))
      {
         SpawnRandomTile();
      }
   }


   private void SpawnRandomTile()
   {
      if (_currentTile != null)
      {
         Destroy(_currentTile);
      }

      if (tileHistory.Count >= tilesPrefabs.Length)
      {
         tileHistory.Clear();
      }

      int randomIndex = GetRandomIndex();
      _currentTile = null;
      _currentTile = Instantiate(tilesPrefabs[randomIndex], transform);
      tileHistory.Add(randomIndex);
   }

   int GetRandomIndex()
   {
      int index = Random.Range(0, tilesPrefabs.Length);
      while (tileHistory.Contains(index))
      {
         index = Random.Range(0, tilesPrefabs.Length);
      }
      return index;
   }
}
