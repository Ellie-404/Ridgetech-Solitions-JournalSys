let currentRole = "patient"; // Default role

function changeRole(role) {
    // Implementation for changing role
    currentRole = role; 
    document.getElementById("rolleCss").href =role + ".css";
    console.log("Role changed to: " + role);
    // You can add your logic here to handle the role change, such as updating the UI or making an API call.
}