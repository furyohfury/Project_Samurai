using UnityEngine;
namespace Samurai
{
    public delegate void SimpleHandle();

    public delegate void ChangeColorHandle(PhaseColor color);

    public interface IChangeColor
    {
        void ChangeColor();
        void ChangeColor(PhaseColor color);
    }
}