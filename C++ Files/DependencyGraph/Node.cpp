#include "Node.h"

Node::Node(std::string s):dependents(new std::list<Node>()), dependees(new std::list<Node>())
			  //Node::Node(std::string s)
{
  name = s;
}

Node::~Node(){}

void Node::add_dependent(Node n)
{
  (*dependents).push_back(n);
  n.add_dependee2(*this);
}

void Node::add_dependent2(Node n)
{
  (*dependents).push_back(n);
}

void Node::add_dependee(Node n)
{
  (*dependees).push_back(n);
  n.add_dependent2(*this);
}

void Node::add_dependee2(Node n)
{
  (*dependees).push_back(n);
}

void Node::remove_dependent(Node n)
{
  (*dependents).remove(n);
  n.remove_dependee(*this);
}

void Node::remove_dependent2(Node n)
{
  (*dependents).remove(n);
}

void Node::remove_dependee(Node n)
{
  (*dependees).remove(n);
}

void Node::remove_all_dependents()
{
  std::list<Node>::iterator it;
  for(it = (*dependents).begin(); it != (*dependents).end(); ++it)
    {
      (*it).remove_dependee(*this);
    }
  (*dependents).clear();
}

void Node::remove_all_dependees()
{
  std::list<Node>::iterator it;
  for(it = (*dependees).begin(); it != (*dependees).end(); ++it)
    {
      (*it).remove_dependent2(*this);
    }
  (*dependees).clear();
}

/*
 * Checks if two Nodes are equal (used in the remove method)
 */
bool Node::operator==(const Node &rhs) 
{
  if(name == rhs.name)
	return true;
  else
	return false;
}

int main(){}
