using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plum.Damage;

namespace Music
{

}
public class Entity : PlumDamageable
{
    public override void Death(IDamageDealer source)
    {
        gameObject.SetActive(false);
    }
}
