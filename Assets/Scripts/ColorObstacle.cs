using UnityEngine;
using Zenject;
namespace Samurai
{
    public class ColorObstacle: ColorObject
    {
        //[Inject]
        private Player Player;
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
        public override void ChangeColor(PhaseColor color){}
        
    }
}