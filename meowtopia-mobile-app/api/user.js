import axiosInstance from './instance';

export const fetchUser = async () => {
    try {
        const response = await axiosInstance('/8a3c8c48-f14e-42db-b8e6-803d8962dd95');
        return response.data;
    } catch (error) {
        console.error(error);
        throw error;
    }
};