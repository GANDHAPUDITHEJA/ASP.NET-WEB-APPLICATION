<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Demo2.aspx.cs" Inherits="csharpcornermeet.Demo2" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Using Knockout.js</title>
    <script src="Scripts/knockout-3.0.0.js"></script>
    <script src="Scripts/jquery-2.1.0.min.js"></script>
</head>
<body>
      <form id="form1" runat="server">
        <div>
            <h3>Add a Student</h3>
            <p>Student Name: <input type="text" data-bind="value: studentName" /></p>
            <p>Roll Number: <input type="text" data-bind="value: rollNumber" /></p>
            <button type="button" data-bind="click: addStudent">Add Student</button>
        </div>
        <div>
            <h3>Students List</h3>
            <ul data-bind="foreach: students">
                <li>
                    <label>
                        <p>Name:<span data-bind="text: name"></span></p>
                        <p>Roll Number: <span data-bind="text: roll"></span></p>
                    </label>
                    <button type="button" data-bind="click: $parent.removeStudent">Remove</button>
                </li>
            </ul>
        </div>
    </form>
    <script type="text/javascript">
        // ViewModel
        var vm = {
            studentName: ko.observable(""),
            rollNumber: ko.observable(""),
            students: ko.observableArray([]),
            addStudent: function () {
                // Add a student object with name and roll number to the list
                if (this.studentName().trim() && this.rollNumber().trim()) {
                    this.students.push({
                        name: this.studentName(),
                        roll: this.rollNumber()
                    });
                    // Clear input fields
                    this.studentName("");
                    this.rollNumber("");
                } else {
                    alert("Please enter both Name and Roll Number.");
                }
            },
            removeStudent: function (student) {
                // Remove the selected student from the list
                vm.students.remove(student);
            }
        };

        // Apply bindings
        ko.applyBindings(vm);
    </script>
</body>
</html>
