using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FancyDeath : MonoBehaviour
{
    // Time delay before "actual" death kicks in
    [SerializeField] float time = 2;
    public void StartAnimation(Movement player)
    {
        StartCoroutine(Animation());
        IEnumerator Animation()
        {
            float elapse = 0;
            while(elapse < time)
            {
                elapse += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            player.Die(true);
        }
    }
}
