using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playeract : MonoBehaviour
{
    public InputAction use;
    private PlayerController body;
    private Combat blade;
    private Uiscript inv;

    private ParticleSystem[] auras = new ParticleSystem[5];
    
    private float[] timers = new float[3];
    private char[] timerass = {'X', 'X', 'X'};

    void Start() //connects to the UI, Player & particle effects
    {
        use.performed += ctx => { Use(ctx); };
        use.Enable();
        inv = GameObject.Find("Canvas").GetComponent<Uiscript>();
        body = gameObject.GetComponentInParent<PlayerController>();
        blade = GameObject.Find("Combat").GetComponent<Combat>();
        for (int i = auras.GetLength(0); i > 0; i--)
        {
            auras[i-1] = transform.GetChild(i-1).GetComponent<ParticleSystem>();
        }
    }

    void Update()  //decrements all of the timers
    {
        int t = 0;
        while (timerass[t] != 'X' && t < 3)  //cycles through all timers in use
        {
            if (timers[t] >= 0)
            {
                timers[t] -= Time.deltaTime;  //decreases the time left
            }
            if (timers[t] < 0)
            {
                Done(t);
                timerass[t] = 'X';  //frees a timer if it is done
            }
            t++;
        }
    }

    void Use(InputAction.CallbackContext context)  //activates an item, and starts a countdown timer for it.
    {
        int boon = inv.Loseitems();
        switch (boon)
        {
            case 0:
                Speed();  //gameplay effect
                auras[boon].Play();   //visual effect
                break;
            case 1:
                Jump();
                auras[boon].Play();
                break;
            case 2:
                Damage();
                auras[boon].Play();
                break;
            case 3:
                Heal('S');
                auras[boon].Play();
                break;
            case 4:
                Heal('L');
                auras[boon-1].Play();
                break;
            case 5:
                Sonic();
                auras[boon-1].Play();
                break;
        }
    }

    void Starttimer(char type, float time)  //Finds the first free timer, and allocates it to the item.
    {
        var begun = false;
        var t = 0;
        while (!begun)
        {
            if (timerass[t] == 'X') //finds a tiemr not in use
            {
                timerass[t] = type;  //allocates it with a char to tell what effect it has to stop
                timers[t] = time;
                begun = true;
            }
            else
            {
                t++;
            }
        }
    }

    void Speed()  //doubles walking speed
    {
        body.speed = body.speed * 2;
        Starttimer('S', 15f);
    }

    void Jump()  //doubles jump strength
    {
        body._jumpForce = body._jumpForce * 2;
        Starttimer('J', 15f);
    }

    void Damage()  //doubles player damage
    {
        blade.damage += blade.damage * 2;
        Starttimer('D', 15f);
    }

    void Heal(char size)  //restores health
    {
        if (size == 'S')
        {
            body.playerHealth += 3;
        }
        else if (size == 'L')
        {
            body.playerHealth += 9;
        }
        body.UpdateHealthBar();
    }

    void Sonic()  //sextuples walking speed
    {
        body.speed = body.speed * 6;
        Starttimer('0', 1f);
    }

    void Done(int t)  //resets values after effects wear off
    {
        timers[t] = 0;
        switch (timerass[t])
        {
            case 'S':  //speed
                body.speed = body.speed / 2;
                break;
            case 'J':  //jump
                body._jumpForce = body._jumpForce / 2;
                break;
            case 'D':  //damage
                //flesh.strength = flesh.strength / 2;
                break;
            case 'O':  //supersonic
                body.speed = body.speed / 6;
                break;
        }
    }

}
