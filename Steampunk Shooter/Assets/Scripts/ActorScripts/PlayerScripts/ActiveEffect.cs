using UnityEngine;
using System.Collections;

//TODO: Unity does not allow instanciating MonoBehaviors using "new", but it still
//does it, just raising a warning. Maybe change this to a regular class later?
public class ActiveEffect : MonoBehaviour
{
    public EffectType EffType { get; private set; }
    public float Value { get; private set; }
    public float RemTime { get; private set; }

    public ActiveEffect(EffectType effType, float val, float time)
    {
        EffType = effType;
        Value = val;
        RemTime = time;
    }

    void Update()
    {
        RemTime -= Time.deltaTime;
        if (RemTime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public ActiveEffect Clone(ActiveEffect eff)
    {
        EffType = eff.EffType;
        Value = eff.Value;
        RemTime = eff.RemTime;
        return this;
    }

    /// <summary>
    /// An identifier for effect type. EffectType.Length is the total number of different effects.
    /// </summary>
    public enum EffectType : byte
    {
        None = 0,
        MovSpdAdd,
        MovSpdMul,
        AtkSpdAdd,
        AtkSpdMul,
        Length
    }
}
