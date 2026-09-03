import axios from "axios";

const API_URL = "http://localhost:5001/api/Products";
export const getProduct = async (id) => {
  const response = await axios.get(`${API_URL}/${id}`);
  return response.data;
};

export const createProduct = async (product) => {
  const response = await axios.post(API_URL, product);
  return response.data;
};

export const updateProduct = async (id, product) => {
  const response = await axios.put(`${API_URL}/${id}`, product);
  return response.data;
};

export const deleteProduct = async (id) => {
  await axios.delete(`${API_URL}/${id}`);
};
export const getProducts = async (page = 1, pageSize = 5) => {
  const response = await axios.get(API_URL, {
    params: {
      page,
      pageSize,
    },
  });
  return response.data;
};