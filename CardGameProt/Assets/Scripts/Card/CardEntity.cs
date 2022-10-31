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
    public SPELL spell;
}

public enum ABILITY
{
    NONE,
    FAST_ATTACK,
    SHIELD,
}

public enum SPELL
{
    NONE,
    DAMAGE_ENEMY,
    DAMAGE_ENEMYS,
    DAMAGE_ENEMY_PLAYER,
    HEAL_FRIEND,
    HEAL_FRIENDS,
    HEAL_PLAYER
}
