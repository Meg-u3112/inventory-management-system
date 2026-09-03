import { useEffect, useState } from "react";
import {
  getProducts,
  createProduct,
  updateProduct,
  deleteProduct,
} from "../services/productService";

import { getCategories } from "../services/categoryService";

function Products() {
  // ==============================
  // Products
  // ==============================

  const [products, setProducts] = useState([]);

  // Pagination
  const [page, setPage] = useState(1);
  const [pageSize] = useState(5);
  const [totalPages, setTotalPages] = useState(1);

  // Categories
  const [categories, setCategories] = useState([]);

  // Add Product
  const [showForm, setShowForm] = useState(false);

  const [newProduct, setNewProduct] = useState({
    name: "",
    price: "",
    categoryId: "",
  });

  // Edit Product
  const [editingProduct, setEditingProduct] = useState(null);


  // ==============================
  // Load Products
  // ==============================

  const loadProducts = async () => {
    try {
      const data = await getProducts(page, pageSize);

      setProducts(data.products);
      setTotalPages(data.totalPages);
    } catch (error) {
      console.error("Error fetching products:", error);
    }
  };


  // ==============================
  // Load Categories
  // ==============================

  const loadCategories = async () => {
    try {
      const data = await getCategories();

      setCategories(data);
    } catch (error) {
      console.error("Error fetching categories:", error);
    }
  };
  // Delete
  const handleDelete = async (id) => {
    const confirmed = window.confirm(
      "Are you sure you want to delete this product?"
    );
  
    if (!confirmed) {
      return;
    }
  
    try {
      await deleteProduct(id);
  
      loadProducts();
    } catch (error) {
      console.error("Error deleting product:", error);
    }
  };


  // ==============================
  // useEffect
  // ==============================

  useEffect(() => {
    loadProducts();
  }, [page]);

  useEffect(() => {
    loadCategories();
  }, []);


  // ==============================
  // Create Product
  // ==============================

  const handleCreate = async (e) => {
    e.preventDefault();

    try {
      await createProduct({
        name: newProduct.name,
        price: Number(newProduct.price),
        categoryId: Number(newProduct.categoryId),
      });

      // Close form
      setShowForm(false);

      // Clear form
      setNewProduct({
        name: "",
        price: "",
        categoryId: "",
      });

      // Reload products
      loadProducts();

    } catch (error) {
      console.error("Error creating product:", error);
    }
  };


  // ==============================
  // Start Editing
  // ==============================

  const handleEdit = (product) => {
    setEditingProduct({
      id: product.id,
      name: product.name,
      price: product.price,
      categoryId: product.categoryId,
    });
  };


  // ==============================
  // Update Product
  // ==============================

  const handleUpdate = async (e) => {
    e.preventDefault();

    try {
      await updateProduct(editingProduct.id, {
        id: editingProduct.id,
        name: editingProduct.name,
        price: Number(editingProduct.price),
        categoryId: Number(editingProduct.categoryId),
      });

      // Close edit form
      setEditingProduct(null);

      // Reload products
      loadProducts();

    } catch (error) {
      console.error("Error updating product:", error);
    }
  };


  // ==============================
  // UI
  // ==============================

  return (
    <div>

      <h1>Products</h1>


      {/* =================================
          ADD PRODUCT BUTTON
      ================================= */}

      <button onClick={() => setShowForm(true)}>
        Add Product
      </button>


      {/* =================================
          ADD PRODUCT FORM
      ================================= */}

      {showForm && (
        <form onSubmit={handleCreate}>

          <h3>Add Product</h3>

          <input
            type="text"
            placeholder="Product name"
            value={newProduct.name}
            onChange={(e) =>
              setNewProduct({
                ...newProduct,
                name: e.target.value,
              })
            }
          />


          <input
            type="number"
            placeholder="Price"
            value={newProduct.price}
            onChange={(e) =>
              setNewProduct({
                ...newProduct,
                price: e.target.value,
              })
            }
          />


          <select
            value={newProduct.categoryId}
            onChange={(e) =>
              setNewProduct({
                ...newProduct,
                categoryId: e.target.value,
              })
            }
          >

            <option value="">
              Select Category
            </option>

            {categories.map((category) => (
              <option
                key={category.id}
                value={category.id}
              >
                {category.name}
              </option>
            ))}

          </select>


          <button type="submit">
            Save
          </button>


          <button
            type="button"
            onClick={() => setShowForm(false)}
          >
            Cancel
          </button>

        </form>
      )}


      {/* =================================
          EDIT PRODUCT FORM
      ================================= */}

      {editingProduct && (
        <form onSubmit={handleUpdate}>

          <h3>Edit Product</h3>


          <input
            type="text"
            value={editingProduct.name}
            onChange={(e) =>
              setEditingProduct({
                ...editingProduct,
                name: e.target.value,
              })
            }
          />


          <input
            type="number"
            value={editingProduct.price}
            onChange={(e) =>
              setEditingProduct({
                ...editingProduct,
                price: e.target.value,
              })
            }
          />


          <select
            value={editingProduct.categoryId}
            onChange={(e) =>
              setEditingProduct({
                ...editingProduct,
                categoryId: e.target.value,
              })
            }
          >

            <option value="">
              Select Category
            </option>

            {categories.map((category) => (
              <option
                key={category.id}
                value={category.id}
              >
                {category.name}
              </option>
            ))}

          </select>


          <button type="submit">
            Update
          </button>


          <button
            type="button"
            onClick={() => setEditingProduct(null)}
          >
            Cancel
          </button>

        </form>
      )}


      {/* =================================
          PRODUCTS TABLE
      ================================= */}

      <table>

        <thead>

          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Price</th>
            <th>Category</th>
            <th>Actions</th>
          </tr>

        </thead>


        <tbody>

          {products.map((product) => (

            <tr key={product.id}>

              <td>
                {product.id}
              </td>

              <td>
                {product.name}
              </td>

              <td>
                ₹{product.price}
              </td>

              <td>
                {product.category?.name}
              </td>

              <td>
                <button
                  onClick={() => handleEdit(product)}
                >
                  Edit
                </button>

                <button
                  onClick={() => handleDelete(product.id)}
                >
                  Delete
                </button>
              </td>

            </tr>

          ))}

        </tbody>

      </table>


      {/* =================================
          PAGINATION
      ================================= */}

      <div>

        <button
          onClick={() => setPage(page - 1)}
          disabled={page === 1}
        >
          Previous
        </button>


        <span>
          {" "}
          Page {page} of {totalPages}
          {" "}
        </span>


        <button
          onClick={() => setPage(page + 1)}
          disabled={page === totalPages}
        >
          Next
        </button>

      </div>

    </div>
  );
}

export default Products;