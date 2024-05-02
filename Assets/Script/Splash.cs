using System.Collections;
using UnityEngine;




namespace Script
{
    
    public class Splash : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start()
        {
            Debug.Log("hello");
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Func();
            }
        }

        private void Func()

        {
            StartCoroutine(nameof(Sa));
        }

        private IEnumerator Sa()
        {
            yield return new WaitForSeconds(.5f);
            print("hello");
        }
    }
}