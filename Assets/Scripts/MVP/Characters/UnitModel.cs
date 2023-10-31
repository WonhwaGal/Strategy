using System;

namespace Code.Units
{
    public class UnitModel : IDisposable
    {
        public UnitModel(int hp, float speed)
        {
            HP = hp;
            Speed = speed;
        }

        public int HP { get; set; }
        public float Speed { get; set; }

        public void Dispose()
        {
        }
    }
}