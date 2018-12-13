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
                int lineCounter = 0;
                foreach (var line in lines)
                {

                    int charCounter = 0;
                    foreach (var c in line)
                    {
                        var current = trackSegmetns[charCounter, lineCounter];
                        if (current.type != TrackType.Empty && current.carts.Where(x => x.lastMove != counter).Count() > 0)
                        {
                            if (current.type == TrackType.Intersection)
                            {
                                var (a, b) = GetDirectionAfterIntersection(current.carts.First().direction, current.carts.First().nextIntersection);
                                current.carts.Insert(1, (a, counter, b));
                                current.carts.RemoveAt(0);
                            }

                            if (current.type == TrackType.UpToLeftTransition)
                            {
                                if (current.carts.First().direction == CartDirection.Right)
                                {
                                    current.carts.Insert(1, (CartDirection.Down, counter, current.carts.First().nextIntersection));
                                    current.carts.RemoveAt(0);
                                }
                                else if (current.carts.First().direction == CartDirection.Left)
                                {

                                    current.carts.Insert(1, (CartDirection.Up, counter, current.carts.First().nextIntersection));
                                    current.carts.RemoveAt(0);
                                }
                                else if (current.carts.First().direction == CartDirection.Down)
                                {

                                    current.carts.Insert(1, (CartDirection.Right, counter, current.carts.First().nextIntersection));
                                    current.carts.RemoveAt(0);
                                }
                                else if (current.carts.First().direction == CartDirection.Up)
                                {

                                    current.carts.Insert(1, (CartDirection.Left, counter, current.carts.First().nextIntersection));
                                    current.carts.RemoveAt(0);
                                }
                            }
                            else if (current.type == TrackType.UpToRightTransition)
                            {
                                if (current.carts.First().direction == CartDirection.Right)
                                {

                                    current.carts.Insert(1, (CartDirection.Up, counter, current.carts.First().nextIntersection));
                                    current.carts.RemoveAt(0);
                                }
                                else if (current.carts.First().direction == CartDirection.Left)
                                {

                                    current.carts.Insert(1, (CartDirection.Down, counter, current.carts.First().nextIntersection));
                                    current.carts.RemoveAt(0);
                                }
                                else if (current.carts.First().direction == CartDirection.Down)
                                {

                                    current.carts.Insert(1, (CartDirection.Left, counter, current.carts.First().nextIntersection));
                                    current.carts.RemoveAt(0);
                                }
                                else if (current.carts.First().direction == CartDirection.Up)
                                {

                                    current.carts.Insert(1, (CartDirection.Right, counter, current.carts.First().nextIntersection));
                                    current.carts.RemoveAt(0);
                                }
                            }

                            if (current.carts.First().direction == CartDirection.Left)
                            {
                                
                                trackSegmetns[charCounter - 1, lineCounter].carts.Add((CartDirection.Left, counter, current.carts.First().nextIntersection));
                                current.carts.RemoveAt(0);
                            }
                            else if (current.carts.First().direction == CartDirection.Right)
                            {
                                
                                trackSegmetns[charCounter + 1, lineCounter].carts.Add((CartDirection.Right, counter, current.carts.First().nextIntersection));
                                current.carts.RemoveAt(0);
                            }
                            else if (current.carts.First().direction == CartDirection.Up)
                            {
                                
                                trackSegmetns[charCounter, lineCounter - 1].carts.Add((CartDirection.Up, counter, current.carts.First().nextIntersection));
                                current.carts.RemoveAt(0);
                            }
                            else if (current.carts.First().direction == CartDirection.Down)
                            {
                                
                                trackSegmetns[charCounter, lineCounter + 1].carts.Add((CartDirection.Down, counter, current.carts.First().nextIntersection));
                                current.carts.RemoveAt(0);
                            }

                            var cartCounter = 0;
                            var lineCounter1 = 0;
                            foreach (var l in lines)
                            {
                                int charCounter1 = 0;
                                foreach (var c1 in line)
                                {
                                    
                                    if (trackSegmetns[charCounter1, lineCounter1].carts.Count > 1)
                                    {
                                        Console.WriteLine($"<<<BASH>>> {charCounter1}, {lineCounter1}");
                                        trackSegmetns[charCounter1, lineCounter1].carts.Clear();
                                        
                                        break;
                                    }
                                    if (trackSegmetns[charCounter1, lineCounter1].carts.Count > 0)
                                    {
                                        //Console.WriteLine($"{charCounter1}, {lineCounter1}");
                                        cartCounter++;
                                    }
                                    charCounter1++;
                                }
                                lineCounter1++;
                            }
                            if(cartCounter == 1)
                            {
                                lineCounter1 = 0;
                                foreach (var l in lines)
                                {
                                    int charCounter1 = 0;
                                    foreach (var c1 in line)
                                    {
                                        if (trackSegmetns[charCounter1, lineCounter1].carts.Count > 0)
                                        {
                                            Console.WriteLine($"Final cart {charCounter1}, {lineCounter1}");
                                            cartCounter++;
                                            found = true;
                                        }
                                        charCounter1++;
                                    }
                                    lineCounter1++;
                                }
                            }


                        }
                        charCounter++;
                    }
                    lineCounter++;
                }

                
                //Console.WriteLine();

                //break;
            }


            //Console.WriteLine(sum);
            Console.WriteLine("done.");
            Console.ReadLine();
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