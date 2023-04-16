using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using DG.Tweening;

public class Drop : MonoBehaviour
{

    [SerializeField] private TMP_Text valTxt;

    public int Val { get; set; }



    public void InitializeDrop(int val)
    {
        SetValue(val);


    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Drop")) return;

        var com = other.GetComponent<Drop>();
        if (com == null) return;


    }



    private void SetValue(int val)
    {
        Val = val;
        valTxt.text = Val.ToString();
    }

    public async Task Move(float duration, Vector3 destination)
    {

        await transform.DOMove(destination, duration).AsyncWaitForCompletion();


    }


    private void ComeDown()
    {
        var isHit = Physics.Raycast(transform.position, Vector3.down, out var hit);


    }


}
