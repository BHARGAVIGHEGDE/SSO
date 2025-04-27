import { useState } from "react";
import { login } from "./api/auth";

function AppA() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [errorMessage, setErrorMessage] = useState("");

  const handleLogin = async (e: { preventDefault: () => void }) => {
    e.preventDefault();
    setErrorMessage("");
    try {
      const data = await login(username, password);
      localStorage.setItem("token", data.token);
      const token = localStorage.getItem("token");
      if (token) {
        console.log("Token stored successfully:", token);
      }
      window.location.href = `http://localhost:5174/?userId=${data.userId}`;

    } catch (error) {
      console.error(error);
      setErrorMessage("Invalid username or password.");
    }
  };

  return (
    <div className="login-container">
      <h2>Login to App-A</h2>
      <form onSubmit={handleLogin}>
        <input
          type="text"
          placeholder="Username"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          required
        />
        <input
          type="password"
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
        <button type="submit">Login</button>
        {errorMessage && <p>{errorMessage}</p>}
      </form>
    </div>
  );
}

export default AppA;
