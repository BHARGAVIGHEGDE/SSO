import  { useState, useEffect } from "react";
import axios from "axios";
const App = () => {
  const [username, setUsername] = useState("");
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const token = localStorage.getItem("token"); 

  useEffect(() => {
    if (token) {
      axios
        .post("http://localhost:5009/api/User/validate", {}, {
          headers: {
            "Authorization": `Bearer ${token}`, 
          }
        })
        .then((response) => {
          if (response.status === 200) {
            axios
              .get("http://localhost:5009/api/User/me", {
                headers: {
                  "Authorization": `Bearer ${token}`, 
                }
              })
              .then((userResponse) => {
                setUsername(userResponse.data.username);
                setIsAuthenticated(true);
              })
              .catch((error) => {
                console.error("Error fetching user details:", error);
                setIsAuthenticated(false);
              });
          }
        })
        .catch((error) => {
          console.error("Token validation failed:", error);
          setIsAuthenticated(false);
        });
    }
  }, [token]);

  return (
    <div>
      {isAuthenticated ? (
        <h1>Welcome, {username}!</h1>
      ) : (
        <h1>Please log in.</h1>
      )}
    </div>
  );
};

export default App;
