using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Doom_SarcophageVisual : MonoBehaviour
{
    [SerializeField] private List<GameObject> branch0State;
    [SerializeField] private List<GameObject> branch1State;
    [SerializeField] private List<GameObject> branch2State;
    [SerializeField] private List<GameObject> branch3State;


    public void ActualizeBranchVisual(int branchToActualize, int BranchActualState)
    {
        if(branchToActualize == 0)
        {
            for (int i = 0; i < branch0State.Count; i++)
            {
                branch0State[i].GetComponent<Image>().color = Color.white;
            }
            branch0State[BranchActualState].GetComponent<Image>().color = Color.red;
        }
        else if (branchToActualize == 1)
        {
            for (int i = 0; i < branch1State.Count; i++)
            {
                branch1State[i].GetComponent<Image>().color = Color.white;
            }
            branch1State[BranchActualState].GetComponent<Image>().color = Color.red;
        }
        else if (branchToActualize == 2)
        {
            for (int i = 0; i < branch2State.Count; i++)
            {
                branch2State[i].GetComponent<Image>().color = Color.white;
            }
            branch2State[BranchActualState].GetComponent<Image>().color = Color.red;
        }
        else if (branchToActualize == 3)
        {
            for (int i = 0; i < branch3State.Count; i++)
            {
                branch3State[i].GetComponent<Image>().color = Color.white;
            }
            branch3State[BranchActualState].GetComponent<Image>().color = Color.red;
        }
    }
}
