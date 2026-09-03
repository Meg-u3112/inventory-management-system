import { useEffect, useState } from "react";

import { getProducts } from "../services/productService";
import { getInventory } from "../services/inventoryService";
import { getPurchases } from "../services/purchaseService";
import { getSales } from "../services/saleService";

function Dashboard() {

  const [products, setProducts] = useState([]);
  const [inventory, setInventory] = useState([]);
  const [purchases, setPurchases] = useState([]);
  const [sales, setSales] = useState([]);

  // ==============================
  // Load Dashboard
  // ==============================

  useEffect(() => {

    loadDashboard();

    const interval = setInterval(() => {
      loadDashboard();
    }, 30000);

    return () => clearInterval(interval);

  }, []);


  const loadDashboard = async () => {

    try {

      const productData = await getProducts(1, 100);
      const inventoryData = await getInventory();
      const purchaseData = await getPurchases();
      const salesData = await getSales();

      setProducts(productData.products);
      setInventory(inventoryData);
      setPurchases(purchaseData);
      setSales(salesData);

    } catch (error) {

      console.error("Dashboard error:", error);

    }
  };


  // ==============================
  // Dashboard Calculations
  // ==============================

  const totalProducts = products.length;

  const lowStockCount = inventory.filter(
    (item) => item.quantity < 10
  ).length;


  // Today's date

  const today = new Date()
    .toISOString()
    .split("T")[0];


  // Today's sales

  const todaySales = sales.filter(
    (sale) =>
      sale.saleDate?.startsWith(today)
  );


  // Today's purchases

  const todayPurchases = purchases.filter(
    (purchase) =>
      purchase.purchaseDate?.startsWith(today)
  );


  // Today's sales amount

  const todaySalesAmount = todaySales.reduce(
    (total, sale) =>
      total + sale.quantity * sale.salePrice,
    0
  );


  // Today's purchase amount

  const todayPurchaseAmount = todayPurchases.reduce(
    (total, purchase) =>
      total +
      purchase.quantity *
      purchase.purchasePrice,
    0
  );


  // Low stock products

  const lowStockProducts = inventory.filter(
    (item) => item.quantity < 10
  );


  // Top 5 products

  const topProducts = [...inventory]
    .sort((a, b) => b.quantity - a.quantity)
    .slice(0, 5);


  // ==============================
  // UI
  // ==============================

  return (

    <div>

      <h1>Dashboard</h1>


      {/* Dashboard Cards */}

      <div>

        <div>
          <h3>Total Products</h3>
          <p>{totalProducts}</p>
        </div>

        <div>
          <h3>Low Stock</h3>
          <p>{lowStockCount}</p>
        </div>

        <div>
          <h3>Today's Sales</h3>
          <p>₹{todaySalesAmount}</p>
        </div>

        <div>
          <h3>Today's Purchases</h3>
          <p>₹{todayPurchaseAmount}</p>
        </div>

      </div>


      {/* Inventory */}

      <h2>Inventory</h2>

      <table>

        <thead>

          <tr>
            <th>Product</th>
            <th>Category</th>
            <th>Stock</th>
            <th>Status</th>
          </tr>

        </thead>


        <tbody>

          {inventory.map((item) => (

            <tr key={item.id}>

              <td>
                {item.product?.name}
              </td>

              <td>
                {item.product?.category?.name}
              </td>

              <td>
                {item.quantity}
              </td>

              <td>

                {item.quantity < 10 ? (

                  <span
                    style={{
                      color: "red",
                      fontWeight: "bold"
                    }}
                  >
                    Low Stock
                  </span>

                ) : (

                  <span
                    style={{
                      color: "green"
                    }}
                  >
                    In Stock
                  </span>

                )}

              </td>

            </tr>

          ))}

        </tbody>

      </table>


      {/* Low Stock Alerts */}

      <h2>⚠️ Low Stock Alerts</h2>

      {lowStockProducts.length === 0 ? (

        <p>
          All products have sufficient stock.
        </p>

      ) : (

        <ul>

          {lowStockProducts.map((item) => (

            <li key={item.id}>

              <strong>
                {item.product?.name}
              </strong>

              {" - "}

              Only {item.quantity} units left

            </li>

          ))}

        </ul>

      )}


      {/* Top 5 Products */}

      <h2>📊 Top 5 Products by Stock</h2>

      <div>

        {topProducts.map((item) => (

          <div
            key={item.id}
            style={{
              marginBottom: "15px"
            }}
          >

            <div>
              {item.product?.name}
            </div>


            <div
              style={{
                width: `${Math.min(
                  item.quantity * 3,
                  100
                )}%`,
                height: "25px",
                background: "steelblue"
              }}
            />

            <span>
              {item.quantity} units
            </span>

          </div>

        ))}

      </div>

    </div>

  );
}

export default Dashboard;