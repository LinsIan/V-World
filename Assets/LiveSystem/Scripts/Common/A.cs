using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A : MonoBehaviour
{
    [SerializeField] private ExampleDependencyMonoBehaviour dependency = null;
    
    private void Awake() {
        dependency = DependenciesContainer.Dependencies.Get<ExampleDependencyMonoBehaviour>();
    }
}
