import axios from 'axios';
import { encode } from 'base-64';

// Settings come from meowtopia-mobile-app/.env (git-ignored); copy .env.example to create it.
// Expo inlines EXPO_PUBLIC_* values into the app bundle, so this suits local development only.
const API_URL = process.env.EXPO_PUBLIC_API_URL ?? 'http://localhost:8080/api';

const username = process.env.EXPO_PUBLIC_API_USERNAME;
const password = process.env.EXPO_PUBLIC_API_PASSWORD;

// Create an Axios instance
const axiosInstance = axios.create({
  baseURL: API_URL,
  headers: {
    Authorization: `Basic ${encode(`${username}:${password}`)}`,
  },
});

export default axiosInstance;