#include<iostream>
#include<string>
#include<list>

#ifndef NODE_H
#define NODE_H

// Node class to assist in Dependency Graph
//
// A node contains a list of dependent nodes and dependee nodes. The dependency graph will have a list of nodes
// which is every cell in the cellContentsMap.
  class Node
  {
  public:
    Node(std::string s); // constructor
    ~Node(); // destructor
      
    // adds one dependent node to this node
    void add_dependent(Node n);
      
    // adds one dependee node to this node
    void add_dependee(Node n);
      
    // removes one dependent node from this node's dependent nodes
    void remove_dependent(Node n);
      
    // removes one dependee node from this node's dependee nodes
    void remove_dependee(Node n);
      
    // removes all dependent nodes from this node's list of dependent nodes
    void remove_all_dependents();
      
    // removes all dependee nodes from this node's list of dependee nodes
    void remove_all_dependees();
      
    // overriding the == operator for a node
    bool operator== (const Node & rhs);
      
    // returns the cell name of this node
    std::string GetName();
      
    // sets the cell name of this node to s
    void SetName(std::string s);
   
    // the cell name of this node
    std::string name;
      
    // this node's list of dependent nodes
    std::list<Node> *dependents;
      
    // this node's list of dependee nodes
    std::list<Node> *dependees;

  private:
    // when a dependee is added to this node, the node passed in needs this node added as a dependent
    void add_dependent2(Node n);
      
    // when a dependent is added to this node, the node passed in needs this node added as a dependee
    void add_dependee2(Node n);
      
    // when a de
    void remove_dependent2(Node n);
  };

#endif
