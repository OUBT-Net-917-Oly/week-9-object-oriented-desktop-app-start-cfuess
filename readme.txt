Week 8 - Object Oriented Desktop App - Part 2

Note: After class I added the Edit Employee feature.  I ended up not having the edit form appear just by a click of the listbox control.  Instead for both editing and deleting it would be useful to just set a class scope variable that tracks what employee was selected.  Then when edit or delete is clicked that value could be used.  Look at the lstEmployees_SelectedIndexChanged function to see how that value is set.  It was necessary to wrap it in a try block because in the loadEmployeeList function it sets the datasource to null and that will cause the SelectedIndexChanged function to fire.  When that happens trying to get the selected item throws an error.  

Assignment 

- Add a Delete Employee, Update Project, and Delete Project feature to the Program:

- Implement a Delete Employee method and call it on the button click, ask the user if they are sure first.

- Create buttons for Edit Project and Delete Project on the EmployeeListForm

- Create a new constructor on the ProjectForm class to be called for edit

- Load the form with the existing project data

- When you click save on the ProjectForm, update the project, update the projectlist control, and save to the file (you'll need to use the event and the subscription in the EmployeeListForm to do it).

- Implement a delete project feature, ask the user if they are sure first.
