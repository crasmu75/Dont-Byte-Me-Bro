#include<iostream>
#include<string>
#include<list>

#ifndef NODE_H
#define NODE_H

  class Node
  {
  public:
    Node(std::string s);
    ~Node();
    void add_dependent(Node n);
    void add_dependee(Node n);
    void remove_dependent(Node n);
    void remove_dependee(Node n);
    void remove_all_dependents();
    void remove_all_dependees();
    bool operator== (const Node & rhs);
    std::string GetName();
    void SetName(std::string s);
   
    std::string name;
    std::list<Node> *dependents;
    std::list<Node> *dependees;

  private:
    void add_dependent2(Node n);
    void add_dependee2(Node n);
    void remove_dependent2(Node n);
  };

#endif
