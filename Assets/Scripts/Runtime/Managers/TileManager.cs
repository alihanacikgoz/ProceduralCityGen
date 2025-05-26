using System.Collections.Generic;
using Runtime.Classes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Managers
{
    public class TileManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] tilesPrefabs;
        [SerializeField] private DirectionInfo[] directions;

        Tile[,] _cityGrid;
        private int _gridSize = 10;
        private int _tileSize = 100;

        private void Start()
        {
            _cityGrid = new Tile[_gridSize, _gridSize];
            GenerateCity();
            ChekcAdjacentTiles();
        }

        void GenerateCity()
        {
            for (int y = 0; y < _gridSize; y++)
            {
                for (int x = 0; x < _gridSize; x++)
                {
                    Vector3 pos = GetTilePosition(x, y);
                    GameObject goTile = Instantiate(tilesPrefabs[0], pos, Quaternion.identity, transform);
                    _cityGrid[x, y] = new Tile(goTile, pos, 0);
                }
            }

            int totalTiles = _gridSize * _gridSize;
            int numCrossings = Mathf.Max(1, Mathf.FloorToInt(totalTiles * 0.1f));
            List<Vector2Int> chosenPositions = new List<Vector2Int>();
            while (chosenPositions.Count < numCrossings)
            {
                int posX = Random.Range(0, _gridSize);
                int posY = Random.Range(0, _gridSize);
                Vector2Int pos = new Vector2Int(posX, posY);
                if (!chosenPositions.Contains(pos))
                {
                    chosenPositions.Add(pos);
                    ReplaceTile(posX, posY, 15);
                }
            }
        }


        void ChekcAdjacentTiles()
        {
            for (int y = 0; y < _gridSize; y++)
            {
                for (int x = 0; x < _gridSize; x++)
                {
                    Tile currentTile = _cityGrid[x, y];
                    if (currentTile.bitValue > 0)
                    {
                        foreach (DirectionInfo dir in directions)
                        {
                            if ((currentTile.bitValue & dir.bit) != 0)
                            {
                                int myX = x + dir.offset.x;
                                int myY = y + dir.offset.y;

                                if (myX >= 0 && myX < _gridSize && myY >= 0 && myY < _gridSize)
                                {
                                    Tile adjacentTile = _cityGrid[myX, myY];
                                    if (adjacentTile.bitValue == 0)
                                    {
                                        int reqBit = GetOppositeBit(dir.bit);
                                        List<int> availableTiles = GetAvailableTiles(reqBit);

                                        if (availableTiles.Count > 0)
                                        {
                                            int randIndex = Random.Range(0, availableTiles.Count);
                                            int bitVal = availableTiles[randIndex];
                                            ReplaceTile(myX, myY, bitVal);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        int GetOppositeBit(int bit)
        {
            switch (bit)
            {
                case 1: return 4;
                case 2: return 8;
                case 4: return 1;
                case 8: return 2;
                default: return 0;
            }
        }

        List<int> GetAvailableTiles(int reqBit)
        {
            List<int> indexList = new List<int>();
            for (int i = 0; i < tilesPrefabs.Length; i++)
            {
                if ((i & reqBit) == reqBit)
                {
                    indexList.Add(i);
                }
            }

            return indexList;
        }

        private void ReplaceTile(int x, int y, int index)
        {
            Tile tile = _cityGrid[x, y];
            Destroy(tile.tileObject);
            GameObject goTile = Instantiate(tilesPrefabs[index], tile.position, Quaternion.identity, transform);
            tile.tileObject = goTile;
            tile.bitValue = index;
        }

        Vector3 GetTilePosition(int x, int y)
        {
            float offset = _gridSize * _tileSize / 2f - _tileSize / 2f;
            float worldX = x * _tileSize - offset;
            float worldz = y * _tileSize - offset;
            return new Vector3(worldX, 0, worldz);
        }
    }
}