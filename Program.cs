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
            string mazeData = "";
            //if (args[0] != null)
            //{
            //    mazeData = args[0];
            //}
            //else
            //{ 
            //    mazeData = "C:\\Users\\mingyili\\Documents\\GitHub\\NewCS440-HW1\\CS440-HW1\\tinySearch.txt";
            //}
            mazeData = "E:\\Softwares\\Visual Studio 2010\\Projects\\ECE448_HW1\\ECE448_AI_HW1\\ECE448_AI_HW1\\tinySearch.txt";
            List<List<char>> mazeBoard = new List<List<char>>();
            List<Node> visitedNodes = new List<Node>();
            List<Node> pathToGoalState = new List<Node>();
            List<Node> otherChildNodes = new List<Node>();

            int refreshDelayMS = 100;
            bool found = false;
            //int goalNums = 0;

            Console.WriteLine("Is this for multiple dots? (Y/N)");
            string part = "";
            part = Console.ReadLine();

            // for 1.1
            if (part.ToLower() == "n")
            {

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
                    foreach (char c in line.ToCharArray())
                    {
                        j++;
                        if (c.Equals('P'))
                        {
                            intStartX = j - 1;
                            intStartY = i - 1;
                        }
                        if (c.Equals('.'))
                        {
                            intGoalX = j - 1;
                            intGoalY = i - 1;
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
                //Node startStateNode = new Node(intStartX, intStartY, null);

                // Goal State
                //Console.WriteLine(mazeBoard[intGoalY][intGoalX]);
                Console.WriteLine("Goal State: " + intGoalX + " " + intGoalY + ": " + mazeBoard[intGoalY][intGoalX]);

                string algorithm = "";
                int value = 0;
                bool keepAsking = true;
                while (keepAsking)
                {
                    Console.WriteLine("Navigating through: " + mazeData);

                    Console.WriteLine("What algorithm do you want to run? (1-4)");
                    Console.WriteLine("1 for DFS");
                    Console.WriteLine("2 for BFS");
                    Console.WriteLine("3 for Greedy");
                    Console.WriteLine("4 for A*");

                    algorithm = Console.ReadLine(); // Read string from console
                    if (int.TryParse(algorithm, out value)) // Try to parse the string as an integer
                    {
                        if (value > 4)
                        {
                            Console.WriteLine("Please enter value between 1 and 4!");
                            continue;
                        }
                        else
                        {
                            keepAsking = false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Not an integer!");
                    }

                }
                keepAsking = true;
                int maxDepth = 0;
                while (keepAsking)
                {
                    Console.WriteLine("What depth do you want to run (integer)");
                    algorithm = Console.ReadLine(); // Read string from console
                    if (int.TryParse(algorithm, out maxDepth)) // Try to parse the string as an integer
                    {
                        keepAsking = false;
                    }
                    else
                    {
                        Console.WriteLine("Not an integer!");
                    }

                }


                //Random rand = new Random();
                // Log start of search
                DateTime start = DateTime.Now;


                List<Node> nextOptions = new List<Node>();
                Node endGoalNode = new Node(intGoalX, intGoalY, null);
                Node startStateNode = new Node(intStartX, intStartY, null, endGoalNode);
                Node currentNode = startStateNode;
                int tmpDepth = 0;
                nextOptions.Add(currentNode);
                while (!found)
                {
                    if (tmpDepth > maxDepth)
                    {
                        break;
                    }

                    // May need this code below if we decide to pass in list of goalStateNodes

                    //if (goalStateNodes != null)
                    //{
                    //found = findAPathHash(currentGeneration, goalStateNodes, visitedNodesHash, pathToGoalState, otherChildNodes, refreshDelayMS, maxDepth);

                    found = findPath(value, nextOptions, endGoalNode, mazeBoard, visitedNodes, pathToGoalState, otherChildNodes, refreshDelayMS, tmpDepth);
                    tmpDepth++;
                    //}
                    //else
                    //{
                    //    break;
                    //}
                }

                // DFS
                //found = findDFSPath(currentNode, endGoalNode, mazeBoard, visitedNodes, pathToGoalState, rand, otherChildNodes, refreshDelayMS);

                //// BFS
                //List<Node> currentGeneration = new List<Node>();
                //currentGeneration.Add(currentNode);
                //found = findBFSPath(currentGeneration, endGoalNode, mazeBoard, visitedNodes, pathToGoalState, otherChildNodes, refreshDelayMS);

                // Greedy 
                //Node startStateNodeG = new Node(intStartX, intStartY, null, endGoalNode);
                //List<Node> currentGeneration = new List<Node>();
                //currentNode = startStateNodeG;
                //currentGeneration.Add(currentNode);
                //found = findGreedyPath(currentGeneration, endGoalNode, mazeBoard, visitedNodes, pathToGoalState, otherChildNodes, refreshDelayMS);

                // A*
                //Node startStateNodeA = new Node(intStartX, intStartY, null, endGoalNode);
                //List<Node> currentGeneration = new List<Node>();
                //currentNode = startStateNodeA;
                //currentGeneration.Add(currentNode);
                //found = findAPath(currentGeneration, endGoalNode, mazeBoard, visitedNodes, pathToGoalState, otherChildNodes, refreshDelayMS);

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

            // for 1.2
            else
            {
                int intStartX = 0;
                int intStartY = 0;
                List<Node> goalList = new List<Node>();
                List<Node> tempGoalList = new List<Node>();
                tempGoalList = goalList;
                int goalNums = 0;
                int currNums = 0;

                int i = 0;
                int j = 0;
                int width = 0;

                string[] lines = System.IO.File.ReadAllLines(@mazeData);

                foreach (string line in lines)
                {
                    i++;
                    List<char> sublist = new List<char>();

                    foreach (char c in line.ToCharArray())
                    {
                        j++;
                        if (c.Equals('P'))
                        {
                            intStartX = j - 1;
                            intStartY = i - 1;
                        }
                        else if (c.Equals('.'))
                        {
                            Node newNode = new Node(j - 1, i - 1, null);
                            goalList.Insert(0, newNode);
                            goalNums++;
                        }
                        sublist.Add(c);
                        width = j;
                    }
                    j = 0;
                    mazeBoard.Add(sublist);
                }

                Console.WriteLine("i: " + i);
                Console.WriteLine("width: " + width);

                Console.ReadKey();

                Thread.Sleep(refreshDelayMS);
                Console.Clear();
                Display(mazeBoard);

                // Start State
                Console.WriteLine("Start State:" + intStartX + " " + intStartY + ": " + mazeBoard[intStartY][intStartX]);

                // Goal State
                Console.WriteLine("Number of Goal States:" + goalNums);

                bool keepAsking = true;
                string parse;
                int maxDepth = 0;
                while (keepAsking)
                {
                    Console.WriteLine("What depth do you want to run (integer)");
                    parse = Console.ReadLine(); // Read string from console
                    if (int.TryParse(parse, out maxDepth)) // Try to parse the string as an integer
                    {
                        keepAsking = false;
                    }
                    else
                    {
                        Console.WriteLine("Not an integer!");
                    }
                }


                //Random rand = new Random();
                // Log start of search
                DateTime start = DateTime.Now;


                List<Node> nextOptions = new List<Node>();
                // Node endGoalNode = new Node(intGoalX, intGoalY, null);
                Node startStateNode = new Node(intStartX, intStartY, null, goalNums);
                Node currentNode = startStateNode;
                int tmpDepth = 0;
                nextOptions.Add(currentNode);
                while (!found)
                {
                    if (tmpDepth > maxDepth)
                    {
                        break;
                    }

                    // May need this code below if we decide to pass in list of goalStateNodes

                    //if (goalStateNodes != null)
                    //{
                    //found = findAPathHash(currentGeneration, goalStateNodes, visitedNodesHash, pathToGoalState, otherChildNodes, refreshDelayMS, maxDepth);

                    found = findPathMultiple(tempGoalList, ref currNums, goalList, nextOptions, goalNums, mazeBoard, visitedNodes, pathToGoalState, otherChildNodes, refreshDelayMS, tmpDepth);
                    tmpDepth++;
                    //}
                    //else
                    //{
                    //    break;
                    //}
                }
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
                Console.WriteLine("found: " + found);
                Console.WriteLine("currNums: " + currNums);

                Console.ReadKey();
            }
        }


        /*
        int calcManhattan12(int x, int y, List<Node> goalList)
        {
            int z = 0;
            int xdelta = 0;
            int ydelta = 0;

            foreach (Node node in goalList)
            {
                xdelta = Math.Abs(x - node.x);
                ydelta = Math.Abs(y - node.y);
                z += xdelta + ydelta;
            }
            return z;
        }
        */

        static bool findPathMultiple(List<Node> tempGoalList, ref int currNums, List<Node> goalList, List<Node> nextOptions, int goalNums, List<List<char>> mazeBoard, List<Node> visitedNodes, List<Node> finalPathOfNodes, List<Node> otherChildNodes, int refreshDelayMS, int maxDepth)
        {
            Node currentNode = new Node(0, 0, null, goalNums);
            Node nextNode = new Node(0, 0, null, goalNums);
            char[] order = { '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

            List<Node> sortList = new List<Node>();
            sortList = nextOptions.OrderBy(o => o.f).ThenBy(n => n.h).ToList();
            if (!sortList.Any())
            {
                if (currNums == goalNums)
                { ; }
                else
                {
                    return false;
                }
            }
           
          
                currentNode = sortList[0];
            
            

            //List<Node> tempGoalList = new List<Node>();
            //tempGoalList = goalList;

            if (currentNode.g > maxDepth)
            {
                return false;
            }

            // Have I visited this already?
            Console.WriteLine("!visitedNodes.Contains(currentNode): " + !visitedNodes.Contains(currentNode));
            
            if (!visitedNodes.Contains(currentNode))
            {
                // Mark as visited
                visitedNodes.Add(currentNode);

                // Update on the board
                //mazeBoard[currentNode.y][currentNode.x] = 'v';

                // Show previous board on console for a few seconds before updating
                Thread.Sleep(refreshDelayMS);

                // Now show new mazeBoard
                Console.Clear();
                Display(mazeBoard);

                // Check goalState
                Console.WriteLine("goalList.Contains(currentNode): " + goalList.Contains(currentNode));
                Console.WriteLine("(currNums < goalNums): " + (currNums < goalNums));
                bool goalListContainsCurrentNode = goalList.Contains(currentNode);
                bool currNumsLessThanGoalNums = (currNums < goalNums);
                bool visitGoals = goalListContainsCurrentNode && currNumsLessThanGoalNums;
                
                if (goalListContainsCurrentNode && currNumsLessThanGoalNums)
                {
                    currNums++;
                    mazeBoard[currentNode.y][currentNode.x] = order[currNums-1];
                    //goalList.Remove(currentNode);
                    tempGoalList.Remove(currentNode);               
                }

                if (goalList.Contains(currentNode)/* && (goalNums == currNums)*/)
                {
                    // I'm done.  Calculate my finalPathOfNodes by backtracking from my currentNode to the node which has no parent (root)
                    finalPathOfNodes.Clear();
                    finalPathOfNodes.Add(currentNode);
                    
                    //int i = 0;
                    Node tmpNode = new Node(currentNode.x, currentNode.y, currentNode.parentNode);
                    // Mark paths with '.'
                    //while (tmpNode.parentNode != null)
                    Node tempNode = new Node(4, 4, null);
                    while (!goalList.Contains(tmpNode.parentNode) && !tempNode.parentNode.Equals(tempNode))
                    {

                        //mazeBoard[tmpNode.y][tmpNode.x] = '.';
                        nextNode = tmpNode.parentNode;
                        finalPathOfNodes.Add(nextNode);
                        tmpNode = nextNode;
                       // i++;
                    }

                    // Mark the root as 'P'
                    mazeBoard[tmpNode.y][tmpNode.x] = 'P';

                    // Show maze for a few seconds
                    Thread.Sleep(refreshDelayMS);
                    Console.Clear();
                    Display(mazeBoard);
                    return true;
                }
                else
                {
                    /*
                    //bool visitGoals = goalListContainsCurrentNode && currNumsLessThanGoalNums;
                    if (goalListContainsCurrentNode && currNumsLessThanGoalNums)
                    {
                        currNums++;
                        //goalList.Remove(currentNode);
                        tempGoalList.Remove(currentNode);               
                    }
                     * */
                    
                    // Not done, need to do work.
                    // What can this currentNode add to the Frontier (N,S,E,W)
                    if (currentNode.childNodes == null)
                    {
                        // May need to compare findEligibleChildren with A version (f,g,h changes and in original, I disallow child nodes to be eligible)
                        currentNode.childNodes = currentNode.findEligibleChildrenAMultiple(mazeBoard, goalList);
                    }

                    // Show node investigating
                    currentNode.showNodeInfo();

                    // Are there child nodes?  What can I add to frontier?
                    if (currentNode.childNodes != null && currentNode.childNodes.Count > 0)
                    {

                        // Mark childNodes as being already a child to some other parent.
                        foreach (Node n in currentNode.childNodes)
                        {
                            // If it doesn't belong to a parent, mark it
                            if (!otherChildNodes.Contains(n))
                            {
                                otherChildNodes.Add(n);
                            }
                            else
                            {
                                // A* needs to check if a node has a cheaper total path cost.  
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


                    // Remove this node from the Frontier
                    nextOptions.Remove(currentNode);

                    // Remove visited childNodes as repeatable options.
                    // Is this needed?  Does it hurt?
                    foreach (Node n in visitedNodes)
                    {
                        if (otherChildNodes.Contains(n))
                        {
                            otherChildNodes.Remove(n);
                        }
                    }

                    // Add new children to the frontier
                    // Update any recalculated otherChildNodes into the nextOption list.
                    foreach (Node n in otherChildNodes)
                    {
                        if (nextOptions.Contains(n))
                        {
                            nextOptions.Remove(n);
                        }

                        nextOptions.Add(n);
                    }

                    //return false;
                    //findGreedyPath(nextOptions, goalStateNode, mazeBoard, visitedNodes, finalPathOfNodes, otherChildNodes, refreshDelayMS);
                }
            }


            else
            {
                // visited already, remove this for Frontier
                // don't return to get out of routine.  Move on to next available from our sortedList (Frontier)
                sortList.Remove(currentNode);  // don't want to keep checking this in a loop.  Needs to be removed from sortList for next iteration.
                nextOptions.Remove(currentNode);  // may not be needed as this section is primarily for moving on to other siblings within this depth.  nextOptions has no impact here.

                //if (nextOptions != null)
                //{
                //    sortList = nextOptions.OrderBy(o => o.f).ToList();
                //    findGreedyPath(sortList, goalStateNode, mazeBoard, visitedNodes, finalPathOfNodes, otherChildNodes, refreshDelayMS);
                //}
                //else
                //{
                //    return false;
                //}
            }

            return false;
        }


        static bool findPath(int algo, List<Node> nextOptions, Node goalStateNode, List<List<char>> mazeBoard, List<Node> visitedNodes, List<Node> finalPathOfNodes, List<Node> otherChildNodes, int refreshDelayMS, int maxDepth)
        {
            Node currentNode = new Node(0, 0, null, goalStateNode);
            Node nextNode = new Node(0, 0, null, goalStateNode);

            List<Node> sortList = new List<Node>();

            while (nextOptions.Count > 0)
            {
                // algo = 1 DFS, 2 BFS, 3 Greedy, 4 A*
                if (algo == 1)
                {
                    // TODO:
                    sortList = nextOptions.OrderByDescending(o => o.g).ToList();
                }
                else if (algo == 2)
                {
                    sortList = nextOptions.OrderBy(o => o.g).ToList();
                }
                else if (algo == 3)
                {
                    sortList = nextOptions.OrderBy(o => o.g).ToList();
                }
                else if (algo == 4)
                {
                    sortList = nextOptions.OrderBy(o => o.f).ThenBy(n => n.h).ToList();
                }

                // Get the best option of the nextOptions.
                // If there is no other options, no solution found.
                currentNode = sortList[0];

                // Has the one I chose, by my algorithm violated my depth
                if (currentNode.g > maxDepth)
                {
                    return false;
                }

                // Have I visited this already?
                if (!visitedNodes.Contains(currentNode))
                {
                    // Mark as visited
                    visitedNodes.Add(currentNode);

                    // Update on the board
                    mazeBoard[currentNode.y][currentNode.x] = 'v';

                    // Show previous board on console for a few seconds before updating
                    Thread.Sleep(refreshDelayMS);

                    // Now show new mazeBoard
                    Console.Clear();
                    Display(mazeBoard);

                    // Check goalState
                    if (currentNode.Equals(goalStateNode))
                    {
                        // I'm done.  Calculate my finalPathOfNodes by backtracking from my currentNode to the node which has no parent (root)
                        finalPathOfNodes.Clear();
                        finalPathOfNodes.Add(currentNode);
                        Node tmpNode = new Node(currentNode.x, currentNode.y, currentNode.parentNode);
                        // Mark paths with '.'
                        while (tmpNode.parentNode != null)
                        {
                            mazeBoard[tmpNode.y][tmpNode.x] = '.';
                            nextNode = tmpNode.parentNode;
                            finalPathOfNodes.Add(nextNode);
                            tmpNode = nextNode;
                        }
                        // Mark the root as 'P'
                        mazeBoard[tmpNode.y][tmpNode.x] = 'P';
                        // Show maze for a few seconds
                        Thread.Sleep(refreshDelayMS);
                        Console.Clear();
                        Display(mazeBoard);

                        return true;
                    }
                    else
                    {
                        // Not done, need to do work.
                        // What can this currentNode add to the Frontier (N,S,E,W)
                        if (currentNode.childNodes == null)
                        {
                            // May need to compare findEligibleChildren with A version (f,g,h changes and in original, I disallow child nodes to be eligible)
                            currentNode.childNodes = currentNode.findEligibleChildrenA(mazeBoard);
                        }

                        // Show node investigating
                        currentNode.showNodeInfo();

                        // Are there child nodes?  What can I add to frontier?
                        if (currentNode.childNodes != null && currentNode.childNodes.Count > 0)
                        {

                            // Mark childNodes as being already a child to some other parent.
                            foreach (Node n in currentNode.childNodes)
                            {
                                // If it doesn't belong to a parent, mark it
                                if (!otherChildNodes.Contains(n))
                                {
                                    otherChildNodes.Add(n);
                                }
                                else
                                {
                                    // If it does (DFS/BFS) need to handle this.
                                    if (algo == 1)
                                    {
                                        // DFS this node can be revisited if we dont remove
                                        // EX:
                                        //                R
                                        //             A    B
                                        //            C D  C E
                                        //  Visit A, C, D...then B, C (gets revisited, but shouldn't).  Remove.
                                        // Remove B's child C                                        
                                        otherChildNodes.Remove(n);
                                    }
                                    else if (algo == 2)
                                    {
                                        // BFS this node is already claimed
                                        // EX:
                                        //                R
                                        //             A    B
                                        //            C D  C E
                                        // Remove B's child C
                                        otherChildNodes.Remove(n);
                                    }
                                    else if (algo == 3)
                                    {
                                        // In Greedy, it's Unit Step Cost.  No need to update anything. But don't want to revisit it.
                                        // Greedy this node is already claimed
                                        // EX:
                                        //                R
                                        //             A    B
                                        //            C D  C E
                                        // Remove B's child C
                                        otherChildNodes.Remove(n);
                                    }
                                    else if (algo == 4)
                                    {
                                        // A* needs to check if a node has a cheaper total path cost.  
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

                        }
                        // Remove this node from the Frontier
                        nextOptions.Remove(currentNode);

                        // Remove visited childNodes as repeatable options.
                        // Is this needed?  Does it hurt?
                        foreach (Node n in visitedNodes)
                        {
                            if (otherChildNodes.Contains(n))
                            {
                                otherChildNodes.Remove(n);
                            }
                        }

                        // Add new children to the frontier
                        // Update any recalculated otherChildNodes into the nextOption list.
                        foreach (Node n in otherChildNodes)
                        {
                            if (nextOptions.Contains(n))
                            {
                                nextOptions.Remove(n);
                            }

                            nextOptions.Add(n);
                        }

                        //return false;
                        //findGreedyPath(nextOptions, goalStateNode, mazeBoard, visitedNodes, finalPathOfNodes, otherChildNodes, refreshDelayMS);

                    }

                }
                else
                {
                    // visited already, remove this for Frontier
                    // don't return to get out of routine.  Move on to next available from our sortedList (Frontier)
                    sortList.Remove(currentNode);  // don't want to keep checking this in a loop.  Needs to be removed from sortList for next iteration.
                    nextOptions.Remove(currentNode);  // may not be needed as this section is primarily for moving on to other siblings within this depth.  nextOptions has no impact here.

                    //if (nextOptions != null)
                    //{
                    //    sortList = nextOptions.OrderBy(o => o.f).ToList();
                    //    findGreedyPath(sortList, goalStateNode, mazeBoard, visitedNodes, finalPathOfNodes, otherChildNodes, refreshDelayMS);
                    //}
                    //else
                    //{
                    //    return false;
                    //}
                }
            }
            return false;

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
            while (nextOptions.Count > 0)
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
            }
            return false;
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
            Console.WriteLine("Maze Board:");
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

