using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptTrigger : MonoBehaviour
{
    [SerializeField] GameObject promptPrefab;
    [SerializeField] float offsetY = 0.45f;
    Prompt prompt;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(prompt == null) prompt = Instantiate(promptPrefab, transform.position + new Vector3(0, offsetY, -1), Quaternion.identity, transform).GetComponent<Prompt>();
            else prompt.transform.position = transform.position + new Vector3(0, offsetY, -1);
            prompt.Open();
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && prompt != null)
        {
            prompt.Close();
        }
    }
}
