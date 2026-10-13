using UnityEngine;

[CreateAssetMenu(menuName = "Pet/Definition")]
public class PetDefinition : ScriptableObject {
    public string petId = "default";
    public PetNeed[] needs;
}
