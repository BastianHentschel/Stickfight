using System;
using System.Threading.Tasks;
using DualPantoFramework;
using UnityEngine;

public class IntroductionHandler : MonoBehaviour
{
    public enum WiggleDirection
    {
        Up,
        Down,
        UpDown,
        Left,
        Right,
        LeftRight
    }

    private static GameObject _panto = GameObject.Find("Panto"), _player = GameObject.Find("Player");
    private static Level _level = _panto.GetComponent<Level>();
    private static PantoHandle _meHandle = _panto.GetComponent<UpperHandle>();

    public static async Task Introduce()
    {
        await _level.PlayIntroduction();
        await _meHandle.MoveToPosition(_player.transform.position);
        await Wiggle(_meHandle, WiggleDirection.Right, 0.25f, 1);
    }

    /**
     * Max movement speed is 1.5f
     */
    public static async Task Wiggle(PantoHandle handle, WiggleDirection direction, float intensity, float extent)
    {
        Vector3 originalPosition = handle.HandlePosition(_player.transform.position);
        
        async Task MoveHandle(Vector3 direction)
        {
            await handle.MoveToPosition(originalPosition + direction * extent, intensity);
            await handle.MoveToPosition(originalPosition, 1.5f);
        }
        
        for (int i = 0; i < 2; i++)
        {
            switch (direction)
            {
                case WiggleDirection.Up:
                    await MoveHandle(new Vector3(0, 0, 1));
                    break;
                case WiggleDirection.Down:
                    await MoveHandle(new Vector3(0, 0, -1));
                    break;
                case WiggleDirection.Left:
                    await MoveHandle(new Vector3(-1, 0, 0));
                    break;
                case WiggleDirection.UpDown:
                    await MoveHandle(new Vector3(0, 0, 1));
                    await MoveHandle(new Vector3(0, 0, -1));
                    break;
                case WiggleDirection.Right:
                    await MoveHandle(new Vector3(1, 0, 0));
                    break;
                case WiggleDirection.LeftRight:
                    await MoveHandle(new Vector3(-1, 0, 0));
                    await MoveHandle(new Vector3(1, 0, 0));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}
