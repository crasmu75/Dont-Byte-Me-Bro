#include "DependencyGraph.h"
#include <iostream>
#include <exception>

DependencyGraph::DependencyGraph()
{
  nodes = new std::list<Node::Node>();
  size = 0;
}

DependencyGraph::~DependencyGraph(){}

int DependencyGraph::GetSize()
{
  return size;
}

bool DependencyGraph::HasDependents(std::string s)
{
  Node::Node current("");
  try
    {
      current = GetNode(s);
    }
  catch (std::exception& e)
    {
      return false;
    }
  
  if ((current.dependents)->size() != 0)
     return true;

  else return false;
}

bool DependencyGraph::HasDependees(std::string s)
{
  Node::Node current("");
  try
    {
      current = GetNode(s);
    }
  catch (std::exception& e)
    {
      return false;
    }
  
  if ((current.dependees)->size() != 0)
     return true;

  else return false;
}

std::list<std::string> DependencyGraph::GetDependents(std::string s)
{
  Node::Node current("");
  std::list<std::string> dependentsList;
  try
    {
      current = GetNode(s);
    }
  catch (std::exception& e)
    {
      return dependentsList;
    }

  std::list<Node::Node>::iterator it;
  for(it = (current.dependents)->begin(); it != (current.dependents)->end(); ++it)
    {
      std::string currentName = (*it).GetName();
      dependentsList.push_back(currentName);
    }
  
  return dependentsList;
}

std::list<std::string> DependencyGraph::GetDependees(std::string s)
{
  Node::Node current("");
  std::list<std::string> dependeesList;
  try
    {
      current = GetNode(s);
    }
  catch (std::exception& e)
    {
      return dependeesList;
    }

  std::list<Node::Node>::iterator it;
  for(it = (current.dependees)->begin(); it != (current.dependees)->end(); ++it)
    {
      std::string currentName = (*it).GetName();
      dependeesList.push_back(currentName);
    }
  
  return dependeesList;
}

void DependencyGraph::AddDependency(std::string s, std::string t)
{
  Node::Node s_node("");
  Node::Node t_node("");

  // check if s and t exist already as nodes
  try
    {
      s_node = GetNode(s);
    }
  // if not, create the node and add to list of nodes
  catch (std::exception& e)
    {
      s_node.SetName(s);
      nodes->push_back(s_node);
    }

  // repeat with t
  try
    {
      t_node = GetNode(t);
    }
  catch (std::exception& e)
    {
      t_node.SetName(t);
      nodes->push_back(t_node);
    }

  bool tDep = false;

  std::list<Node::Node>::iterator it;
  for(it = (s_node.dependents)->begin(); it != (s_node.dependents)->end(); ++it)
    {
      if ((*it).GetName().compare(t) == 0)
	{
	  tDep = true;
	}
    }

  if (!tDep)
    {
      s_node.add_dependent(t_node);
      size++;
    }
}

void DependencyGraph::RemoveDependency(std::string s, std::string t)
{
  Node::Node s_node("");
  Node::Node t_node("");

  // check if s and t exist already as nodes
  try
    {
      s_node = GetNode(s);
    }
  // if not, create the node and add to list of nodes
  catch (std::exception& e)
    {
      return;
    }

  // repeat with t
  try
    {
      t_node = GetNode(t);
    }
  catch (std::exception& e)
    {
      return;
    }

  bool tDep = false;

  std::list<Node::Node>::iterator it;
  for(it = (s_node.dependents)->begin(); it != (s_node.dependents)->end(); ++it)
    {
      if ((*it).GetName().compare(t) == 0)
	{
	  tDep = true;
	}
    }

  if (tDep)
    {
      s_node.remove_dependent(t_node);
      size--;
    }
}

Node::Node DependencyGraph::GetNode(std::string s)
{
  std::list<Node::Node>::iterator it;
  for(it = (nodes)->begin(); it != (nodes)->end(); ++it)
    {
      if ((*it).GetName().compare(s) == 0)
	{
	  return *it;
	}
    }
  throw std::exception();
}
