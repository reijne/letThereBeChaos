using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Point {
  public Vector2 pos;
  public Color color;
  public int age;
}

public struct Spread {
  public Color color;
  public Vector2[] pattern;
}

public class EyeComputer : MonoBehaviour {
  [SerializeField] Board board;
  [SerializeField] ComputeShader computeShader;
  [SerializeField] ComputeBuffer computeBuffer;
  [SerializeField] RenderTexture renderTexture;
  private Point[] points;
  private Spread[] spreads;

  private void Start() {
    points = new Point[board.width * board.height];
    spreads = new Spread[Controls.patterns.Count];
    addPoints();
    addSpreads();
    // makeRenderTexture();
    // makeComputer();
  }

  public void preparePlusCompute() {
    int vector2size = sizeof(float) * 2;
    int colorSize = sizeof(float) * 4;
    int intSize = sizeof(int);
    int patternSize = vector2size * Pattern.SIZE;

    int pointsize = (vector2size + colorSize + intSize);
    int spreadsize = (colorSize + patternSize);

    ComputeBuffer pointsBuffer = new ComputeBuffer(points.Length, pointsize);
    pointsBuffer.SetData(points);
    computeShader.SetBuffer(0, "points", pointsBuffer);

    // ComputeBuffer spreadsBuffer = new ComputeBuffer(spreads.Length, spreadsize);
    // spreadsBuffer.SetData(spreads);
    // computeShader.SetBuffer(0, "spreads", spreadsBuffer);

    computeShader.Dispatch(0, points.Length / 10, 1, 1);

    pointsBuffer.GetData(points);
    Debug.Log(points[0].age);

    pointsBuffer.Dispose();
    // spreadsBuffer.Dispose();
  }

  private void addPoints() {
    for (int y = 0; y < board.height; y++) {
      for (int x = 0; x < board.width; x++) {
        Point p = new Point();
        p.pos = new Vector2(x, y);
        p.color = new Color(0, 0, 0, 1);
        p.age = 0;
        points[x + y * board.height] = p;
      }
    }
  }

  private void addSpreads() {
    foreach (KeyValuePair<Color, List<Vector2>> kvp in Controls.patterns) {
      Spread s = new Spread();
      s.color = kvp.Key;
      s.pattern = new Vector2[kvp.Value.Count];
      for (int i = 0; i < kvp.Value.Count; i++) {
        s.pattern[i] = kvp.Value[i];
      }
    }
  }

  private void makeRenderTexture() {
    renderTexture = new RenderTexture(board.width, board.height, 24);
    renderTexture.enableRandomWrite = true;
    renderTexture.Create();
  }

  private void makeComputer() {
    computeShader.SetTexture(0, "Result", renderTexture);
    computeShader.Dispatch(0, renderTexture.width / 8, renderTexture.height / 8, 1);
  }

  // private void OnRenderImage(RenderTexture src, RenderTexture dest) {
  //   if (renderTexture == null) makeRenderTexture();
  //   makeComputer();

  //   Graphics.Blit(renderTexture, dest);
  // }
}
