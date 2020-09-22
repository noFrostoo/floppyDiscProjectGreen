using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FloppyDiscProjectGreen
{
namespace Abilites
{
public abstract class AbilityBaseGenerics<T> : AbilityBase where T : Component
{
    // class to make possible making static methods and other shit that  cannot be done with abstract classes
}
}
}