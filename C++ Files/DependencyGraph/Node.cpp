#include "Node.h"

// Node constructor
Node::Node(std::string s):dependents(new std::list<Node>()), dependees(new std::list<Node>())
{
  name = s;
}

// Empty destructor
Node::~Node(){}

// Add a dependent to the current node 
//		& make current node a dependee
void Node::add_dependent(Node n)
{
  (*dependents).push_back(n);
  n.add_dependee2(*this);
}

// Helper method to only add a node as a dependent 
//		Creates the reverse relationship of add_dependee
void Node::add_dependent2(Node n)
{
  (*dependents).push_back(n);
}

// Add a dependee to the current node
//		& make the current node a dependent
void Node::add_dependee(Node n)
{
  (*dependees).push_back(n);
  n.add_dependent2(*this);
}

// Helper method to only add a node as a dependee
//		Create the reverse relationship of add_dependent
void Node::add_dependee2(Node n)
{
  (*dependees).push_back(n);
}

// Remove a dependent from the current node
//		& remove the current node from the dependees list
void Node::remove_dependent(Node n)
{
  (*dependents).remove(n);
  n.remove_dependee(*this);
}

// Helper method to only remove a node from the dependents list
//		Eliminate the reverse relationship of remove_dependee
void Node::remove_dependent2(Node n)
{
  (*dependents).remove(n);
}

// Removes a dependee from the current node
void Node::remove_dependee(Node n)
{
  (*dependees).remove(n);
}

// Removes all dependents from the current node
void Node::remove_all_dependents()
{
	// Iterate the list of dependents
  std::list<Node>::iterator it;
  for(it = (*dependents).begin(); it != (*dependents).end(); ++it)
    {
		// Remove current node from itr's list of dependees
      (*it).remove_dependee(*this);
    }
  // Clear the list of dependents
  (*dependents).clear();
}

// Remove all dependees from the current node
void Node::remove_all_dependees()
{
	// Iterate the list of dependees
  std::list<Node>::iterator it;
  for(it = (*dependees).begin(); it != (*dependees).end(); ++it)
    {
		// Remove current node from itr's list of dependents
      (*it).remove_dependent2(*this);
    }
  // Clear the list of dependees
  (*dependees).clear();
}


// Overload == operator
//		Checks if two Nodes are equal (used in the remove method)
bool Node::operator==(const Node &rhs) 
{
  if(name == rhs.name)
	return true;
  else
	return false;
}

// Get the name of the node
std::string Node::GetName()
{
  return name;
}

// Set the name of a node
void Node::SetName(std::string s)
{
  name = s;
}
