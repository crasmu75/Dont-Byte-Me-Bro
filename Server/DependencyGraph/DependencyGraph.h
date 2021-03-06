#include "Node.h"
#include <list>
#include <string>
#include <iostream>

#ifndef DEPENDENCYGRAPH_H
#define DEPENDENCYGRAPH_H

class DependencyGraph
{
 public:
  DependencyGraph();
  ~DependencyGraph();
  int GetSize();
  bool HasDependents(std::string s);
  bool HasDependees(std::string s);
  std::list<std::string> GetDependents(std::string s);
  std::list<std::string> GetDependees(std::string s);
  void AddDependency(std::string s, std::string t);
  void RemoveDependency(std::string s, std::string t);

 private:
  std::list<Node::Node> *nodes;
  int size;
  Node::Node GetNode(std::string s);
};

#endif
