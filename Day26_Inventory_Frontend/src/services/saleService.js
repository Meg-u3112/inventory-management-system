import axios from "axios";

const API_URL = "http://localhost:5001/api/Sales";

export const getSales = async () => {
  const response = await axios.get(API_URL);
  return response.data;
};

export const createSale = async (sale) => {
  const response = await axios.post(API_URL, sale);
  return response.data;
};

export const getSale = async (id) => {
  const response = await axios.get(`${API_URL}/${id}`);
  return response.data;
};