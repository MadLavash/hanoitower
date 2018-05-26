using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuBehaviour : MonoBehaviour
{

    [SerializeField]
    private InputField inputField;
    [SerializeField]
    private Text placeHolderText;
    [SerializeField]
    private Button mainBtn;

    private const string EMPTY = "Empty input!";
    private const string WRONG = "Wrong input!";
    private const string ZERO = "Too small amount of disks!";

    public static event System.Action<int> amountOfDisksChoosed;

    private void Awake()
    {
        mainBtn.onClick.AddListener(MainBtnPressed);
    }

    private void MainBtnPressed()
    {
        int amountOfDisks = 0;

        if (string.IsNullOrEmpty(inputField.text))
        {
            placeHolderText.text = EMPTY;
        }
        else if(!int.TryParse(inputField.text, out amountOfDisks))
        {
            placeHolderText.text = WRONG;
            inputField.text = "";
        }
        else if(amountOfDisks == 0)
        {
            placeHolderText.text = ZERO;
            inputField.text = "";
        }
        else
        {
            if(amountOfDisksChoosed != null)
            {
                amountOfDisksChoosed(amountOfDisks);
            }

            gameObject.SetActive(false);
        }
    }

}
