using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterCamera : MonoBehaviour {
  [SerializeField] Board board;
  [SerializeField] Camera cam;
  [SerializeField] int zoffset;
  [SerializeField] float movespeed;
  Bounds tileBounds = new Bounds();
  private void Start() {
    cam.orthographicSize = Mathf.Max(0.5f * board.width, 0.5f * board.height);
  }
  private void Update() {
    updateBounds();
    movement();
  }

  void movement() {
    float dx = Input.GetAxis("Horizontal") * movespeed;
    float dy = Input.GetAxis("Vertical") * movespeed;
    Vector3 dest = new Vector3(transform.position.x + dx, transform.position.y + dy, transform.position.z);
    transform.position = Vector3.Lerp(transform.position, dest, Time.deltaTime);
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

  private void OnGUI() {
    cam.orthographicSize -= Input.mouseScrollDelta.y;
  }
}
