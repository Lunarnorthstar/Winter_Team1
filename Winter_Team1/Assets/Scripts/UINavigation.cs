using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINavigation : MonoBehaviour
{
    public GameObject eyeButtons;
    public GameObject noseButtons;
    public GameObject armButtons;
    public GameObject miscButtons;
    public GameObject gloveButtons;
    
    
    
    
    public void ButtonInput(string input)
    {
        switch (input)
        {
            case "Eye": eyeButtons.SetActive(true); noseButtons.SetActive(false); armButtons.SetActive(false); miscButtons.SetActive(false); gloveButtons.SetActive(false);
                break;
            case "Nose": eyeButtons.SetActive(false); noseButtons.SetActive(true); armButtons.SetActive(false); miscButtons.SetActive(false); gloveButtons.SetActive(false);
                break;
            case "Arm": eyeButtons.SetActive(false); noseButtons.SetActive(false); armButtons.SetActive(true); miscButtons.SetActive(false); gloveButtons.SetActive(false);
                break;
            case "Misc": eyeButtons.SetActive(false); noseButtons.SetActive(false); armButtons.SetActive(false); miscButtons.SetActive(true); gloveButtons.SetActive(false);
                break;
            case "Glove": eyeButtons.SetActive(false); noseButtons.SetActive(false); armButtons.SetActive(false); miscButtons.SetActive(false); gloveButtons.SetActive(true);
                break;
            default: eyeButtons.SetActive(false); noseButtons.SetActive(false); armButtons.SetActive(false); miscButtons.SetActive(false); Debug.Log("Uh... something broke? Make sure the string is right. Remember, Case Sensitive!");
                break;
        }
    }
}
