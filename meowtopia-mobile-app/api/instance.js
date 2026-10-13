import axios from 'axios';
import { encode } from 'base-64';

// Base URL for your API
const API_URL = 'http://localhost:8080/api';

const username = 'testuser2';
const password = 'Verysecretpassword1?';

// Create an Axios instance
const axiosInstance = axios.create({
  baseURL: API_URL,
  headers: {
    Authorization: `Basic ${encode(`${username}:${password}`)}`,
  },
});

export default axiosInstance;