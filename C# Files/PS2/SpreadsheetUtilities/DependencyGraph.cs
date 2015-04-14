// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{

    /// <summary>
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// (Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.)
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        private List<node> Nodes = new List<node>();
        private int size = 0;
        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            List<node> Nodes = new List<node>();
        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return size; }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get
            {
                node current = getNode(s);
                if (current == null)
                    return 0;
                else 
                    return current.dependees.Count();
            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            node _s = getNode(s);

            if (_s == null)
                return false;

            else if (_s.dependents.Count != 0)
                return true;
            
            else
                return false;
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            node _s = getNode(s);

            if (_s == null)
                return false;

            else if (_s.dependees.Count != 0)
                return true;

            else
                return false;
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            // get s node
            node _s = getNode(s);
            if (_s == null)
                return new HashSet<string>();

            // Create a list to hold the dependents
            HashSet<string> dependentsList = new HashSet<string>();

            // for each node in the dependents hashset, add the name of the node (string) to the list
            foreach (node n in _s.dependents)
                dependentsList.Add(n.name);

            // return
            return dependentsList;
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            // get s node
            node _s = getNode(s);
            if (_s == null)
                return new HashSet<string>();

            // Create a list to hold the dependents
            HashSet<string> dependeesList = new HashSet<string>();

            // for each node in the dependents hashset, add the name of the node (string) to the list
            foreach (node n in _s.dependees)
                dependeesList.Add(n.name);

            // return
            return dependeesList;
        }


        /// <summary>
        /// Adds the ordered pair (s,t), if it doesn't exist
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void AddDependency(string s, string t)
        {
            // check if Nodes contains s
            //    if not, add it
            node _s = getNode(s);

            if (_s == null)
            {
                _s = new node(s);
                Nodes.Add(_s);
            }

            // also check if Nodes contains t
            node _t = getNode(t);

            if (_t == null)
            {
                _t = new node(t);
                Nodes.Add(_t);
            }

            // check if t is already a dependent of s
            bool tDep = false;

            // look for relationship in dependents list
            foreach (node n in _s.dependents)
                if (n.name.Equals(t))
                    tDep = true;

            // add t as dependee of s if relationship doesn't already exist
            if (!tDep)
            {
                _s.addDependent(_t);
                size++;
            }
        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            // check for existence in Node list
            node _s = getNode(s);
            node _t = getNode(t);

            if (_s == null || _t == null)
                return;

            if (_s.dependents.Contains(_t))
            {
                _s.removeDependent(_t);
                size--;
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            // get s node
            node _s = getNode(s);

            if (_s == null)
                Nodes.Add(new node(s));

            else
            {
                // delete all dependents
                _s.removeAllDependents();

                // decrement size
                size -= _s.dependents.Count();
            }

            // add in all newDependents
            foreach (string newDep in newDependents)
            {
                node newNode = getNode(newDep);

                if (newNode == null)
                    newNode = new node(newDep);

                _s.addDependent(newNode);
                size++;
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            // get s node
            node _s = getNode(s);

            if (_s == null)
                Nodes.Add(new node(s));

            else
            {
                // delete all dependents
                _s.removeAllDependees();

                // decrement size
                size -= _s.dependees.Count();
            }

            // add in all newDependents
            foreach (string newDep in newDependees)
            {
                node newNode = getNode(newDep);

                if (newNode == null)
                    newNode = new node(newDep);

                _s.addDependee(newNode);
                size++;
            }
        }

        private node getNode(string s)
        {
            foreach (node n in Nodes)
                if (n.name.Equals(s))
                    return n;
            
            return null;
        }

        class node
        {
            public String name;
            public List<node> dependents;
            public List<node> dependees;

            public node(string s)
            {
                this.name = s;
                this.dependents = new List<node>();
                this.dependees = new List<node>();
            }

            /// <summary>
            /// Add a dependent node to hashset of dependents
            /// Ensures that the reverse dependee relationship is updated
            /// </summary>
            /// <param name="n"></param>
            public void addDependent(node n)
            {
                this.dependents.Add(n);
                n.addDependee2(this);
            }
            /// <summary>
            /// Called by addDependent to assert the reverse relationship of the newly added dependent
            /// </summary>
            /// <param name="n"></param>
            public void addDependee2(node n)
            {
                this.dependees.Add(n);
            }

            /// <summary>
            /// Add a dependee node to hashet of dependees
            /// Ensures that the reverse dependent relatioinship is updated
            /// </summary>
            /// <param name="n"></param>
            public void addDependee(node n)
            {
                this.dependees.Add(n);
                n.addDependent2(this);
            }

            /// <summary>
            /// Called by addDependee to assert the reverse relationship of the newly added dependee
            /// </summary>
            /// <param name="n"></param>
            public void addDependent2(node n)
            {
                this.dependents.Add(n);
            }

            /// <summary>
            /// Removes a dependent from the hashset of dependents
            /// Ensures that the reverse dependent relationship is updated
            /// </summary>
            /// <param name="n"></param>
            public void removeDependent(node n)
            {
                this.dependents.Remove(n);
                n.removeDependee(this);
            }

            /// <summary>
            /// Called by removeDependent to assert the reverse relationship of the newly removed dependent
            /// </summary>
            /// <param name="n"></param>
            public void removeDependee(node n)
            {
                this.dependees.Remove(n);
            }

            /// <summary>
            /// Removes a dependent without removing the corresponding dependee
            /// Used in removeAllDependees method
            /// </summary>
            public void removeDependent2(node n)
            {
                this.dependents.Remove(n);
            }

            /// <summary>
            /// Removes all dependent nodes
            /// </summary>
            public void removeAllDependents()
            {
                // remove all of the reverse relationship
                foreach (node n in this.dependents)
                {
                    n.removeDependee(this);
                }

                // delete all dependents
                this.dependents.Clear();
            }

            /// <summary>
            /// Removes all dependee nodes
            /// </summary>
            public void removeAllDependees()
            {
                // remove all of the reverse relationship
                foreach (node n in this.dependees)
                {
                    n.removeDependent2(this);
                }

                // delete all dependees
                this.dependees.Clear();
            }
        }

    }
    

    }

   