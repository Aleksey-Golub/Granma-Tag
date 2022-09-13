using System;

namespace Assets.CodeBase.Datas
{
    [Serializable]
    public class PlayerLootData
    {
        public int Collected;

        public event Action Changed;

        public void Add(int value)
        {
            Collected += value;
            Changed?.Invoke();
        }

        public void Spend(int value)
        {
            Collected -= value;
            Changed?.Invoke();
        }
    }
}
