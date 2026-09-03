import { useEffect, useState } from "react";

import {
  getSales,
  createSale,
} from "../services/saleService";

import { getProducts } from "../services/productService";

function Sales() {
  const [sales, setSales] = useState([]);
  const [products, setProducts] = useState([]);

  const [newSale, setNewSale] = useState({
    productId: "",
    quantity: "",
    salePrice: "",
    saleDate: "",
  });
  const handleCreateSale = async (e) => {
    e.preventDefault();
  
    // Validation
    if (!newSale.productId) {
      alert("Please select a product.");
      return;
    }
  
    if (Number(newSale.quantity) <= 0) {
      alert("Quantity must be greater than 0.");
      return;
    }
  
    if (Number(newSale.salePrice) <= 0) {
      alert("Sale price must be greater than 0.");
      return;
    }
  
    if (!newSale.saleDate) {
      alert("Please select sale date.");
      return;
    }
  
    try {
      await createSale({
        productId: Number(newSale.productId),
        quantity: Number(newSale.quantity),
        salePrice: Number(newSale.salePrice),
  
        // Convert datetime-local to UTC
        saleDate: new Date(
          newSale.saleDate
        ).toISOString(),
      });
  
      alert("Sale created successfully!");
  
      // Reset form
      setNewSale({
        productId: "",
        quantity: "",
        salePrice: "",
        saleDate: "",
      });
  
      // Refresh sales
      loadSales();
  
    } catch (error) {
      console.error("Error creating sale:", error);
  
      if (error.response) {
        alert(
          typeof error.response.data === "string"
            ? error.response.data
            : "Failed to create sale."
        );
      } else {
        alert("Failed to create sale.");
      }
    }
  };

  // ==============================
  // Load Sales
  // ==============================

  const loadSales = async () => {
    try {
      const data = await getSales();

      setSales(data);
    } catch (error) {
      console.error("Error fetching sales:", error);
    }
  };


  // ==============================
  // Load Products
  // ==============================

  const loadProducts = async () => {
    try {
      const data = await getProducts(1, 100);

      setProducts(data.products);
    } catch (error) {
      console.error("Error fetching products:", error);
    }
  };


  // ==============================
  // Initial Load
  // ==============================

  useEffect(() => {
    loadSales();
    loadProducts();
  }, []);


  return (
    <div>

      <h1>Sales</h1>


      {/* =========================
          NEW SALE FORM
      ========================= */}

      <h2>New Sale</h2>

      <form onSubmit={handleCreateSale}>

        {/* Product */}

        <select
          value={newSale.productId}
          onChange={(e) =>
            setNewSale({
              ...newSale,
              productId: e.target.value,
            })
          }
        >

          <option value="">
            Select Product
          </option>

          {products.map((product) => (
            <option
              key={product.id}
              value={product.id}
            >
              {product.name}
            </option>
          ))}

        </select>


        {/* Quantity */}

        <input
          type="number"
          placeholder="Quantity"
          value={newSale.quantity}
          onChange={(e) =>
            setNewSale({
              ...newSale,
              quantity: e.target.value,
            })
          }
        />


        {/* Sale Price */}

        <input
          type="number"
          placeholder="Sale Price"
          value={newSale.salePrice}
          onChange={(e) =>
            setNewSale({
              ...newSale,
              salePrice: e.target.value,
            })
          }
        />


        {/* Sale Date */}

        <input
          type="datetime-local"
          value={newSale.saleDate}
          onChange={(e) =>
            setNewSale({
              ...newSale,
              saleDate: e.target.value,
            })
          }
        />


        <button type="submit">
          Save Sale
        </button>

      </form>


      {/* =========================
          SALES TABLE
      ========================= */}

      <h2>Sales History</h2>

      <table>

        <thead>

          <tr>
            <th>ID</th>
            <th>Date</th>
            <th>Product</th>
            <th>Quantity</th>
            <th>Sale Price</th>
          </tr>

        </thead>


        <tbody>

          {sales.map((sale) => (

            <tr key={sale.id}>

              <td>
                {sale.id}
              </td>

              <td>
                {new Date(
                  sale.saleDate
                ).toLocaleDateString()}
              </td>

              <td>
                {sale.product?.name}
              </td>

              <td>
                {sale.quantity}
              </td>

              <td>
                ₹{sale.salePrice}
              </td>

            </tr>

          ))}

        </tbody>

      </table>

    </div>
  );
}

export default Sales;