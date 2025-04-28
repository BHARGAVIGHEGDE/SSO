This project is a part of a Single Sign-On (SSO) system.
It allows users to log in by entering their username and password, authenticates against the backend API, and securely stores the JWT token in localStorage.
Upon successful login in the App-A, users are redirected to App-B.

--Built using React and TypeScript.

--API calls are handled using Axios.

--Environment-specific API URLs are managed through a .env file.

--Authentication tokens are securely managed using Bearer Token strategy.


--Tech Stack
++ React (Vite)
++ TypeScript
++ Axios
++ JWT Authentication

--Environment Setup
Create a .env file in the root directory with the following content:
VITE_REACT_APP_API_URL=http://localhost:5009
> (Replace 5009 with your backend server port if different.)
> Follow this step for both app-a-frontend and app-b-frontend
