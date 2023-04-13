using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Uiscript : MonoBehaviour
{
    public InputAction pause;
    public InputAction back;
    public InputAction next;
    private string[] items = {"Speed+", "Jump+", "Damage+","Health S", "Health L", "Supersonic"};
    private int[] supply = new int[6];
    private int readied;

    private string[] collectibles = {"Coins", "Keys", "Doodads"};
    private int[] collection = new int[3];
    
    private int before;
    public TMP_Text prior;
    public TMP_Text current;
    public TMP_Text latter;
    private int after;

    private GameObject panel;
    public TMP_Text coins;
    public TMP_Text keys;
    public TMP_Text doodads;
    
    private bool paused;
    
    
    void Start()  //sets up all the actions & values
    {
        pause.performed += ctx => { Pause(ctx); };
        back.started += ctx => { Cycleitems(ctx, false); };
        next.started += ctx => { Cycleitems(ctx, true); };
        pause.Enable();
        back.Enable();
        next.Enable();

        panel = transform.GetChild(3).gameObject;
        panel.SetActive(false);
        paused = false;

        readied = 0;
        before = 5;
        after = 1;

        Textupdate();
    }

    void Pause(InputAction.CallbackContext context)
    {
        if (!paused)
        {
            Time.timeScale = 0;
            Debug.Log("rreeeeeeeeeee");
            paused = true;
            panel.SetActive(true);
            coins.text = (collectibles[0] + ": " + collection[0]);
            keys.text = (collectibles[1] + ": " + collection[1]);
            doodads.text = (collectibles[2] + ": " + collection[2]);
        }
        else if (paused)
        {
            Time.timeScale = 1;
            Debug.Log("asdfghjkl");
            paused = false;
            panel.SetActive(false);
        }
    }

    void Cycleitems(InputAction.CallbackContext context, bool forward)  //moves through items. forward = going right / down
    {
        if (forward)
        {
            before = readied;
            after++;
            readied++;
            if (readied > items.GetLength(0)-2)
            {
                after = 0;
                if (readied > items.GetLength(0)-1)
                {
                    readied = 0;
                    after++;
                }
            }
        }
        else
        {
            after = readied;
            before--;
            readied--;
            if (readied < 1)
            {
                before = items.GetLength(0)-1;
                if (readied < 0)
                {
                    readied = items.GetLength(0)-1;
                    before--;
                }
            }
        }
        Textupdate();
    }

    public void Gainitems(int item, bool powerup) //add one of the specified item
    {
        if (powerup)
        { supply[item] += 1; }
        else
        { collection[item-(supply.GetLength(0)+1)] += 1;}
        Textupdate();
    }

    public int Loseitems() //lose one of the held item- and return its number.
    {
        if (supply[readied] > 0)
        {
        supply[readied] -= 1;
        Textupdate();
        return readied;
        }
        else
        {
            return -1;
        }
    }

    public bool Spend(int resource, int quantity)
    {
        if (collection[resource] - quantity >= 0)
        {
            collection[resource] -= quantity;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Textupdate()
    {
        prior.text = items[before]+": "+supply[before];
        current.text = items[readied]+": "+supply[readied];
        latter.text = items[after]+": "+supply[after];
    }
}
