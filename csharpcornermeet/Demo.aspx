<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Demo.aspx.cs" Inherits="csharpcornermeet.Demo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>My Web Site</title>
    <script src="Scripts\knockout-3.0.0.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <p>First Name: <span data-bind="text: firstname"/></p>
           
           <p>Last Name: <span data-bind="text: lastname"/></p>
        </div>
        <div>
            <p>First Name :<asp:TextBox ID="txtUser" runat="server" data-bind="value: firstname ,valueUpdate: 'keyup'" /></p>
            <p>Last Name :<asp:TextBox ID="TextBox1" runat="server" data-bind="value: lastname" /></p>
        </div>
        <hr />
        <p>Full Name :<span data-bind="text: fullname" /></p>
    </form>
    <script type="text/javascript">
        var vm = {
            firstname: ko.observable("Teja"),
            lastname: ko.observable("gandhapudi")
        };
        var fullname = ko.dependentObservable(function () {
            return vm.firstname() + " " + vm.lastname()
        });
        vm.firstname.subscribe(function (val) {
            console.log("first Name updated as : "+val)
        });
        ko.applyBindings(vm);
    </script>
</body>
</html>
