using System;
using UnityEngine;

namespace Runtime.Classes
{
    [Serializable]
    public class Tile
    {
        public GameObject tileObject;
        public Vector3 position;
        public int bitValue;

        public Tile(GameObject tileObject, Vector3 position, int bitValue)
        {
            this.tileObject = tileObject;
            this.position = position;
            this.bitValue = bitValue;
        }
    }

    [Serializable]
    public struct DirectionInfo
    {
        public int bit;
        public Vector2Int offset;

        public DirectionInfo(int bit, int x, int y)
        {
            this.bit = bit;
            offset = new Vector2Int(x, y);
        }
    }
}