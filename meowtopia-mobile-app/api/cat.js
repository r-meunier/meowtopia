import axiosInstance from './instance';

export const fetchCats = async () => {
  try {
    const response = await axiosInstance.get('/cats');
    return response.data;
  } catch (error) {
    console.error(error);
    throw error;
  }
};

export const fetchCatsByUser = async (userId) => {
  try {
    const response = await axiosInstance.get(`/users/${userId}/cats`);
    return response.data;
  } catch (error) {
    console.error(error);
    throw error;
  }
};

export const fetchCatsByBreed = async (breedId) => {
  try {
    const response = await axiosInstance.get(`/breed/${breedId}/cats`);
    return response.data;
  } catch (error) {
    console.error(error);
    throw error;
  }
};
