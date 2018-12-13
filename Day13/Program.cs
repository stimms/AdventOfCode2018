using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day13
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var trackSegmetns = new(TrackType type, List<(CartDirection direction, int lastMove, CartDirection nextIntersection)> carts)[lines.First().Length, lines.Count()];

            BuildTrack(lines, trackSegmetns);
            var found = false;
            var counter = 0;
            while (!found)
            {
                counter++;
                for (int y = 0; y < trackSegmetns.GetLength(0); y++)
                {
                    for (int x = 0; x < trackSegmetns.GetLength(1); x++)
                    {
                        var current = trackSegmetns[x, y];
                        if (current.type != TrackType.Empty && current.carts.Where(c => c.lastMove != counter).Count() > 0)
                        {
                            HandleIntersection(counter, current);
                            HandleCorners(counter, current);
                            MoveCart(trackSegmetns, counter, y, x, current);

                            var cartCounter = 0;
                            cartCounter = TestForCollissions(trackSegmetns, cartCounter);
                            if (cartCounter == 1)
                            {
                                for (int y1 = 0; y1 < trackSegmetns.GetLength(0); y1++)
                                {
                                    for (int x1 = 0; x1 < trackSegmetns.GetLength(1); x1++)
                                    {
                                        if (trackSegmetns[x1, y1].carts.Count > 0)
                                        {
                                            Console.WriteLine($"Final cart {x1}, {y1}");
                                            cartCounter++;
                                            found = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine("done.");
            Console.ReadLine();
        }

        private static int TestForCollissions((TrackType type, List<(CartDirection direction, int lastMove, CartDirection nextIntersection)> carts)[,] trackSegmetns, int cartCounter)
        {
            for (int y1 = 0; y1 < trackSegmetns.GetLength(0); y1++)
            {
                for (int x1 = 0; x1 < trackSegmetns.GetLength(1); x1++)
                {
                    if (trackSegmetns[x1, y1].carts.Count > 1)
                    {
                        Console.WriteLine($"<<<BASH>>> {x1}, {y1}");
                        trackSegmetns[x1, y1].carts.Clear();

                        break;
                    }
                    if (trackSegmetns[x1, y1].carts.Count > 0)
                    {
                        cartCounter++;
                    }
                }
            }

            return cartCounter;
        }

        private static void MoveCart((TrackType type, List<(CartDirection direction, int lastMove, CartDirection nextIntersection)> carts)[,] trackSegmetns, int counter, int y, int x, (TrackType type, List<(CartDirection direction, int lastMove, CartDirection nextIntersection)> carts) current)
        {
            if (current.carts.First().direction == CartDirection.Left)
            {
                trackSegmetns[x - 1, y].carts.Add((CartDirection.Left, counter, current.carts.First().nextIntersection));
            }
            else if (current.carts.First().direction == CartDirection.Right)
            {
                trackSegmetns[x + 1, y].carts.Add((CartDirection.Right, counter, current.carts.First().nextIntersection));
            }
            else if (current.carts.First().direction == CartDirection.Up)
            {
                trackSegmetns[x, y - 1].carts.Add((CartDirection.Up, counter, current.carts.First().nextIntersection));
            }
            else if (current.carts.First().direction == CartDirection.Down)
            {
                trackSegmetns[x, y + 1].carts.Add((CartDirection.Down, counter, current.carts.First().nextIntersection));
            }
            current.carts.RemoveAt(0);
        }

        private static void HandleCorners(int counter, (TrackType type, List<(CartDirection direction, int lastMove, CartDirection nextIntersection)> carts) current)
        {
            if (current.type == TrackType.UpToLeftTransition)
            {
                if (current.carts.First().direction == CartDirection.Right)
                {
                    current.carts.Insert(1, (CartDirection.Down, counter, current.carts.First().nextIntersection));
                }
                else if (current.carts.First().direction == CartDirection.Left)
                {
                    current.carts.Insert(1, (CartDirection.Up, counter, current.carts.First().nextIntersection));
                }
                else if (current.carts.First().direction == CartDirection.Down)
                {
                    current.carts.Insert(1, (CartDirection.Right, counter, current.carts.First().nextIntersection));
                }
                else if (current.carts.First().direction == CartDirection.Up)
                {
                    current.carts.Insert(1, (CartDirection.Left, counter, current.carts.First().nextIntersection));
                }
                current.carts.RemoveAt(0);
            }
            else if (current.type == TrackType.UpToRightTransition)
            {
                if (current.carts.First().direction == CartDirection.Right)
                {
                    current.carts.Insert(1, (CartDirection.Up, counter, current.carts.First().nextIntersection));
                }
                else if (current.carts.First().direction == CartDirection.Left)
                {
                    current.carts.Insert(1, (CartDirection.Down, counter, current.carts.First().nextIntersection));
                }
                else if (current.carts.First().direction == CartDirection.Down)
                {
                    current.carts.Insert(1, (CartDirection.Left, counter, current.carts.First().nextIntersection));
                }
                else if (current.carts.First().direction == CartDirection.Up)
                {
                    current.carts.Insert(1, (CartDirection.Right, counter, current.carts.First().nextIntersection));
                }
                current.carts.RemoveAt(0);
            }
        }

        private static void HandleIntersection(int counter, (TrackType type, List<(CartDirection direction, int lastMove, CartDirection nextIntersection)> carts) current)
        {
            if (current.type == TrackType.Intersection)
            {
                var (a, b) = GetDirectionAfterIntersection(current.carts.First().direction, current.carts.First().nextIntersection);
                current.carts.Insert(1, (a, counter, b));
                current.carts.RemoveAt(0);
            }
        }

        private static int BuildTrack(string[] lines, (TrackType type, List<(CartDirection, int, CartDirection)> carts)[,] trackSegmetns)
        {
            int lineCounter = 0;
            foreach (var line in lines)
            {
                int charCounter = 0;
                foreach (var c in line)
                {
                    switch (c)
                    {
                        case '-':
                            trackSegmetns[charCounter, lineCounter] = (TrackType.Horizontal, new List<(CartDirection, int, CartDirection)> { });
                            break;
                        case '|':
                            trackSegmetns[charCounter, lineCounter] = (TrackType.Vertical, new List<(CartDirection, int, CartDirection)> { });
                            break;
                        case '\\':
                            trackSegmetns[charCounter, lineCounter] = (TrackType.UpToLeftTransition, new List<(CartDirection, int, CartDirection)> { });
                            break;
                        case '/':
                            trackSegmetns[charCounter, lineCounter] = (TrackType.UpToRightTransition, new List<(CartDirection, int, CartDirection)> { });
                            break;
                        case '+':
                            trackSegmetns[charCounter, lineCounter] = (TrackType.Intersection, new List<(CartDirection, int, CartDirection)> { });
                            break;
                        case '>':
                            trackSegmetns[charCounter, lineCounter] = (TrackType.Horizontal, new List<(CartDirection, int, CartDirection)> { (CartDirection.Right, 0, CartDirection.Left) });
                            break;
                        case '<':
                            trackSegmetns[charCounter, lineCounter] = (TrackType.Horizontal, new List<(CartDirection, int, CartDirection)> { (CartDirection.Left, 0, CartDirection.Left) });
                            break;
                        case '^':
                            trackSegmetns[charCounter, lineCounter] = (TrackType.Vertical, new List<(CartDirection, int, CartDirection)> { (CartDirection.Up, 0, CartDirection.Left) });
                            break;
                        case 'v':
                            trackSegmetns[charCounter, lineCounter] = (TrackType.Horizontal, new List<(CartDirection, int, CartDirection)> { (CartDirection.Down, 0, CartDirection.Left) });
                            break;
                        default:
                            trackSegmetns[charCounter, lineCounter] = (TrackType.Empty, new List<(CartDirection, int, CartDirection)> { });
                            break;
                    }
                    charCounter++;
                }
                lineCounter++;
            }

            return lineCounter;
        }

        public static (CartDirection, CartDirection) GetDirectionAfterIntersection(CartDirection current, CartDirection turn)
        {//LSR
            var nextTurn = CartDirection.Straight;
            var nextDirection = CartDirection.Straight;
            if (turn == CartDirection.Straight)
            {
                nextTurn = CartDirection.Right;
            }
            else if (turn == CartDirection.Left)
            {
                nextTurn = CartDirection.Straight;
            }
            else if (turn == CartDirection.Right)
            {
                nextTurn = CartDirection.Left;
            }

            if (current == CartDirection.Up)
            {
                if (turn == CartDirection.Straight)
                {
                    nextDirection = CartDirection.Up;
                }
                else if (turn == CartDirection.Left)
                {
                    nextDirection = CartDirection.Left;
                }
                else if (turn == CartDirection.Right)
                {
                    nextDirection = CartDirection.Right;
                }
            }

            if (current == CartDirection.Down)
            {
                if (turn == CartDirection.Straight)
                {
                    nextDirection = CartDirection.Down;
                }
                else if (turn == CartDirection.Left)
                {
                    nextDirection = CartDirection.Right;
                }
                else if (turn == CartDirection.Right)
                {
                    nextDirection = CartDirection.Left;
                }
            }

            if (current == CartDirection.Left)
            {
                if (turn == CartDirection.Straight)
                {
                    nextDirection = CartDirection.Left;
                }
                else if (turn == CartDirection.Left)
                {
                    nextDirection = CartDirection.Down;
                }
                else if (turn == CartDirection.Right)
                {
                    nextDirection = CartDirection.Up;
                }
            }

            if (current == CartDirection.Right)
            {
                if (turn == CartDirection.Straight)
                {
                    nextDirection = CartDirection.Right;
                }
                else if (turn == CartDirection.Left)
                {
                    nextDirection = CartDirection.Up;
                }
                else if (turn == CartDirection.Right)
                {
                    nextDirection = CartDirection.Down;
                }
            }
            return (nextDirection, nextTurn);
        }
    }

    public enum TrackType
    {
        Horizontal,
        Vertical,
        UpToLeftTransition,
        UpToRightTransition,
        Intersection,
        Empty
    }

    public enum CartDirection
    {//facing
        Up,
        Down,
        Left,
        Right,
        Straight
    }
}