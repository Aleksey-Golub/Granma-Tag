using System;

namespace Assets.CodeBase.Datas
{
    [Serializable]
    public class WorldData
    {
        public int LevelNumber;

        public WorldData(int levelNumber)
        {
            LevelNumber = levelNumber;
        }
    }
}
