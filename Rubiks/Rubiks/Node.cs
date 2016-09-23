using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Rubiks
{
    public class Node : IEquatable<Node>
    {
        // Move to arrive to state
        string _move = "";
        // Start and Finish States
        List<List<char>> _myState;
        List<List<char>> _goalState;

        // Heuristic divider
        private int _valueDivideHeuristic = 1;

        // Cost variables
        private int _h;
        private int _g;
        private int _f;

        // parentNode is the startState with 0 moves
        private Node _parentNode;

        // childNodes are the possible states after movement
        private List<Node> _childNodes;

        // goalStateNode is the last move, which results in goalState
        private Node _goalStateNode;

        public bool _isInitialized;

        //public Node(int x, int y, Node parentNode)
        //{
        //    this._x = x;
        //    this._y = y;
        //    this._parentNode = parentNode;
        //    this._isInitialized = true;
        //}                 

        public Node(List<List<char>> myState, Node parentNode)
        {
            this._myState = myState;
            this._parentNode = parentNode;
            this._goalStateNode = goalStateNode;
            this._isInitialized = true;
        }

        public List<List<char>> myState
        {
            get { return this._myState; }
            set { this._myState = value; }
        }

        public int divideHeuristic
        {
            get { return this._valueDivideHeuristic; }
            set { this._valueDivideHeuristic = value; }
        }
        public List<List<char>> goalState
        {
            get { return this._goalState; }
            set { this._goalState = value; }
        }

        public int g
        {
            get { return this._g; }
            set { this._g = value; }
        }
        public int h
        {
            get { return this._h; }
            set { this._h = value; }
        }

        public int f
        {
            get { return this._f; }
            set { this._f = value; }
        }

        public string Move
        {
            get { return this._move; }
            set { this._move = value; }
        }

        public Node parentNode
        {
            get { return this._parentNode; }
            set { this._parentNode = value; }

        }

        public Node goalStateNode
        {
            get { return this._goalStateNode; }
            set { this._goalStateNode = value; }

        }
        public List<Node> childNodes
        {
            get { return this._childNodes; }
            set { this._childNodes = value; }

        }
        public bool Equals(Node n)
        {
         
            string myState = "";
            string tmpState = "";

            foreach (List<char> l in this._myState)
            {
                foreach (char c in l)
                {
                    myState += c;
                }                
            }
            foreach (List<char> l in n.myState)
            {
                foreach (char c in l)
                {
                    tmpState += c;
                }
            }

            if (myState.Equals(tmpState))
            {
                myState = null;
                tmpState = null;
                return true;
            }
            else
            {
                myState = null;
                tmpState = null;
                return false;
            }
        }

        public String myStateString()
        {
            string temp = "";
            foreach (List<char> l in this._myState)
            {
                foreach (char c in l)
                {
                    temp += c;
                }
            }
            return temp;
        }

        // How many matches does the current state have with goalState.  
        // Don't need input if always comparing goalState, but for flexibility, leaving goalState as input.
        public int Matches (Node n)
        {
            int i = 0;
            int j = 0;
            int cnt = 0;

            foreach (List<char> l in this.myState)
            {
                j = 0;
                foreach (char c in l)
                {
                    //Console.Write(this.myState[i][j] + " " + n.myState[i][j]);
                    if (this.myState[i][j] == n.myState[i][j]  && !this.myState[i][j].ToString().Trim().Equals("") && !n.myState[i][j].ToString().Trim().Equals(""))
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

        public int CornerMatches(Node n)
        {            
            int cnt = 0;

            // LeftMatches            
            if (this.myState[0][0] == n.myState[0][0])
            {
                cnt++;
            }
            if (this.myState[0][2] == n.myState[0][2])
            {
                cnt++;
            }
            if (this.myState[1][0] == n.myState[1][0])
            {
                cnt++;
            }
            if (this.myState[1][2] == n.myState[1][2])
            {
                cnt++;
            }
            // Top Matches
            if (this.myState[0][4] == n.myState[0][4])
            {
                cnt++;
            }
            if (this.myState[0][6] == n.myState[0][6])
            {
                cnt++;
            }
            if (this.myState[1][4] == n.myState[1][4])
            {
                cnt++;
            }
            if (this.myState[1][6] == n.myState[1][6])
            {
                cnt++;
            }
            // Top Matches
            if (this.myState[6][4] == n.myState[6][4])
            {
                cnt++;
            }
            if (this.myState[6][6] == n.myState[6][6])
            {
                cnt++;
            }
            if (this.myState[7][4] == n.myState[7][4])
            {
                cnt++;
            }
            if (this.myState[7][6] == n.myState[7][6])
            {
                cnt++;
            }
            return cnt;
        }



        //private int calcManhattanDistance(int x, int y)
        //{
        //    int z = 0;
        //    int xdelta = 0;
        //    int ydelta = 0;

        //    xdelta = Math.Abs(x - this._goalStateNode.x);
        //    ydelta = Math.Abs(y - this._goalStateNode.y);

        //    z = xdelta + ydelta;
        //    return z;
        //}

        public List<List<char>> RotateFaceCW()
        {
            // get a copy of the current state
            List<List<char>> newState = new List<List<char>>();
            newState = CopyState();
            // Check Movements
            // 1. Top shifts into Right
            newState[0][8] = this.myState[0][4];
            newState[0][10] = this.myState[0][6];
            newState[1][8] = this.myState[1][4];
            newState[1][10] = this.myState[1][6];
            // 2. Right shifts into Bottom
            newState[5][6] = this.myState[0][8];
            newState[5][4] = this.myState[0][10];
            newState[4][6] = this.myState[1][8];
            newState[4][4] = this.myState[1][10];
            // 3. Bottom shifts into Left
            newState[0][0] = this.myState[5][6];
            newState[0][2] = this.myState[5][4];
            newState[1][0] = this.myState[4][6];
            newState[1][2] = this.myState[4][4];
            // 4. Left shifts into Top
            newState[0][4] = this.myState[0][0];
            newState[0][6] = this.myState[0][2];
            newState[1][4] = this.myState[1][0];
            newState[1][6] = this.myState[1][2];
            // 5. Shift face tiles
            newState[2][4] = this.myState[3][4];
            newState[2][6] = this.myState[2][4];
            newState[3][4] = this.myState[2][6];
            newState[3][4] = this.myState[3][6];
            // 6. Shift back tiles
            newState[6][4] = this.myState[6][6];
            newState[7][4] = this.myState[6][4];
            newState[7][6] = this.myState[7][4];
            newState[6][6] = this.myState[7][6];

            return newState;

        }

        public List<List<char>> RotateFaceRight()
        {
            // get a copy of the current state
            List<List<char>> newState = new List<List<char>>();
            newState = CopyState();
            // Check Movements
            // 1. Face shifts into Right
            newState[1][8] = this.myState[2][4];
            newState[0][8] = this.myState[2][6];
            newState[1][10] = this.myState[3][4];
            newState[0][10] = this.myState[3][6];
            // 2. Right shifts into Back
            newState[7][6] = this.myState[1][8];
            newState[7][4] = this.myState[0][8];
            newState[6][6] = this.myState[1][10];
            newState[6][4] = this.myState[0][10];
            // 3. Back shifts into Left
            newState[0][2] = this.myState[7][6];
            newState[1][2] = this.myState[7][4];
            newState[0][0] = this.myState[6][6];
            newState[1][0] = this.myState[6][4];
            // 4. Left shifts into Face
            newState[2][4] = this.myState[0][2];
            newState[2][6] = this.myState[1][2];
            newState[3][4] = this.myState[0][0];
            newState[3][6] = this.myState[1][0];
            // 5. Shift top tiles
            newState[1][6] = this.myState[1][4];
            newState[0][6] = this.myState[1][6];
            newState[0][4] = this.myState[0][6];
            newState[1][4] = this.myState[0][4];
            // 6. Shift bottom tiles
            newState[4][6] = this.myState[4][4];
            newState[5][6] = this.myState[4][6];
            newState[5][4] = this.myState[5][6];
            newState[4][4] = this.myState[5][4];

            return newState;

        }

        public List<List<char>> RotateFaceDown()
        {
            // get a copy of the current state
            List<List<char>> newState = new List<List<char>>();
            newState = CopyState();
            // Check Movements
            // 1. Face shifts into Bottom
            newState[4][4] = this.myState[2][4];
            newState[4][6] = this.myState[2][6];
            newState[5][4] = this.myState[3][4];
            newState[5][6] = this.myState[3][6];
            // 2. Bottom shifts into Back
            newState[6][4] = this.myState[4][4];
            newState[6][6] = this.myState[4][6];
            newState[7][4] = this.myState[5][4];
            newState[7][6] = this.myState[5][6];
            // 3. Back shifts into Top
            newState[0][4] = this.myState[6][4];
            newState[0][6] = this.myState[6][6];
            newState[1][4] = this.myState[7][4];
            newState[1][6] = this.myState[7][6];
            // 4. Top shifts into Face
            newState[2][4] = this.myState[0][4];
            newState[2][6] = this.myState[0][6];
            newState[3][4] = this.myState[1][4];
            newState[3][6] = this.myState[1][6];
            // 5. Shift left tiles
            newState[1][2] = this.myState[0][2];
            newState[1][0] = this.myState[1][2];
            newState[0][0] = this.myState[1][0];
            newState[0][2] = this.myState[0][0];
            // 6. Shift right tiles
            newState[1][8] = this.myState[0][8];
            newState[1][10] = this.myState[1][8];
            newState[0][10] = this.myState[1][10];
            newState[0][8] = this.myState[0][10];

            return newState;

        }

        public List<List<char>> CopyState()
        {
            List<List<char>> tmpList = new List<List<char>>();
            List<char> tmpCharList;
            foreach (List<char> l in this._myState)
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

        public List<Node> findEligibleChildren()
        {
            Node tmpNode;
            // initialize child nodes
            if (_childNodes == null)
            {
                _childNodes = new List<Node>();
            }

            // get a copy of the current state
            List<List<char>> newState = CopyState();

            // Check Movements
            // 1. Top ClockWise, denoted T
            // all 4 top cells shift
            newState[0][4] = _myState[1][4];
            newState[1][4] = _myState[1][6];
            newState[1][6] = _myState[0][6];
            newState[0][6] = _myState[0][4];
            // top 2 cells in the front change
            newState[2][4] = _myState[1][8];
            newState[2][6] = _myState[0][8];
            // top 2 cells in the right change
            newState[1][8] = _myState[7][6];
            newState[0][8] = _myState[7][4];
            // top 2 cells in the back change
            newState[7][6] = _myState[0][2];
            newState[7][4] = _myState[1][2];
            // top 2 cells in the left change
            newState[0][2] = _myState[2][4];
            newState[1][2] = _myState[2][6];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "T";
            tmpNode.g = tmpNode._parentNode.g + 1;
            //tmpNode.h = 24 - patternDB[myState];
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 2. Top CounterClockWise, denoted T'
            // all 4 top cells shift
            newState[0][4] = _myState[0][6];
            newState[1][4] = _myState[0][4];
            newState[1][6] = _myState[1][4];
            newState[0][6] = _myState[1][6];
            // top 2 cells in the front change
            newState[2][4] = _myState[0][2];
            newState[2][6] = _myState[1][2];
            // top 2 cells in the right change
            newState[1][8] = _myState[2][4];
            newState[0][8] = _myState[2][6];
            // top 2 cells in the back change
            newState[7][6] = _myState[1][8];
            newState[7][4] = _myState[0][8];
            // top 2 cells in the left change
            newState[0][2] = _myState[7][6];
            newState[1][2] = _myState[7][4];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "T'";
            tmpNode.g = tmpNode._parentNode.g + 1;
            //tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
            //tmpNode.h = 24 - patternDB[myState];
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 3. Bottom ClockWise, denoted Bo
            // all 4 bottom cells shift
            newState[4][4] = _myState[4][6];
            newState[4][6] = _myState[5][6];
            newState[5][6] = _myState[5][4];
            newState[5][4] = _myState[4][4];
            // bottom 2 cells in the front change
            newState[3][4] = _myState[1][10];
            newState[3][6] = _myState[0][10];
            // bottom 2 cells in the right change
            newState[1][10] = _myState[6][6];
            newState[0][10] = _myState[6][4];
            // bottom 2 cells in the back change
            newState[6][6] = _myState[0][0];
            newState[6][4] = _myState[1][0];
            // bottom 2 cells in the left change
            newState[0][0] = _myState[3][4];
            newState[1][0] = _myState[3][6];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "Bo";
            tmpNode.g = tmpNode._parentNode.g + 1;
            //tmpNode.h = 24 - patternDB[myState];
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 4. Bottom CounterClockWise, denoted Bo'
            // all 4 bottom cells shift
            newState[4][4] = _myState[5][4];
            newState[4][6] = _myState[4][4];
            newState[5][6] = _myState[4][6];
            newState[5][4] = _myState[5][6];
            // bottom 2 cells in the front change
            newState[3][4] = _myState[0][0];
            newState[3][6] = _myState[1][0];
            // bottom 2 cells in the right change
            newState[1][10] = _myState[3][4];
            newState[0][10] = _myState[3][6];
            // bottom 2 cells in the back change
            newState[6][6] = _myState[1][10];
            newState[6][4] = _myState[0][10];
            // bottom 2 cells in the left change
            newState[0][0] = _myState[6][6];
            newState[1][0] = _myState[6][4];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "Bo'";
            tmpNode.g = tmpNode._parentNode.g + 1;
            //tmpNode.h = 24 - patternDB[myState];
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 5. Front ClockWise, denoted F
            // all 4 front cells shift
            newState[2][4] = _myState[3][4];
            newState[3][4] = _myState[3][6];
            newState[3][6] = _myState[2][6];
            newState[2][6] = _myState[2][4];
            // front 2 cells in the top change
            newState[1][4] = _myState[1][0];
            newState[1][6] = _myState[1][2];
            // front 2 cells in the left change
            newState[1][0] = _myState[4][6];
            newState[1][2] = _myState[4][4];
            // front 2 cells in the bottom change
            newState[4][6] = _myState[1][8];
            newState[4][4] = _myState[1][10];
            // front 2 cells in the right change
            newState[1][8] = _myState[1][4];
            newState[1][10] = _myState[1][6];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "F";
            tmpNode.g = tmpNode._parentNode.g + 1;
            //tmpNode.h = 24 - patternDB[myState];
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 6. Front CounterClockWise, denoted F'
            // all 4 front cells shift
            newState[2][4] = _myState[2][6];
            newState[3][4] = _myState[2][4];
            newState[3][6] = _myState[3][4];
            newState[2][6] = _myState[3][6];
            // front 2 cells in the top change
            newState[1][4] = _myState[1][8];
            newState[1][6] = _myState[1][10];
            // front 2 cells in the left change
            newState[1][0] = _myState[1][4];
            newState[1][2] = _myState[1][6];
            // front 2 cells in the bottom change
            newState[4][6] = _myState[1][0];
            newState[4][4] = _myState[1][2];
            // front 2 cells in the right change
            newState[1][8] = _myState[4][6];
            newState[1][10] = _myState[4][4];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "F'";
            tmpNode.g = tmpNode._parentNode.g + 1;
            //tmpNode.h = 24 - patternDB[myState];
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 7. Back ClockWise, denoted Ba
            // all 4 back cells shift
            newState[6][4] = _myState[6][6];
            newState[6][6] = _myState[7][6];
            newState[7][6] = _myState[7][4];
            newState[7][4] = _myState[6][4];
            // back 2 cells in the top change
            newState[0][4] = _myState[0][0];
            newState[0][6] = _myState[0][2];
            // back 2 cells in the left change
            newState[0][0] = _myState[5][6];
            newState[0][2] = _myState[5][4];
            // back 2 cells in the bottom change
            newState[5][6] = _myState[0][8];
            newState[5][4] = _myState[0][10];
            // back 2 cells in the right change
            newState[0][8] = _myState[0][4];
            newState[0][10] = _myState[0][6];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "Ba";
            tmpNode.g = tmpNode._parentNode.g + 1;
            //tmpNode.h = 24 - patternDB[myState];
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 8. Back CounterClockWise, denoted Ba'
            // all 4 back cells shift
            newState[6][4] = _myState[7][4];
            newState[6][6] = _myState[6][4];
            newState[7][6] = _myState[6][6];
            newState[7][4] = _myState[7][6];
            // back 2 cells in the top change
            newState[0][4] = _myState[0][8];
            newState[0][6] = _myState[0][10];
            // back 2 cells in the left change
            newState[0][0] = _myState[0][4];
            newState[0][2] = _myState[0][6];
            // back 2 cells in the bottom change
            newState[5][6] = _myState[0][0];
            newState[5][4] = _myState[0][2];
            // back 2 cells in the right change
            newState[0][8] = _myState[5][6];
            newState[0][10] = _myState[5][4];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "Ba'";
            tmpNode.g = tmpNode._parentNode.g + 1;
            //tmpNode.h = 24 - patternDB[myState];
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 9. Left ClockWise, denoted L
            // all 4 left cells shift
            newState[0][0] = _myState[1][0];
            newState[0][2] = _myState[0][0];
            newState[1][0] = _myState[1][2];
            newState[1][2] = _myState[0][2];
            // left 2 cells in the top change
            newState[0][4] = _myState[6][4];
            newState[1][4] = _myState[7][4];
            // left 2 cells in the front change
            newState[2][4] = _myState[0][4];
            newState[3][4] = _myState[1][4];
            // left 2 cells in the bottom change
            newState[4][4] = _myState[2][4];
            newState[5][4] = _myState[3][4];
            // left 2 cells in the back change
            newState[6][4] = _myState[4][4];
            newState[7][4] = _myState[5][4];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "L";
            tmpNode.g = tmpNode._parentNode.g + 1;
            //tmpNode.h = 24 - patternDB[myState];
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 10. Left CounterClockWise, denoted L'
            // all 4 left cells shift
            newState[0][0] = _myState[0][2];
            newState[0][2] = _myState[1][2];
            newState[1][0] = _myState[0][0];
            newState[1][2] = _myState[1][0];
            // left 2 cells in the top change
            newState[0][4] = _myState[2][4];
            newState[1][4] = _myState[3][4];
            // left 2 cells in the front change
            newState[2][4] = _myState[4][4];
            newState[3][4] = _myState[5][4];
            // left 2 cells in the bottom change
            newState[4][4] = _myState[6][4];
            newState[5][4] = _myState[7][4];
            // left 2 cells in the back change
            newState[6][4] = _myState[0][4];
            newState[7][4] = _myState[1][4];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "L'";
            tmpNode.g = tmpNode._parentNode.g + 1;
            //tmpNode.h = 24 - patternDB[myState];
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 11. Right ClockWise, denoted R
            // all 4 right cells shift
            newState[0][8] = _myState[0][10];
            newState[0][10] = _myState[1][10];
            newState[1][8] = _myState[0][8];
            newState[1][10] = _myState[1][8];
            // right 2 cells in the top change
            newState[0][6] = _myState[6][6];
            newState[1][6] = _myState[7][6];
            // right 2 cells in the front change
            newState[2][6] = _myState[0][6];
            newState[3][6] = _myState[1][6];
            // right 2 cells in the bottom change
            newState[4][6] = _myState[2][6];
            newState[5][6] = _myState[3][6];
            // right 2 cells in the back change
            newState[6][6] = _myState[4][6];
            newState[7][6] = _myState[5][6];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "R";
            tmpNode.g = tmpNode._parentNode.g + 1;
            //tmpNode.h = 24 - patternDB[myState];
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 12. Right CounterClockWise, denoted R'
            // all 4 right cells shift
            newState[0][8] = _myState[1][8];
            newState[0][10] = _myState[0][8];
            newState[1][8] = _myState[1][10];
            newState[1][10] = _myState[0][10];
            // right 2 cells in the top change
            newState[0][6] = _myState[2][6];
            newState[1][6] = _myState[3][6];
            // right 2 cells in the front change
            newState[2][6] = _myState[4][6];
            newState[3][6] = _myState[5][6];
            // right 2 cells in the bottom change
            newState[4][6] = _myState[6][6];
            newState[5][6] = _myState[7][6];
            // right 2 cells in the back change
            newState[6][6] = _myState[0][6];
            newState[7][6] = _myState[1][6];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "R'";
            tmpNode.g = tmpNode._parentNode.g + 1;
            //tmpNode.h = 24 - patternDB[myState];
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            if (this._parentNode != null)// && this._childNodes != null)
            {
                _childNodes.Remove(this._parentNode);
            }
            return _childNodes;
        }

        public List<Node> findEligibleChildren(Dictionary<String, int> patternDB)
        {
            Node tmpNode;
            // initialize child nodes
            if (_childNodes == null)
            {
                _childNodes = new List<Node>();
            }

            int h; 

            // get a copy of the current state
            List<List<char>> newState = CopyState();

            // Check Movements
            // 1. Top ClockWise, denoted T
            // all 4 top cells shift
            newState[0][4] = _myState[1][4];
            newState[1][4] = _myState[1][6];
            newState[1][6] = _myState[0][6];
            newState[0][6] = _myState[0][4];
            // top 2 cells in the front change
            newState[2][4] = _myState[1][8];
            newState[2][6] = _myState[0][8];
            // top 2 cells in the right change
            newState[1][8] = _myState[7][6];
            newState[0][8] = _myState[7][4];
            // top 2 cells in the back change
            newState[7][6] = _myState[0][2];
            newState[7][4] = _myState[1][2];
            // top 2 cells in the left change
            newState[0][2] = _myState[2][4];
            newState[1][2] = _myState[2][6];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "T";
            tmpNode.g = tmpNode._parentNode.g + 1;
            if (patternDB.Count > 0)
            {
                try
                {
                    patternDB.TryGetValue(this.myStateString(), out h);
                    tmpNode.h = 24 - h;
                }
                catch
                {
                    tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
                }
            }
            else
            {
                tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
            }
            tmpNode.h = (int)Math.Ceiling((Double)tmpNode.h / (double)_valueDivideHeuristic);
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 2. Top CounterClockWise, denoted T'
            // all 4 top cells shift
            newState[0][4] = _myState[0][6];
            newState[1][4] = _myState[0][4];
            newState[1][6] = _myState[1][4];
            newState[0][6] = _myState[1][6];
            // top 2 cells in the front change
            newState[2][4] = _myState[0][2];
            newState[2][6] = _myState[1][2];
            // top 2 cells in the right change
            newState[1][8] = _myState[2][4];
            newState[0][8] = _myState[2][6];
            // top 2 cells in the back change
            newState[7][6] = _myState[1][8];
            newState[7][4] = _myState[0][8];
            // top 2 cells in the left change
            newState[0][2] = _myState[7][6];
            newState[1][2] = _myState[7][4];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "T'";
            tmpNode.g = tmpNode._parentNode.g + 1;
            //try
            //{
            //    int h = 0;
            //    patternDB.TryGetValue(myState, out h);
            //    tmpNode.h = 24 - h;
            //}
            if (patternDB.Count > 0)
            {
                try
                {
                    patternDB.TryGetValue(this.myStateString(), out h);
                    tmpNode.h = 24 - h;
                }
                catch
                {
                    tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
                }
            }
            else
            {
                tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
            }
            tmpNode.h = (int)Math.Ceiling((Double)tmpNode.h / (double)_valueDivideHeuristic);
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 3. Bottom ClockWise, denoted Bo
            // all 4 bottom cells shift
            newState[4][4] = _myState[4][6];
            newState[4][6] = _myState[5][6];
            newState[5][6] = _myState[5][4];
            newState[5][4] = _myState[4][4];
            // bottom 2 cells in the front change
            newState[3][4] = _myState[1][10];
            newState[3][6] = _myState[0][10];
            // bottom 2 cells in the right change
            newState[1][10] = _myState[6][6];
            newState[0][10] = _myState[6][4];
            // bottom 2 cells in the back change
            newState[6][6] = _myState[0][0];
            newState[6][4] = _myState[1][0];
            // bottom 2 cells in the left change
            newState[0][0] = _myState[3][4];
            newState[1][0] = _myState[3][6];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "Bo";
            tmpNode.g = tmpNode._parentNode.g + 1;
            if (patternDB.Count > 0)
            {
                try
                {
                    patternDB.TryGetValue(this.myStateString(), out h);
                    tmpNode.h = 24 - h;
                }
                catch
                {
                    tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
                }
            }
            else
            {
                tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
            }
            tmpNode.h = (int)Math.Ceiling((Double)tmpNode.h / (double)_valueDivideHeuristic);
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 4. Bottom CounterClockWise, denoted Bo'
            // all 4 bottom cells shift
            newState[4][4] = _myState[5][4];
            newState[4][6] = _myState[4][4];
            newState[5][6] = _myState[4][6];
            newState[5][4] = _myState[5][6];
            // bottom 2 cells in the front change
            newState[3][4] = _myState[0][0];
            newState[3][6] = _myState[1][0];
            // bottom 2 cells in the right change
            newState[1][10] = _myState[3][4];
            newState[0][10] = _myState[3][6];
            // bottom 2 cells in the back change
            newState[6][6] = _myState[1][10];
            newState[6][4] = _myState[0][10];
            // bottom 2 cells in the left change
            newState[0][0] = _myState[6][6];
            newState[1][0] = _myState[6][4];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "Bo'";
            tmpNode.g = tmpNode._parentNode.g + 1;
            if (patternDB.Count > 0)
            {
                try
                {
                    patternDB.TryGetValue(this.myStateString(), out h);
                    tmpNode.h = 24 - h;
                }
                catch
                {
                    tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
                }
            }
            else
            {
                tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
            }
            tmpNode.h = (int)Math.Ceiling((Double)tmpNode.h / (double)_valueDivideHeuristic);
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 5. Front ClockWise, denoted F
            // all 4 front cells shift
            newState[2][4] = _myState[3][4];
            newState[3][4] = _myState[3][6];
            newState[3][6] = _myState[2][6];
            newState[2][6] = _myState[2][4];
            // front 2 cells in the top change
            newState[1][4] = _myState[1][0];
            newState[1][6] = _myState[1][2];
            // front 2 cells in the left change
            newState[1][0] = _myState[4][6];
            newState[1][2] = _myState[4][4];
            // front 2 cells in the bottom change
            newState[4][6] = _myState[1][8];
            newState[4][4] = _myState[1][10];
            // front 2 cells in the right change
            newState[1][8] = _myState[1][4];
            newState[1][10] = _myState[1][6];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "F";
            tmpNode.g = tmpNode._parentNode.g + 1;
            if (patternDB.Count > 0)
            {
                try
                {
                    patternDB.TryGetValue(this.myStateString(), out h);
                    tmpNode.h = 24 - h;
                }
                catch
                {
                    tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
                }
            }
            else
            {
                tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
            }
            tmpNode.h = (int)Math.Ceiling((Double)tmpNode.h / (double)_valueDivideHeuristic);
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 6. Front CounterClockWise, denoted F'
            // all 4 front cells shift
            newState[2][4] = _myState[2][6];
            newState[3][4] = _myState[2][4];
            newState[3][6] = _myState[3][4];
            newState[2][6] = _myState[3][6];
            // front 2 cells in the top change
            newState[1][4] = _myState[1][8];
            newState[1][6] = _myState[1][10];
            // front 2 cells in the left change
            newState[1][0] = _myState[1][4];
            newState[1][2] = _myState[1][6];
            // front 2 cells in the bottom change
            newState[4][6] = _myState[1][0];
            newState[4][4] = _myState[1][2];
            // front 2 cells in the right change
            newState[1][8] = _myState[4][6];
            newState[1][10] = _myState[4][4];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "F'";
            tmpNode.g = tmpNode._parentNode.g + 1;
            if (patternDB.Count > 0)
            {
                try
                {
                    patternDB.TryGetValue(this.myStateString(), out h);
                    tmpNode.h = 24 - h;
                }
                catch
                {
                    tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
                }
            }
            else
            {
                tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
            }
            tmpNode.h = (int)Math.Ceiling((Double)tmpNode.h / (double)_valueDivideHeuristic);
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 7. Back ClockWise, denoted Ba
            // all 4 back cells shift
            newState[6][4] = _myState[6][6];
            newState[6][6] = _myState[7][6];
            newState[7][6] = _myState[7][4];
            newState[7][4] = _myState[6][4];
            // back 2 cells in the top change
            newState[0][4] = _myState[0][0];
            newState[0][6] = _myState[0][2];
            // back 2 cells in the left change
            newState[0][0] = _myState[5][6];
            newState[0][2] = _myState[5][4];
            // back 2 cells in the bottom change
            newState[5][6] = _myState[0][8];
            newState[5][4] = _myState[0][10];
            // back 2 cells in the right change
            newState[0][8] = _myState[0][4];
            newState[0][10] = _myState[0][6];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "Ba";
            tmpNode.g = tmpNode._parentNode.g + 1;
            if (patternDB.Count > 0)
            {
                try
                {
                    patternDB.TryGetValue(this.myStateString(), out h);
                    tmpNode.h = 24 - h;
                }
                catch
                {
                    tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
                }
            }
            else
            {
                tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
            }
            tmpNode.h = (int)Math.Ceiling((Double)tmpNode.h / (double)_valueDivideHeuristic);
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 8. Back CounterClockWise, denoted Ba'
            // all 4 back cells shift
            newState[6][4] = _myState[7][4];
            newState[6][6] = _myState[6][4];
            newState[7][6] = _myState[6][6];
            newState[7][4] = _myState[7][6];
            // back 2 cells in the top change
            newState[0][4] = _myState[0][8];
            newState[0][6] = _myState[0][10];
            // back 2 cells in the left change
            newState[0][0] = _myState[0][4];
            newState[0][2] = _myState[0][6];
            // back 2 cells in the bottom change
            newState[5][6] = _myState[0][0];
            newState[5][4] = _myState[0][2];
            // back 2 cells in the right change
            newState[0][8] = _myState[5][6];
            newState[0][10] = _myState[5][4];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "Ba'";
            tmpNode.g = tmpNode._parentNode.g + 1;
            if (patternDB.Count > 0)
            {
                try
                {
                    patternDB.TryGetValue(this.myStateString(), out h);
                    tmpNode.h = 24 - h;
                }
                catch
                {
                    tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
                }
            }
            else
            {
                tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
            }
            tmpNode.h = (int)Math.Ceiling((Double)tmpNode.h / (double)_valueDivideHeuristic);
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 9. Left ClockWise, denoted L
            // all 4 left cells shift
            newState[0][0] = _myState[1][0];
            newState[0][2] = _myState[0][0];
            newState[1][0] = _myState[1][2];
            newState[1][2] = _myState[0][2];
            // left 2 cells in the top change
            newState[0][4] = _myState[6][4];
            newState[1][4] = _myState[7][4];
            // left 2 cells in the front change
            newState[2][4] = _myState[0][4];
            newState[3][4] = _myState[1][4];
            // left 2 cells in the bottom change
            newState[4][4] = _myState[2][4];
            newState[5][4] = _myState[3][4];
            // left 2 cells in the back change
            newState[6][4] = _myState[4][4];
            newState[7][4] = _myState[5][4];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "L";
            tmpNode.g = tmpNode._parentNode.g + 1;
            if (patternDB.Count > 0)
            {
                try
                {
                    patternDB.TryGetValue(this.myStateString(), out h);
                    tmpNode.h = 24 - h;
                }
                catch
                {
                    tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
                }
            }
            else
            {
                tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
            }
            tmpNode.h = (int)Math.Ceiling((Double)tmpNode.h / (double)_valueDivideHeuristic);
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 10. Left CounterClockWise, denoted L'
            // all 4 left cells shift
            newState[0][0] = _myState[0][2];
            newState[0][2] = _myState[1][2];
            newState[1][0] = _myState[0][0];
            newState[1][2] = _myState[1][0];
            // left 2 cells in the top change
            newState[0][4] = _myState[2][4];
            newState[1][4] = _myState[3][4];
            // left 2 cells in the front change
            newState[2][4] = _myState[4][4];
            newState[3][4] = _myState[5][4];
            // left 2 cells in the bottom change
            newState[4][4] = _myState[6][4];
            newState[5][4] = _myState[7][4];
            // left 2 cells in the back change
            newState[6][4] = _myState[0][4];
            newState[7][4] = _myState[1][4];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "L'";
            tmpNode.g = tmpNode._parentNode.g + 1;
            if (patternDB.Count > 0)
            {
                try
                {
                    patternDB.TryGetValue(this.myStateString(), out h);
                    tmpNode.h = 24 - h;
                }
                catch
                {
                    tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
                }
            }
            else
            {
                tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
            }
            tmpNode.h = (int)Math.Ceiling((Double)tmpNode.h / (double)_valueDivideHeuristic);
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 11. Right ClockWise, denoted R
            // all 4 right cells shift
            newState[0][8] = _myState[0][10];
            newState[0][10] = _myState[1][10];
            newState[1][8] = _myState[0][8];
            newState[1][10] = _myState[1][8];
            // right 2 cells in the top change
            newState[0][6] = _myState[6][6];
            newState[1][6] = _myState[7][6];
            // right 2 cells in the front change
            newState[2][6] = _myState[0][6];
            newState[3][6] = _myState[1][6];
            // right 2 cells in the bottom change
            newState[4][6] = _myState[2][6];
            newState[5][6] = _myState[3][6];
            // right 2 cells in the back change
            newState[6][6] = _myState[4][6];
            newState[7][6] = _myState[5][6];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "R";
            tmpNode.g = tmpNode._parentNode.g + 1;
            if (patternDB.Count > 0)
            {
                try
                {
                    patternDB.TryGetValue(this.myStateString(), out h);
                    tmpNode.h = 24 - h;
                }
                catch
                {
                    tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
                }
            }
            else
            {
                tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
            }
            tmpNode.h = (int)Math.Ceiling((Double)tmpNode.h / (double)_valueDivideHeuristic);
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 12. Right CounterClockWise, denoted R'
            // all 4 right cells shift
            newState[0][8] = _myState[1][8];
            newState[0][10] = _myState[0][8];
            newState[1][8] = _myState[1][10];
            newState[1][10] = _myState[0][10];
            // right 2 cells in the top change
            newState[0][6] = _myState[2][6];
            newState[1][6] = _myState[3][6];
            // right 2 cells in the front change
            newState[2][6] = _myState[4][6];
            newState[3][6] = _myState[5][6];
            // right 2 cells in the bottom change
            newState[4][6] = _myState[6][6];
            newState[5][6] = _myState[7][6];
            // right 2 cells in the back change
            newState[6][6] = _myState[0][6];
            newState[7][6] = _myState[1][6];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "R'";
            tmpNode.g = tmpNode._parentNode.g + 1;
            if (patternDB.Count > 0)
            {
                try
                {
                    //tmpNode.h = 24 - patternDB[myState];
                    patternDB.TryGetValue(this.myStateString(), out h);
                    tmpNode.h = 24 - h;
                }
                catch
                {
                    tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
                }
            }
            else
            {
                tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
            }
            tmpNode.h = (int)Math.Ceiling((Double)tmpNode.h / (double)_valueDivideHeuristic);
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            if (this._parentNode != null)// && this._childNodes != null)
            {
                _childNodes.Remove(this._parentNode);
            }
            return _childNodes;
        }


        public List<Node> findEligibleChildrenOrig()
        {
            Node tmpNode;
            // initialize child nodes
            if (_childNodes == null)
            {
                _childNodes = new List<Node>();
            }

            // get a copy of the current state
            List<List<char>> newState = CopyState();

            // Check Movements
            // 1. Top ClockWise, denoted T
            // all 4 top cells shift
            newState[0][4] = _myState[1][4];
            newState[1][4] = _myState[1][6];
            newState[1][6] = _myState[0][6];
            newState[0][6] = _myState[0][4];
            // top 2 cells in the front change
            newState[2][4] = _myState[1][8];
            newState[2][6] = _myState[0][8];
            // top 2 cells in the right change
            newState[1][8] = _myState[7][6];
            newState[0][8] = _myState[7][4];
            // top 2 cells in the back change
            newState[7][6] = _myState[0][2];
            newState[7][4] = _myState[1][2];
            // top 2 cells in the left change
            newState[0][2] = _myState[2][4];
            newState[1][2] = _myState[2][6];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "T";
            tmpNode.g = tmpNode._parentNode.g + 1;
            tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
            tmpNode.h = (int)Math.Ceiling((Double)tmpNode.h / (double)_valueDivideHeuristic);
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 2. Top CounterClockWise, denoted T'
            // all 4 top cells shift
            newState[0][4] = _myState[0][6];
            newState[1][4] = _myState[0][4];
            newState[1][6] = _myState[1][4];
            newState[0][6] = _myState[1][6];
            // top 2 cells in the front change
            newState[2][4] = _myState[0][2];
            newState[2][6] = _myState[1][2];
            // top 2 cells in the right change
            newState[1][8] = _myState[2][4];
            newState[0][8] = _myState[2][6];
            // top 2 cells in the back change
            newState[7][6] = _myState[1][8];
            newState[7][4] = _myState[0][8];
            // top 2 cells in the left change
            newState[0][2] = _myState[7][6];
            newState[1][2] = _myState[7][4];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "T'";
            tmpNode.g = tmpNode._parentNode.g + 1;
            tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
            tmpNode.h = (int)Math.Ceiling((Double)tmpNode.h / (double)_valueDivideHeuristic);
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 3. Bottom ClockWise, denoted Bo
            // all 4 bottom cells shift
            newState[4][4] = _myState[4][6];
            newState[4][6] = _myState[5][6];
            newState[5][6] = _myState[5][4];
            newState[5][4] = _myState[4][4];
            // bottom 2 cells in the front change
            newState[3][4] = _myState[1][10];
            newState[3][6] = _myState[0][10];
            // bottom 2 cells in the right change
            newState[1][10] = _myState[6][6];
            newState[0][10] = _myState[6][4];
            // bottom 2 cells in the back change
            newState[6][6] = _myState[0][0];
            newState[6][4] = _myState[1][0];
            // bottom 2 cells in the left change
            newState[0][0] = _myState[3][4];
            newState[1][0] = _myState[3][6];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "Bo";
            tmpNode.g = tmpNode._parentNode.g + 1;
            tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
            tmpNode.h = (int)Math.Ceiling((Double)tmpNode.h / (double)_valueDivideHeuristic);
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 4. Bottom CounterClockWise, denoted Bo'
            // all 4 bottom cells shift
            newState[4][4] = _myState[5][4];
            newState[4][6] = _myState[4][4];
            newState[5][6] = _myState[4][6];
            newState[5][4] = _myState[5][6];
            // bottom 2 cells in the front change
            newState[3][4] = _myState[0][0];
            newState[3][6] = _myState[1][0];
            // bottom 2 cells in the right change
            newState[1][10] = _myState[3][4];
            newState[0][10] = _myState[3][6];
            // bottom 2 cells in the back change
            newState[6][6] = _myState[1][10];
            newState[6][4] = _myState[0][10];
            // bottom 2 cells in the left change
            newState[0][0] = _myState[6][6];
            newState[1][0] = _myState[6][4];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "Bo'";
            tmpNode.g = tmpNode._parentNode.g + 1;
            tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
            tmpNode.h = (int)Math.Ceiling((Double)tmpNode.h / (double)_valueDivideHeuristic);
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 5. Front ClockWise, denoted F
            // all 4 front cells shift
            newState[2][4] = _myState[3][4];
            newState[3][4] = _myState[3][6];
            newState[3][6] = _myState[2][6];
            newState[2][6] = _myState[2][4];
            // front 2 cells in the top change
            newState[1][4] = _myState[1][0];
            newState[1][6] = _myState[1][2];
            // front 2 cells in the left change
            newState[1][0] = _myState[4][6];
            newState[1][2] = _myState[4][4];
            // front 2 cells in the bottom change
            newState[4][6] = _myState[1][8];
            newState[4][4] = _myState[1][10];
            // front 2 cells in the right change
            newState[1][8] = _myState[1][4];
            newState[1][10] = _myState[1][6];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "F";
            tmpNode.g = tmpNode._parentNode.g + 1;
            tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
            tmpNode.h = (int)Math.Ceiling((Double)tmpNode.h / (double)_valueDivideHeuristic);
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 6. Front CounterClockWise, denoted F'
            // all 4 front cells shift
            newState[2][4] = _myState[2][6];
            newState[3][4] = _myState[2][4];
            newState[3][6] = _myState[3][4];
            newState[2][6] = _myState[3][6];
            // front 2 cells in the top change
            newState[1][4] = _myState[1][8];
            newState[1][6] = _myState[1][10];
            // front 2 cells in the left change
            newState[1][0] = _myState[1][4];
            newState[1][2] = _myState[1][6];
            // front 2 cells in the bottom change
            newState[4][6] = _myState[1][0];
            newState[4][4] = _myState[1][2];
            // front 2 cells in the right change
            newState[1][8] = _myState[4][6];
            newState[1][10] = _myState[4][4];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "F'";
            tmpNode.g = tmpNode._parentNode.g + 1;
            tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
            tmpNode.h = (int)Math.Ceiling((Double)tmpNode.h / (double)_valueDivideHeuristic);
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 7. Back ClockWise, denoted Ba
            // all 4 back cells shift
            newState[6][4] = _myState[6][6];
            newState[6][6] = _myState[7][6];
            newState[7][6] = _myState[7][4];
            newState[7][4] = _myState[6][4];
            // back 2 cells in the top change
            newState[0][4] = _myState[0][0];
            newState[0][6] = _myState[0][2];
            // back 2 cells in the left change
            newState[0][0] = _myState[5][6];
            newState[0][2] = _myState[5][4];
            // back 2 cells in the bottom change
            newState[5][6] = _myState[0][8];
            newState[5][4] = _myState[0][10];
            // back 2 cells in the right change
            newState[0][8] = _myState[0][4];
            newState[0][10] = _myState[0][6];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "Ba";
            tmpNode.g = tmpNode._parentNode.g + 1;
            tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
            tmpNode.h = (int)Math.Ceiling((Double)tmpNode.h / (double)_valueDivideHeuristic);
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 8. Back CounterClockWise, denoted Ba'
            // all 4 back cells shift
            newState[6][4] = _myState[7][4];
            newState[6][6] = _myState[6][4];
            newState[7][6] = _myState[6][6];
            newState[7][4] = _myState[7][6];
            // back 2 cells in the top change
            newState[0][4] = _myState[0][8];
            newState[0][6] = _myState[0][10];
            // back 2 cells in the left change
            newState[0][0] = _myState[0][4];
            newState[0][2] = _myState[0][6];
            // back 2 cells in the bottom change
            newState[5][6] = _myState[0][0];
            newState[5][4] = _myState[0][2];
            // back 2 cells in the right change
            newState[0][8] = _myState[5][6];
            newState[0][10] = _myState[5][4];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "Ba'";
            tmpNode.g = tmpNode._parentNode.g + 1;
            tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
            tmpNode.h = (int)Math.Ceiling((Double)tmpNode.h / (double)_valueDivideHeuristic);
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 9. Left ClockWise, denoted L
            // all 4 left cells shift
            newState[0][0] = _myState[1][0];
            newState[0][2] = _myState[0][0];
            newState[1][0] = _myState[1][2];
            newState[1][2] = _myState[0][2];
            // left 2 cells in the top change
            newState[0][4] = _myState[6][4];
            newState[1][4] = _myState[7][4];
            // left 2 cells in the front change
            newState[2][4] = _myState[0][4];
            newState[3][4] = _myState[1][4];
            // left 2 cells in the bottom change
            newState[4][4] = _myState[2][4];
            newState[5][4] = _myState[3][4];
            // left 2 cells in the back change
            newState[6][4] = _myState[4][4];
            newState[7][4] = _myState[5][4];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "L";
            tmpNode.g = tmpNode._parentNode.g + 1;
            tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
            tmpNode.h = (int)Math.Ceiling((Double)tmpNode.h / (double)_valueDivideHeuristic);
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 10. Left CounterClockWise, denoted L'
            // all 4 left cells shift
            newState[0][0] = _myState[0][2];
            newState[0][2] = _myState[1][2];
            newState[1][0] = _myState[0][0];
            newState[1][2] = _myState[1][0];
            // left 2 cells in the top change
            newState[0][4] = _myState[2][4];
            newState[1][4] = _myState[3][4];
            // left 2 cells in the front change
            newState[2][4] = _myState[4][4];
            newState[3][4] = _myState[5][4];
            // left 2 cells in the bottom change
            newState[4][4] = _myState[6][4];
            newState[5][4] = _myState[7][4];
            // left 2 cells in the back change
            newState[6][4] = _myState[0][4];
            newState[7][4] = _myState[1][4];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "L'";
            tmpNode.g = tmpNode._parentNode.g + 1;
            tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
            tmpNode.h = (int)Math.Ceiling((Double)tmpNode.h / (double)_valueDivideHeuristic);
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 11. Right ClockWise, denoted R
            // all 4 right cells shift
            newState[0][8] = _myState[0][10];
            newState[0][10] = _myState[1][10];
            newState[1][8] = _myState[0][8];
            newState[1][10] = _myState[1][8];
            // right 2 cells in the top change
            newState[0][6] = _myState[6][6];
            newState[1][6] = _myState[7][6];
            // right 2 cells in the front change
            newState[2][6] = _myState[0][6];
            newState[3][6] = _myState[1][6];
            // right 2 cells in the bottom change
            newState[4][6] = _myState[2][6];
            newState[5][6] = _myState[3][6];
            // right 2 cells in the back change
            newState[6][6] = _myState[4][6];
            newState[7][6] = _myState[5][6];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "R";
            tmpNode.g = tmpNode._parentNode.g + 1;
            tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
            tmpNode.h = (int)Math.Ceiling((Double)tmpNode.h / (double)_valueDivideHeuristic);
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 12. Right CounterClockWise, denoted R'
            // all 4 right cells shift
            newState[0][8] = _myState[1][8];
            newState[0][10] = _myState[0][8];
            newState[1][8] = _myState[1][10];
            newState[1][10] = _myState[0][10];
            // right 2 cells in the top change
            newState[0][6] = _myState[2][6];
            newState[1][6] = _myState[3][6];
            // right 2 cells in the front change
            newState[2][6] = _myState[4][6];
            newState[3][6] = _myState[5][6];
            // right 2 cells in the bottom change
            newState[4][6] = _myState[6][6];
            newState[5][6] = _myState[7][6];
            // right 2 cells in the back change
            newState[6][6] = _myState[0][6];
            newState[7][6] = _myState[1][6];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "R'";
            tmpNode.g = tmpNode._parentNode.g + 1;
            tmpNode.h = (24 - tmpNode.Matches(_goalStateNode));
            tmpNode.h = (int)Math.Ceiling((Double)tmpNode.h / (double)_valueDivideHeuristic);
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            if (this._parentNode != null)// && this._childNodes != null)
            {
                _childNodes.Remove(this._parentNode);
            }
            return _childNodes;
        }

        public List<Node> findEligibleChildren(List<Node> goalStateNodes)
        {
            Node tmpNode;            
            int minH = 2;  // Can't have H > 
            // initialize child nodes
            if (_childNodes == null)
            {
                _childNodes = new List<Node>();
            }
            
            // get a copy of the current state
            List<List<char>> newState = CopyState();

            // Check Movements
            // 1. Top ClockWise, denoted T
            // all 4 top cells shift
            newState[0][4] = _myState[1][4];
            newState[1][4] = _myState[1][6];
            newState[1][6] = _myState[0][6];
            newState[0][6] = _myState[0][4];
            // top 2 cells in the front change
            newState[2][4] = _myState[1][8];
            newState[2][6] = _myState[0][8];
            // top 2 cells in the right change
            newState[1][8] = _myState[7][6];
            newState[0][8] = _myState[7][4];
            // top 2 cells in the back change
            newState[7][6] = _myState[0][2];
            newState[7][4] = _myState[1][2];
            // top 2 cells in the left change
            newState[0][2] = _myState[2][4];
            newState[1][2] = _myState[2][6];

            tmpNode = new Node(newState, this);            
            tmpNode.Move = "T";
            tmpNode.g = tmpNode._parentNode.g + 1;
            tmpNode.h = (12 - tmpNode.CornerMatches(_goalStateNode)) / 12;
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 2. Top CounterClockWise, denoted T'
            // all 4 top cells shift
            newState[0][4] = _myState[0][6];
            newState[1][4] = _myState[0][4];
            newState[1][6] = _myState[1][4];
            newState[0][6] = _myState[1][6];
            // top 2 cells in the front change
            newState[2][4] = _myState[0][2];
            newState[2][6] = _myState[1][2];
            // top 2 cells in the right change
            newState[1][8] = _myState[2][4];
            newState[0][8] = _myState[2][6];
            // top 2 cells in the back change
            newState[7][6] = _myState[1][8];
            newState[7][4] = _myState[0][8];
            // top 2 cells in the left change
            newState[0][2] = _myState[7][6];
            newState[1][2] = _myState[7][4];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "T'";
            tmpNode.g = tmpNode._parentNode.g + 1;
            tmpNode.h = (12 - tmpNode.CornerMatches(_goalStateNode)) / 12;
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 3. Bottom ClockWise, denoted Bo
            // all 4 bottom cells shift
            newState[4][4] = _myState[4][6];
            newState[4][6] = _myState[5][6];
            newState[5][6] = _myState[5][4];
            newState[5][4] = _myState[4][4];
            // bottom 2 cells in the front change
            newState[3][4] = _myState[1][10];
            newState[3][6] = _myState[0][10];
            // bottom 2 cells in the right change
            newState[1][10] = _myState[6][6];
            newState[0][10] = _myState[6][4];
            // bottom 2 cells in the back change
            newState[6][6] = _myState[0][0];
            newState[6][4] = _myState[1][0];
            // bottom 2 cells in the left change
            newState[0][0] = _myState[3][4];
            newState[1][0] = _myState[3][6];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "Bo";
            tmpNode.g = tmpNode._parentNode.g + 1;
            tmpNode.h = (12 - tmpNode.CornerMatches(_goalStateNode)) / 12;
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 4. Bottom CounterClockWise, denoted Bo'
            // all 4 bottom cells shift
            newState[4][4] = _myState[5][4];
            newState[4][6] = _myState[4][4];
            newState[5][6] = _myState[4][6];
            newState[5][4] = _myState[5][6];
            // bottom 2 cells in the front change
            newState[3][4] = _myState[0][0];
            newState[3][6] = _myState[1][0];
            // bottom 2 cells in the right change
            newState[1][10] = _myState[3][4];
            newState[0][10] = _myState[3][6];
            // bottom 2 cells in the back change
            newState[6][6] = _myState[1][10];
            newState[6][4] = _myState[0][10];
            // bottom 2 cells in the left change
            newState[0][0] = _myState[6][6];
            newState[1][0] = _myState[6][4];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "Bo'";
            tmpNode.g = tmpNode._parentNode.g + 1;
            tmpNode.h = (12 - tmpNode.CornerMatches(_goalStateNode)) / 12;
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 5. Front ClockWise, denoted F
            // all 4 front cells shift
            newState[2][4] = _myState[3][4];
            newState[3][4] = _myState[3][6];
            newState[3][6] = _myState[2][6];
            newState[2][6] = _myState[2][4];
            // front 2 cells in the top change
            newState[1][4] = _myState[1][0];
            newState[1][6] = _myState[1][2];
            // front 2 cells in the left change
            newState[1][0] = _myState[4][6];
            newState[1][2] = _myState[4][4];
            // front 2 cells in the bottom change
            newState[4][6] = _myState[1][8];
            newState[4][4] = _myState[1][10];
            // front 2 cells in the right change
            newState[1][8] = _myState[1][4];
            newState[1][10] = _myState[1][6];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "F";
            tmpNode.g = tmpNode._parentNode.g + 1;
            tmpNode.h = (12 - tmpNode.CornerMatches(_goalStateNode)) / 12;
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 6. Front CounterClockWise, denoted F'
            // all 4 front cells shift
            newState[2][4] = _myState[2][6];
            newState[3][4] = _myState[2][4];
            newState[3][6] = _myState[3][4];
            newState[2][6] = _myState[3][6];
            // front 2 cells in the top change
            newState[1][4] = _myState[1][8];
            newState[1][6] = _myState[1][10];
            // front 2 cells in the left change
            newState[1][0] = _myState[1][4];
            newState[1][2] = _myState[1][6];
            // front 2 cells in the bottom change
            newState[4][6] = _myState[1][0];
            newState[4][4] = _myState[1][2];
            // front 2 cells in the right change
            newState[1][8] = _myState[4][6];
            newState[1][10] = _myState[4][4];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "F'";
            tmpNode.g = tmpNode._parentNode.g + 1;
            tmpNode.h = (12 - tmpNode.CornerMatches(_goalStateNode)) / 12;
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 7. Back ClockWise, denoted Ba
            // all 4 back cells shift
            newState[6][4] = _myState[6][6];
            newState[6][6] = _myState[7][6];
            newState[7][6] = _myState[7][4];
            newState[7][4] = _myState[6][4];
            // back 2 cells in the top change
            newState[0][4] = _myState[0][0];
            newState[0][6] = _myState[0][2];
            // back 2 cells in the left change
            newState[0][0] = _myState[5][6];
            newState[0][2] = _myState[5][4];
            // back 2 cells in the bottom change
            newState[5][6] = _myState[0][8];
            newState[5][4] = _myState[0][10];
            // back 2 cells in the right change
            newState[0][8] = _myState[0][4];
            newState[0][10] = _myState[0][6];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "Ba";
            tmpNode.g = tmpNode._parentNode.g + 1;
            tmpNode.h = (12 - tmpNode.CornerMatches(_goalStateNode)) / 12;
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 8. Back CounterClockWise, denoted Ba'
            // all 4 back cells shift
            newState[6][4] = _myState[7][4];
            newState[6][6] = _myState[6][4];
            newState[7][6] = _myState[6][6];
            newState[7][4] = _myState[7][6];
            // back 2 cells in the top change
            newState[0][4] = _myState[0][8];
            newState[0][6] = _myState[0][10];
            // back 2 cells in the left change
            newState[0][0] = _myState[0][4];
            newState[0][2] = _myState[0][6];
            // back 2 cells in the bottom change
            newState[5][6] = _myState[0][0];
            newState[5][4] = _myState[0][2];
            // back 2 cells in the right change
            newState[0][8] = _myState[5][6];
            newState[0][10] = _myState[5][4];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "Ba'";
            tmpNode.g = tmpNode._parentNode.g + 1;
            tmpNode.h = (12 - tmpNode.CornerMatches(_goalStateNode)) / 12;
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 9. Left ClockWise, denoted L
            // all 4 left cells shift
            newState[0][0] = _myState[1][0];
            newState[0][2] = _myState[0][0];
            newState[1][0] = _myState[1][2];
            newState[1][2] = _myState[0][2];
            // left 2 cells in the top change
            newState[0][4] = _myState[6][4];
            newState[1][4] = _myState[7][4];
            // left 2 cells in the front change
            newState[2][4] = _myState[0][4];
            newState[3][4] = _myState[1][4];
            // left 2 cells in the bottom change
            newState[4][4] = _myState[2][4];
            newState[5][4] = _myState[3][4];
            // left 2 cells in the back change
            newState[6][4] = _myState[4][4];
            newState[7][4] = _myState[5][4];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "L";
            tmpNode.g = tmpNode._parentNode.g + 1;
            tmpNode.h = (12 - tmpNode.CornerMatches(_goalStateNode)) / 12;
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 10. Left CounterClockWise, denoted L'
            // all 4 left cells shift
            newState[0][0] = _myState[0][2];
            newState[0][2] = _myState[1][2];
            newState[1][0] = _myState[0][0];
            newState[1][2] = _myState[1][0];
            // left 2 cells in the top change
            newState[0][4] = _myState[2][4];
            newState[1][4] = _myState[3][4];
            // left 2 cells in the front change
            newState[2][4] = _myState[4][4];
            newState[3][4] = _myState[5][4];
            // left 2 cells in the bottom change
            newState[4][4] = _myState[6][4];
            newState[5][4] = _myState[7][4];
            // left 2 cells in the back change
            newState[6][4] = _myState[0][4];
            newState[7][4] = _myState[1][4];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "L'";
            tmpNode.g = tmpNode._parentNode.g + 1;
            tmpNode.h = (12 - tmpNode.CornerMatches(_goalStateNode)) / 12;
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 11. Right ClockWise, denoted R
            // all 4 right cells shift
            newState[0][8] = _myState[0][10];
            newState[0][10] = _myState[1][10];
            newState[1][8] = _myState[0][8];
            newState[1][10] = _myState[1][8];
            // right 2 cells in the top change
            newState[0][6] = _myState[6][6];
            newState[1][6] = _myState[7][6];
            // right 2 cells in the front change
            newState[2][6] = _myState[0][6];
            newState[3][6] = _myState[1][6];
            // right 2 cells in the bottom change
            newState[4][6] = _myState[2][6];
            newState[5][6] = _myState[3][6];
            // right 2 cells in the back change
            newState[6][6] = _myState[4][6];
            newState[7][6] = _myState[5][6];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "R";
            tmpNode.g = tmpNode._parentNode.g + 1;
            tmpNode.h = (12 - tmpNode.CornerMatches(_goalStateNode)) / 12;
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            newState = CopyState();
            // 12. Right CounterClockWise, denoted R'
            // all 4 right cells shift
            newState[0][8] = _myState[1][8];
            newState[0][10] = _myState[0][8];
            newState[1][8] = _myState[1][10];
            newState[1][10] = _myState[0][10];
            // right 2 cells in the top change
            newState[0][6] = _myState[2][6];
            newState[1][6] = _myState[3][6];
            // right 2 cells in the front change
            newState[2][6] = _myState[4][6];
            newState[3][6] = _myState[5][6];
            // right 2 cells in the bottom change
            newState[4][6] = _myState[6][6];
            newState[5][6] = _myState[7][6];
            // right 2 cells in the back change
            newState[6][6] = _myState[0][6];
            newState[7][6] = _myState[1][6];

            tmpNode = new Node(newState, this);
            tmpNode.Move = "R'";
            tmpNode.g = tmpNode._parentNode.g + 1;
            tmpNode.h = (12 - tmpNode.CornerMatches(_goalStateNode)) / 12;
            tmpNode.f = tmpNode.g + tmpNode.h;
            tmpNode.goalStateNode = _goalStateNode;
            _childNodes.Add(tmpNode);

            if (this._parentNode != null)// && this._childNodes != null)
            {
                _childNodes.Remove(this._parentNode);
            }
            return _childNodes;
        }


        public void showNodeInfo()
        {
            Console.WriteLine("******************");
            Console.WriteLine("Current Node : " + this.Move);
            foreach (List<char> l in this._myState)
            {
                foreach (char c in l)
                {
                    //Console.BackgroundColor = ConsoleColor.Black;
                    if (c.Equals('r')) { Console.BackgroundColor = ConsoleColor.Red; }
                    else if (c.Equals('y')) { Console.BackgroundColor = ConsoleColor.Yellow; }
                    else if (c.Equals('o')) { Console.BackgroundColor = ConsoleColor.Magenta; }
                    else if (c.Equals('p')) { Console.BackgroundColor = ConsoleColor.DarkMagenta; }
                    else if (c.Equals('b')) { Console.BackgroundColor = ConsoleColor.Blue; }
                    else if (c.Equals('g')) { Console.BackgroundColor = ConsoleColor.Green; }
                    //else { Console.BackgroundColor = ConsoleColor.Black; }
                    Console.Write(c);
                }
                Console.Write(' ');
                Console.WriteLine();
                Console.BackgroundColor = ConsoleColor.Black;
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("Heuristic value: " + this.h);
            Console.WriteLine("******************");
        }

    }

   

}
