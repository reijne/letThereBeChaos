using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterCamera : MonoBehaviour
{
  [SerializeField] Board board;
  Vector3 offset = new Vector3(0,0,-10);
  Bounds tileBounds = new Bounds();
  private void Update() {
    updateBounds();
  }

  void updateBounds() {
    Bounds newTileBounds = new Bounds();

    foreach(Tile tile in board.tiles) {
      newTileBounds.Encapsulate(tile.transform.position);
    }

    if (tileBounds == null || newTileBounds.center != tileBounds.center) {
      tileBounds = newTileBounds;
      centerCamera();
    }
  }

  void centerCamera() {
    Debug.Log("centering");
    transform.position = tileBounds.center + offset;
  }
}
