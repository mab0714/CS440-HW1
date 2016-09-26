using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace Rubiks
{
    class Program
    {
        static void Main(string[] args)
        {
            // **********************************************************
            // Developed by : Marvin Biscocho
            // Class: CS440 Artificial Intelligence
            // File: Rubiks Cube (2x2x2) Solver
            // Usage: Rubiks <full path to startStateFile> <full path to goalStateFile> <use rotationalInvariance>
            //        EX: Rubiks cube1_1.txt cube_goal.txt true
            // **********************************************************
            // VARIABLES:
            string startStateFile;
            string goalStateFile;
            bool rotationalInvariance = false;
            bool createPatternDB = false;
            int userDefinedDepth = 0;
            DateTime startPatternDB = DateTime.Now;
            DateTime endPatternDB = DateTime.Now;

            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);

            int maxDepth = 0;
            if (args[0] != null)
            {
                startStateFile = args[0];
            }
            else
            {
                startStateFile = "I:\\Backup\\Masters\\UIUC\\2016\\Fall\\CS_440\\Homework\\1\\cube2_3.txt";
            }

            if (args[1] != null)
            {
                goalStateFile = args[1];
            }
            else
            {
                goalStateFile = "I:\\Backup\\Masters\\UIUC\\2016\\Fall\\CS_440\\Homework\\1\\cube_goal.txt";
            }

            string algorithm = "";
            int valueAlg = 0;
            bool keepAsking = true;
            while (keepAsking)
            {
                Console.WriteLine("Navigating through: " + startStateFile);

                Console.WriteLine("What algorithm do you want to run? (1-2)");
                Console.WriteLine("1 Iterative Deepening A* with No Rotational Invariance");
                Console.WriteLine("2 Iterative Deepening A* with Rotational Invariance");
                Console.WriteLine("3 A* with Recursion");
                
                algorithm = Console.ReadLine(); // Read string from console
                if (int.TryParse(algorithm, out valueAlg)) // Try to parse the string as an integer
                {
                    if (valueAlg < 0 && valueAlg > 3)
                    {
                        Console.WriteLine("Please enter value between 1 and 3!");
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

            if (valueAlg == 1 || valueAlg == 3)
            {
                rotationalInvariance = false;
            }
            else
            {
                rotationalInvariance = true;
                string upperBound = "";
                int valueUB = 0;
                keepAsking = true;
                while (keepAsking)
                {
                    Console.WriteLine("Do you want to create a patternDB, which finds the upper bound on depth?");
                    Console.WriteLine("1 Yes");
                    Console.WriteLine("2 No");

                    upperBound = Console.ReadLine(); // Read string from console
                    if (int.TryParse(upperBound, out valueUB)) // Try to parse the string as an integer
                    {
                        if (valueUB > 2)
                        {
                            Console.WriteLine("Please enter value between 1 and 2!");
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
                
                if (valueUB == 1)
                {
                    createPatternDB = true;
                }
                else
                {
                    createPatternDB = false;
                }
            }

            if (valueAlg != 3)
            {
            string depth = "";
            int valueDepth = 0;
            keepAsking = true;
            while (keepAsking)
            {
                Console.WriteLine("What is the max depth desired for the search? (Note, the higher the number, the longer the search)");

                depth = Console.ReadLine(); // Read string from console
                if (int.TryParse(depth, out valueDepth)) // Try to parse the string as an integer
                {
                    if (valueDepth < 1)
                    {
                        Console.WriteLine("Please enter value > 1!");
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

            userDefinedDepth = valueDepth;

            }

            string heuristic = "";
            int valueDivideHeuristic = 0;
            keepAsking = true;
            while (keepAsking)
            {
                Console.WriteLine("Divide the heuristic? If so, by how much? (IE: 4, max of 24)");
                Console.WriteLine("The higher the number may result in a slower search, but perhaps more optimal!");

                heuristic = Console.ReadLine(); // Read string from console
                if (int.TryParse(heuristic, out valueDivideHeuristic)) // Try to parse the string as an integer
                {
                    if (valueDivideHeuristic > 24)
                    {
                        Console.WriteLine("Please enter value below 24!");
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

            // StartState 
            List<List<char>> startState = new List<List<char>>();

            // GoalState (without rotationalInvariance)
            List<List<char>> goalState = new List<List<char>>();

            // List of all nodes which led to goalState
            List<Node> pathToGoalState = new List<Node>();

            // List of childNodes that are on the Frontier.
            List<Node> otherChildNodes = new List<Node>();

            // List of goalStates (with rotationalInvariance)
            List<Node> goalStateNodes = new List<Node>();
            List<List<char>> tmpGoalState = new List<List<char>>();

            // List of visitedNodes without rotationalInvariance
            List<Node> visitedNodes = new List<Node>();

            // Hash of visitedNodes with rotationalInvariance (can be used with/without rotationalInvariance)
            ConcurrentDictionary<int, List<Node>> visitedNodesHash = new ConcurrentDictionary<int, List<Node>>();

            // Create upper bound by creating pattern database
            Dictionary<String, int> patternDB = new Dictionary<String, int>();
            List<String> visitedStates = new List<String>();

            // flag indicating if solution was found
            bool found = false;

            // Delay for display purposes
            int refreshDelayMS = 1;

            // **********************************************************
            // Load the startState
            string[] lines = System.IO.File.ReadAllLines(startStateFile);

            List<char> sublist;
            // Display the file contents by using a foreach loop.
            foreach (string line in lines)
            {
                if (line.Equals(""))
                {
                    continue;
                }
                sublist = new List<char>();
                foreach (char c in line.ToCharArray())
                {
                    sublist.Add(c);
                    //j++;
                }

                startState.Add(sublist);
            }

            // **********************************************************
            // Load the startState
            lines = System.IO.File.ReadAllLines(goalStateFile);

            // Display the file contents by using a foreach loop.
            foreach (string line in lines)
            {
                //i++;
                if (line.Equals(""))
                {
                    continue;
                }
                sublist = new List<char>();
                foreach (char c in line.ToCharArray())
                {
                    sublist.Add(c);
                }



                goalState.Add(sublist);
            }

            Node startStateNode = new Node(startState, null);
            Node tmpGoalStateNode = new Node(startStateNode.CopyState(), null);
            Node endGoalNode = new Node(tmpGoalStateNode.myState, null);

            // **********************************************************
            // Determine goalState based on rotationalInvariance
            if (rotationalInvariance)
            {
                populateGoalStateNodes(goalStateNodes, goalState);
                // Start State 
                // Pick a goal state node.
                // 1. Start by picking a corner to optimize.  Choose Top, Left, Back 
                // 2. In 2D, that is:
                //    a. (2,0) (left face, top row, in the back)...this color determines (2,1)
                //    b. (4,0) (top face, top row, in the back)...this color determines (4,1) and (6,0)
                //    c. (4,7) (back face, top row, on the left side)...this color determines (6,7)

                // Create idealGoalState                
                tmpGoalStateNode.myState[0][0] = ' ';
                tmpGoalStateNode.myState[0][1] = ' ';
                tmpGoalStateNode.myState[0][2] = startState[0][2];
                tmpGoalStateNode.myState[0][3] = ' ';
                tmpGoalStateNode.myState[0][4] = startState[0][4];
                tmpGoalStateNode.myState[0][5] = ' ';
                tmpGoalStateNode.myState[0][6] = startState[0][4];
                tmpGoalStateNode.myState[0][7] = ' ';
                tmpGoalStateNode.myState[0][8] = ' ';
                tmpGoalStateNode.myState[0][9] = ' ';
                tmpGoalStateNode.myState[0][10] = ' ';
                tmpGoalStateNode.myState[1][0] = ' ';
                tmpGoalStateNode.myState[1][1] = ' ';
                tmpGoalStateNode.myState[1][2] = startState[0][2];
                tmpGoalStateNode.myState[1][3] = ' ';
                tmpGoalStateNode.myState[1][4] = startState[0][4];
                tmpGoalStateNode.myState[1][5] = ' ';
                tmpGoalStateNode.myState[1][6] = ' ';
                tmpGoalStateNode.myState[1][7] = ' ';
                tmpGoalStateNode.myState[1][8] = ' ';
                tmpGoalStateNode.myState[1][9] = ' ';
                tmpGoalStateNode.myState[1][10] = ' ';
                tmpGoalStateNode.myState[2][0] = ' ';
                tmpGoalStateNode.myState[2][1] = ' ';
                tmpGoalStateNode.myState[2][2] = ' ';
                tmpGoalStateNode.myState[2][3] = ' ';
                tmpGoalStateNode.myState[2][4] = ' ';
                tmpGoalStateNode.myState[2][5] = ' ';
                tmpGoalStateNode.myState[2][6] = ' ';
                tmpGoalStateNode.myState[3][0] = ' ';
                tmpGoalStateNode.myState[3][1] = ' ';
                tmpGoalStateNode.myState[3][2] = ' ';
                tmpGoalStateNode.myState[3][3] = ' ';
                tmpGoalStateNode.myState[3][4] = ' ';
                tmpGoalStateNode.myState[3][5] = ' ';
                tmpGoalStateNode.myState[3][6] = ' ';
                tmpGoalStateNode.myState[4][0] = ' ';
                tmpGoalStateNode.myState[4][1] = ' ';
                tmpGoalStateNode.myState[4][2] = ' ';
                tmpGoalStateNode.myState[4][3] = ' ';
                tmpGoalStateNode.myState[4][4] = ' ';
                tmpGoalStateNode.myState[4][5] = ' ';
                tmpGoalStateNode.myState[4][6] = ' ';
                tmpGoalStateNode.myState[5][0] = ' ';
                tmpGoalStateNode.myState[5][1] = ' ';
                tmpGoalStateNode.myState[5][2] = ' ';
                tmpGoalStateNode.myState[5][3] = ' ';
                tmpGoalStateNode.myState[5][4] = ' ';
                tmpGoalStateNode.myState[5][5] = ' ';
                tmpGoalStateNode.myState[5][6] = ' ';
                tmpGoalStateNode.myState[6][0] = ' ';
                tmpGoalStateNode.myState[6][1] = ' ';
                tmpGoalStateNode.myState[6][2] = ' ';
                tmpGoalStateNode.myState[6][3] = ' ';
                tmpGoalStateNode.myState[6][4] = ' ';
                tmpGoalStateNode.myState[6][5] = ' ';
                tmpGoalStateNode.myState[6][6] = ' ';
                tmpGoalStateNode.myState[7][0] = ' ';
                tmpGoalStateNode.myState[7][1] = ' ';
                tmpGoalStateNode.myState[7][2] = ' ';
                tmpGoalStateNode.myState[7][3] = ' ';
                tmpGoalStateNode.myState[7][4] = startState[7][4];
                tmpGoalStateNode.myState[7][5] = ' ';
                tmpGoalStateNode.myState[7][6] = startState[7][4];

                int bestFitGoalStateNode = 0;
                foreach (Node n in goalStateNodes)
                {
                    if (tmpGoalStateNode.Matches(n) > bestFitGoalStateNode)
                    {
                        bestFitGoalStateNode = tmpGoalStateNode.Matches(n);
                        endGoalNode.myState = n.myState;
                    }
                }
            }
            else
            {
                endGoalNode.myState = goalState;
            }

            endGoalNode.Move = "End";

            startStateNode.Move = "Start";
            startStateNode.g = 0;
            startStateNode.h = (int)Math.Ceiling((24.0 - (Double)startStateNode.Matches(endGoalNode)) / (double) valueDivideHeuristic);
            startStateNode.f = startStateNode.g + startStateNode.h;
            startStateNode.goalStateNode = endGoalNode;

            // Log start of search
            DateTime start = DateTime.Now;

            // **********************************************************
            // Determine searching method based on rotationalInvariance
            // A*
            List<Node> currentGeneration = new List<Node>();
            currentGeneration.Add(startStateNode);
            bool maxDepthHit = false;
            if (!rotationalInvariance) {                 
                goalStateNodes.Clear();
                goalStateNodes.Add(endGoalNode);
                if (valueAlg == 3)
                {
                    found = findAPath(currentGeneration, endGoalNode, visitedNodes, pathToGoalState, otherChildNodes, refreshDelayMS, valueDivideHeuristic);
                }
                else
                {
                    visitedNodesHash.TryAdd(startStateNode.Matches(endGoalNode), new List<Node>());

                    // Log start of search
                    start = DateTime.Now;
                    Console.WriteLine("Search started: " + start);
                    int f = 0;
                    maxDepth = userDefinedDepth;
                    while (!found)
                    {
                        //if (d > maxDepth)
                        if (maxDepthHit)
                        {
                            break;
                        }

                        found = findAPathHash(currentGeneration, endGoalNode, visitedNodesHash, pathToGoalState, otherChildNodes, refreshDelayMS, f, patternDB, valueDivideHeuristic, maxDepth, out maxDepthHit);
                        f++;
                    }
                }
            }
            else { 
            
                //visitedNodesHash.Add(startStateNode.CornerMatches(endGoalNode), new List<Node>());
                visitedNodesHash.TryAdd(startStateNode.Matches(endGoalNode), new List<Node>());

                int d = 0;
                if (createPatternDB)
                {
                    // Create lookup patternDatabase
                    // State, Matches...many permutations
                    // 1 move state, matches (12 total)
                    // 2 move state, matches (12*12 total)
                    // 3 move state, matches (12*12*12 total)
                    Node tmpNode = new Node(startState, null);

                    List<List<List<char>>> tmpList = new List<List<List<char>>>();
                    List<List<List<char>>> tmpChildList = new List<List<List<char>>>();
                    List<List<char>> tmpStartState = CopyState(startState);

                    tmpList.Add(tmpStartState);

                    d = 0;
                    int i = 0;
                    bool foundMaxDepth = false;
                    maxDepth = userDefinedDepth;
                    startPatternDB = DateTime.Now;
                    Console.WriteLine("PatternDB started: " + startPatternDB);
                    while (d <= userDefinedDepth)
                    {
                        for (i = 0; i < tmpList.Count; i++)
                        {
                            // Add to patternDB
                            Console.WriteLine("Pattern Database DB depth: " + d + ", size: " + patternDB.Count());

                            String tmpString = "";
                            foreach (List<char> l in tmpList[i])
                            {
                                foreach (char c in l)
                                {
                                    tmpString += c;
                                }
                            }
                            if (!visitedStates.Contains(tmpString))
                            {
                                visitedStates.Add(tmpString);
                                patternDB.Add(tmpString, Matches(tmpList[i], endGoalNode.myState));
                            }
                            else
                            {
                                continue;
                            }
                                                   
                            // Simulate all 12 moves, Add each in a tmpList
                            foreach (List<List<char>> l in findEligibleChildrenStatic(tmpList[i]))
                            {
                                // Don't loop through children if it brings us to a previously visited state in patternDB
                                tmpString = "";
                                foreach (List<char> tmpL in l)
                                {
                                    foreach (char c in tmpL)
                                    {
                                        tmpString += c;
                                    }
                                }
                                if (!visitedStates.Contains(tmpString))
                                {
                                    tmpChildList.Add(l);
                                }
                            }
                            //tmpChildList.AddRange(findEligibleChildrenStatic(tmpList[i]));

                            // Remove any children that cause the parent to be created again, do I have the parent?
                            // Remove if it's in teh patternDB already?

                            //foreach (List<List<char>> l in tmpChildList)
                            //{
                            //    if (patternDB.ContainsKey(l))
                            //    {
                            //        tmpChildList.Remove(l);
                            //    }
                            //}

                            tmpChildList.Distinct<List<List<char>>>().ToString();

                            if (Matches(tmpList[i], endGoalNode.myState) == 24)
                            {
                                maxDepth = d;
                                foundMaxDepth = true;
                                break;
                            }
                        }

                        if (foundMaxDepth)
                        {
                            break;
                        }


                        //foreach (List<List<char>> n in tmpList)
                        //{
                        //    // Add to patternDB
                        //    Console.WriteLine("Pattern Database DB depth: " + d + ", size: " + patternDB.Count());

                        //    if (d > 12)
                        //    {
                        //        patternDB.Add(n, Matches(n, goalState));
                        //    }
                        //    // Simulate all 12 moves, Add each in a tmpList
                        //    tmpChildList.AddRange(findEligibleChildrenStatic(n));

                        //    if (Matches(n, goalState) == 24)
                        //    {
                        //        maxDepth = d;
                        //        break;
                        //    }
                        //}

                        // Remove states that were already added to patternDB
                        tmpList.Clear();

                        // After investigating current level, add childNodes for next iteration
                        tmpList.AddRange(tmpChildList);

                        // Remove childlist
                        tmpChildList.Clear();

                        d++;
                    }
                    endPatternDB = DateTime.Now;
                }
                else
                {
                    maxDepth = userDefinedDepth;
                }
                                        
                // Log start of search
                start = DateTime.Now;
                Console.WriteLine("Search started: " + start);
                
                // this makes it A*, not IDA
                int f = 0;
                while (!found)
                {
                    //if (d > maxDepth)
                    if (maxDepthHit)
                    {
                        break;
                    }
     
                    found = findAPathHash(currentGeneration, endGoalNode, visitedNodesHash, pathToGoalState, otherChildNodes, refreshDelayMS, f, patternDB, valueDivideHeuristic, maxDepth, out maxDepthHit);
                    f++;

                }

            }

            // Log end of search
            DateTime end = DateTime.Now;

            if (found)
            {
                // Display the finalPath backwards
                pathToGoalState.Reverse();
                List<String> moves = new List<String>();

                Console.WriteLine("****************************************************************");
                Console.WriteLine("GOAL FOUND WITHIN " + userDefinedDepth + " SEQUENTIAL MOVES"); 
                Console.WriteLine("Summary for " + startStateFile);
                if (rotationalInvariance)
                {
                    Console.WriteLine("Results are for: Rotational Invariance");
                }
                else
                {
                    Console.WriteLine("Results are for: Non Rotational Invariance");
                }
                
                if (createPatternDB)
                {
                    Console.WriteLine("PatternDB created: " + (endPatternDB - startPatternDB));
                    Console.WriteLine("# of moves to goalState is: " + maxDepth);
                }
                Console.WriteLine("User defined maxDepth: " + userDefinedDepth);
                Console.WriteLine("Search Started: " + start);
                Console.WriteLine("Search Ended: " + end);
                Console.WriteLine("Duration: " + (end - start));
                Console.WriteLine("Divided Heuristic by: " + valueDivideHeuristic);
                if (valueAlg == 3)
                {
                    int nodesVisited = visitedNodes.Count();
                    Console.WriteLine("Nodes visited: " + nodesVisited);
                }
                else
                {
                int nodesVisited = 0;
                foreach (KeyValuePair<int, List<Node>> pair in visitedNodesHash)
                {
                    nodesVisited = nodesVisited + pair.Value.Count();
                }
                
                Console.WriteLine("Nodes visited: " + nodesVisited);
                }
                
                Console.WriteLine("Nodes in final path: " + pathToGoalState.Count());
                Console.WriteLine("Cost of final path: " + pathToGoalState[pathToGoalState.Count - 1].f);
                foreach (Node n in pathToGoalState)
                {
                    n.showNodeInfo();                    
                    moves.Add(n.Move);
                    Console.WriteLine("Show next step! Press any key to continue");
                    Console.ReadKey();
                }
                Console.Write("Final Moves: ");
                foreach(String m in moves)
                {
                    Console.Write(m + " ");
                }
                Console.WriteLine();                         
                Console.WriteLine("****************************************************************");

            }
            else
            {

                Console.WriteLine("****************************************************************");
                Console.WriteLine("NO GOAL FOUND WITHIN " + userDefinedDepth + " SEQUENTIAL MOVES");
                Console.WriteLine("Summary for " + startStateFile);
                if (rotationalInvariance)
                {
                    Console.WriteLine("Results are for: Rotational Invariance");
                }
                else
                {
                    Console.WriteLine("Results are for: Non Rotational Invariance");
                }
                if (createPatternDB)
                {
                    Console.WriteLine("PatternDB created: " + (endPatternDB - startPatternDB));
                    Console.WriteLine("# of moves to goalState is: " + maxDepth);
                }
                
                Console.WriteLine("User defined maxDepth: " + userDefinedDepth);
                Console.WriteLine("Search Started: " + start);
                Console.WriteLine("Search Ended: " + end);
                Console.WriteLine("Duration: " + (end - start));
                Console.WriteLine("Divided Heuristic by: " + valueDivideHeuristic);
                if (valueAlg == 3)
                {
                    int nodesVisited = visitedNodes.Count();
                    Console.WriteLine("Nodes visited: " + nodesVisited);
                }
                else
                {
                int nodesVisited = 0;
                    foreach (KeyValuePair<int, List<Node>> pair in visitedNodesHash)
                    {
                        nodesVisited = nodesVisited + pair.Value.Count();
                    }
                
                Console.WriteLine("Nodes visited: " + nodesVisited);
                }
                Console.WriteLine("****************************************************************");
            }

            //Console.WriteLine("Press anykey to quit");
            //Console.ReadKey();
            do
            {
                Console.WriteLine("Press q to quit");
            } while (Console.ReadKey().KeyChar != 'q');
            
        }

        static bool findAPath(List<Node> nextOptions, Node goalStateNode, List<Node> visitedNodes, List<Node> finalPathOfNodes, List<Node> otherChildNodes, int refreshDelayMS, int valueDivideHeuristic)
        {
            // HERE
            Node currentNode = new Node(nextOptions[0].myState, null);
            Node nextNode = new Node(goalStateNode.myState, null);

            // Query for lowest cost, use lowest heuristic as tiebreaker?
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

                Thread.Sleep(refreshDelayMS);
                Console.Clear();                

                if (currentNode.Equals(goalStateNode))
                {
                    currentNode.Move = "End";
                    finalPathOfNodes.Clear();
                    finalPathOfNodes.Add(currentNode);
                    Node tmpNode = new Node(currentNode.myState, currentNode.parentNode);
                    while (tmpNode.parentNode != null)
                    {
                        //mazeBoard[tmpNode.y][tmpNode.x] = '.';
                        nextNode = tmpNode.parentNode;
                        finalPathOfNodes.Add(nextNode);
                        tmpNode = nextNode;
                    }
                    //mazeBoard[tmpNode.y][tmpNode.x] = 'P';
                    Thread.Sleep(refreshDelayMS);
                    Console.Clear();

                    return true;
                }
                else
                {
                    if (currentNode.childNodes == null)
                    {
                        currentNode.divideHeuristic = valueDivideHeuristic;
                        currentNode.childNodes = currentNode.findEligibleChildrenOrig();
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

                    findAPath(nextOptions, goalStateNode, visitedNodes, finalPathOfNodes, otherChildNodes, refreshDelayMS, valueDivideHeuristic);

                }

            }
            else
            {
                nextOptions.Remove(currentNode);
                if (nextOptions != null)
                {
                    //sortList = nextOptions.OrderBy(o => o.f).ToList();
                    findAPath(nextOptions, goalStateNode, visitedNodes, finalPathOfNodes, otherChildNodes, refreshDelayMS, valueDivideHeuristic);
                }
                else
                {
                    return false;
                }
            }

            return true;

        }

        static bool findAPathHash(List<Node> nextOptions, Node goalStateNode, ConcurrentDictionary<int, List<Node>> visitedNodesDict, List<Node> finalPathOfNodes, List<Node> otherChildNodes, int refreshDelayMS, int cost, Dictionary<String, int> patternDB, int valueDivideHeuristic, int maxDepth, out bool maxDepthHit)
        //static bool findAPathHash(List<Node> nextOptions, List<Node> goalStateNodes, Dictionary<int, List<Node>> visitedNodesDict, List<Node> finalPathOfNodes, List<Node> otherChildNodes, int refreshDelayMS, int maxDepth)
        {
            Node currentNode = new Node(nextOptions[0].myState, null);
            //Node nextNode = new Node(goalStateNodes[0].myState, null);
            Node nextNode = new Node(goalStateNode.myState, null);
            List<Node> sortList;
            List<Node> minList;
            List<Node> maxGList;
            maxDepthHit = false;
            // TODO:  Sometimes I get caught picking nodes that exceed maxDepth while there 
            // instead of returning on line 824, I should remove from nextoptions and continue
            // AND visited nodes not working too

            while (nextOptions.Count > 0)
            {

                // VERY SLOW. KEEP RESORTING EVERYTIME
                // Query for lowest cost, use lowest heuristic as tiebreaker?
                //sortList = nextOptions.OrderBy(o => o.f).ThenBy(m => m.h).ToList();

                // Only select items at this level.  The visitedNodes will prevent them from being reselected in future iterations.
                int maxG = nextOptions.Max(entry => entry.g);
                maxGList = nextOptions.Where(entry => entry.g <= maxDepth).ToList<Node>();
                
                if (maxGList.Count == 0)
                {
                    maxDepthHit = true;
                    return false;
                }

                int minF = maxGList.Min(entry => entry.f);
                minList = maxGList.Where(entry => entry.f == minF).ToList<Node>();

                //int minF = nextOptions.Min(entry => entry.f);
                //minList = nextOptions.Where(entry => entry.f == minF).ToList<Node>();

                // Tie breaker?
                int minH = minList.Min(entry => entry.h);
                minList = minList.Where(entry => entry.h == minH).ToList<Node>();

                sortList = minList;

                // Get the best option of the nextOptions.
                // If there is no other options, no solution found.
                if (sortList[0] != null)
                {
                    currentNode = sortList[0];
                }
                else
                {
                    maxDepthHit = true;
                    return false;
                }

                if (currentNode.f > cost)
                {
                    return false;
                }

                // Check the Dictionary/Hash if node has been visited.
                // Dictionary Structure
                // Key          Value
                // <NumMatches, ListOfNodesWithThatNumMatches>
                // IE:  
                // <24, {N1, N2, N3}> means that N1, N2, N3 are nodes with states that have 24 matches of a goal state.
                List<Node> visitedNodesByMatch;
                currentNode.showNodeInfo();
                if (visitedNodesDict.TryGetValue(currentNode.Matches(currentNode.goalStateNode), out visitedNodesByMatch)) 
                {
                    ;
                }
                else
                {
                    visitedNodesDict[currentNode.Matches(currentNode.goalStateNode)] = new List<Node>();
                }

                //List<Node> L;
                //try
                //{
                //    visitedNodesDict.TryGetValue(currentNode.Matches(currentNode.goalStateNode), out L);
                //}
                //catch
                //{
                //    continue;
                //}

                if (!visitedNodesDict[currentNode.Matches(currentNode.goalStateNode)].Contains(currentNode))
                {

                    visitedNodesDict[currentNode.Matches(currentNode.goalStateNode)].Add(currentNode);
                    //visitedNodes.Add(currentNode);

                    Thread.Sleep(refreshDelayMS);
                    Console.Clear();
                    Console.WriteLine("Investigating Cost: " + currentNode.f);

                    //foreach (Node goalStateNode in goalStateNodes)
                    //{
                        if (currentNode.Equals(goalStateNode))
                        {
                            //currentNode.Move = "End";
                            finalPathOfNodes.Clear();
                            finalPathOfNodes.Add(currentNode);
                            Node tmpNode = new Node(currentNode.myState, currentNode.parentNode);
                            while (tmpNode.parentNode != null)
                            {
                                //mazeBoard[tmpNode.y][tmpNode.x] = '.';
                                nextNode = tmpNode.parentNode;
                                finalPathOfNodes.Add(nextNode);
                                tmpNode = nextNode;
                            }
                            //mazeBoard[tmpNode.y][tmpNode.x] = 'P';
                            Thread.Sleep(refreshDelayMS);
                            Console.Clear();

                            return true;
                        }
                        else
                        {
                            if (currentNode.childNodes == null)
                            {
                                currentNode.divideHeuristic = valueDivideHeuristic;
                                currentNode.childNodes = currentNode.findEligibleChildren(patternDB);
                            }

                            if (currentNode.childNodes != null && currentNode.childNodes.Count > 0)
                            {
                                // Mark childNodes as being already a child to some other parent.
                                foreach (Node n in currentNode.childNodes)
                                {
                                    // add check to make sure we only add childNodes that don't exceed our maxDepth
                                    if (n.g <= maxDepth)
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
                                                try
                                                {
                                                    if (visitedNodesDict[n.Matches(n.goalStateNode)].Contains(n))
                                                    {

                                                        //if (visitedNodes.Contains(n))
                                                        {
                                                            visitedNodesDict[currentNode.Matches(currentNode.goalStateNode)].Remove(n);
                                                            //visitedNodes.Remove(n);
                                                        }
                                                    }
                                                }
                                                catch
                                                {
                                                    continue;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }

                            }
                            nextOptions.Remove(currentNode);

                        // Remove visited childNodes as repeatable options.
                        // Do I need this?
                        //foreach (KeyValuePair<int, List<Node>> pair in visitedNodesDict)
                        //{
                        //    foreach (Node n in pair.Value)
                        //    {
                        //        if (otherChildNodes.Contains(n))
                        //        {
                        //            otherChildNodes.Remove(n);
                        //        }
                        //    }
                        //}

                        // Remove any childNodes from the list if it's already visited
                        for (int x = 0; x < otherChildNodes.Count; x++)
                        //foreach (Node n in otherChildNodes)
                        {
                            if (visitedNodesDict.ContainsKey(otherChildNodes[x].Matches(goalStateNode)))
                            {
                                List<Node> tmpList = visitedNodesDict[otherChildNodes[x].Matches(goalStateNode)];
                                if (tmpList.Contains(otherChildNodes[x]))
                                {
                                    otherChildNodes.Remove(otherChildNodes[x]);
                                }
                            }
                        }

                        // Update any recalculated otherChildNodes into the nextOption list.
                        foreach (Node n in otherChildNodes)
                            {
                                if (nextOptions.Contains(n))
                                {
                                    nextOptions.Remove(n);
                                }

                                //if (n.g <= maxDepth)
                                //{
                                    nextOptions.Add(n);
                                //}                            
                            }

                            //return false;
                            //findAPathHash(nextOptions, goalStateNodes, visitedNodes, finalPathOfNodes, otherChildNodes, refreshDelayMS, maxDepth);

                        }
                    //}

                }
                else
                {
                     nextOptions.Remove(currentNode);
                    //if (nextOptions != null)
                    //{
                    //    //sortList = nextOptions.OrderBy(o => o.f).ToList();
                    //    //findAPathHash(nextOptions, goalStateNodes, visitedNodes, finalPathOfNodes, otherChildNodes, refreshDelayMS, maxDepth);
                    //    //return false;
                    //}
                    //else
                    //{
                    //    //return false;
                    //}
                }
            }
            
            return false;
        }

        //static Node getClosestNode(List<Node> childNodes, Node goalStateNode)
        //{
        //    int z = -1;
        //    int temp_z = 0;
        //    Node closestNode = goalStateNode;
        //    foreach (Node n in childNodes)
        //    {
        //        temp_z = calcManhattanDistance(n, goalStateNode);
        //        if (temp_z < z || z == -1)
        //        {
        //            z = temp_z;
        //            closestNode = n;
        //        }
        //    }
        //    return closestNode;
        //}

        //static int calcManhattanDistance(Node n, Node goalStateNode)
        //{
        //    int z = 0;
        //    int xdelta = 0;
        //    int ydelta = 0;

        //    xdelta = Math.Abs(n.x - goalStateNode.x);
        //    ydelta = Math.Abs(n.y - goalStateNode.y);

        //    z = xdelta + ydelta;
        //    return z;
        //}

        static void populateGoalStateNodes(List<Node> goalStateNodes, List<List<char>> currentGoalState)
        {
            // List of possible goalStateNodes
            Node tmpNode = new Node(currentGoalState, null);
            List<List<char>> tmpList = new List<List<char>>();

            // Use current face as initial goal state (Front)
            // Add the default one given
            goalStateNodes.Add(new Node(currentGoalState, null));
            //Console.WriteLine("Node 1: Front Face");
            //goalStateNodes[0].showNodeInfo();

            // Rotate current goal state clockwise 4 times to get 3 2D goal states (3)
            tmpList = tmpNode.RotateFaceCW();
            tmpNode.myState = tmpList;
            goalStateNodes.Add(new Node(tmpList, null));
            //Console.WriteLine("Node 2: Front Face");
            //goalStateNodes[1].showNodeInfo();

            tmpNode.myState = tmpList;
            tmpList = tmpNode.RotateFaceCW();
            goalStateNodes.Add(new Node(tmpList, null));
            //Console.WriteLine("Node 3: Front Face");
            //goalStateNodes[2].showNodeInfo();

            tmpNode.myState = tmpList;
            tmpList = tmpNode.RotateFaceCW();
            goalStateNodes.Add(new Node(tmpList, null));
            //Console.WriteLine("Node 4: Front Face");
            //goalStateNodes[3].showNodeInfo();

            // We got 4 goal states now, rotate to get to original
            tmpNode.myState = tmpList;
            tmpList = tmpNode.RotateFaceCW();
            tmpNode.myState = tmpList;

            // Original Left is now Face, add to list
            tmpList = tmpNode.RotateFaceRight();
            goalStateNodes.Add(new Node(tmpList, null));
            tmpNode.myState = tmpList;
            //Console.WriteLine("Node 5: Left Face");
            //goalStateNodes[4].showNodeInfo();

            // Rotate current goal state clockwise 4 times to get 3 2D goal states (3)
            tmpList = tmpNode.RotateFaceCW();
            tmpNode.myState = tmpList;
            goalStateNodes.Add(new Node(tmpList, null));
            //Console.WriteLine("Node 6: Left Face");
            //goalStateNodes[5].showNodeInfo();

            tmpNode.myState = tmpList;
            tmpList = tmpNode.RotateFaceCW();
            goalStateNodes.Add(new Node(tmpList, null));
            //Console.WriteLine("Node 7: Left Face");
            //goalStateNodes[6].showNodeInfo();

            tmpNode.myState = tmpList;
            tmpList = tmpNode.RotateFaceCW();
            goalStateNodes.Add(new Node(tmpList, null));
            //Console.WriteLine("Node 8: Left Face");
            //goalStateNodes[7].showNodeInfo();

            // We got 8 goal states now, rotate to get to (Left side is face)
            tmpNode.myState = tmpList;
            tmpList = tmpNode.RotateFaceCW();
            tmpNode.myState = tmpList;

            // Original Top is now Face, add to list
            tmpList = tmpNode.RotateFaceDown();
            goalStateNodes.Add(new Node(tmpList, null));
            tmpNode.myState = tmpList;
            //Console.WriteLine("Node 9: Top Face");
            //goalStateNodes[8].showNodeInfo();

            // Rotate current goal state clockwise 4 times to get 3 2D goal states (3)
            tmpList = tmpNode.RotateFaceCW();
            tmpNode.myState = tmpList;
            goalStateNodes.Add(new Node(tmpList, null));
            //Console.WriteLine("Node 10: Top Face");
            //goalStateNodes[9].showNodeInfo();

            tmpNode.myState = tmpList;
            tmpList = tmpNode.RotateFaceCW();
            goalStateNodes.Add(new Node(tmpList, null));
            //Console.WriteLine("Node 11: Top Face");
            //goalStateNodes[10].showNodeInfo();

            tmpNode.myState = tmpList;
            tmpList = tmpNode.RotateFaceCW();
            goalStateNodes.Add(new Node(tmpList, null));
            //Console.WriteLine("Node 12: Top Face");
            //goalStateNodes[11].showNodeInfo();

            // We got 12 goal states now, rotate to get to (Left side is face)
            tmpNode.myState = tmpList;
            tmpList = tmpNode.RotateFaceCW();
            tmpNode.myState = tmpList;

            // Original Back is now Face, add to list
            tmpList = tmpNode.RotateFaceRight();
            goalStateNodes.Add(new Node(tmpList, null));
            tmpNode.myState = tmpList;
            //Console.WriteLine("Node 13: Back Face");
            //goalStateNodes[12].showNodeInfo();

            // Rotate current goal state clockwise 4 times to get 3 2D goal states (3)
            tmpList = tmpNode.RotateFaceCW();
            tmpNode.myState = tmpList;
            goalStateNodes.Add(new Node(tmpList, null));
            //Console.WriteLine("Node 14: Back Face");
            //goalStateNodes[13].showNodeInfo();

            tmpNode.myState = tmpList;
            tmpList = tmpNode.RotateFaceCW();
            goalStateNodes.Add(new Node(tmpList, null));
            //Console.WriteLine("Node 15: Back Face");
            //goalStateNodes[14].showNodeInfo();

            tmpNode.myState = tmpList;
            tmpList = tmpNode.RotateFaceCW();
            goalStateNodes.Add(new Node(tmpList, null));
            //Console.WriteLine("Node 16: Back Face");
            //goalStateNodes[15].showNodeInfo();

            // We got 16 goal states now, rotate to get to (Back side is face)
            tmpNode.myState = tmpList;
            tmpList = tmpNode.RotateFaceCW();
            tmpNode.myState = tmpList;

            // Original Right is now Face, add to list
            tmpList = tmpNode.RotateFaceDown();
            goalStateNodes.Add(new Node(tmpList, null));
            //Console.WriteLine("Node 17: Right Face");
            //goalStateNodes[16].showNodeInfo();

            // Rotate current goal state clockwise 4 times to get 3 2D goal states (3)
            tmpNode.myState = tmpList;
            tmpList = tmpNode.RotateFaceCW();
            tmpNode.myState = tmpList;
            goalStateNodes.Add(new Node(tmpList, null));
            //Console.WriteLine("Node 18: Right Face");
            //goalStateNodes[17].showNodeInfo();

            tmpNode.myState = tmpList;
            tmpList = tmpNode.RotateFaceCW();
            goalStateNodes.Add(new Node(tmpList, null));
            //Console.WriteLine("Node 19: Right Face");
            //goalStateNodes[18].showNodeInfo();

            tmpNode.myState = tmpList;
            tmpList = tmpNode.RotateFaceCW();
            goalStateNodes.Add(new Node(tmpList, null));
            //Console.WriteLine("Node 20: Right Face");
            //goalStateNodes[19].showNodeInfo();

            // We got 20 goal states now, rotate to get to (Left side is face)
            tmpNode.myState = tmpList;
            tmpList = tmpNode.RotateFaceCW();
            tmpNode.myState = tmpList;

            // Original Bottom is now Face, add to list
            tmpList = tmpNode.RotateFaceRight();
            goalStateNodes.Add(new Node(tmpList, null));
            tmpNode.myState = tmpList;
            //Console.WriteLine("Node 21: Bottom Face");
            //goalStateNodes[20].showNodeInfo();

            // Rotate current goal state clockwise 4 times to get 3 2D goal states (3)
            tmpList = tmpNode.RotateFaceCW();
            tmpNode.myState = tmpList;
            goalStateNodes.Add(new Node(tmpList, null));
            //Console.WriteLine("Node 22: Bottom Face");
            //goalStateNodes[21].showNodeInfo();

            tmpNode.myState = tmpList;
            tmpList = tmpNode.RotateFaceCW();
            goalStateNodes.Add(new Node(tmpList, null));
            //Console.WriteLine("Node 23: Bottom Face");
            //goalStateNodes[22].showNodeInfo();

            tmpNode.myState = tmpList;
            tmpList = tmpNode.RotateFaceCW();
            goalStateNodes.Add(new Node(tmpList, null));
            //Console.WriteLine("Node 24: Bottom Face");
            //goalStateNodes[23].showNodeInfo();

            // We got 24 goal states now, rotate to get to (Back side is face)
            tmpNode.myState = tmpList;
            tmpList = tmpNode.RotateFaceCW();
            tmpNode.myState = tmpList;

            //foreach (Node n in goalStateNodes)
            //{
            //    n.showNodeInfo();
            //}
            

            // Change Top to Face for another set of 2D goal states
            // Repeat rotation of new goal state clockwise 4 times for more 2D goal states (4)

            // Change Back to Face for another set of 2D goal states
            // Repeat rotation of new goal state clockwise 4 times for more 2D goal states (4)

            // Change Bottom to Face for another set of 2D goal states
            // Repeat rotation of new goal state clockwise 4 times for more 2D goal states (4)

            // Change Left to Face for another set of 2D goal states
            // Repeat rotation of new goal state clockwise 4 times for more 2D goal states (4)

            // Change Right to Face for another set of 2D goal states
            // Repeat rotation of new goal state clockwise 4 times for more 2D goal states (4)
        }

        public static List<List<char>> CopyState(List<List<char>> myState)
        {
            List<List<char>> tmpList = new List<List<char>>();
            List<char> tmpCharList;
            foreach (List<char> l in myState)
            {
                tmpCharList = new List<char>();
                foreach (char c in l)
                {
                    tmpCharList.Add(c);
                }
                tmpList.Add(tmpCharList);
            }
            tmpCharList = null;
            return tmpList;
        }

        public static int Matches(List<List<char>> current, List<List<char>> goal)
        {
            int i = 0;
            int j = 0;
            int cnt = 0;

            foreach (List<char> l in current)
            {
                j = 0;
                foreach (char c in l)
                {
                    if (current[i][j] == goal[i][j] && !current.ToString().Trim().Equals("") && !goal[i][j].ToString().Trim().Equals(""))
                    {
                        cnt++;
                    }
                    j++;
                }
                //Console.WriteLine();
                i++;
            }
            return cnt;
        }

        public static List<List<List<char>>> findEligibleChildrenStatic(List<List<char>> myState)
        {
            List<List<char>> newState = new List<List<char>>();
            List<List<List<char>>> listStates = new List<List<List<char>>>();

            newState = CopyState(myState);

            // Check Movements
            // 1. Top ClockWise, denoted T
            // all 4 top cells shift
            newState[0][4] = myState[1][4];
            newState[1][4] = myState[1][6];
            newState[1][6] = myState[0][6];
            newState[0][6] = myState[0][4];
            // top 2 cells in the front change
            newState[2][4] = myState[1][8];
            newState[2][6] = myState[0][8];
            // top 2 cells in the right change
            newState[1][8] = myState[7][6];
            newState[0][8] = myState[7][4];
            // top 2 cells in the back change
            newState[7][6] = myState[0][2];
            newState[7][4] = myState[1][2];
            // top 2 cells in the left change
            newState[0][2] = myState[2][4];
            newState[1][2] = myState[2][6];

            listStates.Add(newState);

            newState = CopyState(myState);
            // 2. Top CounterClockWise, denoted T'
            // all 4 top cells shift
            newState[0][4] = myState[0][6];
            newState[1][4] = myState[0][4];
            newState[1][6] = myState[1][4];
            newState[0][6] = myState[1][6];
            // top 2 cells in the front change
            newState[2][4] = myState[0][2];
            newState[2][6] = myState[1][2];
            // top 2 cells in the right change
            newState[1][8] = myState[2][4];
            newState[0][8] = myState[2][6];
            // top 2 cells in the back change
            newState[7][6] = myState[1][8];
            newState[7][4] = myState[0][8];
            // top 2 cells in the left change
            newState[0][2] = myState[7][6];
            newState[1][2] = myState[7][4];

            listStates.Add(newState);

            newState = CopyState(myState);
            // 3. Bottom ClockWise, denoted Bo
            // all 4 bottom cells shift
            newState[4][4] = myState[4][6];
            newState[4][6] = myState[5][6];
            newState[5][6] = myState[5][4];
            newState[5][4] = myState[4][4];
            // bottom 2 cells in the front change
            newState[3][4] = myState[1][10];
            newState[3][6] = myState[0][10];
            // bottom 2 cells in the right change
            newState[1][10] = myState[6][6];
            newState[0][10] = myState[6][4];
            // bottom 2 cells in the back change
            newState[6][6] = myState[0][0];
            newState[6][4] = myState[1][0];
            // bottom 2 cells in the left change
            newState[0][0] = myState[3][4];
            newState[1][0] = myState[3][6];

            listStates.Add(newState);

            newState = CopyState(myState);
            // 4. Bottom CounterClockWise, denoted Bo'
            // all 4 bottom cells shift
            newState[4][4] = myState[5][4];
            newState[4][6] = myState[4][4];
            newState[5][6] = myState[4][6];
            newState[5][4] = myState[5][6];
            // bottom 2 cells in the front change
            newState[3][4] = myState[0][0];
            newState[3][6] = myState[1][0];
            // bottom 2 cells in the right change
            newState[1][10] = myState[3][4];
            newState[0][10] = myState[3][6];
            // bottom 2 cells in the back change
            newState[6][6] = myState[1][10];
            newState[6][4] = myState[0][10];
            // bottom 2 cells in the left change
            newState[0][0] = myState[6][6];
            newState[1][0] = myState[6][4];

            listStates.Add(newState);

            newState = CopyState(myState);
            // 5. Front ClockWise, denoted F
            // all 4 front cells shift
            newState[2][4] = myState[3][4];
            newState[3][4] = myState[3][6];
            newState[3][6] = myState[2][6];
            newState[2][6] = myState[2][4];
            // front 2 cells in the top change
            newState[1][4] = myState[1][0];
            newState[1][6] = myState[1][2];
            // front 2 cells in the left change
            newState[1][0] = myState[4][6];
            newState[1][2] = myState[4][4];
            // front 2 cells in the bottom change
            newState[4][6] = myState[1][8];
            newState[4][4] = myState[1][10];
            // front 2 cells in the right change
            newState[1][8] = myState[1][4];
            newState[1][10] = myState[1][6];

            listStates.Add(newState);

            newState = CopyState(myState);
            // 6. Front CounterClockWise, denoted F'
            // all 4 front cells shift
            newState[2][4] = myState[2][6];
            newState[3][4] = myState[2][4];
            newState[3][6] = myState[3][4];
            newState[2][6] = myState[3][6];
            // front 2 cells in the top change
            newState[1][4] = myState[1][8];
            newState[1][6] = myState[1][10];
            // front 2 cells in the left change
            newState[1][0] = myState[1][4];
            newState[1][2] = myState[1][6];
            // front 2 cells in the bottom change
            newState[4][6] = myState[1][0];
            newState[4][4] = myState[1][2];
            // front 2 cells in the right change
            newState[1][8] = myState[4][6];
            newState[1][10] = myState[4][4];

            listStates.Add(newState);

            newState = CopyState(myState);

            // 7. Back ClockWise, denoted Ba
            // all 4 back cells shift
            newState[6][4] = myState[6][6];
            newState[6][6] = myState[7][6];
            newState[7][6] = myState[7][4];
            newState[7][4] = myState[6][4];
            // back 2 cells in the top change
            newState[0][4] = myState[0][0];
            newState[0][6] = myState[0][2];
            // back 2 cells in the left change
            newState[0][0] = myState[5][6];
            newState[0][2] = myState[5][4];
            // back 2 cells in the bottom change
            newState[5][6] = myState[0][8];
            newState[5][4] = myState[0][10];
            // back 2 cells in the right change
            newState[0][8] = myState[0][4];
            newState[0][10] = myState[0][6];

            listStates.Add(newState);

            newState = CopyState(myState);
            // 8. Back CounterClockWise, denoted Ba'
            // all 4 back cells shift
            newState[6][4] = myState[7][4];
            newState[6][6] = myState[6][4];
            newState[7][6] = myState[6][6];
            newState[7][4] = myState[7][6];
            // back 2 cells in the top change
            newState[0][4] = myState[0][8];
            newState[0][6] = myState[0][10];
            // back 2 cells in the left change
            newState[0][0] = myState[0][4];
            newState[0][2] = myState[0][6];
            // back 2 cells in the bottom change
            newState[5][6] = myState[0][0];
            newState[5][4] = myState[0][2];
            // back 2 cells in the right change
            newState[0][8] = myState[5][6];
            newState[0][10] = myState[5][4];

            listStates.Add(newState);

            newState = CopyState(myState);
            // 9. Left ClockWise, denoted L
            // all 4 left cells shift
            newState[0][0] = myState[1][0];
            newState[0][2] = myState[0][0];
            newState[1][0] = myState[1][2];
            newState[1][2] = myState[0][2];
            // left 2 cells in the top change
            newState[0][4] = myState[6][4];
            newState[1][4] = myState[7][4];
            // left 2 cells in the front change
            newState[2][4] = myState[0][4];
            newState[3][4] = myState[1][4];
            // left 2 cells in the bottom change
            newState[4][4] = myState[2][4];
            newState[5][4] = myState[3][4];
            // left 2 cells in the back change
            newState[6][4] = myState[4][4];
            newState[7][4] = myState[5][4];

            listStates.Add(newState);

            newState = CopyState(myState);
            // 10. Left CounterClockWise, denoted L'
            // all 4 left cells shift
            newState[0][0] = myState[0][2];
            newState[0][2] = myState[1][2];
            newState[1][0] = myState[0][0];
            newState[1][2] = myState[1][0];
            // left 2 cells in the top change
            newState[0][4] = myState[2][4];
            newState[1][4] = myState[3][4];
            // left 2 cells in the front change
            newState[2][4] = myState[4][4];
            newState[3][4] = myState[5][4];
            // left 2 cells in the bottom change
            newState[4][4] = myState[6][4];
            newState[5][4] = myState[7][4];
            // left 2 cells in the back change
            newState[6][4] = myState[0][4];
            newState[7][4] = myState[1][4];

            listStates.Add(newState);

            newState = CopyState(myState);
            // 11. Right ClockWise, denoted R
            // all 4 right cells shift
            newState[0][8] = myState[0][10];
            newState[0][10] = myState[1][10];
            newState[1][8] = myState[0][8];
            newState[1][10] = myState[1][8];
            // right 2 cells in the top change
            newState[0][6] = myState[6][6];
            newState[1][6] = myState[7][6];
            // right 2 cells in the front change
            newState[2][6] = myState[0][6];
            newState[3][6] = myState[1][6];
            // right 2 cells in the bottom change
            newState[4][6] = myState[2][6];
            newState[5][6] = myState[3][6];
            // right 2 cells in the back change
            newState[6][6] = myState[4][6];
            newState[7][6] = myState[5][6];

            listStates.Add(newState);

            newState = CopyState(myState);
            // 12. Right CounterClockWise, denoted R'
            // all 4 right cells shift
            newState[0][8] = myState[1][8];
            newState[0][10] = myState[0][8];
            newState[1][8] = myState[1][10];
            newState[1][10] = myState[0][10];
            // right 2 cells in the top change
            newState[0][6] = myState[2][6];
            newState[1][6] = myState[3][6];
            // right 2 cells in the front change
            newState[2][6] = myState[4][6];
            newState[3][6] = myState[5][6];
            // right 2 cells in the bottom change
            newState[4][6] = myState[6][6];
            newState[5][6] = myState[7][6];
            // right 2 cells in the back change
            newState[6][6] = myState[0][6];
            newState[7][6] = myState[1][6];

            listStates.Add(newState);

            return listStates;

        }

    }
}
