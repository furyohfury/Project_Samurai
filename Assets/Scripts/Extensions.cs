using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
namespace Samurai
{
    public delegate void SimpleHandle();    

    public delegate void ChangeColorHandle(PhaseColor color);

    public delegate void RangeWeaponChangeHandle(RangeWeaponEnum weapon);

    public delegate void UnitHandle(Unit unit);

    [System.Serializable]
    public class MaterialColorDictionary : SerializableDictionaryBase<PhaseColor, Material> { }

    [System.Serializable]
    public class WeaponSpriteDictionary : SerializableDictionaryBase<RangeWeaponEnum, Sprite> { }

    public static class StructsTransformer<T>
    {

    }
}