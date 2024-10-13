using develop_common;
using develop_timeline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableBox : MonoBehaviour
{
    public GameObject DirectorPlayerPrefab;

    private string _hitObjectName = "Player";

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Play(other.gameObject);
    }

    private async void Play(GameObject other)
    {
        if (other.name != _hitObjectName) return;
        bool check = true;
        if (!check) return;

        if (DirectorPlayerPrefab != null)
        {
            var player = DirectorManager.Instance.Player;
            var enemy = transform.parent.GetComponent<Animator>();

            var director = Instantiate(DirectorPlayerPrefab, transform.position, Quaternion.identity);
            if(director.TryGetComponent<DirectorPlayer>(out var dp)) dp.OnSetPlayDirector(player, enemy);
            gameObject.SetActive(false);
        }
    }
}
