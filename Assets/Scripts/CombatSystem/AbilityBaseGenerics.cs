using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FloppyDiscProjectGreen
{
namespace Abilites
{
public abstract class AbilityBaseGenerics<T> : AbilityBase where T : Component
{
    public static void AddAbility(GameObject holder)
    {
        if(holder.GetComponent<T>() == null)
            throw new AbilityAlreadyOnCharacter();
        holder.AddComponent<T>();
    }
}
}
}