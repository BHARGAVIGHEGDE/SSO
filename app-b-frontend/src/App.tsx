import React, { useEffect, useState } from "react";
import axios from "axios";

const AppB = () => {
  const [username, setUsername] = useState("");
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  useEffect(() => {
    const queryParams = new URLSearchParams(window.location.search);
    const userId = queryParams.get("userId");
    if (userId) {
      axios
        .get(`http://localhost:5009/api/User/token/${userId}`)
        .then((response) => {
          const token = response.data;
          axios
            .post(
              "http://localhost:5009/api/User/validate",
              {},
              {
                headers: {
                  Authorization: `Bearer ${token}`,
                },
              }
            )
            .then((validateResponse) => {
              if (validateResponse.status === 200) {
                axios
                  .get("http://localhost:5009/api/User/me", {
                    headers: {
                      Authorization: `Bearer ${token}`,
                    },
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
        })
        .catch((error) => {
          console.error("Error retrieving token from DB:", error);
          setIsAuthenticated(false);
        });
    }
  }, []);

  return (
    <div>
      {isAuthenticated ? (
        <h1>Welcome, {username}!</h1>
      ) : (
        <h1>Invalid Token or Token Expired. Please log in again.</h1>
      )}
    </div>
  );
};
export default AppB;
