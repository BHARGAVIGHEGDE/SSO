import axios from 'axios';
const API_URL = "http://localhost:5009/api";

export const login = async (username: string, password: string) => {
  try {
    const response = await axios.post(`${API_URL}/User/login`, {
      username,
      password,
    });
    return response.data; 
  } catch (error) {
    throw error;
  }
};
