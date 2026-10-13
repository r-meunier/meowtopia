import axiosInstance from './instance';

export const login = async (username, password) => {
  try {
    const response = await axiosInstance.post('/auth/login', { username, password });
    return response.data;
  } catch (error) {
    console.error(error);
    throw error;
  }
};