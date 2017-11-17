Week 9 - Object Oriented Desktop App - Part 3

Note: After class I modified the interaction between the main form and the Edit Employee form to make things more encapsulated.  I also added a lot of comments to try to make it understandable.  

Now the main form will only pass the employee to be edited and whether it is doing an add or edit operation.  The employee form will then pass that back via a new event with a custom delegate.  The parent that has the list of employees will then add it to the collection if it is doing an add and perform the save.  

I realized later this could be improved more by only having the main form know whether it is add or edit and not even have the employee form be aware. Then the delegate can be simplified.  