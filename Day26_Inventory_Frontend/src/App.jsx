import {BrowserRouter ,Routes,Route,Link} from "react-router-dom"
import Products from "./pages/Products";
import Purchases from "./pages/Purchases";
import Dashboard from "./pages/Dashboard";
function App() {
  return (
    <BrowserRouter>
    <nav>
      <Link to="/dashboard">Dashboard</Link>
      {" | "}
      <Link to="/products">Products</Link>
      {" | "}
      <Link to="/purchases">Purchases</Link>
    </nav>
    <Routes>
      <Route path="/dashboard" element={<Dashboard />}/>
      <Route path="/products" element={<Products />}/>
      <Route path="/purchases" element={<Purchases />}/>
      <Route path ="/" element={<Dashboard/>}/>
    </Routes>
    </BrowserRouter>
);
}

export default App;