using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anim;
    public Transform player, destination;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       StartCoroutine(RunAnimation());
    }

    public IEnumerator RunAnimation()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetBool("IsWalking", true);
            yield return new WaitForSeconds(10f);
        }
        else if (player.position == destination.position)
        {
            anim.SetBool("IsWalking", false);
        }
    }
}
