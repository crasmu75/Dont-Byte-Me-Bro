#include "DependencyGraph.h"
#include <iostream>
#include <exception>

// Dependency Graph constructor
DependencyGraph::DependencyGraph()
{
  nodes = new std::list<Node::Node>();
  size = 0;
}

// Empty destructor
DependencyGraph::~DependencyGraph(){}

// Returns the size of the Dependency Graph
int DependencyGraph::GetSize()
{
  return size;
}

// Checks if the current node has dependents
bool DependencyGraph::HasDependents(std::string s)
{
	// Check if parameter node exists
  Node::Node current("");
  try
    {
      current = GetNode(s);
    }
  catch (std::exception& e)
    {
      return false;
    }
  
  // Check if dependents list contains any nodes
  if ((current.dependents)->size() != 0)
     return true;

  else return false;
}

// Checks if current node has dependees
bool DependencyGraph::HasDependees(std::string s)
{
	// Check if parameter node exists
  Node::Node current("");
  try
    {
      current = GetNode(s);
    }
  catch (std::exception& e)
    {
      return false;
    }
  
  // Check if dependees list contains any nodes
  if ((current.dependees)->size() != 0)
     return true;

  else return false;
}

// Get a list of all dependents of a node
std::list<std::string> DependencyGraph::GetDependents(std::string s)
{
	// Check if parameter node exists
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

  // Iterate list of dependents and add all nodes to the list to return
  std::list<Node::Node>::iterator it;
  for(it = (current.dependents)->begin(); it != (current.dependents)->end(); ++it)
    {
      std::string currentName = (*it).GetName();
      dependentsList.push_back(currentName);
    }
  
  // Return list of dependents
  return dependentsList;
}

// Get a list of dependees of a node
std::list<std::string> DependencyGraph::GetDependees(std::string s)
{
	// Check if parameter node exists
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

  // Iterate list of dependees and add all ndoes to the list to return
  std::list<Node::Node>::iterator it;
  for(it = (current.dependees)->begin(); it != (current.dependees)->end(); ++it)
    {
      std::string currentName = (*it).GetName();
      dependeesList.push_back(currentName);
    }
  
  // Return list of dependees
  return dependeesList;
}

// Add a dependency (s, t) where t is a dependent of s
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

  // Check if t is already a dependent of s
  std::list<Node::Node>::iterator it;
  for(it = (s_node.dependents)->begin(); it != (s_node.dependents)->end(); ++it)
    {
      if ((*it).GetName().compare(t) == 0)
	{
	  tDep = true;
	}
    }

  // If not, add t as a dependent
  if (!tDep)
    {
      s_node.add_dependent(t_node);
      size++;
    }
}

// Remove dependency (s, t) where t was a dependent of s
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

  // Check if t was previously a dependent of s
  std::list<Node::Node>::iterator it;
  for(it = (s_node.dependents)->begin(); it != (s_node.dependents)->end(); ++it)
    {
      if ((*it).GetName().compare(t) == 0)
	{
	  tDep = true;
	}
    }

  // If it was, remove the dependency
  if (tDep)
    {
      s_node.remove_dependent(t_node);
      size--;
    }
}

// Get the node with a specified name
Node::Node DependencyGraph::GetNode(std::string s)
{
	// Check if the node exists
  std::list<Node::Node>::iterator it;
  for(it = (nodes)->begin(); it != (nodes)->end(); ++it)
    {
      if ((*it).GetName().compare(s) == 0)
	{
	  return *it;
	}
    }
  
  // Throw an exception if the node does not exist
  throw std::exception();
}
