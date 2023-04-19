using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Analytics;
using Random = UnityEngine.Random;
using GameAnalyticsSDK;

public class Manager : Singleton<Manager>
{

    [SerializeField] private int width = 4;
    [SerializeField] private int height = 4;
    [SerializeField] private Node _nodePrefab;
    [SerializeField] private Block _blockPrefab;
    [SerializeField] private SpriteRenderer _boardPrefab;
    [SerializeField] private List<BlockType> _types;
    [SerializeField] private float travelTime = 0.2f;
    [SerializeField] private int winCondition = 2048;

    [SerializeField] private int startNumber = 8;

    public GameState state;
    private List<Node> nodes;
    private List<Block> blocks;

    private BlockType GetBlockTypeByValue(int value) => _types.FirstOrDefault(t => t.Value == value);

    [SerializeField] private Flower flower;


    private void Start()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        InputManager.Instacne.OnDragDown += () =>
        {
            Shift(Vector2.down);
        };

        InputManager.Instacne.OnDragRight += () =>
        {
            Shift(Vector2.right);

        };

        InputManager.Instacne.OnDragLeft += () =>
        {
            Shift(Vector2.left);

        };

        ChangeState(GameState.GenerateLevel);
    }

    private void Update()
    {
        for (int i = blocks.Count - 1; i > 0; i--)
        {
            var block = blocks[i];
            if (block.Value >= winCondition)
            {
                blocks.Remove(block);
                block.transform.DOMove(flower.transform.position, travelTime);
            }
        }
    }

    private void OnDisable()
    {
        if (InputManager.Instacne == null) return;
        InputManager.Instacne.OnDragDown -= () =>
        {
            Shift(Vector2.down);

        };

        InputManager.Instacne.OnDragRight -= () =>
        {
            Shift(Vector2.right);

        };

        InputManager.Instacne.OnDragLeft -= () =>
        {
            Shift(Vector2.left);

        };
    }

    public void ChangeState(GameState newState)
    {
        state = newState;
        switch (newState)
        {
            case GameState.GenerateLevel:
                GenerateGrid();
                Analytics.CustomEvent("StartGame");
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "StartGame");
                break;

            case GameState.SpawningBlocks:
                SpawnBlocks(1);
                break;

            case GameState.WaitingInput:
                break;

            case GameState.Moving:
                break;

            case GameState.Win:
                HUDManager.Instacne.ShowGameResult(GameState.Win);
                Analytics.CustomEvent("WinGame");
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "WinGame");


                break;

            case GameState.Lose:
                HUDManager.Instacne.ShowGameResult(GameState.Lose);
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "LoseGame");
                Analytics.CustomEvent("LoseGame");
                break;

            default:
                break;
        }
    }


    void GenerateGrid()
    {
        nodes = new List<Node>();
        blocks = new List<Block>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var node = Instantiate(_nodePrefab, new Vector2(x, y), Quaternion.identity);
                node.name = "X: " + x + " Y:" + y;
                nodes.Add(node);
            }
        }

        var center = new Vector2((float)width / 2 - 0.5f, (float)height / 2 - 0.5f);
        var board = Instantiate(_boardPrefab, center, Quaternion.identity);
        board.size = new Vector2(width, height);
        Camera.main.transform.position = new Vector3(center.x, center.y, -10f);

        ChangeState(GameState.SpawningBlocks);
    }

    void SpawnBlocks(int amount)
    {
        var freeNodes = nodes.Where(n => n.OccupiedBlock == null).OrderBy(b => Random.value).ToList();

        foreach (var node in freeNodes.Take(amount))
        {
            SpawnBlock(node, startNumber, out var block);
        }

        if (freeNodes.Count == 0)
        {
            ChangeState(GameState.Lose);
            return;
        }

        ChangeState(GameState.WaitingInput);
    }

    void SpawnBlock(Node node, int value, out Block createBlock)
    {

        var block = Instantiate(_blockPrefab, node.Pos, Quaternion.identity);
        block.Init(GetBlockTypeByValue(value));
        block.SetBlock(node);
        block.name = "Block: " + node.name;
        createBlock = block;
        blocks.Add(block);
    }

    void Shift(Vector2 dir)
    {
        ChangeState(GameState.Moving);

        var orderedBlocks = blocks.OrderBy(b => b.Pos.x).ThenBy(b => b.Pos.y).ToList();
        if (dir == Vector2.right || dir == Vector2.up) orderedBlocks.Reverse();

        foreach (var block in orderedBlocks)
        {
            var next = block.Node;

            do
            {
                block.SetBlock(next);

                var possibleNode = GetNodeAtPosition(next.Pos + dir);

                if (possibleNode != null)
                {
                    if (possibleNode.OccupiedBlock != null && possibleNode.OccupiedBlock.CanMerge(block.Value))
                    {
                        block.MergeBlock(possibleNode.OccupiedBlock);
                    }
                    if (possibleNode.OccupiedBlock == null)
                    {
                        next = possibleNode;
                    }
                }

            } while (next != block.Node);
        }

        var sequence = DOTween.Sequence();

        foreach (var block in orderedBlocks)
        {
            var movePoint = block.MergingBlock != null ? block.MergingBlock.Node.Pos : block.Node.Pos;
            sequence.Insert(0, block.transform.DOMove(movePoint, travelTime));
        }

        sequence.OnComplete(() =>
        {
            foreach (var block in orderedBlocks.Where(b => b.MergingBlock != null))
            {
                MergeBlocks(block.MergingBlock, block);
            }




            ChangeState(GameState.SpawningBlocks);
        });

    }

    void MergeBlocks(Block baseBlock, Block mergingBlock)
    {
        SpawnBlock(baseBlock.Node, baseBlock.Value * 2, out var block);


        RemoveBlock(baseBlock);
        RemoveBlock(mergingBlock);
    }

    void RemoveBlock(Block block)
    {
        blocks.Remove(block);
        Destroy(block.gameObject);
    }

    Node GetNodeAtPosition(Vector2 pos) => nodes.FirstOrDefault(n => n.Pos == pos);

}

[Serializable]
public struct BlockType
{
    public int Value;
    public Color color;
}

public enum GameState
{
    GenerateLevel,
    SpawningBlocks,
    WaitingInput,
    Moving,
    Win,
    Lose
}