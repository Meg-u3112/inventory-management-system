import { useEffect, useState } from "react";
import { getPurchases,createPurchase } from "../services/purchaseService";
import { getProducts } from "../services/productService";

function Purchases() {
  const [purchases, setPurchases] = useState([]);
  const [products, setProducts] = useState([]);
  const [newPurchase, setNewPurchase] = useState({
    productId: "",
    quantity: "",
    purchasePrice: "",
    supplier: "",
    purchaseDate: ""
  });
  const [updatedStock, setUpdatedStock] = useState(null);

  const loadPurchases = async () => {
    try {
      const data = await getPurchases();

      setPurchases(data);
    } catch (error) {
      console.error("Error fetching purchases:", error);
    }
  };
  const loadProducts = async () => {
    try {
      const data = await getProducts(1,100);

      setProducts(data.products);
    } catch (error) {
      console.error("Error fetching purchases:", error);
    }
  };
  const handleCreatePurchase = async (e) => {
    e.preventDefault();
  
    // Validation
    if (!newPurchase.productId) {
      alert("Please select a product.");
      return;
    }
  
    if (Number(newPurchase.quantity) <= 0) {
      alert("Quantity must be greater than 0.");
      return;
    }
  
    if (Number(newPurchase.purchasePrice) <= 0) {
      alert("Purchase price must be greater than 0.");
      return;
    }
  
    if (!newPurchase.supplier.trim()) {
      alert("Please enter supplier.");
      return;
    }
  
    if (!newPurchase.purchaseDate) {
      alert("Please select purchase date.");
      return;
    }
  
    try {
      await createPurchase({
        productId: Number(newPurchase.productId),
        quantity: Number(newPurchase.quantity),
        purchasePrice: Number(newPurchase.purchasePrice),
        supplier: newPurchase.supplier,
        purchaseDate: new Date(
          newPurchase.purchaseDate
        ).toISOString()
      });
  
      const selectedProduct = products.find(
        (product) => product.id === Number(newPurchase.productId)
      );
  
      alert(
        `Purchase created successfully!\n\n${selectedProduct?.name} stock updated.`
      );
  
      // Reset form
      setNewPurchase({
        productId: "",
        quantity: "",
        purchasePrice: "",
        supplier: "",
        purchaseDate: ""
      });
  
      // Refresh purchase list
      loadPurchases();
  
      // Refresh products
      loadProducts();
  
    } catch (error) {
      console.error("Error creating purchase:", error);
  
      if (error.response) {
        alert(error.response.data);
      } else {
        alert("Failed to create purchase.");
      }
    }
  };

  useEffect(() => {
    loadPurchases();
    loadProducts();
  }, []);

  return (
    <div>
      <h1>Purchases</h1>

      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Date</th>
            <th>Product</th>
            <th>Quantity</th>
            <th>Supplier</th>
            <th>Purchase Price</th>
          </tr>
        </thead>

        <tbody>
          {purchases.map((purchase) => (
            <tr key={purchase.id}>
              <td>{purchase.id}</td>

              <td>
                {new Date(purchase.purchaseDate).toLocaleDateString()}
              </td>

              <td>
                {purchase.product?.name}
              </td>

              <td>
                {purchase.quantity}
              </td>
              <td>{purchase.supplier}
              </td>
              <td>
                ₹{purchase.purchasePrice}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      <h2>New Purchase</h2>

<form onSubmit={handleCreatePurchase}>
  <select
    value={newPurchase.productId}
    onChange={(e) =>
      setNewPurchase({
        ...newPurchase,
        productId: e.target.value
      })
    }
  >
    <option value="">Select Product</option>

    {products.map((product) => (
      <option key={product.id} value={product.id}>
        {product.name}
      </option>
    ))}
  </select>

  <input
    type="number"
    placeholder="Quantity"
    value={newPurchase.quantity}
    onChange={(e) =>
      setNewPurchase({
        ...newPurchase,
        quantity: e.target.value
      })
    }
  />

  <input
    type="number"
    placeholder="Purchase Price"
    value={newPurchase.purchasePrice}
    onChange={(e) =>
      setNewPurchase({
        ...newPurchase,
        purchasePrice: e.target.value
      })
    }
  />
  <input
  type="text"
  placeholder="Supplier"
  value={newPurchase.supplier}
  onChange={(e) =>
    setNewPurchase({
      ...newPurchase,
      supplier: e.target.value
    })
  }
/>

  <input
    type="datetime-local"
    value={newPurchase.purchaseDate}
    onChange={(e) =>
      setNewPurchase({
        ...newPurchase,
        purchaseDate: e.target.value
      })
    }
  />

  <button type="submit">
    Save Purchase
  </button>
</form>
    </div>
  );
}

export default Purchases;