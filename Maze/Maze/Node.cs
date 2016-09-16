using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Maze
{
    public class Node : IEquatable<Node>
    {
        // Node location
        private int _x;
        private int _y;

        // Unused
        //private char _val;

        // Cost variables
        private int _h;  // cost to get to goal
        private int _g;  // cost to get to location
        private int _f;  // total cost (f+g)

        private Node _parentNode;  // reference to parent node
        private List<Node> _childNodes; // list of child nodes
        private Node _goalStateNode; 

        public bool _isInitialized;

        public Node(int x, int y, Node parentNode)
        {
            this._x = x;
            this._y = y;
            this._parentNode = parentNode;
            this._isInitialized = true;
        }

        public Node(int x, int y, Node parentNode, Node goalStateNode)
        {
            this._x = x;
            this._y = y;
            this._parentNode = parentNode;
            this._goalStateNode = goalStateNode;
            this._isInitialized = true;
        }

        public int x {
            get { return this._x; }
            set { this._x = value; }
        }
        public int y
        {
            get { return this._y; }
            set { this._y = value; }
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
            if (n.x == this._x && n.y == this._y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private int calcManhattanDistance(int x, int y)
        {
            int z = 0;
            int xdelta = 0;
            int ydelta = 0;

            xdelta = Math.Abs(x - this._goalStateNode.x);
            ydelta = Math.Abs(y - this._goalStateNode.y);

            z = xdelta + ydelta;
            return z;
        }

        public List<Node> findEligibleChildren(List<List<char>> mazeBoard, List<Node> otherChildNodes)
        {
            // See N   check if parentNode is not null, then check i'm not going to revisit a parent
            Node tmpNode = new Node(this._x, this._y - 1, this);

            if (isWalkable(this._x, this._y - 1, mazeBoard) && 
               //(((this._parentNode != null && !(this._parentNode._x != this.x || this._parentNode._y != this._y - 1)) || this._parentNode == null)) && 
               !otherChildNodes.Contains(tmpNode))
            {
                //tmpNode.g = tmpNode.parentNode.g + 10;
                //tmpNode.h = calcManhattanDistance(this._x, this.y-1);
                //tmpNode.f = tmpNode.g + tmpNode.h;
                //tmpNode.goalStateNode = this.goalStateNode;
                AddChild(tmpNode);
            }

            

            //// See NE
            //tmpNode = new Node(this._x + 1, this._y - 1, this);
            //if (isWalkable(this._x + 1, this._y - 1, mazeBoard) && 
            //    //(((this._parentNode != null && !(this._parentNode._x != this.x + 1 || this._parentNode._y != this._y - 1)) || this._parentNode == null)) && 
            //    !otherChildNodes.Contains(tmpNode))
            //{
            //    //tmpNode.g = tmpNode.parentNode.g + 14;
            //    //tmpNode.h = calcManhattanDistance(this._x+1, this.y-1);
            //    //tmpNode.f = tmpNode.g + tmpNode.h;
            //    //tmpNode.goalStateNode = this.goalStateNode;
            //    AddChild(tmpNode);
            //}
            
            // See E
            tmpNode = new Node(this._x + 1, this._y, this);
            if (isWalkable(this._x + 1, this._y, mazeBoard) && 
                //(((this._parentNode != null && !(this._parentNode._x != this.x + 1 || this._parentNode._y != this._y)) || this._parentNode == null)) && 
                !otherChildNodes.Contains(tmpNode))
            {
                //tmpNode.g = tmpNode.parentNode.g + 10;
                //tmpNode.h = calcManhattanDistance(this._x+1, this._y);
                //tmpNode.f = tmpNode.g + tmpNode.h;
                //tmpNode.goalStateNode = this.goalStateNode;
                AddChild(tmpNode);
            }

            //// See SE
            //tmpNode = new Node(this._x + 1, this._y + 1, this);
            //if (isWalkable(this._x + 1, this._y + 1, mazeBoard) && 
            //    //(((this._parentNode != null && !(this._parentNode._x != this.x + 1 || this._parentNode._y != this._y + 1)) || this._parentNode == null)) && 
            //    !otherChildNodes.Contains(tmpNode))
            //{
            //    //tmpNode.g = tmpNode.parentNode.g + 14;
            //    //tmpNode.h = calcManhattanDistance(this._x+1, this._y+1);
            //    //tmpNode.f = tmpNode.g + tmpNode.h;
            //    //tmpNode.goalStateNode = this.goalStateNode;
            //    AddChild(tmpNode);
            //}

            // See S
            tmpNode = new Node(this._x, this._y + 1, this);
            if (isWalkable(this._x, this._y + 1, mazeBoard) && 
                //(((this._parentNode != null && !(this._parentNode._x != this.x || this._parentNode._y != this._y + 1)) || this._parentNode == null)) && 
                !otherChildNodes.Contains(tmpNode))
            {
                //tmpNode.g = tmpNode.parentNode.g + 10;
                //tmpNode.h = calcManhattanDistance(this._x, this._y+1);
                //tmpNode.f = tmpNode.g + tmpNode.h;
                //tmpNode.goalStateNode = this.goalStateNode;
                AddChild(tmpNode);
            }

            //// See SW
            //tmpNode = new Node(this._x - 1, this._y + 1, this);
            //if (isWalkable(this._x - 1, this._y + 1, mazeBoard) && 
            //    //(((this._parentNode != null && !(this._parentNode._x != this.x - 1 || this._parentNode._y != this._y + 1)) || this._parentNode == null)) && 
            //    !otherChildNodes.Contains(tmpNode))
            //{
            //    //tmpNode.g = tmpNode.parentNode.g + 14;
            //    //tmpNode.h = calcManhattanDistance(this._x-1, this.y+1);
            //    //tmpNode.f = tmpNode.g + tmpNode.h;
            //    //tmpNode.goalStateNode = this.goalStateNode;
            //    AddChild(tmpNode);
            //}

            // See W
            tmpNode = new Node(this._x - 1, this._y, this);
            if (isWalkable(this._x - 1, this._y, mazeBoard) && 
                //(((this._parentNode != null && !(this._parentNode._x != this.x - 1 || this._parentNode._y != this._y)) || this._parentNode == null)) && 
                !otherChildNodes.Contains(tmpNode))
            {
                //tmpNode.g = tmpNode.parentNode.g + 10;
                //tmpNode.h = calcManhattanDistance(this._x-1, this._y);
                //tmpNode.f = tmpNode.g + tmpNode.h;
                //tmpNode.goalStateNode = this.goalStateNode;
                AddChild(tmpNode);
            }

            //// See NW
            //tmpNode = new Node(this._x - 1, this._y - 1, this);
            //if (isWalkable(this._x - 1, this._y - 1, mazeBoard) && 
            //    //(((this._parentNode != null && !(this._parentNode._x != this.x - 1 || this._parentNode._y != this._y - 1)) || this._parentNode == null)) && 
            //    !otherChildNodes.Contains(tmpNode))
            //{
            //    //tmpNode.g = tmpNode.parentNode.g + 14;
            //    //tmpNode.h = calcManhattanDistance(this._x-1, this._y-1);
            //    //tmpNode.f = tmpNode.g + tmpNode.h;
            //    //tmpNode.goalStateNode = this.goalStateNode;
            //    AddChild(tmpNode);
            //}

            if (this._parentNode != null && this._childNodes != null)
            {
                _childNodes.Remove(this._parentNode);
            }
            return _childNodes;
        }

        public List<Node> findEligibleChildrenA(List<List<char>> mazeBoard)
        {
            // See N   check if parentNode is not null, then check i'm not going to revisit a parent
            Node tmpNode = new Node(this._x, this._y - 1, this);

            if (isWalkable(this._x, this._y - 1, mazeBoard))
            {
                tmpNode.g = tmpNode.parentNode.g + 10;
                tmpNode.h = calcManhattanDistance(this._x, this.y - 1);
                tmpNode.f = tmpNode.g + tmpNode.h;
                tmpNode.goalStateNode = this.goalStateNode;
                AddChild(tmpNode);
            }



            //// See NE
            //tmpNode = new Node(this._x + 1, this._y - 1, this);
            //if (isWalkable(this._x + 1, this._y - 1, mazeBoard))
            //{
            //    tmpNode.g = tmpNode.parentNode.g + 14;
            //    tmpNode.h = calcManhattanDistance(this._x + 1, this.y - 1);
            //    tmpNode.f = tmpNode.g + tmpNode.h;
            //    tmpNode.goalStateNode = this.goalStateNode;
            //    AddChild(tmpNode);
            //}

            // See E
            tmpNode = new Node(this._x + 1, this._y, this);
            if (isWalkable(this._x + 1, this._y, mazeBoard))
            {
                tmpNode.g = tmpNode.parentNode.g + 10;
                tmpNode.h = calcManhattanDistance(this._x + 1, this._y);
                tmpNode.f = tmpNode.g + tmpNode.h;
                tmpNode.goalStateNode = this.goalStateNode;
                AddChild(tmpNode);
            }

            //// See SE
            //tmpNode = new Node(this._x + 1, this._y + 1, this);
            //if (isWalkable(this._x + 1, this._y + 1, mazeBoard))
            //{
            //    tmpNode.g = tmpNode.parentNode.g + 14;
            //    tmpNode.h = calcManhattanDistance(this._x + 1, this._y + 1);
            //    tmpNode.f = tmpNode.g + tmpNode.h;
            //    tmpNode.goalStateNode = this.goalStateNode;
            //    AddChild(tmpNode);
            //}

            // See S
            tmpNode = new Node(this._x, this._y + 1, this);
            if (isWalkable(this._x, this._y + 1, mazeBoard))
            {
                tmpNode.g = tmpNode.parentNode.g + 10;
                tmpNode.h = calcManhattanDistance(this._x, this._y + 1);
                tmpNode.f = tmpNode.g + tmpNode.h;
                tmpNode.goalStateNode = this.goalStateNode;
                AddChild(tmpNode);
            }

            //// See SW
            //tmpNode = new Node(this._x - 1, this._y + 1, this);
            //if (isWalkable(this._x - 1, this._y + 1, mazeBoard))
            //{
            //    tmpNode.g = tmpNode.parentNode.g + 14;
            //    tmpNode.h = calcManhattanDistance(this._x - 1, this.y + 1);
            //    tmpNode.f = tmpNode.g + tmpNode.h;
            //    tmpNode.goalStateNode = this.goalStateNode;
            //    AddChild(tmpNode);
            //}

            // See W
            tmpNode = new Node(this._x - 1, this._y, this);
            if (isWalkable(this._x - 1, this._y, mazeBoard))
            {
                tmpNode.g = tmpNode.parentNode.g + 10;
                tmpNode.h = calcManhattanDistance(this._x - 1, this._y);
                tmpNode.f = tmpNode.g + tmpNode.h;
                tmpNode.goalStateNode = this.goalStateNode;
                AddChild(tmpNode);
            }

            //// See NW
            //tmpNode = new Node(this._x - 1, this._y - 1, this);
            //if (isWalkable(this._x - 1, this._y - 1, mazeBoard))
            //{
            //    tmpNode.g = tmpNode.parentNode.g + 14;
            //    tmpNode.h = calcManhattanDistance(this._x - 1, this._y - 1);
            //    tmpNode.f = tmpNode.g + tmpNode.h;
            //    tmpNode.goalStateNode = this.goalStateNode;
            //    AddChild(tmpNode);
            //}
            if (this._parentNode != null && this._childNodes != null)
            {
                _childNodes.Remove(this._parentNode);
            }
            return _childNodes;
        }
        private bool isWalkable(int x, int y, List<List<char>> mazeBoard)
        {
            try
            {
                if (mazeBoard[y][x].Equals(' ') || mazeBoard[y][x].Equals('.'))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public void AddChild(Node tmpNode)
        {
            try
            {
                if (_childNodes == null) {
                    _childNodes = new List<Node>();
                }
                _childNodes.Add(tmpNode);  //Null reference

            }
            catch (Exception e)
            {
                Console.WriteLine("Error adding Child for x: " + tmpNode._x + ", y: " + tmpNode._y + " due to: " + e.InnerException + " " + e.Message );
            
            }
        }

        public void showNodeInfo()
        {
            Console.WriteLine("******************");
            Console.WriteLine("Current Node ");
            Console.WriteLine("X Coordinate: " + this._x);
            Console.WriteLine("Y Coordinate: " + this._y);
            Console.WriteLine("******************");
        }
        
    }

   

}
