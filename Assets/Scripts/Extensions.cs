using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
namespace Samurai
{
    public delegate void SimpleHandle();

    public delegate void ChangeColorHandle(PhaseColor color);

    [System.Serializable]
    public class MaterialColorDictionary : SerializableDictionaryBase<PhaseColor, Material> { }
}