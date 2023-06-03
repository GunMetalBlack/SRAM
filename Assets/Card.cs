using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public  string CardName;
    public string Description;
    public Sprite Artwork;
    public int EnergyCost;
    public string StatusEffect;
    public bool isAttack;
    public int Damage;
    public int AttackAmount;

}
