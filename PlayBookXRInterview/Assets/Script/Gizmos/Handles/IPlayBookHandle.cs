using UnityEngine;

public interface IPlayBookHandle
{
    public void OnHover();
    public void OnHoverExit();

    public void OnMousePressDown(Vector3 mouseHitPoint);

    public void OnMouseRelease();
}
