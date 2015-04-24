#include "Node.h"
#include <list>
#include <string>
#include <iostream>

#ifndef DEPENDENCYGRAPH_H
#define DEPENDENCYGRAPH_H

// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
// (Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
// set, and the element is already in the set, the set remains unchanged.)
// 
// Given a DependencyGraph DG:
// 
//    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
//        
//    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
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
// *Comments copied from C# Dependency Graph Skeleton
class DependencyGraph
{
 public:
  // constructor - Creates an empty DependencyGraph.
  DependencyGraph();
  // destructor
  ~DependencyGraph();

  // gets the number of ordered pairs in the DependencyGraph.
  int GetSize();

  // using the string representation of a cell name, reports whether dependents(s) is non-empty.
  bool HasDependents(std::string s);

  // using the string representation of a cell name, Reports whether dependees(s) is non-empty
  bool HasDependees(std::string s);

  // returns this cell's list of dependents
  std::list<std::string> GetDependents(std::string s);

  // returns this cell's list of dependees
  std::list<std::string> GetDependees(std::string s);

  // adds cell t (using string representation) as a dependent of cell s
  void AddDependency(std::string s, std::string t);

  // removes cell t (using string representation) as a dependent of cell s
  void RemoveDependency(std::string s, std::string t);

 private:
  
  std::list<Node::Node> *nodes;
  int size;
  Node::Node GetNode(std::string s);
};

#endif
