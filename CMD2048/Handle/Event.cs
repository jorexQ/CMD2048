using CMD2048.Handle.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD2048.Handle
{
    /// <summary>
    /// 
    /// </summary>
    public class Event
    {
        private Dictionary<Direction, Action> _directionHandler
                = new Dictionary<Direction, Action>();

        private Action _reStartHandle;
        private Action _rollBackHandle;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directionHandler"></param>
        /// <param name="reStartHandle"></param>
        /// <param name="rollBackHandle"></param>
        public void RegisterHandle(Dictionary<Direction, Action> directionHandler,
            Action reStartHandle,
            Action rollBackHandle)
        {
            _directionHandler = directionHandler;
            _reStartHandle = reStartHandle;
            _rollBackHandle = rollBackHandle;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Listening()
        {
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    _directionHandler[Direction.Up]();
                    Listening();
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    _directionHandler[Direction.Down]();
                    Listening();
                    break;
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    _directionHandler[Direction.Left]();
                    Listening();
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    _directionHandler[Direction.Right]();
                    Listening();
                    break;
                case ConsoleKey.Q:
                    _reStartHandle();
                    Listening();
                    break;
                case ConsoleKey.E:
                    _rollBackHandle();
                    Listening();
                    break;
                default:
                    Listening();
                    break;
            }
        }
    }
}
