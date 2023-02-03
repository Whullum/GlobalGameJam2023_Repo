using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeAbility : MonoBehaviour
{
    // Start is called before the first frame update
    private bool dodgeActive = false;
    public float dodgeSpeed = 15f;
    public float dodgeCooldown = 0.1f;
    private float prevSpeed;
    PlayerController playerControls;
    Rigidbody2D rb;
    void Start()
    {
        
        playerControls = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        PlayerDodge();
    }
    void PlayerDodge()
    {
        ///<summary>
        ///Allows player to perform a dodge by pressing the space button. 
        /// </summary>
        
        
        if (Input.GetButtonUp("Jump") && Time.time > playerControls.GetTimeLimit() && dodgeActive == false)
        {

            playerControls.GetAnimator().enabled = false;
            playerControls.GetAnimator().SetTrigger("dodge");
            playerControls.GetAnimator().enabled = true;
            if (playerControls.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName("walk") == false)
            {
                prevSpeed = playerControls.GetSpeed();
                playerControls.SetSpeed(dodgeSpeed);
                rb.MovePosition(rb.position + playerControls.GetMovementDirection() * playerControls.GetSpeed() * Time.fixedDeltaTime);

                playerControls.AddToTimeLimit(dodgeCooldown);
                dodgeActive = true;
            }
            
            //Debug.Log(timeLimit);

            

        }
        else if (Time.time < playerControls.GetTimeLimit() && dodgeActive)
        {
            //Debug.Log(Time.time);
        }
        else if (Time.time > playerControls.GetTimeLimit() &&  dodgeActive)
        {
            Debug.Log("Cooldown Ended");
            dodgeActive = false;
            playerControls.ResetTimeLimit();
            playerControls.SetSpeed(prevSpeed);
        }

    }
}
