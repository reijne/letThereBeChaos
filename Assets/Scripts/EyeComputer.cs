using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeComputer : MonoBehaviour {
  [SerializeField] Board board;
  [SerializeField] ComputeShader computeShader;
  [SerializeField] ComputeBuffer computeBuffer;
  [SerializeField] RenderTexture renderTexture;

  private void Start() {
    // makeRenderTexture();
    // makeComputer();
  }

  // private void OnRenderImage(RenderTexture src, RenderTexture dest) {
  //   if (renderTexture == null) makeRenderTexture();
  //   makeComputer();

  //   Graphics.Blit(renderTexture, dest);
  // }

  private void makeRenderTexture() {
    renderTexture = new RenderTexture(board.width, board.height, 24);
    renderTexture.enableRandomWrite = true;
    renderTexture.Create();
  }

  private void makeComputer() {
    computeShader.SetTexture(0, "Result", renderTexture);
    computeShader.Dispatch(0, renderTexture.width / 8, renderTexture.height / 8, 1);
  }
}
