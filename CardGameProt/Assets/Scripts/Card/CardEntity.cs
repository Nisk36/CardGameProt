using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CardEntity", menuName = "Create CardEntity")]
public class CardEntity : ScriptableObject
{
    public new string name;
    public int hp;
    public int attack;
    public int cost;
    public ABILITY ability;
}

public enum ABILITY
{
    NONE,
    FAST_ATTACK,
    SHIELD,
}
