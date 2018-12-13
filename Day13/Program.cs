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
            var trackSegments = new(TrackType type, List<(CartDirection direction, int lastMove, IntersectionActions nextIntersection)> carts)[lines.First().Length, lines.Count()];

            BuildTrack(lines, trackSegments);
            var found = false;
            var counter = 0;
            while (!found)
            {
                counter++;
                for (int y = 0; y < trackSegments.GetLength(0); y++)
                {
                    for (int x = 0; x < trackSegments.GetLength(1); x++)
                    {
                        var current = trackSegments[x, y];
                        if (current.type != TrackType.Empty && AreUnmovedCarts(counter, current) > 0)
                        {
                            HandleIntersection(counter, current);
                            HandleCorners(counter, current);
                            MoveCart(trackSegments, counter, y, x, current);

                            var cartCounter = 0;
                            cartCounter = TestForCollisions(trackSegments, cartCounter);
                            if (cartCounter == 1)
                            {
                                for (int y1 = 0; y1 < trackSegments.GetLength(0); y1++)
                                {
                                    for (int x1 = 0; x1 < trackSegments.GetLength(1); x1++)
                                    {
                                        if (trackSegments[x1, y1].carts.Count > 0)
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

        private static int AreUnmovedCarts(int counter, (TrackType type, List<(CartDirection direction, int lastMove, IntersectionActions nextIntersection)> carts) current)
        {
            return current.carts.Where(c => c.lastMove != counter).Count();
        }

        private static int TestForCollisions((TrackType type, List<(CartDirection direction, int lastMove, IntersectionActions nextIntersection)> carts)[,] trackSegments, int cartCounter)
        {
            for (int y1 = 0; y1 < trackSegments.GetLength(0); y1++)
            {
                for (int x1 = 0; x1 < trackSegments.GetLength(1); x1++)
                {
                    if (trackSegments[x1, y1].carts.Count > 1)
                    {
                        Console.WriteLine($"<<<BASH>>> {x1}, {y1}");
                        trackSegments[x1, y1].carts.Clear();

                        break;
                    }
                    if (trackSegments[x1, y1].carts.Count > 0)
                    {
                        cartCounter++;
                    }
                }
            }

            return cartCounter;
        }

        private static void MoveCart((TrackType type, List<(CartDirection direction, int lastMove, IntersectionActions nextIntersection)> carts)[,] trackSegments, int counter, int y, int x, (TrackType type, List<(CartDirection direction, int lastMove, IntersectionActions nextIntersection)> carts) current)
        {
            var firstCart = current.carts.First();
            if (firstCart.direction == CartDirection.Left)
            {
                trackSegments[x - 1, y].carts.Add((CartDirection.Left, counter, firstCart.nextIntersection));
            }
            else if (firstCart.direction == CartDirection.Right)
            {
                trackSegments[x + 1, y].carts.Add((CartDirection.Right, counter, firstCart.nextIntersection));
            }
            else if (firstCart.direction == CartDirection.Up)
            {
                trackSegments[x, y - 1].carts.Add((CartDirection.Up, counter, firstCart.nextIntersection));
            }
            else if (firstCart.direction == CartDirection.Down)
            {
                trackSegments[x, y + 1].carts.Add((CartDirection.Down, counter, firstCart.nextIntersection));
            }
            current.carts.RemoveAt(0);
        }

        private static void HandleCorners(int counter, (TrackType type, List<(CartDirection direction, int lastMove, IntersectionActions nextIntersection)> carts) current)
        {
            var firstCart = current.carts.First();
            if (current.type == TrackType.UpToLeftTransition)
            {
                if (firstCart.direction == CartDirection.Right)
                {
                    current.carts.Insert(1, (CartDirection.Down, counter, firstCart.nextIntersection));
                }
                else if (firstCart.direction == CartDirection.Left)
                {
                    current.carts.Insert(1, (CartDirection.Up, counter, firstCart.nextIntersection));
                }
                else if (firstCart.direction == CartDirection.Down)
                {
                    current.carts.Insert(1, (CartDirection.Right, counter, firstCart.nextIntersection));
                }
                else if (firstCart.direction == CartDirection.Up)
                {
                    current.carts.Insert(1, (CartDirection.Left, counter, firstCart.nextIntersection));
                }
                current.carts.RemoveAt(0);
            }
            else if (current.type == TrackType.UpToRightTransition)
            {
                if (firstCart.direction == CartDirection.Right)
                {
                    current.carts.Insert(1, (CartDirection.Up, counter, firstCart.nextIntersection));
                }
                else if (firstCart.direction == CartDirection.Left)
                {
                    current.carts.Insert(1, (CartDirection.Down, counter, firstCart.nextIntersection));
                }
                else if (firstCart.direction == CartDirection.Down)
                {
                    current.carts.Insert(1, (CartDirection.Left, counter, firstCart.nextIntersection));
                }
                else if (firstCart.direction == CartDirection.Up)
                {
                    current.carts.Insert(1, (CartDirection.Right, counter, firstCart.nextIntersection));
                }
                current.carts.RemoveAt(0);
            }
        }

        private static void HandleIntersection(int counter, (TrackType type, List<(CartDirection direction, int lastMove, IntersectionActions nextIntersection)> carts) current)
        {
            if (current.type == TrackType.Intersection)
            {
                var (a, b) = GetDirectionAfterIntersection(current.carts.First().direction, current.carts.First().nextIntersection);
                current.carts.Insert(1, (a, counter, b));
                current.carts.RemoveAt(0);
            }
        }

        private static int BuildTrack(string[] lines, (TrackType type, List<(CartDirection, int, IntersectionActions)> carts)[,] trackSegments)
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
                            trackSegments[charCounter, lineCounter] = (TrackType.Horizontal, new List<(CartDirection, int, IntersectionActions)> { });
                            break;
                        case '|':
                            trackSegments[charCounter, lineCounter] = (TrackType.Vertical, new List<(CartDirection, int, IntersectionActions)> { });
                            break;
                        case '\\':
                            trackSegments[charCounter, lineCounter] = (TrackType.UpToLeftTransition, new List<(CartDirection, int, IntersectionActions)> { });
                            break;
                        case '/':
                            trackSegments[charCounter, lineCounter] = (TrackType.UpToRightTransition, new List<(CartDirection, int, IntersectionActions)> { });
                            break;
                        case '+':
                            trackSegments[charCounter, lineCounter] = (TrackType.Intersection, new List<(CartDirection, int, IntersectionActions)> { });
                            break;
                        case '>':
                            trackSegments[charCounter, lineCounter] = (TrackType.Horizontal, new List<(CartDirection, int, IntersectionActions)> { (CartDirection.Right, 0, IntersectionActions.Left) });
                            break;
                        case '<':
                            trackSegments[charCounter, lineCounter] = (TrackType.Horizontal, new List<(CartDirection, int, IntersectionActions)> { (CartDirection.Left, 0, IntersectionActions.Left) });
                            break;
                        case '^':
                            trackSegments[charCounter, lineCounter] = (TrackType.Vertical, new List<(CartDirection, int, IntersectionActions)> { (CartDirection.Up, 0, IntersectionActions.Left) });
                            break;
                        case 'v':
                            trackSegments[charCounter, lineCounter] = (TrackType.Horizontal, new List<(CartDirection, int, IntersectionActions)> { (CartDirection.Down, 0, IntersectionActions.Left) });
                            break;
                        default:
                            trackSegments[charCounter, lineCounter] = (TrackType.Empty, new List<(CartDirection, int, IntersectionActions)> { });
                            break;
                    }
                    charCounter++;
                }
                lineCounter++;
            }

            return lineCounter;
        }

        public static (CartDirection, IntersectionActions) GetDirectionAfterIntersection(CartDirection current, IntersectionActions turn)
        {//LSR
            var nextTurn = IntersectionActions.Straight;
            var nextDirection = CartDirection.Left;
            switch (turn)
            {
                case IntersectionActions.Straight:
                    nextTurn = IntersectionActions.Right;
                    break;
                case IntersectionActions.Left:
                    nextTurn = IntersectionActions.Straight;
                    break;
                case IntersectionActions.Right:
                    nextTurn = IntersectionActions.Left;
                    break;
            }

            switch (current)
            {
                case CartDirection.Up:
                    switch (turn)
                    {
                        case IntersectionActions.Left:
                            nextDirection = CartDirection.Left;
                            break;
                        case IntersectionActions.Right:
                            nextDirection = CartDirection.Right;
                            break;
                        case IntersectionActions.Straight:
                            nextDirection = CartDirection.Up;
                            break;
                    }
                    break;
                case CartDirection.Down:
                    switch (turn)
                    {
                        case IntersectionActions.Left:
                            nextDirection = CartDirection.Right;
                            break;
                        case IntersectionActions.Right:
                            nextDirection = CartDirection.Left;
                            break;
                        case IntersectionActions.Straight:
                            nextDirection = CartDirection.Down;
                            break;
                    }
                    break;
                case CartDirection.Left:
                    switch (turn)
                    {
                        case IntersectionActions.Left:
                            nextDirection = CartDirection.Down;
                            break;
                        case IntersectionActions.Right:
                            nextDirection = CartDirection.Up;
                            break;
                        case IntersectionActions.Straight:
                            nextDirection = CartDirection.Left;
                            break;
                    }
                    break;
                case CartDirection.Right:
                    switch (turn)
                    {
                        case IntersectionActions.Left:
                            nextDirection = CartDirection.Up;
                            break;
                        case IntersectionActions.Right:
                            nextDirection = CartDirection.Down;
                            break;
                        case IntersectionActions.Straight:
                            nextDirection = CartDirection.Right;
                            break;
                    }
                    break;
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
        Right
    }

    public enum IntersectionActions
    {
        Left,
        Right,
        Straight
    }
}