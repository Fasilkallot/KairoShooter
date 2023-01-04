using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletCount : MonoBehaviour
{
    private PlayerControler playerControler;
    [SerializeField]
    Text bulletCount;
    [SerializeField]
    Text score;
    // Start is called before the first frame update
    void Start()
    {
        playerControler= GetComponent<PlayerControler>();
    }

    // Update is called once per frame
    void Update()
    {
        bulletCount.text = playerControler.bulletCount.ToString();
        score.text = playerControler.score.ToString();
    }
}
