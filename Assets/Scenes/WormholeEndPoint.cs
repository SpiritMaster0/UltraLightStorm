using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormholeEndPoint : MonoBehaviour {
    public static WormholeEndPoint Instance { get; private set; }
    public BoxCollider2D boxCollider;
    private Wormhole wormhole;
    [SerializeField] private GameObject _highlight;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        wormhole = FindObjectOfType<Wormhole>();
    }
    void Update() {
       if (wormhole.wormholeLifeSpan >= wormhole.wormholePointTime && wormhole.wormholeLifeSpan <= wormhole.wormholePointTime + 10) {
        _highlight.SetActive(true);
       } else {
         _highlight.SetActive(false);
       }
    }

    public void UpdateEndPointCollider(Vector2 endPoint)
    {
        boxCollider.size = new Vector2(4f, 4f);
        boxCollider.transform.position = endPoint;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (wormhole != null && wormhole.wormholePointTracker < wormhole.wormholePoints.Length + 1)
        {
            wormhole.UpdateCollider();
            Debug.Log("Triggered endPointCollider");
        }
    }
}