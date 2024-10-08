using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerAction : MonoBehaviour
{

    
    [SerializeField]
    private Knife Knife;
    private Animator animator;
  

  public void Awake()
  {
    animator = GetComponent<Animator>();
  }

}
