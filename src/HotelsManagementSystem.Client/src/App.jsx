import "./App.css";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Navigation from "./components/Navigation/Navigation";
import Home from "./components/Home/Home";
import Footer from "./components/Footer/Footer";
import Register from "./components/Auth/Register/Register";
import Login from "./components/Auth/Login/Login";
import ErrorBoundary from "./components/Error-Boundary/Error-Boundary";

function App() {
  return (
    <ErrorBoundary>
      <div className="app">
        <BrowserRouter>
          <Navigation />
          <main className="main-content">
            <Routes>
              <Route path="/" element={<Home />} />
              <Route path="/register" element={<Register />} />
              <Route path="/login" element={<Login />} />
            </Routes>
          </main>
          <Footer />
        </BrowserRouter>
      </div>
    </ErrorBoundary>
  );
}

export default App;
