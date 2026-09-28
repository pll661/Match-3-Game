using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public int x {  get; private set; }
    public int y { get; private set; }
    public PieceType type {  get; private set; }
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void InitPiece(int x,int y,PieceType type,Sprite sprite)
    {
        this.x = x;
        this.y = y;
        this.type = type;
        spriteRenderer.sprite = sprite;
    }
    public void SetPos(int x,int y)
    {
        this.x = x;
        this.y = y;
    }
}
