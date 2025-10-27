import "./App.css";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Navigation from "./components/Navigation/Navigation";
import Home from "./components/Home/Home";
import Footer from "./components/Footer/Footer";
import Register from "./components/Auth/Register/Register";
import Login from "./components/Auth/Login/Login";
import ErrorBoundary from "./components/Error-Boundary/Error-Boundary";
import AuthProvider from "./components/AuthProvider/AuthProvider";
import AuthenticatedUser from "./components/Route-Guards/AuthenticatedUser";
import AdminUser from "./components/Route-Guards/AdminUser";
import ReceptionistUser from "./components/Route-Guards/ReceptionistUser";
import NonAuthenticatedUser from "./components/Route-Guards/NonAuthenticatedUser";
import AdminDashboard from "./components/Dashboards/AdminDashBoard/AdminDashboard";
import ReceptionistDashBoard from "./components/Dashboards/ReceptionistDashBoard/ReceptionistDashBoard";
import NotFound404 from "./components/StatusCodePages/404";
import Contact from "./components/Contact/Contact";

function App() {
  return (
    <ErrorBoundary>
      <AuthProvider>
        <div className="app">
          <BrowserRouter>
            <Navigation />
            <main className="main-content">
              <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/contact" element={<Contact />} />
                <Route path="/404" element={<NotFound404 />} />
                <Route path="*" element={<NotFound404 />} />
                <Route element={<NonAuthenticatedUser />}>
                  <Route path="/register" element={<Register />} />
                  <Route path="/login" element={<Login />} />
                </Route>
                <Route element={<AuthenticatedUser />}>
                  {/* You have to put here all the routes that require authentication */}
                  <Route element={<AdminUser />}>
                    {/* You have to put here all the routes that require admin role */}
                    <Route
                      path="/admin-dashboard"
                      element={<AdminDashboard />}
                    />
                  </Route>
                  <Route element={<ReceptionistUser />}>
                    {/* You have to put here all the routes that require receptionist role */}
                    <Route
                      path="/receptionist-dashboard"
                      element={<ReceptionistDashBoard />}
                    />
                  </Route>
                </Route>
              </Routes>
            </main>
            <Footer />
          </BrowserRouter>
        </div>
      </AuthProvider>
    </ErrorBoundary>
  );
}

export default App;
