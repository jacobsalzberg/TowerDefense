using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

    public GameObject gameManager;

    private void Awake()
    {
        KeyValuePair<int, string> course = new KeyValuePair<int, string>(1, "Jacob");
        course.Print();
    }

    public class KeyValuePair<TKey,Tvalue> 
    {
        public TKey key;
        public Tvalue value;
        
        //initializer
        public KeyValuePair (TKey _key, Tvalue _value)
        {
            key = _key;
            value = _value;
        }

        public void Print()
        {
            Debug.Log("Key:" + key.ToString());
            Debug.Log("Value:" + value.ToString());
        }
    }

    
}
