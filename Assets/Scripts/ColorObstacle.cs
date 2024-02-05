using UnityEngine;
using Zenject;
namespace Samurai
{
    public class ColorObstacle: ColorObject, IChangeColor
    {
        [Inject]
        private Player Player; //todo inj
        private void OnEnable()
        {
            Player.OnPlayerSwapColor += ChangeColor;
        }
        private void OnDisable()
        {
            Player.OnPlayerSwapColor -= ChangeColor;
        }
        public void ChangeColor()
        {
            
        }
        public void ChangeColor(PhaseColor color){}
        
    }
}