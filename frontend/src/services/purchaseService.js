import axios from "axios";

const API_URL = "http://localhost:5001/api/Purchases";

export const getPurchases = async () => {
  const response = await axios.get(API_URL);
  return response.data;
};

export const createPurchase = async (purchase) => {
  const response = await axios.post(API_URL, purchase);
  return response.data;
};

export const getPurchase = async (id) => {
  const response = await axios.get(`${API_URL}/${id}`);
  return response.data;
};