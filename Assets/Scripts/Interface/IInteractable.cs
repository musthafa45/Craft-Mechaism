
using UnityEngine;

public interface IInteractable
{
    Vector3 GetPosition();
    void Interact();
    void SetActiveSelectedVisual(bool active);
}
