using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MarioMachineChandelierButton : MonoBehaviour
{
    [SerializeField] Image chandelierVisuals;
    [SerializeField] Swinger chandelierCode;
    [SerializeField] int toadcount;
    [SerializeField] int failCount;
    [SerializeField] int failCountThreshold;
    [SerializeField] UnityEvent tooMuchToads;
    [SerializeField] UnityEvent onHit;
    [SerializeField] UnityEvent Miss;
    [SerializeField] MachineController machine;
    [SerializeField] GameObject button ;

    void Start()
    {
        toadcount = 0;
        failCount = 0;
    }

    public void OnClick()
    {
        if (chandelierCode.isInRange)
        {
            ++toadcount;
            onHit.Invoke();
        }
        if (!chandelierCode.isInRange)
        {
            if (failCountThreshold > failCount)
            {
                Miss.Invoke();
                ++failCount;
            }
        }
        if (toadcount > 10)
        {
            chandelierVisuals.color = Color.red;
        }
        if (toadcount > 20)
        {
            chandelierVisuals.color = Color.blue;
        }
        if (toadcount > 30)
        {
            chandelierVisuals.color = Color.green;
            tooMuchToads.Invoke();
        }
    }

    public void EndAnim()
    {
        StartCoroutine(EndAnimation());
    }

    public IEnumerator EndAnimation()
    {
        button.GetComponent<Button>().interactable = false;
        chandelierCode.GoToMiddle(1f);
        yield return new WaitForSeconds(1f);
        chandelierCode.Shake(0.5f);
        yield return new WaitForSeconds(0.5f);
        machine.OnCloseButton();
    }
}
