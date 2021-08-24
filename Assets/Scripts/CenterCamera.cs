using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterCamera : MonoBehaviour {
  [SerializeField] Board board;
  [SerializeField] int zoffset = -10;
  Bounds tileBounds = new Bounds();
  private void Update() {
    updateBounds();
  }

  void updateBounds() {
    Bounds newTileBounds = new Bounds();

    foreach (Tile tile in board.tiles) {
      newTileBounds.Encapsulate(tile.transform.position);
    }

    if (tileBounds == null || newTileBounds.center != tileBounds.center) {
      tileBounds = newTileBounds;
      centerCamera();
    }
  }

  void centerCamera() {
    transform.position = new Vector3(tileBounds.center.x, tileBounds.center.y, zoffset);
  }
}
