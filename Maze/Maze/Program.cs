using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace Maze
{
    class Program
    {        
        static void Main(string[] args)
        {
            string mazeData = "I:\\Backup\\Masters\\UIUC\\2016\\Fall\\CS_440\\Homework\\1\\bigMaze.txt";
            List<List<char>> mazeBoard = new List<List<char>>();
            List<Node> visitedNodes = new List<Node>();
            List<Node> pathToGoalState = new List<Node>();
            List<Node> otherChildNodes = new List<Node>();

            int refreshDelayMS = 300;
            bool found = false;

            int intGoalX = 0;
            int intGoalY = 0;
            int intStartX = 1;
            int intStartY = 1;

            int i = 0;
            int j = 0;

            string[] lines = System.IO.File.ReadAllLines(@mazeData);

            // Display the file contents by using a foreach loop.
            foreach (string line in lines)
            {
                i++;                
                List<char> sublist = new List<char>();
                foreach(char c in line.ToCharArray())
                {
                    j++;
                    if (c.Equals('P'))
                    {
                        intStartX = j-1;
                        intStartY = i-1;
                    }
                    if (c.Equals('.'))
                    {
                        intGoalX = j-1;
                        intGoalY = i-1;
                    }
                    sublist.Add(c);
                    
                }
                j = 0;
                

                mazeBoard.Add(sublist);
            }

            //Console.WriteLine(args[0]);
            Thread.Sleep(refreshDelayMS);
            Console.Clear();
            Display(mazeBoard);

            // Start State 
            //Console.WriteLine(mazeBoard[intStartY][intStartX]);
            Console.WriteLine("Start State: " + intStartX + " " + intStartY + ": " + mazeBoard[intStartY][intStartX]);
            Node startStateNode = new Node(intStartX, intStartY, null);
            
            // Goal State
            //Console.WriteLine(mazeBoard[intGoalY][intGoalX]);
            Console.WriteLine("Goal State: " + intGoalX + " " + intGoalY + ": " + mazeBoard[intGoalY][intGoalX]);
            Node endGoalNode = new Node(intGoalX, intGoalY, null);
            Node currentNode = startStateNode;
            
            Random rand = new Random();
            // Log start of search
            DateTime start = DateTime.Now;
            // DFS
            //found = findDFSPath(currentNode, endGoalNode, mazeBoard, visitedNodes, pathToGoalState, rand, otherChildNodes, refreshDelayMS);

            //// BFS
            //List<Node> currentGeneration = new List<Node>();
            //currentGeneration.Add(currentNode);
            //found = findBFSPath(currentGeneration, endGoalNode, mazeBoard, visitedNodes, pathToGoalState, otherChildNodes, refreshDelayMS);

            // Greedy 
            Node startStateNodeG = new Node(intStartX, intStartY, null, endGoalNode);
            List<Node> currentGeneration = new List<Node>();
            currentNode = startStateNodeG;
            currentGeneration.Add(currentNode);
            found = findGreedyPath(currentGeneration, endGoalNode, mazeBoard, visitedNodes, pathToGoalState, otherChildNodes, refreshDelayMS);

            // A*
            //Node startStateNodeA = new Node(intStartX, intStartY, null, endGoalNode);
            //List<Node> currentGeneration = new List<Node>();
            //currentNode = startStateNodeA;
            //currentGeneration.Add(currentNode);
            //found = findAPath(currentGeneration, endGoalNode, mazeBoard, visitedNodes, pathToGoalState, otherChildNodes, refreshDelayMS);

            //Node n1 = new Node(1, 1, null);
            //n1.f = 192;
            //Node n2 = new Node(2, 3, null);
            //n2.f = 89;

            //List<Node> nList = new List<Node>();
            //nList.Add(n1);
            //nList.Add(n2);

            //List<Node> sortList = nList.OrderBy(o => o.f).ToList();

            //Console.WriteLine("Node (" + sortList[0].x + "," + sortList[0].y + ") has f: " + sortList[0].f);


            // Log end of search
            DateTime end = DateTime.Now;

            if (found)
            {
                // Display the finalPath backwards
                pathToGoalState.Reverse();
                foreach (Node n in pathToGoalState)
                {
                    n.showNodeInfo();
                }

                Console.WriteLine("****************");
                Console.WriteLine("Summary: ");
                Console.WriteLine("Search Started: " + start);
                Console.WriteLine("Search Ended: " + end);
                Console.WriteLine("Duration: " + (end - start));
                Console.WriteLine("Nodes visited: " + visitedNodes.Count());
                Console.WriteLine("Nodes in final path: " + pathToGoalState.Count());
                Console.WriteLine("Cost of final path: " + pathToGoalState[pathToGoalState.Count - 1].f);
                Console.WriteLine("****************");

            }
            else
            {
                Console.WriteLine("****************");
                Console.WriteLine("Summary: ");
                Console.WriteLine("Search Started: " + start);
                Console.WriteLine("Search Ended: " + end);
                Console.WriteLine("Duration: " + (end - start));
                Console.WriteLine("Nodes visited: " + visitedNodes.Count());
                Console.WriteLine("****************");
            }

                Console.WriteLine("Press anykey to quit");
            Console.ReadKey();
            
        }

        static bool findDFSPath(Node currentNode, Node goalStateNode, List<List<char>> mazeBoard, List<Node> visitedNodes, List<Node> finalPathOfNodes, Random rand, List<Node> otherChildNodes, int refreshDelayMS)
        {
            // In case of backtracking, no need to add a revisited node 
            // Perhaps a wall was hit, and backtracking is necessary.  No need to add the
            // revisited node again.
            if (!visitedNodes.Contains(currentNode))
            {
                visitedNodes.Add(currentNode);
            }

            mazeBoard[currentNode.y][currentNode.x] = 'v';
            Thread.Sleep(refreshDelayMS);
            Console.Clear();
            Display(mazeBoard);

            Node nextNode = new Node(0, 0, null);
            if (currentNode.Equals(goalStateNode))
            {
                finalPathOfNodes.Clear();
                finalPathOfNodes.Add(currentNode);

                while (currentNode.parentNode != null)
                {
                    mazeBoard[currentNode.y][currentNode.x] = '.';
                    nextNode = currentNode.parentNode;
                    finalPathOfNodes.Add(nextNode);
                    currentNode = nextNode;
                }
                mazeBoard[currentNode.y][currentNode.x] = 'P';

                Thread.Sleep(refreshDelayMS);
                Console.Clear();
                Display(mazeBoard);
            
                return true;
            }
            else
            {
                if (currentNode.childNodes == null)
                {
                    currentNode.childNodes = currentNode.findEligibleChildren(mazeBoard, otherChildNodes);
                }

                currentNode.showNodeInfo();

                int randNumber = 0;
                if (currentNode.childNodes != null && currentNode.childNodes.Count > 0)
                {

                    // Mark childNodes as being already a child to some other parent.
                    foreach (Node n in currentNode.childNodes)
                    {
                        if (!otherChildNodes.Contains(n))
                        {
                            otherChildNodes.AddRange(currentNode.childNodes);
                        }
                    }

                    // Remove visited childNodes as repeatable options.
                    foreach (Node n in visitedNodes)
                    {
                        if (currentNode.childNodes.Contains(n))
                        {
                            currentNode.childNodes.Remove(n);
                        }
                    }
                    // Any unvisited children should be visited next
                    if (currentNode.childNodes.Count > 0)
                    {
                        randNumber = rand.Next(0, currentNode.childNodes.Count - 1);

                        nextNode = currentNode.childNodes[randNumber];
                        if (findDFSPath(nextNode, goalStateNode, mazeBoard, visitedNodes, finalPathOfNodes, rand, otherChildNodes, refreshDelayMS))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        // If all childNodes are visited, then go back to parentNode.
                        // If parentNode is null, perhaps there is no goalState
                        if (currentNode.parentNode != null)
                        {
                            nextNode = currentNode.parentNode;
                            if (findDFSPath(nextNode, goalStateNode, mazeBoard, visitedNodes, finalPathOfNodes, rand, otherChildNodes, refreshDelayMS))
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }

                    //foreach (Node n in childNodes)
                    //{
                    //}
                    //   n.showNodeInfo();
                }
                else
                {
                    if (currentNode.parentNode != null)
                    {
                        nextNode = currentNode.parentNode;
                        if (findDFSPath(nextNode, goalStateNode, mazeBoard, visitedNodes, finalPathOfNodes, rand, otherChildNodes, refreshDelayMS))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

            }

            return false;

        }

        static bool findInformedDFSPath(Node currentNode, Node goalStateNode, List<List<char>> mazeBoard, List<Node> visitedNodes, List<Node> finalPathOfNodes, Random rand, List<Node> otherChildNodes, int refreshDelayMS)
        {
            // In case of backtracking, no need to add a revisited node 
            // Perhaps a wall was hit, and backtracking is necessary.  No need to add the
            // revisited node again.
            if (!visitedNodes.Contains(currentNode))
            {
                visitedNodes.Add(currentNode);
            }

            mazeBoard[currentNode.y][currentNode.x] = 'v';
            Thread.Sleep(refreshDelayMS);
            Console.Clear();
            Display(mazeBoard);

            Node nextNode = new Node(0, 0, null);
            if (currentNode.Equals(goalStateNode))
            {
                finalPathOfNodes.Clear();
                finalPathOfNodes.Add(currentNode);

                while (currentNode.parentNode != null)
                {
                    mazeBoard[currentNode.y][currentNode.x] = '.';
                    nextNode = currentNode.parentNode;
                    finalPathOfNodes.Add(nextNode);
                    currentNode = nextNode;
                }
                mazeBoard[currentNode.y][currentNode.x] = 'P';
                Thread.Sleep(refreshDelayMS);
                Console.Clear();
                Display(mazeBoard);

                return true;
            }
            else
            {
                if (currentNode.childNodes == null)
                {
                    currentNode.childNodes = currentNode.findEligibleChildren(mazeBoard, otherChildNodes);
                }

                currentNode.showNodeInfo();

                //int randNumber = 0;
                if (currentNode.childNodes != null && currentNode.childNodes.Count > 0)
                {

                    // Mark childNodes as being already a child to some other parent.
                    foreach (Node n in currentNode.childNodes)
                    {
                        if (!otherChildNodes.Contains(n))
                        {
                            otherChildNodes.AddRange(currentNode.childNodes);
                        }
                    }

                    // Remove visited childNodes as repeatable options.
                    foreach (Node n in visitedNodes)
                    {
                        if (currentNode.childNodes.Contains(n))
                        {
                            currentNode.childNodes.Remove(n);
                        }
                    }
                    // Any unvisited children should be visited next
                    if (currentNode.childNodes.Count > 0)
                    {
                        //randNumber = rand.Next(0, currentNode.childNodes.Count - 1);

                        //nextNode = currentNode.childNodes[randNumber];
                        nextNode = getClosestNode(currentNode.childNodes, goalStateNode);
                        if (findInformedDFSPath(nextNode, goalStateNode, mazeBoard, visitedNodes, finalPathOfNodes, rand, otherChildNodes, refreshDelayMS))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        // If all childNodes are visited, then go back to parentNode.
                        // If parentNode is null, perhaps there is no goalState
                        if (currentNode.parentNode != null)
                        {
                            nextNode = currentNode.parentNode;
                            if (findInformedDFSPath(nextNode, goalStateNode, mazeBoard, visitedNodes, finalPathOfNodes, rand, otherChildNodes, refreshDelayMS))
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if (currentNode.parentNode != null)
                    {
                        nextNode = currentNode.parentNode;
                        if (findInformedDFSPath(nextNode, goalStateNode, mazeBoard, visitedNodes, finalPathOfNodes, rand, otherChildNodes, refreshDelayMS))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

            }

            return false;

        }
        static bool findGreedyPath(List<Node> nextOptions, Node goalStateNode, List<List<char>> mazeBoard, List<Node> visitedNodes, List<Node> finalPathOfNodes, List<Node> otherChildNodes, int refreshDelayMS)
        {
            Node currentNode = new Node(0, 0, null, goalStateNode);
            Node nextNode = new Node(0, 0, null, goalStateNode);
            List<Node> sortList = nextOptions.OrderBy(o => o.f).ToList();

            // Get the best option of the nextOptions.
            // If there is no other options, no solution found.
            if (sortList[0] != null)
            {
                currentNode = sortList[0];
            }
            else
            {
                return false;
            }

            // Only difference between Greedy and A is the g component
            currentNode.g = 0;

            if (!visitedNodes.Contains(currentNode))
            {
                visitedNodes.Add(currentNode);

                mazeBoard[currentNode.y][currentNode.x] = 'v';
                Thread.Sleep(refreshDelayMS);
                Console.Clear();
                Display(mazeBoard);

                if (currentNode.Equals(goalStateNode))
                {
                    finalPathOfNodes.Clear();
                    finalPathOfNodes.Add(currentNode);
                    Node tmpNode = new Node(currentNode.x, currentNode.y, currentNode.parentNode);
                    while (tmpNode.parentNode != null)
                    {
                        mazeBoard[tmpNode.y][tmpNode.x] = '.';
                        nextNode = tmpNode.parentNode;
                        finalPathOfNodes.Add(nextNode);
                        tmpNode = nextNode;
                    }
                    mazeBoard[tmpNode.y][tmpNode.x] = 'P';
                    Thread.Sleep(refreshDelayMS);
                    Console.Clear();
                    Display(mazeBoard);

                    return true;
                }
                else
                {
                    if (currentNode.childNodes == null)
                    {
                        currentNode.childNodes = currentNode.findEligibleChildrenA(mazeBoard);
                    }

                    currentNode.showNodeInfo();

                    if (currentNode.childNodes != null && currentNode.childNodes.Count > 0)
                    {

                        // Mark childNodes as being already a child to some other parent.
                        foreach (Node n in currentNode.childNodes)
                        {
                            if (!otherChildNodes.Contains(n))
                            {
                                otherChildNodes.Add(n);
                            }
                            else
                            {
                                if (otherChildNodes.ElementAt(otherChildNodes.IndexOf(n)).f > n.f)
                                {
                                    // Update parentNode, g, h, f 
                                    otherChildNodes.ElementAt(otherChildNodes.IndexOf(n)).parentNode = currentNode;
                                    otherChildNodes.ElementAt(otherChildNodes.IndexOf(n)).f = n.f;
                                    otherChildNodes.ElementAt(otherChildNodes.IndexOf(n)).g = n.g;
                                    otherChildNodes.ElementAt(otherChildNodes.IndexOf(n)).h = n.h;
                                    // If it's updated, it may need to be revisited.
                                    if (visitedNodes.Contains(n))
                                    {
                                        visitedNodes.Remove(n);
                                    }

                                }
                            }
                        }

                    }
                    nextOptions.Remove(currentNode);

                    // Remove visited childNodes as repeatable options.
                    foreach (Node n in visitedNodes)
                    {
                        if (otherChildNodes.Contains(n))
                        {
                            otherChildNodes.Remove(n);
                        }
                    }

                    // Update any recalculated otherChildNodes into the nextOption list.
                    foreach (Node n in otherChildNodes)
                    {
                        if (nextOptions.Contains(n))
                        {
                            nextOptions.Remove(n);
                        }

                        nextOptions.Add(n);
                    }


                    findGreedyPath(nextOptions, goalStateNode, mazeBoard, visitedNodes, finalPathOfNodes, otherChildNodes, refreshDelayMS);

                }

            }
            else
            {
                nextOptions.Remove(currentNode);
                if (nextOptions != null)
                {
                    sortList = nextOptions.OrderBy(o => o.f).ToList();
                    findGreedyPath(sortList, goalStateNode, mazeBoard, visitedNodes, finalPathOfNodes, otherChildNodes, refreshDelayMS);
                }
                else
                {
                    return false;
                }
            }

            return true;

        }

        static bool findBFSPath(List<Node> currGen, Node goalStateNode, List<List<char>> mazeBoard, List<Node> visitedNodes, List<Node> finalPathOfNodes, List<Node> otherChildNodes, int refreshDelayMS)
        {
            List<Node> nextGeneration = new List<Node>();
            Node nextNode = new Node(0, 0, null);
            foreach (Node currentNode in currGen)
            {
                if (!visitedNodes.Contains(currentNode))
                {
                    visitedNodes.Add(currentNode);
                }

                mazeBoard[currentNode.y][currentNode.x] = 'v';
                Thread.Sleep(refreshDelayMS);
                Console.Clear();
                Display(mazeBoard);

                if (currentNode.Equals(goalStateNode))
                {
                    finalPathOfNodes.Clear();
                    finalPathOfNodes.Add(currentNode);
                    Node tmpNode = new Node(currentNode.x, currentNode.y, currentNode.parentNode);
                    while (tmpNode.parentNode != null)
                    {
                        mazeBoard[tmpNode.y][tmpNode.x] = '.';
                        nextNode = tmpNode.parentNode;
                        finalPathOfNodes.Add(nextNode);
                        tmpNode = nextNode;
                    }
                    mazeBoard[tmpNode.y][tmpNode.x] = 'P';
                    Thread.Sleep(refreshDelayMS);
                    Console.Clear();
                    Display(mazeBoard);

                    return true;
                }
                else
                {
                    if (currentNode.childNodes == null)
                    {
                        currentNode.childNodes = currentNode.findEligibleChildren(mazeBoard, otherChildNodes);
                    }
                    
                    currentNode.showNodeInfo();

                    //int randNumber = 0;
                    if (currentNode.childNodes != null && currentNode.childNodes.Count > 0)
                    {

                        // Mark childNodes as being already a child to some other parent.
                        foreach (Node n in currentNode.childNodes)
                        {
                            if (!otherChildNodes.Contains(n))
                            {
                                // TODO: Should I just add n?
                                otherChildNodes.AddRange(currentNode.childNodes);
                            }
                        }

                        // Remove visited childNodes as repeatable options.
                        foreach (Node n in visitedNodes)
                        {
                            if (currentNode.childNodes.Contains(n))
                            {
                                currentNode.childNodes.Remove(n);
                            }
                        }

                    }

                    if (currentNode.childNodes != null)
                    {
                        nextGeneration.AddRange(currentNode.childNodes);
                    }
                }
            }
            if (nextGeneration != null)
            {
                findBFSPath(nextGeneration, goalStateNode, mazeBoard, visitedNodes, finalPathOfNodes, otherChildNodes, refreshDelayMS);
            }
            else
            {
                return false;
            }

            return true;

        }


        static bool findAPath(List<Node> nextOptions, Node goalStateNode, List<List<char>> mazeBoard, List<Node> visitedNodes, List<Node> finalPathOfNodes, List<Node> otherChildNodes, int refreshDelayMS)
        {
            Node currentNode = new Node(0, 0, null, goalStateNode);
            Node nextNode = new Node(0, 0, null, goalStateNode);    
            List<Node> sortList = nextOptions.OrderBy(o => o.f).ToList();

            // Get the best option of the nextOptions.
            // If there is no other options, no solution found.
            if (sortList[0] != null)
            {
                currentNode = sortList[0];
            }
            else {
                return false;
            }

            if (!visitedNodes.Contains(currentNode))
            {
                visitedNodes.Add(currentNode);

                mazeBoard[currentNode.y][currentNode.x] = 'v';
                Thread.Sleep(refreshDelayMS);
                Console.Clear();
                Display(mazeBoard);

                if (currentNode.Equals(goalStateNode))
                {
                    finalPathOfNodes.Clear();
                    finalPathOfNodes.Add(currentNode);
                    Node tmpNode = new Node(currentNode.x, currentNode.y, currentNode.parentNode);
                    while (tmpNode.parentNode != null)
                    {
                        mazeBoard[tmpNode.y][tmpNode.x] = '.';
                        nextNode = tmpNode.parentNode;
                        finalPathOfNodes.Add(nextNode);
                        tmpNode = nextNode;
                    }
                    mazeBoard[tmpNode.y][tmpNode.x] = 'P';
                    Thread.Sleep(refreshDelayMS);
                    Console.Clear();
                    Display(mazeBoard);

                    return true;
                }
                else
                {
                    if (currentNode.childNodes == null)
                    {
                        currentNode.childNodes = currentNode.findEligibleChildrenA(mazeBoard);
                    }

                    currentNode.showNodeInfo();
                    //foreach(Node n in sortList)
                    //{
                    //    Console.WriteLine("Node f: " + n.f + " (" + n.x + "," + n.y + ")");
                    //}

                            if (currentNode.childNodes != null && currentNode.childNodes.Count > 0)
                    {

                        //// Remove visited childNodes as repeatable options.
                        //foreach (Node n in visitedNodes)
                        //{
                        //    if (currentNode.childNodes.Contains(n))
                        //    {
                        //        currentNode.childNodes.Remove(n);
                        //    }
                        //}

                        // Mark childNodes as being already a child to some other parent.
                        foreach (Node n in currentNode.childNodes)
                        {
                            if (!otherChildNodes.Contains(n))
                            {
                                otherChildNodes.Add(n);
                            }
                            else
                            {
                                if (otherChildNodes.ElementAt(otherChildNodes.IndexOf(n)).f > n.f)
                                {
                                    // Update parentNode, g, h, f 
                                    otherChildNodes.ElementAt(otherChildNodes.IndexOf(n)).parentNode = currentNode;
                                    otherChildNodes.ElementAt(otherChildNodes.IndexOf(n)).f = n.f;
                                    otherChildNodes.ElementAt(otherChildNodes.IndexOf(n)).g = n.g;
                                    otherChildNodes.ElementAt(otherChildNodes.IndexOf(n)).h = n.h;
                                    // If it's updated, it may need to be revisited.
                                    if (visitedNodes.Contains(n))
                                    {
                                        visitedNodes.Remove(n);
                                    }

                                }
                            }
                        }

                    }
                    nextOptions.Remove(currentNode);

                    //if (currentNode.x == 15 && currentNode.y == 8)
                    //{
                    //    Console.Write("test");
                    //}

                    // Remove visited childNodes as repeatable options.
                    foreach (Node n in visitedNodes)
                    {
                        if (otherChildNodes.Contains(n))
                        {
                            otherChildNodes.Remove(n);
                        }
                    }

                    // Update any recalculated otherChildNodes into the nextOption list.
                    foreach (Node n in otherChildNodes)
                    {
                        if (nextOptions.Contains(n))
                        {
                            nextOptions.Remove(n);
                        }

                        nextOptions.Add(n);
                    }
                    
                    findAPath(nextOptions, goalStateNode, mazeBoard, visitedNodes, finalPathOfNodes, otherChildNodes, refreshDelayMS);

                }

            }
            else
            {
                nextOptions.Remove(currentNode);
                if (nextOptions != null)
                {
                    sortList = nextOptions.OrderBy(o => o.f).ToList();
                    findAPath(sortList, goalStateNode, mazeBoard, visitedNodes, finalPathOfNodes, otherChildNodes, refreshDelayMS);
                }
                else
                {
                    return false;
                }
            }

            return true;

        }

        static Node getClosestNode(List<Node> childNodes, Node goalStateNode)
        {
            int z = -1;
            int temp_z = 0;
            Node closestNode = goalStateNode;
            foreach (Node n in childNodes)
            {
                temp_z = calcManhattanDistance(n, goalStateNode);
                if (temp_z < z || z == -1)
                {
                    z = temp_z;
                    closestNode = n;
                }
            }
            return closestNode;
        }

        static int calcManhattanDistance(Node n, Node goalStateNode)
        {
            int z = 0;
            int xdelta = 0;
            int ydelta = 0;

            xdelta = Math.Abs(n.x - goalStateNode.x);
            ydelta = Math.Abs(n.y - goalStateNode.y);

            z = xdelta + ydelta;
            return z;
        }
        static void Display(List<List<char>> list)
        {
            //
            // Display everything in the List.
            //
            Console.WriteLine("Board:");
            foreach (var sublist in list)
            {
                foreach (var value in sublist)
                {
                    Console.Write(value);
                    Console.Write(' ');
                }
                Console.WriteLine();
            }

        }
    }
}
