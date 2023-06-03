using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class cardDisplay : MonoBehaviour
{
   public Card card;

   public TextMeshProUGUI Name;
   public GameObject CardArt;

   public TextMeshProUGUI Effect;

   public TextMeshProUGUI Stat;
    public TextMeshProUGUI Cost;

   //Will Have to be called when the player pulls a new card from the deck
   public void UpdateCardInformation(Card newCard){ card = newCard; }
   //Draws the current card to the screen
   public void UpdateCardDisplay()
   {
      Name.text = card.name;
      CardArt.GetComponent<Image>().sprite = card.Artwork;
      Effect.text = card.StatusEffect;
      Stat.text = card.Damage.ToString();
      Cost.text = card.EnergyCost.ToString();
   }

}
